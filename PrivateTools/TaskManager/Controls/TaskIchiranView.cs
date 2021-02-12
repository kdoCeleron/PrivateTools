﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManager.Data;

namespace TaskManager.Controls
{
    public class TaskIchiranView : DataGridView
    {
        public event EventHandler<TaskItem> UpdateEvent;

        private Dictionary<int, DataGridColumnInfo> columnInfoMap;

        private bool isVisibleAddTask;

        private TaskGroupInfo showingGroup;

        public TaskIchiranView() : base()
        {
            this.columnInfoMap = new Dictionary<int, DataGridColumnInfo>();

            var index = 0;
            var item1 = new DataGridColumnInfo(index, DataGridColumnType.Button, DataGridViewContentAlignment.MiddleCenter, 50, string.Empty, "完了");
            this.columnInfoMap.Add(index, item1);

            index++;
            var item2 = new DataGridColumnInfo(index, DataGridColumnType.Label, DataGridViewContentAlignment.MiddleLeft, 100, "グループ", string.Empty);
            this.columnInfoMap.Add(index, item2);

            index++;
            var item3 = new DataGridColumnInfo(index, DataGridColumnType.Label, DataGridViewContentAlignment.MiddleLeft, 100, "タイトル", string.Empty);
            this.columnInfoMap.Add(index, item3);

            index++;
            var item4 = new DataGridColumnInfo(index, DataGridColumnType.Label, DataGridViewContentAlignment.MiddleLeft, 80, "期限", string.Empty);
            this.columnInfoMap.Add(index, item4);

            index++;
            var item5 = new DataGridColumnInfo(index, DataGridColumnType.Label, DataGridViewContentAlignment.MiddleLeft, 250, "メモ", string.Empty);
            this.columnInfoMap.Add(index, item5);

            index++;
            var item6 = new DataGridColumnInfo(index, DataGridColumnType.Button, DataGridViewContentAlignment.MiddleCenter, 50, string.Empty, "編集");
            this.columnInfoMap.Add(index, item6);

            index++;
            var item7 = new DataGridColumnInfo(index, DataGridColumnType.Button, DataGridViewContentAlignment.MiddleCenter, 50, string.Empty, "削除");
            this.columnInfoMap.Add(index, item7);

            index++;
            var item8 = new DataGridColumnInfo(index, DataGridColumnType.Button, DataGridViewContentAlignment.MiddleCenter, 50, string.Empty, "複製");
            this.columnInfoMap.Add(index, item8);
        }

        public void Initialize(bool canAddTask)
        {
            this.isVisibleAddTask = canAddTask;

            this.AllowUserToAddRows = canAddTask;
            this.AllowUserToResizeColumns = true;
            this.AllowUserToResizeRows = false;
            this.AllowDrop = false;
            this.MultiSelect = false;
            this.RowHeadersVisible = false;
            // this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.AllowUserToOrderColumns = false;
            this.ScrollBars = ScrollBars.Vertical;
            this.BackgroundColor = SystemColors.ControlLight;

            var style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style.BackColor = SystemColors.Control;
            style.Font = new Font("MS UI Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
            style.ForeColor = SystemColors.WindowText;
            style.WrapMode = DataGridViewTriState.True;
            style.SelectionBackColor = Color.Transparent;
            style.SelectionForeColor = Color.Black;

            this.ColumnHeadersDefaultCellStyle = style;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            //this.CellClick += this.OnCellClick;
            //this.CellEndEdit += this.OnCellEndEdit;
            //this.EditingControlShowing += this.OnEditingControlShowing;
            this.UserAddedRow += this.OnUserAddedRow;
            //this.Scroll += this.ScrollHandler;
            //this.CellEnter += this.CellEnterHandler;

            this.CellContentClick += OnCellContentClick;

            foreach (var info in this.columnInfoMap)
            {
                var data = info.Value;
                if (data.ColumnType == DataGridColumnType.Label)
                {
                    this.AddTextBoxHeader(data);
                }
                else if (data.ColumnType == DataGridColumnType.Button)
                {
                    this.AddButtonHeader(data);
                }
                else
                {
                    // TODO:追々
                }
            }

            this.UpdateCellStatus();
            this.SetShowScrollBar();
        }

        public void RefleshTaskItems(List<TaskItem> taskItems, TaskGroupInfo showingTaskGroup)
        {
            this.showingGroup = showingTaskGroup;

            foreach (var taskItem in taskItems)
            {
                this.AddTaskItem(taskItem);
            }

            this.UpdateCellStatus();
        }

        public void ClearAllTaskItems()
        {
            this.showingGroup = null;

            this.Rows.Clear();
            this.UpdateCellStatus();
        }

        private void AddTaskItem(TaskItem taskItem)
        {
            var dgvRow = new DataGridViewRow();
            dgvRow.CreateCells(this);

            for (int i = 0; i < dgvRow.Cells.Count; i++)
            {
                var btnCell = dgvRow.Cells[i] as DataGridViewButtonCell;
                if (btnCell != null)
                {
                    btnCell.Value = this.columnInfoMap[i].ContentName;
                }
            }

            dgvRow.Tag = taskItem;

            this.SetTaskItemToRow(taskItem, dgvRow);

            this.Rows.Add(dgvRow);
        }

        private void OnCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridColumnInfo info;
            if (this.columnInfoMap.TryGetValue(e.ColumnIndex, out info))
            {
                if (info != null)
                {
                    // TODO:このあたりの固定文字は調整
                    var orgRowIndex = e.RowIndex;
                    var orgColumnIndex = e.ColumnIndex;

                    var cell = this.Rows[orgRowIndex].Cells[orgColumnIndex];
                    //var text = string.Format("{0} {1} row:{2} column:{3}", cell.Value.ToString(), info.ContentName, e.RowIndex, e.ColumnIndex);
                    //MessageBox.Show(text);

                    if (cell.Value == null)
                    {
                        return;
                    }

                    var isShowEditForm = false;
                    if (cell.Value.ToString() == "追加")
                    {
                        this.AddRow();
                        isShowEditForm = true;
                    }

                    // Addrow でe.RowIndexの値が変更されるので取り直し。
                    var fixedCell = this.Rows[orgRowIndex].Cells[orgColumnIndex];
                    if (fixedCell.Value == null)
                    {
                        return;
                    }

                    var isButton = info.ColumnType == DataGridColumnType.Button;
                    if (!isButton)
                    {
                        return;
                    }

                    var contentName = fixedCell.Value.ToString();
                    var isEdit = contentName == "編集";
                    var isDelete = contentName == "削除";
                    var isCopy = contentName == "複製";
                    var isComplete = contentName == "完了";
                    var isTorikeshi = contentName == "取消";
                    if (isShowEditForm || isEdit)
                    {
                        var row = this.Rows[e.RowIndex];
                        var item = row.Tag as TaskItem;
                        if (item != null)
                        {
                            var win = new TaskEditForm();
                            win.Initialize(this.showingGroup, item);
                            win.UpdateEvent += TaskEditForm;
                            win.ShowDialog();
                        }
                    }
                    else if (isDelete)
                    {
                        var message = "タスクを削除します。";
                        var msgRet = MessageBox.Show(message, "確認", MessageBoxButtons.YesNo);
                        if (msgRet == DialogResult.Yes)
                        {
                            var row = this.Rows[e.RowIndex];
                            var item = row.Tag as TaskItem;
                            if (item != null)
                            {
                                this.Rows.Remove(row);
                                ResourceManager.Instance.TaskInfoRoot.RemoveTaskItem(item);

                                this.RefleshTaskItems(this.showingGroup.ChildTaskItems.ToList(), this.showingGroup);
                            }
                        }
                    }
                    else if (isCopy)
                    {
                        var row = this.Rows[e.RowIndex];
                        var item = row.Tag as TaskItem;
                        if (item != null)
                        {
                            var newItem = new TaskItem();

                            newItem.Group = item.Group;
                            newItem.Title = item.Title;
                            newItem.Memo = item.Memo;
                            newItem.DateTimeLimit = item.DateTimeLimit;

                            this.AddTaskItem(newItem);
                        }
                    }
                    else if (isComplete)
                    {
                        var row = this.Rows[e.RowIndex];
                        var item = row.Tag as TaskItem;
                        if (item != null)
                        {
                            item.IsComeplate = true;
                        }
                    }
                    else if (isTorikeshi)
                    {
                        var row = this.Rows[e.RowIndex];
                        var item = row.Tag as TaskItem;
                        if (item != null)
                        {
                            item.IsComeplate = false;
                        }
                    }

                    this.UpdateCellStatus();
                }
            }
        }

        private void TaskEditForm(object sender, TaskItem e)
        {
            for (int i = 0; i < this.Rows.Count; i++)
            {
                var row = this.Rows[i];
                var item = row.Tag as TaskItem;
                if (item != null && item == e)
                {
                    ResourceManager.Instance.TaskInfoRoot.AddTaskItem(e.Group, e);
                    SetTaskItemToRow(item, row);
                }
            }
        }

        private void SetTaskItemToRow(TaskItem item, DataGridViewRow row)
        {
            foreach (var info in this.columnInfoMap)
            {
                if (info.Value.ColumnType != DataGridColumnType.Button)
                {
                    var columnIndex = info.Value.ColumnIndex;

                    // TODO:プロパティの参照とインデックスの組み合わせ
                    string setValue = string.Empty;
                    if (columnIndex == 1)
                    {
                        setValue = ResourceManager.Instance.TaskInfoRoot.TaskGroupList[item.Group].Name;
                    }
                    else if (columnIndex == 2)
                    {
                        setValue = item.Title;
                    }
                    else if (columnIndex == 3)
                    {
                        setValue = item.DateTimeLimit.ToString("yyyy/MM/dd");
                    }
                    else if (columnIndex == 4)
                    {
                        setValue = item.Memo;
                    }

                    row.Cells[columnIndex].Value = setValue;
                }
            }
        }

        private void AddRow()
        {
            var dgvRow = new DataGridViewRow();
            dgvRow.CreateCells(this);

            for (int i = 0; i < dgvRow.Cells.Count; i++)
            {
                var btnCell = dgvRow.Cells[i] as DataGridViewButtonCell;
                if (btnCell != null)
                {
                    btnCell.Value = this.columnInfoMap[i].ContentName;
                }
            }

            dgvRow.Tag = new TaskItem();

            this.Rows.Add(dgvRow);
        }

        /// <summary>
        /// テキストボックス用のヘッダを追加します
        /// </summary>
        private void AddTextBoxHeader(DataGridColumnInfo info)
        {
            var header = new DataGridViewTextBoxColumn();
            this.SetCommnoHeaderStyle(info.HeaderName, info.Width, info.Alignment, header);
            header.Tag = info;
            this.Columns.Add(header);
        }

        /// <summary>
        /// コンボボックス用のヘッダを追加します
        /// </summary>
        private void AddButtonHeader(DataGridColumnInfo info)
        {
            var header = new DataGridViewButtonColumn();

            this.SetCommnoHeaderStyle(info.HeaderName, info.Width, info.Alignment, header);
            header.Tag = info;

            this.Columns.Add(header);
        }
        
        /// <summary>
        /// 列ヘッダの共通設定を行います。
        /// </summary>
        /// <param name="headerName">ヘッダ名</param>
        /// <param name="headerSize">ヘッダの幅</param>
        /// <param name="alignment">アライメント</param>
        /// <param name="header">列ヘッダオブジェクト</param>
        private void SetCommnoHeaderStyle(string headerName, int headerSize, DataGridViewContentAlignment alignment, DataGridViewColumn header)
        {
            var headerStyle = new DataGridViewCellStyle();
            headerStyle.Alignment = alignment;
            headerStyle.SelectionForeColor = Color.Black;
            headerStyle.SelectionBackColor = Color.Transparent;

            header.DefaultCellStyle = headerStyle;
            header.HeaderText = headerName;
            header.Resizable = DataGridViewTriState.False;
            header.SortMode = DataGridViewColumnSortMode.NotSortable;

            header.Width = headerSize;
            header.FillWeight = headerSize;
        }
        
        private void UpdateCellStatus()
        {
            for (var index = 0; index < this.RowCount; index++)
            {
                var row = this.Rows[index];
                this.CellReadOnly(index, row);

                var grpData = row.Tag as TaskItem;
                if (grpData != null)
                {
                    var isEnable = false;
                    var kanryoCell = row.Cells[0] as DataGridViewButtonCell;
                    if (kanryoCell != null)
                    {
                        if (grpData.IsComeplate)
                        {
                            kanryoCell.Value = "取消";
                            for (int i = 0; i < row.Cells.Count; i++)
                            {
                                row.Cells[i].Style.BackColor = Color.Gray;
                            }
                        }
                        else
                        {
                            kanryoCell.Value = "完了";
                            for (int i = 0; i < row.Cells.Count; i++)
                            {
                                row.Cells[i].Style.BackColor = Color.White;
                            }

                            isEnable = true;
                        }
                    }

                    if (isEnable)
                    {
                        var tmp = DateTime.Now;
                        var now = new DateTime(tmp.Year, tmp.Month, tmp.Day);
                        var date = grpData.DateTimeLimit;

                        var cell = row.Cells[3] as DataGridViewCell;
                        if (now <= (date.AddDays(1)))
                        {
                            cell.Style.BackColor = Color.Red;
                        }
                        else if (now <= (date.AddDays(3)))
                        {
                            cell.Style.BackColor = Color.Yellow;
                        }
                    }
                }

                if (this.isVisibleAddTask)
                {
                    if (this.RowCount - 1 == index)
                    {
                        var btnCell = row.Cells[0] as DataGridViewButtonCell;
                        if (btnCell != null)
                        {
                            btnCell.Value = "追加";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ユーザ入力によって行追加が行われた際のイベントです
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        /// <remarks>
        /// <para>CREATE 2020-10-01 アルファテクノロジー 近藤 匠 </para>
        /// <para>UPDATE </para>
        /// </remarks>
        private void OnUserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            this.CellReadOnly(e.Row.Index, e.Row);
        }

        private void CellReadOnly(int rowIndx, DataGridViewRow row)
        {
            for (var index = 1; index < row.Cells.Count; index++)
            {
                var cell = this[index, rowIndx];
                cell.ReadOnly = true;
            }
        }


        private void SetShowScrollBar()
        {
            foreach (Control control in this.Controls)
            {
                if (control is VScrollBar)
                {
                    var vsb = (VScrollBar)control;
                    vsb.Visible = true;
                    vsb.VisibleChanged += new EventHandler(this.ScrollBarOnVisibleChanged);
                    vsb.BackColor = Color.Black;
                }
            }
        }

        private void ScrollBarOnVisibleChanged(object sender, EventArgs e)
        {
            var scrollBar = sender as VScrollBar;
            if (scrollBar == null)
            {
                return;
            }

            if (!scrollBar.Visible)
            {
                scrollBar.Location = new Point(this.ClientRectangle.Width - scrollBar.Width - 1, 1);
                scrollBar.Size = new Size(scrollBar.Width, this.ClientRectangle.Height - 2);
                scrollBar.Show();
            }
        }

    }
}
