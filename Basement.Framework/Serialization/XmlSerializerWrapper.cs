using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Basement.Framework.Serialization
{
    /// <summary>
    /// Xml序列化。
    /// </summary>
    public class XmlSerializerWrapper<T> : SerializerBase<T> where T : class , new()
    {
        #region [ Fields ]

        private static ISerializer<T> instance = null;
        private static ISerializer<T> instanceXsr = null;

        private static object lockObj = new object();
        private XmlSerializer serializer = null;
        #endregion [ Fields ]

        #region [ Methods ]

        public XmlSerializerWrapper(XmlSerializer xsr) {
            serializer = xsr;
        }

        /// <summary>
        /// 当前实例。
        /// </summary>
        public static ISerializer<T> GetInstance() {
            if (instance == null) {
                lock (lockObj) {
                    if (instance == null) {
                        instance = new XmlSerializerWrapper<T>(new XmlSerializer(typeof(T), string.Empty));
                        //instance = new XmlSerializerWrapper<T>(new XmlSerializer(typeof(T), new Type[] { }));
                    }
                }
            }
            return instance;
        }

        public static ISerializer<T> GetInstance(XmlSerializer xsr) {
            if (instanceXsr == null) {
                lock (lockObj) {
                    if (instanceXsr == null) {
                        instanceXsr = new XmlSerializerWrapper<T>(xsr);
                    }
                }
            }
            return instanceXsr;
        }

        /// <summary>
        /// 把二进制数据反序列化成对象。
        /// </summary>
        /// <param name="type">要转换成的对象类型。</param> <param name="bytes">二进制数据</param>
        /// <param name="encoding">编码。</param>
        /// <param name="extraTypes">要序列化的其他对象类型的 Type 数组。</param> <returns>对象。</returns>
        public override T FromBinary(byte[] bytes, Encoding encoding) {
            T ret = default(T);
            if (bytes == null || bytes.Length <= 0) {
                return ret;
            }

            using (MemoryStream stream = new MemoryStream(bytes)) {
                //XmlSerializer serializer = new XmlSerializer(type);
                ret = (T)serializer.Deserialize(stream);
            }

            return ret;
        }

        /// <summary>
        /// 把对象转换成二进制数据。
        /// </summary>
        /// <typeparam name="T">要转换成二进制数据对象的类型。</typeparam> <param name="obj">要转换成二进制数据的对象。</param>
        /// <param name="encoding">二进制数据编码格式。</param>
        /// <param name="extraTypes">要序列化的其他对象类型的 Type 数组。</param> <returns>二进制数据。</returns>
        public override byte[] ToBinary(T obj, Encoding encoding) {
            byte[] bytes = null;

            if (obj != null) {
                MemoryStream ms = null;
                XmlTextWriter xtw = null;
                try {
                    ms = new MemoryStream();
                    xtw = new XmlTextWriter(ms, encoding);
                    serializer.Serialize(xtw, obj);
                    ms.Position = 0;
                    bytes = ms.ToArray();
                }
                catch (System.Exception ex) {
                    //Logger.Error(ex.ToString());
                    throw ex;
                }
                finally {
                    if (xtw != null) {
                        xtw.Close();
                        xtw = null;
                    }
                    if (ms != null) {
                        ms.Dispose();
                        ms = null;
                    }
                }
            }
            else {
                bytes = new byte[] { };
            }

            return bytes;
        }

        /// <summary>
        /// 将对象序列化为指定编码格式的文本。
        /// </summary>
        /// <param name="obj">要序列化为文本的对象。</param>
        /// <param name="encoding">文本编码格式。如果为空引用则默认为UTF-8编码格式。</param>
        /// <param name="extraTypes">要序列化的其他对象类型的 Type 数组。</param>
        /// <returns>如果
        /// <paramref name="obj"/>是空引用，返回 <seealso cref="String.Empty"/>, 反之返回序列化文本。</returns>
        public override string ToSerializedString(T obj, Encoding encoding) {
            string ret = string.Empty;

            //XmlSerializer serializer = new XmlSerializer(typeof(T));
            MemoryStream ms = null;
            XmlTextWriter xtw = null;
            StreamReader sr = null;
            try {
                ms = new MemoryStream();
                xtw = new System.Xml.XmlTextWriter(ms, encoding);

                //xtw.Formatting = System.Xml.Formatting.Indented;
                serializer.Serialize(xtw, obj);

                ms.Seek(0, SeekOrigin.Begin);
                sr = new StreamReader(ms);
                ret = sr.ReadToEnd();
            }
            catch (Exception ex) {
                //Logger.Error(ex.ToString());
                throw ex;
            }
            finally {
                if (xtw != null) {
                    xtw.Close();
                    xtw = null;
                }
                if (sr != null) {
                    sr.Dispose();
                    sr = null;
                }
                if (ms != null) {
                    ms.Dispose();
                    ms = null;
                }
            }

            return ret;
        }

        /// <summary>
        /// 将对象序列化为指定编码格式的文本
        /// </summary>
        /// <typeparam name="T"></typeparam> <param name="obj">要序列化为文本的对象</param>
        /// <param name="encoding">文本编码格式。如果为空引用则默认为UTF-8编码格式</param>
        /// <param name="extraTypes">要序列化的其他对象类型的 Type 数组。</param>
        /// <param name="xmlNamespace">是否需要Root节点默认名空间</param>
        /// <param name="xmlHead">是否需要&lt;?xml version="1.0" encoding="utf-8"?&gt;</param>
        /// <returns></returns>
        public override string ToSerializedString(T obj, Encoding encoding, bool xmlNamespace, bool xmlHead) {
            string ret = string.Empty;

            //XmlSerializer serializer = new XmlSerializer(typeof(T));
            MemoryStream ms = null;
            XmlTextWriter xtw = null;
            StreamReader sr = null;
            XmlWriter xwrt = null;
            try {
                //xtw.Formatting = System.Xml.Formatting.Indented;
                ms = new MemoryStream();

                if (xmlNamespace) {
                    if (xmlHead) {
                        xtw = new System.Xml.XmlTextWriter(ms, encoding);
                        serializer.Serialize(xtw, obj);
                    }
                    else {
                        XmlWriterSettings settings = new XmlWriterSettings();
                        // Remove the <?xml version="1.0" encoding="utf-8"?>
                        settings.OmitXmlDeclaration = true;
                        xwrt = XmlWriter.Create(ms, settings);
                        serializer.Serialize(xwrt, obj);
                    }
                }
                else {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "");
                    if (xmlHead) {
                        xtw = new System.Xml.XmlTextWriter(ms, encoding);
                        serializer.Serialize(xtw, obj, ns);
                    }
                    else {
                        XmlWriterSettings settings = new XmlWriterSettings();
                        // Remove the <?xml version="1.0" encoding="utf-8"?>
                        settings.OmitXmlDeclaration = true;
                        xwrt = XmlWriter.Create(ms, settings);
                        serializer.Serialize(xwrt, obj, ns);
                    }
                }

                ms.Seek(0, SeekOrigin.Begin);
                sr = new StreamReader(ms);
                ret = sr.ReadToEnd();
            }
            catch (Exception ex) {
                //Logger.Error(ex.ToString());
                throw ex;
            }
            finally {
                if (xtw != null) {
                    xtw.Close();
                    xtw = null;
                }
                if (sr != null) {
                    sr.Dispose();
                    sr = null;
                }
                if (ms != null) {
                    ms.Dispose();
                    ms = null;
                }
            }

            return ret;
        }

        #endregion [ Methods ]
    }
}