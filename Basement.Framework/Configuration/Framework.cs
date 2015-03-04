using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Basement.Framework.Data.Configuration;
//using Basement.Framework.Mongo.Configuration;
//using Basement.Framework.Queue.Configuration;
//using Basement.Framework.Redis.Configuration;

namespace Basement.Framework.Configuration
{
    [Serializable]
    [XmlRoot("framework")]
    public class Framework
    {
        //[XmlElement("redisSetting")]
        //public RedisSetting RedisSetting { get; set; }

        //[XmlElement("mongoSetting")]
        //public MongoSetting MongoSetting { get; set; }

        [XmlElement("appSetting")]
        public AppSetting AppSetting { get; set; }

        [XmlElement("monitoring")]
        public Monitoring Monitoring { get; set; }

        //[XmlElement("messageEngine")]
        //public MessageEngine MessageEngine { get; set; }

        //[XmlElement("databases")]
        //public Databases Databases { get; set; }

        //[XmlElement("logging")]
        //public Uni2uni.Framework.Logging.Configuration.Logging Logging { get; set; }
    }
}
