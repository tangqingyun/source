using Basement.Framework.Common;
using log4net;
using log4net.Layout;
using log4net.Layout.Pattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Basement.Framework.Logging
{

    public class Log4NetLog
    {
        private static object _objLock = new object();
        private string _loggerName;
        public string LoggerName
        {
            get { return _loggerName; }
        }
        private ILog curLog = null;
        public Log4NetLog(string loggerName)
        {
            this._loggerName = loggerName;
        }
        protected ILog GetLogger()
        {
            ILog resultLog = null;
            if (resultLog == null)
            {
                if (curLog == null)
                {
                    lock (_objLock)
                    {
                        if (curLog == null && !string.IsNullOrEmpty(LoggerName))
                        {
                            curLog = LogManager.GetLogger(LoggerName);
                        }
                    }
                }
                return curLog;
            }
            else
            {
                return resultLog;
            }
        }
        
        protected void Write(ILog log, EnumLogLevel level, object message, Exception ex = null)
        {
            switch (level)
            { 
                case EnumLogLevel.Info:
                    if (log.IsInfoEnabled)
                    {
                        log.Info(message);
                    }
                    break;
                case EnumLogLevel.Warn:
                    if (log.IsWarnEnabled)
                    {
                        log.Warn(message);
                    }
                    break;
                case EnumLogLevel.Debug:
                    if (log.IsDebugEnabled)
                    {
                        log.Debug(message, ex);
                    }
                    break;
                case EnumLogLevel.Error:
                    if (log.IsErrorEnabled)
                    {
                        log.Error(message, ex);
                    }
                    break;
                case EnumLogLevel.Fatal:
                    if (log.IsFatalEnabled)
                    {
                        log.Fatal(message);
                    }
                    break;
            }
        }
    }

    public class CommonPatternLayout : PatternLayout {
        public CommonPatternLayout()
        {
            this.AddConverter("property", typeof(XPatternLayoutConverter));
        }
    }

    public class XPatternLayoutConverter : PatternLayoutConverter
    {
        protected override void Convert(System.IO.TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
        {
            if (Option != null)
            {
                // Write the value for the specified key
                WriteObject(writer, loggingEvent.Repository, LookupProperty(Option, loggingEvent));
            }
            else
            {
                // Write all the key value pairs
                WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
            }
        }

        /// <summary>
        /// 通过反射获取传入的日志对象的某个属性的值
        /// </summary>
        /// <param name="property"></param>
        /// <param name="loggingEvent"> </param>
        /// <returns></returns>
        private Object LookupProperty(String property, log4net.Core.LoggingEvent loggingEvent)
        {
            Object propertyValue = String.Empty;
            PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);
            if (propertyInfo != null)
            {
                propertyValue = propertyInfo.GetValue(loggingEvent.MessageObject, null);
            }
            return propertyValue;
        }

    }

}
