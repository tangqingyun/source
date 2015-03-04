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
        /// 获取枚举值的详细文本
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetEnumDescription(object e)
        {
            //获取字段信息
            System.Reflection.FieldInfo[] ms = e.GetType().GetFields();

            Type t = e.GetType();
            foreach (System.Reflection.FieldInfo f in ms)
            {
                //判断名称是否相等
                if (f.Name != e.ToString()) continue;

                //反射出自定义属性
                foreach (Attribute attr in f.GetCustomAttributes(true))
                {
                    //类型转换找到一个Description，用Description作为成员名称
                    System.ComponentModel.DescriptionAttribute dscript = attr as System.ComponentModel.DescriptionAttribute;
                    if (dscript != null)
                        return dscript.Description;
                }

            }

            //如果没有检测到合适的注释，则用默认名称
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
        /// 获得某个Enum类型的绑定列表
        /// </summary>
        /// <param name="enumType">枚举的类型，例如：typeof(Sex)</param>
        /// <returns>
        /// 返回一个DataTable
        /// DataTable 有两列：    "Text"    : System.String;
        ///                        "Value"    : System.Char
        /// </returns>
        /// <example>http://www.cnblogs.com/zellzhang/archive/2005/10/17/256576.html</example>
        public static DataTable EnumListTable(Type enumType)
        {
            if (enumType.IsEnum != true)
            {    //不是枚举的要报错
                throw new InvalidOperationException();
            }

            //建立DataTable的列信息
            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(System.String));
            dt.Columns.Add("Value", typeof(System.Char));

            //获得特性Description的类型信息
            Type typeDescription = typeof(DescriptionAttribute);

            //获得枚举的字段信息（因为枚举的值实际上是一个static的字段的值）
            System.Reflection.FieldInfo[] fields = enumType.GetFields();

            //检索所有字段
            foreach (FieldInfo field in fields)
            {
                //过滤掉一个不是枚举值的，记录的是枚举的源类型
                if (field.FieldType.IsEnum == true)
                {
                    DataRow dr = dt.NewRow();

                    // 通过字段的名字得到枚举的值
                    // 枚举的值如果是long的话，ToChar会有问题，但这个不在本文的讨论范围之内
                    dr["Value"] = (char)(int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);

                    //获得这个字段的所有自定义特性，这里只查找Description特性
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        //因为Description这个自定义特性是不允许重复的，所以我们只取第一个就可以了！
                        DescriptionAttribute aa = (DescriptionAttribute)arr[0];
                        //获得特性的描述值，也就是‘男’‘女’等中文描述
                        dr["Text"] = aa.Description;
                    }
                    else
                    {
                        //如果没有特性描述（-_-!忘记写了吧~）那么就显示英文的字段名
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
