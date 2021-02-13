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

            // var text = File.ReadAllText(@".\tasks.txt");
            // var jsonObj = JsonConvert.DeserializeObject<TaskInfoRoot>(text);
            //this.TaskGroupList.AddRange(jsonObj);
            this.TaskInfoRoot.AddTaskGroup(TaskGroupInfo.GetRootGroup());
            this.TaskInfoRoot.AddTaskGroup(TaskGroupInfo.GetDefaultGroup());

            //TaskInfoRoot = jsonObj;

            this.isInitialized = true;

            return true;
        }

        public List<TaskGroupInfo> GetRootGroups()
        {
            var result = new List<TaskGroupInfo>();
            foreach (var item in instance.TaskInfoRoot.TaskGroupList)
            {
                var key = item.Key;
                var value = item.Value;

                if (value.ParentGroup != null)
                {
                    if (value.ParentGroup.Equals(TaskGroupInfo.GetRootGroup().Key))
                    {
                        result.Add(value);
                    }
                }
            }

            return result;
        }

        public TaskGroupInfo AddTaskGroup(string name, TaskGroupInfo parent)
        {
            return instance.TaskInfoRoot.AddTaskGroup(name, parent);
        }

        public TaskGroupInfo EditTaskGroup(TaskGroupInfo original, string editName = null, TaskGroupInfo editParent = null)
        {
            return instance.TaskInfoRoot.EditTaskGroup(original, editName, editParent);
        }

        public void RemoveTaskGroup(TaskGroupInfo taskGroupInfo)
        {
            instance.TaskInfoRoot.RemoveTaskGroup(taskGroupInfo);
        }

        public void AddTaskItem(KeyInfo group, TaskItem taskItem)
        {
            instance.TaskInfoRoot.AddTaskItem(group, taskItem);
        }

        public void RemoveTaskItem(TaskItem taskItem)
        {
            instance.TaskInfoRoot.RemoveTaskItem(taskItem);
        }
    }
}
