using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Turing.Model
{
    class Response
    {
        /// <summary>
        /// 标识码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 提示语
        /// </summary>
        public string text { get; set; }
    }

    class ResponseLink : Response
    {
        /// <summary>
        /// 链接类的链接
        /// </summary>
        public string url { get; set; }
    }

    class ResponseNews : Response
    {
        /// <summary>
        /// 新闻列表
        /// </summary>
        public List<News> list { get; set; }
    }

    class ResponseCookbook : Response
    {
        /// <summary>
        /// 菜谱列表
        /// </summary>
        public List<CookBook> list { get; set; }
    }
}
