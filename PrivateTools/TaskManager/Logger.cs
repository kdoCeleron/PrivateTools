using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace TaskManager
{
    public class Logger
    {
        #region フィールド

        /// <summary>
        /// Appロガー
        /// </summary>
        private static readonly ILog AppLog = LogManager.GetLogger("ApplicationLog");

        /// <summary>
        /// Errorロガー
        /// </summary>
        private static readonly ILog ErrorLog = LogManager.GetLogger("ErrorLog");
        
        #endregion

        #region メソッド
        
        public static void WriteDebugLog(
            string message, 
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string methodName = "")
        {
            AppLog.Debug(CreateMessage(message, filePath, line, methodName));
        }

        public static void WriteAppLog(
            string message, 
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string methodName = "")
        {
            AppLog.Info(CreateMessage(message, filePath, line, methodName));
        }
        
        public static void WriteErrorLog(
            string message, 
            Exception ex = null,
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string methodName = "")
        {
            var createMessage = CreateMessage(message, filePath, line, methodName);
            if (ex != null)
            {
                var msg = string.Format("{0}\n{1}", createMessage, ex.ToString());
                AppLog.Error(msg);
                ErrorLog.Error(msg, ex);
            }
            else
            {
                AppLog.Error(createMessage);
                ErrorLog.Error(createMessage);
            }
        }

        public static void LoadConfig(string configFilePath)
        {
            if (File.Exists(configFilePath))
            {
                var fullPath = Path.GetFullPath(configFilePath);
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(fullPath));
            }
        }

        private static string CreateMessage(string message, string filePath, int line, string methodName)
        {
            return string.Format("{0}[{1}][{2}][{3}]", message, filePath, line, methodName);
        }

        #endregion
    }
}
