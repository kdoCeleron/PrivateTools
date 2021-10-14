using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyTools.Common.Utils;
using TaskManager.Configration;
using TaskManager.Forms;
using TaskManager.Interfaces;

namespace TaskManager
{
    /// <summary>
    /// タスクトレイのイベントクラス
    /// </summary>
    public static class TaskTrayMenuEvents
    {
        /// <summary>
        /// NotifyIconの参照
        /// </summary>
        public static NotifyIcon Icon = null;

        /// <summary>
        /// 表示中画面の管理情報
        /// </summary>
        private static Dictionary<ViewKind, ICanShowFromTaskTray> showingViewMap = new Dictionary<ViewKind, ICanShowFromTaskTray>();

        /// <summary>
        /// タスクトレイアイコンのメニュー表示を左クリックでも有効にします。
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        public static void IconOnMouseUp(object sender, MouseEventArgs e)
        {
            if (Icon != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    var method = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (method != null)
                    {
                        method.Invoke(Icon, null);
                    }
                }
            }
        }

        /// <summary>
        /// アプリケーションを終了します。
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        public static void ApplicationExit(object sender, EventArgs e)
        {
            Utils.SaveConfigs();

            if (Icon != null)
            {
                Icon.Visible = false;
            }

            Application.Exit();
        }

        /// <summary>
        /// メイン画面を表示します。
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        public static void ShowMainForm(object sender, EventArgs e)
        {
            var viewKind = ViewKind.MainView;
            if (!showingViewMap.ContainsKey(viewKind))
            {
                showingViewMap.Add(viewKind, null);
            }

            if (showingViewMap[viewKind] != null)
            {
                var form = showingViewMap[viewKind] as Form;
                if (form != null)
                {
                    form.Activate();
                    
                    return;
                }
            }

            var win = new MainForm();
            win.IsShowFromTaskTray = true;
            win.Closed += WinOnClosed;

            showingViewMap[viewKind] = win;

            win.Show();
        }

        /// <summary>
        /// タスク一覧をCSV出力します。
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト7</param>
        /// <param name="e">イベント引数</param>
        public static void OutputCsvTaskList(object sender, EventArgs e)
        {
            if (!Directory.Exists(Config.Instance.CsvRootDir))
            {
                Directory.CreateDirectory(Config.Instance.CsvRootDir);
            }

            var allTaskList = ResourceManager.Instance.GetAllTaskItems();

            var list = new List<string>();
            foreach (var taskItem in allTaskList)
            {
                var tmp = taskItem.GetInfoText(",");
                tmp = tmp.Replace(Environment.NewLine, @"\r\n");
                list.Add(tmp);
            }
            
            var fileName = PathUtils.CreateFilePathAppendDateTime(Config.Instance.CsvRootDir, "TaskAllList", "csv", false);
            var path = Utils.GetFullPath(fileName);
            File.WriteAllLines(path, list, Encoding.UTF8);

            var dir = Path.GetDirectoryName(path);
            if (Directory.Exists(dir))
            {
                System.Diagnostics.Process.Start(dir);
            }
        }
        
        /// <summary>
        /// 実行フォルダを開きます
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        public static void ShowExecFolder(object sender, EventArgs e)
        {
            var path = Environment.CurrentDirectory;
            if (Directory.Exists(path))
            {
                System.Diagnostics.Process.Start(path);
            }
        }
        
        /// <summary>
        /// 設定変更画面を表示します
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        public static void ShowConfigEditForm(object sender, EventArgs e)
        {
            var viewKind = ViewKind.ConfigEditView;
            if (!showingViewMap.ContainsKey(viewKind))
            {
                showingViewMap.Add(viewKind, null);
            }

            if (showingViewMap[viewKind] != null)
            {
                var form = showingViewMap[viewKind] as Form;
                if (form != null)
                {
                    form.Activate();

                    return;
                }
            }

            var win = new ConfigEditForm();
            win.IsShowFromTaskTray = true;
            win.Closed += WinOnClosed;

            showingViewMap[viewKind] = win;

            win.Show();
        }

        /// <summary>
        /// 画面クローズ時に、表示中情報から画面情報を削除します。
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private static void WinOnClosed(object sender, EventArgs e)
        {
            var win = sender as ICanShowFromTaskTray;
            if (win != null)
            {
                if (showingViewMap.ContainsKey(win.ViewType))
                {
                    // 表示情報から削除
                    showingViewMap[win.ViewType] = null;
                }
            }
        }
    }
}
