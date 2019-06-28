// Team Foundation Server 

namespace TrushFIleExporter
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net;

    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.Framework.Common;
    using Microsoft.TeamFoundation.VersionControl.Client;

    /// <summary>
    /// TFSとのデータのやり取りをするユーティリティ
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1630:DocumentationTextMustContainWhitespace", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1309:FieldNamesMustNotBeginWithUnderscore", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here.")]
    public class TfsClient
    {
        /// <summary>
        /// TFSインスタンス
        /// </summary>
        private VersionControlServer _vcs;

        /// <summary>
        /// 接続URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 接続ユーザー名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 接続パスワード
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 接続 
        /// </summary>
        /// <returns>処理結果</returns>
        public bool Connect()
        {
            bool result = false;

            try
            {
                // 認証 
                var nc = new NetworkCredential(this.UserName, this.Password);
                var configurationServer = new TfsConfigurationServer(new Uri(this.Url), nc);
                configurationServer.Authenticate();

                if (configurationServer.HasAuthenticated)
                {
                    // VSC接続 
                    var configurationServerNode = configurationServer.CatalogNode;

                    var tpcNodes =
                        configurationServerNode.QueryChildren(
                            new[] { CatalogResourceTypes.ProjectCollection },
                            false,
                            CatalogQueryOptions.None);

                    foreach (var tpcNode in tpcNodes)
                    {
                        var tpcId = new Guid(tpcNode.Resource.Properties["InstanceId"]);
                        var tpc = configurationServer.GetTeamProjectCollection(tpcId);
                        this._vcs = tpc.GetService<VersionControlServer>();
                    }
                }

                result = configurationServer.HasAuthenticated;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 指定したワークスペースのファイルをチェックアウトします。
        /// </summary>
        /// <param name="localPath">ローカルワークスペースパス</param>
        /// <param name="filePath">チェックアウトファイルパス</param>
        /// <returns>チェックアウトしたファイル数</returns>
        public int CheckOut(string localPath, string filePath)
        {
            var ws = this._vcs.TryGetWorkspace(localPath);
            if (ws == null)
            {
                return -1;
            }

            return ws.PendEdit(filePath);
        }

        /// <summary>
        /// 指定したワークスペースのディレクトリをチェックアウトします。
        /// </summary>
        /// <param name="localPath">ローカルワークスペースパス</param>
        /// <param name="directory">チェックアウトディレクトリパス</param>
        /// <returns>チェックアウトしたディレクトリ数</returns>
        public int CheckOutDirectory(string localPath, string directory)
        {
            var ws = this._vcs.TryGetWorkspace(localPath);
            int result = 0;

            var files = Directory.GetFiles(directory);
            if (files.Any())
            {
                result += ws.PendEdit(files);
            }

            return result;
        }

        /// <summary>
        /// 指定したワークスペースのファイルを追加します。
        /// </summary>
        /// <param name="localPath">ローカルワークスペースパス</param>
        /// <param name="filePath">チェックアウトファイルパス</param>
        /// <returns>追加したファイル数</returns>
        public int Add(string localPath, string filePath)
        {
            var ws = this._vcs.TryGetWorkspace(localPath);
            if (ws == null)
            {
                return -1;
            }

            return ws.PendAdd(filePath, true);
        }

        /// <summary>
        /// 指定したワークスペースのファイルを削除します。
        /// </summary>
        /// <param name="localPath">ローカルワークスペースパス</param>
        /// <param name="filePath">削除ファイルパス</param>
        /// <returns>削除したファイル数</returns>
        /// <remarks>
        /// 削除対象のファイルがチェックアウトされていない場合のみ、削除することができます。
        /// </remarks>
        public int Delete(string localPath, string filePath)
        {
            var ws = this._vcs.TryGetWorkspace(localPath);
            if (ws == null)
            {
                return -1;
            }

            return ws.PendDelete(filePath);
        }

        /// <summary>
        /// プロジェクトを取得します。
        /// </summary>
        /// <param name="repository">リポジトリ</param>
        /// <param name="localPath">ダウンロード先</param>
        /// <param name="tempWorkSpaceName">一時ワークスペース名</param>
        /// <param name="version">取得するバージョン</param>
        public void Get(string repository, string localPath, string tempWorkSpaceName, VersionSpec version)
        {
            // テンポラリワークスペースの削除
            var tempWorkspace = this._vcs.GetWorkspace(tempWorkSpaceName, this._vcs.AuthorizedUser);
            if (tempWorkspace != null)
            {
                this._vcs.DeleteWorkspace(tempWorkSpaceName, this._vcs.AuthorizedUser);
            }

            // テンポラリワークスペース作成
            Workspace ws = this._vcs.TryGetWorkspace(localPath);
            if (ws == null)
            {
                ws = this._vcs.CreateWorkspace(tempWorkSpaceName, this._vcs.AuthorizedUser);
                WorkingFolder folder = new WorkingFolder(repository, localPath);
                ws.CreateMapping(folder);
            }

            // 出力先フォルダ再作成
            if (Directory.Exists(localPath))
            {
                Directory.Delete(localPath, true);
            }

            Directory.CreateDirectory(localPath);

            // ファイル取得
            ws.Get(version, GetOptions.GetAll);

            // ファイルの日付変更
            ItemSet vcsItemSet = this._vcs.GetItems(repository, version, RecursionType.Full);
            foreach (Item vcsItem in vcsItemSet.Items)
            {
                string filePath = vcsItem.ServerItem.Replace(repository, localPath).Replace("/", "\\");

                try
                {
                    if (vcsItem.ItemType == ItemType.Folder)
                    {
                        if (Directory.Exists(filePath))
                        {
                            DirectoryInfo dInfo = new System.IO.DirectoryInfo(filePath);

                            // ディレクトリの日付をチェックイン日付に変更
                            Directory.SetCreationTime(filePath, vcsItem.CheckinDate);
                            Directory.SetLastAccessTime(filePath, vcsItem.CheckinDate);
                            Directory.SetLastWriteTime(filePath, vcsItem.CheckinDate);
                        }
                    }
                    else
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            FileInfo fInfo = new System.IO.FileInfo(filePath);
                            File.SetAttributes(filePath, FileAttributes.Normal);

                            // ファイルの日付をチェックイン日付に変更
                            File.SetCreationTime(filePath, vcsItem.CheckinDate);
                            File.SetLastAccessTime(filePath, vcsItem.CheckinDate);
                            File.SetLastWriteTime(filePath, vcsItem.CheckinDate);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("{0} {1} {2}", ex.Message, vcsItem.ServerItem, filePath);
                }
            }

            // テンポラリワークスペース削除
            this._vcs.DeleteWorkspace(tempWorkSpaceName, this._vcs.AuthorizedUser);
        }
    }
}
