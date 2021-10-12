﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManager.Configration;
using TaskManager.ConfigrationData;
using TaskManager.Controls;

namespace TaskManager.Forms
{
    /// <summary>
    /// 設定変更画面
    /// </summary>
    public partial class ConfigEditForm : SubWindowBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConfigEditForm()
        {
            InitializeComponent();
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

            this.chkIsInitShowMainForm.Checked = config.IsInitShowMainForm;
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
            Config.Instance.EditableItems.IsInitShowMainForm = this.chkIsInitShowMainForm.Checked;
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