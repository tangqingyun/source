using Basement.Framework.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Basement.Framework.Net
{
    public class HttpUtility
    {
        /// <summary>
        /// 默认用户代理
        /// </summary>
        private const string DEFAULT_USER_AGENT = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET4.0C; .NET4.0E; .NET CLR 3.5.30729; .NET CLR 3.0.30729; InfoPath.3)";
        /// <summary>
        /// 默认类型
        /// </summary>
        private const string DEFAULT_CONTENT_TYPE = "application/x-www-form-urlencoded";
        /// <summary>
        /// 默认请求编码
        /// </summary>
        private static Encoding DEFAULT_ENCODING = Encoding.UTF8;
        public delegate void DelegateGetComplated(string url, string statusCode, string result, int usedTime);
        public delegate void DelegatePostComplated(string url, string paramStr, string statusCode, string result, int usedTime);
        public static DelegateGetComplated GetComplated;
        public static DelegatePostComplated PostComplated;

        public T PostAsModel<T>(string url, Dictionary<string, string> datas, Encoding requestEncoding = null, int? timeout = null, string userAgent = null, CookieCollection cookies = null, string contentType = null, Dictionary<string, string> heards = null)
        {
            string result = SendPost(url, datas, requestEncoding, timeout, userAgent, cookies, contentType, heards);
            try
            {
                return JsonHelper.ConvertToObject<T>(result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public T PostAsModel<T>(string url, Dictionary<string, string> datas, out CookieCollection hwrCookies, Encoding requestEncoding = null, int? timeout = null, string userAgent = null, CookieCollection cookies = null, string contentType = null, Dictionary<string, string> heards = null)
        {
            string result = SendPost(url, datas, out hwrCookies, requestEncoding, timeout, userAgent, cookies, contentType, heards);
            try
            {
                return JsonHelper.ConvertToObject<T>(result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public T GetAsModel<T>(string url, Encoding requestEncoding = null, int? timeout = null, string userAgent = null, CookieCollection cookies = null, string contentType = null, Dictionary<string, string> heards = null)
        {
            string result = SendGet( url,  requestEncoding ,  timeout ,  userAgent ,  cookies ,  contentType , heards);
            try
            {
                return JsonHelper.ConvertToObject<T>(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="datas">随同请求POST的参数数值字典</param>
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>
        /// <param name="timeout">请求的超时时间</param>
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，可以为空</param>
        /// <param name="contentType">获取或设置 Content-typeHTTP 标头的值</param>
        /// <param name="heards">请求头信息</param>
        /// <returns></returns>
        public static string SendPost(string url, Dictionary<string, string> datas, Encoding requestEncoding = null, int? timeout = null, string userAgent = null, CookieCollection cookies = null, string contentType = null, Dictionary<string, string> heards = null)
        {
            HttpWebResponse httpResponse = null;
            DateTime timeBegin = DateTime.Now;
            DateTime timeEnd;
            string htmlContent = string.Empty;
            try
            {
                ServicePointManager.Expect100Continue = false;
                httpResponse = CreatePostHttpResponse(url, datas, requestEncoding, timeout, userAgent, cookies, contentType, heards);
                htmlContent = GetTxtByHttpWebResponse(httpResponse);
            }
            catch (Exception ex)
            {
                htmlContent = ex.Message;
                throw;
            }
            timeEnd = DateTime.Now;
            if (PostComplated != null)
            {
                PostComplated(url, ParamListToStr(datas, requestEncoding), httpResponse == null ? "" : httpResponse.StatusCode.ToString(), htmlContent, Convert.ToInt32((timeEnd - timeBegin).TotalMilliseconds));
            }
            return htmlContent;
        }

        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="datas">随同请求POST的参数数值字典</param>
        /// <param name="hwrCookies">返回的Cookie数据</param>
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>
        /// <param name="timeout">请求的超时时间</param>
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，可以为空</param>
        /// <param name="contentType">获取或设置 Content-typeHTTP 标头的值</param>
        /// <param name="heards">请求头信息</param>
        /// <returns></returns>
        public static string SendPost(string url, Dictionary<string, string> datas, out CookieCollection hwrCookies, Encoding requestEncoding = null, int? timeout = null, string userAgent = null, CookieCollection cookies = null, string contentType = null, Dictionary<string, string> heards = null)
        {
            HttpWebResponse httpResponse = null;
            DateTime timeBegin = DateTime.Now;
            DateTime timeEnd;
            string htmlContent = string.Empty;
            try
            {
                ServicePointManager.Expect100Continue = false;
                httpResponse = CreatePostHttpResponse(url, datas, requestEncoding, timeout, userAgent, cookies, contentType, heards);
                hwrCookies = httpResponse.Cookies;
                htmlContent = GetTxtByHttpWebResponse(httpResponse);
            }
            catch (Exception ex)
            {
                htmlContent = ex.Message;
                throw;
            }
            timeEnd = DateTime.Now;
            if (PostComplated != null)
            {
                PostComplated(url, ParamListToStr(datas, requestEncoding), httpResponse == null ? "" : httpResponse.StatusCode.ToString(), htmlContent, Convert.ToInt32((timeEnd - timeBegin).TotalMilliseconds));
            }
            return htmlContent;
        }

        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>
        /// <param name="timeout">请求的超时时间</param>
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，可以为空</param>
        /// <param name="contentType">获取或设置 Content-typeHTTP 标头的值</param>
        /// <param name="heards">请求头信息</param>
        /// <returns></returns>
        public static string SendGet(string url, Encoding requestEncoding = null, int? timeout = null, string userAgent = null, CookieCollection cookies = null, string contentType = null, Dictionary<string, string> heards = null)
        {
            HttpWebResponse httpResponse = null;
            DateTime timeBegin = DateTime.Now;
            DateTime timeEnd;
            string htmlContent = string.Empty;
            try
            {
                ServicePointManager.Expect100Continue = false;
                httpResponse = CreateGetHttpResponse(url, timeout, userAgent, cookies, contentType, heards);
                htmlContent = GetTxtByHttpWebResponse(httpResponse);
            }
            catch (Exception ex)
            {
                htmlContent = ex.Message;
                throw;
            }
            timeEnd = DateTime.Now;
            if (PostComplated != null)
            {
                GetComplated(url, httpResponse == null ? "" : httpResponse.StatusCode.ToString(), htmlContent, Convert.ToInt32((timeEnd - timeBegin).TotalMilliseconds));
            }
            return htmlContent;
        }

        
        private static HttpWebResponse CreateGetHttpResponse(string url, int? timeout = null, string userAgent = null, CookieCollection cookies = null, string contentType = null, Dictionary<string, string> heards = null)
        {
            var request = CreateHttpResponse(url, timeout, userAgent, cookies, contentType, heards);
            request.Method = "GET";
            return request.GetResponse() as HttpWebResponse;
        }

        private static HttpWebResponse CreatePostHttpResponse(string url, Dictionary<string, string> datas, Encoding requestEncoding = null, int? timeout = null, string userAgent = null, CookieCollection cookies = null, string contentType = null, Dictionary<string, string> heards = null)
        {
            if (requestEncoding == null)
            {
                requestEncoding = DEFAULT_ENCODING;
            }
            var request = CreateHttpResponse(url, timeout, userAgent, cookies, contentType, heards);
            request.Method = "POST";
            request.AllowAutoRedirect = false;
            request.KeepAlive = true;
            //封装POST数据 
            string dataPost = "";
            if (datas.Count > 0)
            {
                dataPost = ParamListToStr(datas, requestEncoding);
            }
            if (!string.IsNullOrWhiteSpace(dataPost))
            {
                byte[] data = requestEncoding.GetBytes(dataPost);
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            return response;
        }

        private static HttpWebRequest CreateHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies, string contentType, Dictionary<string, string> heards)
        {
            if (String.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("请求的URL不能为空！");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            //设置编码
            request.ContentType = !string.IsNullOrWhiteSpace(contentType) ? contentType : DEFAULT_CONTENT_TYPE;
            request.UserAgent = !String.IsNullOrEmpty(userAgent) ? userAgent : DEFAULT_USER_AGENT;//设置用户代理

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            else
            {
                request.CookieContainer = new CookieContainer();
            }

            if (heards != null)
            {
                foreach (var heard in heards)
                {
                    request.Headers.Add(heard.Key, heard.Value);
                }
            }
            return request;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors
errors)
        {
            return true; //总是接受  
        }

        /// <summary>
        /// 通过HTTP请求获取HTML
        /// </summary>
        /// <param name="hwr"></param>
        /// <param name="closeHttpWebResponse">是否关闭HttpWebResponse</param>
        /// <returns></returns>
        private static string GetTxtByHttpWebResponse(HttpWebResponse hwr, bool closeHttpWebResponse = false)
        {
            var stream = hwr.GetResponseStream();
            var reader = new StreamReader(stream, Encoding.GetEncoding(hwr.CharacterSet));
            var result = reader.ReadToEnd();
            reader.Close();
            if (stream != null) stream.Close();
            if (closeHttpWebResponse)
            {
                hwr.Close();
            }
            return result;
        }

        /// <summary>
        /// 发送数据封装
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static string ParamListToStr(Dictionary<string, string> dic, Encoding encoding = null)
        {
            if (dic == null || dic.Count == 0)
            {
                return "";
            }
            var buffer = new StringBuilder();
            foreach (var parameter in dic)
            {
                if (buffer.Length > 0)
                {
                    buffer.AppendFormat("&");
                }
                //buffer.AppendFormat("{0}={1}", parameter.Key, parameter.Value);
                buffer.AppendFormat("{0}={1}", System.Web.HttpUtility.UrlEncode(parameter.Key, encoding), System.Web.HttpUtility.UrlEncode(parameter.Value, encoding));
            }
            return buffer.ToString();
        }

    }

    public class Parameter
    {
        public Parameter(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public string Key { set; get; }
        public string Value { set; get; }
    }

}
