using System;
using System.Text;

namespace WindowsFormsApplication9
{
    using System.IO;
    using System.Xml.Linq;

    public class TFSUtils
    {
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="targetPath">削除ファイル</param>
        public void RemoveBind(string targetPath)
        {
            var files = Directory.GetFiles(targetPath, "*.scc", SearchOption.AllDirectories);

            for (var i = 0; i <= files.Length - 1; i++)
            {
                File.SetAttributes(files[i], FileAttributes.Normal);
                File.Delete(files[i]);
            }

            files = Directory.GetFiles(targetPath, "*.vspscc", SearchOption.AllDirectories);

            for (var i = 0; i <= files.Length - 1; i++)
            {
                File.SetAttributes(files[i], FileAttributes.Normal);
                File.Delete(files[i]);
            }

            files = Directory.GetFiles(targetPath, "*.vssscc", SearchOption.AllDirectories);

            for (var i = 0; i <= files.Length - 1; i++)
            {
                File.SetAttributes(files[i], FileAttributes.Normal);
                File.Delete(files[i]);
            }

            files = Directory.GetFiles(targetPath, "*.sln", SearchOption.AllDirectories);

            for (var i = 0; i <= files.Length - 1; i++)
            {
                RemoveSolutionBind(files[i], "GlobalSection(SourceCodeControl)");
                RemoveSolutionBind(files[i], "GlobalSection(TeamFoundationVersionControl)");
            }

            files = Directory.GetFiles(targetPath, "*.vbproj", SearchOption.AllDirectories);

            for (var i = 0; i <= files.Length - 1; i++)
            {
                RemoveProjectBind(files[i]);
            }

            files = Directory.GetFiles(targetPath, "*.csproj", SearchOption.AllDirectories);

            for (var i = 0; i <= files.Length - 1; i++)
            {
                RemoveProjectBind(files[i]);
            }

        }

        /// <summary>
        /// ソリューション削除
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="sectionName">選択名</param>
        private void RemoveSolutionBind(string filePath, string sectionName)
        {
            var lines = File.ReadAllLines(filePath);

            var startRow = -1;
            var finishRow = -1;

            for (var i = 0; i <= lines.Length - 1; i++)
            {
                if (lines[i].IndexOf(sectionName, StringComparison.CurrentCultureIgnoreCase) > 0)
                {
                    startRow = i;
                    break;
                }
            }

            if (startRow != -1)
            {
                for (var i = startRow; i <= lines.Length - 1; i++)
                {
                    if (lines[i].IndexOf("EndGlobalSection", StringComparison.CurrentCultureIgnoreCase) > 0)
                    {
                        finishRow = i;
                        break;
                    }
                }

                if (startRow != -1 && finishRow != -1)
                {
                    var sb = new StringBuilder();

                    for (var i = 0; i <= lines.Length - 1; i++)
                    {
                        if (startRow <= i && i <= finishRow)
                        {
                            continue;
                        }

                        sb.AppendLine(lines[i]);
                    }

                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Delete(filePath);
                    File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                }
            }
        }

        /// <summary>
        /// プロジェクト削除
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        private void RemoveProjectBind(string filePath)
        {
            var xmlDoc = XDocument.Load(filePath);

            RecursiveSearch(xmlDoc.Root, "SccProjectName");
            RecursiveSearch(xmlDoc.Root, "SccLocalPath");
            RecursiveSearch(xmlDoc.Root, "SccAuxPath");
            RecursiveSearch(xmlDoc.Root, "SccProvider");

            File.SetAttributes(filePath, FileAttributes.Normal);
            File.Delete(filePath);
            xmlDoc.Save(filePath);
        }

        /// <summary>
        /// 再帰検索
        /// </summary>
        /// <param name="xmlElement">xmlエレメント</param>
        /// <param name="targetTag">対象タグ</param>
        private void RecursiveSearch(XElement xmlElement, string targetTag)
        {
            if (string.Compare(xmlElement.Name.LocalName, targetTag, true) == 0)
            {
                xmlElement.Value = string.Empty;
            }

            foreach (var item in xmlElement.Elements())
            {
                RecursiveSearch(item, targetTag);
            }
        }
    }
}
