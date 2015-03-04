using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.Data.Enum
{
    /// <summary>
    /// 数据库Action枚举
    /// </summary>
    public enum EnumAction
    {
        /// <summary>
        /// 新增
        /// </summary>
        INSERT = 0,
        /// <summary>
        /// 修改
        /// </summary>
        UPDATE = 1,
        /// <summary>
        /// 删除
        /// </summary>
        DELETE = 2,
        /// <summary>
        /// 查询
        /// </summary>
        SELECT = 3
    }
}
