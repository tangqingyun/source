using Basement.Framework.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Basement.Framework.Logging.LogProvider
{
    public class LogTextProvider : ILogProvider
    {
        private object lockobj = new object();
        public void Log(EnumLogLevel level, object message, string module = null, Exception ex = null)
        {
            string dir = SysBaseHandle.ROOT_DIR + "/Log/";
            if (!string.IsNullOrWhiteSpace(module))
            {
                dir = dir + module + "/";
            }
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            lock (lockobj)
            {
                var fs = new FileStream(dir + DateTime.Now.ToString("yyyyMMdd") + "_log.txt",
                    FileMode.OpenOrCreate, FileAccess.Write);
                using (var mStreamWriter = new StreamWriter(fs))
                {
                    mStreamWriter.BaseStream.Seek(0, SeekOrigin.End);
                    StringBuilder sb = new StringBuilder();
                    sb.Append(message ==null? string.Empty : message.ToString());
                    mStreamWriter.WriteLine(sb.ToString());
                }
                fs.Close();
            }
        }

    }
}
