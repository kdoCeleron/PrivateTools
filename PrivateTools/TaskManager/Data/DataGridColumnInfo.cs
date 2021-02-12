using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManager.Data
{
    public class DataGridColumnInfo
    {
        public DataGridColumnInfo(int columnIndex, DataGridColumnType type, DataGridViewContentAlignment alingn, int width, string header, string content)
        {
            this.ColumnIndex = columnIndex;
            this.ColumnType = type;
            this.Alignment = alingn;
            this.Width = width;
            this.HeaderName = header;
            this.ContentName = content;
        }

        public int ColumnIndex { get; set; }

        public DataGridColumnType ColumnType { get; set; }

        public DataGridViewContentAlignment Alignment { get; set; }

        public int Width { get; set; }

        public string HeaderName { get; set; }

        public string ContentName { get; set; }
    }
}
