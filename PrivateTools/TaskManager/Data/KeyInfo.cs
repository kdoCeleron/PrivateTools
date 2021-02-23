using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TaskManager.Data
{
    [JsonObject]
    public class KeyInfo
    {
        private static HashSet<string> groupAllKeys;

        private static Dictionary<string, HashSet<string>> taskAllKeys;
        
        private KeyType keyType;

        private enum KeyType
        {
            None,

            Group,

            Task
        }

        private KeyInfo()
        {
            this.keyType = KeyType.None;
            this.KeyTask = string.Empty;
            this.KeyGroup = string.Empty;
        }

        [JsonProperty("KeyGroup")]
        public string KeyGroup { get; set; }

        [JsonProperty("KeyTask")]
        public string KeyTask { get; set; }
        
        public string Key
        {
            get
            {
                return this.KeyGroup + "_" + this.KeyTask;
            }
        }

        public static void Initialize(List<string> groupKeys = null, List<string> taskKeys = null)
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
                    groupAllKeys.Remove(groupKey);
                }
            }

            if (taskKeys != null)
            {
                foreach (var taskKey in taskKeys)
                {
                    taskAllKeys.Remove(taskKey);
                }
            }
        }

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

            // TODO:key枯渇

            return null;
        }

        public static bool IsCreatedKeyGroup(KeyInfo keyInfo)
        {
            return !groupAllKeys.Contains(keyInfo.KeyGroup);
        }

        public static bool IsCreatedKeyTask(KeyInfo group, KeyInfo task)
        {
            if(groupAllKeys.Contains(group.KeyGroup))
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
            
            // TODO:key枯渇
            return null;
        }

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

        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }

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
