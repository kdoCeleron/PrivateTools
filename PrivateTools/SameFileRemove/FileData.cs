using System.Collections.Generic;
using System.Linq;

namespace SameFileRemove
{
    public class FileData
    {
        public List<byte> Data { get; set; }

        public bool IsEquals(object obj)
        {
            var item = obj as FileData;
            if (item == null)
            {
                return false;
            }

            if (Data.Count != item.Data.Count)
            {
                return false;
            }

            var index = 0;
            foreach (var itemDat in item.Data)
            {
                if (this.Data[index] == itemDat)
                {
                    index++;
                    continue;
                }

                return false;
            }

            return true;
        }
    }
}