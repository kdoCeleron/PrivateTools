using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTools.Common
{
    using MyTools.Common.Interfaces;

    /// <summary>
    /// プロパティアクセッサ
    /// </summary>
    /// <typeparam name="TTarget">ターゲット</typeparam>
    /// <typeparam name="TProperty">プロパティ</typeparam>
    public class PropertyAccessor<TTarget, TProperty> : IPropertyAccessor
    {
        /// <summary>
        /// 取得処理
        /// </summary>
        private Func<TTarget, TProperty> getter;

        /// <summary>
        /// 設定処理
        /// </summary>
        private Action<TTarget, TProperty> setter;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="getter">取得処理</param>
        /// <param name="setter">設定処理</param>
        public PropertyAccessor(Func<TTarget, TProperty> getter, Action<TTarget, TProperty> setter)
        {
            this.getter = getter;
            this.setter = setter;
        }

        /// <summary>
        /// 値を取得します
        /// </summary>
        /// <param name="target">インスタンス</param>
        /// <returns>プロパティ値</returns>
        public object GetValue(object target)
        {
            return this.getter((TTarget)target);
        }

        /// <summary>
        /// 値を設定します
        /// </summary>
        /// <param name="target">インスタンス</param>
        /// <param name="value">プロパティ値</param>
        public void SetValue(object target, object value)
        {
            this.setter((TTarget)target, (TProperty)value);
        }
    }
}
