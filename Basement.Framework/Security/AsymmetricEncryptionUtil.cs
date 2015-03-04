using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Basement.Framework.Security
{
    public static class AsymmetricEncryptionUtil
    {
        private static bool _ProtectKey;
        /// <summary>
        /// 是否对私钥使用系统密钥加密后再存储
        /// </summary>
        public static bool ProtectKey
        {
            get { return _ProtectKey; }
            set { _ProtectKey = value; }
        }
        /// <summary>
        /// 生成私钥并存储，返回公钥
        /// </summary>
        /// <param name="targetFile"></param>
        /// <returns></returns>
        public static string GenerateKey(string targetFile)
        {
            RSACryptoServiceProvider Algorithm = new RSACryptoServiceProvider();//对于非对称加密算法目前.net只支持RSA

            // Save the private key
            string CompleteKey = Algorithm.ToXmlString(true);
            byte[] KeyBytes = Encoding.UTF8.GetBytes(CompleteKey);

            if (ProtectKey)
            {
                KeyBytes = ProtectedData.Protect(KeyBytes,
                                null, DataProtectionScope.LocalMachine);
            }
            using (FileStream fs = new FileStream(targetFile, FileMode.Create))
            {
                fs.Write(KeyBytes, 0, KeyBytes.Length);
            }
            
            // Return the public key
            return Algorithm.ToXmlString(false);
        }
        /// <summary>
        /// 从本地存储读取私钥
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="keyFile"></param>
        private static void ReadKey(RSACryptoServiceProvider algorithm, string keyFile)
        {
            byte[] KeyBytes;

            using(FileStream fs = new FileStream(keyFile, FileMode.Open)) 
            {
                KeyBytes = new byte[fs.Length];
                fs.Read(KeyBytes, 0, (int)fs.Length);
            }
            if (ProtectKey)
            {
                KeyBytes = ProtectedData.Unprotect(KeyBytes,
                                null, DataProtectionScope.LocalMachine);
            }
            algorithm.FromXmlString(Encoding.UTF8.GetString(KeyBytes));
        }

        public static byte[] EncryptData(string data, string publicKey)
        {
            // Create the algorithm based on the key
            RSACryptoServiceProvider Algorithm = new RSACryptoServiceProvider();
            Algorithm.FromXmlString(publicKey);

            // Now encrypt the data
            return Algorithm.Encrypt(
                        Encoding.UTF8.GetBytes(data), true);
        }

        public static string DecryptData(byte[] data, string keyFile)
        {
            RSACryptoServiceProvider Algorithm = new RSACryptoServiceProvider();
            ReadKey(Algorithm, keyFile);

            byte[] ClearData = Algorithm.Decrypt(data, true);
            return Convert.ToString(
                        Encoding.UTF8.GetString(ClearData));
        }
    }
}
