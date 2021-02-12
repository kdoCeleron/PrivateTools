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
            this.ChildGroups = new List<TaskGroupInfo>();
            this.ChildTaskItems = new List<TaskItem>();
        }

        private static TaskGroupInfo _defaultGroup;
        private static TaskGroupInfo _rootGroup;

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("ParentGroup")]
        public TaskGroupInfo ParentGroup { get; set; }

        [JsonProperty("ChildGroups")]
        public List<TaskGroupInfo> ChildGroups { get; set; }

        [JsonProperty("ChildTaskItems")]
        public List<TaskItem> ChildTaskItems { get; set; }

        public static TaskGroupInfo GetDefaultGroup()
        {
            if (_defaultGroup == null)
            {
                _defaultGroup = new TaskGroupInfo();
                _defaultGroup.Name = "未分類";
                _defaultGroup.ChildTaskItems = new List<TaskItem>();
                _defaultGroup.ChildGroups = new List<TaskGroupInfo>();
                _defaultGroup.ParentGroup = GetRootGroup();
            }

            return _defaultGroup;
        }

        public static TaskGroupInfo GetRootGroup()
        {
            if (_rootGroup == null)
            {
                _rootGroup = new TaskGroupInfo();
                _rootGroup.Name = string.Empty;
                _rootGroup.ChildTaskItems = new List<TaskItem>();

                _rootGroup.ChildGroups = new List<TaskGroupInfo>();
                _rootGroup.ChildGroups.Add(GetDefaultGroup());
            }

            return _rootGroup;
        }
    }
}
