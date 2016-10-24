using Newtonsoft.Json;
using Nimble.Turing.Model;
using NimbleFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Turing
{
    public class Smart : BasicQMessage
    {
        public Smart()
        {
            AppName = "图录机器人";
            GUID = "{E29DE314-C4C9-42DC-A04E-4196F99AF6D8}";
            Version = "1.0";
        }

        public override string Process(string message)
        {
            string result = Utility.Post(Define.URL, new LingshaRequest() { info = message }.ToString());
            var basicResponse = JsonConvert.DeserializeObject<Response>(result);

            string msg = string.Empty;
            switch ((ResponseCode)basicResponse.code)
            {
                case ResponseCode.TEXT:
                    msg = basicResponse.text;
                    break;
                case ResponseCode.LINK:
                    var linkResponse = JsonConvert.DeserializeObject<ResponseLink>(result);
                    msg = linkResponse.text + " " + linkResponse.url;
                    break;
                case ResponseCode.NEWS:
                    var newsResponse = JsonConvert.DeserializeObject<ResponseNews>(result);
                    msg = newsResponse.text;
                    foreach (var row in newsResponse.list)
                    {
                        msg += string.Format(" 《{0}》- {1} 链接：{2}", row.article, row.source, row.detailurl);
                    }
                    break;
                case ResponseCode.COOKBOOK:
                    var cookbookResponse = JsonConvert.DeserializeObject<ResponseCookbook>(result);
                    msg = cookbookResponse.text;
                    foreach (var row in cookbookResponse.list)
                    {
                        msg += string.Format(" 《{0}》主料：{1} 链接：{2}", row.name, row.info, row.detailurl);
                    }
                    break;
                case ResponseCode.EMPTY_CONTENT:
                case ResponseCode.WRONG_KEY:
                case ResponseCode.WRONG_DATA:
                case ResponseCode.OUT_TIMES:
                    msg = basicResponse.text;
                    break;
                default:
                    msg = basicResponse.text;
                    break;
            }
            return msg;
        }
    }
}
