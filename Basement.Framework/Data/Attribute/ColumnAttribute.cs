using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Basement.Framework.Data.Attribute
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class ColumnAttribute : System.Attribute
    {
        /// <summary>
        /// 数据库字段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public DbType DataType { get; set; }

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool PrimaryKey { get; set; }

        /// <summary>
        /// 只读,如自增主键，计算列等
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>字段名称, 只读
        /// </summary>
        /// <param name="name"></param>
        /// <param name="readOnly"></param>
        public ColumnAttribute(string name, DbType dbType, bool primaryKey = false, bool readOnly = false)
        {
            this.Name = name;
            this.DataType = dbType;
            this.PrimaryKey = primaryKey;
            this.ReadOnly = readOnly;
        }
    }
}
