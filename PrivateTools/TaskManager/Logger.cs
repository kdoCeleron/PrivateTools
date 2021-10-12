namespace TaskManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using log4net;

    /// <summary>
    /// ログ出力クラス
    /// </summary>
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

        /// <summary>
        /// デバッグログを出力します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="filePath">ファイルパス(指定しないこと)</param>
        /// <param name="line">行番号(指定しないこと)</param>
        /// <param name="methodName">メソッド名(指定しないこと)</param>
        public static void WriteDebugLog(
            string message, 
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string methodName = "")
        {
            AppLog.Debug(CreateMessage(message, filePath, line, methodName));
        }

        /// <summary>
        /// アプリログを出力します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="filePath">ファイルパス(指定しないこと)</param>
        /// <param name="line">行番号(指定しないこと)</param>
        /// <param name="methodName">メソッド名(指定しないこと)</param>
        public static void WriteAppLog(
            string message, 
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string methodName = "")
        {
            AppLog.Info(CreateMessage(message, filePath, line, methodName));
        }
        
        /// <summary>
        /// エラーログを出力します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="ex">例外情報</param>
        /// <param name="filePath">ファイルパス(指定しないこと)</param>
        /// <param name="line">行番号(指定しないこと)</param>
        /// <param name="methodName">メソッド名(指定しないこと)</param>
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

        /// <summary>
        /// 設定ファイルの読み込みを行います。
        /// </summary>
        /// <param name="configFilePath">ファイルパス</param>
        public static void LoadConfig(string configFilePath)
        {
            if (File.Exists(configFilePath))
            {
                var fullPath = Utils.GetFullPath(configFilePath);
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(fullPath));
            }
        }

        /// <summary>
        /// 出力文言を生成します
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="line">行番号</param>
        /// <param name="methodName">メソッド名</param>
        /// <returns>生成結果</returns>
        private static string CreateMessage(string message, string filePath, int line, string methodName)
        {
            return string.Format("{0}[{1}][{2}][{3}]", message, filePath, line, methodName);
        }

        #endregion
    }
}
