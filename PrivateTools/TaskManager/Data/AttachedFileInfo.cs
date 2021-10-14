using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Data
{
    /// <summary>
    /// 添付ファイル管理クラス
    /// </summary>
    public class AttachedFileInfo
    {
        /// <summary>
        /// ファイルパス(ツール管理内)
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 添付時の元ファイルパス
        /// </summary>
        public string OrgFilePath { get; set; }
    }
}
