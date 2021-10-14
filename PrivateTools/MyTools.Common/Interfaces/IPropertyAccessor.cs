namespace MyTools.Common.Interfaces
{
    /// <summary>
    /// プロパティアクセスのデータクラスです。
    /// </summary>
    public interface IPropertyAccessor
    {
        /// <summary>
        /// プロパティの値を取得します
        /// </summary>
        /// <param name="target">インスタンス</param>
        /// <returns>プロパティ値</returns>
        object GetValue(object target);

        /// <summary>
        /// プロパティの値を設定します
        /// </summary>
        /// <param name="target">インスタンス</param>
        /// <param name="value">プロパティ値</param>
        void SetValue(object target, object value);
    }
}
