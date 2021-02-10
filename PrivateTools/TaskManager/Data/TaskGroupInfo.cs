using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Data
{
    public class TaskGroupInfo
    {
        public TaskGroupInfo()
        {
            this.Name = string.Empty;
            this.ChildGroups = new List<TaskGroupInfo>();
            this.ChildTaskItems = new List<TaskItem>();
        }

        private static TaskGroupInfo _defaultGroup;

        public string Name { get; set; }

        public TaskGroupInfo ParentGroup { get; set; }

        public List<TaskGroupInfo> ChildGroups { get; set; }

        public List<TaskItem> ChildTaskItems { get; set; }

        public static TaskGroupInfo GetDefaultGroup()
        {
            if (_defaultGroup == null)
            {
                _defaultGroup = new TaskGroupInfo();
                _defaultGroup.Name = "未分類";
                _defaultGroup.ChildTaskItems = new List<TaskItem>();
            }

            return _defaultGroup;
        }
    }
}
