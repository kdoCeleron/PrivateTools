using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTools.Common.Utils
{
    using System.Reflection;

    using MyTools.Common.Interfaces;

    public static class PropertyUtils
    {
        /// <summary>
        /// プロパティ情報をアクセッサに変更します
        /// </summary>
        /// <param name="pi">プロパティ情報</param>
        /// <returns>アクセッサ</returns>
        public static IPropertyAccessor PropertyInfoToAccessor(PropertyInfo pi)
        {
            Type getterDelegateType = typeof(Func<,>).MakeGenericType(pi.DeclaringType, pi.PropertyType);
            var getMethodInfo = pi.GetGetMethod();
            Delegate getter = null;
            if (getMethodInfo != null)
            {
                getter = Delegate.CreateDelegate(getterDelegateType, getMethodInfo);
            }

            Type setterDelegateType = typeof(Action<,>).MakeGenericType(pi.DeclaringType, pi.PropertyType);
            var setMethodInfo = pi.GetSetMethod();
            Delegate setter = null;
            if (setMethodInfo != null)
            {
                setter = Delegate.CreateDelegate(setterDelegateType, setMethodInfo);
            }

            Type accessorType = typeof(PropertyAccessor<,>).MakeGenericType(pi.DeclaringType, pi.PropertyType);
            IPropertyAccessor accessor = (IPropertyAccessor)Activator.CreateInstance(accessorType, getter, setter);

            return accessor;
        }
    }
}
