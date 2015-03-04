using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Basement.Framework.Security
{
    public static class SymmetricEncryptionUtil
    {
        
        /// <summary>
        /// 加密算法名称：Rijndael，DES
        /// </summary>
        private static string _algorithmName = "DES";
        public static string AlgorithmName
        {
            get { return SymmetricEncryptionUtil._algorithmName; }
            set { SymmetricEncryptionUtil._algorithmName = value; }
        }

        public static byte[] Key { get; set; }

        /// <summary>
        /// 是否对密钥使用系统密钥加密后再存储
        /// </summary>
        private static bool _protectKey = false;
        public static bool ProtectKey
        {
            get { return SymmetricEncryptionUtil._protectKey; }
            set { SymmetricEncryptionUtil._protectKey = value; }
        }

        public static string KeyFilePath
        {
            get
            {
                return Path.Combine(System.Web.HttpContext.Current.Server.MapPath("/"), "Content/Encryption/SymmetricEncryption.key");
            }
        }

        public static string KeyFileDirectoryPath
        {
            get
            {
                return Path.Combine(System.Web.HttpContext.Current.Server.MapPath("/"), "Content/Encryption");
            }
        }

        public static byte[] EncryptData(byte[] data)
        {
            // Now Create the algorithm
            SymmetricAlgorithm Algorithm = SymmetricAlgorithm.Create(AlgorithmName);
            if (Key == null)
            {
                Algorithm.GenerateKey();
                Key = Algorithm.Key;
                #region Store the key in a file
                byte[] storedKey;
                if (ProtectKey)
                    storedKey = ProtectedData.Unprotect(Key, null, DataProtectionScope.LocalMachine);
                else
                    storedKey = Key;

                DirectoryInfo d = new DirectoryInfo(KeyFileDirectoryPath);
                if (!d.Exists)
                {
                    d.Create();
                }
                using (FileStream fs = new FileStream(KeyFilePath, FileMode.Create))
                {
                    fs.Write(storedKey, 0, storedKey.Length);
                }
                #endregion
            }
            else
            {
                Algorithm.Key = Key;
            }

            // Encrypt information
            MemoryStream Target = new MemoryStream();

            // Append IV
            Algorithm.GenerateIV();
            Target.Write(Algorithm.IV, 0, Algorithm.IV.Length);//Write IV

            // Encrypt actual data
            using (CryptoStream cs = new CryptoStream(Target, Algorithm.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                //cs.FlushFinalBlock();//报错
            }
            return Target.ToArray();
        }

        public static byte[] DecryptData(byte[] data)
        {
            // Now create the algorithm
            SymmetricAlgorithm Algorithm = SymmetricAlgorithm.Create(AlgorithmName);
            if (Key == null)
            {
                #region Read the Key from File
                if (!File.Exists(KeyFilePath))
                    throw new Exception("Can't find the Encrypt/Decrypt Key");
                byte[] storedKey;
                using (FileStream fs = new FileStream(KeyFilePath, FileMode.Open))
                {
                    storedKey = new byte[fs.Length];
                    fs.Read(storedKey, 0, (int)fs.Length);
                }
                if (ProtectKey)
                    Key = ProtectedData.Unprotect(storedKey, null, DataProtectionScope.LocalMachine);
                else
                    Key = storedKey;
                #endregion
            }
            Algorithm.Key = Key;

            // Decrypt information
            MemoryStream Target = new MemoryStream();

            // Read IV
            int ReadPos = 0;
            byte[] IV = new byte[Algorithm.IV.Length];
            Array.Copy(data, IV, IV.Length);
            Algorithm.IV = IV;
            ReadPos += Algorithm.IV.Length;

            using (CryptoStream cs = new CryptoStream(Target, Algorithm.CreateDecryptor(), CryptoStreamMode.Read))
            {
                cs.Read(data, ReadPos, data.Length - ReadPos);
                //cs.FlushFinalBlock();//报错
            }

            return Target.ToArray();
        }

        public static string EncryptData(string data)
        {
            byte[] encrypted = EncryptData(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptData(string data)
        {
            byte[] encrypted = Convert.FromBase64String(data);
            byte[] decrypted = DecryptData(encrypted);
            return Encoding.UTF8.GetString(decrypted);
        }
    }
}
