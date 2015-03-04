using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace Basement.Framework.Data.Configuration
{
    [Serializable]
    [XmlRoot("databases")]
    public class Databases
    {
        [XmlElement("connection")]
        public List<Connection> Connections { get; set; }

        private IDictionary<string, Connection> _dictConnections = null;
        public string this[string name]
        {
            get
            {
                string index = name.ToLower();
                if (_dictConnections == null) _dictConnections = Connections.ToDictionary<Connection, string>(s => s.Name.ToLower());
                if (_dictConnections.Keys.Contains(index)) return _dictConnections[index].ConnectionString;
                throw new NullReferenceException(string.Format("[{1}] not found.", name));
            }
        }
    }
}
