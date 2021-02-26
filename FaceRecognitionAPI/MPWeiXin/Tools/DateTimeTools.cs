using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPWeiXin.Tools
{
    class DateTimeTools
    {
        public static long GetDateTimeInt() {
            return (long)(DateTime.Now - DateTime.Parse("1970-1-1 00:00:00")).TotalMilliseconds;
        }
    }
}
