using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Basement.Framework.Configuration
{
    [Serializable]
    [XmlRoot("appSetting")]
    public class AppSetting
    {
        [XmlElement("add")]
        public List<AppSettingItem> Items { get; set; }
    }
}
