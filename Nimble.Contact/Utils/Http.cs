using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nimble.Contact.Utils
{
    /// <summary>
    /// Http helper.
    /// </summary>
    public static class Http
    {
        public static CookieContainer Cookies = new CookieContainer();

        /// <summary>
        /// Http Get,just get response stream.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Stream EasyGet(string url)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.CookieContainer = Cookies;
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                return res.GetResponseStream();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Http Get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="referer"></param>
        /// <param name="timeout"></param>
        /// <param name="encode"></param>
        /// <param name="NoProxy"></param>
        /// <returns></returns>
        public static string Get(string url, string referer = Define.Refer, int timeout = Define.Timeout,
            Encoding encode = null, bool NoProxy = false)
        {
            string result = string.Empty;
            HttpWebResponse response;
            HttpWebRequest request;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = Cookies;
                request.AllowAutoRedirect = false;
                request.Timeout = timeout;
                request.Referer = referer;

                if (NoProxy)
                    request.Proxy = null;
                request.UserAgent = Define.UserAgent;
                response = (HttpWebResponse)request.GetResponse();

                Cookies.Add(response.Cookies);
            }
            catch (Exception ex)
            {
                return "";
            }
            StreamReader reader;

            reader = new StreamReader(response.GetResponseStream(), encode == null ? Encoding.UTF8 : encode);
            result = reader.ReadToEnd();

            response.Close();
            request.Abort();
            return result;
        }

        /// <summary>
        /// Http Post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="refer"></param>
        /// <param name="timeout"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string Post(string url, string data, string refer = Define.Refer, int timeout = Define.Timeout, Encoding encode = null)
        {
            string result = string.Empty;
            HttpWebRequest request;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = Cookies;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                request.Proxy = null;
                request.Timeout = timeout;
                request.UserAgent = Define.UserAgent;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Referer = refer;

                byte[] dataBytes = Encoding.Default.GetBytes(data);
                request.ContentLength = dataBytes.Length;

                Stream stream = request.GetRequestStream();
                stream.Write(dataBytes, 0, dataBytes.Length);

                HttpWebResponse res = (HttpWebResponse)request.GetResponse();

                Cookies.Add(res.Cookies);
                stream.Close();

                StreamReader reader = new StreamReader(res.GetResponseStream(), encode == null ? Encoding.UTF8 : encode);
                result = reader.ReadToEnd();
                res.Close();
                request.Abort();
            }
            catch (Exception ex)
            {
                return "";
            }

            //TODO:LOG dat
            return result;
        }
        public delegate void Post_Async_Action(string data);
        private class Post_Async_Data
        {
            public HttpWebRequest req;
            public Post_Async_Action post_Async_Action;
        }

        /// <summary>
        /// Http Post async.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="PostData"></param>
        /// <param name="action"></param>
        /// <param name="Referer"></param>
        /// <param name="timeout"></param>
        public static void Post_Async(string url, string PostData, Post_Async_Action action, string Referer = Define.Refer,
            int timeout = Define.Timeout)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = Cookies;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.Referer = Referer;
            request.UserAgent = Define.UserAgent;
            request.Proxy = null;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ContinueTimeout = timeout;

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(PostData);
            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            Post_Async_Data asyncData = new Post_Async_Data();
            asyncData.req = request;
            asyncData.post_Async_Action = action;
            request.BeginGetResponse(new AsyncCallback(Post_Async_ResponesProceed), asyncData);
        }

        private static void Post_Async_ResponesProceed(IAsyncResult ar)
        {
            StreamReader reader = null;
            Post_Async_Data dat = ar.AsyncState as Post_Async_Data;
            HttpWebRequest req = dat.req;
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;
            reader = new StreamReader(res.GetResponseStream());
            string temp = reader.ReadToEnd();
            res.Close();
            req.Abort();
            dat.post_Async_Action(temp);
        }
    }
}
