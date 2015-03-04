using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;

namespace Basement.Framework.Utility
{

    public class TypeParse
    {
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object Expression)
        {
            if (Expression != null)
            {
                string str = Expression.ToString();
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?\d*[.]?\d*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;

        }


        public static bool IsDouble(object Expression)
        {
            if (Expression != null)
            {
                return new Regex(@"^([0-9])[0-9]*(\.\w*)?$", Utils.GetRegexCompiledOptions()).IsMatch(Expression.ToString());
            }
            return false;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object Expression, bool defValue)
        {
            if (Expression != null)
            {
                if (string.Compare(Expression.ToString(), "true", true) == 0)
                {
                    return true;
                }
                else if (string.Compare(Expression.ToString(), "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object Expression, int defValue)
        {

            if (Expression != null)
            {
                string str = Expression.ToString();
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?\d*[.]?\d*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return Convert.ToInt32(str);
                    }
                }
            }
            return defValue;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            if ((strValue == null) || (strValue.ToString().Length > 10))
            {
                return defValue;
            }

            float intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = new Regex(@"^([-]|[0-9])[0-9]*(\.\w*)?$").IsMatch(strValue.ToString());
                if (IsFloat)
                {
                    intValue = Convert.ToSingle(strValue);
                }
            }
            return intValue;
        }


        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            foreach (string id in strNumber)
            {
                if (!IsNumeric(id))
                {
                    return false;
                }
            }
            return true;

        }

        #region 类型转换数组
        /*
        * 数组参照：
        */
        private static String[,] DBTypeConvertrsionKey = new String[,]
        {
            {"AnsiString", "System.String"},
            {"Binary", "System.Byte[]"},
            {"Byte", "System.Byte"},
            {"Boolean", "System.Boolean"},
            {"Currency", "System.Decimal"},
            {"Date", "System.DateTime"},
            {"DateTime", "System.DateTime"},
            {"Decimal", "System.Decimal"},
            {"Double", "System.Double"},
            {"Guid", "System.Guid"},
            {"Int16", "System.Int16"},
            {"Int32", "System.Int32"},
            {"Int64", "System.Int64"},
            {"Object", "System.Object"},
            {"SByte", "System.SByte"},
            {"Single", "System.Single"},
            {"String", "System.String"},
            {"Time", "System.DateTime"},
            {"UInt16", "System.UInt16"},
            {"UInt32", "System.UInt32"},
            {"UInt64", "System.UInt64"},
            {"VarNumeric", "System.Object"},
            {"AnsiStringFixedLength", "System.String"},
            {"StringFixedLength", "System.String"},
            {"Xml", "System.Xml"}
        };
        #endregion

        #region 系统类型转成数据库类型
        /// <summary>
        /// 实现 System.Type 转换成 DbType
        /// 因为只找到 SqlDbType 与 System.Type的对照表，而没有 DbType 与 System.Type的对照表，
        /// 所以还要通过SqlParameter把 SqlDbType 转成 DbType.
        /// </summary>
        /// <param name="SourceType">传入要转换的System.Type</param>
        /// <returns>返回对应的 DbType </returns>
        public static DbType SystemTypeToDbType(System.Type SourceType)
        {
            String dbTypeName = String.Empty;
            int keyCount = DBTypeConvertrsionKey.GetLength(0);
            for (int i = 0; i < keyCount; i++)
            {
                if (DBTypeConvertrsionKey[i, 1].Equals(SourceType.FullName))
                {
                    dbTypeName = DBTypeConvertrsionKey[i, 0];
                    break;
                }
            }
            if (dbTypeName == String.Empty) dbTypeName = "Variant";
            return (DbType)Enum.Parse(typeof(DbType), dbTypeName);
        }
        #endregion

        /// <summary>
        /// 字符串转换成GUID
        /// </summary>
        /// <param name="source">需要转换的字符串值</param>
        /// <returns>GUID值</returns>
        public static Guid StringToGuid(string source)
        {
            Guid guid;
            try
            {
                guid = new Guid(source);
            }
            catch
            {
                guid = new Guid();
            }
            return guid;
        }
    }
}
