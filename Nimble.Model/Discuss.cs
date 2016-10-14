using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Model
{
    public class Discuss
    {
        public string name;
        public Dictionary<string, DiscussMember> MemberList = new Dictionary<string, DiscussMember>();

        public string Messages = "";
    }
}
