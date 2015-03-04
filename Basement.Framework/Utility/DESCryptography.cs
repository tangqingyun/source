using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Basement.Framework.Utility
{

    public static class DESCryptography
    {
        #region = Encrypt =
        /// <summary>
        /// ���� DES
        /// </summary>
        /// <param name="pToEncrypt">��Ҫ�����ַ���</param>
        /// <param name="sKey">��Կ</param>
        /// <returns></returns>
        public static string Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //���ַ����ŵ�byte������
            //ԭ��ʹ�õ�UTF8���룬�Ҹĳ�Unicode�����ˣ�����
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);

            //�������ܶ������Կ��ƫ����
            //ʹ�����������������Ӣ���ı�
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }
        #endregion

        #region = Decrypt =
        /// <summary>
        /// ���ܷ���
        /// </summary>
        /// <param name="pToDecrypt">��Ҫ�����ַ���</param>
        /// <param name="sKey">��Կ</param>
        /// <returns></returns>
        public static string Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //����StringBuild����CreateDecryptʹ�õ��������󣬱���ѽ��ܺ���ı����������
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());

        }
        #endregion
    }
}
