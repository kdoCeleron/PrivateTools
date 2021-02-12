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

        public static TaskGroupInfo GetDefaultGroup()
        {
            if (_defaultGroup == null)
            {
                _defaultGroup = new TaskGroupInfo();
                _defaultGroup.Name = "未分類";
                _defaultGroup.ChildTaskItems = new HashSet<TaskItem>();
                _defaultGroup.ChildGroups = new HashSet<KeyInfo>();
                _defaultGroup.ParentGroup = GetRootGroup().Key;
                _defaultGroup.Key = KeyInfo.CreateKeyInfoGroup();
            }

            return _defaultGroup;
        }

        public static TaskGroupInfo GetRootGroup()
        {
            if (_rootGroup == null)
            {
                _rootGroup = new TaskGroupInfo();
                _rootGroup.Name = string.Empty;
                _rootGroup.ChildTaskItems = new HashSet<TaskItem>();
                _rootGroup.ChildGroups = new HashSet<KeyInfo>();
                _rootGroup.ChildGroups.Add(GetDefaultGroup().Key);
                _rootGroup.Key = KeyInfo.CreateKeyInfoGroup();
            }

            return _rootGroup;
        }
    }
}
