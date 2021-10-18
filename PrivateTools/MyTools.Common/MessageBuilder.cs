using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyTools.Common.Extensions;
using MyTools.Common.Utils;

namespace MyTools.Common
{
    /// <summary>
    /// メッセージビルダー
    /// </summary>
    public class MessageBuilder
    {
        /// <summary>
        /// メッセージ情報
        /// </summary>
        private List<MessageItem> messages = new List<MessageItem>();

        /// <summary>
        /// メッセージを追加します
        /// </summary>
        /// <param name="msgKind">メッセージ種別</param>
        /// <param name="message">メッセージ</param>
        /// <param name="args">メッセージのフォーマット引数</param>
        public void Add(MessageKind msgKind, string message, params object[] args)
        {
            var item = new MessageItem();
            item.MessageKind = msgKind;
            item.Message = message.Fmt(args);

            this.messages.Add(item);
        }

        /// <summary>
        /// メッセージを生成します。
        /// </summary>
        /// <param name="connector">結合文字</param>
        /// <returns>生成結果</returns>
        public string CreateMessage(string connector = "\r\n")
        {
            var tmp = this.messages.Select(x => "{0}：{1}".Fmt(this.GetMessageKindStr(x.MessageKind), x.Message)).ToList();

            return ListUtils.JoinString(tmp, connector);
        }

        private string GetMessageKindStr(MessageKind messageKind)
        {
            var ret = "未定義";
            switch (messageKind)
            {
                case MessageKind.Confirm:
                    ret = "確認";
                    break;
                case MessageKind.Info:
                    ret = "情報";
                    break;
                case MessageKind.Warn:
                    ret = "警告";
                    break;
                case MessageKind.Error:
                    ret = "エラー";
                    break;
            }

            return "[{0}]".Fmt(ret);
        }

        /// <summary>
        /// メッセージ譲歩う
        /// </summary>
        private class MessageItem
        {
            /// <summary>
            /// エラー種別
            /// </summary>
            public MessageKind MessageKind { get; set; }

            /// <summary>
            /// メッセージ
            /// </summary>
            public string Message { get; set; }
        }
    }
}
