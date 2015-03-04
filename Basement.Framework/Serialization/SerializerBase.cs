using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Basement.Framework.Serialization
{
    /// <summary>
    /// Xml序列化基类。
    /// </summary>
    public class SerializerBase<T> : ISerializer<T> where T : class , new()
    {
        #region [ ToFile ]

        /// <summary>
        /// 把对象序列化为Xml文本后，保存到Xml文件中。
        /// </summary>
        /// <typeparam name="T">要转换成Xml文本对象的类型。</typeparam> <param name="obj">要转换成Xml文本对象。</param>
        /// <param name="fileName">Xml文件名。</param>
        /// <returns>如果对象成功保存到Xml文件中返加 <b>true</b>, 反之返回 <b>false</b>。</returns>
        public bool ToFile(T obj, string fileName) {
            return ToFile(obj, fileName, Encoding.UTF8);
        }

        /// <summary>
        /// 把对象序列化为Xml文本后，保存到Xml文件中。
        /// </summary>
        /// <typeparam name="T">要转换成Xml文本对象的类型。</typeparam> <param name="obj">要转换成Xml文本对象。</param>
        /// <param name="fileName">Xml文件名。</param> <param name="encoding">文件编码。</param>
        /// <param name="extraTypes">要序列化的其他对象类型的 Type 数组。</param>
        /// <returns>如果对象成功保存到Xml文件中返加 <b>true</b>, 反之返回 <b>false</b>。</returns>
        public virtual bool ToFile(T obj, string fileName, Encoding encoding) {
            bool saved = true;

            try {
                string serializedString = ToSerializedString(obj, encoding);
                if (!string.IsNullOrEmpty(serializedString))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(serializedString);
                    doc.Save(fileName);
                }

                saved = true;
            }
            catch (Exception ex) {
                saved = false;
                //Logger.Error(ex.ToString());
            }

            return saved;
        }

        #endregion [ ToFile ]

        #region [ ToSerializedString ]

        /// <summary>
        /// 将对象序列化为UTF-8格式的文本。
        /// </summary>
        /// <param name="obj">要序列化为文本的对象。</param>
        /// <returns>如果
        /// <paramref name="obj"/>是空引用，返回 <seealso cref="String.Empty"/>, 反之返回序列化文本。</returns>
        public string ToSerializedString(T obj) {
            return ToSerializedString(obj, Encoding.UTF8);
        }

        /// <summary>
        /// 将对象序列化为指定编码格式的文本。
        /// </summary>
        /// <param name="obj">要序列化为文本的对象。</param>
        /// <param name="encoding">文本编码格式。如果为空引用则默认为UTF-8编码格式。</param>
        /// <param name="extraTypes">要序列化的其他对象类型的 Type 数组。</param>
        /// <returns>如果
        /// <paramref name="obj"/>是空引用，返回 <seealso cref="String.Empty"/>, 反之返回序列化文本。</returns>
        public virtual string ToSerializedString(T obj, Encoding encoding) {
            //byte[] bytes = ToBinary(obj, encoding);
            ////处理掉第一位的不可见字符
            //string xml = encoding.GetString(bytes).TrimStart();
            //return xml.Substring(xml.IndexOf('<'));

            throw new NotImplementedException();
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
        public virtual string ToSerializedString(T obj, Encoding encoding, bool xmlNamespace, bool xmlHead) {
            throw new NotImplementedException();
        }

        #endregion [ ToSerializedString ]

        #region [ ToBinary ]

        /// <summary>
        /// 把对象转换成二进制数据。
        /// </summary>
        /// <param name="obj">要转换成二进制数据的对象。</param> <returns>二进制数据。</returns>
        public byte[] ToBinary(T obj) {
            return ToBinary(obj, Encoding.UTF8);
        }

        /// <summary>
        /// 把对象转换成二进制数据。
        /// </summary>
        /// <typeparam name="T">要转换成二进制数据对象的类型。</typeparam> <param name="obj">要转换成二进制数据的对象。</param>
        /// <param name="encoding">二进制数据编码格式。</param>
        /// <param name="extraTypes">要序列化的其他对象类型的 Type 数组。</param> <returns>二进制数据。</returns>
        public virtual byte[] ToBinary(T obj, Encoding encoding) {
            throw new NotImplementedException();
        }

        #endregion [ ToBinary ]

        #region [ FromFile ]

        /// <summary>
        /// 把xml文件反序列化成对象。
        /// </summary>
        /// <typeparam name="T">要转换成的对象类型。</typeparam> <param name="fileName">xml文件。</param>
        /// <returns>对象。</returns>
        /// <remarks>
        /// 默认编码为UTF8。
        /// </remarks>
        public T FromFile(string fileName) {
            return FromFile(fileName, Encoding.UTF8);
        }

        /// <summary>
        /// 把xml文件反序列化成对象。
        /// </summary>
        /// <typeparam name="T">要转换成的对象类型。</typeparam> <param name="fileName">xml文件。</param>
        /// <param name="encoding">文件编码格式。</param>
        /// <param name="extraTypes">要序列化的其他对象类型的 Type 数组。</param> <returns>对象。</returns>
        public T FromFile(string fileName, Encoding encoding) {
            T ret = default(T);
            FileStream fs = null;
            BinaryReader r = null;
            try {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                byte[] bytes = new byte[0];
                r = new BinaryReader(fs, encoding);
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                bytes = r.ReadBytes((int)r.BaseStream.Length);
                ret = (T)FromBinary(bytes, encoding);
            }
            catch (System.Exception ex) {
                //Logger.Error(ex.ToString());
                throw ex;
            }
            finally {
                if (fs != null) {
                    fs.Dispose();
                    fs = null;
                }
                if (r != null) {
                    r.Dispose();
                    r = null;
                }
            }

            return ret;
        }

        #endregion [ FromFile ]

        #region [ FromSerializedString ]

        /// <summary>
        /// 把xml文本反序列化成对象。
        /// </summary>
        /// <typeparam name="T">要转换成的对象类型。</typeparam> <param name="xml">xml文本</param>
        /// <returns>对象。</returns>
        public T FromSerializedString(string serializedString) {
            return FromSerializedString(serializedString, Encoding.UTF8);
        }

        /// <summary>
        /// 把文本反序列化成对象。
        /// </summary>
        /// <typeparam name="T">要转换成的对象类型。</typeparam> <param name="serializedString">序列化的文本。</param>
        /// <param name="encoding">xml文本编码。</param>
        /// <param name="extraTypes">要序列化的其他对象类型的 Type 数组。</param> <returns>对象。</returns>
        public virtual T FromSerializedString(string serializedString, Encoding encoding) {
            T ret = default(T);
            if (string.IsNullOrEmpty(serializedString))
            {
                return ret;
            }

            byte[] bytes = encoding.GetBytes(serializedString);

            return FromBinary(bytes, encoding);
        }

        #endregion [ FromSerializedString ]

        #region [ FromBinary ]

        /// <summary>
        /// 把二进制数据反序列化成对象。
        /// </summary>
        /// <typeparam name="T">要转换成的对象类型。</typeparam> <param name="bytes">二进制数据</param>
        /// <param name="extraTypes">要序列化的其他对象类型的 Type 数组。</param> <returns>对象。</returns>
        public T FromBinary(byte[] bytes) {
            return FromBinary(bytes, Encoding.UTF8);
        }

        /// <summary>
        /// 把二进制数据反序列化成对象。
        /// </summary>
        /// <param name="type">要转换成的对象类型。</param> <param name="bytes">二进制数据</param>
        /// <param name="encoding">编码。</param>
        /// <param name="extraTypes">要序列化的其他对象类型的 Type 数组。</param> <returns>对象。</returns>
        public virtual T FromBinary(byte[] bytes, Encoding encoding) {
            throw new NotImplementedException();
        }

        #endregion [ FromBinary ]
    }
}