namespace TaskManager
{
    /// <summary>
    /// タスク一覧の列種別
    /// </summary>
    public enum DataGridColumnType
    {
        /// <summary>
        /// ボタン
        /// </summary>
        Button,

        /// <summary>
        /// ラベル
        /// </summary>
        Label,

        /// <summary>
        /// テキストボックス
        /// </summary>
        TextBox
    }

    /// <summary>
    /// サブ画面の結果
    /// </summary>
    public enum SubWindowResult
    {
        /// <summary>
        /// なし
        /// </summary>
        None,

        /// <summary>
        /// 確定
        /// </summary>
        Submit,

        /// <summary>
        /// キャンセル
        /// </summary>
        Cancel
    }
}
