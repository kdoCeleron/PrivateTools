using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManager.Controls
{
    /// <summary>
    /// サブ画面基底クラス
    /// </summary>
    public class SubWindowBase : Form
    {
        /// <summary>
        ///     タスク処理の結果
        /// </summary>
        private TaskCompletionSource<SubWindowResult> _tcs;
        
        /// <summary>
        /// 画面を表示します。
        /// </summary>
        /// <param name="parent">親画面</param>
        /// <returns>画面の操作結果</returns>
        public Task<SubWindowResult> ShowWindow(Form parent)
        {
            this._tcs = new TaskCompletionSource<SubWindowResult>();

            this.StartPosition = FormStartPosition.Manual;
            this.Owner = parent;

            var win = this.Owner;
            if (win != null)
            {
                var winPos = win.Location;
                var winWidth = this.Owner.Width;
                var winHeight = this.Owner.Height;
                var winCenterPosX = winPos.X + (winWidth / 2);
                var winCenterPosY = winPos.Y + (winHeight / 2);

                var width = this.Width;
                var height = this.Height;

                var posX = winCenterPosX - (width / 2);
                var posY = winCenterPosY - (height / 2);

                this.Location = new Point(posX, posY);
            }

            this.Closing += OnClosing;

            this.Owner.Enabled = false;

            this.Show();

            return this._tcs.Task;
        }

        /// <summary>
        /// 画面が閉じられる際のイベント
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void OnClosing(object sender, CancelEventArgs e)
        {
            this._tcs.TrySetResult(SubWindowResult.None);

            this.Owner.Enabled = true;
        }

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        /// <param name="result">結果</param>
        protected void CloseWindow(SubWindowResult result)
        {
            this._tcs.SetResult(result);

            this.Close();
        }

        /// <summary>
        /// コマンドキー処理
        /// </summary>
        /// <param name="msg">msg</param>
        /// <param name="keyData">keyData</param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.CloseWindow(SubWindowResult.Cancel);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
