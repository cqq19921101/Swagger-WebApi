using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Liteon.Mes.Utility
{
    /// <summary>
    /// 加密和解密字符串
    /// </summary>
    public class Encrypt
    {
        #region 加密解密字符串

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="EncryptString">待加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string DesEncrypt(string EncryptString)
        {
            byte[] inputByteArray = Encoding.UTF8.GetBytes(EncryptString);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            des.IV = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="DecryptString">加密后的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string DesDecrypt(string DecryptString)
        {
            try
            {
                byte[] inputByteArray = Convert.FromBase64String(DecryptString);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                des.IV = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, des.CreateDecryptor(), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return "";
            }
        }

        #endregion
    }
}
