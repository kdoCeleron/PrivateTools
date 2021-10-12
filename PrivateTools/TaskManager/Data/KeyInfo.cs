namespace TaskManager.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// タスクおよびグループを特定するキー情報
    /// </summary>
    [JsonObject]
    public class KeyInfo
    {
        /// <summary>
        /// 未使用の全キー情報(グループ)
        /// </summary>
        private static HashSet<string> groupAllKeys;

        /// <summary>
        /// 未使用の全キー情報(タスク)
        /// </summary>
        private static Dictionary<string, HashSet<string>> taskAllKeys;
        
        /// <summary>
        /// キーの種別
        /// </summary>
        private KeyType keyType;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private KeyInfo()
        {
            this.keyType = KeyType.None;
            this.KeyTask = string.Empty;
            this.KeyGroup = string.Empty;
        }

        /// <summary>
        /// キーの種別
        /// </summary>
        private enum KeyType
        {
            /// <summary>
            /// なし
            /// </summary>
            None,

            /// <summary>
            /// グループ
            /// </summary>
            Group,

            /// <summary>
            /// タスク
            /// </summary>
            Task
        }

        /// <summary>
        /// グループのキー
        /// </summary>
        [JsonProperty("KeyGroup")]
        public string KeyGroup { get; set; }

        /// <summary>
        /// タスクのキー
        /// </summary>
        [JsonProperty("KeyTask")]
        public string KeyTask { get; set; }
        
        /// <summary>
        /// グループとタスクを統合したキー
        /// </summary>
        public string Key
        {
            get
            {
                return this.KeyGroup + "_" + this.KeyTask;
            }
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="groupKeys">生成済みとするするグループのキー</param>
        /// <param name="taskKeys">生成済みとするタスクのキー</param>
        public static void Initialize(List<KeyInfo> groupKeys = null, List<KeyInfo> taskKeys = null)
        {
            groupAllKeys = new HashSet<string>();
            taskAllKeys = new Dictionary<string, HashSet<string>>();

            var maxKeyNum = 100;

            for (int i = 0; i < maxKeyNum; i++)
            {
                var groupKey = string.Format("Group_{0:D5}", i);
                groupAllKeys.Add(groupKey);
                taskAllKeys.Add(groupKey, new HashSet<string>());
                for (int j = 0; j < maxKeyNum; j++)
                {
                    var taskKey = string.Format("Task_{0:D5}", j);
                    taskAllKeys[groupKey].Add(taskKey);
                }
            }

            if (groupKeys != null)
            {
                foreach (var groupKey in groupKeys)
                {
                    if (groupAllKeys.Contains(groupKey.KeyGroup))
                    {
                        groupAllKeys.Remove(groupKey.KeyGroup);
                    }
                }
            }

            if (taskKeys != null)
            {
                foreach (var taskKey in taskKeys)
                {
                    if (taskAllKeys.ContainsKey(taskKey.KeyGroup))
                    {
                        if (taskAllKeys[taskKey.KeyGroup].Contains(taskKey.KeyTask))
                        {
                            taskAllKeys[taskKey.KeyGroup].Remove(taskKey.KeyTask);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// グループのキー情報を生成する。
        /// </summary>
        /// <returns>生成結果</returns>
        public static KeyInfo CreateKeyInfoGroup()
        {
            if (groupAllKeys.Any())
            {
                var info = new KeyInfo();
                info.keyType = KeyType.Group;

                var key = groupAllKeys.First();

                info.KeyGroup = key;

                groupAllKeys.Remove(key);

                return info;
            }

            // key枯渇
            return null;
        }

        /// <summary>
        /// グループのキー情報を生成済みかチェックする。
        /// </summary>
        /// <param name="keyInfo">キー情報</param>
        /// <returns>true: 生成済み/false: 未生成</returns>
        public static bool IsCreatedKeyGroup(KeyInfo keyInfo)
        {
            return !groupAllKeys.Contains(keyInfo.KeyGroup);
        }

        /// <summary>
        /// タスクのキー情報を生成済みかチェックする。
        /// </summary>
        /// <param name="group">グループのキー情報</param>
        /// <param name="task">タスクのキー情報</param>
        /// <returns>true: 生成済み/false: 未生成</returns>
        public static bool IsCreatedKeyTask(KeyInfo group, KeyInfo task)
        {
            if (groupAllKeys.Contains(group.KeyGroup))
            {
                if (taskAllKeys.ContainsKey(group.KeyTask))
                {
                    if (taskAllKeys[group.Key].Contains(task.KeyTask))
                    {
                        // 未生成のキー
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// タスクのキーを生成する。
        /// </summary>
        /// <param name="parentGroupKey">親グループのキー情報</param>
        /// <returns>生成結果</returns>
        public static KeyInfo CreateKeyInfoTask(KeyInfo parentGroupKey)
        {
            if (parentGroupKey != null)
            {
                var parentKey = parentGroupKey.KeyGroup;
                if (taskAllKeys[parentKey].Any())
                {
                    var info = new KeyInfo();
                    info.keyType = KeyType.Task;

                    var key = taskAllKeys[parentKey].First();

                    info.KeyGroup = parentKey;
                    info.KeyTask = key;

                    taskAllKeys[parentKey].Remove(key);

                    return info;
                }
            } 
            
            // key枯渇
            return null;
        }

        /// <summary>
        /// キー情報を削除します。
        /// </summary>
        /// <param name="keyInfo">キー情報</param>
        public static void DeleteKeyInfo(KeyInfo keyInfo)
        {
            if (keyInfo == null)
            {
                return;
            }

            // キーを再利用
            if (keyInfo.keyType == KeyType.Group)
            {
                groupAllKeys.Add(keyInfo.KeyGroup);
            }
            else if (keyInfo.keyType == KeyType.Task)
            {
                taskAllKeys[keyInfo.KeyGroup].Add(keyInfo.KeyTask);
            }
        }

        /// <summary>
        /// ハッシュコードを取得します。
        /// </summary>
        /// <returns>ハッシュコード</returns>
        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }

        /// <summary>
        /// オブジェクトの同一性検証
        /// </summary>
        /// <param name="obj">検証対象</param>
        /// <returns>true:同一/ false:異なる</returns>
        public override bool Equals(object obj)
        {
            if (obj is KeyInfo)
            {
                return this.Key == ((KeyInfo)obj).Key;
            }

            return false;
        }
    }
}
