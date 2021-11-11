using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyTools.Common.Utils
{
    /// <summary>
    /// タイマ管理クラス
    /// </summary>
    public static class TimerManager
    {
        /// <summary>
        /// 内部管理中のタイマ
        /// </summary>
        private static Dictionary<int, Timer> _timers = new Dictionary<int, Timer>();

        /// <summary>
        /// タイマを生成します。
        /// </summary>
        /// <param name="callBack">タイムアウト時のコールバック</param>
        /// <returns>タイマを一意に特定するキー</returns>
        public static int CreateTimer(TimerCallback callBack)
        {
            var timer = new Timer(callBack);
            var key = timer.GetHashCode();

            _timers.Add(key, timer);
            

            return key;
        }

        /// <summary>
        /// タイマの時間間隔を設定します。
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="dueTime">開始時刻</param>
        /// <param name="period">時間間隔</param>
        /// <returns>設定結果</returns>
        public static bool Change(int key, TimeSpan dueTime, TimeSpan period)
        {
            if (!_timers.ContainsKey(key))
            {
                return false;
            }

            return _timers[key].Change(dueTime, period);
        }

        /// <summary>
        /// タイマの破棄を行います。
        /// </summary>
        /// <param name="key">キー</param>
        public static void DisposeTimer(int key)
        {
            if (!_timers.ContainsKey(key))
            {
                return;
            }

            var timer = _timers[key];
            _timers.Remove(key);
            timer.Dispose();
        }
    }
}
