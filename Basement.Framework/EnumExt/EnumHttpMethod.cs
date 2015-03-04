using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.EnumExt
{
    /// <summary>
    /// Http请求方式枚举
    /// </summary>
    public enum EnumHttpMethod
    {
        Other = 0,
        Unknow = 999,
        Post = 1,
        Get = 2,
        Delete = 3,
        Head = 4,
        Method = 5,
        Options = 6,
        Put = 7,
        Trace = 8,
    }
}
