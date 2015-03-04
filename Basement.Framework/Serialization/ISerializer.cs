using System;
using System.Text;

namespace Basement.Framework.Serialization
{
    /// <summary>
    /// 序列化器。
    /// </summary>
    public interface ISerializer<T> where T : class, new()
    {
        #region [ ToFile ]

        /// <summary>
        /// 把对象序列化为Xml文本后，保存到Xml文件中。
        /// </summary>
        /// <typeparam name="T">要转换成Xml文本对象的类型。</typeparam> <param name="obj">要转换成Xml文本对象。</param>
        /// <param name="fileName">Xml文件名。</param>
        /// <returns>如果对象成功保存到Xml文件中返加 <b>true</b>, 反之返回 <b>false</b>。</returns>
        bool ToFile(T obj, string fileName);

        /// <summary>
        /// 把对象序列化为Xml文本后，保存到Xml文件中。
        /// </summary>
        /// <typeparam name="T">要转换成Xml文本对象的类型。</typeparam> <param name="obj">要转换成Xml文本对象。</param>
        /// <param name="fileName">Xml文件名。</param> <param name="encoding">文件编码。</param>
        /// <returns>如果对象成功保存到Xml文件中返加 <b>true</b>, 反之返回 <b>false</b>。</returns>
        bool ToFile(T obj, string fileName, Encoding encoding);

        #endregion [ ToFile ]

        #region [ ToSerializedString ]

        /// <summary>
        /// 将对象序列化为UTF-8格式的文本。
        /// </summary>
        /// <param name="obj">要序列化为文本的对象。</param>
        /// <returns>如果
        /// <paramref name="obj"/>是空引用，返回 <seealso cref="String.Empty"/>, 反之返回序列化文本。</returns>
        string ToSerializedString(T obj);

        /// <summary>
        /// 将对象序列化为指定编码格式的文本。
        /// </summary>
        /// <param name="obj">要序列化为文本的对象。</param>
        /// <param name="encoding">文本编码格式。如果为空引用则默认为UTF-8编码格式。</param>
        /// <returns>如果
        /// <paramref name="obj"/>是空引用，返回 <seealso cref="String.Empty"/>, 反之返回序列化文本。</returns>
        string ToSerializedString(T obj, Encoding encoding);

        /// <summary>
        /// 将对象序列化为指定编码格式的文本
        /// </summary>
        /// <typeparam name="T"></typeparam> <param name="obj">要序列化为文本的对象</param>
        /// <param name="encoding">文本编码格式。如果为空引用则默认为UTF-8编码格式</param>
        /// <param name="extraTypes">要序列化的其他对象类型的 Type 数组。</param>
        /// <param name="xmlNamespace">是否需要Root节点默认名空间</param>
        /// <param name="xmlHead">是否需要&lt;?xml version="1.0" encoding="utf-8"?&gt;</param>
        /// <returns></returns>
        string ToSerializedString(T obj, Encoding encoding, bool xmlNamespace, bool xmlHead);

        #endregion [ ToSerializedString ]

        #region [ ToBinary ]

        /// <summary>
        /// 把对象转换成二进制数据。
        /// </summary>
        /// <typeparam name="T">要转换成二进制数据对象的类型。</typeparam> <param name="obj">要转换成二进制数据的对象。</param>
        /// <returns>二进制数据。</returns>
        byte[] ToBinary(T obj);

        /// <summary>
        /// 把对象转换成二进制数据。
        /// </summary>
        /// <typeparam name="T">要转换成二进制数据对象的类型。</typeparam> <param name="obj">要转换成二进制数据的对象。</param>
        /// <param name="encoding">二进制数据编码格式。</param> <returns>二进制数据。</returns>
        byte[] ToBinary(T obj, Encoding encoding);

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
        T FromFile(string fileName);

        /// <summary>
        /// 把xml文件反序列化成对象。
        /// </summary>
        /// <typeparam name="T">要转换成的对象类型。</typeparam> <param name="fileName">xml文件。</param>
        /// <param name="encoding">文件编码格式。</param> <returns>对象。</returns>
        T FromFile(string fileName, Encoding encoding);

        #endregion [ FromFile ]

        #region [ FromSerializedString ]

        /// <summary>
        /// 把文本反序列化成对象。
        /// </summary>
        /// <typeparam name="T">要转换成的对象类型。</typeparam> <param name="serializedString">序列化的文本。</param>
        /// <returns>对象。</returns>
        /// <remarks>
        /// 默认编码为UTF8。
        /// </remarks>
        T FromSerializedString(string serializedString);

        /// <summary>
        /// 把文本反序列化成对象。
        /// </summary>
        /// <typeparam name="T">要转换成的对象类型。</typeparam> <param name="serializedString">序列化的文本。</param>
        /// <param name="encoding">xml文本编码。</param> <returns>对象。</returns>
        T FromSerializedString(string serializedString, Encoding encoding);

        #endregion [ FromSerializedString ]

        #region [ FromBinary ]

        /// <summary>
        /// 把二进制数据反序列化成对象。
        /// </summary>
        /// <typeparam name="T">要转换成的对象类型。</typeparam> <param name="bytes">二进制数据</param>
        /// <param name="encoding">编码。</param> <returns>对象。</returns>
        T FromBinary(byte[] bytes, Encoding encoding);

        /// <summary>
        /// 把二进制数据反序列化成对象。
        /// </summary>
        /// <param name="type">要转换成的对象类型。</param> <param name="bytes">二进制数据</param>
        /// <returns>对象。</returns>
        T FromBinary(byte[] bytes);

        #endregion [ FromBinary ]
    }
}