using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Turing.Model
{
    class News
    {
        /// <summary>
        /// 新闻标题
        /// </summary>
        public string article { get; set; }
        /// <summary>
        /// 新闻来源
        /// </summary>
        public string source { get; set; }
        /// <summary>
        /// 新闻图片
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// 新闻详情链接
        /// </summary>
        public string detailurl { get; set; }
    }
}
