using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.ConfigrationData
{
    /// <summary>
    /// 編集可能な設定情報
    /// </summary>
    [Serializable]
    public class EditableConfigData
    {
        /// <summary>
        /// タスクトレイ常駐起動をするかどうか
        /// </summary>
        public bool IsStayInTaskTray { get; set; }

        /// <summary>
        /// 初期表示時にメイン画面を表示するかどうか
        /// (タスクトレイ常駐起動時のみ有効)
        /// </summary>
        public bool IsInitShowMainForm { get; set; }

        /// <summary>
        /// Windowsの通知(トースト)を行うかどうか
        /// </summary>
        public bool IsNotifyWindowsToast { get; set; }

        /// <summary>
        /// 赤色表示のしきい値
        /// </summary>
        public int ThresDaysRed { get; set; }

        /// <summary>
        /// 黄色表示のしきい値
        /// </summary>
        public int ThresDaysYellow { get; set; }
    }
}
