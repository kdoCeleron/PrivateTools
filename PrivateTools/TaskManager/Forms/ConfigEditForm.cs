using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel.Chat;
using Windows.ApplicationModel.Contacts;
using TaskManager.Configration;
using TaskManager.ConfigrationData;
using TaskManager.Controls;
using TaskManager.Interfaces;

namespace TaskManager.Forms
{
    /// <summary>
    /// 設定変更画面
    /// </summary>
    public partial class ConfigEditForm : SubWindowBase, ICanShowFromTaskTray
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConfigEditForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// タスクトレイから表示中かどうか
        /// </summary>
        public bool IsShowFromTaskTray { get; set; }

        /// <summary>
        /// 画面種別
        /// </summary>
        public ViewKind ViewType
        {
            get
            {
                return ViewKind.ConfigEditView;
            }
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="config">編集対象</param>
        /// <returns>true:正/false:それ以外</returns>
        public bool Initialize(EditableConfigData config)
        {
            if (config == null)
            {
                return false;
            }

            this.chkIsStayInTaskTray.Checked = config.IsStayInTaskTray;
            this.chkIsInitShowMainForm.Checked = config.IsInitShowMainForm;
            this.chkIsNotifyWindowsToast.Checked = config.IsNotifyWindowsToast;
            this.spnNotifyTermOutSpanDay.Value = config.NotifyTermOutSpanDay;
            this.spnWarnRed.Value = config.ThresDaysRed;
            this.spnWarnYellow.Value = config.ThresDaysYellow;

            return true;
        }

        /// <summary>
        /// 確定ボタン押下時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void BtnKakutei_Click(object sender, EventArgs e)
        {
            Config.Instance.EditableItems.IsStayInTaskTray = this.chkIsStayInTaskTray.Checked;
            Config.Instance.EditableItems.IsInitShowMainForm = this.chkIsInitShowMainForm.Checked;
            Config.Instance.EditableItems.IsNotifyWindowsToast = this.chkIsNotifyWindowsToast.Checked;
            Config.Instance.EditableItems.NotifyTermOutSpanDay = (int)this.spnNotifyTermOutSpanDay.Value;
            Config.Instance.EditableItems.ThresDaysRed = (int)this.spnWarnRed.Value;
            Config.Instance.EditableItems.ThresDaysYellow = (int)this.spnWarnYellow.Value;

            Config.Instance.WriteConfig();

            this.CloseWindow(SubWindowResult.Submit);
        }

        /// <summary>
        /// キャンセルボタン押下時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.CloseWindow(SubWindowResult.Cancel);
        }
    }
}
