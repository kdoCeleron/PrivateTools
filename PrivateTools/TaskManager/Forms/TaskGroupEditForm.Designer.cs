namespace TaskManager.Forms
{
    partial class TaskGroupEditForm
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
            this.CmbParentGroup = new System.Windows.Forms.ComboBox();
            this.TxtGroupName = new System.Windows.Forms.TextBox();
            this.BtnKakutei = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "親グループ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "グループ名";
            // 
            // CmbParentGroup
            // 
            this.CmbParentGroup.FormattingEnabled = true;
            this.CmbParentGroup.Location = new System.Drawing.Point(73, 7);
            this.CmbParentGroup.Name = "CmbParentGroup";
            this.CmbParentGroup.Size = new System.Drawing.Size(207, 20);
            this.CmbParentGroup.TabIndex = 2;
            // 
            // TxtGroupName
            // 
            this.TxtGroupName.Location = new System.Drawing.Point(73, 41);
            this.TxtGroupName.Name = "TxtGroupName";
            this.TxtGroupName.Size = new System.Drawing.Size(207, 19);
            this.TxtGroupName.TabIndex = 3;
            // 
            // BtnKakutei
            // 
            this.BtnKakutei.Location = new System.Drawing.Point(122, 76);
            this.BtnKakutei.Name = "BtnKakutei";
            this.BtnKakutei.Size = new System.Drawing.Size(75, 23);
            this.BtnKakutei.TabIndex = 4;
            this.BtnKakutei.Text = "編集";
            this.BtnKakutei.UseVisualStyleBackColor = true;
            this.BtnKakutei.Click += new System.EventHandler(this.BtnKakutei_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(203, 76);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 5;
            this.BtnCancel.Text = "キャンセル";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // TaskGroupEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 106);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnKakutei);
            this.Controls.Add(this.TxtGroupName);
            this.Controls.Add(this.CmbParentGroup);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TaskGroupEditForm";
            this.Text = "タスクグループ編集";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CmbParentGroup;
        private System.Windows.Forms.TextBox TxtGroupName;
        private System.Windows.Forms.Button BtnKakutei;
        private System.Windows.Forms.Button BtnCancel;
    }
}