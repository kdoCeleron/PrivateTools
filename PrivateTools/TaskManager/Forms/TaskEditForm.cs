﻿using TaskManager.Interfaces;

namespace TaskManager.Forms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;
    using TaskManager.Controls;
    using TaskManager.Data;

    /// <summary>
    /// タスク編集画面
    /// </summary>
    public partial class TaskEditForm : SubWindowBase, ICanShowFromTaskTray
    {
        /// <summary>
        /// 編集対象
        /// </summary>
        private TaskItem target;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskEditForm()
        {
            this.InitializeComponent();

            this.Load += this.OnLoad;

            this.DtpLimit.Format = DateTimePickerFormat.Custom;
            this.DtpLimit.CustomFormat = "yyyy年 MM月 dd日 HH:mm:ss";
            this.CmbGroup.DisplayMember = "Name";
            this.CmbGroup.ValueMember = "Name";

            this.TxtTitle.ImeMode = ImeMode.Hiragana;
            this.TxtMemo.ImeMode = ImeMode.Hiragana;
        }

        /// <summary>
        /// 更新時イベント
        /// </summary>
        public event EventHandler<TaskItem> UpdateEvent;

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
                return ViewKind.TaskEditView;
            }
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="groupInfo">グループ情報</param>
        /// <param name="taskItem">タスク情報</param>
        /// <returns>結果</returns>
        public bool Initialize(TaskGroupInfo groupInfo, TaskItem taskItem)
        {
            var list = ResourceManager.Instance.GetGroupListExcludeRoot().ToArray();
            foreach (var item in list)
            {
                this.CmbGroup.Items.Add(item);
            }

            if (taskItem.Group == null)
            {
                if (groupInfo != null)
                {
                    // 新規の場合は、表示中グループにする
                    taskItem.Group = groupInfo.Key;
                }
                else
                {
                    taskItem.Group = TaskGroupInfo.GetDefaultGroup().Key;
                }
            }

            this.CmbGroup.SelectedItem = ResourceManager.Instance.GetGroupInfo(taskItem.Group);
            this.TxtTitle.Text = taskItem.Title;
            this.DtpLimit.Value = taskItem.DateTimeLimit;
            this.TxtMemo.Text = taskItem.Memo;

            this.CmbAttachFiles.Enabled = false;

            this.target = taskItem;

            return true;
        }

        /// <summary>
        /// 画面ロード時処理
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void OnLoad(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        /// <summary>
        /// 更新ボタン押下時処理
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            var canUpdate = true;
            this.TxtTitle.BackColor = Color.White;
            this.DtpLimit.BackColor = Color.White;
            this.CmbAttachFiles.BackColor = Color.White;

            if (string.IsNullOrEmpty(this.TxtTitle.Text))
            {
                // MessageBox.Show("タイトルを入力してください");
                this.TxtTitle.BackColor = Color.Red;
                canUpdate = false;
            }

            if (string.IsNullOrEmpty(this.DtpLimit.Text))
            {
                // MessageBox.Show("期限を入力してください");
                this.DtpLimit.BackColor = Color.Red;
                canUpdate = false;
            }

            var file = this.CmbAttachFiles.Text;
            var isAppendFile = false;
            var saveFileName = string.Empty;
            if (!string.IsNullOrEmpty(file))
            {
                // 複数ファイル対応
                // UIにファイル追加/削除ボタンでもよいか
                if (!File.Exists(file))
                {
                    // MessageBox.Show("該当ファイルが存在しません。");
                    this.CmbAttachFiles.BackColor = Color.Red;
                    canUpdate = false;
                }

                if (canUpdate)
                {
                    var fileName = Path.GetFileName(file);
                    var curFileNameList = new List<string>();
                    foreach (var tmp in this.target.AttachFile)
                    {
                        curFileNameList.Add(Path.GetFileName(tmp));
                    }

                    if (curFileNameList.Contains(fileName))
                    {
                        var ret = MessageBox.Show("同名のファイルを添付済みです。追加して保存しますか？\nYes:リネームして保存\nNo:保存しない", "確認", MessageBoxButtons.YesNo);
                        if (ret == DialogResult.Yes)
                        {
                            var extension = Path.GetExtension(fileName);
                            var removeExtension = Path.GetFileNameWithoutExtension(fileName);
                            var append = removeExtension + "_コピー";
                            fileName = append + "." + extension;

                            isAppendFile = true;
                        }
                    }

                    saveFileName = fileName;
                }
            }

            if (canUpdate)
            {   
                if (!this.target.Group.Equals(this.CmbGroup.SelectedItem))
                {
                    var prevParent = ResourceManager.Instance.GetGroupInfo(this.target.Group);
                    if (prevParent != null)
                    {
                        if (prevParent.ChildTaskItems.Contains(this.target))
                        {
                            prevParent.ChildTaskItems.Remove(this.target);
                        }
                    }

                    if (string.IsNullOrEmpty(this.CmbGroup.Text) || this.CmbGroup.SelectedItem == null)
                    {
                        this.target.Group = TaskGroupInfo.GetDefaultGroup().Key;
                    }
                    else
                    {
                        this.target.Group = ((TaskGroupInfo)this.CmbGroup.SelectedItem).Key;
                    }

                    var parent = ResourceManager.Instance.GetGroupInfo(this.target.Group);
                    parent.ChildTaskItems.Add(this.target);
                }

                this.target.Title = this.TxtTitle.Text;
                this.target.DateTimeLimit = this.DtpLimit.Value;
                this.target.Memo = this.TxtMemo.Text;

                if (isAppendFile)
                {
                    this.target.AttachFileOrg.Add(file);

                    // コピー処理
                    // savefilename
                }

                if (this.UpdateEvent != null)
                {
                    this.UpdateEvent(this, this.target);
                }

                this.CloseWindow(SubWindowResult.Submit);
            }
        }

        /// <summary>
        /// キャンセルボタン押下時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            var groupEdited = this.target.Group.Equals(this.CmbGroup.SelectedItem);
            var titleEdited = this.target.Title != this.TxtTitle.Text;
            var limitEdited = this.DtpLimit.Value.ToString() != this.target.DateTimeLimit.ToString();
            var memoEdited = this.target.Memo != this.TxtMemo.Text;
            var fileEdited = !string.IsNullOrEmpty(this.CmbAttachFiles.Text);

            if (groupEdited || titleEdited || limitEdited || memoEdited || fileEdited)
            {
                var ret = MessageBox.Show("入力内容に変更があります。破棄しますか？", "確認", MessageBoxButtons.YesNo);
                if (ret != DialogResult.Yes)
                {
                    return;
                }
            }

            this.CloseWindow(SubWindowResult.Cancel);
        }
    }
}
