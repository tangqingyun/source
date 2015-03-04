using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.Utility
{
    public class MathHelper
    {
        /// <summary>
        /// 实现数据的四舍五入法
        /// </summary>
        /// <param name="d">要进行处理的数据</param>
        /// <param name="i">保留的小数位数</param>
        /// <returns>四舍五入后的结果</returns>
        public static double Round(double d, int i)
        {
            {
                string str = d.ToString();
                int idot = str.IndexOf('.');
                if (idot <= 0)
                {
                    return d;
                }

                if (d >= 0)
                {
                    d += 5 * Math.Pow(10, -(i + 1));
                }
                else
                {
                    d += -5 * Math.Pow(10, -(i + 1));
                }
                string[] strs = str.Split('.');
                string prestr = strs[0];
                string poststr = strs[1];
                if (poststr.Length > i)
                {
                    poststr = str.Substring(idot + 1, i);
                }
                string strd = prestr + "." + poststr;
                d = Double.Parse(strd);
                return d;
            }
        }

    }
}
