using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble
{
    public class RobotConfig
    {
        private static string keyword = "@机器人";
        /// <summary>
        /// 响应消息的关键字
        /// </summary>
        public static string ResponseKeyword { get { return keyword; } set { if (!string.IsNullOrEmpty(keyword)) keyword = value; } }
    }
}
