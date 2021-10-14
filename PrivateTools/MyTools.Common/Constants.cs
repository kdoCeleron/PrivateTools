using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool.Common
{
    using System.IO;

    using MyTools.Common.Extensions;

    public static class Constants
    {

        /// <summary>
        /// ロガーのコンフィグファイルのパス
        /// </summary>
        public static readonly string LoggerConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configs\Log4netConfig.xml");

        public static class Messages
        {
            public static string ErrorDirectoryNotExist(string dirName)
            {
                return "ディレクトリが存在しません.\n パス：{0}".Fmt(dirName);
            }

            public static string ErrorFileNotExist(string fileName)
            {
                return "ファイルが存在しません.\n パス：{0}".Fmt(fileName);
            }
        }
    }
}
