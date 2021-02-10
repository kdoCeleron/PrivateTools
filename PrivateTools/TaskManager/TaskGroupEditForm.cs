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
using TaskManager.Data;

namespace TaskManager
{
    public partial class TaskGroupEditForm : Form
    {
        private TaskGroupInfo target;

        private bool isAdd;

        public TaskGroupEditForm()
        {
            InitializeComponent();

            this.Load += OnLoad;
            this.CmbParentGroup.DisplayMember = "Name";
            this.CmbParentGroup.ValueMember = "Name";
        }

        public void Initialize(TaskGroupInfo group, bool isAddOperation, TaskGroupInfo parent)
        {
            this.isAdd = isAddOperation;

            foreach (var item in ResourceManager.Instance.TaskGroupList)
            {
                this.CmbParentGroup.Items.Add(item);
            }

            if (parent != null)
            {
                this.CmbParentGroup.SelectedItem = parent;
            }

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

        private void OnLoad(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private void BtnKakutei_Click(object sender, EventArgs e)
        {
            this.CmbParentGroup.BackColor = Color.White;
            this.TxtGroupName.BackColor = Color.White;

            var canUpdate = true;
            TaskGroupInfo parent;
            if (string.IsNullOrEmpty(this.CmbParentGroup.Text))
            {
                // 親なし
                parent = null;
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

                this.Close();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
