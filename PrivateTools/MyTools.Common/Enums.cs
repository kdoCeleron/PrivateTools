using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTools.Common
{
    /// <summary>
    /// メッセージ種別
    /// </summary>
    public enum MessageKind
    {
        /// <summary>
        /// 確認
        /// </summary>
        Confirm,

        /// <summary>
        /// 情報
        /// </summary>
        Info,

        /// <summary>
        /// 警告
        /// </summary>
        Warn,

        /// <summary>
        /// エラー
        /// </summary>
        Error,
    }
}
