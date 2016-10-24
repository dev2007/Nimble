using Nimble.Model.JsonModel;
using NimbleFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Contact
{
    public class Message
    {
        public bool Running = true;
        private int Count103 = 0;

        private IDictionary<string, IQMessage> invokerDic;
        private string keyword = string.Empty;
        public Message()
        {
            invokerDic = new Dictionary<string, IQMessage>();
        }

        public bool BindInvoker(IQMessage invoker)
        {
            if (invokerDic.ContainsKey(invoker.GUID))
                return false;
            invokerDic.Add(invoker.GUID, invoker);
            return true;
        }

        public bool UnbindInvoker(IQMessage invoker)
        {
            if (invokerDic.ContainsKey(invoker.GUID))
            {
                invokerDic.Remove(invoker.GUID);
                return true;
            }
            return false;
        }

        public void SetResponseKeyword(string keyword)
        {
            this.keyword = keyword;
        }

        public bool RemoveAllInvoker()
        {
            invokerDic.Clear();
            return true;
        }

        /// <summary>
        /// 私聊消息处理
        /// </summary>
        /// <param name="value">poll包中的value</param>
        public void ProcessMsg(JsonPollMessage.paramResult.paramValue value, Action<int, string, string> action)
        {
            string message = Message_Process_GetMessageText(value.content);
            var responseFlag = message.Contains(keyword);
            if (action != null && responseFlag)
            {
                foreach (var invoker in invokerDic)
                {
                    action((int)MessageType.MESSAGE, value.from_uin, ProcessMsg(message, invoker.Value));
                }
            }
        }

        /// <summary>
        /// 群聊消息处理
        /// </summary>
        /// <param name="value">poll包中的value</param>
        public void GroupMessage(JsonPollMessage.paramResult.paramValue value, Action<int, string, string> action)
        {
            string message = Message_Process_GetMessageText(value.content);
            var responseFlag = message.Contains(keyword);
            if (action != null && responseFlag)
            {
                foreach (var invoker in invokerDic)
                {
                    action((int)MessageType.GROUP, value.from_uin, ProcessMsg(message, invoker.Value));
                }
            }
        }

        /// <summary>
        /// 讨论组消息处理
        /// </summary>
        /// <param name="value">poll包中的value</param>
        public void DisscussMessage(JsonPollMessage.paramResult.paramValue value, Action<int, string, string> action)
        {
            string message = Message_Process_GetMessageText(value.content);
            var responseFlag = message.Contains(keyword);
            if (action != null && responseFlag)
            {
                foreach (var invoker in invokerDic)
                {
                    action((int)MessageType.DISCUSS, value.from_uin, ProcessMsg(message, invoker.Value));
                }
            }
        }

        private string ProcessMsg(string message, IQMessage invoker)
        {
            var returnMsg = invoker.Process(message);
            if (invoker.ShowTail)
            {
                returnMsg += string.Format("【{0}】", invoker.AppName);
            }
            return returnMsg;
        }


        /// <summary>
        /// 处理poll包中的消息数组
        /// </summary>
        /// <param name="content">消息数组</param>
        /// <returns></returns>
        public string Message_Process_GetMessageText(List<object> content)
        {
            string message = "";
            for (int i = 1; i < content.Count; i++)
            {
                if (content[i].ToString().Contains("[\"cface\","))
                    continue;
                else if (content[i].ToString().Contains("\"face\","))
                    message += ("{..[face" + content[i].ToString().Replace("\"face\",", "").Replace("]", "").Replace("[", "").Replace(" ", "").Replace("\r", "").Replace("\n", "") + "]..}");
                else
                    message += content[i].ToString();
            }
            message = message.Replace("\\\\n", Environment.NewLine).Replace("＆", "&");
            return message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poll"></param>
        /// <returns></returns>
        public string ErrorMsg(JsonPollMessage poll)
        {
            int TempCount103 = Count103;
            Count103 = 0;
            if (poll.retcode == 102)
            {
                return "";
            }
            else if (poll.retcode == 103)
            {
                Count103 = TempCount103 + 1;
                if (Count103 > 20)
                {
                    Running = false;
                }
                return "";
            }
            else if (poll.retcode == 116)
            {
                return poll.p;
            }
            else if (poll.retcode == 108 || poll.retcode == 114)
            {
                Running = false;
                return "";
            }
            else if (poll.retcode == 120 || poll.retcode == 121)
            {
                Running = false;
                return "";
            }
            else if (poll.retcode == 100006 || poll.retcode == 100003)
            {
                Running = false;
                return "";
            }
            return "";
        }
    }
}
