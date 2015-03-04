using System;
using System.Collections.Generic;
using System.Text;

namespace Basement.Framework.Utility
{
    public class SimpleLog
    {
        private static object logSyncobject = new object();
        public static void WriteLog(string strModelName, string strFunctionName, string strErrorMessage)
        {
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\log\\" + DateTime.Now.ToString("yyyyMMdd");
            System.IO.Directory.CreateDirectory(strPath);
            string strFile = strPath + "\\" + strModelName + ".log";
            string strMess = "LogTime:\t" + DateTime.Now.ToString() + "\r\n" + "ModelName:\t" + strModelName + "\r\nFunctionName:\t" +
                strFunctionName + "\r\nLogMessage:\t" + strErrorMessage + "\r\n";
            lock (logSyncobject)
            {
                //uni2uni.com.Framework.Common.FileHelper.WriteText(strFile, strMess, true, Encoding.UTF8);
            }
        }
    }
}
