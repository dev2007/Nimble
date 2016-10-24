using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble
{
    public class RobotConfig
    {
        /// <summary>
        /// 响应消息的前缀
        /// </summary>
        public static string ResponseKeyword { get; set; }
        /// <summary>
        /// 消息是否响应
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool IsResponse(string message)
        {
            if (string.IsNullOrEmpty(ResponseKeyword))
                return true;

            return message.Contains(ResponseKeyword);
        }
    }
}
