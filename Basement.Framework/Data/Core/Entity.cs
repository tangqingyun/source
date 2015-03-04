using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using Basement.Framework.Data.Attribute;

namespace Basement.Framework.Data.Core
{
    [Serializable]
    public static class Entity<T>
    {
        public static void Initialize() { }
        /// <summary>
        /// 表名
        /// </summary>
        public static string TableName { get; private set; }        
        /// <summary>
        /// 实体属性
        /// </summary>
        public static IDictionary<string, EntityProperty> Properties { get; private set; }

        public static string PrimaryKey { get; set; }

        public static PropertyInfo[] ColumnsPropertyWithOutReadony { get; private set; }

        public static PropertyInfo[] ColumnsPropertyWithReadony { get; private set; }

        public static PropertyInfo[] ColumnsProperty { get; private set; }

        public static string[] ColumnsWithOutReadony { get; private set; }

        public static string[] ColumnsWithReadony { get; private set; }

        public static string[] Columns { get; private set; }

        public static Type EntityType { get; private set; }

        public static IList<EntityProperty> EntityProperties { get; private set; }

        static Entity()
        {
            initialization();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        static void initialization()
        {
            EntityType = typeof(T);

            TableAttribute tableAttribute = System.Attribute.GetCustomAttribute(EntityType, typeof(TableAttribute), true) as TableAttribute;
            
            TableName = string.Format("[{0}]", (tableAttribute != null) ? tableAttribute.Name : EntityType.Name);

            EntityProperties = GetEntityProperties().ToList();
            Properties = GetEntityProperties().ToDictionary(s => s.PropertyInfo.Name);
           
            PrimaryKey = Properties.Values.Where(s => s.PrimaryKey).Select(s => s.ColumnName).FirstOrDefault();

            ColumnsPropertyWithOutReadony = Properties.Values.Where(s => !s.ReadOnly).Select(s => s.PropertyInfo).ToArray();
            ColumnsPropertyWithReadony = Properties.Values.Where(s => s.ReadOnly).Select(s => s.PropertyInfo).ToArray();
            ColumnsProperty = Properties.Values.Select(s => s.PropertyInfo).ToArray();

            ColumnsWithOutReadony = Properties.Values.Where(s => !s.ReadOnly).Select(s => s.ColumnName).ToArray();
            ColumnsWithReadony = Properties.Values.Where(s => s.ReadOnly).Select(s => s.ColumnName).ToArray();
            Columns = Properties.Values.Select(s => s.ColumnName).ToArray();
        }

        private static IEnumerable<EntityProperty> GetEntityProperties()
        {
            foreach (var prop in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (prop.GetIndexParameters().Length > 0) continue;
                var setMethod = prop.GetSetMethod(false);
                if (setMethod == null) continue;
                var attr = System.Attribute.GetCustomAttribute(prop, typeof(ColumnAttribute), true) as ColumnAttribute;
                if (attr == null) continue;
                yield return new EntityProperty(prop, attr);
            }
        }

    }
}
