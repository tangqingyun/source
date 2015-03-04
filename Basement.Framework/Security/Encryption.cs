using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basement.Framework.Security
{
    /// <summary>加密/解密类
    /// </summary>
    public static class Encryption
    {
        #region MD5,SHA1,SHA256,SHA384,SHA512,RIPEMD160 加密

        /// <summary>算法名称枚举
        /// </summary>
        public enum HashType : int
        {
            MD5,
            SHA1,
            SHA256,
            SHA384,
            SHA512,
            RIPEMD160
        }

        /// <summary>算法名称字典
        /// </summary>
        private static Dictionary<string, HashType> DictHashType = null;

        /// <summary>构造方法,初始化算法名称字典
        /// </summary>
        static Encryption()
        {
            DictHashType = new Dictionary<string, HashType>();
            DictHashType.Add("MD5", HashType.MD5);
            DictHashType.Add("SHA1", HashType.SHA1);
            DictHashType.Add("SHA256", HashType.SHA256);
            DictHashType.Add("SHA384", HashType.SHA384);
            DictHashType.Add("SHA512", HashType.SHA512);
            DictHashType.Add("RIPEMD160", HashType.RIPEMD160);
        }

        /// <summary>密码加密,默认SHA1
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        public static string FormatPassword(string password)
        {
            return Encrypt(password, HashType.SHA1);
        }

        /// <summary>加密,默认SHA1
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        public static string Encrypt(string @string)
        {
            return Encrypt(@string, HashType.SHA1);
        }

        /// <summary>加密
        /// </summary>
        /// <param name="string">字符串</param>
        /// <param name="hashtype">加密算法名称</param>
        /// <returns>加密后字符串</returns>
        public static string Encrypt(string @string, string hashtype)
        {
            if (!string.IsNullOrEmpty(@string) && !string.IsNullOrEmpty(hashtype) && DictHashType.Keys.Contains(hashtype.ToUpper()))
            {
                return Encrypt(@string, DictHashType[hashtype]);
            }
            return string.Empty;
        }

        /// <summary>加密
        /// </summary>
        /// <param name="string">字符串</param>
        /// <param name="hashtype">加密算法名称</param>
        /// <returns>加密后字符串</returns>
        public static string Encrypt(string @string, HashType hashtype)
        {
            string output = String.Empty;

            if (!string.IsNullOrEmpty(@string))
            {

                Byte[] clearBytes;
                Byte[] hashedBytes;

                switch (hashtype)
                {
                    case HashType.RIPEMD160:
                        clearBytes = new UTF8Encoding().GetBytes(@string);
                        System.Security.Cryptography.RIPEMD160 myRIPEMD160 = System.Security.Cryptography.RIPEMD160Managed.Create();
                        hashedBytes = myRIPEMD160.ComputeHash(clearBytes);
                        output = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                        break;
                    case HashType.MD5:
                        clearBytes = new UTF8Encoding().GetBytes(@string);
                        hashedBytes = ((System.Security.Cryptography.HashAlgorithm)System.Security.Cryptography.CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
                        output = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                        break;
                    case HashType.SHA1:
                        clearBytes = Encoding.UTF8.GetBytes(@string);
                        System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
                        sha1.ComputeHash(clearBytes);
                        hashedBytes = sha1.Hash;
                        sha1.Clear();
                        output = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                        break;
                    case HashType.SHA256:
                        clearBytes = Encoding.UTF8.GetBytes(@string);
                        System.Security.Cryptography.SHA256 sha256 = new System.Security.Cryptography.SHA256Managed();
                        sha256.ComputeHash(clearBytes);
                        hashedBytes = sha256.Hash;
                        sha256.Clear();
                        output = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                        break;
                    case HashType.SHA384:
                        clearBytes = Encoding.UTF8.GetBytes(@string);
                        System.Security.Cryptography.SHA384 sha384 = new System.Security.Cryptography.SHA384Managed();
                        sha384.ComputeHash(clearBytes);
                        hashedBytes = sha384.Hash;
                        sha384.Clear();
                        output = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                        break;
                    case HashType.SHA512:
                        clearBytes = Encoding.UTF8.GetBytes(@string);
                        System.Security.Cryptography.SHA512 sha512 = new System.Security.Cryptography.SHA512Managed();
                        sha512.ComputeHash(clearBytes);
                        hashedBytes = sha512.Hash;
                        sha512.Clear();
                        output = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                        break;
                }
            }
            return output;
        }

        #endregion 不可逆加密

        #region 加密解密

        /// <summary>根据KEy加密字符串
        /// </summary>
        /// <param name="string"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReversibleEncrypt(string @string, string key)
        {
            byte[] buff = System.Text.Encoding.Default.GetBytes(@string);
            byte[] kb = System.Text.Encoding.Default.GetBytes(key);
            return Convert.ToBase64String(Encrypt(buff, kb));
        }

        /// <summary>根据KEY解密字符串
        /// </summary>
        /// <param name="original"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReversibleDecrypt(string encrypeString, string key)
        {
            return Decrypt(encrypeString, key, System.Text.Encoding.Default);
        }

        private static string Decrypt(string encrypted, string key, Encoding encoding)
        {
            byte[] buff = Convert.FromBase64String(encrypted);
            byte[] kb = System.Text.Encoding.Default.GetBytes(key);
            return encoding.GetString(Decrypt(buff, kb));
        }

        private static byte[] MakeMD5(byte[] original)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider hashmd5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] keyhash = hashmd5.ComputeHash(original);
            hashmd5 = null;
            return keyhash;
        }

        private static byte[] Encrypt(byte[] original, byte[] key)
        {
            System.Security.Cryptography.TripleDESCryptoServiceProvider des = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
            des.Key = MakeMD5(key);
            des.Mode = System.Security.Cryptography.CipherMode.ECB;

            return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
        }

        private static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            System.Security.Cryptography.TripleDESCryptoServiceProvider des = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
            des.Key = MakeMD5(key);
            des.Mode = System.Security.Cryptography.CipherMode.ECB;

            return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
        }
        #endregion
    }
}
