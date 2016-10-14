using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Contact
{
    /// <summary>
    /// 登录二维码状态
    /// </summary>
    public enum QRStatus
    {
        /// <summary>
        /// 未知
        /// </summary>
        UNKNOWN,
        /// <summary>
        /// 失效
        /// </summary>
        INVALID,
        /// <summary>
        /// 等待扫描
        /// </summary>
        WAIT,
        /// <summary>
        /// 确认中
        /// </summary>
        CONFIRMING,
        /// <summary>
        /// 已确认
        /// </summary>
        CONFIRMED
    }
}
