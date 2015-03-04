using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Basement.Framework.Utility
{
    /// <summary>
    /// 格式检查
    /// </summary>
    public class CheckFormat
    {
        /// <summary>
        /// ReturnBoolValue
        /// </summary>
        private static bool ReturnBoolValue;
        /// <summary>
        /// 输入字符格式检查
        /// </summary>
        /// <param name="Input">输入的字符</param>
        /// <param name="Pattern">正则表达式</param>
        /// <returns>bool</returns>
        public static bool CheckInputFormat(string Input, string Pattern)
        {
            ReturnBoolValue = false;
            Regex regex = new Regex(Pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            if (regex.IsMatch(Input)) ReturnBoolValue = true;
            return ReturnBoolValue;
        }
    }

    /// <summary>
    ///结构 - 通用正则表达式 
    /// </summary>
    public struct PatternStruct
    {
        /// <summary>
        /// 无符号整数
        /// </summary>
        public static readonly string UnsignedInteger = @"^\d+$";

        /// <summary>
        /// 正则表达式 - Email
        /// </summary>
        public static readonly string Email = @"^([a-zA-Z0-9])([a-zA-Z0-9_\.-])*@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,}){1,8})$";

        /// <summary>
        /// 正则表达式 - Mobile
        /// </summary>
        public static readonly string Mobile = @"^((\(\d{3}\))|(\d{3}\-))?13\d{9}|15\d{9}|14\d{9}|18\d{9}$";

        /// <summary>
        /// 正则表达式 - Telephone
        /// </summary>
        public static readonly string Telephone = @"^(0[0-9]{2,3}\-)([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$";

        /// <summary>
        /// 正则表达式 - DateTime (验证是否日期格式
        /// 可判断格式如下（其中-可替换为/，不影响验证)
        /// YYYY | YYYY-MM | YYYY-MM-DD | YYYY-MM-DD HH:MM:SS | YYYY-MM-DD HH:MM:SS.FFF)
        /// </summary>
        public static readonly string DateTime = @"[1-2]{1}[0-9]{3}((-|\/){1}(([0]?[1-9]{1})|(1[0-2]{1}))((-|\/){1}((([0]?[1-9]{1})|([1-2]{1}[0-9]{1})|(3[0-1]{1})))( (([0-1]{1}[0-9]{1})|2[0-3]{1}):([0-5]{1}[0-9]{1}):([0-5]{1}[0-9]{1})(\.[0-9]{3})?)?)?)?$";

        /// <summary>
        /// 正则表达式 - ChinaUnicomNumber (是否是联通的号码 测试通过)
        /// </summary>
        public static readonly string ChinaUnicomNumber = @"^(130|131|132|133|152|155|156|185|186)[0-9]{8}";

        /// <summary>
        /// 正则表达式 - ChinaMobileNumber (是否是移动的号码 测试通过)
        /// </summary>
        public static readonly string ChinaMobileNumber = @"^(135|136|137|138|139|134|150|151|157|158|159|187|188)[1-9]{7,8}";

        /// <summary>
        /// 正则表达式 - Uni2uniCradCode (联嘉卡号 - 至少输入16位数字 - 需要根据实际需求进行调整)
        /// </summary>
        public static readonly string Uni2uniCradCode = @"^\d{16,}$";

        /// <summary>
        /// 正则表达式 - UserName (检验用户名格式,允许输入中文)
        /// </summary>
        public static readonly string UserName = @"^[a-zA-Z0-9\u4e00-\u9fa5]*[^']\w$";

        /// <summary>
        /// 正则表达式 - Password (密码由6-16个字符组成，请使用英文字母加数字或符号的组合密码)
        /// </summary>
        public static readonly string Password = @"^[a-zA-Z0-9]*[^']\w$";

        /// <summary>
        /// 正则表达式 - ZipCode (邮政编码6位数字)
        /// </summary>
        public static readonly string ZipCode = @"^\d{6}$";
    }

}
