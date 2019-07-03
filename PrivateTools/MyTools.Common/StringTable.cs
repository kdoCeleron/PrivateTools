namespace MyTools.Common
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 文字列定義
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1630:DocumentationTextMustContainWhitespace", Justification = "Reviewed. Suppression is OK here.")]
    public static class StringTable
    {
        /// <summary>
        /// 画面初期化時エラーのメッセージ
        /// </summary>
        public static string ErrWindowInit = "初期化処理中にエラーが発生しました。";
    }
}
