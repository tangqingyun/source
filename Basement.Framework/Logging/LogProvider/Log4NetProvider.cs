using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.Logging.LogProvider
{
    public class Log4NetProvider : Log4NetLog, ILogProvider
    {
        public Log4NetProvider(string loggerName)
            : base(loggerName) { }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="level">级别</param>
        /// <param name="message">内容</param>
        /// <param name="ex">异常</param>
        public void Log(EnumLogLevel level, object message, string module=null, Exception ex = null)
        {
            ILog log = base.GetLogger();
            base.Write(log, level, message, ex);
        }

    }


}
