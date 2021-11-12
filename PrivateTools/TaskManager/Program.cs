using System.Runtime.CompilerServices;
using log4net.Repository.Hierarchy;
using Microsoft.Toolkit.Uwp.Notifications;
using MyTools.Common.Utils;
using TaskManager.Configration;
using TaskManager.Data;
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

    using MyTools.Common;

    /// <summary>
    /// エントリポイント
    /// </summary>
    public static class Program
    {
        #region フィールド

        /// <summary>
        /// 期限切れ検知用タイマのキー
        /// </summary>
        public static int TermOutTimerKey;

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

        /// <summary>
        /// 前回期限切れ判定時の期限切れタスクのリスト
        /// </summary>
        private static List<TaskItem> prevTermOutTaskList = new List<TaskItem>();

        /// <summary>
        /// 前回期限切れ判定時の日時
        /// </summary>
        private static DateTime prevNotifedDateTime;

        #endregion

        #region イベント

        /// <summary>
        /// 期限切れ検知用タイマのタイムアウト完了時処理
        /// </summary>
        public static event Action ActionTermOutTimerEventOnCompleted = delegate { };

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

            // 各種設定ファイル読み込み
            Config.Instance.ReadConfig();
            ResourceManager.Instance.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetExceptionHandlers();

            // 2重起動チェック
            if (IsDuplicate())
            {
                Environment.Exit(0);
            }

            TermOutTimerKey = TimerManager.CreateTimer(TermOutTimerCallBack);
            TimerManager.Change(TermOutTimerKey, new TimeSpan(), TimeSpan.FromMinutes(1));
            if (Config.Instance.EditableItems.IsStayInTaskTray)
            {
                // タスクトレイ常駐
                InitializeTaskTray();
                if (Config.Instance.EditableItems.IsInitShowMainForm)
                {
                    // メイン画面を初期表示
                    TaskTrayMenuEvents.ShowMainForm(null, null);
                }

                Application.Run();
            }
            else
            {
                var mainform = new MainForm();
                Application.Run(mainform);
            }
        }

        /// <summary>
        /// 期限切れ判定タイマのタイムアウト時コールバック
        /// </summary>
        /// <param name="state">状態</param>
        private static void TermOutTimerCallBack(object state)
        {
            if (Config.Instance.EditableItems.IsNotifyWindowsToast)
            {
                var tasks = ResourceManager.Instance.GetAllTaskItems().Where(x => Utils.IsOverLimit(x.DateTimeLimit));
                var tmpTasks = tasks.ToList();
                var today = DateTime.Today;
                var isNotifyToast = true;
                if (prevTermOutTaskList.SequenceEqual(tmpTasks))
                {
                    var diff = (today - prevNotifedDateTime).Days;
                    if (Config.Instance.EditableItems.NotifyTermOutSpanDay > diff)
                    {
                        // 同一リストかつ再通知期限内の場合は通知しない。
                        isNotifyToast = false;
                    }
                }

                if (isNotifyToast)
                {
                    prevNotifedDateTime = today;
                    prevTermOutTaskList = tmpTasks;
                    if (prevTermOutTaskList.Any())
                    {
                        var toast = new ToastContentBuilder()
                            .AddText("タスクの期限切れ通知")
                            .AddText("期限切れのタスクがあります。確認してください");
                        toast.Show();
                    }
                }
            }

            ActionTermOutTimerEventOnCompleted();
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
            Logger.Error(unKnownMessage, e);
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

        /// <summary>
        /// タスクトレイの設定を初期化します。
        /// </summary>
        private static void InitializeTaskTray()
        {
            // タスクトレイの設定
            var icon = new NotifyIcon();

            icon.Icon = new System.Drawing.Icon(@".\icon\main.ico");
            icon.Visible = true;
            icon.Text = "タスク管理ツール";
            icon.MouseUp += TaskTrayMenuEvents.IconOnMouseUp;

            var menu = new ContextMenuStrip();
            {
                var menuItem = new ToolStripMenuItem();
                menuItem.Text = "メイン画面表示";
                menuItem.Click += TaskTrayMenuEvents.ShowMainForm;
                menu.Items.Add(menuItem);
            }

            {
                var menuItem = new ToolStripMenuItem();
                menuItem.Text = "タスク一覧をCSV出力";
                menuItem.Click += TaskTrayMenuEvents.OutputCsvTaskList;
                menu.Items.Add(menuItem);
            }

            {
                var menuItem = new ToolStripMenuItem();
                menuItem.Text = "実行フォルダを開く";
                menuItem.Click += TaskTrayMenuEvents.ShowExecFolder;
                menu.Items.Add(menuItem);
            }
            
            {
                var menuItem = new ToolStripMenuItem();
                menuItem.Text = "設定変更";
                menuItem.Click += TaskTrayMenuEvents.ShowConfigEditForm;
                menu.Items.Add(menuItem);
            }
            
            {
                var menuItem = new ToolStripMenuItem();
                menuItem.Text = "終了";
                menuItem.Click += TaskTrayMenuEvents.ApplicationExit;
                menu.Items.Add(menuItem);
            }

            icon.ContextMenuStrip = menu;

            TaskTrayMenuEvents.Icon = icon;

            Application.ApplicationExit += (sender, e) =>
            {
                if (TaskTrayMenuEvents.Icon != null)
                {
                    TaskTrayMenuEvents.Icon.Dispose();
                    TaskTrayMenuEvents.Icon = null;
                }

                TimerManager.DisposeTimer(TermOutTimerKey);
            };
        }

        #endregion
    }
}
