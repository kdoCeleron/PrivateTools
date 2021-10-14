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
    /// <summary>
    /// リソース管理
    /// </summary>
    public class ResourceManager
    {
        /// <summary>
        /// タスク情報保存ファイル
        /// </summary>
        public static string TaskListSavePath = @".\taskList.json";

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        private static ResourceManager instance = new ResourceManager();

        /// <summary>
        /// タスク情報
        /// </summary>
        private TaskInfoRoot taskInfoRoot;

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        public static ResourceManager Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// メイン画面への参照
        /// </summary>
        public Form MainForm { get; set; }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>実行結果</returns>
        public bool Initialize()
        {
            this.taskInfoRoot = new TaskInfoRoot();

            KeyInfo.Initialize();

            var path = Utils.GetFullPath(TaskListSavePath);
            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                var jsonObj = JsonConvert.DeserializeObject<TaskInfoRoot>(text);
                
                instance.taskInfoRoot = jsonObj;

                var rootGroupKey = TaskGroupInfo.GetRootGroup().Key;
                if (instance.taskInfoRoot.TaskGroupListJsonObj.Any(x => x.Key.Equals(rootGroupKey)))
                {
                    var item = instance.taskInfoRoot.TaskGroupListJsonObj.First(x => x.Key.Equals(rootGroupKey));
                    TaskGroupInfo.OverriteRootGroup(item);
                }

                var defaultGroupKey = TaskGroupInfo.GetDefaultGroup().Key;
                if (instance.taskInfoRoot.TaskGroupListJsonObj.Any(x => x.Key.Equals(defaultGroupKey)))
                {
                    var item = instance.taskInfoRoot.TaskGroupListJsonObj.First(x => x.Key.Equals(defaultGroupKey));
                    TaskGroupInfo.OverriteDefaultGroup(item);
                }

                // 全グループに対してキーの再生成
                foreach (var taskGroupInfo in instance.taskInfoRoot.TaskGroupListJsonObj)
                {
                    if (!KeyInfo.IsCreatedKeyGroup(taskGroupInfo.Key))
                    {
                        var keyGroup = KeyInfo.CreateKeyInfoGroup();

                        taskGroupInfo.ChildGroups.Clear();
                        var filetered =
                            instance.taskInfoRoot.TaskGroupListJsonObj.Where(x =>
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

                    if (!instance.taskInfoRoot.TaskGroupList.ContainsKey(taskGroupInfo.Key))
                    {
                        instance.taskInfoRoot.TaskGroupList.Add(taskGroupInfo.Key, taskGroupInfo);
                    }
                }
            }
            else
            {
                instance.taskInfoRoot.AddTaskGroup(TaskGroupInfo.GetRootGroup(), null);
                instance.taskInfoRoot.AddTaskGroup(TaskGroupInfo.GetDefaultGroup(), TaskGroupInfo.GetRootGroup());
            }
            
            return true;
        }

        /// <summary>
        /// 全タスク情報を保存します。
        /// </summary>
        public void SaveTaskList()
        {
            foreach (var taskGroupInfo in instance.taskInfoRoot.TaskGroupListJsonObj)
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

            var jsonStr = JsonConvert.SerializeObject(ResourceManager.Instance.taskInfoRoot, Formatting.Indented);
            File.WriteAllText(Utils.GetFullPath(TaskListSavePath), jsonStr);
        }

        /// <summary>
        /// ルートのグループ情報を取得します。
        /// </summary>
        /// <returns>取得けっｋ</returns>
        public List<TaskGroupInfo> GetRootGroups()
        {
            var result = new List<TaskGroupInfo>();
            foreach (var item in instance.taskInfoRoot.TaskGroupList)
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

        /// <summary>
        /// 全タスクの情報を取得します。
        /// </summary>
        /// <returns>取得けっｋ</returns>
        public List<TaskItem> GetAllTaskItems()
        {
            var list = new List<TaskItem>();
            ResourceManager.Instance.ExecInnerGroupAndTasks(
                TaskGroupInfo.GetRootGroup(),
                null,
                (task) => { list.Add(task); });

            return list;
        }

        /// <summary>
        /// グループを追加します。
        /// </summary>
        /// <param name="name">グループ名称</param>
        /// <param name="parent">親グループ</param>
        /// <returns>追加結果</returns>
        public TaskGroupInfo AddTaskGroup(string name, TaskGroupInfo parent)
        {
            return instance.taskInfoRoot.AddTaskGroup(name, parent);
        }

        /// <summary>
        /// グループ情報を編集します。
        /// </summary>
        /// <param name="original">元のグループ情報</param>
        /// <param name="editName">編集先名称</param>
        /// <param name="editParent">編集先親グループ</param>
        /// <returns>編集結果</returns>
        public TaskGroupInfo EditTaskGroup(TaskGroupInfo original, string editName = null, TaskGroupInfo editParent = null)
        {
            return instance.taskInfoRoot.EditTaskGroup(original, editName, editParent);
        }

        /// <summary>
        /// グループ情報を削除します。
        /// </summary>
        /// <param name="taskGroupInfo">削除対象のグループ情報</param>
        public void RemoveTaskGroup(TaskGroupInfo taskGroupInfo)
        {
            instance.taskInfoRoot.RemoveTaskGroup(taskGroupInfo);
        }

        /// <summary>
        /// タスクを追加します。
        /// </summary>
        /// <param name="group">所属グル－プ</param>
        /// <param name="taskItem">追加するタスク情報</param>
        public void AddTaskItem(KeyInfo group, TaskItem taskItem)
        {
            instance.taskInfoRoot.AddTaskItem(group, taskItem);
        }

        /// <summary>
        /// タスクを削除します。
        /// </summary>
        /// <param name="taskItem">削除するタスク情報</param>
        public void RemoveTaskItem(TaskItem taskItem)
        {
            instance.taskInfoRoot.RemoveTaskItem(taskItem);
        }

        /// <summary>
        /// キーに一致するグループ情報を取得します。
        /// </summary>
        /// <param name="groupKey">キー</param>
        /// <returns>取得結果</returns>
        public TaskGroupInfo GetGroupInfo(KeyInfo groupKey)
        {
            if (instance.taskInfoRoot.TaskGroupList.ContainsKey(groupKey))
            {
                return instance.taskInfoRoot.TaskGroupList[groupKey];
            }

            return null;
        }

        /// <summary>
        /// キ－に一致するグループ名称を取得します。
        /// </summary>
        /// <param name="groupKey">キー</param>
        /// <returns>取得けっｋ</returns>
        public string GetGroupName(KeyInfo groupKey)
        {
            var group = instance.GetGroupInfo(groupKey);
            if (group != null)
            {
                return group.Name;
            }

            return string.Empty;
        }

        /// <summary>
        /// 全グループの情報を取得します。
        /// </summary>
        /// <returns>取得結果</returns>
        public List<TaskGroupInfo> GetGroupList()
        {
            return ResourceManager.Instance.taskInfoRoot.TaskGroupList                             
                .Select(x => x.Value)
                .ToList();
        }

        /// <summary>
        /// ルートグループ以外の全グループを取得します。
        /// </summary>
        /// <returns>取得結果</returns>
        public List<TaskGroupInfo> GetGroupListExcludeRoot()
        {
            return ResourceManager.Instance.taskInfoRoot.TaskGroupList
                .Where(x => !x.Key.Equals(TaskGroupInfo.GetRootGroup().Key))
                .Select(x => x.Value)
                .ToList();
        }
        
        /// <summary>
        /// 管理内のグループおよびタスクに対して指定の処理を実行します。
        /// </summary>
        /// <param name="rootGroup">探索のルートグループ</param>
        /// <param name="groupAction">グループに対して実行する処理</param>
        /// <param name="taskAction">タスクに対して実行する処理</param>
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
                var group = instance.taskInfoRoot.TaskGroupList[childGroup];
                this.ExecInnerGroupAndTasks(group, groupAction, taskAction);

                if (groupAction != null)
                {
                    groupAction(group);
                }
            }
        }
    }
}
