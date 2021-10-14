
namespace MyTools.Common
{
    using System;
    using System.Diagnostics;

    /// <summary>
    ///     処理時間計測を簡易に行うことができるクラスです。
    /// </summary>
    /// <remarks>
    ///     <see cref="IDisposable" />を実装しているため、usingで使用できます。
    /// </remarks>
    public class TimeTracer : IDisposable
    {
        /// <summary>
        ///     タイトル
        /// </summary>
        private readonly string _title;

        /// <summary>
        ///     ストップウォッチ
        /// </summary>
        private readonly Stopwatch _watch;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="title">タイトル</param>
        public TimeTracer(string title)
        {
            this._title = title;
            this._watch = Stopwatch.StartNew();
        }

        /// <summary>
        ///     デバッグ情報送信アクション
        /// </summary>
        public static Action<DateTime, string, long> DebugSender { get; set; }

        /// <summary>
        ///     処理時間を取得します。
        /// </summary>
        /// <remarks>
        ///     本プロパティは主にデバッグ用に利用されることを想定しています。
        ///     内部のStopwatchの値を返却するので、<see cref="Dispose" />メソッドが
        ///     呼ばれた後でないと正確な値が取得できません。
        /// </remarks>
        public TimeSpan Elapsed
        {
            get
            {
                return this._watch.Elapsed;
            }
        }

        /// <summary>
        ///     オブジェクトが保持しているリソースを破棄します。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     オブジェクトが保持しているリソースを破棄します。
        /// </summary>
        /// <param name="dispoing">
        ///     マネージドとアンマネージの両方を破棄する場合はTrue,それ以外はFalse.
        /// </param>
        protected virtual void Dispose(bool dispoing)
        {
            if (dispoing)
            {
                this._watch.Stop();

                var message = string.Format("【処理時間】{0} {1}", this._title, this._watch.Elapsed);
                
                Console.WriteLine(message);

                if (DebugSender != null)
                {
                    DebugSender(DateTime.Now, this._title, this._watch.ElapsedMilliseconds);
                }
            }
        }
    }
}