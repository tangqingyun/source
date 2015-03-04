using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.Data.Attribute
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class TableAttribute : System.Attribute
    {
        /// <summary>
        /// 数据库表名
        /// </summary>
        public string Name { get; set; }

        public TableAttribute(string name)
        {
            this.Name = name;
        }
    }
}
