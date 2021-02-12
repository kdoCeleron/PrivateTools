using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class InfoViewForm : Form
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
            msgList.Add("・追加のキャンセル時、列が追加されてしまう");
            msgList.Add("・初期表示時はグループによらない全タスクを表示する。");
            msgList.Add("・グループ選択時、サブグループに存在するタスクも表示する。");
            msgList.Add("・直近のタスク一覧に3日(黄色)以内のタスクを表示する。");
            msgList.Add("・サブ画面はescキーで閉じたい。");
            msgList.Add("・全体的に処理を共通化");

            var txt = string.Empty;
            foreach (var msg in msgList)
            {
                txt += msg + "\r\n";
            }

            this.TxtInfo.Text = txt;
        }
    }
}
