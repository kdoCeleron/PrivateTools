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

        [JsonProperty("Key")]
        public KeyInfo Key { get; set; }
        
        [JsonProperty("Group")]
        public KeyInfo Group { get; set; }

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

        public TaskGroupInfo GetGroup()
        {
            return ResourceManager.Instance.TaskInfoRoot.TaskGroupList[this.Group];
        }

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
