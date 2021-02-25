using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManager
{
    using TaskManager.Data;

    /// <summary>
    /// 汎用ユーティリティクラス
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// フルパスを取得します。
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns>取得結果</returns>
        public static string GetFullPath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                // 相対パスでない場合は、そのまま返す
                return path;
            }

            // 相対パスの場合は、exeの実行パスを基点としたフルパスにする。
            var cur = Path.GetDirectoryName(Application.ExecutablePath);
            if (cur != null)
            {
                return Path.Combine(cur, path);
            }

            return path;
        }

        public static List<TaskItem> FilterRecentLimitTask(List<TaskItem> taskItem)
        {
            return taskItem.Where(x => 
                    Utils.IsOverRedZone(x.DateTimeLimit)
                    || Utils.IsOverYellowZone(x.DateTimeLimit))
                .OrderBy(x => x.DateTimeLimit).ToList();
        }

        public static bool IsOverRedZone(DateTime value)
        {
            var tmp = DateTime.Now;
            var now = new DateTime(tmp.Year, tmp.Month, tmp.Day + 1); // 繰り上げ
            var date = new DateTime(value.Year, value.Month, value.Day);

            // TODO: Configへ
            var redZone = now.AddDays(1);
            var normalZone = now.AddDays(3);
            if (redZone >= date)
            {
                // 期限間近 or 期限超過
                return true;
            }
            else if (redZone < date && normalZone > date)
            {
                // 1～3日前
                return false;
            }

            return false;
        }

        public static bool IsOverYellowZone(DateTime value)
        {
            var tmp = DateTime.Now;
            var now = new DateTime(tmp.Year, tmp.Month, tmp.Day + 1);  // 繰り上げ
            var date = new DateTime(value.Year, value.Month, value.Day);

            // TODO: Configへ
            var redZone = now.AddDays(1);
            var normalZone = now.AddDays(3);
            if (redZone >= date)
            {
                // 期限間近 or 期限超過
                return false;
            }
            else if (redZone < date && normalZone > date)
            {
                // 1～3日前
                return true;
            }

            return false;
        }
    }
}
