using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManager.Data
{
    /// <summary>
    /// 一覧の列情報定義
    /// </summary>
    public class DataGridColumnInfo
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="columnIndex">列インデックス</param>
        /// <param name="type">列の種別</param>
        /// <param name="alingn">アライメント</param>
        /// <param name="width">幅</param>
        /// <param name="header">ヘッダ名称</param>
        /// <param name="content">コンテンツ(ボタン名称)</param>
        public DataGridColumnInfo(int columnIndex, DataGridColumnType type, DataGridViewContentAlignment alingn, int width, string header, string content)
        {
            this.ColumnIndex = columnIndex;
            this.ColumnType = type;
            this.Alignment = alingn;
            this.Width = width;
            this.HeaderName = header;
            this.ContentName = content;
        }

        /// <summary>
        /// 列インデックス
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// 列の種別
        /// </summary>
        public DataGridColumnType ColumnType { get; set; }

        /// <summary>
        /// アライメント
        /// </summary>
        public DataGridViewContentAlignment Alignment { get; set; }

        /// <summary>
        /// 幅
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// ヘッダ名
        /// </summary>
        public string HeaderName { get; set; }

        /// <summary>
        /// コンテンツ(ボタン名称)
        /// </summary>
        public string ContentName { get; set; }
    }
}
