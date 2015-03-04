using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Basement.Framework.Utility
{
    public class EnvironmentUtility
    {
        /// <summary>
        /// 应用程序域名。
        /// </summary>
        public static string AppDomainName
        {
            get
            {
                string appDomainName = string.Empty;
                try
                {
                    appDomainName = AppDomain.CurrentDomain.FriendlyName;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return appDomainName;
            }
        }

        /// <summary>
        /// 获取些本地计算机所在域的名称。
        /// </summary>
        public static string DomainName
        {
            get
            {
                string domainName = string.Empty;
                string hostName = MachineFullName;
                if (hostName.StartsWith(MachineName, StringComparison.InvariantCultureIgnoreCase))
                {
                    domainName = hostName.Substring(MachineName.Length + 1);
                }

                return domainName;
            }
        }

        /// <summary>
        /// 获取启动当前线程的人的域用户名。
        /// </summary>
        public static string DomainUserName
        {
            get
            {
                string domainUserName = string.Empty;
                try
                {
                    if (!string.IsNullOrEmpty(domainUserName))
                    {
                        domainUserName = string.Format("{0}/{1}", Environment.UserDomainName, Environment.UserName);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return domainUserName;
            }
        }

        /// <summary>
        /// 获取此本地计算机的 NetBIOS 全名称。
        /// </summary>
        public static string MachineFullName
        {
            get
            {
                string fullName = string.Empty;
                try
                {
                    fullName = Dns.GetHostEntry("LocalHost").HostName;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return fullName;
            }
        }

        /// <summary>
        /// 获取此本地计算机的 NetBIOS 名称。
        /// </summary>
        public static string MachineName
        {
            get
            {
                string machineName = string.Empty;
                try
                {
                    machineName = Environment.MachineName;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return machineName;
            }
        }

        /// <summary>
        /// 获取当前计算机上的处理器数。
        /// </summary>
        public static int ProcessorCount
        {
            get { return Environment.ProcessorCount; }
        }

        #region [ Process Info ]

        /// <summary>
        /// 进程代号。
        /// </summary>
        public static string ProcessId
        {
            get
            {
                string processId = string.Empty;
                try
                {
                    new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
                    using (Process process = Process.GetCurrentProcess())
                    {
                        processId = process.Id.ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return processId;
            }
        }

        /// <summary>
        /// 进程名称。
        /// </summary>
        public static string ProcessName
        {
            get
            {
                string processName = string.Empty;
                try
                {
                    new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
                    using (Process process = Process.GetCurrentProcess())
                    {
                        processName = process.ProcessName;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return processName;
            }
        }

        #endregion [ Process Info ]

        #region [ Thread Info ]

        /// <summary>
        /// 线程代码。
        /// </summary>
        public static int ThreadId
        {
            get { return Thread.CurrentThread.ManagedThreadId; }
        }

        /// <summary>
        /// 线程名称。
        /// </summary>
        public static string ThreadName
        {
            get { return Thread.CurrentThread.Name; }
        }

        #endregion [ Thread Info ]

        /// <summary>
        ///
        /// </summary>
        public static string StackTrace
        {
            get
            {
                string stackTrace = string.Empty;
                try
                {
                    new EnvironmentPermission(PermissionState.Unrestricted).Demand();
                    stackTrace = Environment.StackTrace;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return stackTrace;
            }
        }

        /// <summary>
        /// Sustitute the Environment Variables
        /// </summary>
        /// <param name="fileName">The filename.</param> <returns></returns>
        public static string ReplaceEnvironmentVariables(string fileName)
        {
            // Check EnvironmentPermission for the ability to access the environment variables.
            try
            {
                string variables = Environment.ExpandEnvironmentVariables(fileName);

                // If an Environment Variable is not found then remove any invalid tokens
                Regex filter = new Regex("%(.*?)%", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                string filePath = filter.Replace(variables, "");

                if (Path.GetDirectoryName(filePath) == null)
                {
                    filePath = Path.GetFileName(filePath);
                }

                return filePath;
            }
            catch (SecurityException ex)
            {
                throw ex;
            }
        }
    }
}
