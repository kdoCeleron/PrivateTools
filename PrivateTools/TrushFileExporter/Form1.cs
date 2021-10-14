using MyTools.Common.Extensions;

namespace TrushFIleExporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.IO;
    using System.Windows.Forms;

    using System.Diagnostics.CodeAnalysis;

    using MyTools.Common;

    /// <summary>
    /// メインフォーム
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1630:DocumentationTextMustContainWhitespace", Justification = "Reviewed. Suppression is OK here.")]
    public partial class Form1 : Form
    {
        /// <summary>
        /// TFS連携
        /// </summary>
        private TfsClient tfsClient = new TfsClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class. 
        /// コンストラクタ
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();

            this.txtUrl.Text = @"http://172.17.0.50:8080/tfs";

            this.Load += this.OnLoad;
        }

        /// <summary>
        /// 画面ロード時処理
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void OnLoad(object sender, EventArgs e)
        {
            try
            {
                Config.ReadConfig();
            }
            catch (Exception exception)
            {
                MessageBox.Show(StringTable.ErrWindowInit + " {0}".Fmt(exception));
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 出力釦押下時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void BtnExport_Click(object sender, EventArgs e)
        {
            var dir = this.txtRootPath.Text;
            if (!Directory.Exists(dir))
            {
                MessageBox.Show("ディレクトリ不正");
                return;
            }

            var result = new List<PathInfo>();
            this.GetTrushFileAndFolders(dir, result);

            if (!result.Any())
            {
                MessageBox.Show("Trush ファイルなし");
                return;
            }

            var items = new List<string>();
            foreach (var pathInfo in result)
            {
                var path = pathInfo.Path;

                var name = this.GetName(path);

                var row = path + "\t" + name;
                items.Add(row);
            }

            File.WriteAllLines(@"不要なファイルリスト.tsv", items);
            System.Diagnostics.Process.Start(@".");
        }

        /// <summary>
        /// 削除釦押下時イベント
        /// </summary>
        /// <param name="sender">イベント送信オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var isUseTfs = this.chkIsTfsConnect.Checked;
            if (isUseTfs)
            {
                this.tfsClient = new TfsClient();
                this.tfsClient.Url = this.txtUrl.Text;
                this.tfsClient.UserName = this.txtUser.Text;
                this.tfsClient.Password = this.txtPass.Text;
                if (!this.tfsClient.Connect())
                {
                    MessageBox.Show("TFSとの接続に失敗しました。", "AutoGenerate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            var dlg = new OpenFileDialog();
            dlg.InitialDirectory = @".";
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var filePath = dlg.FileName;
            if (!File.Exists(filePath))
            {
                return;
            }

            var read = File.ReadAllLines(filePath);

            var items = new List<string>();
            foreach (var str in read)
            {
                var splitted = str.Split('\t');
                if (splitted.Length < 1)
                {
                    continue;
                }

                var path = splitted[0];
                items.Add(path);
            }

            foreach (var item in items)
            {
                var path = item;

                // ファイル/フォルダ、いずれかで存在している場合のみ
                var isExists = Directory.Exists(path) || File.Exists(path);
                if (!isExists)
                {
                    continue;
                }

                var isDir = File.GetAttributes(path).HasFlag(FileAttributes.Directory);
                if (isDir)
                {
                    if (isUseTfs)
                    {
                        var ret = this.tfsClient.Delete(path, path);
                        if (ret == -1)
                        {
                            this.DeleteFolder(path);
                        }
                    }
                    else
                    {
                        this.DeleteFolder(path);
                    }
                }
                else
                {
                    if (isUseTfs)
                    {
                        var ret = this.tfsClient.Delete(path, path);
                        if (ret == -1)
                        {
                            this.DeleteFile(path);
                        }
                    }
                    else
                    {
                        this.DeleteFile(path);
                    }
                }
            }

            MessageBox.Show("削除完了しました。");
        }

        /// <summary>
        /// 削除対象のファイル/フォルダパスを取得します。
        /// </summary>
        /// <param name="path">探索対象のフォルダパス</param>
        /// <param name="infoList">格納先</param>
        private void GetTrushFileAndFolders(string path, List<PathInfo> infoList)
        {
            // ディレクトリ内のファイル探索
            foreach (var file in Directory.EnumerateFiles(path))
            {
                if (File.Exists(file))
                {
                    if (this.IsTrushFile(file))
                    {
                        infoList.Add(new PathInfo(false, file));
                    }
                }
            }

            // ディレクトリ内のディレクトリ探索
            foreach (var directory in Directory.EnumerateDirectories(path))
            {
                if (!Directory.Exists(directory))
                {
                    continue;
                }

                var dirName = this.GetName(path);
                if (dirName.StartsWith("."))
                {
                    // 「.」から始まるファイルは対象外
                    infoList.Add(new PathInfo(true, path));
                    continue;
                }

                this.GetTrushFileAndFolders(directory, infoList);
            }
        }

        /// <summary>
        /// ファイル/フォルダの名称を取得します
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns>取得結果</returns>
        private string GetName(string path)
        {
            var splitted = path.Split('\\');
            if (!splitted.Any())
            {
                return string.Empty;
            }

            var name = splitted.Last();

            return name;
        }

        /// <summary>
        /// 削除対象かどうかを判定します
        /// </summary>
        /// <param name="filePath">パス</param>
        /// <returns>true: 削除対象/ false: それ以外</returns>
        private bool IsTrushFile(string filePath)
        {
            var fileName = this.GetName(filePath);

            // 拡張子で分割
            var splitted = fileName.Split('.');
            if (splitted.Length < 2)
            {
                // 分割なしは、対象外(makefile 等)
                return false;
            }

            if (fileName == "CMakeLists.txt")
            {
                // CMakeLists.txt は除外
                return false;
            }

            var extension = splitted.Last().ToLower();
            var knownExtensions = Config.Instance.IgnoreExtensions;

            if (!knownExtensions.Contains(extension))
            {
                // 把握していない拡張子が対象
                return true;
            }

            return false;
        }

        /// <summary>
        /// フォルダを削除します。
        /// </summary>
        /// <param name="path">フォルダパス</param>
        private void DeleteFolder(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            dirInfo.Attributes = FileAttributes.Normal;

            if ((dirInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                dirInfo.Attributes = FileAttributes.Normal;
            }

            foreach (var fileInfo in dirInfo.GetFiles())
            {
                this.RemoveReadonlyAttributeFile(fileInfo);
            }

            foreach (var subDirInfo in dirInfo.GetDirectories())
            {
                this.RemoveReadonlyAttributeDirectory(subDirInfo);
            }

            dirInfo.Delete(true);
        }

        /// <summary>
        /// ファイルを削除します。
        /// </summary>
        /// <param name="path">ファイルパス</param>
        private void DeleteFile(string path)
        {
            var fileInfo = new FileInfo(path);
            this.RemoveReadonlyAttributeFile(fileInfo);

            fileInfo.Delete();
        }

        /// <summary>
        /// ファイルの読み取り専用属性を外す.
        /// </summary>
        /// <param name="fileInfo">ファイル情報</param>
        private void RemoveReadonlyAttributeFile(FileInfo fileInfo)
        {
            if ((fileInfo.Attributes & FileAttributes.ReadOnly) > 0)
            {
                fileInfo.Attributes &= ~FileAttributes.ReadOnly;
            }
        }

        /// <summary>
        /// フォルダの読み取り専用属性を外す.
        /// </summary>
        /// <param name="dirInfo">フォルダ情報</param>
        private void RemoveReadonlyAttributeDirectory(DirectoryInfo dirInfo)
        {
            if ((dirInfo.Attributes & FileAttributes.ReadOnly) > 0)
            {
                dirInfo.Attributes &= ~FileAttributes.ReadOnly;
            }

            foreach (var fileInfo in dirInfo.GetFiles())
            {
                this.RemoveReadonlyAttributeFile(fileInfo);
            }

            foreach (var subDirInfo in dirInfo.GetDirectories())
            {
                this.RemoveReadonlyAttributeDirectory(subDirInfo);
            }
        }
    }
}
