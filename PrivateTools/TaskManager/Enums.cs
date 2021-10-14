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

    /// <summary>
    /// 画面種別
    /// </summary>
    public enum ViewKind
    {
        /// <summary>
        /// メイン画面
        /// </summary>
        MainView,

        /// <summary>
        /// 設定編集画面
        /// </summary>
        ConfigEditView,

        /// <summary>
        /// タスク編集(追加)画面
        /// </summary>
        TaskEditView,

        /// <summary>
        /// グループ編集(追加)画面
        /// </summary>
        TaskGroupEditView,
    }
}
