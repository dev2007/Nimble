using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Contact
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 聊天消息
        /// </summary>
        MESSAGE = 0,
        /// <summary>
        /// 群消息
        /// </summary>
        GROUP = 1,
        /// <summary>
        /// 讨论组消息
        /// </summary>
        DISCUSS = 2
    }
}
