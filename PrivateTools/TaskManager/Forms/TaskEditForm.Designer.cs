namespace TaskManager.Forms
{
    partial class TaskEditForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.CmbGroup = new System.Windows.Forms.ComboBox();
            this.TxtTitle = new System.Windows.Forms.TextBox();
            this.DtpLimit = new System.Windows.Forms.DateTimePicker();
            this.TxtMemo = new System.Windows.Forms.TextBox();
            this.CmbAttachFiles = new System.Windows.Forms.ComboBox();
            this.BtnUpdate = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "グループ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "タイトル";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "期限";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "メモ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 244);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "添付ファイル";
            // 
            // CmbGroup
            // 
            this.CmbGroup.FormattingEnabled = true;
            this.CmbGroup.Location = new System.Drawing.Point(87, 16);
            this.CmbGroup.Name = "CmbGroup";
            this.CmbGroup.Size = new System.Drawing.Size(266, 20);
            this.CmbGroup.TabIndex = 5;
            // 
            // TxtTitle
            // 
            this.TxtTitle.Location = new System.Drawing.Point(87, 60);
            this.TxtTitle.Name = "TxtTitle";
            this.TxtTitle.Size = new System.Drawing.Size(266, 19);
            this.TxtTitle.TabIndex = 6;
            // 
            // DtpLimit
            // 
            this.DtpLimit.Location = new System.Drawing.Point(87, 95);
            this.DtpLimit.Name = "DtpLimit";
            this.DtpLimit.Size = new System.Drawing.Size(266, 19);
            this.DtpLimit.TabIndex = 7;
            // 
            // TxtMemo
            // 
            this.TxtMemo.Location = new System.Drawing.Point(87, 130);
            this.TxtMemo.Multiline = true;
            this.TxtMemo.Name = "TxtMemo";
            this.TxtMemo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtMemo.Size = new System.Drawing.Size(266, 97);
            this.TxtMemo.TabIndex = 8;
            // 
            // CmbAttachFiles
            // 
            this.CmbAttachFiles.FormattingEnabled = true;
            this.CmbAttachFiles.Location = new System.Drawing.Point(87, 241);
            this.CmbAttachFiles.Name = "CmbAttachFiles";
            this.CmbAttachFiles.Size = new System.Drawing.Size(266, 20);
            this.CmbAttachFiles.TabIndex = 9;
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Location = new System.Drawing.Point(197, 283);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(75, 23);
            this.BtnUpdate.TabIndex = 10;
            this.BtnUpdate.Text = "更新";
            this.BtnUpdate.UseVisualStyleBackColor = true;
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(278, 283);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 11;
            this.BtnCancel.Text = "キャンセル";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // TaskEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 319);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnUpdate);
            this.Controls.Add(this.CmbAttachFiles);
            this.Controls.Add(this.TxtMemo);
            this.Controls.Add(this.DtpLimit);
            this.Controls.Add(this.TxtTitle);
            this.Controls.Add(this.CmbGroup);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TaskEditForm";
            this.Text = "タスク編集画面";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox CmbGroup;
        private System.Windows.Forms.TextBox TxtTitle;
        private System.Windows.Forms.DateTimePicker DtpLimit;
        private System.Windows.Forms.TextBox TxtMemo;
        private System.Windows.Forms.ComboBox CmbAttachFiles;
        private System.Windows.Forms.Button BtnUpdate;
        private System.Windows.Forms.Button BtnCancel;
    }
}