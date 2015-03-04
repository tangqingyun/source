using Basement.Framework.Data.Attribute;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Basement.Framework.Data.Core
{
    /// <summary>
    /// 实体属性
    /// </summary>
    [Serializable]
    public struct EntityProperty
    {
        public readonly System.Reflection.PropertyInfo PropertyInfo;
        public readonly string PropertyName;
        public readonly MethodInfo SetMethod;
        public readonly MethodInfo GetMethod;
        
        public readonly Type PropertyType;
        public readonly string ColumnName;
        public readonly string ParameterName;
        public readonly bool PrimaryKey;
        public readonly DbType ColumnType;
        public readonly bool ReadOnly;

        public EntityProperty(PropertyInfo prop, ColumnAttribute attr)
        {
            PropertyInfo = prop;
            PropertyName = prop.Name;
            PropertyType = prop.PropertyType;
            SetMethod = prop.GetSetMethod();
            GetMethod = prop.GetGetMethod();

            ColumnName = attr.Name;
            ParameterName = "@" + attr.Name;
            PrimaryKey = attr.PrimaryKey;
            ColumnType = attr.DataType;
            ReadOnly = attr.ReadOnly;
        }
    }
}
