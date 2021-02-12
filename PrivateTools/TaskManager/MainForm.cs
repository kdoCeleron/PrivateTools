using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;
using TaskManager.Data;
using Timer = System.Threading.Timer;

namespace TaskManager
{
    public partial class MainForm : Form
    {
        private Timer _nowTimer;

        public MainForm()
        {
            InitializeComponent();

            this.Load += OnLoad;
            this.Closing += OnClosing;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            //var jsonStr = JsonConvert.SerializeObject(ResourceManager.Instance.TaskInfoRoot, Formatting.None);
            //File.WriteAllText(@".\tasks.txt", jsonStr);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            UiContext.Initialize();

            ResourceManager.Instance.Initialize();

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            this._nowTimer = new Timer(NowTimerCallBack);
            this._nowTimer.Change(new TimeSpan(), TimeSpan.FromMinutes(1));

            this.DgvAllTasks.Initialize(true);
            this.DgvRecentTasks.Initialize(false);

            this.DgvAllTasks.UpdateEvent += DgvAllTasksOnUpdateEvent;
            
            // TODO:デフォルトは全タスクを表示
            this.LblDisplayGroup.Text = string.Format("[{0}]", "全てのタスク");

            this.InitializeGroupList();
        }

        private void DgvAllTasksOnUpdateEvent(object sender, TaskItem e)
        {
            // TODO:直近のタスク一覧の更新処理
            // タスク一覧との処理の調整があるため、後回し
            //var allGroup = ResourceManager.Instance.TaskGroupList;
            //var list = new List<TaskItem>();
            //foreach (var info in allGroup)
            //{
            //    list.AddRange(info.ChildTaskItems);

            //    // TODO:再帰
            //    foreach (var infoChildGroup in info.ChildGroups)
            //    {
            //        list.AddRange(infoChildGroup.ChildTaskItems);
            //    }
            //}

            // TODO:更新したときにグループの件数も再描画する。
            //this.DgvRecentTasks.RefleshTaskItems(list, null);
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
            //this.LsvGroup.DoubleClick += this.SymbolListView_OnDoubleClick;
            this.LsvGroup.ShowItemToolTips = true;

            RefleshTaskGroupIchiran();
        }

        private void LsvGroupOnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.LsvGroup.SelectedItems.Count > 0)
            {
                var selected = this.LsvGroup.SelectedItems[0].Tag as TaskGroupInfo;
                if (selected != null)
                {
                    this.DgvAllTasks.ClearAllTaskItems();

                    var tasks = new List<TaskItem>();
                    this.DgvAllTasks.RefleshTaskItems(selected.ChildTaskItems, selected);

                    // TODO:サブグループのタスクも


                    this.LblDisplayGroup.Text = string.Format("[{0}]", selected.Name);
                }
            }
        }

        private void RefleshTaskGroupIchiran()
        {
            this.LsvGroup.BeginUpdate();

            this.LsvGroup.Items.Clear();

            var topTasks = ResourceManager.Instance.TaskGroupList.Where(x => x.ParentGroup == TaskGroupInfo.GetRootGroup()).ToList();
            foreach (var taskGroupInfo in topTasks)
            {
                var taskName = taskGroupInfo.Name;
                var taskNum = taskGroupInfo.ChildTaskItems.Count;
                var lvItem = this.LsvGroup.Items.Add(taskName);
                lvItem.SubItems.Add(string.Format("{0:D}件", taskNum));
                lvItem.Tag = taskGroupInfo;

                // TODO:再帰
                foreach (var childGroup in taskGroupInfo.ChildGroups)
                {
                    var childTaskName = childGroup.Name;
                    var childTaskNum = childGroup.ChildTaskItems.Count;
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
                this.LabelDateTime.Text = "現在日時：" + now.ToString("yyyy/MM/dd HH:mm:ss");
            });
        }

        private void BtnAddGroup_Click(object sender, EventArgs e)
        {
            var parent = TaskGroupInfo.GetRootGroup();
            if (this.LsvGroup.SelectedItems.Count > 0)
            {
                var selected = this.LsvGroup.SelectedItems[0].Tag as TaskGroupInfo;
                if (selected != null)
                {
                    parent = selected;
                }
            }

            var win = new TaskGroupEditForm();
            win.Initialize(null, true, parent);
            win.ShowDialog();

            RefleshTaskGroupIchiran();
        }

        private void BtnRemoveGroup_Click(object sender, EventArgs e)
        {
            if (this.LsvGroup.SelectedItems.Count > 0)
            {
                var selected = this.LsvGroup.SelectedItems[0].Tag as TaskGroupInfo;
                if (selected != null)
                {
                    ResourceManager.Instance.RemoveTaskGroup(selected);
                    RefleshTaskGroupIchiran();
                }
            }
        }

        private void BtnReNameGroup_Click(object sender, EventArgs e)
        {
            if (this.LsvGroup.SelectedItems.Count > 0)
            {
                var selected = this.LsvGroup.SelectedItems[0].Tag as TaskGroupInfo;
                if (selected != null)
                {
                    var win = new TaskGroupEditForm();
                    win.Initialize(selected, false, selected.ParentGroup);
                    win.ShowDialog();

                    RefleshTaskGroupIchiran();
                }
            }
        }

        private void BtnInfos_Click(object sender, EventArgs e)
        {
            var win = new InfoViewForm();
            win.ShowDialog();
        }
    }
}
