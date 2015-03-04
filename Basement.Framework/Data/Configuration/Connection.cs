using System;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Data.Common;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Basement.Framework.Data.Configuration
{
    [Serializable]
    [XmlRoot("connection")]
    public class Connection //:ICloneable
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        [XmlAttribute("retry")]
        public int Retry { get; set; }

        public string this[string name]
        {
            get
            {
                return this.ConnectionString;
            }
        }
     
        //public Object Clone()
        //{
        //    MemoryStream _stream = new MemoryStream();
        //    BinaryFormatter _formatter = new BinaryFormatter();
        //    _formatter.Serialize(_stream, this);
        //    _stream.Position = 0;
        //    return _formatter.Deserialize(_stream);
        //}
    }
}
