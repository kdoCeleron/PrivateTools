using System.Linq;

namespace TaskManager.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    using MyTools.Common;
    using TaskManager.Configration;
    using TaskManager.Controls;
    using TaskManager.Data;

    /// <summary>
    /// タスク編集画面
    /// </summary>
    public partial class TaskEditForm : SubWindowBase
    {
        /// <summary>
        /// 編集対象
        /// </summary>
        private TaskItem target;

        /// <summary>
        /// 添付ファイル追加リスト
        /// </summary>
        private List<AttachedFileInfo> attachFileAddList = new List<AttachedFileInfo>();

        /// <summary>
        /// 添付ファイル削除リスト
        /// </summary>
        private List<AttachedFileInfo> attachFileDeleteList = new List<AttachedFileInfo>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskEditForm()
        {
            this.InitializeComponent();

            this.Load += this.OnLoad;

            this.DtpLimit.Format = DateTimePickerFormat.Custom;
            this.DtpLimit.CustomFormat = "yyyy年 MM月 dd日 HH:mm";
            this.CmbGroup.DisplayMember = "Name";
            this.CmbGroup.ValueMember = "Name";

            this.TxtTitle.ImeMode = ImeMode.Hiragana;
            this.TxtMemo.ImeMode = ImeMode.Hiragana;

            this.CmbAttachFiles.AllowDrop = true;
            this.CmbAttachFiles.DragDrop += this.CmbAttachFiles_DragDrop;
            this.CmbAttachFiles.DragEnter += this.CmbAttachFiles_DragEnter;
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="groupInfo">親グループ情報</param>
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
                    // 親の指定が無い場合は、未分類グループ
                    taskItem.Group = TaskGroupInfo.GetDefaultGroup().Key;
                }
            }

            this.CmbGroup.SelectedItem = ResourceManager.Instance.GetGroupInfo(taskItem.Group);
            this.TxtTitle.Text = taskItem.Title;
            this.DtpLimit.Value = taskItem.DateTimeLimit;
            this.TxtMemo.Text = taskItem.Memo;
            
            this.CmbAttachFiles.DisplayMember = "DisplayName";
            foreach (var info in taskItem.AttachFile)
            {
                this.CmbAttachFiles.Items.Add(info);
            }


            this.btnOpenDirAttachedFiles.Enabled = false;
            if (taskItem.AttachFile.Any())
            {
                this.btnOpenDirAttachedFiles.Enabled = true;
            }

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

            var msgBuilder = new MessageBuilder();

            if (string.IsNullOrEmpty(this.TxtTitle.Text))
            {
                msgBuilder.Add(MessageKind.Error, "タイトルを入力してください");

                this.TxtTitle.BackColor = Color.Red;
                canUpdate = false;
            }

            if (string.IsNullOrEmpty(this.DtpLimit.Text))
            {
                msgBuilder.Add(MessageKind.Error, "期限を入力してください");

                this.DtpLimit.BackColor = Color.Red;
                canUpdate = false;
            }

            if (!canUpdate)
            {
                var errMsg = msgBuilder.CreateMessage();
                MessageBox.Show(errMsg);
                return;
            }

            if (!this.target.Group.Equals(this.CmbGroup.SelectedItem))
            {
                // グループ管理の差し替え
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

            // 添付ファイルの取得
            {
                // 削除ファイルの処理
                var destattachList = new List<AttachedFileInfo>();
                destattachList.AddRange(this.target.AttachFile);

                foreach (var info in this.attachFileDeleteList)
                {
                    var path = info.FilePath;
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    if (destattachList.Contains(info))
                    {
                        destattachList.Remove(info);
                    }
                }

                // 追加ファイルの処理
                foreach (var info in this.attachFileAddList)
                {
                    destattachList.Add(info);

                    File.Copy(info.OrgFilePath, info.FilePath, true);
                }

                this.target.AttachFile.Clear();
                this.target.AttachFile.AddRange(destattachList);
            }

            this.CloseWindow(SubWindowResult.Submit);
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
            var limitEdited = this.DtpLimit.Value.Year != this.target.DateTimeLimit.Year
                                  || this.DtpLimit.Value.Month != this.target.DateTimeLimit.Month
                                  || this.DtpLimit.Value.Day != this.target.DateTimeLimit.Day
                                  || this.DtpLimit.Value.Hour != this.target.DateTimeLimit.Hour 
                                  || this.DtpLimit.Value.Minute != this.target.DateTimeLimit.Minute;
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

        /// <summary>
        /// 添付ファイルの追加ボタン押下時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void BtnTenpuAdd_Click(object sender, EventArgs e)
        {
            var selected = this.CmbAttachFiles.Text;
            if (!File.Exists(selected))
            {
                MessageBox.Show("指定したファイルが見つかりません。");
                return;
            }

            var selectedItem = this.CmbAttachFiles.SelectedItem as AttachedFileInfo;
            if (selectedItem != null)
            {
                MessageBox.Show("追加時は、テキスト入力欄にパスを入力もしくはドラッグアンドドロップした後に実施してください。\n(リスト内のデータ選択時は無効です)");
                return;
            }

            var filePath = selected;

            var item = new AttachedFileInfo();
            if (!Directory.Exists(Config.Instance.AttachedFileRootDir))
            {
                Directory.CreateDirectory(Config.Instance.AttachedFileRootDir);
            }

            var taskKey = this.target.Key.Key;
            var taskDir = Path.Combine(Config.Instance.AttachedFileRootDir, taskKey);
            if (!Directory.Exists(taskDir))
            {
                Directory.CreateDirectory(taskDir);
            }

            // item.FilePath = Path.Combine(taskDir, info.DisplayName);
            item.OrgFilePath = filePath;

            var curFileNameList = new List<string>();
            foreach (var cmbItem in this.CmbAttachFiles.Items)
            {
                var info = cmbItem as AttachedFileInfo;
                if (info != null)
                {
                    curFileNameList.Add(info.DisplayName);
                }
            }

            var fileName = Path.GetFileName(selected);
            var destFilePath = Path.Combine(taskDir, fileName);
            if (curFileNameList.Contains(fileName))
            {
                var ret = MessageBox.Show("同名のファイルを添付済みです。別名で保存しますか？\nYes:リネームして保存\nNo:上書き保存", "確認", MessageBoxButtons.YesNoCancel);
                if (ret == DialogResult.Yes)
                {
                    // コピーで保存
                    var copyFilePath = filePath;
                    int offset = 0;
                    var tmpFileName = fileName;
                    var extension = Path.GetExtension(tmpFileName);
                    var removeExtension = Path.GetFileNameWithoutExtension(tmpFileName);
                    while (true)
                    {
                        var append = removeExtension + string.Format("{0:D03}", offset);
                        tmpFileName = append;
                        if (!string.IsNullOrEmpty(extension))
                        {
                            tmpFileName = tmpFileName + "." + extension;
                        }

                        if (!curFileNameList.Contains(tmpFileName))
                        {
                            // ファイルが存在しなくなるまでリネーム
                            copyFilePath = Path.Combine(taskDir, tmpFileName);
                            break;
                        }

                        offset++;
                    }

                    destFilePath = copyFilePath;
                }
                else if (ret == DialogResult.No)
                {
                    // 上書き保存
                }
                else
                {
                    return;
                }
            }

            item.FilePath = destFilePath;

            this.CmbAttachFiles.Items.Add(item);

            this.attachFileAddList.Add(item);

            this.CmbAttachFiles.Text = string.Empty;
        }

        /// <summary>
        /// 添付ファイルの削除ボタン押下時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void BtnTenpuDelete_Click(object sender, EventArgs e)
        {
            var selected = this.CmbAttachFiles.SelectedItem as AttachedFileInfo;
            if (selected == null)
            {
                MessageBox.Show("指定したファイルは、未追加です。リストより選択後実施してください。");
                return;
            }

            if (this.CmbAttachFiles.Items.Contains(selected))
            {
                this.CmbAttachFiles.Items.Remove(selected);
            }

            this.attachFileDeleteList.Add(selected);
            if (!this.target.AttachFile.Contains(selected))
            {
                // 元データに含まれていない場合は、
                // 追加->削除されたデータであるため、追加リストからも削除しておく
                if (this.attachFileAddList.Contains(selected))
                {
                    this.attachFileAddList.Remove(selected);
                }
            }

            this.CmbAttachFiles.Text = string.Empty;
        }

        /// <summary>
        /// ルートフォルダへのドラッグアンドドロップ時のイベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void CmbAttachFiles_DragDrop(object sender, DragEventArgs e)
        {
            var files = this.GetDragAndGropFileList(e);
            if (files.Any())
            {
                if (File.Exists(files[0]))
                {
                    this.CmbAttachFiles.Text = files[0];
                }
            }
        }

        /// <summary>
        /// ルートフォルダへのドラッグ入力時のイベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void CmbAttachFiles_DragEnter(object sender, DragEventArgs e)
        {
            this.AllowDropWhenDragEnter(e);
        }

        /// <summary>
        /// ドラッグアンドドロップされたファイルのパスを取得する
        /// </summary>
        /// <param name="eventArgs">イベント引数</param>
        /// <returns>取得結果</returns>
        private List<string> GetDragAndGropFileList(DragEventArgs eventArgs)
        {
            var files = (string[])eventArgs.Data.GetData(DataFormats.FileDrop, false);
            return files.ToList();
        }

        /// <summary>
        /// ドラッグ入力時にドロップを許容する
        /// </summary>
        /// <param name="eventArgs">イベント引数</param>
        private void AllowDropWhenDragEnter(DragEventArgs eventArgs)
        {
            if (eventArgs.Data.GetDataPresent(DataFormats.FileDrop))
            {
                eventArgs.Effect = DragDropEffects.All;
            }
            else
            {
                eventArgs.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// 添付ファイルのフォルダを開くボタン押下時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnOpenDirAttachedFiles_Click(object sender, EventArgs e)
        {
            var taskKey = this.target.Key.Key;
            var taskDir = Path.Combine(Config.Instance.AttachedFileRootDir, taskKey);
            if (!Directory.Exists(taskDir))
            {
                return;
            }

            System.Diagnostics.Process.Start(taskDir);
        }
    }
}
