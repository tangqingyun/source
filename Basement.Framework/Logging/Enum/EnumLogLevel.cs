using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.Logging
{
    /// <summary>
    /// 日志级别枚举
    /// </summary>
    public enum EnumLogLevel
    {
        /// <summary>
        /// 信息
        /// </summary>
        Info = 1,
        /// <summary>
        /// 调试
        /// </summary>
        Debug = 2,
        /// <summary>
        /// 警告
        /// </summary>
        Warn = 3,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 4,
        /// <summary>
        /// 致命错误
        /// </summary>
        Fatal = 5
    }
}
