using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Basement.Framework.Configuration
{
    [Serializable]
    [XmlRoot("monitoring")]
    public class Monitoring
    {
        [XmlElement("app")]
        public List<MonitoringItem> Items { get; set; }

        [XmlAttribute("subject")]
        public string Subject { get; set; }
    }
}
