namespace TrushFIleExporter
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// パス情報
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1630:DocumentationTextMustContainWhitespace", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1642:ConstructorSummaryDocumentationMustBeginWithStandardText", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here.")]
    public class PathInfo
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="isDirectory">ディレクトリかどうか</param>
        /// <param name="path">パス</param>
        public PathInfo(bool isDirectory, string path)
        {
            this.IsDirectory = isDirectory;
            this.Path = path;
        }

        /// <summary>
        /// ディレクトリかどうか
        /// </summary>
        public bool IsDirectory { get; set; }

        /// <summary>
        /// パス
        /// </summary>
        public string Path { get; set; }
    }
}