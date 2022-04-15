
namespace CreateDateFolder
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtCurrentFolder = new System.Windows.Forms.TextBox();
            this.txtFolderName = new System.Windows.Forms.TextBox();
            this.btnInsertDate = new System.Windows.Forms.Button();
            this.btnInsertTime = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDateFmt = new System.Windows.Forms.ComboBox();
            this.cmbTimeFmt = new System.Windows.Forms.ComboBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "現在のフォルダ";
            // 
            // txtCurrentFolder
            // 
            this.txtCurrentFolder.Location = new System.Drawing.Point(14, 24);
            this.txtCurrentFolder.Name = "txtCurrentFolder";
            this.txtCurrentFolder.ReadOnly = true;
            this.txtCurrentFolder.Size = new System.Drawing.Size(305, 19);
            this.txtCurrentFolder.TabIndex = 12;
            // 
            // txtFolderName
            // 
            this.txtFolderName.Location = new System.Drawing.Point(14, 49);
            this.txtFolderName.Name = "txtFolderName";
            this.txtFolderName.Size = new System.Drawing.Size(305, 19);
            this.txtFolderName.TabIndex = 13;
            // 
            // btnInsertDate
            // 
            this.btnInsertDate.Location = new System.Drawing.Point(14, 82);
            this.btnInsertDate.Name = "btnInsertDate";
            this.btnInsertDate.Size = new System.Drawing.Size(110, 23);
            this.btnInsertDate.TabIndex = 14;
            this.btnInsertDate.Text = "日付を挿入(&D)";
            this.btnInsertDate.UseVisualStyleBackColor = true;
            this.btnInsertDate.Click += new System.EventHandler(this.btnInsertDate_Click);
            // 
            // btnInsertTime
            // 
            this.btnInsertTime.Location = new System.Drawing.Point(14, 111);
            this.btnInsertTime.Name = "btnInsertTime";
            this.btnInsertTime.Size = new System.Drawing.Size(110, 23);
            this.btnInsertTime.TabIndex = 15;
            this.btnInsertTime.Text = "時間を挿入(&T)";
            this.btnInsertTime.UseVisualStyleBackColor = true;
            this.btnInsertTime.Click += new System.EventHandler(this.buttonInsertTime_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(144, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "日付の書式";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(144, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "時間の書式";
            // 
            // cmbDateFmt
            // 
            this.cmbDateFmt.FormattingEnabled = true;
            this.cmbDateFmt.Location = new System.Drawing.Point(213, 82);
            this.cmbDateFmt.Name = "cmbDateFmt";
            this.cmbDateFmt.Size = new System.Drawing.Size(106, 20);
            this.cmbDateFmt.TabIndex = 18;
            // 
            // cmbTimeFmt
            // 
            this.cmbTimeFmt.FormattingEnabled = true;
            this.cmbTimeFmt.Location = new System.Drawing.Point(213, 113);
            this.cmbTimeFmt.Name = "cmbTimeFmt";
            this.cmbTimeFmt.Size = new System.Drawing.Size(106, 20);
            this.cmbTimeFmt.TabIndex = 19;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(74, 149);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(89, 23);
            this.btnSubmit.TabIndex = 20;
            this.btnSubmit.Text = "OK(&ENTER)";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(169, 149);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel(ESC)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 189);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.cmbTimeFmt);
            this.Controls.Add(this.cmbDateFmt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnInsertTime);
            this.Controls.Add(this.btnInsertDate);
            this.Controls.Add(this.txtFolderName);
            this.Controls.Add(this.txtCurrentFolder);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "フォルダの作成";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCurrentFolder;
        private System.Windows.Forms.TextBox txtFolderName;
        private System.Windows.Forms.Button btnInsertDate;
        private System.Windows.Forms.Button btnInsertTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbDateFmt;
        private System.Windows.Forms.ComboBox cmbTimeFmt;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;
    }
}

