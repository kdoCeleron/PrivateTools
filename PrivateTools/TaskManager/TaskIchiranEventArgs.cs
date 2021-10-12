namespace TaskManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TaskManager.Data;

    /// <summary>
    /// タスク一覧コントロールのイベント引数
    /// </summary>
    public class TaskIchiranEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskIchiranEventArgs()
        {
        }

        /// <summary>
        /// グループ情報
        /// </summary>
        public TaskGroupInfo GroupItem { get; set; }

        /// <summary>
        /// タスク情報
        /// </summary>
        public List<TaskItem> TaskItem { get; set; }
    }
}
