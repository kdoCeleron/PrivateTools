using System;
using System.Linq;

namespace MyTools.Common.Utils
{
    using System.Collections.Generic;

    /// <summary>
    ///     リスト関連のユーティリティ処理が定義されています
    /// </summary>
    public static class ListUtils
    {
        /// <summary>
        ///     文字列リストの連結
        /// </summary>
        /// <typeparam name="T">
        ///     データの型
        /// </typeparam>
        /// <param name="list">
        ///     文字列リスト
        /// </param>
        /// <param name="delimiter">
        ///     デリミタ
        /// </param>
        /// <returns>
        ///     文字列
        /// </returns>
        public static string JoinString<T>(IList<T> list, string delimiter)
        {
            var result = string.Empty;

            if ((list == null) || (list.Count == 0))
            {
                return result;
            }

            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (item == null)
                {
                    continue;
                }

                result += item.ToString();

                if (i == list.Count - 1)
                {
                    break;
                }

                result += delimiter;
            }

            return result;
        }

        /// <summary>
        /// リストの値を取得します
        /// </summary>
        /// <param name="list">リスト</param>
        /// <param name="index">インデックス</param>
        /// <returns>値</returns>
        public static string GetListValue(List<string> list, int index)
        {
            if ((list == null) || (list.Count <= index) || (index < 0))
            {
                return string.Empty;
            }

            return list[index];
        }

        public static bool IsEmpty<T>(this IList<T> self)
        {
            return self == null || self.Count == 0;
        }

        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> self, int chunkSize)
        {
            if (chunkSize <= 0)
            {
                throw new ArgumentException("チャンクサイズは０より大きな数でないといけません。", "chunkSize");
            }
            

            while (self.Any())
            {
                yield return self.Take(chunkSize);
                self = self.Skip(chunkSize);
            }
        }

    }
}