using System;
using System.Web;

namespace Basement.Framework.Utility
{
    public class ClientIP
    {
        /// <summary>
        /// 获取客户端的IP，可以取到代理后的IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIp()
        {
            var lRet = string.Empty;
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))
                lRet = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            if (string.IsNullOrEmpty(lRet))
                lRet = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            return lRet;
        }

        public static string GetUserIP()
        {
            try
            {
                var hc = HttpContext.Current;
                if (hc == null) return "0.0.0.0";
                var ip = hc.Request.UserHostAddress;
                if (ip == "::1")
                {
                    ip = "127.0.0.1";
                }
                return ip;
            }
            catch
            {
                return "0.0.0.0";
            }
        }

        public static long GetUserIPNum()
        {
            var ipStr = GetUserIP();
            return IPStrToNumber(ipStr);
        }

        /// <summary>
        /// 将IPv4格式的字符串转换为int型表示
        /// </summary>
        /// <param name="strIPAddress">IPv4格式的字符</param>
        /// <returns></returns>
        public static long IPStrToNumber(string strIPAddress)
        {
            //将目标IP地址字符串strIPAddress转换为数字   
            if (string.IsNullOrWhiteSpace(strIPAddress))
            {
                return -1;
            }
            var arrayIP = strIPAddress.Split('.');
            var sip1 = Int64.Parse(arrayIP[0]);
            var sip2 = Int64.Parse(arrayIP[1]);
            var sip3 = Int64.Parse(arrayIP[2]);
            var sip4 = Int64.Parse(arrayIP[3]);
            long tmpIpNumber = (sip1 << 24) + (sip2 << 16) + (sip3 << 8) + sip4;
            return tmpIpNumber;
        }
        /// <summary>
        /// 将int型表示的IP还原成正常IPv4格式。
        /// int型表示的IP
        ///</param>
        /// <returns></returns>
        public static string IPNumberToStr(long intIPAddress)
        {
            var bs = BitConverter.GetBytes(intIPAddress);
            return string.Format("{0}.{1}.{2}.{3}", bs[3], bs[2], bs[1], bs[0]);
        }

    }
}
