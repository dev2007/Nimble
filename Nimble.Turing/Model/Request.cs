using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Turing.Model
{
    class Request
    {
        protected string Format = "key={0}&info={1}&userid={2}&loc={3}";
        public string key { get; set; }
        public string info { get; set; }
        public string userid { get; set; }
        public string loc { get; set; }

        public override string ToString()
        {
            return string.Format(Format, key, info, userid, loc);
        }
    }

    class LingshaRequest : Request
    {
        public new string key { get { return Define.LingshaKey; } }
        public override string ToString()
        {
            return string.Format(Format, Define.LingshaKey, info, userid, loc);
        }
    }
}
