﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TaskManager.Data
{
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
            // this.Group = TaskGroupInfo.GetDefaultGroup();

            this.Title = string.Empty;
            this.DateTimeLimit = DateTime.Now;
            this.Memo = string.Empty;

            this.AttachFileOrg = new List<string>();
            this.AttachFile = new List<string>();
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
        /// 添付ファイル(元ファイルパス)
        /// </summary>
        [JsonProperty("AttachFileOrg")]
        public List<string> AttachFileOrg { get; set; }

        /// <summary>
        /// 添付ファイル
        /// </summary>
        [JsonProperty("AttachFile")]
        public List<string> AttachFile { get; set; }

        // TODO:追々。。
        //public TaskItem ParentTask { get; set; }

        //public List<TaskItem> ChildTasks { get; set; }

        /// <summary>
        /// 親グループの情報を取得する。
        /// </summary>
        /// <returns>取得結果</returns>
        public TaskGroupInfo GetGroup()
        {
            // TODO:別の箇所に共通関数化
            return ResourceManager.Instance.TaskInfoRoot.TaskGroupList[this.Group];
        }

        /// <summary>
        /// タスクの情報を示すテキストを取得します。
        /// </summary>
        /// <param name="delimiter">項目ごとのセパレータ</param>
        /// <returns>取得結果</returns>
        public string GetInfoText(string delimiter = "\t")
        {
            var ret = string.Empty;
            if (this.Group != null)
            {
                if (ResourceManager.Instance.TaskInfoRoot.TaskGroupList.ContainsKey(this.Group))
                {
                    ret += ResourceManager.Instance.TaskInfoRoot.TaskGroupList[this.Group].Name;
                    ret += delimiter;
                }
            }

            ret += this.Title + delimiter;
            ret += this.DateTimeLimit + delimiter;
            ret += this.Memo + delimiter;

            return ret;
        }
    }
}
