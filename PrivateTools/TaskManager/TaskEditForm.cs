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
using TaskManager.Data;

namespace TaskManager
{
    public partial class TaskEditForm : Form
    {
        private TaskItem target;

        public event EventHandler<TaskItem> UpdateEvent;

        public TaskEditForm()
        {
            InitializeComponent();

            this.Load += OnLoad;

            this.DtpLimit.Format = DateTimePickerFormat.Custom;
            this.DtpLimit.CustomFormat = "yyyy年 MM月 dd日 HH:mm:ss";
            this.CmbGroup.DisplayMember = "Name";
            this.CmbGroup.ValueMember = "Name";

            this.TxtTitle.ImeMode = ImeMode.Hiragana;
            this.TxtMemo.ImeMode = ImeMode.Hiragana;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            var win = this.Owner;
            if (win != null)
            {
                var winPos = win.Location;
                var winWidth = this.Owner.Width;
                var winHeight = this.Owner.Height;
                var winCenterPosX = winPos.X + (winWidth / 2);
                var winCenterPosY = winPos.Y + (winHeight / 2);

                var width = this.Width;
                var height = this.Height;

                var posX = winCenterPosX - (width / 2);
                var posY = winCenterPosY - (height / 2);

                this.Location = new Point(posX, posY);
            }
        }

        public bool Initialize(TaskGroupInfo groupInfo, TaskItem taskItem)
        {
            var list = ResourceManager.Instance.TaskGroupList.ToArray();
            foreach (var item in list)
            {
                this.CmbGroup.Items.Add(item);
            }

            if (taskItem.Group == null)
            {
                // 新規の場合は、表示中グループにする
                taskItem.Group = groupInfo;
            }

            this.CmbGroup.SelectedItem = taskItem.Group;
            this.TxtTitle.Text = taskItem.Title;
            this.DtpLimit.Value = taskItem.DateTimeLimit;
            this.TxtMemo.Text = taskItem.Memo;
            foreach (var file in taskItem.AttachFile)
            {
                var fileName = Path.GetFileName(file);
                this.CmbAttachFiles.Items.Add(fileName);
            }

            this.target = taskItem;

            return true;
        }

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
                // TODO:複数ファイル対応
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
                if (string.IsNullOrEmpty(this.CmbGroup.Text))
                {
                    this.target.Group = TaskGroupInfo.GetDefaultGroup();
                }
                else
                {
                    this.target.Group = this.CmbGroup.SelectedItem as TaskGroupInfo;
                }

                this.target.Title = this.TxtTitle.Text;
                this.target.DateTimeLimit = this.DtpLimit.Value;
                this.target.Memo = this.TxtMemo.Text;

                if (isAppendFile)
                {
                    this.target.AttachFileOrg.Add(file);

                    // TODO:コピー処理
                    // savefilename
                }

                if (this.UpdateEvent != null)
                {
                    this.UpdateEvent(this, this.target);
                }

                this.Close();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            var groupEdited = this.target.Group != this.CmbGroup.SelectedItem;
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

            this.Close();
        }
    }
}
