using TaskManager.Forms;

namespace TaskManager
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// エントリポイント
    /// </summary>
    public static class Program
    {
        #region フィールド

        /// <summary>
        /// ウインドウ表示パラメータ
        /// </summary>
        private const int SwNormal = 1;

        /// <summary>
        /// 二重起動防止用Mutex文字列
        /// </summary>
        private const string DuplicateInstanceMutex = @"{6D4A4F49-22B3-4E19-9532-7DAF8432E193}";

        /// <summary>
        /// 二重起動防止用Mutexオブジェクト
        /// </summary>
        private static Mutex duplicateMutex;

        #endregion

        #region コンストラクタ

        #endregion

        #region プロパティ

        #endregion

        #region メソッド

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        public static void Main()
        {
            // Logger の設定ファイルを読み込み
            var log4netConfigPath = @".\Log4net.xml";
            Logger.LoadConfig(log4netConfigPath);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetExceptionHandlers();

            if (IsDuplicate())
            {
                Environment.Exit(0);
            }

            Application.Run(new MainForm());
        }

        /// <summary>
        /// ウインドウをフォアグラウンドに設定
        /// </summary>
        /// <param name="hWnd">ウインドウハンドル</param>
        /// <returns>処理結果</returns>
        [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// ウインドウ表示
        /// </summary>
        /// <param name="hWnd">ウインドウハンドル</param>
        /// <param name="nCmdShow">コマンド</param>
        /// <returns>処理結果</returns>
        [DllImport("USER32.DLL", CharSet = CharSet.Auto)]
        private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// 既定例外ハンドラを設定します。
        /// </summary>
        private static void SetExceptionHandlers()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;
        }

        /// <summary>
        /// <see cref="AppDomain.UnhandledException" />が発生した際に呼ばれます。
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                ReportUnhandledException(ex);
            }
        }

        /// <summary>
        /// 未処理例外が発生した際に呼ばれます。
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            var ex = e.Exception;
            if (ex != null)
            {
                ReportUnhandledException(ex);
            }
        }

        /// <summary>
        /// 未処理例外の処理
        /// </summary>
        /// <param name="e">例外情報</param>
        private static void ReportUnhandledException(Exception e)
        {
            // 例外情報をエラーログに出力する
            var unKnownMessage = string.Format("予期せぬエラーが発生しました。\nツール管理者へ連絡してください。\n例外情報：{0}", e.ToString());
            Logger.WriteErrorLog(unKnownMessage, e);
            MessageBox.Show(unKnownMessage, "エラー");
        }

        /// <summary>
        /// 2重起動チェック
        /// </summary>
        /// <returns>実行結果</returns>
        private static bool IsDuplicate()
        {
            try
            {
                bool createdMutex;
                duplicateMutex = new Mutex(true, DuplicateInstanceMutex, out createdMutex);
                if (!createdMutex)
                {
                    duplicateMutex.Close();

                    var currentProcess = Process.GetCurrentProcess();
                    var currentProcessName = currentProcess.ProcessName;

                    if (!currentProcessName.Contains("vshost"))
                    {
                        var processNames = Process.GetProcessesByName(currentProcessName);
                        if (processNames.Count() > 1)
                        {
                            foreach (var hProcess in processNames)
                            {
                                if (hProcess.Id != currentProcess.Id)
                                {
                                    ShowWindow(hProcess.MainWindowHandle, SwNormal);
                                    SetForegroundWindow(hProcess.MainWindowHandle);
                                }
                            }
                        }
                    }

                    return true;
                }
            }
            catch
            {
                // Nop
            }

            return false;
        }

        #endregion
    }
}
