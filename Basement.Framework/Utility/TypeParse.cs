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
        /// �ж϶����Ƿ�ΪInt32���͵�����
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
        /// string��ת��Ϊbool��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����bool���ͽ��</returns>
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
        /// ������ת��ΪInt32����
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
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
        /// string��ת��Ϊfloat��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
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
        /// �жϸ������ַ�������(strNumber)�е������ǲ��Ƕ�Ϊ��ֵ��
        /// </summary>
        /// <param name="strNumber">Ҫȷ�ϵ��ַ�������</param>
        /// <returns>���򷵼�true �����򷵻� false</returns>
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

        #region ����ת������
        /*
        * ������գ�
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

        #region ϵͳ����ת�����ݿ�����
        /// <summary>
        /// ʵ�� System.Type ת���� DbType
        /// ��Ϊֻ�ҵ� SqlDbType �� System.Type�Ķ��ձ���û�� DbType �� System.Type�Ķ��ձ�
        /// ���Ի�Ҫͨ��SqlParameter�� SqlDbType ת�� DbType.
        /// </summary>
        /// <param name="SourceType">����Ҫת����System.Type</param>
        /// <returns>���ض�Ӧ�� DbType </returns>
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
        /// �ַ���ת����GUID
        /// </summary>
        /// <param name="source">��Ҫת�����ַ���ֵ</param>
        /// <returns>GUIDֵ</returns>
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
