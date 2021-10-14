using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool.Common.Data
{
    /// <summary>
    /// テキスト分割情報
    /// </summary>
    public class TextSeparatorItem
    {
        /// <summary>
        /// セパレータ
        /// </summary>
        public string Separator { get; set; }

        /// <summary>
        /// C#クラスのパラメータ設定時のプロパティ名
        /// (リフレクションを用いた動的設定用)
        /// </summary>
        public string ParameterPropertyName { get; set; }
    }
}
