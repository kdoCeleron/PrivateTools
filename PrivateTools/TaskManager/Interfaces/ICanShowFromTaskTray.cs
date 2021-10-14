using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Interfaces
{
    /// <summary>
    /// タスクトレイから表示できることを示すインターフェースです
    /// </summary>
    public interface ICanShowFromTaskTray
    {
        /// <summary>
        /// タスクトレイから表示中かどうか
        /// </summary>
        bool IsShowFromTaskTray { get; set; }

        /// <summary>
        /// 画面種別
        /// </summary>
        ViewKind ViewType { get; }
    }
}
