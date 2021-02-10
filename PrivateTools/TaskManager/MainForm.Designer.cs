﻿
using TaskManager.Controls;

namespace TaskManager
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
            this.PanelHeader = new System.Windows.Forms.Panel();
            this.LabelDateTime = new System.Windows.Forms.Label();
            this.PanelTop = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LabelRecentTasks = new System.Windows.Forms.Label();
            this.PanelRight = new System.Windows.Forms.Panel();
            this.BtnRemoveGroup = new System.Windows.Forms.Button();
            this.BtnAddGroup = new System.Windows.Forms.Button();
            this.LabelGroupView = new System.Windows.Forms.Label();
            this.LsvGroup = new System.Windows.Forms.ListView();
            this.ColumnGroups = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PanelLeft = new System.Windows.Forms.Panel();
            this.LabelTaskView = new System.Windows.Forms.Label();
            this.PanelButtom = new System.Windows.Forms.Panel();
            this.BtnReNameGroup = new System.Windows.Forms.Button();
            this.ColumnTaskNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DgvAllTasks = new TaskManager.Controls.TaskIchiranView();
            this.DgvRecentTasks = new TaskManager.Controls.TaskIchiranView();
            this.label1 = new System.Windows.Forms.Label();
            this.LblDisplayGroup = new System.Windows.Forms.Label();
            this.PanelHeader.SuspendLayout();
            this.PanelTop.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.PanelRight.SuspendLayout();
            this.PanelLeft.SuspendLayout();
            this.PanelButtom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvAllTasks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvRecentTasks)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.Controls.Add(this.LabelDateTime);
            this.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelHeader.Location = new System.Drawing.Point(0, 0);
            this.PanelHeader.Name = "PanelHeader";
            this.PanelHeader.Size = new System.Drawing.Size(1043, 22);
            this.PanelHeader.TabIndex = 0;
            // 
            // LabelDateTime
            // 
            this.LabelDateTime.AutoSize = true;
            this.LabelDateTime.Location = new System.Drawing.Point(846, 6);
            this.LabelDateTime.Name = "LabelDateTime";
            this.LabelDateTime.Size = new System.Drawing.Size(29, 12);
            this.LabelDateTime.TabIndex = 0;
            this.LabelDateTime.Text = "label";
            // 
            // PanelTop
            // 
            this.PanelTop.Controls.Add(this.panel2);
            this.PanelTop.Controls.Add(this.panel1);
            this.PanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelTop.Location = new System.Drawing.Point(0, 22);
            this.PanelTop.Name = "PanelTop";
            this.PanelTop.Size = new System.Drawing.Size(1043, 173);
            this.PanelTop.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.DgvRecentTasks);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1043, 145);
            this.panel2.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LabelRecentTasks);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1043, 25);
            this.panel1.TabIndex = 2;
            // 
            // LabelRecentTasks
            // 
            this.LabelRecentTasks.AutoSize = true;
            this.LabelRecentTasks.Location = new System.Drawing.Point(16, 7);
            this.LabelRecentTasks.Name = "LabelRecentTasks";
            this.LabelRecentTasks.Size = new System.Drawing.Size(88, 12);
            this.LabelRecentTasks.TabIndex = 1;
            this.LabelRecentTasks.Text = "直近のタスク一覧";
            // 
            // PanelRight
            // 
            this.PanelRight.Controls.Add(this.BtnReNameGroup);
            this.PanelRight.Controls.Add(this.BtnRemoveGroup);
            this.PanelRight.Controls.Add(this.BtnAddGroup);
            this.PanelRight.Controls.Add(this.LabelGroupView);
            this.PanelRight.Controls.Add(this.LsvGroup);
            this.PanelRight.Dock = System.Windows.Forms.DockStyle.Left;
            this.PanelRight.Location = new System.Drawing.Point(0, 0);
            this.PanelRight.Name = "PanelRight";
            this.PanelRight.Size = new System.Drawing.Size(265, 376);
            this.PanelRight.TabIndex = 2;
            // 
            // BtnRemoveGroup
            // 
            this.BtnRemoveGroup.Location = new System.Drawing.Point(171, 9);
            this.BtnRemoveGroup.Name = "BtnRemoveGroup";
            this.BtnRemoveGroup.Size = new System.Drawing.Size(23, 23);
            this.BtnRemoveGroup.TabIndex = 3;
            this.BtnRemoveGroup.Text = "-";
            this.BtnRemoveGroup.UseVisualStyleBackColor = true;
            this.BtnRemoveGroup.Click += new System.EventHandler(this.BtnRemoveGroup_Click);
            // 
            // BtnAddGroup
            // 
            this.BtnAddGroup.Location = new System.Drawing.Point(146, 9);
            this.BtnAddGroup.Name = "BtnAddGroup";
            this.BtnAddGroup.Size = new System.Drawing.Size(23, 23);
            this.BtnAddGroup.TabIndex = 2;
            this.BtnAddGroup.Text = "+";
            this.BtnAddGroup.UseVisualStyleBackColor = true;
            this.BtnAddGroup.Click += new System.EventHandler(this.BtnAddGroup_Click);
            // 
            // LabelGroupView
            // 
            this.LabelGroupView.AutoSize = true;
            this.LabelGroupView.Location = new System.Drawing.Point(16, 14);
            this.LabelGroupView.Name = "LabelGroupView";
            this.LabelGroupView.Size = new System.Drawing.Size(68, 12);
            this.LabelGroupView.TabIndex = 1;
            this.LabelGroupView.Text = "タスクグループ";
            // 
            // LsvGroup
            // 
            this.LsvGroup.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnGroups,
            this.ColumnTaskNum});
            this.LsvGroup.HideSelection = false;
            this.LsvGroup.Location = new System.Drawing.Point(12, 44);
            this.LsvGroup.Name = "LsvGroup";
            this.LsvGroup.Size = new System.Drawing.Size(240, 320);
            this.LsvGroup.TabIndex = 0;
            this.LsvGroup.UseCompatibleStateImageBehavior = false;
            // 
            // ColumnGroups
            // 
            this.ColumnGroups.Text = "グループ";
            this.ColumnGroups.Width = 165;
            // 
            // PanelLeft
            // 
            this.PanelLeft.Controls.Add(this.LblDisplayGroup);
            this.PanelLeft.Controls.Add(this.label1);
            this.PanelLeft.Controls.Add(this.LabelTaskView);
            this.PanelLeft.Controls.Add(this.DgvAllTasks);
            this.PanelLeft.Dock = System.Windows.Forms.DockStyle.Right;
            this.PanelLeft.Location = new System.Drawing.Point(271, 0);
            this.PanelLeft.Name = "PanelLeft";
            this.PanelLeft.Size = new System.Drawing.Size(772, 376);
            this.PanelLeft.TabIndex = 3;
            // 
            // LabelTaskView
            // 
            this.LabelTaskView.AutoSize = true;
            this.LabelTaskView.Location = new System.Drawing.Point(11, 14);
            this.LabelTaskView.Name = "LabelTaskView";
            this.LabelTaskView.Size = new System.Drawing.Size(54, 12);
            this.LabelTaskView.TabIndex = 2;
            this.LabelTaskView.Text = "タスク一覧";
            // 
            // PanelButtom
            // 
            this.PanelButtom.Controls.Add(this.PanelRight);
            this.PanelButtom.Controls.Add(this.PanelLeft);
            this.PanelButtom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelButtom.Location = new System.Drawing.Point(0, 195);
            this.PanelButtom.Name = "PanelButtom";
            this.PanelButtom.Size = new System.Drawing.Size(1043, 376);
            this.PanelButtom.TabIndex = 4;
            // 
            // BtnReNameGroup
            // 
            this.BtnReNameGroup.Location = new System.Drawing.Point(198, 9);
            this.BtnReNameGroup.Name = "BtnReNameGroup";
            this.BtnReNameGroup.Size = new System.Drawing.Size(54, 23);
            this.BtnReNameGroup.TabIndex = 4;
            this.BtnReNameGroup.Text = "変更";
            this.BtnReNameGroup.UseVisualStyleBackColor = true;
            this.BtnReNameGroup.Click += new System.EventHandler(this.BtnReNameGroup_Click);
            // 
            // ColumnTaskNum
            // 
            this.ColumnTaskNum.Text = "";
            this.ColumnTaskNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ColumnTaskNum.Width = 50;
            // 
            // DgvAllTasks
            // 
            this.DgvAllTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvAllTasks.Location = new System.Drawing.Point(9, 44);
            this.DgvAllTasks.Name = "DgvAllTasks";
            this.DgvAllTasks.RowTemplate.Height = 21;
            this.DgvAllTasks.Size = new System.Drawing.Size(754, 320);
            this.DgvAllTasks.TabIndex = 1;
            // 
            // DgvRecentTasks
            // 
            this.DgvRecentTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvRecentTasks.Location = new System.Drawing.Point(12, 6);
            this.DgvRecentTasks.Name = "DgvRecentTasks";
            this.DgvRecentTasks.RowTemplate.Height = 21;
            this.DgvRecentTasks.Size = new System.Drawing.Size(754, 130);
            this.DgvRecentTasks.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(146, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "表示中グループ";
            // 
            // LblDisplayGroup
            // 
            this.LblDisplayGroup.AutoSize = true;
            this.LblDisplayGroup.Location = new System.Drawing.Point(231, 14);
            this.LblDisplayGroup.Name = "LblDisplayGroup";
            this.LblDisplayGroup.Size = new System.Drawing.Size(35, 12);
            this.LblDisplayGroup.TabIndex = 4;
            this.LblDisplayGroup.Text = "label2";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 571);
            this.Controls.Add(this.PanelButtom);
            this.Controls.Add(this.PanelTop);
            this.Controls.Add(this.PanelHeader);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "タスク管理ツール";
            this.PanelHeader.ResumeLayout(false);
            this.PanelHeader.PerformLayout();
            this.PanelTop.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.PanelRight.ResumeLayout(false);
            this.PanelRight.PerformLayout();
            this.PanelLeft.ResumeLayout(false);
            this.PanelLeft.PerformLayout();
            this.PanelButtom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvAllTasks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvRecentTasks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelHeader;
        private System.Windows.Forms.Label LabelDateTime;
        private System.Windows.Forms.Panel PanelTop;
        private System.Windows.Forms.Panel PanelRight;
        private System.Windows.Forms.Panel PanelLeft;
        private System.Windows.Forms.Panel PanelButtom;
        private System.Windows.Forms.Label LabelRecentTasks;
        private TaskIchiranView DgvRecentTasks;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LabelGroupView;
        private System.Windows.Forms.ListView LsvGroup;
        private TaskIchiranView DgvAllTasks;
        private System.Windows.Forms.Label LabelTaskView;
        private System.Windows.Forms.ColumnHeader ColumnGroups;
        private System.Windows.Forms.Button BtnRemoveGroup;
        private System.Windows.Forms.Button BtnAddGroup;
        private System.Windows.Forms.Button BtnReNameGroup;
        private System.Windows.Forms.ColumnHeader ColumnTaskNum;
        private System.Windows.Forms.Label LblDisplayGroup;
        private System.Windows.Forms.Label label1;
    }
}

