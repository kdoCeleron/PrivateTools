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
        /// 初期表示時にメイン画面を表示するかどうか
        /// </summary>
        public bool IsInitShowMainForm { get; set; }

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
