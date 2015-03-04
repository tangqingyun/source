using Basement.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;


namespace Basement.Framework.Utility.Web
{
    public class UrlHandle
    {
        #region = SafeAddQueryToURL =
        /// <summary>
        /// Add a query to an URL.
        /// if the URL has not any query,then append the query key and value to it.
        /// if the URL has some queries, then check it if exists the query key already,replace the value, or append the key and value
        /// if the URL has any fragment, append fragments to the URL end.
        /// </summary>
        /// <example>
        ///             string s = "http://www.cnblogs.com/?a=1&b=2&c=3#tag";
        /// WL(SafeRemoveQueryFromURL("a",s));
        /// WL(SafeRemoveQueryFromURL("b",s));
        /// WL(SafeRemoveQueryFromURL("c",s));
        /// WL(SafeAddQueryToURL("d","new",s));
        /// WL(SafeAddQueryToURL("a","newvalue",s));
        ///            输出如下:
        ///            http://www.cnblogs.com/?b=2&c=3#tag
        ///            http://www.cnblogs.com/?a=1&c=3#tag
        ///            http://www.cnblogs.com/?a=1&b=2#tag
        ///            http://www.cnblogs.com/?a=1&b=2&c=3&d=new#tag
        ///            http://www.cnblogs.com/?a=newvalue&b=2&c=3#tag
        /// </example>
        public static string SafeAddQueryToURL(string key, string value, string url)
        {
            int fragPos = url.LastIndexOf("#");
            string fragment = string.Empty;
            if (fragPos > -1)
            {
                fragment = url.Substring(fragPos);
                url = url.Substring(0, fragPos);
            }
            int querystart = url.IndexOf("?");
            if (querystart < 0)
            {
                url += "?" + key + "=" + value;
            }
            else
            {
                Regex reg = new Regex(@"(?<=[&\?])" + key + @"=[^\s&#]*", RegexOptions.Compiled);
                if (reg.IsMatch(url))
                    url = reg.Replace(url, key + "=" + value);
                else
                    url += "&" + key + "=" + value;
            }
            return url + fragment;
        }
        #endregion

        #region = SafeRemoveQueryFromURL =
        /// <summary>
        /// Remove a query from url
        /// </summary>
        /// <param name="key"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string SafeRemoveQueryFromURL(string key, string url)
        {
            Regex reg = new Regex(@"[&\?]" + key + @"=[^\s&#]*&?", RegexOptions.Compiled);
            return reg.Replace(url, new MatchEvaluator(PutAwayGarbageFromURL));
        }
        private static string PutAwayGarbageFromURL(Match match)
        {
            string value = match.Value;
            if (value.EndsWith("&"))
                return value.Substring(0, 1);
            else
                return string.Empty;
        }
        #endregion

        #region = Url =
        private string _url = string.Empty;

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        #endregion

        #region = SiteRoot =
        /// <summary>
        /// http://localhost:1466
        /// </summary>
        public string SiteRoot
        {
            get
            {
                return this.FullUrl.GetLeftPart(UriPartial.Authority);
            }
        }
        #endregion

        #region = Domain =
        /// <summary>
        /// localhost
        /// </summary>
        public string Domain
        {
            get { return this.FullUrl.Host; }
        }
        #endregion

        #region = Path =
        /// <summary>
        /// 完整路径
        /// /Smaple/default.aspx 
        /// </summary>
        public string Path
        {
            get
            {
                return this.FullUrl.LocalPath;
            }
        }
        #endregion

        #region = Scheme =
        /// <summary>
        /// http
        /// </summary>
        public string Scheme
        {
            get
            {
                return this.FullUrl.Scheme;
            }
        }
        #endregion

        #region = FileName =
        /// <summary>
        /// Gets the name of the file.
        /// default.aspx
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get
            {

                return System.IO.Path.GetFileName(this.Path);
            }
        }
        #endregion

        #region = DirectoryName =
        /// <summary>
        /// 文件夹的完整名称
        /// </summary>
        /// <remarks>http://localhost:1466/a/b/</remarks>
        /// <value>The name of the get directory.</value>
        public string DirectoryName
        {
            get
            {
                if (string.IsNullOrEmpty(this.Url) == false)
                {
                    int num1 = this.Url.LastIndexOf('/');
                    if (num1 > 0)
                    {
                        return this.Url.Substring(0, num1);
                    }
                }
                return null;
            }
        }
        #endregion

        #region = Query =
        /// <summary>
        /// ?id=123
        /// </summary>
        public string Query
        {
            get
            {
                return this.FullUrl.Query;
            }
        }
        #endregion

        #region = FullUrl =
        public Uri FullUrl
        {
            get
            {
                return new Uri(GetFullUrl());
            }
        }
        #endregion

        #region = Current =
        public static UrlHandle Current
        {
            get
            {
                return new UrlHandle();
            }
        }

        #endregion

        #region = Encode =
        public static string Encode(string strInput)
        {
            if (strInput == null)
            {
                return null;
            }
            if (strInput.Length == 0)
            {
                return string.Empty;
            }
            StringBuilder builder1 = new StringBuilder("", strInput.Length * 2);
            foreach (char ch1 in strInput)
            {
                if ((((ch1 > '`') && (ch1 < '{')) || ((ch1 > '@') && (ch1 < '['))) || (((ch1 > '/') && (ch1 < ':')) || (((ch1 == '.') || (ch1 == '-')) || (ch1 == '_'))))
                {
                    builder1.Append(ch1);
                }
                else if (ch1 > '\x007f')
                {
                    builder1.Append("%u" + CharTool.TwoByteHex(ch1));
                }
                else
                {
                    builder1.Append("%" + CharTool.SingleByteHex(ch1));
                }
            }
            return builder1.ToString();
        }
        #endregion

        #region = Constructor =
        public UrlHandle()
        {
            if (HttpContext.Current == null)
            {
                throw new Exception("HttpContext.Current IS NULL");
            }
            this.Url = HttpContext.Current.Request.Url.ToString();
        }

        public UrlHandle(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute) == false)
            {
                throw new UriFormatException("输入的url不是正确的格式(" + url + ")");
            }
            this.Url = url;
        }
        #endregion

        #region = GetFullUrl =
        /// <summary>
        /// 获取完整的url
        /// </summary>
        /// <returns></returns>
        public string GetFullUrl()
        {
            if (HttpContext.Current == null)
            {
                if (Uri.IsWellFormedUriString(this.Url, UriKind.Absolute) == true)
                {
                    return this.Url;
                }
                else
                {
                    throw new UriFormatException("输入的Url不是绝对地址");
                }
            }
            else
            {
                return GetFullUrl(HttpContext.Current.Request.Url);
            }
        }

        /// <summary>
        /// 获取完整的url
        /// </summary>
        /// <param name="baseurl">参考的完整的url</param>
        /// <returns></returns>
        public string GetFullUrl(Uri baseUri)
        {
            //绝对的地址 直接返回
            if (Uri.IsWellFormedUriString(this.Url, UriKind.Absolute) == true)
            {
                return this.Url;
            }

            //如果是相对相对链接,进行处理
            Uri uri = new Uri(baseUri, this.Url);

            return uri.ToString();
        }

        public string GetFullUrl(string baseUri)
        {
            if (Uri.IsWellFormedUriString(baseUri, UriKind.Absolute) == false)
            {
                throw new UriFormatException("输入的url不是绝对地址");
            }

            return GetFullUrl(new Uri(baseUri));
        }
        #endregion
    }
}
