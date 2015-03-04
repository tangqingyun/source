using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Basement.Framework.Configuration
{
    [Serializable]
    [XmlRoot("app")]
    public class MonitoringItem
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("interval")]
        public string Interval { get; set; }

        [XmlAttribute("mail")]
        public string Mail { get; set; }
    }
}
