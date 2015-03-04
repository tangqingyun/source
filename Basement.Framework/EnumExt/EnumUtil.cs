using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Basement.Framework.EnumExt
{
 
    public static class EnumUtil
    {
        /// <summary>从枚举类型和它的特性读出并返回一个键值对
        /// </summary>
        /// <param name="enumType">Type,该参数的格式为typeof(需要读的枚举类型)</param>
        /// <returns>键值对</returns>
        public static NameValueCollection GetNVCFromEnumValue(Type enumType)
        {
            NameValueCollection nvc = new NameValueCollection();
            Type typeDescription = typeof(DescriptionAttribute);
            System.Reflection.FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            string strValue = string.Empty;
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        DescriptionAttribute aa = (DescriptionAttribute)arr[0];
                        strText = aa.Description;
                    }
                    else
                    {
                        strText = field.Name;
                    }
                    nvc.Add(strValue, strText);
                }
            }

            return nvc;
        }

        /// <summary>返回 Dic<枚举项，描述>
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns>Dic<枚举项，描述></returns>
        public static Dictionary<string, string> GetEnumDic(Type enumType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            FieldInfo[] fieldinfos = enumType.GetFields();
            foreach (FieldInfo field in fieldinfos)
            {
                if (field.FieldType.IsEnum)
                {
                    Object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    dic.Add(field.Name, ((DescriptionAttribute)objs[0]).Description);
                }
            }

            return dic;
        }


        /// <summary>获得某个枚举项的描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDes(object value)
        {
            FieldInfo fieldinfo = value.GetType().GetField(value.ToString());
            Object[] objs = fieldinfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (objs == null || objs.Length == 0)
            {
                return value.ToString();
            }
            else
            {
                return ((DescriptionAttribute)objs[0]).Description;
            }
        }

        #region Enum Example
        public enum TimeOfDay
        {
            [Description("上午")]
            Moning = 0,
            [Description("下午")]
            Afternoon,
            [Description("晚上")]
            Evening,
        };

        public enum TimeOfDays
        {
            上午,
            下午,
            晚上
        };

        /// <summary>枚举的测试
        /// </summary>
        public static void EnumTest()
        {
            NameValueCollection nvc = GetNVCFromEnumValue(typeof(TimeOfDay));
            foreach (string key in nvc.Keys)
            {
                Console.WriteLine(string.Format(key + ": {0}", nvc[key]));
            }

            Dictionary<string, string> dic = GetEnumDic(typeof(TimeOfDay));
            foreach (string key in dic.Keys)
            {
                Console.WriteLine(key + ":{0}", dic[key]);
            }

            Console.WriteLine(string.Format(TimeOfDay.Moning.ToString() + ":{0}", GetEnumDes(TimeOfDay.Moning)));
            //输出：Moning：上午
        }
        #endregion
    }
}
