using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Data
{
    public class TaskItem
    {
        public TaskItem()
        {
            // this.Group = TaskGroupInfo.GetDefaultGroup();

            this.Title = string.Empty;
            this.DateTimeLimit = DateTime.Now;
            this.Memo = string.Empty;

            this.AttachFileOrg = new List<string>();
            this.AttachFile = new List<string>();
            this.ChildTasks = new List<TaskItem>();
        }

        public TaskGroupInfo Group { get; set; }

        public string Title { get; set; }

        public DateTime DateTimeLimit { get; set; }

        public string Memo { get; set; }

        public List<string> AttachFileOrg { get; set; }

        public List<string> AttachFile { get; set; }

        // TODO:追々。。
        public TaskItem ParentTask { get; set; }

        public List<TaskItem> ChildTasks { get; set; }
    }
}
