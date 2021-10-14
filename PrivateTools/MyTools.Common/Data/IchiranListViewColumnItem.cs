using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool.Common.Data
{
    using System.Windows.Forms;

    /// <summary>
    /// 一覧listviewの列項目情報
    /// </summary>
    [Serializable]
    public class IchiranListViewColumnItem
    {
        /// <summary>
        /// datatypeのキャッシュ
        /// </summary>
        private Type _dataType = null;

        /// <summary>
        /// タイトル
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// 幅
        /// </summary>
        public int Width{ get; set; }
        
        /// <summary>
        /// 表示するかどうか
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// 項目のアライメント
        /// </summary>
        public HorizontalAlignment Alignment { get; set; }

        /// <summary>
        /// 型名
        /// </summary>
        public string DataTypeName { get; set; }

        /// <summary>
        /// 型
        /// </summary>
        public Type DataType
        {
            get
            {
                if (this._dataType == null)
                {
                    this._dataType = Type.GetType(this.DataTypeName);
                }

                return this._dataType;
            }
        }
    }
}
