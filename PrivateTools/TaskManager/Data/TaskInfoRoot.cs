using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TaskManager.Data
{
    /// <summary>
    /// タスク管理情報のルートクラスです。
    /// </summary>
    [JsonObject]
    public class TaskInfoRoot
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskInfoRoot()
        {
            this.TaskGroupList = new Dictionary<KeyInfo, TaskGroupInfo>();
            this.TaskGroupListJsonObj = new List<TaskGroupInfo>();
        }

        /// <summary>
        /// Jsonシリアライザ用のタスク情報
        /// </summary>
        [JsonProperty("TaskGroupInfos")]
        public List<TaskGroupInfo> TaskGroupListJsonObj { get; set; }

        /// <summary>
        /// グループ情報のマッピング
        /// </summary>
        [JsonIgnore]
        public Dictionary<KeyInfo, TaskGroupInfo> TaskGroupList { get; set; }

        // TODO:削除予定
        /// <summary>
        /// 新しいグループ情報を追加します。
        /// </summary>
        /// <param name="groupInfo">追加する情報</param>
        /// <param name="parent">親グループ</param>
        public void AddTaskGroup(TaskGroupInfo groupInfo, TaskGroupInfo parent)
        {
            this.TaskGroupList.Add(groupInfo.Key, groupInfo);
            this.TaskGroupListJsonObj.Add(groupInfo);

            if (parent != null)
            {
                groupInfo.ParentGroup = parent.Key;
                parent.ChildGroups.Add(groupInfo.Key);
            }
        }

        /// <summary>
        /// 新しいグループ情報を追加します。
        /// </summary>
        /// <param name="name">追加するグループの名称</param>
        /// <param name="parent">親グループ</param>
        public TaskGroupInfo AddTaskGroup(string name, TaskGroupInfo parent)
        {
            var group = new TaskGroupInfo();
            group.Name = name;

            group.Key = KeyInfo.CreateKeyInfoGroup();
            this.TaskGroupList.Add(group.Key, group);
            this.TaskGroupListJsonObj.Add(group);
            if (parent != null)
            {
                group.ParentGroup = parent.Key;
                parent.ChildGroups.Add(group.Key);
            }

            return group;
        }

        /// <summary>
        /// グループの情報を編集します。
        /// </summary>
        /// <param name="original">編集元のデータ</param>
        /// <param name="editName">編集後の名称</param>
        /// <param name="editParent">編集後の親グループ</param>
        /// <returns>編集後のデータ</returns>
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

        /// <summary>
        /// グループ情報を削除します。
        /// </summary>
        /// <param name="taskGroupInfo">削除対象のグループ</param>
        public void RemoveTaskGroup(TaskGroupInfo taskGroupInfo)
        {
            if (this.TaskGroupList.ContainsKey(taskGroupInfo.Key))
            {
                this.TaskGroupList.Remove(taskGroupInfo.Key);
            }

            if (this.TaskGroupListJsonObj.Contains(taskGroupInfo))
            {
                this.TaskGroupListJsonObj.Remove(taskGroupInfo);
            }

            // ほかの参照も除去
            foreach (var groupInfo in this.TaskGroupList)
            {
                if (groupInfo.Value.ParentGroup != null)
                {
                    if (groupInfo.Value.ParentGroup.Equals(taskGroupInfo.Key))
                    {
                        // 親無しに変更。
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

                // TODO:さらに下位も
            }

            foreach (var childTaskItem in taskGroupInfo.ChildTaskItems)
            {
                // 子のタスクは削除
                KeyInfo.DeleteKeyInfo(childTaskItem.Key);
            }
        }

        /// <summary>
        /// タスク情報を追加します。
        /// </summary>
        /// <param name="group">グループ情報</param>
        /// <param name="taskItem">タスク情報</param>
        public void AddTaskItem(KeyInfo group, TaskItem taskItem)
        {
            if (this.TaskGroupList.ContainsKey(group))
            {
                this.TaskGroupList[group].ChildTaskItems.Add(taskItem);
            }
        }

        /// <summary>
        /// タスク情報を削除します。
        /// </summary>
        /// <param name="taskItem">タスク情報</param>
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
