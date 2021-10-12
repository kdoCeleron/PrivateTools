// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskIchiranView.cs" company="">
//   
// </copyright>
// <summary>
//   タスク一覧コントロール
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TaskManager.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using TaskManager.Data;

    /// <summary>
    /// タスク一覧コントロール
    /// </summary>
    public class TaskIchiranView : DataGridView
    {
        /// <summary>
        /// 列情報のマッピング
        /// </summary>
        private Dictionary<int, DataGridColumnInfo> columnInfoMap;

        /// <summary>
        /// 「追加」ボタンを表示するかどうか
        /// </summary>
        private bool isVisibleAddTask;

        /// <summary>
        /// コンストラクタ
        /// </summary>
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

        /// <summary>
        /// 一覧更新時イベント
        /// </summary>
        public event EventHandler<TaskIchiranEventArgs> UpdateEvent;

        /// <summary>
        /// 表示中のタスクグループ
        /// </summary>
        public TaskGroupInfo ShowingGroup { get; private set; }
        
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="canAddTask">タスク追加機能を有効にするか</param>
        public void Initialize(bool canAddTask)
        {
            this.isVisibleAddTask = canAddTask;

            this.AllowUserToAddRows = canAddTask;
            this.AllowUserToResizeColumns = true;
            this.AllowUserToResizeRows = false;
            this.AllowDrop = false;
            this.MultiSelect = false;
            this.RowHeadersVisible = false;
            this.AllowUserToOrderColumns = false;
            this.ScrollBars = ScrollBars.Both;
            this.BackgroundColor = SystemColors.ControlLight;
            this.CellDoubleClick += this.OnCellDoubleClick;

            var style = new DataGridViewCellStyle();
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            style.BackColor = SystemColors.Control;
            style.Font = new Font("MS UI Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
            style.ForeColor = SystemColors.WindowText;
            style.WrapMode = DataGridViewTriState.True;
            style.SelectionBackColor = Color.Aqua;
            style.SelectionForeColor = Color.Black;

            this.ColumnHeadersDefaultCellStyle = style;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            
            this.UserAddedRow += this.OnUserAddedRow;
            this.Scroll += this.ScrollHandler;

            this.CellContentClick += this.OnCellContentClick;

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
        }

        /// <summary>
        /// /タスク一覧の内容を指定のデータでリフレッシュします。
        /// </summary>
        /// <param name="taskItems">表示対象のタスク</param>
        /// <param name="showingTaskGroup">表示対象のグループ</param>
        public void RefleshTaskItems(List<TaskItem> taskItems, TaskGroupInfo showingTaskGroup)
        {
            this.ClearAllTaskItems();

            this.ShowingGroup = showingTaskGroup;

            foreach (var taskItem in taskItems)
            {
                this.AddRow(taskItem);
            }

            this.UpdateCellStatus();
        }

        /// <summary>
        /// セルのダブルクリックイベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private async void OnCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var rowIndex = e.RowIndex;
            var columnIndex = e.ColumnIndex;
            if (rowIndex >= 0)
            {
                var row = this.Rows[rowIndex];
                var task = this.GetTaskItemInRow(row);
                if (task != null)
                {
                    var cell = row.Cells[columnIndex];
                    if (cell != null && cell is DataGridViewButtonCell)
                    {
                        // ボタンのセルは反応させない
                        return;
                    }

                    if (!task.IsComeplate)
                    {
                        var ret = await this.ExecuteEdit(task);
                    }
                }
            }
        }

        /// <summary>
        /// 一覧中の全てのタスクをクリアします。
        /// </summary>
        private void ClearAllTaskItems()
        {
            this.Rows.Clear();
            this.UpdateCellStatus();
        }

        /// <summary>
        /// タスクを一覧に追加します。
        /// </summary>
        /// <param name="item">タスク</param>
        private void AddRow(TaskItem item)
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

            dgvRow.Tag = item;

            var menu = new ContextMenuStrip();
            menu.ItemClicked += this.MenuOnItemClicked;
            menu.Items.Add("クリップボードにコピー");
            dgvRow.ContextMenuStrip = menu;

            this.SetTaskItemToRow(item, dgvRow);

            this.Rows.Add(dgvRow);
        }
        
        /// <summary>
        /// セル上のボタン押下時イベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private async void OnCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var rowIndex = e.RowIndex;
            var columnIndex = e.ColumnIndex;

            DataGridColumnInfo info;
            if (this.columnInfoMap.TryGetValue(columnIndex, out info))
            {
                if (info != null)
                {
                    if (rowIndex < 0 || rowIndex >= this.RowCount)
                    {
                        return;
                    }

                    if (columnIndex < 0 || columnIndex >= this.ColumnCount)
                    {
                        return;
                    }

                    var cell = this.Rows[rowIndex].Cells[columnIndex];
                    if (cell.Value == null)
                    {
                        return;
                    }

                    var isButton = info.ColumnType == DataGridColumnType.Button;
                    if (!isButton)
                    {
                        return;
                    }

                    var row = this.Rows[rowIndex];

                    var contentName = cell.Value.ToString();
                    var isAdd = contentName == "追加";
                    var isEdit = contentName == "編集";
                    var isDelete = contentName == "削除";
                    var isCopy = contentName == "複製";
                    var isComplete = contentName == "完了";
                    var isTorikeshi = contentName == "取消";

                    var curItem = this.GetTaskItemInRow(row);

                    var ret = false;

                    // 完了済みタスクかどうか
                    var isCompleted = curItem != null ? curItem.IsComeplate : false;
                    if (isCompleted)
                    {
                        // 完了済みの場合は取り消し,削除のみ許可
                        if (isTorikeshi)
                        {
                            var item = this.GetTaskItemInRow(row);
                            if (item != null)
                            {
                                item.IsComeplate = false;
                                ret = true;
                            }
                        }
                        else if (isDelete)
                        {
                            ret = this.ExecuteDelete(row);
                        }
                    }
                    else
                    {
                        // 未完了のタスク
                        if (isAdd)
                        {
                            // 追加
                            ret = await this.ExecuteAdd();
                        }
                        else if (isEdit)
                        {
                            // 編集
                            if (curItem != null)
                            {
                                ret = await this.ExecuteEdit(curItem);
                            }
                        }
                        else if (isDelete)
                        {
                            // 削除
                            ret = this.ExecuteDelete(row);
                        }
                        else if (isCopy)
                        {
                            // 複製
                            if (curItem != null)
                            {
                                ret = this.ExecuteCopy(curItem);
                            }
                        }
                        else if (isComplete)
                        {
                            // 完了
                            if (curItem != null)
                            {
                                curItem.IsComeplate = true;
                                ret = true;
                            }
                        }
                    }
                    
                    this.UpdateCellStatus();

                    if (ret)
                    {
                        if (this.UpdateEvent != null)
                        {
                            var args = new TaskIchiranEventArgs();
                            args.GroupItem = this.ShowingGroup;
                            args.TaskItem = new List<TaskItem>();

                            this.UpdateEvent(this, args);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 追加処理を実行します。
        /// </summary>
        /// <returns>Task</returns>
        private async Task<bool> ExecuteAdd()
        {
            var item = new TaskItem();
            KeyInfo groupKey = null;
            if (this.ShowingGroup != null)
            {
                groupKey = this.ShowingGroup.Key;
            }

            item.Key = KeyInfo.CreateKeyInfoTask(groupKey);

            var win = new TaskEditForm();
            win.Initialize(this.ShowingGroup, item);
            var ret = await win.ShowWindow(ResourceManager.Instance.MainForm);
            if (ret == SubWindowResult.Submit)
            {
                if (item.Group != null)
                {
                    if (item.Group.Equals(groupKey) || this.ShowingGroup == null)
                    {
                        ResourceManager.Instance.AddTaskItem(item.Group, item);
                        this.AddRow(item);
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 編集処理を実行します。
        /// </summary>
        /// <param name="taskItem">タスク</param>
        /// <returns>Task</returns>
        private async Task<bool> ExecuteEdit(TaskItem taskItem)
        {
            var win = new TaskEditForm();
            win.Initialize(this.ShowingGroup, taskItem);
            var ret = await win.ShowWindow(ResourceManager.Instance.MainForm);
            if (ret == SubWindowResult.Submit)
            {
                ResourceManager.Instance.AddTaskItem(taskItem.Group, taskItem);
                if (this.ShowingGroup != null)
                {
                    this.RefleshTaskItems(this.ShowingGroup.ChildTaskItems.ToList(), this.ShowingGroup);
                }
                else
                {
                    if (!this.isVisibleAddTask)
                    {
                        var taskList = ResourceManager.Instance.GetAllTaskItems();

                        var filtered = Utils.FilterRecentLimitTask(taskList);
                        this.RefleshTaskItems(filtered, null);
                    }
                    else
                    {
                        var list = ResourceManager.Instance.GetAllTaskItems();
                        this.RefleshTaskItems(list, null);
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 削除処理を実行します。
        /// </summary>
        /// <param name="row">行</param>
        /// <returns>Task</returns>
        private bool ExecuteDelete(DataGridViewRow row)
        {
            var message = "タスクを削除します。";
            var msgRet = MessageBox.Show(message, "確認", MessageBoxButtons.YesNo);
            if (msgRet == DialogResult.Yes)
            {
                var item = this.GetTaskItemInRow(row);
                if (item != null)
                {
                    this.Rows.Remove(row);
                    ResourceManager.Instance.RemoveTaskItem(item);

                    if (this.ShowingGroup != null)
                    {
                        this.RefleshTaskItems(this.ShowingGroup.ChildTaskItems.ToList(), this.ShowingGroup);
                    }
                    else
                    {
                        // TODO: 編集と共通化
                        if (!this.isVisibleAddTask)
                        {
                            var taskList = ResourceManager.Instance.GetAllTaskItems();

                            var filtered = Utils.FilterRecentLimitTask(taskList);
                            this.RefleshTaskItems(filtered, null);
                        }
                        else
                        {
                            var list = ResourceManager.Instance.GetAllTaskItems();
                            this.RefleshTaskItems(list, null);
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 複製処理を実行します。
        /// </summary>
        /// <param name="taskItem">タスク</param>
        /// <returns>Task</returns>
        private bool ExecuteCopy(TaskItem taskItem)
        {
            var newItem = new TaskItem();

            newItem.Group = taskItem.Group;
            newItem.Title = taskItem.Title;
            newItem.Memo = taskItem.Memo;
            newItem.DateTimeLimit = taskItem.DateTimeLimit;
            newItem.Key = KeyInfo.CreateKeyInfoTask(taskItem.Group);

            this.AddRow(newItem);

            if (taskItem.Group != null)
            {
                ResourceManager.Instance.AddTaskItem(taskItem.Group, newItem);
            }

            // TODO: 共通化
            if (this.ShowingGroup != null)
            {
                this.RefleshTaskItems(this.ShowingGroup.ChildTaskItems.ToList(), this.ShowingGroup);
            }
            else
            {
                // TODO: 編集と共通化
                if (!this.isVisibleAddTask)
                {
                    var taskList = ResourceManager.Instance.GetAllTaskItems();

                    var filtered = Utils.FilterRecentLimitTask(taskList);
                    this.RefleshTaskItems(filtered, null);
                }
                else
                {
                    var list = ResourceManager.Instance.GetAllTaskItems();
                    this.RefleshTaskItems(list, null);
                }
            }

            return true;
        }

        /// <summary>
        /// 行の内容にタスク情報を設定します。
        /// </summary>
        /// <param name="item">タスク</param>
        /// <param name="row">行</param>
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
                        setValue = ResourceManager.Instance.GetGroupName(item.Group);
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

        /// <summary>
        /// コンテキストメニュー実行時のイベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void MenuOnItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var selected = this.SelectedCells;
            if (selected.Count == 1)
            {
                var row = selected[0].OwningRow;
                var task = row.Tag as TaskItem;
                if (task != null)
                {
                    var txt = task.GetInfoText();
                    Clipboard.SetText(txt);
                }
            }
        }

        /// <summary>
        /// テキストボックス用のヘッダを追加します
        /// </summary>
        /// <param name="info">ヘッダ情報</param>
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
        /// <param name="info">ヘッダ情報</param>
        private void AddButtonHeader(DataGridColumnInfo info)
        {
            var header = new DataGridViewButtonColumn();

            this.SetCommnoHeaderStyle(info.HeaderName, info.Width, info.Alignment, header);
            header.SortMode = DataGridViewColumnSortMode.NotSortable;
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
            headerStyle.SelectionBackColor = Color.Aqua;

            header.DefaultCellStyle = headerStyle;
            header.HeaderText = headerName;
            header.Resizable = DataGridViewTriState.True;
            header.SortMode = DataGridViewColumnSortMode.Automatic;

            header.Width = headerSize;
            header.FillWeight = headerSize;
        }
        
        /// <summary>
        /// セルの状態を更新します。
        /// </summary>
        private void UpdateCellStatus()
        {
            var taskList = new List<TaskItem>();
            for (var index = 0; index < this.RowCount; index++)
            {
                var row = this.Rows[index];
                this.CellReadOnly(index, row);

                var grpData = row.Tag as TaskItem;
                if (grpData != null)
                {
                    taskList.Add(grpData);

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
                        var date = new DateTime(grpData.DateTimeLimit.Year, grpData.DateTimeLimit.Month, grpData.DateTimeLimit.Day);

                        var cell = row.Cells[3] as DataGridViewCell;

                        // TODO: Configへ
                        if (Utils.IsOverRedZone(date))
                        {
                            cell.Style.BackColor = Color.Red;
                        }
                        else if (Utils.IsOverYellowZone(date))
                        {
                            cell.Style.BackColor = Color.Yellow;
                        }
                        else
                        {
                            cell.Style.BackColor = Color.White;
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
        /// 行内のタスク情報を取得します。
        /// </summary>
        /// <param name="row">行</param>
        /// <returns>タスク情報</returns>
        private TaskItem GetTaskItemInRow(DataGridViewRow row)
        {
            if (row != null)
            {
                var item = row.Tag as TaskItem;
                if (item != null)
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// ユーザ入力によって行追加が行われた際のイベントです
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void OnUserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            this.CellReadOnly(e.Row.Index, e.Row);
        }

        /// <summary>
        /// セルを編集不可状態にします。
        /// </summary>
        /// <param name="rowIndx">行index</param>
        /// <param name="row">行</param>
        private void CellReadOnly(int rowIndx, DataGridViewRow row)
        {
            for (var index = 1; index < row.Cells.Count; index++)
            {
                var cell = this[index, rowIndx];
                cell.ReadOnly = true;
            }
        }

        /// <summary>
        /// スクロール実施時イベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void ScrollHandler(object sender, ScrollEventArgs e)
        {
            this.ClearSelection();
        }
    }
}
