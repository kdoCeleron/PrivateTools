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
using TaskManager.Configration;
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
            ResourceManager.Instance.SaveTaskList();
            Config.Instance.WriteConfig();
        }

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

            this.InitializeTaskList();
            this.DgvAllTasksOnUpdateEvent(null, null);

            this.InitializeGroupList();
        }

        private bool isUpdatingTaskIchiran = false;

        private void DgvRecentTasksOnUpdateEvent(object sender, TaskItem e)
        {
            if (isUpdatingTaskIchiran)
            {
                return;
            }

            isUpdatingTaskIchiran = true;

            // TODO:
            //this.tmpTaskList.Clear();

            //ResourceManager.Instance.ExecInnerGroupAndTasks(TaskGroupInfo.GetRootGroup(), null, CollectTaskItem);
            
            //this.DgvAllTasks.ClearAllTaskItems();

            //this.RefleshTaskGroupIchiran();
            this.DgvAllTasks.ClearAllTaskItems();
            if (this.DgvAllTasks.showingGroup != null)
            {
                this.DgvAllTasks.RefleshTaskItems(this.DgvAllTasks.showingGroup.ChildTaskItems.ToList(), this.DgvAllTasks.showingGroup);
            }
            else
            {
                InitializeTaskList();
            }

            this.isUpdatingTaskIchiran = false;
        }

        private void DgvAllTasksOnUpdateEvent(object sender, TaskItem e)
        {
            if (isUpdatingTaskIchiran)
            {
                return;
            }

            isUpdatingTaskIchiran = true;

            this.tmpTaskList.Clear();

            ResourceManager.Instance.ExecInnerGroupAndTasks(TaskGroupInfo.GetRootGroup(), null, CollectTaskItem);

            var tmp = DateTime.Now;
            var now = new DateTime(tmp.Year, tmp.Month, tmp.Day);
            var thres = now.AddDays(3);

            //now >= date
            // 超過、1日前、2日前、3日前
            var filtered = tmpTaskList.Where(x => x.DateTimeLimit < thres).OrderBy(x => x.DateTimeLimit).ToList();
            this.DgvRecentTasks.ClearAllTaskItems();
            this.DgvRecentTasks.RefleshTaskItems(filtered, null);

            this.RefleshTaskGroupIchiran();

            this.isUpdatingTaskIchiran = false;
        }

        private List<TaskItem> tmpTaskList = new List<TaskItem>();

        private void InitializeTaskList()
        {
            this.LblDisplayGroup.Text = string.Format("[{0}]", "全てのタスク");
            this.tmpTaskList.Clear();

            ResourceManager.Instance.ExecInnerGroupAndTasks(TaskGroupInfo.GetRootGroup(), null, CollectTaskItem);

            this.DgvAllTasks.RefleshTaskItems(this.tmpTaskList, null);
        }

        private void CollectTaskItem(TaskItem obj)
        {
            this.tmpTaskList.Add(obj);
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
                var selectedItemIndex = this.LsvGroup.SelectedItems[0].Index;
                var selected = this.LsvGroup.SelectedItems[0].Tag as KeyInfo;
                if (selected != null)
                {
                    this.DgvAllTasks.ClearAllTaskItems();

                    var group = ResourceManager.Instance.TaskInfoRoot.TaskGroupList[selected];
                    
                    this.DgvAllTasks.RefleshTaskItems(group.ChildTaskItems.ToList(), group);

                    // TODO:サブグループのタスクも
                    
                    this.LblDisplayGroup.Text = string.Format("[{0}]", group.Name);

                    this.isSuspentGroupListView = true;
                    this.LsvGroup.Items[selectedItemIndex].Focused = true;
                    this.LsvGroup.Items[selectedItemIndex].Selected = true;
                    this.isSuspentGroupListView = false;
                }
            }
        }

        private bool isSuspentGroupListView = false;

        private void RefleshTaskGroupIchiran()
        {
            if (this.isSuspentGroupListView)
            {
                return;
            }

            this.LsvGroup.BeginUpdate();

            this.LsvGroup.Items.Clear();

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
                    var group = ResourceManager.Instance.TaskInfoRoot.TaskGroupList[childGroup];
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
                    var item = ResourceManager.Instance.TaskInfoRoot.TaskGroupList[selected];
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
                    var item = ResourceManager.Instance.TaskInfoRoot.TaskGroupList[selected];
                    if (!selected.Equals(TaskGroupInfo.GetRootGroup().Key) && !selected.Equals(TaskGroupInfo.GetDefaultGroup().Key))
                    {
                        ResourceManager.Instance.TaskInfoRoot.RemoveTaskGroup(item);
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
                    var item = ResourceManager.Instance.TaskInfoRoot.TaskGroupList[selected];
                    if (!selected.Equals(TaskGroupInfo.GetRootGroup().Key) && !selected.Equals(TaskGroupInfo.GetDefaultGroup().Key))
                    {
                        var win = new TaskGroupEditForm();
                        win.Initialize(item, false, ResourceManager.Instance.TaskInfoRoot.TaskGroupList[item.ParentGroup]);
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
    }
}
