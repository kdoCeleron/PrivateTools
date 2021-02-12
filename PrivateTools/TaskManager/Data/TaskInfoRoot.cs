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
            this.TaskGroupList = new Dictionary<KeyInfo, TaskGroupInfo>();
        }

        [JsonProperty("TaskGroupInfos")]
        public Dictionary<KeyInfo, TaskGroupInfo> TaskGroupList { get; set; }

        public TaskGroupInfo AddTaskGroup(string name, TaskGroupInfo parent)
        {
            var group = new TaskGroupInfo();
            group.Name = name;

            group.Key = KeyInfo.CreateKeyInfoGroup();
            this.TaskGroupList.Add(group.Key, group);
            if (parent != null)
            {
                group.ParentGroup = parent.Key;
                parent.ChildGroups.Add(group.Key);
            }

            return group;
        }

        public TaskGroupInfo EditTaskGroup(TaskGroupInfo original, string editName = null, TaskGroupInfo editParent = null)
        {
            if (original != null)
            {
                if (editName != null)
                {
                    original.Name = editName;
                }

                if (editParent != null)
                {
                    original.ParentGroup = editParent.Key;
                    foreach (var taskGroupInfo in this.TaskGroupList)
                    {
                        // 元の親情報を削除
                        if (taskGroupInfo.Value.ChildGroups.Contains(original.Key))
                        {
                            taskGroupInfo.Value.ChildGroups.Remove(original.Key);
                        }
                    }

                    // 新しい親に設定
                    editParent.ChildGroups.Add(original.Key);
                }
            }

            return original;
        }

        public void RemoveTaskGroup(TaskGroupInfo taskGroupInfo)
        {
            if (this.TaskGroupList.ContainsKey(taskGroupInfo.Key))
            {
                this.TaskGroupList.Remove(taskGroupInfo.Key);
            }

            // ほかの参照も除去
            foreach (var groupInfo in this.TaskGroupList)
            {
                if (groupInfo.Value.ParentGroup != null)
                {
                    if (groupInfo.Value.ParentGroup.Equals(taskGroupInfo.Key))
                    {
                        groupInfo.Value.ParentGroup = null;
                    }
                }

                if (groupInfo.Value.ChildGroups.Contains(taskGroupInfo.Key))
                {
                    groupInfo.Value.ChildGroups.Remove(taskGroupInfo.Key);
                }
            }

            // 配下のグループ/タスクのキー除去
            foreach (var childGroup in taskGroupInfo.ChildGroups)
            {
                KeyInfo.DeleteKeyInfo(childGroup);
            }

            foreach (var childTaskItem in taskGroupInfo.ChildTaskItems)
            {
                KeyInfo.DeleteKeyInfo(childTaskItem.Key);
            }
        }

        public void AddTaskItem(KeyInfo group, TaskItem taskItem)
        {
            this.TaskGroupList[group].ChildTaskItems.Add(taskItem);
        }

        public void RemoveTaskItem(TaskItem taskItem)
        {
            var taskItems = this.TaskGroupList[taskItem.Group].ChildTaskItems;
            if (taskItems.Contains(taskItem))
            {
                taskItems.Remove(taskItem);
            }
        }
    }
}
