using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basement.Framework.Data.Configuration;
//using Basement.Framework.Queue.Configuration;
//using Basement.Framework.Redis.Configuration;
//using Basement.Framework.Logging.Configuration;
using Basement.Framework.Serialization;
using Basement.Framework.Utility;
//using Basement.Framework.Mongo.Configuration;
//using Basement.Framework.Utility;
//using Basement.Framework.Serialization;
using Basement.Framework.Common;
using MongoDB.Driver;

namespace Basement.Framework.Configuration
{
    public static class FrameworkConfig
    {
        static FrameworkConfig()
        {
            Initialize();
        }
        public static void Initialize()
        {
            try
            {
                Framework framework = XmlSerialize.DeserializeXmlFile<Framework>(Path.Combine(SysBaseHandle.BIN_DIR, "Configs\\Framework.config"));
                Singleton<Framework>.SetInstance(framework);
            }
            catch
            {
                throw new Exception("/Configs/Framework.config未找到，或文件读取失败。");
            }
            
        }

        //public static List<RedisServer> RedisServers
        //{
        //    get
        //    {
        //        return Singleton<Framework>.GetInstance().RedisSetting.RedisServers;
        //    }
        //}

        //public static RedisServer GetRedisServer(string name)
        //{
        //    return Singleton<Framework>.GetInstance().RedisSetting.RedisServers.FirstOrDefault(s => s.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        //}

        //public static List<MongoServer> MongoServers
        //{
        //    get
        //    {
        //        return Singleton<Framework>.GetInstance().MongoSetting.MongoServers;
        //    }
        //}

        public static MongoServer GetMongoServer(string name)
        {
          //  return Singleton<Framework>.GetInstance().MongoSetting.MongoServers.FirstOrDefault(s => s.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            return null;
        }

        private static List<AppSettingItem> AppSettings
        {
            get
            {
                return Singleton<Framework>.GetInstance().AppSetting.Items;
            }
        }

        public static string GetAppSetting(string name)
        {
            string value = null;
            AppSettingItem item = Singleton<Framework>.GetInstance().AppSetting.Items.FirstOrDefault(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (item != null) value = item.Value;
            return value;
        }

        private static Monitoring Monitoring
        {
            get
            {
                return Singleton<Framework>.GetInstance().Monitoring;
            }
        }

        public static MonitoringItem GetMonitoringItem(string name)
        {
            MonitoringItem item = Singleton<Framework>.GetInstance().Monitoring.Items.FirstOrDefault(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            return item;
        }

        //public static List<ServerChannel> QueueServerChannels
        //{
        //    get
        //    {
        //        return Singleton<Framework>.GetInstance().MessageEngine.ServerChannels;
        //    }
        //}

        //public static List<QueueChanel> QueueChannels
        //{
        //    get
        //    {
        //        return Singleton<Framework>.GetInstance().MessageEngine.QueueChanels;
        //    }
        //}

        //public static QueueChanel GetQueueChannel(string name)
        //{
        //    return Singleton<Framework>.GetInstance().MessageEngine.QueueChanels.FirstOrDefault(s => s.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        //}

        //public static ServerChannel GetQueueServerChannel(string name)
        //{
        //    return Singleton<Framework>.GetInstance().MessageEngine.ServerChannels.FirstOrDefault(s => s.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        //}

        //public static Databases Connections
        //{
        //    get
        //    {
        //        return Singleton<Framework>.GetInstance().Databases;
        //    }
        //}

        //public static string GetConnectionString(string name)
        //{
        //    return Singleton<Framework>.GetInstance().Databases[name];
        //}

        //public static Uni2uni.Framework.Logging.Configuration.Logging Logging
        //{
        //    get
        //    {
        //        return Singleton<Framework>.GetInstance().Logging;
        //    }
        //}

        //public static LogItem GetLogging(string name)
        //{
        //    LogItem item = Logging.LogItems.FirstOrDefault(s => s.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        //    return item;
        //}
    }
}
