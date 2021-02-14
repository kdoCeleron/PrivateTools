using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManager.Controls;

namespace TaskManager
{
    public partial class InfoViewForm : SubWindowBase
    {
        public InfoViewForm()
        {
            InitializeComponent();

            this.TxtInfo.Enabled = false;

            this.Load += OnLoad;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            var msgList = new List<string>();
            msgList.Add("★実装予定★");
            msgList.Add("・タスク編集画面で添付ファイルの機能実装");
            msgList.Add("・グループ選択時、サブグループに存在するタスクも表示する。");
            msgList.Add("・サブ画面表示中は、本体画面を触れない様にする。");
            msgList.Add("==========================");
            msgList.Add("★内部予定★");
            msgList.Add("・各種定数をConfig化");
            msgList.Add("・全体的に処理を共通化");
            msgList.Add("==========================");

            var txt = string.Empty;
            foreach (var msg in msgList)
            {
                txt += msg + "\r\n";
            }

            this.TxtInfo.Text = txt;
        }
    }
}
