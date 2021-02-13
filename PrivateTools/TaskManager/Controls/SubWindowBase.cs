using System;
using System.Collections.Generic;
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

        protected SubWindowResult windowResult;

        public Task<SubWindowResult> ShowWindow(Form parent)
        {
            this._tcs = new TaskCompletionSource<SubWindowResult>();

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

            this.Show();
            
            return this._tcs.Task;
        }
    }
}
