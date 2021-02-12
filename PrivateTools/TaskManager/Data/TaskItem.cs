using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TaskManager.Data
{
    [JsonObject]
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
        }

        [JsonProperty("Group")]
        public TaskGroupInfo Group { get; set; }

        public bool IsComeplate { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("DateTimeLimit")]
        public DateTime DateTimeLimit { get; set; }

        [JsonProperty("Memo")]
        public string Memo { get; set; }

        [JsonProperty("AttachFileOrg")]
        public List<string> AttachFileOrg { get; set; }

        [JsonProperty("AttachFile")]
        public List<string> AttachFile { get; set; }

        // TODO:追々。。
        //public TaskItem ParentTask { get; set; }

        //public List<TaskItem> ChildTasks { get; set; }
    }
}
