
namespace TaskManager.Forms
{
    partial class ConfigEditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkIsInitShowMainForm = new System.Windows.Forms.CheckBox();
            this.spnWarnRed = new System.Windows.Forms.NumericUpDown();
            this.lblWarnRedThres = new System.Windows.Forms.Label();
            this.lblWarnYellowThres = new System.Windows.Forms.Label();
            this.spnWarnYellow = new System.Windows.Forms.NumericUpDown();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnKakutei = new System.Windows.Forms.Button();
            this.chkIsStayInTaskTray = new System.Windows.Forms.CheckBox();
            this.chkIsNotifyWindowsToast = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.spnNotifyTermOutSpanDay = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.spnWarnRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnWarnYellow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnNotifyTermOutSpanDay)).BeginInit();
            this.SuspendLayout();
            // 
            // chkIsInitShowMainForm
            // 
            this.chkIsInitShowMainForm.AutoSize = true;
            this.chkIsInitShowMainForm.Location = new System.Drawing.Point(12, 34);
            this.chkIsInitShowMainForm.Name = "chkIsInitShowMainForm";
            this.chkIsInitShowMainForm.Size = new System.Drawing.Size(395, 16);
            this.chkIsInitShowMainForm.TabIndex = 0;
            this.chkIsInitShowMainForm.Text = "初期起動時にメイン画面を表示するかどうか(タスクトレイ常駐起動時のみ有効)";
            this.chkIsInitShowMainForm.UseVisualStyleBackColor = true;
            // 
            // spnWarnRed
            // 
            this.spnWarnRed.Location = new System.Drawing.Point(191, 105);
            this.spnWarnRed.Name = "spnWarnRed";
            this.spnWarnRed.Size = new System.Drawing.Size(54, 19);
            this.spnWarnRed.TabIndex = 1;
            // 
            // lblWarnRedThres
            // 
            this.lblWarnRedThres.AutoSize = true;
            this.lblWarnRedThres.Location = new System.Drawing.Point(12, 109);
            this.lblWarnRedThres.Name = "lblWarnRedThres";
            this.lblWarnRedThres.Size = new System.Drawing.Size(121, 12);
            this.lblWarnRedThres.TabIndex = 2;
            this.lblWarnRedThres.Text = "警告色(赤色)表示日数";
            // 
            // lblWarnYellowThres
            // 
            this.lblWarnYellowThres.AutoSize = true;
            this.lblWarnYellowThres.Location = new System.Drawing.Point(12, 132);
            this.lblWarnYellowThres.Name = "lblWarnYellowThres";
            this.lblWarnYellowThres.Size = new System.Drawing.Size(121, 12);
            this.lblWarnYellowThres.TabIndex = 4;
            this.lblWarnYellowThres.Text = "警告色(黄色)表示日数";
            // 
            // spnWarnYellow
            // 
            this.spnWarnYellow.Location = new System.Drawing.Point(191, 128);
            this.spnWarnYellow.Name = "spnWarnYellow";
            this.spnWarnYellow.Size = new System.Drawing.Size(54, 19);
            this.spnWarnYellow.TabIndex = 3;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(344, 162);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 7;
            this.BtnCancel.Text = "キャンセル";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnKakutei
            // 
            this.BtnKakutei.Location = new System.Drawing.Point(263, 162);
            this.BtnKakutei.Name = "BtnKakutei";
            this.BtnKakutei.Size = new System.Drawing.Size(75, 23);
            this.BtnKakutei.TabIndex = 6;
            this.BtnKakutei.Text = "編集";
            this.BtnKakutei.UseVisualStyleBackColor = true;
            this.BtnKakutei.Click += new System.EventHandler(this.BtnKakutei_Click);
            // 
            // chkIsStayInTaskTray
            // 
            this.chkIsStayInTaskTray.AutoSize = true;
            this.chkIsStayInTaskTray.Location = new System.Drawing.Point(12, 12);
            this.chkIsStayInTaskTray.Name = "chkIsStayInTaskTray";
            this.chkIsStayInTaskTray.Size = new System.Drawing.Size(145, 16);
            this.chkIsStayInTaskTray.TabIndex = 8;
            this.chkIsStayInTaskTray.Text = "タスクトレイに常駐させるか";
            this.chkIsStayInTaskTray.UseVisualStyleBackColor = true;
            // 
            // chkIsNotifyWindowsToast
            // 
            this.chkIsNotifyWindowsToast.AutoSize = true;
            this.chkIsNotifyWindowsToast.Location = new System.Drawing.Point(12, 56);
            this.chkIsNotifyWindowsToast.Name = "chkIsNotifyWindowsToast";
            this.chkIsNotifyWindowsToast.Size = new System.Drawing.Size(326, 16);
            this.chkIsNotifyWindowsToast.TabIndex = 9;
            this.chkIsNotifyWindowsToast.Text = "期限切れタスク発生時にWindowsの通知(トースト)を行うかどうか";
            this.chkIsNotifyWindowsToast.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "期限切れ通知の再通知間隔";
            // 
            // spnNotifyTermOutSpanDay
            // 
            this.spnNotifyTermOutSpanDay.Location = new System.Drawing.Point(191, 82);
            this.spnNotifyTermOutSpanDay.Name = "spnNotifyTermOutSpanDay";
            this.spnNotifyTermOutSpanDay.Size = new System.Drawing.Size(54, 19);
            this.spnNotifyTermOutSpanDay.TabIndex = 11;
            // 
            // ConfigEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 193);
            this.Controls.Add(this.spnNotifyTermOutSpanDay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkIsNotifyWindowsToast);
            this.Controls.Add(this.chkIsStayInTaskTray);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnKakutei);
            this.Controls.Add(this.lblWarnYellowThres);
            this.Controls.Add(this.spnWarnYellow);
            this.Controls.Add(this.lblWarnRedThres);
            this.Controls.Add(this.spnWarnRed);
            this.Controls.Add(this.chkIsInitShowMainForm);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigEditForm";
            this.Text = "設定変更";
            ((System.ComponentModel.ISupportInitialize)(this.spnWarnRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnWarnYellow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spnNotifyTermOutSpanDay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkIsInitShowMainForm;
        private System.Windows.Forms.NumericUpDown spnWarnRed;
        private System.Windows.Forms.Label lblWarnRedThres;
        private System.Windows.Forms.Label lblWarnYellowThres;
        private System.Windows.Forms.NumericUpDown spnWarnYellow;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnKakutei;
        private System.Windows.Forms.CheckBox chkIsStayInTaskTray;
        private System.Windows.Forms.CheckBox chkIsNotifyWindowsToast;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown spnNotifyTermOutSpanDay;
    }
}