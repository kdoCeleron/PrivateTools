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
    public class SubWindowBase : Form
    {
        /// <summary>
        ///     タスク処理の結果
        /// </summary>
        private TaskCompletionSource<SubWindowResult> _tcs;
        
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

            //this.Owner.Enabled = false;

            this.Show();

            return this._tcs.Task;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            this._tcs.TrySetResult(SubWindowResult.None);

            //this.Owner.Enabled = true;
        }

        protected void CloseWindow(SubWindowResult result)
        {
            this._tcs.SetResult(result);
            this.Close();
        }
    }
}
