using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SameFileRemove
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var rootDir = this.txtDirRemoveSameFile.Text;
            if(Directory.Exists(rootDir) == false)
            {
                MessageBox.Show("ディレクトリがありません");
                return;
            }

            var taihiPath = Path.Combine(rootDir, @"除外フォルダ");
            if (Directory.Exists(taihiPath) == false)
            {
                Directory.CreateDirectory(taihiPath);
            }

            var loaded = new List<FileData>();
            foreach (var directory in Directory.EnumerateDirectories(rootDir).ToList())
            {
                if (directory == taihiPath)
                {
                    continue;
                }

                var files = Directory.EnumerateFiles(directory).ToList();
                foreach (var file in files)
                {
                    try
                    {
                        var bs = File.ReadAllBytes(file);
                        var newFileItem = new FileData();
                        newFileItem.Data = new List<byte>();
                        foreach (var b in bs)
                        {
                            newFileItem.Data.Add(b);
                        }

                        var isExist = loaded.Any(x => x.IsEquals(newFileItem));
                        if (isExist)
                        {
                            var fileName = Path.GetFileName(file);
                            var destPath = Path.Combine(taihiPath, fileName);
                            if (File.Exists(destPath))
                            {
                                File.Move(file, destPath + @".bak");
                            }
                            else
                            {
                                File.Move(file, destPath);
                            }
                        }
                        else
                        {
                            loaded.Add(newFileItem);
                        }
                    }
                    catch (Exception)
                    {
                        // do noting
                    }
                }
            }
            
            foreach (var fileData in loaded)
            {
                fileData.Data.Clear();
            }

            loaded.Clear();

            GC.Collect();
            MessageBox.Show("処理が完了しました。");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var directory = this.txtDirRename.Text;
            if (Directory.Exists(directory) == false)
            {
                MessageBox.Show("ディレクトリがありません");
                return;
            }

            var taihiPath = Path.Combine(directory, @"renamed");
            if (Directory.Exists(taihiPath) == false)
            {
                Directory.CreateDirectory(taihiPath);
            }

            var files = Directory.EnumerateFiles(directory).ToList();
            foreach (var file in files)
            {
                try
                {
                    var extension = Path.GetExtension(file);
                    var created = File.GetLastWriteTimeUtc(file);
                    var createdFmt = created.ToString("yyyy-MM-dd-HH-mm-ss-fff");
                    var fmt = "{0}_{1:D2}{2}"; // 日時 + 通番 + 拡張子

                    var tsuBan = 0;
                    var destFileName = string.Format(fmt, createdFmt, tsuBan, extension);
                    while (File.Exists(Path.Combine(taihiPath, destFileName)))
                    {
                        tsuBan++;
                        destFileName = string.Format(fmt, createdFmt, tsuBan, extension);
                    }

                    var dest = Path.Combine(taihiPath, destFileName);
                    File.Move(file, dest);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            MessageBox.Show("処理が完了しました。");
        }
    }
}
