using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    /// <summary>
    /// タスク一覧の列種別
    /// </summary>
    public enum DataGridColumnType
    {
        Button,

        Label,

        TextBox
    }

    /// <summary>
    /// サブ画面の結果
    /// </summary>
    public enum SubWindowResult
    {
        None,

        Submit,

        Cancel
    }
}
