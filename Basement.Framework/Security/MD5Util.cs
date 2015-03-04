using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Basement.Framework.Security
{
    public static class MD5Util
    {
        public static string GetMD5Hash(byte[] input)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(input);
            return HexUtil.GetString(data);
        }
        public static string GetMD5Hash(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            return GetMD5Hash(inputBytes);
        }
    }
}
