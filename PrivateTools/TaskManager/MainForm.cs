using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
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

            // TODO:仮処理
            for (int i = 0; i < 10; i++)
            {
                var group = ResourceManager.Instance.AddTaskGroup("グループ" + string.Format("{0:D}", i));
            }

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

            var topTasks = ResourceManager.Instance.TaskGroupList.Where(x => x.ParentGroup == null).ToList();
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
            if (this.LsvGroup.SelectedItems.Count > 0)
            {
                var selected = this.LsvGroup.SelectedItems[0].Tag as TaskGroupInfo;
                if (selected != null)
                {
                    var win = new TaskGroupEditForm();
                    win.Initialize(null, true, selected);
                    win.ShowDialog();

                    RefleshTaskGroupIchiran();
                }
            }
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
    }
}
