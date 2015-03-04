using System;
using System.Collections.Generic;
using System.Web;
using System.Threading;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
namespace Basement.Framework.Utility
{
    public class Proxy : IComparable
    {
        private int port;
        private double speed;
        private string proxyip;

        /// <summary>
        /// 代理端口
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        /// <summary>
        /// 代理IP
        /// </summary>
        public string Proxyip
        {
            get { return proxyip; }
            set { proxyip = value; }
        }

        /// <summary>
        /// 代理速度
        /// </summary>
        public double Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        /*       /// <summary>
               /// 重载操作符==
               /// </summary>
               /// <param name="compareA"></param>
               /// <param name="compareB"></param>
               /// <returns></returns>
               public static bool operator ==(Proxy compareA,Proxy compareB)
               {
                   bool ret = false;
                        
                   if (compareA.Proxyip == compareB.Proxyip && compareA.Port == compareB.Port)
                   {
                       ret = true;
                   }
                   return ret;
               }

               public static bool operator !=(Proxy compareA, Proxy compareB)
               {
                   bool ret = false;
                   if (compareA.Proxyip != compareB.Proxyip || compareA.Port != compareB.Port)
                   {
                       ret = true;
                   }
                   return ret;
               }*/

        #region IComparable 成员
        int IComparable.CompareTo(object obj)
        {
            Proxy compareInstance = (Proxy)obj;
            if (this.speed > compareInstance.speed)
                return 1;
            else if (this.speed == compareInstance.speed)
                return 0;
            else
                return -1;
        }
        #endregion
    }

    /// <summary>
    /// 代理抓取类
    /// </summary>
    public class ProxyCatch
    {
        Thread hMainThread = null;                          //抓取线程
        List<Proxy> proxyList = new List<Proxy>();          //代理列表
        bool m_bStop = true;                                //停止标记
        private DateTime dtPrevTime;                        //上次采集时间
        private int pos = 0;
        public enum ProxyOrder { MaxSpeed, Random, Sequence };
        private static readonly ProxyCatch instance = new ProxyCatch();


        /// <summary>
        /// SingleTon
        /// </summary>
        private ProxyCatch()
        {
        }

        public static ProxyCatch Instance
        {
            get { return instance; }
        }
        public Proxy GetProxy(ProxyOrder proxyOrder)
        {
            Proxy proxy = null;
            if (proxyList.Count > 0)
            {
                Random random = new Random(DateTime.Now.Millisecond);
                switch (proxyOrder)
                {
                    case ProxyOrder.MaxSpeed:
                        proxy = proxyList[0];
                        break;
                    case ProxyOrder.Random:
                        proxy = proxyList[random.Next(proxyList.Count)];
                        break;
                    case ProxyOrder.Sequence:
                        proxy = proxyList[pos++];
                        if (pos == proxyList.Count)
                            pos = 0;
                        break;
                    default:
                        proxy = proxyList[random.Next(proxyList.Count)];
                        break;
                }
            }
            return proxy;
        }

        public void Start()
        {
            if (m_bStop && (hMainThread == null || hMainThread.ThreadState == ThreadState.Stopped))
            {
                hMainThread = new Thread(new ThreadStart(MainProc));
                hMainThread.Start();
                m_bStop = false;
            }
        }

        /// <summary>
        /// 抓取线程主函数
        /// </summary>
        protected void MainProc()
        {
            dtPrevTime = DateTime.Now;
            CatchProxy();
            while (!m_bStop)
            {
                TimeSpan timespan = DateTime.Now - dtPrevTime;
                if (timespan.TotalHours >= 6)
                {
                    dtPrevTime = DateTime.Now;
                    CatchProxy();
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 停止抓取
        /// </summary>
        public void Stop()
        {
            m_bStop = true;
            hMainThread.Join();
        }

        /// <summary>
        /// 获取网页数据流
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>数据流</returns>
        private string GetHtmlContext(string url, string code, Proxy proxy)
        {
            string html = string.Empty;
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                CookieContainer cc = new CookieContainer();
                myRequest.Method = "GET";
                if (proxy != null)
                {
                    myRequest.Proxy = new WebProxy(proxy.Proxyip, proxy.Port);
                }
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.CookieContainer = cc;
                myRequest.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-shockwave-flash, */*";
                myRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; InfoPath.2; CIBA; Alexa Toolbar)";
                myRequest.Timeout = 15000;
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                /*switch (myResponse.StatusCode)
                {
                    case HttpStatusCode.Redirect:
                        return GetHtmlContext(myResponse.GetResponseHeader(
                }*/
                Encoding encoding = Encoding.UTF8;
                if (code.ToLower() == "gb2312")
                {
                    encoding = Encoding.GetEncoding("gb2312");
                }
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), encoding);
                html = reader.ReadToEnd();
                if (html == "" || html == null)
                {
                    html = "Error";
                }
                myResponse.Close();
                reader.Close();
            }
            catch
            {
                html = "Error";
            }
            return html;
        }

        protected void CatchProxy()
        {
            foreach (Proxy proxy in proxyList)
            {
                DateTime dtStart = DateTime.Now;
                if (CheckProxy(proxy))
                {
                    DateTime dtEnd = DateTime.Now;
                    TimeSpan span = dtEnd - dtStart;
                    proxy.Speed = span.TotalSeconds;
                }
                else
                {
                    proxy.Speed = 999999999999;
                }
            }
            proxyList.Sort();
            string strWebdata = GetHtmlContext("http://www.proxycn.com/html_proxy/30fastproxy-1.html", "GB2312", null);
            ParseProxy(strWebdata);
        }

        protected bool CheckProxy(Proxy proxy)
        {
            bool bResult = false;
            string strHtml = GetHtmlContext("http://www.baidu.com", "GB2312", proxy);
            if (Regex.IsMatch(strHtml, "百度一下，你就知道"))
            {
                bResult = true;
            }
            return bResult;
        }

        private bool IsExists(Proxy proxy)
        {
            bool bExists = false;
            foreach (Proxy item in proxyList)
            {
                if (item.Proxyip == proxy.Proxyip && item.Port == proxy.Port)
                {
                    bExists = true;
                }
            }
            return bExists;
        }

        private void ParseProxy(string webData)
        {
            MatchCollection matchs = Regex.Matches(webData, "(?<ip>\\d+\\.\\d+\\.\\d+\\.\\d+):(?<port>\\d+)");
            string strHtml = string.Empty;
            foreach (Match match in matchs)
            {
                Proxy proxy = new Proxy();
                proxy.Proxyip = match.Groups["ip"].ToString();
                proxy.Port = Int32.Parse(match.Groups["port"].ToString());
                DateTime dtStart = DateTime.Now;
                if (!IsExists(proxy))
                {
                    if (CheckProxy(proxy))
                    {
                        DateTime dtEnd = DateTime.Now;
                        TimeSpan span = dtEnd - dtStart;
                        proxy.Speed = span.TotalSeconds;
                        proxyList.Add(proxy);
                    }
                }
            }
            proxyList.Sort();
        }
    }
}