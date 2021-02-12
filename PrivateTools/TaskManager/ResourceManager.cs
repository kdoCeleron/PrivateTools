using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using TaskManager.Data;

namespace TaskManager
{
    public class ResourceManager
    {
        private bool isInitialized = false;

        private static ResourceManager instance = new ResourceManager();

        public static ResourceManager Instance
        {
            get
            {
                return instance;
            }
        }

        public TaskInfoRoot TaskInfoRoot;

        public bool Initialize()
        {
            this.TaskInfoRoot = new TaskInfoRoot();

            KeyInfo.Initialize();

            //var text = File.ReadAllText(".\tasks.txt");
            //var jsonObj = JsonConvert.DeserializeObject<List<TaskGroupInfo>>(text);
            //this.TaskGroupList.AddRange(jsonObj);
            //this.TaskInfoRoot.AddTaskGroup(TaskGroupInfo.GetRootGroup().Name, null);
            this.TaskInfoRoot.AddTaskGroup(TaskGroupInfo.GetDefaultGroup().Name, TaskGroupInfo.GetRootGroup());

            this.isInitialized = true;

            return true;
        }
    }
}
