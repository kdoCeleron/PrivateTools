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

            var path = Utils.GetFullPath(taskListSavePath);
            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                var jsonObj = JsonConvert.DeserializeObject<TaskInfoRoot>(text);
                
                instance.TaskInfoRoot = jsonObj;

                var rootGroupKey = TaskGroupInfo.GetRootGroup().Key;
                if (instance.TaskInfoRoot.TaskGroupListJsonObj.Any(x => x.Key.Equals(rootGroupKey)))
                {
                    var item = instance.TaskInfoRoot.TaskGroupListJsonObj.First(x => x.Key.Equals(rootGroupKey));
                    TaskGroupInfo.OverriteRootGroup(item);
                }

                var defaultGroupKey = TaskGroupInfo.GetDefaultGroup().Key;
                if (instance.TaskInfoRoot.TaskGroupListJsonObj.Any(x => x.Key.Equals(defaultGroupKey)))
                {
                    var item = instance.TaskInfoRoot.TaskGroupListJsonObj.First(x => x.Key.Equals(defaultGroupKey));
                    TaskGroupInfo.OverriteDefaultGroup(item);
                }

                // 全グループに対してキーの再生成
                foreach (var taskGroupInfo in instance.TaskInfoRoot.TaskGroupListJsonObj)
                {
                    if (!KeyInfo.IsCreatedKeyGroup(taskGroupInfo.Key))
                    {
                        var keyGroup = KeyInfo.CreateKeyInfoGroup();

                        taskGroupInfo.ChildGroups.Clear();
                        var filetered =
                            instance.TaskInfoRoot.TaskGroupListJsonObj.Where(x =>
                                x.ParentGroup != null && x.ParentGroup.Equals(taskGroupInfo.Key)).ToList();
                        if (filetered.Any())
                        {
                            foreach (var groupInfo in filetered)
                            {
                                if (!KeyInfo.IsCreatedKeyGroup(groupInfo.Key))
                                {
                                    var keyGroupChildGroup = KeyInfo.CreateKeyInfoGroup();
                                    groupInfo.Key = keyGroupChildGroup;
                                }

                                groupInfo.ParentGroup = keyGroup;
                                taskGroupInfo.ChildGroups.Add(groupInfo.Key);
                            }
                        }

                        taskGroupInfo.Key = keyGroup;
                    }
                    
                    foreach (var childTaskItem in taskGroupInfo.ChildTaskItems)
                    {
                        if (!KeyInfo.IsCreatedKeyTask(taskGroupInfo.Key, childTaskItem.Key))
                        {
                            var keyGroup = KeyInfo.CreateKeyInfoTask(taskGroupInfo.Key);
                            childTaskItem.Key = keyGroup;
                        }

                        childTaskItem.Group = taskGroupInfo.Key;
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
            
            return true;
        }

        public void SaveTaskList()
        {
            foreach (var taskGroupInfo in instance.TaskInfoRoot.TaskGroupListJsonObj)
            {
                var removeTasks = new List<TaskItem>();
                foreach (var childTaskItem in taskGroupInfo.ChildTaskItems)
                {
                    if (childTaskItem.IsComeplate)
                    {
                        removeTasks.Add(childTaskItem);
                    }
                }

                foreach (var removeTask in removeTasks)
                {
                    taskGroupInfo.ChildTaskItems.Remove(removeTask);
                }
            }

            var jsonStr = JsonConvert.SerializeObject(ResourceManager.Instance.TaskInfoRoot, Formatting.Indented);
            File.WriteAllText(Utils.GetFullPath(taskListSavePath), jsonStr);
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

        public List<TaskItem> GetAllTaskItems()
        {
            var list = new List<TaskItem>();
            ResourceManager.Instance.ExecInnerGroupAndTasks(
                TaskGroupInfo.GetRootGroup(),
                null,
                (task) => { list.Add(task); });

            return list;
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
