using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Model
{
    public class Group
    {
        public string name;
        public string code;
        public string markname;
        public string memo;
        public int face;
        public string createtime;
        public int level;
        public string owner;
        public GroupManage GroupManage = new GroupManage();
      
        public Dictionary<string, GroupMember> MemberList = new Dictionary<string, GroupMember>();
      
        public string Messages = "";
    }
}
