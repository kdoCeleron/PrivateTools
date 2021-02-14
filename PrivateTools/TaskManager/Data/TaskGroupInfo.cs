using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TaskManager.Data
{
    [JsonObject]
    public class TaskGroupInfo
    {
        public TaskGroupInfo()
        {
            this.Name = string.Empty;
            this.ChildGroups = new HashSet<KeyInfo>();
            this.ChildTaskItems = new HashSet<TaskItem>();
        }

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

        private static TaskGroupInfo _defaultGroup;
        private static TaskGroupInfo _rootGroup;

        [JsonProperty("Key")]
        public KeyInfo Key { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("ParentGroup")]
        public KeyInfo ParentGroup { get; set; }

        [JsonProperty("ChildGroups")]
        public HashSet<KeyInfo> ChildGroups { get; set; }

        [JsonProperty("ChildTaskItems")]
        public HashSet<TaskItem> ChildTaskItems { get; set; }

        public static void SetDefaultGroup(TaskGroupInfo value)
        {
            _defaultGroup = value;
        }

        public static void SetRootGroup(TaskGroupInfo value)
        {
            _rootGroup = value;
        }

        public static TaskGroupInfo GetDefaultGroup()
        {
            return _defaultGroup;
        }

        public static TaskGroupInfo GetRootGroup()
        {
            return _rootGroup;
        }
    }
}
