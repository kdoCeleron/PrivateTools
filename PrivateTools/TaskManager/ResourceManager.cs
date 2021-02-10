using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public List<TaskGroupInfo> TaskGroupList;

        public bool Initialize()
        {
            this.TaskGroupList = new List<TaskGroupInfo>();
            this.TaskGroupList.Add(TaskGroupInfo.GetDefaultGroup());

            this.isInitialized = true;

            return true;
        }

        public TaskGroupInfo AddTaskGroup(string name, TaskGroupInfo parent = null)
        {
            var group = new TaskGroupInfo();
            group.Name = name;
            group.ParentGroup = parent;

            var groups = instance.TaskGroupList.Where(x => x.ParentGroup == parent);
            if (groups.Any(x => x.Name == name))
            {
                // 同名称のグループあり
                return null;
            }

            instance.TaskGroupList.Add(group);
            if (parent != null)
            {
                parent.ChildGroups.Add(group);
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
                    original.ParentGroup = editParent;
                    foreach (var taskGroupInfo in instance.TaskGroupList)
                    {
                        // 元の親情報を削除
                        if (taskGroupInfo.ChildGroups.Contains(original))
                        {
                            taskGroupInfo.ChildGroups.Remove(original);
                        }
                    }

                    // 新しい親に設定
                    editParent.ChildGroups.Add(original);
                }
            }

            return original;
        }

        public void RemoveTaskGroup(TaskGroupInfo taskGroupInfo)
        {
            if (instance.TaskGroupList.Contains(taskGroupInfo))
            {
                instance.TaskGroupList.Remove(taskGroupInfo);
            }

            // ほかの参照も除去
            foreach (var groupInfo in instance.TaskGroupList)
            {
                if (groupInfo.ParentGroup != null)
                {
                    if (groupInfo.ParentGroup == taskGroupInfo)
                    {
                        groupInfo.ParentGroup = null;
                    }
                }

                if (groupInfo.ChildGroups.Contains(taskGroupInfo))
                {
                    groupInfo.ChildGroups.Remove(taskGroupInfo);
                }
            }
        }

        public void AddTaskItem(TaskItem taskItem)
        {
            var group = instance.TaskGroupList.FirstOrDefault(x => x == taskItem.Group);
            if (group != null)
            {
                group.ChildTaskItems.Add(taskItem);
            }

        }
    }
}
