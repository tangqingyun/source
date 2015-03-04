using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace Basement.Framework.Utility
{
    #region  BrowserInfo
    public class BrowserInfo
    {
        public static BrowserInfo Current
        {
            get
            {
                return new BrowserInfo();
            }
        }

        private HttpContext _context;

        /// <summary>
        /// Gets the browser.
        /// </summary>
        /// <value>The browser.</value>
        public string Browser
        {
            get
            {
                try
                {
                    string strResult = _context.Request.UserAgent;
                    return GetBrowser(strResult);
                }
                catch
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Gets the OS.
        /// </summary>
        /// <value>The OS.</value>
        public string OS
        {
            get
            {
                try
                {

                    string strResult = _context.Request.UserAgent;
                    return GetOS(strResult);
                }
                catch
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Gets the IP.
        /// </summary>
        /// <value>The IP.</value>
        public string IP
        {
            get
            {
                string result = string.Empty;
                if (_context == null) return result;

                result = _context.Request.ServerVariables["REMOTE_ADDR"];
                if (string.IsNullOrEmpty(result) == true)
                    result = _context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                return result;
            }
        }

        /// <summary>
        /// Gets the referrer.
        /// </summary>
        /// <value>The referrer.</value>
        public Uri Referrer
        {
            get
            {
                return _context.Request.UrlReferrer;
            }
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get
            {
                return (_context.Request.IsAuthenticated) ? _context.User.Identity.Name : "";
            }
        }

        /// <summary>
        /// Gets the site root.
        /// </summary>
        /// <value>The site root.</value>
        public string SiteRoot
        {
            get
            {
                string Port = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
                if (Port == null || Port == "80" || Port == "443")
                    Port = "";
                else
                    Port = ":" + Port;

                string Protocol = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
                if (Protocol == null || Protocol == "0")
                    Protocol = "http://";
                else
                    Protocol = "https://";

                string sOut = Protocol + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + Port + System.Web.HttpContext.Current.Request.ApplicationPath;
                sOut = Regex.Replace(sOut, "/$", string.Empty);
                return sOut;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BrowserInfo"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public BrowserInfo(HttpContext context)
        {
            _context = context;
        }

        public BrowserInfo()
        {
            this._context = HttpContext.Current;
        }



        /// <summary>
        /// Gets the OS.
        /// </summary>
        /// <param name="strPara">The STR para.</param>
        /// <returns></returns>
        private string GetOS(string strPara)
        {
            string GetInfo = string.Empty;

            if (strPara.IndexOf("Tel") > -1)
            {
                GetInfo = "Telport";
            }
            else if (strPara.IndexOf("webzip") > -1)
            {
                GetInfo = "webzip";
            }
            else if (strPara.IndexOf("flashget") > -1)
            {
                GetInfo = "flashget";
            }
            else if (strPara.IndexOf("offline") > -1)
            {
                GetInfo = "offline";
            }
            else if (strPara.IndexOf("Win95") > -1)
            {
                GetInfo = "Windows 95";
            }
            else if (strPara.IndexOf("Win 9x 4.90") > -1)
            {
                GetInfo = "Windows ME";
            }
            else if (strPara.IndexOf("Win98") > -1)
            {
                GetInfo = "Windows 98";
            }
            else if (strPara.IndexOf("Windows NT 5.0") > -1)
            {
                GetInfo = "Windows 2000";
            }
            else if (strPara.IndexOf("Windows NT 5.1") > -1)
            {
                GetInfo = "Windows XP";
            }
            else if (strPara.IndexOf("Windows NT 5.2") > -1)
            {
                GetInfo = "Windows 2003";
            }
            else if (strPara.IndexOf("Windows NT 6.0") > -1)
            {
                GetInfo = "Windows Vista";
            }
            else if (strPara.IndexOf("Windows NT") > -1)
            {
                GetInfo = "Windows NT 4";
            }
            else if (strPara.IndexOf("WinNT4.0") > -1)
            {
                GetInfo = "Windows NT 4";
            }
            else if (strPara.IndexOf("Windows NT 4.0") > -1)
            {
                GetInfo = "Windows NT4";
            }
            else if (strPara.IndexOf("mac") > -1)
            {
                //Mozilla/4.0 (compatible; MSIE 6.0; Mac_PowerPC Mac OS X; zh-cn) Opera 8.5
                GetInfo = "Mac";
            }
            else
            {
                GetInfo = strPara;
            }

            return GetInfo;

        }

        /// <summary>
        /// Gets the browser.
        /// </summary>
        /// <param name="strPara">The STR para.</param>
        /// <returns></returns>
        private string GetBrowser(string strPara)
        {
            //strPara = strPara.Replace(".0", ".x");
            //return strPara;
            string GetInfo = string.Empty;
            if (strPara.IndexOf("Opera") > -1)
            {
                //Mozilla/4.0 (compatible; MSIE 6.0; X11; Linux i686; Chinese) Opera 8.52
                GetInfo = Regex.Replace(strPara, @".*Opera([/ ]{0,1}[\w\.]+).+", "Opera $1");
            }
            else if (strPara.IndexOf("NetCaptor") > -1)
            {
                //Mozilla/4.0 (comp; ver; plat; SV1; NetCaptor 7.5.4)
                GetInfo = Regex.Replace(strPara, @".+(NetCaptor [\w\.]+).+", "$1");
            }
            else if (strPara.IndexOf("MSIE") > -1)
            {
                //Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)
                GetInfo = Regex.Replace(strPara, @".+MSIE (\d+?\.\d+?[a-z]{0,1}).+", "Internet Explorer $1");
            }
            else if (strPara.IndexOf("Firefox") > -1)
            {
                //Mozilla/5.0 (X11; U; Linux i686; zh-CN; rv:1.8.0.6) Gecko/20060628 Firefox/1.5.0.6 (RAYS-3:1.5.dfsg+1.5.0.6-1.rays1)
                GetInfo = Regex.Replace(strPara, @".+Firefox\/([\w\.]+).+", "Firefox $1");
            }
            else if (strPara.IndexOf("KHTML") > -1)
            {
                if (strPara.IndexOf("AppleWebKit") > -1)
                {
                    //Mozilla/5.0 (Macintosh; U; PPC Mac OS X; ja-jp) AppleWebKit/418 (KHTML, like Gecko)
                    GetInfo = Regex.Replace(strPara, @".+AppleWebKit/([\d\.]+).+", "Safari $1");
                }
                else if (strPara.IndexOf("Konqueror") > -1)
                {
                    //Mozilla/5.0 (compatible; Konqueror/3.4; Linux) KHTML/3.4.2 (like Gecko)
                    GetInfo = Regex.Replace(strPara, @".+Konqueror/([\d\.]+).+", "Konqueror $1");
                }
            }
            else if (strPara.IndexOf("Netscape") > -1)
            {
                //Mozilla/5.0 (Windows; U; Win 9x 4.90; en-US; rv:1.7.5) Gecko/20060127 Netscape/8.1
                GetInfo = Regex.Replace(strPara, @".+Netscape/([\d\.]+).*", "Netscape $1");
            }
            else
            {
                GetInfo = strPara;
            }

            return GetInfo;

        }
    }
    #endregion
}
