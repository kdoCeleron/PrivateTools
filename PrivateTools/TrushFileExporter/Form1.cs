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

    /// <summary>
    /// メインフォーム
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1630:DocumentationTextMustContainWhitespace", Justification = "Reviewed. Suppression is OK here.")]
    public partial class Form1 : Form
    {
        /// <summary>
        /// 出力文字(ディレクトリ)
        /// </summary>
        private static string outputNameIsDirectory = "Directory";

        /// <summary>
        /// 出力文字(ファイル)
        /// </summary>
        private static string outputNameIsFile = "File";

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
                var kind = pathInfo.IsDirectory ? outputNameIsDirectory : outputNameIsFile;
                var path = pathInfo.Path;

                var name = this.GetName(path);

                var row = kind + "\t" + path + "\t" + name;
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

            // Item1:ディレクトリかどうか、Item2:パス
            var items = new List<Tuple<bool, string>>();

            foreach (var str in read)
            {
                var splitted = str.Split('\t');
                if (splitted.Length < 2)
                {
                    continue;
                }

                var kind = splitted[0];
                var path = splitted[1];

                var item = Tuple.Create(kind == outputNameIsDirectory, path);
                items.Add(item);
            }

            foreach (var item in items.OrderBy(x => x.Item1))
            {
                var path = item.Item2;
                if (item.Item1)
                {
                    if (Directory.Exists(path))
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
                }
                else
                {
                    if (File.Exists(path))
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
            var knownExtensions = new List<string>()
            {
                "c",
                "h",
                "so",
                "sh",
            };

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
                this.UnsetDirectoryReadOnly(fileInfo);
            }

            foreach (var subDirInfo in dirInfo.GetDirectories())
            {
                this.UnsetReadonlyAttributeDirectory(subDirInfo);
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
            this.UnsetDirectoryReadOnly(fileInfo);

            fileInfo.Delete();
        }

        /// <summary>
        /// ファイルの読み取り専用属性を外す.
        /// </summary>
        /// <param name="fileInfo">ファイル情報</param>
        private void UnsetDirectoryReadOnly(FileInfo fileInfo)
        {
            if ((fileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                fileInfo.Attributes = FileAttributes.Normal;
            }
        }

        /// <summary>
        /// フォルダの読み取り専用属性を外す.
        /// </summary>
        /// <param name="dirInfo">フォルダ情報</param>
        private void UnsetReadonlyAttributeDirectory(DirectoryInfo dirInfo)
        {
            if ((dirInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                dirInfo.Attributes = FileAttributes.Normal;
            }

            foreach (var fileInfo in dirInfo.GetFiles())
            {
                this.UnsetDirectoryReadOnly(fileInfo);
            }

            foreach (var subDirInfo in dirInfo.GetDirectories())
            {
                this.UnsetReadonlyAttributeDirectory(subDirInfo);
            }
        }
    }
}
