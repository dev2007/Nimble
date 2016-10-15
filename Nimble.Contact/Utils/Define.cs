using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Contact.Utils
{
    class Define
    {
        public const int Timeout = 100000;
        public const string Refer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";
        public const string UserAgent = "Mozilla/5.0 (Windows NT 10.0;%20WOW64; rv:47.0) Gecko/20100101 Firefox/47.0";
    }

    class UrlDefine
    {
        public const string Host = "http://web2.qq.com/";
        public const string QR = "https://ssl.ptlogin2.qq.com/ptqrshow?appid=501004106&e=0&l=M&s=5&d=72&v=4&t={0}";
        public const string QR_Status = "https://ssl.ptlogin2.qq.com/ptqrlogin?webqq_type=10&remember_uin=1&login2qq=1&aid=501004106 &u1=http%3A%2F%2Fw.qq.com%2Fproxy.html%3Flogin2qq%3D1%26webqq_type%3D10 &ptredirect=0&ptlang=2052&daid=164&from_ui=1&pttype=1&dumy=&fp=loginerroralert &action=0-0-157510&mibao_css=m_webqq&t=1&g=1&js_type=0&js_ver=10143&login_sig=&pt_randsalt=0";        
        public const string QR_Status_Refer = "https://ui.ptlogin2.qq.com/cgi-bin/login?daid=164&target=self&style=16&mibao_css=m_webqq&appid=501004106&enable_qlogin=0&no_verifyimg=1 &s_url=http%3A%2F%2Fw.qq.com%2Fproxy.html&f_url=loginerroralert &strong_login=1&login_state=10&t=20131024001";
        public const string PT = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
        public const string VF = "http://s.web2.qq.com/api/getvfwebqq?ptwebqq=#{ptwebqq}&clientid=53999199&psessionid=&t=#{t}";
        public const string Pession = "http://d1.web2.qq.com/channel/login2";
        public const string PerssionData_Format = "{\"ptwebqq\":\"#{ptwebqq}\",\"clientid\":53999199,\"psessionid\":\"\",\"status\":\"online\"}";
        public const string Pession_Refer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";
        public const string REAL_QQ = "http://s.web2.qq.com/api/get_friend_uin2?tuin=#{uin}&type=1&vfwebqq=#{vfwebqq}&t=#{t}";
        public const string POLL = "http://d1.web2.qq.com/channel/poll2";
        public const string HeartPackdata = "{\"ptwebqq\":\"#{ptwebqq}\",\"clientid\":53999199,\"psessionid\":\"#{psessionid}\",\"key\":\"\"}";
        public const string Friend_List = "http://s.web2.qq.com/api/get_user_friends2";
        public const string Group_List = "http://s.web2.qq.com/api/get_group_name_list_mask2";
        public const string Discuss_List = "http://s.web2.qq.com/api/get_discus_list?clientid=53999199&psessionid=#{psessionid}&vfwebqq=#{vfwebqq}&t=#{t}";
        public const string Self_Info = "http://s.web2.qq.com/api/get_self_info2?t=#{t}";
        public const string Send_User = "http://d1.web2.qq.com/channel/send_buddy_msg2";
        public const string Send_Group = "http://d1.web2.qq.com/channel/send_qun_msg2";
        public const string Send_Discuss = "http://d1.web2.qq.com/channel/send_discu_msg2";
        public const string Recent_List = "http://d1.web2.qq.com/channel/get_recent_list2";
        public const string Online_Recent = "http://d1.web2.qq.com/channel/get_online_buddies2?vfwebqq=#{vfwebqq}&clientid=53999199&psessionid=#{psessionid}&t=#{t}";
    }
}
