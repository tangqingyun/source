using Basement.Framework.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Basement.Framework.Configuration
{
    public class Log4NetConfig
    {
        private static readonly string Log4NetFile = SysBaseHandle.BIN_DIR + @"Configs\Log4Net.config";
        /// <summary>
        /// 加载日志
        /// </summary>
        public Log4NetConfig() {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Log4NetFile));//加载log4net配置文件;
        }
        /// <summary>
        /// 初始化配置
        /// </summary>
        public static void InitLog4NetConfig() { 
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Log4NetFile));
        }
    }
}
