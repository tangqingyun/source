using System;
using System.Collections.Generic;
using System.Text;

namespace Basement.Framework.Utility
{
    /// <summary>
    /// Base64 的摘要说明。
    /// </summary>
    public class Base64
    {
        public static string Encode(string Value, string Charset)
        {
            if (Charset == String.Empty)
            {
                Charset = String.Empty;
            }

            byte[] barray;
            barray = System.Text.Encoding.GetEncoding(Charset).GetBytes(Value);
            return Convert.ToBase64String(barray);
        }

        public static string Decode(string Value, string Charset)
        {
            if (Charset == String.Empty)
            {
                Charset = String.Empty;
            }

            byte[] barray;
            barray = System.Convert.FromBase64String(Value);
            return System.Text.Encoding.GetEncoding(Charset).GetString(barray);
        }


    }
}
