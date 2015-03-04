using System;
using System.Collections.Generic;
using System.Text;

namespace Basement.Framework.Net
{

    public class IPAddress
    {
        #region 编码IP地址
        /// <summary>
        /// 编码IP地址
        /// </summary>
        /// <param name="IP">原始IP地址</param>
        /// <returns>编码IP地址</returns>
        public static Int64 EncodeIP(string IP)
        {
            string[] UserIP = IP.Split('.');
            Int64 EncodeIP = (Convert.ToInt64(UserIP[0]) * 256 * 256 * 256) + (Convert.ToInt64(UserIP[1]) * 256 * 256) + (Convert.ToInt64(UserIP[2]) * 256) + Convert.ToInt64(UserIP[3]);
            return EncodeIP;
        }
        #endregion

        #region 解码IP地址
        /// <summary>
        /// 解码IP地址
        /// </summary>
        /// <param name="IP">编码IP地址</param>
        /// <returns>原始IP地址</returns>
        public static string DecodeIP(Int64 IP)
        {
            Int64 int_0, int_1;

            //得到第四位IP
            int_0 = IP % 256;
            int_1 = IP / 256;
            string ReturnIP = int_0.ToString();

            //得到第三位IP
            int_0 = int_1 % 256;
            int_1 = int_1 / 256;
            ReturnIP = int_0.ToString() + "." + ReturnIP;

            //得到第二位IP
            int_0 = int_1 % 256;
            int_1 = int_1 / 256;
            ReturnIP = int_0.ToString() + "." + ReturnIP;

            //得到第一位IP
            ReturnIP = (int_1 % 256).ToString() + "." + ReturnIP;

            return ReturnIP;
        }
        #endregion

        public static bool IsValidIP(string ip)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
            {
                string[] ips = ip.Split('.');
                if (ips.Length == 4 || ips.Length == 6)
                {
                    if (System.Int32.Parse(ips[0]) < 256 && System.Int32.Parse(ips[1]) < 256 & System.Int32.Parse(ips[2]) < 256 & System.Int32.Parse(ips[3]) < 256)
                        return true;
                    else
                        return false;
                }
                else
                    return false;

            }
            else
                return false;
        }
    }
}
