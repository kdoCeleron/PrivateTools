using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClipBoardToPng
{
    using System.IO;

    public partial class Form1 : Form
    {
        private string FilNameTemplate = "{0}_{1:D3}.png";

        public Form1()
        {
            InitializeComponent();

            this.lblout.Text = string.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var outDir = this.txt1.Text;
            var subDir = this.txt2.Text;

            var combined = Path.Combine(outDir, subDir);

            if (!Directory.Exists(combined))
            {
                Directory.CreateDirectory(combined);
            }

            // 出力通番を取得
            var fileNo = 0;
            foreach (var file in Directory.EnumerateFiles(combined))
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var tmpFileNo = 0;

                var split = fileName.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length != 2)
                {
                    continue;
                }

                var parse = int.TryParse(split[1], out tmpFileNo);
                if (!parse)
                {
                    continue;
                }

                if (fileNo < tmpFileNo)
                {
                    fileNo = tmpFileNo;
                }
            }

            fileNo++;

            try
            {
                if (Clipboard.ContainsImage())
                {
                    var img = Clipboard.GetImage();

                    if (img != null)
                    {
                        var bmp = new Bitmap(img);

                        var outputPath = Path.Combine(combined, string.Format(this.FilNameTemplate, subDir, fileNo));
                        if (File.Exists(outputPath))
                        {
                            outputPath = outputPath + "_NEW";
                        }

                        bmp.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);
                        Clipboard.Clear();
                        img.Dispose();
                        img = null;
                        MessageBox.Show("ファイルを保存しました。", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("エラーが発生しました。詳細:{0}",ex.StackTrace), "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt1_TextChanged(object sender, EventArgs e)
        {
            var outDir = this.txt1.Text;
            var subDir = this.txt2.Text;

            var combined = Path.Combine(outDir, subDir);
            var fullPath = Path.GetFullPath(combined);

            this.lblout.Text = fullPath;
        }

        private void txt2_TextChanged(object sender, EventArgs e)
        {
            var outDir = this.txt1.Text;
            var subDir = this.txt2.Text;

            var combined = Path.Combine(outDir, subDir);
            var fullPath = Path.GetFullPath(combined);

            this.lblout.Text = fullPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

    }
}
