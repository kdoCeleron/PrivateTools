using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TaskManager.Data
{
    [JsonObject]
    public class TaskInfoRoot
    {
        public TaskInfoRoot()
        {
            this.TaskGroupInfos = new List<TaskGroupInfo>();
        }

        [JsonProperty("TaskGroupInfos")]
        public List<TaskGroupInfo> TaskGroupInfos { get; set; }
    }
}
