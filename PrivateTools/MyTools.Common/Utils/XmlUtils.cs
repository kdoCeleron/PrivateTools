using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTools.Common.Utils
{
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;

    /// <summary>
    /// XML関連のユーティリティ
    /// </summary>
    public static class XmlUtils
    {
        /// <summary>
        /// プロパティのサニタイズ
        /// </summary>
        /// <param name="obj">サニタイズ対象オブジェクト</param>
        public static void SanitizeProperties(object obj)
        {
            if (obj == null)
            {
                return;
            }

            try
            {
                Type objType = obj.GetType();
                PropertyInfo[] properties = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        object propValue = property.GetValue(obj, null);
                        if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                        {
                            if (property.PropertyType == typeof(string) && property.GetSetMethod() != null)
                            {
                                property.SetValue(obj, Sanitize(propValue.ToString()));
                            }
                        }
                        else if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                        {
                            IEnumerable enumerable = (IEnumerable)propValue;
                            foreach (object child in enumerable)
                            {
                                SanitizeProperties(child);
                            }
                        }
                        else
                        {
                            SanitizeProperties(propValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                    }
                }
            }
            catch (Exception ex2)
            {
                Trace.WriteLine(ex2.ToString());
            }
        }

        /// <summary>
        /// サニタイズを行います。
        /// </summary>
        /// <param name="str">対象の文字列</param>
        /// <returns>サニタイズ後の文字列</returns>
        public static string Sanitize(string str)
        {
            var sb = new StringBuilder();

            foreach (var c in str)
            {
                var code = (int)c;

                if (code == 0x9 ||
                    code == 0xa ||
                    code == 0xd ||
                    (0x20 <= code && code <= 0xd7ff) ||
                    (0xe000 <= code && code <= 0xfffd) ||
                    (0x10000 <= code && code <= 0x10ffff))
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
    }
}
