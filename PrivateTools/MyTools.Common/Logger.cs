namespace MyTools.Common
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using log4net;
    
    /// <summary>
    /// ログ出力機能を提供します。
    /// </summary>
    /// <remarks>
    /// 内部でLog4Netを利用してログ出力を行います。
    /// </remarks>
    public static class Logger
    {
        /// <summary>
        /// Appロガー
        /// </summary>
        private static readonly ILog AppLog = LogManager.GetLogger("ApplicationLog");

        /// <summary>
        /// Errorロガー
        /// </summary>
        private static readonly ILog ErrorLog = LogManager.GetLogger("ErrorLog");
        
        /// <summary>
        /// 通信ロガー
        /// </summary>
        private static readonly ILog TransportLog = LogManager.GetLogger("TransportLog");
        
        /// <summary>
        /// タイムアウトロガー
        /// </summary>
        private static readonly ILog TimeoutLog = LogManager.GetLogger("TimeoutLog");
        
        /// <summary>
        /// アプリケーションログ(Debug)出力.
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="member">呼び出し元メソッド名</param>
        /// <param name="file">呼び出し元ファイル名</param>
        /// <param name="line">呼び出し元行番号</param>
        public static void Debug(string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            try
            {
                if (AppLog.IsDebugEnabled)
                {
                    AppLog.Debug(CreateMessage(member, file, line, message));   
                }
            }
            catch (Exception)
            {
                // noop
            }
        }

        /// <summary>
        /// アプリケーションログ(Info)出力.
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="member">呼び出し元メソッド名</param>
        /// <param name="file">呼び出し元ファイル名</param>
        /// <param name="line">呼び出し元行番号</param>
        public static void Info(string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            try
            {
                if (AppLog.IsInfoEnabled)
                {
                    AppLog.Info(CreateMessage(member, file, line, message));                
                }
            }
            catch (Exception)
            {
                // noop
            }
        }

        /// <summary>
        /// アプリケーションログ(Warn)出力.
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="member">呼び出し元メソッド名</param>
        /// <param name="file">呼び出し元ファイル名</param>
        /// <param name="line">呼び出し元行番号</param>
        public static void Warn(string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            try
            {
                if (AppLog.IsWarnEnabled)
                {
                    AppLog.Warn(CreateMessage(member, file, line, message));                
                }
            }
            catch (Exception)
            {
                // noop
            }
        }

        /// <summary>
        /// アプリケーションログ(Error)とErrorログ出力.
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="cause">エラー発生元例外</param>
        /// <param name="member">呼び出し元メソッド名</param>
        /// <param name="file">呼び出し元ファイル名</param>
        /// <param name="line">呼び出し元行番号</param>
        public static void Error(string message, Exception cause = null, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            try
            {
                if (AppLog.IsErrorEnabled || ErrorLog.IsErrorEnabled)
                {
                    var now = DateTime.Now;
                    var s = now.ToString("yyyy/MM/dd HH:mm:ss");
                    var prefixLine = $"[{s}]";
                    AppLog.Error(prefixLine);
                    ErrorLog.Error(prefixLine);

                    if (null == cause)
                    {
                        var log = CreateMessage(member, file, line, message);
                        AppLog.Error(log);
                        ErrorLog.Error(log);
                    }
                    else
                    {
                        var log = CreateMessage(member, file, line, string.Format(@"{0}[{1}]", message, cause.Message));
                        AppLog.Error(log);
                        ErrorLog.Error(log, cause);
                    }
                }
            }
            catch (Exception)
            {
                // noop
            }
        }

        /// <summary>
        /// Logger の設定ファイルを読み込みます。
        /// </summary>
        /// <param name="configFilePath">設定ファイルのパス</param>
        public static void LoadConfig(string configFilePath)
        {
            if (File.Exists(configFilePath))
            {
                var fullPath = Path.GetFullPath(configFilePath);
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(fullPath));
            }
        }

        /// <summary>
        /// 呼び出し元メンバ名とメッセージを合成してログメッセージを生成する.
        /// </summary>
        /// <param name="member">呼び出し元メソッド名</param>
        /// <param name="file">呼び出し元ファイル名</param>
        /// <param name="line">呼び出し元行番号</param>
        /// <param name="message">メッセージ</param>
        /// <returns>合成したログメッセージ</returns>
        private static string CreateMessage(string member, string file, int line, string message)
        {
            System.Diagnostics.Debug.WriteLine("[{0}]: {1}({2})[{3}]: {4}", DateTime.Now, Path.GetFileName(file), line, member, message);
            var fileMember = string.Format("({0}.{1})", Path.GetFileNameWithoutExtension(file), member);
            return string.Format("{0, -40} - {1}", fileMember, message);
        }
    }
}