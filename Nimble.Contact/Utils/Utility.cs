using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Contact.Utils
{
    public class Utility
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="type">1，10位；2，13位。</param>
        /// <returns></returns>
        public static string AID_TimeStamp(int type = 1)
        {
            if (type == 1)
            {
                DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return ((DateTime.UtcNow.Ticks - dt1970.Ticks) / 10000).ToString();
            }
            else if (type == 2)
            {
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return Convert.ToInt64(ts.TotalSeconds).ToString();
            }
            else return "ERROR";
        }
    }
}
