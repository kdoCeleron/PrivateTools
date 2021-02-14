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

        public Form MainForm { get; set; }

        public TaskInfoRoot TaskInfoRoot;

        public bool Initialize()
        {
            this.TaskInfoRoot = new TaskInfoRoot();

            KeyInfo.Initialize();

            var path = @".\tasks.txt";
            if (File.Exists(path))
            {
                var text = File.ReadAllText(@".\tasks.txt");
                var jsonObj = JsonConvert.DeserializeObject<TaskInfoRoot>(text);
                foreach (var info in jsonObj.TaskGroupListJsonObj)
                {
                    jsonObj.TaskGroupList.Add(info.Key, info);
                }

                instance.TaskInfoRoot = jsonObj;
            }
            else
            {
                instance.TaskInfoRoot.AddTaskGroup(TaskGroupInfo.GetRootGroup(), null);
                instance.TaskInfoRoot.AddTaskGroup(TaskGroupInfo.GetDefaultGroup(), TaskGroupInfo.GetRootGroup());
            }

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
        
        public void ExecInnerGroupAndTasks(TaskGroupInfo rootGroup, Action<TaskGroupInfo> groupAction, Action<TaskItem> taskAction)
        {
            if (rootGroup == null)
            {
                return;
            }

            foreach (var taskItem in rootGroup.ChildTaskItems)
            {
                if (taskAction != null)
                {
                    taskAction(taskItem);
                }
            }

            foreach (var childGroup in rootGroup.ChildGroups)
            {
                var group = instance.TaskInfoRoot.TaskGroupList[childGroup];
                this.ExecInnerGroupAndTasks(group, groupAction, taskAction);

                if (groupAction != null)
                {
                    groupAction(group);
                }
            }
        }
    }
}
