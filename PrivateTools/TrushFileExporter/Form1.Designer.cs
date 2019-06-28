namespace TrushFIleExporter
{
    partial class Form1
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
            this.txtRootPath = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.grpTFS = new System.Windows.Forms.GroupBox();
            this.chkIsTfsConnect = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.grpTFS.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtRootPath
            // 
            this.txtRootPath.Location = new System.Drawing.Point(13, 13);
            this.txtRootPath.Name = "txtRootPath";
            this.txtRootPath.Size = new System.Drawing.Size(386, 19);
            this.txtRootPath.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(148, 38);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(142, 23);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "不要ファイルリスト出力";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(296, 38);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(101, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "不要ファイル削除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // grpTFS
            // 
            this.grpTFS.Controls.Add(this.txtPass);
            this.grpTFS.Controls.Add(this.label3);
            this.grpTFS.Controls.Add(this.txtUser);
            this.grpTFS.Controls.Add(this.txtUrl);
            this.grpTFS.Controls.Add(this.label2);
            this.grpTFS.Controls.Add(this.label1);
            this.grpTFS.Controls.Add(this.chkIsTfsConnect);
            this.grpTFS.Location = new System.Drawing.Point(12, 67);
            this.grpTFS.Name = "grpTFS";
            this.grpTFS.Size = new System.Drawing.Size(394, 154);
            this.grpTFS.TabIndex = 3;
            this.grpTFS.TabStop = false;
            this.grpTFS.Text = "TFS連携";
            // 
            // chkIsTfsConnect
            // 
            this.chkIsTfsConnect.AutoSize = true;
            this.chkIsTfsConnect.Location = new System.Drawing.Point(15, 21);
            this.chkIsTfsConnect.Name = "chkIsTfsConnect";
            this.chkIsTfsConnect.Size = new System.Drawing.Size(69, 16);
            this.chkIsTfsConnect.TabIndex = 0;
            this.chkIsTfsConnect.Text = "TFS連携";
            this.chkIsTfsConnect.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Url";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "User";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(51, 43);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(253, 19);
            this.txtUrl.TabIndex = 3;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(51, 76);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(131, 19);
            this.txtUser.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Pass";
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(51, 110);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(129, 19);
            this.txtPass.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 244);
            this.Controls.Add(this.grpTFS);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.txtRootPath);
            this.Name = "Form1";
            this.Text = "TrushFileExporter";
            this.grpTFS.ResumeLayout(false);
            this.grpTFS.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRootPath;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox grpTFS;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkIsTfsConnect;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Label label3;
    }
}

