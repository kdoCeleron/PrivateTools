using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private string _subDirFormat = @"{0:D2}巻";

        public Form1()
        {
            this.InitializeComponent();

            this.textBox1.Text = @"C:\Users\tkondo\Pictures\ScanSnap";
            this.textBox1.Enabled = false;

            this.textBox4.Text = @"C:\Users\tkondo\Pictures\ScanResult";
            this.textBox4.Enabled = false;

            this.listView1.FullRowSelect = true;
            this.listView1.View = View.Details;
            this.listView1.Columns.Add("", 500);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var folder = this.textBox1.Text;
            if (!Directory.Exists(folder))
            {
                MessageBox.Show("フォルダがありません"); 
                return;
            }

            var title = this.textBox2.Text;
            var path = Path.Combine(folder, title);
            if (Directory.Exists(path))
            {
                MessageBox.Show("すでに存在しています。");
                return;
            }

            var kansu = this.textBox3.Text;
            var value = 0;
            var num = int.TryParse(kansu, out value);
            if(num == false)
            {
                MessageBox.Show("数値を入力してください");
                return;
            }

            for(var index = 1; index <= value; index++)
            {
                var dir = Path.Combine(path, string.Format(this._subDirFormat, index));
                Directory.CreateDirectory(dir);
            }

            this.MoveFiles(path, title, value);

            this.Output(path);

            MessageBox.Show(string.Format("フォルダの生成が完了しました。{0}", path));
        }

        private void Output(string outDir)
        {
            var itemList = new List<string>();
            var folders = Directory.EnumerateDirectories(outDir);
            foreach(var folder in folders)
            {
                itemList.Add(folder);
                var files = Directory.EnumerateFiles(folder).Select(x => "\t" + Path.GetFileName(x)).ToList();
                itemList.AddRange(files);
            }

            this.listView1.Items.AddRange(itemList.Select(x => new ListViewItem(x)).ToArray());
        }

        private void MoveFiles(string destDir, string title, int subFolderNum)
        {
            var sourceDir = this.textBox4.Text;
            if (Directory.Exists(sourceDir) == false)
            {
                return;
            }

            var sourceFiles = Directory.EnumerateFiles(sourceDir).OrderBy(x => Path.GetFileName(x)).ToList();
            if(subFolderNum != sourceFiles.Count)
            {
                MessageBox.Show("出力済みファイル数と、作成済みフォルダ数が異なります。");
                return;
            }

            var now = DateTime.Now.ToString("yyyyMMddHHmmss");
            var backup = Path.Combine(sourceDir, title);
            Directory.CreateDirectory(backup);
            foreach (var file in sourceFiles)
            {
                var fileName = Path.GetFileName(file);
                var destSubDir = Path.Combine(destDir, backup);
                var destFile = Path.Combine(destSubDir, Path.GetFileNameWithoutExtension(file) + ".pdf");
                File.Copy(file, destFile);
            }

            foreach (var file in sourceFiles)
            {
                var index = sourceFiles.IndexOf(file) + 1;
                var fileName = Path.GetFileName(file);

                var subDirName = string.Format(this._subDirFormat, index);
                var destSubDir = Path.Combine(destDir, subDirName);
                var destFile = Path.Combine(destSubDir, title + "_" + subDirName + ".pdf");

                File.Copy(file, destFile);
            }
        }

        private void btnChangeDir_Click(object sender, EventArgs e)
        {
            var baseDir = this.textBox5.Text;
            var regex = new Regex(".*\\d巻.*", RegexOptions.Compiled);

            try
            {
                DoChangeDirRecursive(baseDir, regex);
            }
            catch (Exception exception)
            {
                MessageBox.Show(string.Format("エラーが発生。{0}", exception.Message));
            }

            MessageBox.Show("処理完了");
        }

        private void DoChangeDirRecursive(string baseDir, Regex regex)
        {
            foreach (var directory in Directory.EnumerateDirectories(baseDir))
            {
                var isMatch = regex.IsMatch(directory);
                if (isMatch)
                {
                    foreach (var file in Directory.EnumerateFiles(directory))
                    {
                        var fileName = Path.GetFileName(file);
                        if (fileName != null)
                        {
                            var dest = Path.Combine(baseDir, fileName);
                            File.Move(file, dest);
                        }
                    }

                    Directory.Delete(directory);
                }
                else
                {
                    this.DoChangeDirRecursive(directory, regex);
                }
            }
        }
    }
}
