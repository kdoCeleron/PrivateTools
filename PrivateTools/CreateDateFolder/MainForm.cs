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

namespace CreateDateFolder
{
    /// <summary>
    /// フォルダ作成画面
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            this.KeyPreview = true;
            this.Load += OnLoad;
            this.KeyDown += OnKeyDown;

            this.cmbDateFmt.Items.Add("yyyyMMdd");
            this.cmbDateFmt.Items.Add("yyyy-MM-dd");
            this.cmbDateFmt.Items.Add("yyyyMM");
            this.cmbDateFmt.Items.Add("yyyy-MM");
            this.cmbDateFmt.SelectedIndex = 0;
            this.cmbDateFmt.DropDownStyle = ComboBoxStyle.DropDownList;

            this.cmbTimeFmt.Items.Add("HHmm");
            this.cmbTimeFmt.Items.Add("HHmmss");
            this.cmbTimeFmt.Items.Add("HH-mm");
            this.cmbTimeFmt.Items.Add("HH-mm-ss");
            this.cmbTimeFmt.SelectedIndex = 0;
            this.cmbTimeFmt.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        
        /// <summary>
        /// キー入力のフック
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Control && e.KeyCode == Keys.Enter)
            {
                // OKボタン
                this.btnSubmit.PerformClick();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                // キャンセルボタン
                this.btnCancel.PerformClick();
            }
        }

        /// <summary>
        /// 画面ロード時のイベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void OnLoad(object sender, EventArgs e)
        {
            var baseFolder = Environment.CurrentDirectory;
            var args = Environment.GetCommandLineArgs();
            if (args.Length >= 2)
            {
                baseFolder = args[1];
            }

            this.txtCurrentFolder.Text = baseFolder;

            this.txtFolderName.Focus();
            this.txtFolderName.Select();
        }

        /// <summary>
        /// 日付挿入ボタン押下時イベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnInsertDate_Click(object sender, EventArgs e)
        {
            var cursor = this.txtFolderName.SelectionStart;
            var fmt = this.cmbDateFmt.SelectedItem.ToString();

            var dateStr = DateTime.Now.ToString(fmt) + "_";

            this.txtFolderName.Text = this.txtFolderName.Text.Insert(cursor, dateStr);
            this.txtFolderName.SelectionStart = cursor + dateStr.Length;
        }

        /// <summary>
        /// 時刻挿入ボタン押下時イベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void buttonInsertTime_Click(object sender, EventArgs e)
        {
            var cursor = this.txtFolderName.SelectionStart;
            var fmt = this.cmbTimeFmt.SelectedItem.ToString();

            var dateStr = DateTime.Now.ToString(fmt) + "_";

            this.txtFolderName.Text = this.txtFolderName.Text.Insert(cursor, dateStr);
            this.txtFolderName.SelectionStart = cursor + dateStr.Length;
        }

        /// <summary>
        /// OKボタン押下時イベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtFolderName.Text))
            {
                MessageBox.Show("フォルダ名を入力してください。");
                return;
            }

            var path = Path.Combine(this.txtCurrentFolder.Text, this.txtFolderName.Text);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            this.Close();
        }

        /// <summary>
        /// キャンセルボタン押下自イベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">eventArgs</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
