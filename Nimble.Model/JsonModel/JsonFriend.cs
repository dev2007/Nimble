using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Model.JsonModel
{
    public class JsonFriend
    {
        public int retcode;
        public paramResult result;
        public class paramResult
        {
            public int face;
            public paramBirthday birthday;
            public string occupation;
            public string phone;
            public string college;
            public int constel;
            public int blood;
            public string homepage;
            public int stat;
            public string city;
            public string personal;
            public string nick;
            public int shengxiao;
            public string email;
            public string province;
            public string gender;
            public string mobile;
            public string country;
            public int vip_info;

            public class paramBirthday
            {
                public int month;
                public int year;
                public int day;
            }
        }
    }
}
