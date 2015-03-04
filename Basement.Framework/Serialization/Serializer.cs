using System;

namespace Basement.Framework.Serialization
{
    public class Serializer<T> where T : class, new()
    {
        /// <summary>
        /// 二进制序列化。
        /// </summary>
        public static ISerializer<T> BinaryFormatter {
            get { return GetSerializer(SerializationMode.BinaryFormatter); }
        }

        /// <summary>
        /// Xml 序列化。
        /// </summary>
        public static ISerializer<T> XmlSerializer {
            get { return GetSerializer(SerializationMode.XmlSerializer); }
        }

        /// <summary>
        /// 获取序列化器。
        /// </summary>
        /// <param name="mode">序列化器的模式。</param> <returns>序列化器。</returns>
        public static ISerializer<T> GetSerializer(SerializationMode mode) {
            ISerializer<T> serializer = null;
            switch (mode) {
                case SerializationMode.XmlSerializer:
                    serializer = XmlSerializerWrapper<T>.GetInstance();
                    break;
                case SerializationMode.BinaryFormatter:
                    serializer = BinaryFormatterWrapper<T>.GetInstance();
                    break;

                default:
                    throw new NotSupportedException();
            }

            return serializer;
        }
    }
}