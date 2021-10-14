namespace MyTools.Common.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// ログメッセージ作成ユーティリティクラス
    /// </summary>
    public static class LogUtils
    {
        /// <summary>
        /// 処理開始文言を生成
        /// </summary>
        /// <param name="eventName">イベント名</param>
        /// <param name="args">可変引数(ToVStrやToRStrで変換した文字列を複数件)</param>
        /// <returns>ログ文字列</returns>
        public static string Begin(string eventName, params object[] args)
        {
            return $@"{eventName}【開始】{JoinArgs(args)}";
        }

        /// <summary>
        /// 処理終了文言を生成
        /// </summary>
        /// <param name="eventName">イベント名</param>
        /// <param name="args">可変引数(ToVStrやToRStrで変換した文字列を複数件)</param>
        /// <returns>ログ文字列</returns>
        public static string End(string eventName, params object[] args)
        {
            return $@"{eventName}【終了】{JoinArgs(args)}";
        }

        /// <summary>
        /// 値を出力する文言を生成
        /// </summary>
        /// <param name="comment">コメント</param>
        /// <param name="args">可変引数(ToVStrやToRStrで変換した文字列を複数件)</param>
        /// <returns>ログ文字列</returns>
        public static string Variable(string comment, params object[] args)
        {
            return $@"{comment} {JoinArgs(args)}";
        }

        /// <summary>
        /// 例外を出力する文言を生成
        /// </summary>
        /// <param name="comment">コメント</param>
        /// <param name="args">可変引数(ToVStrやToRStrで変換した文字列を複数件)</param>
        /// <returns>ログ文字列</returns>
        public static string Exception(string comment, params object[] args)
        {
            return $@"{comment} {JoinArgs(args)}";
        }

        /// <summary>
        /// 値型を[パラメータ名=値]文字列に変換.
        /// </summary>
        /// <param name="o">文字列変換したいオブジェクト</param>
        /// <param name="paramName">パラメータ名</param>
        /// <returns>[パラメータ名=値]文字列</returns>
        public static string ToVStr(this object o, string paramName = null)
        {
            if (o == null)
            {
                return string.Empty;
            }

            return $@"{paramName ?? o.GetType().Name}={o}";
        }

        /// <summary>
        /// クラス型を[パラメータ名=値]文字列に変換.
        /// </summary>
        /// <param name="o">文字列変換したいオブジェクト</param>
        /// <param name="paramName">パラメータ名</param>
        /// <returns>文字列</returns>
        public static string ToRStr(this object o, string paramName = null)
        {
            if (o == null)
            {
                return string.Empty;
            }

            var l = new List<string>();

            foreach (var fi in o.GetType().GetFields())
            {
                l.Add($"{fi.Name}:{fi.GetValue(o)}");
            }

            foreach (var pi in o.GetType().GetProperties())
            {
                l.Add($"{pi.Name}:{pi.GetValue(o, null)}");
            }

            var sb = new StringBuilder(paramName ?? o.GetType().Name);
            sb.Append($"={{{string.Join(", ", l.ToArray())}}}");

            return sb.ToString();
        }

        /// <summary>
        /// シリアライズデータを出力します
        /// </summary>
        /// <param name="self">自分自身</param>
        /// <param name="path">出力先パス</param>
        public static void BinarySave(this byte[] self, string path)
        {
            if ((null == self) || string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException();
            }

            // ディレクトリがない場合は作っておく。
            var dir = Path.GetDirectoryName(path);
            if (null != dir)
            {
                Directory.CreateDirectory(dir);
            }

            // ファイルに出力する.
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(self, 0, self.Length);
            }
        }

        /// <summary>
        /// 可変引数を連結して文字列化する.
        /// </summary>
        /// <param name="args">オブジェクト配列</param>
        /// <returns>連結した文字列</returns>
        private static string JoinArgs(object[] args)
        {
            return args.Any() ? string.Join(" ", args) : string.Empty;
        }

    }
}