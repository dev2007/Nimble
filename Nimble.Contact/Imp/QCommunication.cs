using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Nimble.Contact.Utils;
using System.Web;
using Nimble.Model;
using Nimble.Model.JsonModel;
using Newtonsoft.Json;

namespace Nimble.Contact.Imp
{
    public class QCommunication
    {
        public static string QQNum { get; private set; }
        public static Friend SelfInfo = new Friend();

        private Random rand = new Random();

        private static string vfwebqq, ptwebqq, psessionid, uin, hash;

        private Message message;

        public QCommunication(Message msg)
        {
            this.message = msg;
        }

        /// <summary>
        /// 获取登录二维码
        /// </summary>
        /// <returns></returns>
        public Stream GetLoginQR()
        {
            try
            {
                return Http.EasyGet(string.Format(UrlDefine.QR, rand.NextDouble().ToString()));
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// 获取当前二维码扫描情况
        /// </summary>
        /// <param name="loginSuccessAction"></param>
        /// <returns></returns>
        public QRStatus GetQRStatus(Action loginSuccessAction = null)
        {
            string result;
            result = Http.Get(UrlDefine.QR_Status, UrlDefine.QR_Status_Refer);
            if (string.IsNullOrEmpty(result))
                return QRStatus.UNKNOWN;

            string[] status = result.Split('\'');
            switch (status[1])
            {
                case ("65")://二维码失效，刷新 
                    return QRStatus.INVALID;
                case ("66"): //等待扫描                                           
                    return QRStatus.WAIT;
                case ("67")://等待确认
                    return QRStatus.CONFIRMING;
                case ("0")://已经确认
                    LoginSuccess(status[5]);
                    if (loginSuccessAction != null)
                    {
                        loginSuccessAction();
                    }
                    return QRStatus.CONFIRMED;
                default:
                    return QRStatus.UNKNOWN;
            }
        }


        /// <summary>
        /// 刷新二维码
        /// </summary>
        /// <param name="refreshAction"></param>
        /// <returns></returns>
        public Stream RefreshQR()
        {
            Stream s = GetLoginQR();
            return s;
        }

        /// <summary>
        /// 登录成功的操作
        /// </summary>
        /// <param name="url"></param>
        private void LoginSuccess(string url)
        {
            GetPTWebQQ(url);
            GetVfWebQQ();
            GetPSessionId();
            GetFriendList();
            GetGroupList();
            GetDiscussList();
            GetSelfInfo();
            GetOnlineAndRecent();
        }

        private void GetFriendList()
        {
            string url = UrlDefine.Friend_List;
            string sendData = string.Format("r={{\"vfwebqq\":\"{0}\",\"hash\":\"{1}\"}}", vfwebqq, hash);
            string dat = Http.Post(url, sendData, UrlDefine.Friend_List_Refer);
        }

        private void GetGroupList()
        {
            string url = UrlDefine.Group_List;
            string sendData = string.Format("r={{\"vfwebqq\":\"{0}\",\"hash\":\"{1}\"}}", vfwebqq, hash);
            string dat = Http.Post(url, sendData, UrlDefine.Group_List_Refer);
        }

        private void GetDiscussList()
        {
            string url = UrlDefine.Discuss_List.Replace("#{psessionid}", psessionid).Replace("#{vfwebqq}", vfwebqq).Replace("#{t}", Utility.AID_TimeStamp());
            string dat = Http.Get(url, UrlDefine.Discuss_List_Refer);
        }

        /// <summary>
        /// 获取自己的信息
        /// </summary>
        private void GetSelfInfo()
        {
            string url = UrlDefine.Self_Info.Replace("#{t}", Utility.AID_TimeStamp());
            string dat = Http.Get(url, UrlDefine.Self_Info_Refer);
            JsonFriend inf = (JsonFriend)JsonConvert.DeserializeObject(dat, typeof(JsonFriend));

            SelfInfo.face = inf.result.face;
            SelfInfo.occupation = inf.result.occupation;
            SelfInfo.phone = inf.result.phone;
            SelfInfo.college = inf.result.college;
            SelfInfo.blood = inf.result.blood;
            SelfInfo.homepage = inf.result.homepage;
            SelfInfo.vip_info = inf.result.vip_info;
            SelfInfo.country = inf.result.country;
            SelfInfo.city = inf.result.city;
            SelfInfo.personal = inf.result.personal;
            SelfInfo.nick = inf.result.nick;
            SelfInfo.shengxiao = inf.result.shengxiao;
            SelfInfo.email = inf.result.email;
            SelfInfo.province = inf.result.province;
            SelfInfo.gender = inf.result.gender;
            if (inf.result.birthday.year != 0 && inf.result.birthday.month != 0 && inf.result.birthday.day != 0)
                SelfInfo.birthday = new DateTime(inf.result.birthday.year, inf.result.birthday.month, inf.result.birthday.day);
        }


        /// <summary>
        /// 根据uin获取真实QQ号
        /// </summary>
        /// <param name="uin"></param>
        /// <returns></returns>
        public static string GetRealQQ(string uin)
        {
            string url = UrlDefine.REAL_QQ.Replace("#{uin}", uin).Replace("#{vfwebqq}", vfwebqq).Replace("#{t}", Utility.AID_TimeStamp());
            string data = Http.Get(url);
            string temp = data.Split('\"')[10].Split(',')[0].Replace(":", "");

            return temp;
        }


        /// <summary>
        /// 拉取消息
        /// </summary>
        public bool Poll()
        {
            if (!message.Running)
                return false;
            try
            {
                string HeartPackdata = UrlDefine.HeartPackdata.Replace("#{ptwebqq}", ptwebqq).Replace("#{psessionid}", psessionid);
                HeartPackdata = "r=" + HttpUtility.UrlEncode(HeartPackdata);
                Http.Post_Async_Action action = Message_Get;
                Http.Post_Async(UrlDefine.POLL, HeartPackdata, action);
            }
            catch (Exception)
            {
                Poll();
            }
            return true;
        }

        private void Message_Get(string data)
        {
            Task.Run(() =>
            {
                Poll();
            });

            JsonPollMessage poll = (JsonPollMessage)JsonConvert.DeserializeObject(data, typeof(JsonPollMessage));
            if (poll.retcode != 0)
            {
                var msg = message.ErrorMsg(poll);
                if (!string.IsNullOrEmpty(msg))
                    ptwebqq = msg;
            }
            else if (poll.result != null && poll.result.Count > 0)
                for (int i = 0; i < poll.result.Count; i++)
                {
                    switch (poll.result[i].poll_type)
                    {
                        case "kick_message":
                            message.Running = false;
                            break;
                        case "message":
                            message.ProcessMsg(poll.result[i].value, SendMsgAction);
                            break;
                        case "group_message":
                            message.GroupMessage(poll.result[i].value, SendMsgAction);
                            break;
                        case "discu_message":
                            message.DisscussMessage(poll.result[i].value, SendMsgAction);
                            break;
                        default:
                            //poll.result[i].poll_type;
                            break;
                    }
                }
        }

        private void SendMsgAction(int type, string uid, string sendMsg)
        {
            if(!Message_Send(type, uid, sendMsg))
            {
                Message_Send(type, uid, sendMsg);
            }
        }


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="type">接受者类型：0，用户；1，群；2，讨论组</param>
        /// <param name="id">用户：uid；群：qid；讨论组：did</param>
        /// <param name="messageToSend">要发送的消息</param>
        /// <returns></returns>
        public bool Message_Send(int type, string id, string messageToSend)
        {
            if (messageToSend.Equals("") || id.Equals(""))
                return false;

            string[] tmp = messageToSend.Split("{}".ToCharArray());
            messageToSend = "";
            for (int i = 0; i < tmp.Length; i++)
                if (!tmp[i].Trim().StartsWith("..[face") || !tmp[i].Trim().EndsWith("].."))
                    messageToSend += "\\\"" + tmp[i] + "\\\",";
                else
                    messageToSend += tmp[i].Replace("..[face", "[\\\"face\\\",").Replace("]..", "],");
            messageToSend = messageToSend.Remove(messageToSend.LastIndexOf(','));
            messageToSend = messageToSend.Replace("\r\n", "\n").Replace("\n\r", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
            try
            {
                string to_groupuin_did, url;
                switch (type)
                {
                    case 0:
                        to_groupuin_did = "to";
                        url = UrlDefine.Send_User;
                        break;
                    case 1:
                        to_groupuin_did = "group_uin";
                        url = UrlDefine.Send_Group;
                        break;
                    case 2:
                        to_groupuin_did = "did";
                        url = UrlDefine.Send_Discuss;
                        break;
                    default:
                        return false;
                }
                string postData = "{\"#{type}\":#{id},\"content\":\"[#{msg},[\\\"font\\\",{\\\"name\\\":\\\"宋体\\\",\\\"size\\\":10,\\\"style\\\":[0,0,0],\\\"color\\\":\\\"000000\\\"}]]\",\"face\":#{face},\"clientid\":53999199,\"msg_id\":#{msg_id},\"psessionid\":\"#{psessionid}\"}";
                postData = "r=" + HttpUtility.UrlEncode(postData.Replace("#{type}", to_groupuin_did).Replace("#{id}", id).Replace("#{msg}", messageToSend).Replace("#{face}", SelfInfo.face.ToString()).Replace("#{msg_id}", rand.Next(10000000, 99999999).ToString()).Replace("#{psessionid}", psessionid));

                string dat = Http.Post(url, postData, "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2");
                return dat.Equals("{\"errCode\":0,\"msg\":\"send ok\"}");
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void GetPTWebQQ(string url)
        {
            string dat = Http.Get(url, UrlDefine.PT);
            Uri uri = new Uri(UrlDefine.Host);
            ptwebqq = Http.Cookies.GetCookies(uri)["ptwebqq"].Value;
        }

        private void GetVfWebQQ()
        {
            string url = UrlDefine.VF.Replace("#{ptwebqq}", ptwebqq).Replace("#{t}", Utility.AID_TimeStamp());
            string dat = Http.Get(url, UrlDefine.VF_Refer);
            vfwebqq = dat.Split('\"')[7];
        }

        private void GetPSessionId()
        {
            string url1 = UrlDefine.PerssionData_Format.Replace("#{ptwebqq}", ptwebqq);
            url1 = "r=" + HttpUtility.UrlEncode(url1);
            string dat = Http.Post(UrlDefine.Pession, url1, UrlDefine.Pession_Refer);
            psessionid = dat.Replace(":", ",").Replace("{", "").Replace("}", "").Replace("\"", "").Split(',')[10];
            QQNum = uin = dat.Replace(":", ",").Replace("{", "").Replace("}", "").Replace("\"", "").Split(',')[14];
            hash = AID_Hash(QQNum, ptwebqq);
        }

        private static void GetOnlineAndRecent()
        {
            string url = "http://d1.web2.qq.com/channel/get_online_buddies2?vfwebqq=#{vfwebqq}&clientid=53999199&psessionid=#{psessionid}&t=#{t}".Replace("#{vfwebqq}", vfwebqq).Replace("#{psessionid}", psessionid).Replace("#{t}", Utility.AID_TimeStamp());
            Http.Get(url, "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2");

            url = "http://d1.web2.qq.com/channel/get_recent_list2";
            string url1 = "{\"vfwebqq\":\"#{vfwebqq}\",\"clientid\":53999199,\"psessionid\":\"#{psessionid}\"}".Replace("#{vfwebqq}", vfwebqq).Replace("#{psessionid}", psessionid);
            string dat = Http.Post(url, "r=" + HttpUtility.UrlEncode(url1), "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2");
        }

        /// <summary>
        /// 根据QQ号和ptwebqq值获取hash值，用于获取好友列表和群列表
        /// </summary>
        /// <param name="QQNum">QQ号</param>
        /// <param name="ptwebqq">ptwebqq</param>
        /// <returns>hash值</returns>
        private static string AID_Hash(string QQNum, string ptwebqq)
        {
            int[] N = new int[4];
            long QQNum_Long = long.Parse(QQNum);
            for (int T = 0; T < ptwebqq.Length; T++)
            {
                N[T % 4] ^= ptwebqq.ToCharArray()[T];
            }
            string[] U = { "EC", "OK" };
            long[] V = new long[4];
            V[0] = QQNum_Long >> 24 & 255 ^ U[0].ToCharArray()[0];
            V[1] = QQNum_Long >> 16 & 255 ^ U[0].ToCharArray()[1];
            V[2] = QQNum_Long >> 8 & 255 ^ U[1].ToCharArray()[0];
            V[3] = QQNum_Long & 255 ^ U[1].ToCharArray()[1];

            long[] U1 = new long[8];

            for (int T = 0; T < 8; T++)
            {
                U1[T] = T % 2 == 0 ? N[T >> 1] : V[T >> 1];
            }

            string[] N1 = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
            string V1 = "";

            for (int i = 0; i < U1.Length; i++)
            {
                V1 += N1[(int)((U1[i] >> 4) & 15)];
                V1 += N1[(int)(U1[i] & 15)];
            }
            return V1;
        }
    }
}
