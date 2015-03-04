using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.Logging.LogProvider
{
    public class LogFactoryProvider
    {
        /// <summary>
        /// 获取日志驱动
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="loggerName">如果使用Log4Net记录日志、则必须选择日志Adapter名称</param>
        /// <returns></returns>
        public static ILogger GetLogProvider(EnumLogType type, string loggerName=null)
        {
            ILogger logger = null;
            switch (type)
            {
                case EnumLogType.Text:
                    logger = new LogTextProvider();
                    break;
                case EnumLogType.Log4Net:
                    logger = new Log4NetProvider(loggerName);
                    break;
            }
            return logger;
        }
    }
}
