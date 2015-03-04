using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.Logging
{
    public interface ILogger
    {
        void Log(EnumLogLevel level, object obj, string module = null, Exception ex = null);
    }
}
