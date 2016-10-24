using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Turing.Model
{
    enum ResponseCode
    {
        /// <summary>
        /// 文本类
        /// </summary>
        TEXT = 100000,
        /// <summary>
        /// 链接类
        /// </summary>
        LINK = 200000,
        /// <summary>
        /// 新闻类
        /// </summary>
        NEWS = 302000,
        /// <summary>
        /// 菜谱类
        /// </summary>
        COOKBOOK = 308000,
        /// <summary>
        /// key异常
        /// </summary>
        WRONG_KEY = 40001,
        /// <summary>
        /// 请求内容空
        /// </summary>
        EMPTY_CONTENT = 40002,
        /// <summary>
        /// 请求超次数
        /// </summary>
        OUT_TIMES = 40003,
        /// <summary>
        /// 数据格式异常
        /// </summary>
        WRONG_DATA = 40004
    }
}
