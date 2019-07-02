
namespace TrushFIleExporter
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    /// <summary>
    /// 設定ファイルクラス
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1630:DocumentationTextMustContainWhitespace", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1642:ConstructorSummaryDocumentationMustBeginWithStandardText", Justification = "Reviewed. Suppression is OK here.")]
    [System.Xml.Serialization.XmlRootAttribute("Config")]
    public class Config
    {
        #region Private Fields

        /// <summary>
        /// 設定ファイルパス
        /// </summary>
        private const string CsConfigPath = @"./Config.xml";

        #endregion

        #region Constructor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal Config()
        {
            // Do Nothing
        }

        #endregion

        #region Property

        /// <summary>
        /// インスタンス
        /// </summary>
        public static Config Instance { get; set; }

        /// <summary>
        /// 処理対象外拡張子
        /// </summary>
        [XmlArray("IgnoreExtensionList")]
        [XmlArrayItem("Extension")]
        public List<string> IgnoreExtensions { get; set; }

        #endregion

        #region Static Methods

        /// <summary>
        /// 設定ファイル読み込み
        /// </summary>
        public static void ReadConfig()
        {
            Instance = new Config();

            XmlSerializer serializer = new XmlSerializer(typeof(Config));

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            Config item = null;

            StreamReader sr = new StreamReader(CsConfigPath, Encoding.UTF8);
            item = (Config)serializer.Deserialize(sr);
            sr.Close();
            Instance = item;
        }

        /// <summary>
        /// 設定ファイル書き込み
        /// </summary>
        public static void WriteConfig()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            StreamWriter sw = new StreamWriter(CsConfigPath, false, Encoding.UTF8);
            serializer.Serialize(sw, Config.Instance, ns);
            sw.Close();
        }

        #endregion
    }
}
