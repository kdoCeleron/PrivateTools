namespace MyTools.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// 汎用ロジックのサンプル処理
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1630:DocumentationTextMustContainWhitespace", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here.")]
    [TestClass]
    public class CommonLogicTest
    {
        /// <summary>
        /// バイト取得
        /// </summary>
        [TestMethod]
        public void GetByte()
        {
            int i = 15; // 1111
            int and = 12; // 1100
            int and2 = 3; // 0011

            // 上２ビットの取得
            var upper = Convert.ToString((i & and) >> 2, 2);

            // 下２ビットの取得
            var under = Convert.ToString((i & and2), 2);


            Assert.AreEqual(upper, 11.ToString());
            Assert.AreEqual(under, 11.ToString());
        }

        /// <summary>
        /// リフレクション
        /// </summary>
        [TestMethod]
        public void Reflection()
        {
            var data1 = new DataClass()
                            {
                                A = "AA",
                                B = "BB",
                                C = new DataClass2()
                                        {
                                            A = "ABB",
                                            B = "BBB"
                                        }
                            };

            var properties = typeof(DataClass).GetProperties();
            foreach (var prop in properties)
            {
                Console.WriteLine(prop.GetValue(data1));
            }
        }

        /// <summary>
        /// データクラス
        /// </summary>
        public class DataClass
        {
            /// <summary>
            /// プロパティ1
            /// </summary>
            public string A { get; set; }

            /// <summary>
            /// プロパティ1
            /// </summary>
            public string B { get; set; }

            /// <summary>
            /// プロパティ3(クラス)
            /// </summary>
            public DataClass2 C { get; set; }
        }

        /// <summary>
        /// データクラス
        /// </summary>
        public class DataClass2
        {
            /// <summary>
            /// プロパティ1
            /// </summary>
            public string A { get; set; }

            /// <summary>
            /// プロパティ2
            /// </summary>
            public string B { get; set; }
        }
    }
}
