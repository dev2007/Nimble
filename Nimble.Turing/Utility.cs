using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Turing
{
    public class Utility
    {
        /// <summary>
        /// POST方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static string Post(string url,string requestData)
        {
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "post";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] buffer = encoding.GetBytes(requestData);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8")))
            {
                return  reader.ReadToEnd();
            }
        }
    }
}
