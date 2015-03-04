using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MongoDB.Driver;

namespace Basement.Framework.Configuration
{
    [Serializable]
    [XmlRoot("mongoSetting")]
    public class MongoSetting
    {
        [XmlElement("mongoServer")]
        public List<MongoServer> MongoServers { get; set; }
    }
}
