using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.ComponentModel;

namespace Basement.Framework.EnumExt
{

    public static class EnumExt
    {
        #region = GetEnumDescription =
        /// <summary>
        /// ��ȡö��ֵ����ϸ�ı�
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetEnumDescription(object e)
        {
            //��ȡ�ֶ���Ϣ
            System.Reflection.FieldInfo[] ms = e.GetType().GetFields();

            Type t = e.GetType();
            foreach (System.Reflection.FieldInfo f in ms)
            {
                //�ж������Ƿ����
                if (f.Name != e.ToString()) continue;

                //������Զ�������
                foreach (Attribute attr in f.GetCustomAttributes(true))
                {
                    //����ת���ҵ�һ��Description����Description��Ϊ��Ա����
                    System.ComponentModel.DescriptionAttribute dscript = attr as System.ComponentModel.DescriptionAttribute;
                    if (dscript != null)
                        return dscript.Description;
                }

            }

            //���û�м�⵽���ʵ�ע�ͣ�����Ĭ������
            return e.ToString();
        }

        public static string GetEnumDescription<T>(T enumeratedType)
        {
            string description = enumeratedType.ToString();

            Type enumType = typeof(T);
            // Can't use type constraints on value types, so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            FieldInfo fieldInfo = enumeratedType.GetType().GetField(enumeratedType.ToString());

            if (fieldInfo != null)
            {
                object[] attribues =
                    fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attribues != null && attribues.Length > 0)
                {
                    description = ((DescriptionAttribute)attribues[0]).Description;
                }
            }

            return description;
        }
        #endregion

        #region - EnumToTable -
        /// <summary>
        /// ���ĳ��Enum���͵İ��б�
        /// </summary>
        /// <param name="enumType">ö�ٵ����ͣ����磺typeof(Sex)</param>
        /// <returns>
        /// ����һ��DataTable
        /// DataTable �����У�    "Text"    : System.String;
        ///                        "Value"    : System.Char
        /// </returns>
        /// <example>http://www.cnblogs.com/zellzhang/archive/2005/10/17/256576.html</example>
        public static DataTable EnumListTable(Type enumType)
        {
            if (enumType.IsEnum != true)
            {    //����ö�ٵ�Ҫ����
                throw new InvalidOperationException();
            }

            //����DataTable������Ϣ
            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(System.String));
            dt.Columns.Add("Value", typeof(System.Char));

            //�������Description��������Ϣ
            Type typeDescription = typeof(DescriptionAttribute);

            //���ö�ٵ��ֶ���Ϣ����Ϊö�ٵ�ֵʵ������һ��static���ֶε�ֵ��
            System.Reflection.FieldInfo[] fields = enumType.GetFields();

            //���������ֶ�
            foreach (FieldInfo field in fields)
            {
                //���˵�һ������ö��ֵ�ģ���¼����ö�ٵ�Դ����
                if (field.FieldType.IsEnum == true)
                {
                    DataRow dr = dt.NewRow();

                    // ͨ���ֶε����ֵõ�ö�ٵ�ֵ
                    // ö�ٵ�ֵ�����long�Ļ���ToChar�������⣬��������ڱ��ĵ����۷�Χ֮��
                    dr["Value"] = (char)(int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);

                    //�������ֶε������Զ������ԣ�����ֻ����Description����
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        //��ΪDescription����Զ��������ǲ������ظ��ģ���������ֻȡ��һ���Ϳ����ˣ�
                        DescriptionAttribute aa = (DescriptionAttribute)arr[0];
                        //������Ե�����ֵ��Ҳ���ǡ��С���Ů������������
                        dr["Text"] = aa.Description;
                    }
                    else
                    {
                        //���û������������-_-!����д�˰�~����ô����ʾӢ�ĵ��ֶ���
                        dr["Text"] = field.Name;
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }


        #endregion
    }
}
