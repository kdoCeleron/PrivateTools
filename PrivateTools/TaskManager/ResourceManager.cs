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

        private static string taskListSavePath = @".\taskList.json";

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

            var path = taskListSavePath;
            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                var jsonObj = JsonConvert.DeserializeObject<TaskInfoRoot>(text);
                //foreach (var info in jsonObj.TaskGroupListJsonObj)
                //{
                //    jsonObj.TaskGroupList.Add(info.Key, info);
                //}

                instance.TaskInfoRoot = jsonObj;

                var rootGroupKey = TaskGroupInfo.GetRootGroup().Key;
                if (instance.TaskInfoRoot.TaskGroupList.ContainsKey(rootGroupKey))
                {
                    var item = instance.TaskInfoRoot.TaskGroupList[rootGroupKey];
                    TaskGroupInfo.SetRootGroup(item);
                }

                var defaultGroupKey = TaskGroupInfo.GetDefaultGroup().Key;
                if (instance.TaskInfoRoot.TaskGroupList.ContainsKey(defaultGroupKey))
                {
                    var item = instance.TaskInfoRoot.TaskGroupList[defaultGroupKey];
                    TaskGroupInfo.SetDefaultGroup(item);
                }

                foreach (var taskGroupInfo in instance.TaskInfoRoot.TaskGroupListJsonObj)
                {
                    if (!KeyInfo.IsCreatedKeyGroup(taskGroupInfo.Key))
                    {
                        var keyGroup = KeyInfo.CreateKeyInfoGroup();
                        taskGroupInfo.Key = keyGroup;
                    }

                    // TODO:サブグループのタスクも
                    foreach (var childTaskItem in taskGroupInfo.ChildTaskItems)
                    {
                        if (!KeyInfo.IsCreatedKeyTask(taskGroupInfo.Key, childTaskItem.Key))
                        {
                            var keyGroup = KeyInfo.CreateKeyInfoTask(taskGroupInfo.Key);
                            childTaskItem.Key = keyGroup;
                        }
                    }

                    if (!instance.TaskInfoRoot.TaskGroupList.ContainsKey(taskGroupInfo.Key))
                    {
                        instance.TaskInfoRoot.TaskGroupList.Add(taskGroupInfo.Key, taskGroupInfo);
                    }
                }
            }
            else
            {
                instance.TaskInfoRoot.AddTaskGroup(TaskGroupInfo.GetRootGroup(), null);
                instance.TaskInfoRoot.AddTaskGroup(TaskGroupInfo.GetDefaultGroup(), TaskGroupInfo.GetRootGroup());
            }

            this.isInitialized = true;

            return true;
        }

        public void SaveTaskList()
        {
            var jsonStr = JsonConvert.SerializeObject(ResourceManager.Instance.TaskInfoRoot, Formatting.Indented);
            File.WriteAllText(taskListSavePath, jsonStr);
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
