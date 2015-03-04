using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;
using System.Web;
using System.Collections.Specialized;

namespace Basement.Framework.Utility
{

    public sealed class CookieHelper
    {
        #region Write cookie
        /// <summary>
        /// write cookie value
        /// </summary>
        /// <param name="CookieName">cookie name</param>
        /// <param name="Nvc">NameValueCollection</param>
        /// <param name="days">cookie date</param>
        /// <param name="Domain">Domain</param>
        /// <returns>bool</returns>
        public static bool WriteCookie(string CookieName, NameValueCollection Nvc, int day, int Hours, string Domain)
        {
            bool ReturnBoolValue = false;
            if (Nvc != null && !string.IsNullOrEmpty(CookieName))
            {
                HttpCookie httpCookie = new HttpCookie(CookieName);
                for (int i = 0; i < Nvc.Count; i++)
                {
                    httpCookie[Nvc.GetKey(i)] = Nvc.Get(i);
                }

                int Time = 0;

                if (day > 0)
                {
                    Time = 24 * day;
                }

                if (Hours > 0)
                {
                    Time = Time + Hours;
                }

                if (Time > 0)
                {
                    httpCookie.Expires = DateTime.Now.AddHours(Time + Hours);
                }
                else if (Time <= 0)
                {
                    httpCookie.Expires = DateTime.MinValue;
                }
                if (!string.IsNullOrEmpty(Domain))
                {
                    httpCookie.Domain = Domain;
                }
                HttpContext.Current.Response.Cookies.Add(httpCookie);
                ReturnBoolValue = true;
            }
            return ReturnBoolValue;
        }

        /// <summary>
        /// write cookie value
        /// </summary>
        /// <param name="CookieName">cookie name</param>
        /// <param name="Nvc">NameValueCollection</param>
        /// <param name="days">cookie date</param>
        /// <returns>bool</returns>
        public static bool WriteCookie(string CookieName, NameValueCollection Nvc, int days, int Hours)
        {
            return WriteCookie(CookieName, Nvc, days, Hours, null);
        }
        #endregion

        #region Add Cookie Key value
        public static bool AddCookie(string CookieName, NameValueCollection Nvc)
        {
            bool ReturnBoolValue = false;
            if (Nvc != null && !string.IsNullOrEmpty(CookieName))
            {
                for (int i = 0; i < Nvc.Count; i++)
                {
                    HttpContext.Current.Response.Cookies[CookieName][Nvc.GetKey(i)] = Nvc.Get(i);
                }
            }
            return ReturnBoolValue;
        }
        #endregion


        #region update cookie
        /// <summary>
        /// update cookie
        /// </summary>
        /// <param name="CookieName">cookie name</param>
        /// <param name="Nvc">NameValueCollection</param>
        /// <returns>bool</returns>
        public static bool UpdateCookie(string CookieName, NameValueCollection Nvc)
        {
            bool ReturnBoolValue = false;
            if (Nvc != null && !string.IsNullOrEmpty(CookieName))
            {
                HttpCookie httpCookie = new HttpCookie(CookieName);
                NameValueCollection NonceNvc = HttpContext.Current.Request.Cookies[CookieName].Values;
                if (NonceNvc != null)
                {
                    string CookieValue = string.Empty;
                    for (int i = 0; i < NonceNvc.Count; i++)
                    {
                        CookieValue = NonceNvc.Get(i);
                        for (int y = 0; y < Nvc.Count; y++)
                        {
                            if (NonceNvc.GetKey(i) == Nvc.GetKey(y))
                            {
                                if (CookieValue != Nvc.Get(y))
                                {
                                    CookieValue = Nvc.Get(y);
                                }
                                break;
                            }
                        }
                        httpCookie[NonceNvc.GetKey(i)] = CookieValue;
                        CookieValue = string.Empty;
                    }
                    HttpContext.Current.Response.Cookies.Add(httpCookie);
                    ReturnBoolValue = true;
                }
            }
            return ReturnBoolValue;
        }
        #endregion

        #region get cookie
        /// <summary>
        /// get cookie 
        /// </summary> 
        /// <param name="CookieName">cookie name</param>
        /// <returns>NameValueCollection</returns>
        public static NameValueCollection GetCookie(string CookieName)
        {
            NameValueCollection Nvc = new NameValueCollection();
            if (!string.IsNullOrEmpty(CookieName))
            {
                if (CheckCookie(CookieName)) Nvc = HttpContext.Current.Request.Cookies[CookieName].Values;
            }
            return Nvc;
        }
        #endregion

        #region delete cookie

        public static bool DeleteCookie(string CookieName)
        {
            return DeleteCookie(CookieName, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CookieName"></param>
        /// <returns></returns>
        public static bool DeleteCookie(string CookieName,string Domain)
        {
            bool ReturnBoolValue = false;

            if (HttpContext.Current.Request.Cookies[CookieName] != null)
            {
                HttpCookie httpCookie = HttpContext.Current.Request.Cookies[CookieName];
                httpCookie.Expires = DateTime.Now.AddDays(-1);
                if(!string.IsNullOrEmpty(Domain))httpCookie.Domain = Domain;
                HttpContext.Current.Response.Cookies.Add(httpCookie);

                ReturnBoolValue = true;
            }
            return ReturnBoolValue;
        }

        #endregion

        #region check cookie
        public static bool CheckCookie(string CookieName)
        {
            bool ReturnBoolValue = false;
            if (HttpContext.Current.Request.Cookies[CookieName] != null)
            {
                if (HttpContext.Current.Request.Cookies[CookieName].Values != null)
                {
                    ReturnBoolValue = true;
                }
            }
            return ReturnBoolValue;
        }
        #endregion

        #region userdata
        //public static string SerializerUserData(object obj)
        //{
        //    string ReturnStrValue = string.Empty;
        //    if (obj != null)
        //    {
        //        ReturnStrValue = Convert.ToBase64String(Serializer.ConvertToBytes(obj));
        //    }
        //    return ReturnStrValue;
        //}

        //public static T UserData<T>(string Str) where T : new()
        //{
        //    T t = new T();
        //    if (!string.IsNullOrEmpty(Str))
        //    {
        //        t = (T)Serializer.ConvertToObject(Convert.FromBase64String(Str));
        //    }
        //    return t;
        //}
        #endregion

        #region FormsAuthenticationTicket
        public static bool WriteTicketCookie(int version, string name, DateTime issueDate, DateTime expiration, bool isPersistent, string userData)
        {
            bool ReturnBoolValue = false;
            //try
            //{
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(version, name, issueDate, expiration, isPersistent, userData);
            string CookieStr = FormsAuthentication.Encrypt(ticket);
            HttpCookie httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, CookieStr);
            HttpContext.Current.Response.Cookies.Add(httpCookie);
            ReturnBoolValue = true;
            //}
            //catch
            //{

            //}
            return ReturnBoolValue;
        }

        public static T GetTicketCookie<T>(string UserData) where T : new()
        {
            T t = new T();
            if (!string.IsNullOrEmpty(UserData))
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(UserData);
             //   t = UserData<T>(ticket.UserData);
            }
            return t;
        }
        #endregion

        /// <summary>
        /// 清除Cookie
        /// </summary>
        public static void ClearCookie(string CookieName)
        {
            HttpContext.Current.Response.Cookies[CookieName].Expires = DateTime.Now.AddDays(-30);
        }
    }

}
