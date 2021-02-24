using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TaskManager.Data
{
    /// <summary>
    /// グループ情報
    /// </summary>
    [JsonObject]
    public class TaskGroupInfo
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskGroupInfo()
        {
            this.Name = string.Empty;
            this.ChildGroups = new HashSet<KeyInfo>();
            this.ChildTaskItems = new HashSet<TaskItem>();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static TaskGroupInfo()
        {
            _rootGroup = new TaskGroupInfo();
            _rootGroup.Name = string.Empty;
            _rootGroup.ChildTaskItems = new HashSet<TaskItem>();
            _rootGroup.ChildGroups = new HashSet<KeyInfo>();
            _rootGroup.Key = KeyInfo.CreateKeyInfoGroup();

            _defaultGroup = new TaskGroupInfo();
            _defaultGroup.Name = "未分類";
            _defaultGroup.ChildTaskItems = new HashSet<TaskItem>();
            _defaultGroup.ChildGroups = new HashSet<KeyInfo>();
            _defaultGroup.Key = KeyInfo.CreateKeyInfoGroup();

            _rootGroup.ChildGroups.Add(_defaultGroup.Key);
            _defaultGroup.ParentGroup = _rootGroup.Key;
        }

        /// <summary>
        /// デフォルト(未分類)グループ
        /// </summary>
        private static TaskGroupInfo _defaultGroup;

        /// <summary>
        /// ルートのグループ
        /// </summary>
        private static TaskGroupInfo _rootGroup;

        /// <summary>
        /// グループのキー情報
        /// </summary>
        [JsonProperty("Key")]
        public KeyInfo Key { get; set; }

        /// <summary>
        /// グループの名称
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 親グループ
        /// </summary>
        [JsonProperty("ParentGroup")]
        public KeyInfo ParentGroup { get; set; }

        /// <summary>
        /// 子グループ
        /// </summary>
        [JsonProperty("ChildGroups")]
        public HashSet<KeyInfo> ChildGroups { get; set; }

        /// <summary>
        /// 子タスク
        /// </summary>
        [JsonProperty("ChildTaskItems")]
        public HashSet<TaskItem> ChildTaskItems { get; set; }

        /// <summary>
        /// デフォルトのグループ情報を更新します。
        /// </summary>
        /// <param name="value">更新値</param>
        public static void OverriteDefaultGroup(TaskGroupInfo value)
        {
            _defaultGroup = value;
        }

        /// <summary>
        /// ルートのグループ情報を更新します。
        /// </summary>
        /// <param name="value">更新値</param>
        public static void OverriteRootGroup(TaskGroupInfo value)
        {
            _rootGroup = value;
        }

        /// <summary>
        /// デフォルトグループの情報を取得します。
        /// </summary>
        /// <returns>取得結果</returns>
        public static TaskGroupInfo GetDefaultGroup()
        {
            return _defaultGroup;
        }

        /// <summary>
        /// ルートグループの情報を取得します。
        /// </summary>
        /// <returns>取得結果</returns>
        public static TaskGroupInfo GetRootGroup()
        {
            return _rootGroup;
        }
    }
}
