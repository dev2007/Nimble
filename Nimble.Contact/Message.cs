using Nimble.Model.JsonModel;
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

        public Message()
        {

        }

        /// <summary>
        /// 私聊消息处理
        /// </summary>
        /// <param name="value">poll包中的value</param>
        public void ProcessMsg(JsonPollMessage.paramResult.paramValue value,Action<int,string,string> action)
        {
            string message = Message_Process_GetMessageText(value.content);
            //TODO：invoker 插件

            if (action != null)
            {
                action(0, value.from_uin, message);
            }
        }

        /// <summary>
        /// 群聊消息处理
        /// </summary>
        /// <param name="value">poll包中的value</param>
        public void GroupMessage(JsonPollMessage.paramResult.paramValue value, Action<int, string, string> action)
        {
            string message = Message_Process_GetMessageText(value.content);
            //TODO：invoker 插件

            if (action != null)
            {
                action(0, value.from_uin, message);
            }
        }

        /// <summary>
        /// 讨论组消息处理
        /// </summary>
        /// <param name="value">poll包中的value</param>
        public void DisscussMessage(JsonPollMessage.paramResult.paramValue value, Action<int, string, string> action)
        {
            string message = Message_Process_GetMessageText(value.content);
            //TODO：invoker 插件
            if (action != null)
            {
                action(0, value.from_uin, message);
            }
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
