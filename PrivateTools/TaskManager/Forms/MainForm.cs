// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainForm.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the MainForm type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Toolkit.Uwp.Notifications;
using MyTools.Common.Extensions;
using MyTools.Common.Utils;
using TaskManager.Configration;
using TaskManager.Data;
using TaskManager.Interfaces;
using Timer = System.Threading.Timer;

namespace TaskManager.Forms
{
    /// <summary>
    /// メインフォーム
    /// </summary>
    public partial class MainForm : Form, ICanShowFromTaskTray
    {
        /// <summary>
        /// 現在時刻表示タイマ
        /// </summary>
        private Timer _nowTimer;

        /// <summary>
        /// グループ一覧の更新を抑止するフラグ
        /// </summary>
        private bool isSuspentGroupListView = false;

        /// <summary>
        /// タスク一覧の表示を更新中かどうか
        /// </summary>
        private bool isUpdatingTaskIchiran = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();

            this.Load += this.OnLoad;
            this.Closing += this.OnClosing;
        }

        /// <summary>
        /// タスクトレイから表示中かどうか
        /// </summary>
        public bool IsShowFromTaskTray { get; set; }

        /// <summary>
        /// 画面種別
        /// </summary>
        public ViewKind ViewType
        {
            get
            {
                return ViewKind.MainView;
            }
        }

        /// <summary>
        /// 画面クローズ時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (this.IsShowFromTaskTray)
            {
                // タスクトレイからの表示中の場合は、アプリ終了ではないため
                // このタイミングでの保存は不要
                return;
            }

            Utils.SaveConfigs();
        }

        /// <summary>
        /// 画面ロード時のイベントです
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void OnLoad(object sender, EventArgs e)
        {
            UiContext.Initialize();

            ResourceManager.Instance.MainForm = this;

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            this._nowTimer = new Timer(this.NowTimerCallBack);
            this._nowTimer.Change(new TimeSpan(), TimeSpan.FromMinutes(1));

            this.DgvAllTasks.Initialize(true);
            this.DgvRecentTasks.Initialize(false);

            this.DgvAllTasks.UpdateEvent += this.DgvAllTasksOnUpdateEvent;
            this.DgvRecentTasks.UpdateEvent += this.DgvRecentTasksOnUpdateEvent;

            this.ShowAllTaskListInDgvAllTasks();
            this.DgvAllTasksOnUpdateEvent(null, null);

            this.InitializeGroupList();

            this.TxtFilter.TextChanged += this.TxtFilter_OnTextChanged;
        }

        /// <summary>
        /// フィルタテキストボックスの入力値変更イベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void TxtFilter_OnTextChanged(object sender, EventArgs e)
        {
            var curText = this.TxtFilter.Text;

            var target = new List<TaskItem>();
            List<TaskItem> currentList;
            if (this.DgvAllTasks.ShowingGroup != null)
            {
                currentList = this.DgvAllTasks.ShowingGroup.ChildTaskItems.ToList();
            }
            else
            {
                currentList = ResourceManager.Instance.GetAllTaskItems();
            }

            foreach (var taskItem in currentList)
            {
                var str = taskItem.GetItemStrs();
                if (str.Any(x => x.Contains(curText)))
                {
                    target.Add(taskItem);
                }
            }

            this.DgvAllTasks.RefleshTaskItems(target, this.DgvAllTasks.ShowingGroup);
        }

        /// <summary>
        /// 直近のタスク一覧更新時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void DgvRecentTasksOnUpdateEvent(object sender, TaskIchiranEventArgs e)
        {
            if (this.isUpdatingTaskIchiran)
            {
                return;
            }

            this.isUpdatingTaskIchiran = true;

            if (this.DgvAllTasks.ShowingGroup != null)
            {
                this.DgvAllTasks.RefleshTaskItems(this.DgvAllTasks.ShowingGroup.ChildTaskItems.ToList(), this.DgvAllTasks.ShowingGroup);
            }
            else
            {
                this.ShowAllTaskListInDgvAllTasks();
            }

            this.TxtFilter_OnTextChanged(null, null);

            this.RefleshTaskGroupIchiran();

            this.isUpdatingTaskIchiran = false;
        }

        /// <summary>
        /// 全タスク一覧の更新時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void DgvAllTasksOnUpdateEvent(object sender, TaskIchiranEventArgs e)
        {
            if (this.isUpdatingTaskIchiran)
            {
                return;
            }

            this.isUpdatingTaskIchiran = true;
            
            var taskList = ResourceManager.Instance.GetAllTaskItems();
            
            var filtered = Utils.FilterRecentLimitTask(taskList);
            this.DgvRecentTasks.RefleshTaskItems(filtered, null);

            this.TxtFilter_OnTextChanged(null, null);

            this.RefleshTaskGroupIchiran();

            this.isUpdatingTaskIchiran = false;
        }
        
        /// <summary>
        /// タスク一覧に全てのタスクを表示します。
        /// </summary>
        private void ShowAllTaskListInDgvAllTasks()
        {
            this.LblDisplayGroup.Text = string.Format("[{0}]", "全てのタスク");
            var taskList = ResourceManager.Instance.GetAllTaskItems();
            this.DgvAllTasks.RefleshTaskItems(taskList, null);
        }

        /// <summary>
        /// タスクグループの一覧を初期化します。
        /// </summary>
        private void InitializeGroupList()
        {
            this.LsvGroup.HideSelection = false;
            this.LsvGroup.FullRowSelect = true;
            this.LsvGroup.MultiSelect = false;
            this.LsvGroup.GridLines = true;
            this.LsvGroup.View = View.Details;
            this.LsvGroup.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.LsvGroup.ColumnWidthChanging += this.LsvGroup_OnColumnWidthChanging;
            this.LsvGroup.SelectedIndexChanged += this.LsvGroupOnSelectedIndexChanged;
            this.LsvGroup.ShowItemToolTips = true;

            this.RefleshTaskGroupIchiran();
        }

        /// <summary>
        /// タスクグループ一覧の選択変更時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void LsvGroupOnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.LsvGroup.SelectedItems.Count > 0)
            {
                var selectedItemIndex = this.LsvGroup.SelectedItems[0].Index;
                var selected = this.LsvGroup.SelectedItems[0].Tag as KeyInfo;
                if (selected != null)
                {
                    if (selected.Equals(TaskGroupInfo.GetRootGroup().Key))
                    {
                        this.ShowAllTaskListInDgvAllTasks();
                    }
                    else
                    {
                        var group = ResourceManager.Instance.GetGroupInfo(selected);

                        this.DgvAllTasks.RefleshTaskItems(group.ChildTaskItems.ToList(), group);
                        
                        this.LblDisplayGroup.Text = string.Format("[{0}]", group.Name);
                    }

                    this.isSuspentGroupListView = true;
                    this.LsvGroup.Items[selectedItemIndex].Focused = true;
                    this.LsvGroup.Items[selectedItemIndex].Selected = true;
                    this.isSuspentGroupListView = false;
                }
            }
        }

        /// <summary>
        /// タスクグループ一覧を再描画します。
        /// </summary>
        private void RefleshTaskGroupIchiran()
        {
            if (this.isSuspentGroupListView)
            {
                return;
            }

            this.LsvGroup.BeginUpdate();

            this.LsvGroup.Items.Clear();

            {
                // 全タスク表示用の項目
                var lvItem = this.LsvGroup.Items.Add("全てのタスク");
                var allTaskList = ResourceManager.Instance.GetAllTaskItems();
                lvItem.SubItems.Add(string.Format("{0:D}件", allTaskList.Count));
                lvItem.Tag = TaskGroupInfo.GetRootGroup().Key;
            }

            // グループ表示
            var topTasks = ResourceManager.Instance.GetRootGroups();
            foreach (var taskGroupInfo in topTasks)
            {
                var taskName = taskGroupInfo.Name;
                var taskNum = taskGroupInfo.ChildTaskItems.Count;
                var lvItem = this.LsvGroup.Items.Add(taskName);
                lvItem.SubItems.Add(string.Format("{0:D}件", taskNum));
                lvItem.Tag = taskGroupInfo.Key;

                foreach (var childGroup in taskGroupInfo.ChildGroups)
                {
                    var group = ResourceManager.Instance.GetGroupInfo(childGroup);
                    var childTaskName = group.Name;
                    var childTaskNum = group.ChildTaskItems.Count;
                    var childLvItem = this.LsvGroup.Items.Add("  |-- " + childTaskName);
                    childLvItem.SubItems.Add(string.Format("{0:D}件", childTaskNum));
                    childLvItem.Tag = childGroup;
                }
            }

            this.LsvGroup.EndUpdate();
        }

        /// <summary>
        /// タスグループ一覧の列幅変更抑止イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void LsvGroup_OnColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            var maxColumnIdx = this.LsvGroup.Columns.Count - 1;

            if (e.ColumnIndex > maxColumnIdx)
            {
                return;
            }

            // シンボル一覧の行サイズ変更をキャンセルする。
            e.NewWidth = this.LsvGroup.Columns[e.ColumnIndex].Width;

            e.Cancel = true;
        }

        /// <summary>
        /// 現在時刻更新タイマイベント
        /// </summary>
        /// <param name="state">イベント引数</param>
        private void NowTimerCallBack(object state)
        {
            UiContext.Post(() =>
            {
                var now = DateTime.Now;
                this.LabelDateTime.Text = "現在日時：" + now.ToString("yyyy/MM/dd HH:mm");

                this.DgvAllTasksOnUpdateEvent(null, null);

                // TODO:windows通知の仮実装。
                var tasks = ResourceManager.Instance.GetAllTaskItems().Any(x => Utils.IsOverRedZone(x.DateTimeLimit));
                if (tasks)
                {
                    var toast = new ToastContentBuilder()
                        .AddText("タスクの期限切れ通知")
                        .AddText("期限切れのタスクがあります。確認してください");
                    toast.Show();
                }
            });
        }

        /// <summary>
        /// グループ追加ボタン押下時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private async void BtnAddGroup_Click(object sender, EventArgs e)
        {
            var parent = TaskGroupInfo.GetRootGroup();
            if (this.LsvGroup.SelectedItems.Count > 0)
            {
                var selected = this.LsvGroup.SelectedItems[0].Tag as KeyInfo;
                if (selected != null)
                {
                    var item = ResourceManager.Instance.GetGroupInfo(selected);
                    parent = item;
                }
            }

            var win = new TaskGroupEditForm();
            win.Initialize(null, true, parent);
            var ret = await win.ShowWindow(this);

            this.RefleshTaskGroupIchiran();
        }

        /// <summary>
        /// グループ削除ボタン押下時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void BtnRemoveGroup_Click(object sender, EventArgs e)
        {
            if (this.LsvGroup.SelectedItems.Count > 0)
            {
                var selected = this.LsvGroup.SelectedItems[0].Tag as KeyInfo;
                if (selected != null)
                {
                    var item = ResourceManager.Instance.GetGroupInfo(selected);
                    if (!selected.Equals(TaskGroupInfo.GetRootGroup().Key) && !selected.Equals(TaskGroupInfo.GetDefaultGroup().Key))
                    {
                        ResourceManager.Instance.RemoveTaskGroup(item);
                        this.RefleshTaskGroupIchiran();
                    }
                }
            }
        }

        /// <summary>
        /// グループリネームボタン押下時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private async void BtnReNameGroup_Click(object sender, EventArgs e)
        {
            if (this.LsvGroup.SelectedItems.Count > 0)
            {
                var selected = this.LsvGroup.SelectedItems[0].Tag as KeyInfo;
                if (selected != null)
                {
                    var item = ResourceManager.Instance.GetGroupInfo(selected);
                    if (!selected.Equals(TaskGroupInfo.GetRootGroup().Key) && !selected.Equals(TaskGroupInfo.GetDefaultGroup().Key))
                    {
                        var win = new TaskGroupEditForm();
                        win.Initialize(item, false, ResourceManager.Instance.GetGroupInfo(item.ParentGroup));
                        var ret = await win.ShowWindow(this);

                        this.RefleshTaskGroupIchiran();
                    }
                }
            }
        }
        
        /// <summary>
        /// フォルダを開くボタン押下時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void BtnOpenExecFolder_Click(object sender, EventArgs e)
        {
            TaskTrayMenuEvents.ShowExecFolder(sender, e);
        }

        /// <summary>
        /// CSV出力ボタン押下時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト7</param>
        /// <param name="e">イベント引数</param>
        private void BtnOutputCsv_Click(object sender, EventArgs e)
        {
            TaskTrayMenuEvents.OutputCsvTaskList(sender, e);
        }

        /// <summary>
        /// 編集ボタン押下時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void BtnEditConfig_Click(object sender, EventArgs e)
        {
            var win = new ConfigEditForm();
            win.Initialize(Config.Instance.EditableItems);

            var ret = win.ShowWindow(this);
        }

        /// <summary>
        /// バックアップボタン押下時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void BtnBackup_Click(object sender, EventArgs e)
        {
            var msg = MessageBox.Show("現在の設定および管理情報のバックアップを行います。", "確認", MessageBoxButtons.YesNo);
            if (msg == DialogResult.Yes)
            {
                if (!Directory.Exists(Config.Instance.BackupRootDir))
                {
                    Directory.CreateDirectory(Config.Instance.BackupRootDir);
                }

                var configFile = Config.ConfigFilePath;
                var taskFile = ResourceManager.TaskListSavePath;

                var dateDir = PathUtils.CreateDateTimeFolder(Config.Instance.BackupRootDir, isFullPath: true);
                if (Directory.Exists(dateDir))
                {
                    var configFileName = Path.GetFileName(configFile);
                    var taskFileName = Path.GetFileName(taskFile);
                    File.Copy(configFile, Path.Combine(dateDir, configFileName));
                    File.Copy(taskFileName, Path.Combine(dateDir, taskFileName));

                    MessageBox.Show("バックアップ完了しました。以下に生成されています。\n[{0}]".Fmt(dateDir));
                }
            }
        }
    }
}
