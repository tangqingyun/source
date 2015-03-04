using Basement.Framework.Data.Core;
using Basement.Framework.Data.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Basement.Framework.Utility
{

    public class ReflectHandle
    {

        #region == 01 将IDataReader转化成实体类

        /// <summary>
        /// 将IDataReader转化成实体类
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dr">当前行</param>
        /// <returns></returns>
        public static T DataReaderAsModel<T>(IDataReader dr)
        {
            if (dr == null)
            {
                throw new ArgumentNullException("dr");
            }

            T item = Activator.CreateInstance<T>();
            var columnCount = dr.FieldCount;
            for (int i = 0; i < columnCount; i++)
            {
                //根据数据列名称得到匹配的实体属性对象。
                var columnName = dr.GetName(i);
                var itemProperty = item.GetType().GetProperty(columnName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                //如果实体属性对象未找到，则跳过赋值操作
                if (itemProperty == null)
                {
                    continue;
                }

                //实体属性对象进行赋值操作
                try
                {
                    if (dr[i] == System.DBNull.Value)
                    {
                        var defultValue = GetDefaultValue(itemProperty);

                        var curItemProperty = itemProperty.GetValue(item, null);
                        if (curItemProperty == null || string.IsNullOrEmpty(curItemProperty.ToString()))
                        {
                            itemProperty.SetValue(item, defultValue, null);
                        }
                    }
                    else
                    {
                        itemProperty.SetValue(item, Convert.ChangeType(dr[i], itemProperty.PropertyType), null);
                    }
                }
                catch (Exception ex)
                {
                    itemProperty.SetValue(item, GetIsObjectNullValue(itemProperty, dr[i]), null);
                }
            }

            return item;

        }

        #endregion

        #region == 02 获得指定属性的默认值
        /// <summary>
        /// 获得指定属性的默认值
        /// </summary>
        /// <param name="p">属性对象</param>
        /// <returns>获得的默认值</returns>
        private static object GetDefaultValue(PropertyInfo p)
        {
            Type pt = p.PropertyType;
            System.ComponentModel.DefaultValueAttribute dva =
                (System.ComponentModel.DefaultValueAttribute)
                Attribute.GetCustomAttribute(
                p, typeof(System.ComponentModel.DefaultValueAttribute));
            if (dva == null)
            {
                if (pt.Equals(typeof(byte)))
                    return (byte)0;
                else if (pt.Equals(typeof(short)))
                    return (short)0;
                else if (pt.Equals(typeof(int)))
                    return (int)0;
                else if (pt.Equals(typeof(uint)))
                    return (uint)0;
                else if (pt.Equals(typeof(long)))
                    return (long)0;
                else if (pt.Equals(typeof(float)))
                    return (float)0;
                else if (pt.Equals(typeof(double)))
                    return (double)0;
                else if (pt.Equals(typeof(decimal)))
                    return (decimal)0;
                else if (pt.Equals(typeof(DateTime)))
                    return DateTime.MinValue;
                else if (pt.Equals(typeof(char)))
                    return char.MinValue;
                else if (pt.Equals(typeof(Guid)))
                    return Guid.Empty;
                else if (pt.Equals(typeof(string)))
                    return string.Empty;
                else
                    return null;
            }
            else
            {
                System.ComponentModel.TypeConverter converter =
                    System.ComponentModel.TypeDescriptor.GetConverter(pt);
                if (dva.Value != null)
                {
                    Type t = dva.Value.GetType();
                    if (t.Equals(pt) || t.IsSubclassOf(pt))
                    {
                        return dva.Value;
                    }
                }
                if (converter == null)
                {
                    return dva.Value;
                }
                else
                {
                    return converter.ConvertFrom(dva.Value);
                }
            }
        }
        #endregion

        #region == 03 获取可空类型值
        /// <summary>
        /// 获取可空类型值
        /// </summary>
        /// <param name="itemProperty"></param>
        /// <returns></returns>
        private static object GetIsObjectNullValue(PropertyInfo itemProperty, object col)
        {
            try
            {
                var pt = itemProperty.PropertyType;
                if (pt == typeof(DateTime?))
                {
                    return Convert.ToDateTime(col);
                }
                else if (pt == typeof(int?))
                {
                    return Convert.ToInt32(col);
                }
                else if (pt == typeof(Guid?))
                {
                    return Guid.Parse(col.ToString());
                }
                else if (pt == typeof(decimal?))
                {
                    return Convert.ToDecimal(col);
                }
                else if (pt == typeof(long?))
                {
                    return Convert.ToInt64(col.ToString());
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region == 04 获得某个实体字段的值
        /// <summary>
        /// 获得某个实体字段的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public static object GetFieldValue<T>(T t, string FieldName)
        {
            if (string.IsNullOrEmpty(FieldName)) throw new ArgumentNullException("FieldName");
            if (t == null) throw new ArgumentNullException("t");
            object ReturnObjValue = null;
            try
            {
                System.Reflection.PropertyInfo[] propertys = t.GetType().GetProperties();
                //检索所有字段
                foreach (PropertyInfo property in propertys)
                {
                    if (property.Name.ToLower() == FieldName.ToLower())
                    {
                        ReturnObjValue = property.GetValue(t, null);
                        break;
                    }
                }
            }
            catch
            {
                ReturnObjValue = null;
            }

            return ReturnObjValue;
        }
        #endregion

        #region == 05 将Table转化成List集合
        /// <summary>
        /// 将Table转化成List集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> DataTableAsList<T>(DataTable dt)
        {
            if (dt == null) new Exception("DataTale is null");

            if (dt.Rows.Count == 0) new Exception("row is 0");

            IList<T> list = new List<T>();
            Type type = typeof(T);
            PropertyInfo[] allPropertyArray = type.GetProperties();
            if (allPropertyArray.Count() == 0) new Exception("类型没有公共属性");
            foreach (DataRow dr in dt.Rows)
            {
                T element = Activator.CreateInstance<T>();
                if (dt.Columns.Count == 0) new Exception("没有找到列");
                foreach (DataColumn columnElement in dt.Columns)
                {
                    SetPropertyValue(allPropertyArray, dr, columnElement);
                }

                list.Add(element);
            }
            return list;
        }

        #endregion

        #region == 06 给类型属性赋值
        /// <summary>
        /// 给类型属性赋值
        /// </summary>
        /// <param name="allPropertyArray">属性数组</param>
        /// <param name="dr">当前行</param>
        /// <param name="column">当前列</param>
        private static void SetPropertyValue(PropertyInfo[] allPropertyArray, DataRow dr, DataColumn column)
        {
            foreach (PropertyInfo property in allPropertyArray)
            {
                if (!property.Name.ToLower().Equals(column.ColumnName.ToLower().Replace("_", ""))) 
                    continue;
                var columnValue = dr[column.ColumnName];
                if (columnValue == DBNull.Value)
                    property.SetValue(column, GetDefaultValue(property), null);
                else
                    property.SetValue(column, columnValue,null);
            }
        }
        #endregion

        #region == 07 反射动态创建实例
        /// <summary>
        /// 反射动态创建实例
        /// </summary>
        /// <param name="assemblyString">程序集</param>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public static object CreateInstance(string assemblyString, string className)
        {
            try
            {
                Assembly asb = Assembly.Load(assemblyString);
                return asb.CreateInstance(assemblyString + "." + className);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        
        /// <summary>
        /// 通过实体构造SQL语句
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="action">数据库Action枚举</param>
        /// <param name="listParameter">参数</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static string BuildScript<T>(EnumAction action, out IList<DbParameter> listParameter, string tableName = null)
        {
            StringBuilder sql = new StringBuilder();
            T t = Activator.CreateInstance<T>();
            Type type = t.GetType();
            PropertyInfo[] PropertyInfo = type.GetProperties();
            listParameter = new List<DbParameter>();
            int len = PropertyInfo.Length;
            if (action == EnumAction.INSERT)
            {
                sql.Append("INSERT INTO " + tableName + "(");
                for (int i = 0; i < len; i++)
                {
                    sql.AppendFormat("{0}", i < len ? PropertyInfo[i].Name + "," : PropertyInfo[i].Name);
                    listParameter.Add(new SqlParameter("@" + PropertyInfo[i].Name, PropertyInfo[i].GetValue(type, null)));
                }
                sql.Append(") VALUES(");
                for (int i = 0; i < len; i++)
                {
                    sql.AppendFormat("@{0}", i < len ? PropertyInfo[i].Name + "," : PropertyInfo[i].Name);
                }
                sql.Append(")");
            }
            else if (action == EnumAction.UPDATE)
            {
                sql.Append("UPDATE " + tableName + " SET ");
                for (int i = 0; i < len; i++)
                {
                    sql.AppendFormat("{0}=@{1}", PropertyInfo[i].Name, i < len ? PropertyInfo[i].Name + "," : PropertyInfo[i].Name);
                    listParameter.Add(new SqlParameter("@" + PropertyInfo[i].Name, PropertyInfo[i].GetValue(type, null)));
                }
            }

            return sql.ToString();
        }
       

    }

 

}
