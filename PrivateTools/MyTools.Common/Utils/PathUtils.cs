using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTools.Common.Utils
{
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;

    using MyTools.Common.Extensions;

    public static class PathUtils
    {
        /// <summary>
        /// フォルダが無い場合にフォルダを作成します。
        /// </summary>
        /// <param name="dirPath">フォルダパス</param>
        public static void CreateDirectoryIfNonExists(string dirPath)
        {
            if (!System.IO.Directory.Exists(dirPath))
            {
                System.IO.Directory.CreateDirectory(dirPath);
            }
        }

        /// <summary>
        /// アプリケーションの実行ディレクトリを取得します
        /// </summary>
        /// <returns>アプリケーションの実行ディレクトリ</returns>
        public static string GetAppPath()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// ファイル探索(再帰)
        /// </summary>
        /// <param name="folder">フォルダ</param>
        /// <param name="filter"></param>
        public static List<string> SearchFilesInDirecotryRecursive(string folder, Func<string, bool> filter)
        {
            if (!Directory.Exists(folder))
            {
                return new List<string>();
            }

            var ret = new List<string>();
            foreach (var file in Directory.EnumerateFiles(folder))
            {
                if (filter != null)
                {
                    if (!filter(file))
                    {
                        continue;
                    }
                }

                ret.Add(file);
            }

            foreach (var directory in Directory.EnumerateDirectories(folder))
            {
                var tmpRet = SearchFilesInDirecotryRecursive(directory, filter);

                ret.AddRange(tmpRet);
            }

            return ret;
        }

        /// <summary>
        /// ファイル探索(非再帰)
        /// </summary>
        /// <param name="folder">フォルダ</param>
        /// <param name="filter"></param>
        public static List<string> SearchFilesInDirecotry(string folder, Func<string, bool> filter)
        {
            if (!Directory.Exists(folder))
            {
                return new List<string>();
            }
            
            var ret = new List<string>();
            foreach (var file in Directory.EnumerateFiles(folder))
            {
                if (filter != null)
                {
                    if (!filter(file))
                    {
                        continue;
                    }
                }

                ret.Add(file);
            }

            return ret;
        }

        /// <summary>
        /// ファイルパスを生成します。
        /// </summary>
        /// <param name="folder">フォルダ</param>
        /// <param name="fileNameWithoutExtension">拡張子を除くファイル名</param>
        /// <param name="extension">拡張子("."なし)</param>
        /// <returns>生成結果</returns>
        public static string CreateFilePath(string folder, string fileNameWithoutExtension, string extension)
        {
            var fmt = "{0}_{1:D3}.{2}";
            var index = 0;

            if (!Directory.Exists(folder))
            {
                // フォルダが存在しない場合は、初期値で生成
                return Path.Combine(folder, fmt.Fmt(fileNameWithoutExtension, 0, extension));
            }

            string path;
            do
            {
                var fileName = fmt.Fmt(fileNameWithoutExtension, index, extension);
                path = Path.Combine(folder, fileName);
                index++;
            }
            while (File.Exists(path));

            return path;
        }

        /// <summary>
        /// ファイルパスを生成します。(日時追加)
        /// </summary>
        /// <param name="folder">フォルダ</param>
        /// <param name="fileNameWithoutExtension">拡張子を除くファイル名</param>
        /// <param name="extension">拡張子("."なし)</param>
        /// <param name="isAppendMillisec">ミリ秒まで出力するか</param>
        /// <returns>生成結果</returns>
        public static string CreateFilePathAppendDateTime(string folder, string fileNameWithoutExtension, string extension, bool isAppendMillisec)
        {
            var now = DateTime.Now;
            string fileName;
            if (isAppendMillisec)
            {
                var fmt = "{0}_{1:D4}{2:D2}{3:D2}{4:D2}{5:D2}{6:D2}{7:D3}.{8}";
                fileName = fmt.Fmt(
                    fileNameWithoutExtension,
                    now.Year,
                    now.Month,
                    now.Day,
                    now.Hour,
                    now.Minute,
                    now.Second,
                    now.Millisecond,
                    extension);
            }
            else
            {
                var fmt = "{0}_{1:D4}{2:D2}{3:D2}{4:D2}{5:D2}{6:D2}.{7}";
                fileName = fmt.Fmt(
                    fileNameWithoutExtension,
                    now.Year,
                    now.Month,
                    now.Day,
                    now.Hour,
                    now.Minute,
                    now.Second,
                    extension);
            }
            
            return Path.Combine(folder, fileName);
        }

        public static string CreateDateTimeFolder(string baseFolder, string prefix = null, string suffix = null, bool isFullPath = false)
        {
            if (!Directory.Exists(baseFolder))
            {
                return string.Empty;
            }

            var now = DateTime.Now; 
            var fmt = "{0:D4}{1:D2}{2:D2}{3:D2}{4:D2}{5:D2}";
            var foldername = fmt.Fmt(
                    now.Year,
                    now.Month,
                    now.Day,
                    now.Hour,
                    now.Minute,
                    now.Second);

            if (prefix != null)
            {
                foldername = prefix + "_" + foldername;
            }

            if (suffix != null)
            {
                foldername = foldername + "_" + suffix;
            }
            
            var path = Path.Combine(baseFolder, foldername);
            Directory.CreateDirectory(path);

            if (isFullPath)
            {
                return Path.GetFullPath(path);
            }

            return path;
        }

        /// <summary>
        /// バイナリファイル(.bin)かどうかチェックします。
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>bool</returns>
        public static bool IsBinaryFile(string fileName)
        {
            var r = new Regex(@".+\.(bin|dat)$", RegexOptions.IgnoreCase);

            var result = r.Match(fileName);

            return result.Success;
        }
    }
}
