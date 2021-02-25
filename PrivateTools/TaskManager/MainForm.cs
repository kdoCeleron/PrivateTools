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

using TaskManager.Configration;
using TaskManager.Data;
using Timer = System.Threading.Timer;

namespace TaskManager
{
    /// <summary>
    /// メインフォーム
    /// </summary>
    public partial class MainForm : Form
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
        /// 画面が閉じられる際のイベントです
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void OnClosing(object sender, CancelEventArgs e)
        {
            ResourceManager.Instance.SaveTaskList();
            Config.Instance.WriteConfig();
        }

        /// <summary>
        /// 画面ロード時のイベントです
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void OnLoad(object sender, EventArgs e)
        {
            UiContext.Initialize();

            ResourceManager.Instance.Initialize();
            ResourceManager.Instance.MainForm = this;

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            this._nowTimer = new Timer(NowTimerCallBack);
            this._nowTimer.Change(new TimeSpan(), TimeSpan.FromMinutes(1));

            this.DgvAllTasks.Initialize(true);
            this.DgvRecentTasks.Initialize(false);

            this.DgvAllTasks.UpdateEvent += DgvAllTasksOnUpdateEvent;
            this.DgvRecentTasks.UpdateEvent += DgvRecentTasksOnUpdateEvent;

            this.ShowAllTaskListInDgvAllTasks();
            this.DgvAllTasksOnUpdateEvent(null, null);

            this.InitializeGroupList();

            this.TxtFilter.TextChanged += TxtFilter_OnTextChanged;
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

        private void DgvAllTasksOnUpdateEvent(object sender, TaskIchiranEventArgs e)
        {
            if (this.isUpdatingTaskIchiran)
            {
                return;
            }

            isUpdatingTaskIchiran = true;
            
            // TODO:一覧のdelete処理と統一
            var taskList = ResourceManager.Instance.GetAllTaskItems();
            
            var filtered = Utils.FilterRecentLimitTask(taskList);
            this.DgvRecentTasks.RefleshTaskItems(filtered, null);

            this.RefleshTaskGroupIchiran();

            this.isUpdatingTaskIchiran = false;
        }
        
        private void ShowAllTaskListInDgvAllTasks()
        {
            this.LblDisplayGroup.Text = string.Format("[{0}]", "全てのタスク");
            var taskList = ResourceManager.Instance.GetAllTaskItems();
            this.DgvAllTasks.RefleshTaskItems(taskList, null);
        }

        private void InitializeGroupList()
        {
            // シンボル一覧の設定
            this.LsvGroup.HideSelection = false;
            this.LsvGroup.FullRowSelect = true;
            this.LsvGroup.MultiSelect = false;
            this.LsvGroup.GridLines = true;
            this.LsvGroup.View = View.Details;
            this.LsvGroup.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.LsvGroup.ColumnWidthChanging += this.SymbolListView_OnColumnWidthChanging;
            this.LsvGroup.SelectedIndexChanged += LsvGroupOnSelectedIndexChanged;
            this.LsvGroup.ShowItemToolTips = true;

            this.RefleshTaskGroupIchiran();
        }

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

                        // TODO:サブグループのタスクも

                        this.LblDisplayGroup.Text = string.Format("[{0}]", group.Name);
                    }

                    this.isSuspentGroupListView = true;
                    this.LsvGroup.Items[selectedItemIndex].Focused = true;
                    this.LsvGroup.Items[selectedItemIndex].Selected = true;
                    this.isSuspentGroupListView = false;
                }
            }
        }

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

                // TODO:再帰
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

        private void SymbolListView_OnColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
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

        private void NowTimerCallBack(object state)
        {
            UiContext.Post(() =>
            {
                var now = DateTime.Now;
                this.LabelDateTime.Text = "現在日時：" + now.ToString("yyyy/MM/dd HH:mm");

                // TODO:recenttask update
            });
        }

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

            RefleshTaskGroupIchiran();
        }

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
                        RefleshTaskGroupIchiran();
                    }
                }
            }
        }

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

                        RefleshTaskGroupIchiran();
                    }
                }
            }
        }

        private async void BtnInfos_Click(object sender, EventArgs e)
        {
            var win = new InfoViewForm();
            var ret = await win.ShowWindow(this);
        }

        private void BtnOpenExecFolder_Click(object sender, EventArgs e)
        {
            var path = Environment.CurrentDirectory;
            if (Directory.Exists(path))
            {
                System.Diagnostics.Process.Start(path);
            }
        }

        private void BtnOutputCsv_Click(object sender, EventArgs e)
        {
            var allTaskList = ResourceManager.Instance.GetAllTaskItems();

            var list = new List<string>();
            foreach (var taskItem in allTaskList)
            {
                var tmp = taskItem.GetInfoText(",");
                tmp = tmp.Replace(Environment.NewLine, @"\r\n");
                list.Add(tmp);
            }

            var fileName = string.Format("TaskAllList_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss"));
            var path = Utils.GetFullPath(fileName);
            File.WriteAllLines(path, list, Encoding.UTF8);

            var dir = Path.GetDirectoryName(path);
            if (Directory.Exists(dir))
            {
                System.Diagnostics.Process.Start(dir);
            }
        }
    }
}
