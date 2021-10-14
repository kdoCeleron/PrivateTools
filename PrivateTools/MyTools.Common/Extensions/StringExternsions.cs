using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTools.Common.Extensions
{
    public static class StringExternsions
    {
        public static string Fmt(this string self, params object[] args)
        {
            return string.Format(self, args);
        }

        public static bool IsEmpty(this string self)
        {
            return string.IsNullOrEmpty(self);
        }

        public static DateTime ToDateTime(this string self, string format)
        {
            return DateTime.ParseExact(self, format, null);
        }
    }
}
