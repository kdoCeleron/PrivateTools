namespace TaskManager.Forms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;
    using TaskManager.Controls;
    using TaskManager.Data;

    /// <summary>
    /// グループ編集画面
    /// </summary>
    public partial class TaskGroupEditForm : SubWindowBase
    {
        /// <summary>
        /// 編集対象
        /// </summary>
        private TaskGroupInfo target;

        /// <summary>
        /// 追加画面かどうか
        /// </summary>
        private bool isAdd;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskGroupEditForm()
        {
            this.InitializeComponent();

            this.Load += this.OnLoad;
            this.CmbParentGroup.DisplayMember = "Name";
            this.CmbParentGroup.ValueMember = "Name";

            this.TxtGroupName.ImeMode = ImeMode.Hiragana;
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="group">編集対象のグループ情報</param>
        /// <param name="isAddOperation">追加画面かどうか</param>
        /// <param name="parent">親グループ</param>
        public void Initialize(TaskGroupInfo group, bool isAddOperation, TaskGroupInfo parent)
        {
            this.isAdd = isAddOperation;
            
            foreach (var item in ResourceManager.Instance.GetGroupList())
            {
                this.CmbParentGroup.Items.Add(item);
            }

            this.CmbParentGroup.Enabled = false;
            this.CmbParentGroup.SelectedItem = TaskGroupInfo.GetRootGroup();

            if (group != null)
            {
                this.target = group;

                // 自分を削除
                if (this.CmbParentGroup.Items.Contains(group))
                {
                    this.CmbParentGroup.Items.Remove(group);
                }

                if (group.ParentGroup != null)
                {
                    this.CmbParentGroup.SelectedItem = group.ParentGroup;
                }

                this.TxtGroupName.Text = group.Name;
            }
        }

        /// <summary>
        /// 画面ロード時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void OnLoad(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        /// <summary>
        /// 確定ボタン押下時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void BtnKakutei_Click(object sender, EventArgs e)
        {
            this.CmbParentGroup.BackColor = Color.White;
            this.TxtGroupName.BackColor = Color.White;

            var canUpdate = true;
            TaskGroupInfo parent;
            if (string.IsNullOrEmpty(this.CmbParentGroup.Text))
            {
                // トップの親
                parent = TaskGroupInfo.GetRootGroup();
            }
            else
            {
                var selected = this.CmbParentGroup.SelectedItem as TaskGroupInfo;
                if (selected == null)
                {
                    this.CmbParentGroup.BackColor = Color.Red;
                    canUpdate = false;
                    parent = null;
                }
                else
                {
                    parent = selected;
                }
            }

            if (string.IsNullOrEmpty(this.TxtGroupName.Text))
            {
                this.TxtGroupName.BackColor = Color.Red;
                canUpdate = false;
            }

            if (canUpdate)
            {
                if (this.isAdd)
                {
                    ResourceManager.Instance.AddTaskGroup(this.TxtGroupName.Text, parent);
                }
                else
                {
                    ResourceManager.Instance.EditTaskGroup(this.target, this.TxtGroupName.Text, parent);
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
            this.CloseWindow(SubWindowResult.Cancel);
        }
    }
}
