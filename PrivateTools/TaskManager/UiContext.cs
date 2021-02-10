using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManager
{
    public class UiContext
    {
        /// <summary>
        /// 同期コンテキスト
        /// </summary>
        private static SynchronizationContext _context;

        /// <summary>
        /// 内部に<see cref="SynchronizationContext"/>をキャプチャします。
        /// </summary>
        /// <remarks>
        /// <para>CREATE 2020-10-01 アルファテクノロジー 近藤 匠 </para>
        /// <para>UPDATE </para>
        /// </remarks>
        public static void Initialize()
        {
            _context = SynchronizationContext.Current;
        }

        /// <summary>
        /// 指定されたアクションをUIスレッド上で非同期実行します。
        /// </summary>
        /// <param name="action">アクション</param>
        /// <remarks>
        /// <para>CREATE 2020-10-01 アルファテクノロジー 近藤 匠 </para>
        /// <para>UPDATE </para>
        /// </remarks>
        public static void Post(Action action)
        {
            if (_context == null)
            {
                throw new InvalidOperationException("Initializeが呼ばれていません。");
            }

            _context.Post(state => { action(); }, null);
        }

        /// <summary>
        /// 指定されたアクションをUIスレッド上で非同期実行します。
        /// </summary>
        /// <param name="action">アクション</param>
        /// <param name="parameter">パラメータ</param>
        /// <remarks>
        /// <para>CREATE 2020-10-01 アルファテクノロジー 近藤 匠 </para>
        /// <para>UPDATE </para>
        /// </remarks>
        public static void Post(Action<object> action, object parameter)
        {
            if (_context == null)
            {
                throw new InvalidOperationException("Initializeが呼ばれていません。");
            }

            _context.Post(state => { action(state); }, parameter);
        }

        /// <summary>
        /// 指定されたアクションをUIスレッド上で同期実行します。
        /// </summary>
        /// <param name="action">アクション</param>
        /// <remarks>
        /// <para>CREATE 2020-10-01 アルファテクノロジー 近藤 匠 </para>
        /// <para>UPDATE </para>
        /// </remarks>
        public static void Send(Action action)
        {
            if (_context == null)
            {
                throw new InvalidOperationException("Initializeが呼ばれていません。");
            }

            _context.Send(state => { action(); }, new object());
        }

        /// <summary>
        /// 指定されたアクションをUIスレッド上で同期実行します。
        /// </summary>
        /// <param name="action">アクション</param>
        /// <param name="parameter">パラメータ</param>
        /// <remarks>
        /// <para>CREATE 2020-10-01 アルファテクノロジー 近藤 匠 </para>
        /// <para>UPDATE </para>
        /// </remarks>
        public static void Send(Action<object> action, object parameter)
        {
            if (_context == null)
            {
                throw new InvalidOperationException("Initializeが呼ばれていません。");
            }

            _context.Send(state => { action(state); }, parameter);
        }
    }
}
