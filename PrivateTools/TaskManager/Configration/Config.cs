
namespace TaskManager.Configration
{
    using System.Text;
    using System.IO;
    using System.Xml.Serialization;

    public class Config
    {
        #region フィールド

        /// <summary>
        /// ファイルパス
        /// </summary>
        public const string ConfigFilePath = @".\Config.xml";

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
        }

        #endregion

        #region プロパティ


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

            StreamReader sr = new StreamReader(ConfigFilePath, Encoding.UTF8);
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

            StreamWriter sw = new StreamWriter(ConfigFilePath, false, Encoding.UTF8);
            serializer.Serialize(sw, Config.Instance, ns);
            sw.Close();
        }

        #endregion
    }
}
