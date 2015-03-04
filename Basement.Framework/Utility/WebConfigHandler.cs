using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Basement.Framework.Utility
{
    public class WebConfigurationHandler : IConfigurationSectionHandler
    {
        public WebConfigurationHandler()
        { 
        }

        #region IConfigurationSectionHandler 成员

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            NameValueCollection configs;
            NameValueSectionHandler baseHandler = new NameValueSectionHandler();
            configs = (NameValueCollection)baseHandler.Create(parent, configContext, section);
            return configs;
        }
        #endregion
    }
}
