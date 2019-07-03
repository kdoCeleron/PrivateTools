namespace MyTools.Common
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 拡張メソッドの定義クラス
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1630:DocumentationTextMustContainWhitespace", Justification = "Reviewed. Suppression is OK here.")]
    public static class Extensions
    {
        /// <summary>
        ///     string.Formatを行います。
        /// </summary>
        /// <param name="self">自分自身</param>
        /// <param name="args">引数</param>
        /// <returns>整形済み文字列</returns>
        public static string Fmt(this string self, params object[] args)
        {
            return string.Format(self, args);
        }
    }
}
