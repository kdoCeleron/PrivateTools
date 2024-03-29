﻿namespace TaskManager.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// タスク情報の管理クラス
    /// </summary>
    [JsonObject]
    public class TaskItem
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskItem()
        {
            //// this.Group = TaskGroupInfo.GetDefaultGroup();

            this.Title = string.Empty;
            this.DateTimeLimit = DateTime.Now;
            this.Memo = string.Empty;
            
            this.AttachFile = new List<AttachedFileInfo>();
        }

        /// <summary>
        /// キー情報
        /// </summary>
        [JsonProperty("Key")]
        public KeyInfo Key { get; set; }
        
        /// <summary>
        /// 親グループ情報
        /// </summary>
        [JsonProperty("Group")]
        public KeyInfo Group { get; set; }

        /// <summary>
        /// 完了済みタスクかどうか
        /// </summary>
        public bool IsComeplate { get; set; }

        /// <summary>
        /// タイトル
        /// </summary>
        [JsonProperty("Title")]
        public string Title { get; set; }

        /// <summary>
        /// 期限
        /// </summary>
        [JsonProperty("DateTimeLimit")]
        public DateTime DateTimeLimit { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        [JsonProperty("Memo")]
        public string Memo { get; set; }
        
        /// <summary>
        /// 添付ファイル
        /// </summary>
        [JsonProperty("AttachFile")]
        public List<AttachedFileInfo> AttachFile { get; set; }
        
        /// <summary>
        /// 親グループの情報を取得する。
        /// </summary>
        /// <returns>取得結果</returns>
        public TaskGroupInfo GetGroup()
        {
            return ResourceManager.Instance.GetGroupInfo(this.Group);
        }

        /// <summary>
        /// タスクの情報を示すテキストを取得します。
        /// </summary>
        /// <param name="delimiter">項目ごとのセパレータ</param>
        /// <returns>取得結果</returns>
        public string GetInfoText(string delimiter = "\t")
        {
            var ret = string.Empty;
            var items = this.GetItemStrs();
            for (int i = 0; i < items.Count; i++)
            {
                ret += items[i];
                if (i != items.Count - 1)
                {
                    ret += delimiter;
                }
            }
            
            return ret;
        }

        /// <summary>
        /// タスクの内容を示す文字列を取得します。
        /// </summary>
        /// <returns>取得結果</returns>
        public List<string> GetItemStrs()
        {
            var ret = new List<string>();
            if (this.Group != null)
            {
                ret.Add(ResourceManager.Instance.GetGroupName(this.Group));
            }

            ret.Add(this.Title);
            ret.Add(this.DateTimeLimit.ToString());
            ret.Add(this.Memo);

            return ret;
        }
    }
}
