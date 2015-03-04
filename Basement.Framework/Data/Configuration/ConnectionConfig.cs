using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Basement.Framework.Utility;
using Basement.Framework.Serialization;
using Basement.Framework.Common;

namespace Basement.Framework.Data.Configuration
{
    public static class ConnectionConfig
    {
        static ConnectionConfig()
        {
            Initialize();
        }

        public static void Initialize()
        {
            try
            {
                Databases db = XmlSerialize.DeserializeXmlFile<Databases>(Path.Combine(SysBaseHandle.BIN_DIR, "Configs\\Database.config"));
                Singleton<Databases>.SetInstance(db);
            }
            catch
            {
                throw new Exception("/Configs/Database.config未找到，或文件读取失败。");
            }
           
        }

        public static Databases Connections
        {
            get
            {
                return Singleton<Databases>.GetInstance();
            }
        }

    }
}
