using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        /// 表示中画面の管理情報
        /// </summary>
        private static Dictionary<ViewKind, ICanShowFromTaskTray> showingViewMap = new Dictionary<ViewKind, ICanShowFromTaskTray>();

        /// <summary>
        /// アプリケーションを終了します。
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        public static void ApplicationExit(object sender, EventArgs e)
        {
            Utils.SaveConfigs();
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
