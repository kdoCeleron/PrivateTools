using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Data
{
    public class KeyInfo
    {
        private static List<string> groupKeys;

        private static List<string> taskKeys;

        public string Key { get; private set; }

        public static void Initialize()
        {
            groupKeys = new List<string>();
            taskKeys = new List<string>();

            var maxKeyNum = 500;

        }

        public static string GenerateKey(object src)
        {
            if (src == null)
            {
                return string.Empty;
            }

            var prefix = "Other_";
            var hashCode = src.GetHashCode().ToString();
            if (src is TaskGroupInfo)
            {
                prefix = "Group_";
            }

            if (src is TaskItem)
            {
                prefix = "Task_";
            }

            return prefix + hashCode;
        }
    }
}
