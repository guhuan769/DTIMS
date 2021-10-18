using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace BJ.Sys.Comm
{
    /// <summary>
    /// 功能描述：加密算法
    /// 修改日期：2010-4-15 新增
    /// <author>MXJ</author>
    /// </summary>
    public class EnDeCrypt
    {
        private static string passKey = "C网综合查询系统伵伵×è囯?啫再戛&@傯喱*-がsss";

        #region encrypt

        /// <summary>
        /// 功能描述：加密
        /// </summary>
        /// <author>MXJ</author>
        /// <param name="originalText">加密字符串</param>
        /// <returns></returns>
        public static string Encrypt(string originalText)
        {
            return Encrypt(originalText, passKey);
        }

        /// <summary>
        /// 功能描述：加密
        /// </summary>
        /// <author>MXJ</author>
        /// <param name="originalText">加密字符串</param>
        /// <param name="secretKey">密码密钥</param>
        /// <returns></returns>
        public static string Encrypt(string originalText, string secretKey)
        {
            byte[] orgText = Encoding.Default.GetBytes(originalText);
            byte[] key = Encoding.Default.GetBytes(secretKey);

            return Encrypt(orgText, key);
        }

        protected static string Encrypt(byte[] originalText, byte[] secretKey)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] keyhash = md5.ComputeHash(secretKey);
            des.Key = keyhash;
            des.Mode = CipherMode.ECB;
            ICryptoTransform transform = des.CreateEncryptor();
            byte[] decrypted = transform.TransformFinalBlock(originalText, 0, originalText.Length);

            return Convert.ToBase64String(decrypted);
        }

        #endregion encrypt

        #region Decrypt

        /// <summary>
        /// 功能描述：解密
        /// 修改说明：2010-4-15 新增
        /// <author>MXJ</author>
        /// </summary>
        /// <param name="cryptograph">解密字符串</param>
        /// <returns></returns>
        public static string Decrypt(string cryptograph)
        {
            return Decrypt(cryptograph, passKey);
        }

        /// <summary>
        /// 功能描述：解密
        /// 修改说明：2010-4-15 新增
        /// <author>MXJ</author>
        /// </summary>
        /// <param name="cryptograph">解密字符串</param>
        /// <param name="secretKey">解密密钥</param>
        /// <returns></returns>
        public static string Decrypt(string cryptograph, string secretKey)
        {
            byte[] decrypted = Convert.FromBase64String(cryptograph);
            byte[] key = Encoding.Default.GetBytes(secretKey);

            return Decrypt(decrypted, key);
        }

        protected static string Decrypt(byte[] cryptograph, byte[] secretKey)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] keyhash = md5.ComputeHash(secretKey);
            des.Key = keyhash;
            des.Mode = CipherMode.ECB;
            ICryptoTransform transform = des.CreateDecryptor();
            byte[] originalText = transform.TransformFinalBlock(cryptograph, 0, cryptograph.Length);

            return Encoding.Default.GetString(originalText);
        }

        #endregion Decrypt
    }
}
