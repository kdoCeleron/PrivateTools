﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Config.cs" company="">
//   
// </copyright>
// <summary>
//   設定ファイル管理クラス
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using TaskManager.ConfigrationData;

namespace TaskManager.Configration
{
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    /// <summary>
    /// 設定ファイル管理クラス
    /// </summary>
    public class Config
    {
        #region フィールド

        /// <summary>
        /// ファイルパス
        /// </summary>
        public const string ConfigFilePath = @".\Configs\Config.xml";

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        public static Config Instance;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static Config()
        {
            Config.Instance = new Config();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Config()
        {
            // Do Noting
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// バックアップ用のディレクトリ
        /// </summary>
        public string BackupRootDir { get; set; }

        /// <summary>
        /// CSV出力機能用のディレクトリ
        /// </summary>
        public string CsvRootDir { get; set; }

        /// <summary>
        /// 添付ファイルの管理ディレクトリ
        /// </summary>
        public string AttachedFileRootDir { get; set; }

        /// <summary>
        /// 変更可能設定
        /// </summary>
        public EditableConfigData EditableItems { get; set; }

        #endregion

        #region メソッド

        /// <summary>
        /// 設定ファイル読み込み
        /// </summary>
        public void ReadConfig()
        {
            Instance = new Config();

            XmlSerializer serializer = new XmlSerializer(typeof(Config));

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            Config item = null;
            
            var fullPath = Utils.GetFullPath(ConfigFilePath);
            StreamReader sr = new StreamReader(fullPath, Encoding.UTF8);
            item = (Config)serializer.Deserialize(sr);
            sr.Close();
            Instance = item;
        }

        /// <summary>
        /// 設定ファイル書き込み
        /// </summary>
        public void WriteConfig()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            var fullPath = Utils.GetFullPath(ConfigFilePath);
            StreamWriter sw = new StreamWriter(fullPath, false, Encoding.UTF8);
            serializer.Serialize(sw, Config.Instance, ns);
            sw.Close();
        }

        #endregion
    }
}
