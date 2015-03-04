using System;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Basement.Framework.Security
{

	public class ValidateUtil
	{
		public static Regex RegNumber = new Regex("^[0-9]+$");//无符号整数（允许前面有多余0）
        public static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");//有符号整数（允许前面有多余0）
        public static Regex RegDecimal = new Regex("^[0-9]+(([.][0-9]+)|[0-9]?)$");//无符号整数或小数（允许前后有多余0）
        public static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+(([.][0-9]+)|[0-9]?)$"); //有符号整数或小数（允许前后有多余0）
       
        public static Regex RegEmail = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");//Email
        //日期yyyy-MM-dd（yyyy:0001-9999 已经考虑平年和闰年）
        public static Regex RegDate = new Regex("^(([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8]))))|((([0-9]{2})(0[48]|[2468][048]|[13579][26])|((0[48]|[2468][048]|[3579][26])00))-02-29)$");
        //日期MM/dd/yyyy（yyyy:0001-9999 已经考虑平年和闰年）
        public static Regex RegDate2 = new Regex("^((((0[13578]|1[02]))/(0[1-9]|[12][0-9]|3[01])|((0[469]|11)/(0[1-9]|[12][0-9]|30))|(02)/(0[1-9]|[1][0-9]|2[0-8]))/([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3}))|(02/29/(([0-9]{2})(0[48]|[2468][048]|[13579][26])|((0[48]|[2468][048]|[3579][26])00)))$");
        
        public static Regex RegLegalUsername = new Regex(@"^[a-zA-Z]\w*$");//合法用户名检测（只能包含字母、数字、下划线，且以字母开头，长度>=3,<=20）
        public static Regex RegLegalPassword = new Regex(@"^\w*$");//合法的密码（只能包含字母、数字、下划线,长度>=6,<=20）
        public static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");//中文字符检测
        public static Regex RegLegalUsernameOrEmail = new Regex(@"(^[a-zA-Z]\w{2,19}$)|(^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$)");//合法的用户名或Email

		#region 数字字符串检查
		/// <summary>
		/// 是否数字字符串
		/// </summary>
		/// <param name="inputData">输入字符串</param>
		/// <returns></returns>
		public static bool IsNumber(string inputData)
		{
			Match m = RegNumber.Match(inputData);
			return m.Success;
		}		
		/// <summary>
		/// 是否数字字符串 可带正负号
		/// </summary>
		/// <param name="inputData">输入字符串</param>
		/// <returns></returns>
		public static bool IsNumberSign(string inputData)
		{
			Match m = RegNumberSign.Match(inputData);
			return m.Success;
		}		
		/// <summary>
		/// 是否是浮点数
		/// </summary>
		/// <param name="inputData">输入字符串</param>
		/// <returns></returns>
		public static bool IsDecimal(string inputData)
		{
			Match m = RegDecimal.Match(inputData);
			return m.Success;
		}		
		/// <summary>
		/// 是否是浮点数 可带正负号
		/// </summary>
		/// <param name="inputData">输入字符串</param>
		/// <returns></returns>
		public static bool IsDecimalSign(string inputData)
		{
			Match m = RegDecimalSign.Match(inputData);
			return m.Success;
		}

        /// <summary>
        /// 验证手机号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsMobile(string str)
        {
            return Regex.IsMatch(str, "^1[0-9]{10}$", RegexOptions.IgnoreCase);
        }


		#endregion
        #region 邮件地址
        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入待校验的字符串信息</param>
        /// <returns></returns>
        public static bool IsEmail(string inputData)
        {
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }
        #endregion
        #region 日期字符串检查
        /// <summary>
        /// 监测日期字符串
        /// </summary>
        /// <param name="inputData">输入待校验的字符串信息</param>
        /// <returns></returns>
        public static bool IsDate(string inputData)
        {
            Match m = RegDate.Match(inputData);
            Match m2 = RegDate2.Match(inputData);
            return m.Success || m2.Success;
        }
        #endregion
        #region 合法用户名密码
        /// <summary>
        /// 检测是否为合法用户名
        /// </summary>
        /// <param name="inputData">输入待校验的字符串信息</param>
        /// <returns></returns>
        public static bool IsLegalUsername(string inputData)
        {
            Match m = RegLegalUsername.Match(inputData);
            return m.Success;
        }

        public static bool IsLegalUsernameOrEmail(string inputData)
        {
            Match m = RegLegalUsernameOrEmail.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 检测是否为合法密码
        /// </summary>
        /// <param name="inputData">输入待校验的字符串信息</param>
        /// <returns></returns>
        public static bool IsLegalPassword(string inputData)
        {
            Match m = RegLegalPassword.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 校验手机号码是否符合标准。
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool ValidateMobile(string mobile)
        {
            if (string.IsNullOrEmpty(mobile))
                return false;

            return Regex.IsMatch(mobile, @"^(13|14|15|16|18|19)\d{9}$");
        }

        #endregion
        #region 中文检测
        /// <summary>
		/// 检测是否有中文字符
		/// </summary>
        /// <param name="inputData">输入待校验的字符串信息</param>
		/// <returns></returns>
		public static bool IsHasCHZN(string inputData)
		{
			Match m = RegCHZN.Match(inputData);
			return m.Success;
		}	
		#endregion
        #region 集合比较
        public static bool IsCollectionEqual<T>(ICollection<T> a, ICollection<T> b)
        {
            foreach (T ta in a)
            {
                bool find = false;
                foreach (T tb in b)
                {
                    if (tb.Equals(ta))
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                    return false;
            }
            foreach (T tb in b)
            {
                bool find = false;
                foreach (T ta in a)
                {
                    if (ta.Equals(tb))
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                    return false;
            }
            if (b.Count != a.Count)
                return false;
            return true;
        }

        public static bool IsChildCollection<T>(ICollection<T> a, ICollection<T> b)
        {
            foreach (T at in a)
            {
                if (!b.Contains(at))
                    return false;
            }
            return true;
        }

        public static bool IsSet<T>(ICollection<T> a)
        {
            Collection<T> temp = new Collection<T>();
            foreach (T at in a)
            {
                if (temp.Contains(at))
                    return false;
                temp.Add(at);
            }
            return true;
        }
        #endregion
    }
}
