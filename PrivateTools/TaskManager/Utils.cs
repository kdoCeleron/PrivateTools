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
    /// <summary>
    /// 汎用ユーティリティクラス
    /// </summary>
    public static class Utils
    {
        public static string GetFullPath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                // 相対パスでない場合は、そのまま返す
                return path;
            }

            // 相対パスの場合は、exeの実行パスを基点としたフルパスにする。
            var cur = Path.GetDirectoryName(Application.ExecutablePath);
            //var cur = Environment.CurrentDirectory;
            if (cur != null)
            {
                return Path.Combine(cur, path);
            }

            return path;
        }
    }
}
