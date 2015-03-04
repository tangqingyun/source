using System;
using System.Collections.Generic;
using System.Text;

namespace Basement.Framework.Net
{

    public class IPAddress
    {
        #region ����IP��ַ
        /// <summary>
        /// ����IP��ַ
        /// </summary>
        /// <param name="IP">ԭʼIP��ַ</param>
        /// <returns>����IP��ַ</returns>
        public static Int64 EncodeIP(string IP)
        {
            string[] UserIP = IP.Split('.');
            Int64 EncodeIP = (Convert.ToInt64(UserIP[0]) * 256 * 256 * 256) + (Convert.ToInt64(UserIP[1]) * 256 * 256) + (Convert.ToInt64(UserIP[2]) * 256) + Convert.ToInt64(UserIP[3]);
            return EncodeIP;
        }
        #endregion

        #region ����IP��ַ
        /// <summary>
        /// ����IP��ַ
        /// </summary>
        /// <param name="IP">����IP��ַ</param>
        /// <returns>ԭʼIP��ַ</returns>
        public static string DecodeIP(Int64 IP)
        {
            Int64 int_0, int_1;

            //�õ�����λIP
            int_0 = IP % 256;
            int_1 = IP / 256;
            string ReturnIP = int_0.ToString();

            //�õ�����λIP
            int_0 = int_1 % 256;
            int_1 = int_1 / 256;
            ReturnIP = int_0.ToString() + "." + ReturnIP;

            //�õ��ڶ�λIP
            int_0 = int_1 % 256;
            int_1 = int_1 / 256;
            ReturnIP = int_0.ToString() + "." + ReturnIP;

            //�õ���һλIP
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
