using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TaskManager.Data
{
    /// <summary>
    /// 添付ファイル管理クラス
    /// </summary>
    [JsonObject]
    public class AttachedFileInfo
    {
        /// <summary>
        /// ファイルパス(ツール管理内)
        /// </summary>
        [JsonProperty("FilePath")]
        public string FilePath { get; set; }

        /// <summary>
        /// 添付時の元ファイルパス
        /// </summary>
        [JsonProperty("OrgFilePath")]
        public string OrgFilePath { get; set; }

        /// <summary>
        /// 表示ファイル名
        /// </summary>
        [JsonIgnore]
        public string DisplayName
        {
            get
            {
                // コピー後のパスから取得したファイル名を表示名とするが、
                // 未添付状態の場合を考慮し、添付元ファイルからも取得する。
                var fileName = Path.GetFileName(this.FilePath);
                if (string.IsNullOrEmpty(fileName))
                {
                    return Path.GetFileName(this.OrgFilePath);
                }

                return fileName;
            }
        }
    }
}
