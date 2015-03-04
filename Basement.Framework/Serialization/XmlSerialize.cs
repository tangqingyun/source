using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Basement.Framework.Serialization
{
    public class XmlSerialize
    {
        private static XmlSerializerNamespaces _namespaces = null;
        private static XmlSerializerNamespaces namespaces
        {
            get
            {
                if (_namespaces == null)
                {
                    _namespaces = new XmlSerializerNamespaces();
                    _namespaces.Add("", "");
                }
                return _namespaces;
            }
        }
        
        public static T DeserializeXml<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public static T DeserializeXmlFile<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            string xml = File.ReadAllText(filePath);
            using (StringReader reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public static string SerializeXml<T>(T model)
        {
            using (StringWriter writer = new StringWriter())
            {
                new XmlSerializer(typeof(T)).Serialize((TextWriter)writer, model);
                return writer.ToString();
            }
        }
      
        public static void SerializeXmlToFile<T>(T model, string filePath)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            System.IO.MemoryStream mem = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mem, Encoding.UTF8);
            ser.Serialize(writer, model, namespaces);
            writer.Close();
            File.WriteAllText(filePath, Encoding.UTF8.GetString(mem.ToArray()));
        }

        //public static void SerializeXmlToFile<T>(T model, string filePath)
        //{
        //    using (TextWriter writer = new StringWriter())
        //    {
        //        new XmlSerializer(typeof(T)).Serialize((TextWriter)writer, model, namespaces);
        //        new XmlSerializer(typeof(T)).Serialize(File.Create("C:\\Filename.xml"), model, namespaces);
        //    }
        //}
    }  
}
