using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace BJ.Sys.Comm
{
    /// <summary>
    /// ���������������㷨
    /// �޸����ڣ�2010-4-15 ����
    /// <author>MXJ</author>
    /// </summary>
    public class EnDeCrypt
    {
        private static string passKey = "C���ۺϲ�ѯϵͳ�Łš�����?�����&@�����*-��sss";

        #region encrypt

        /// <summary>
        /// ��������������
        /// </summary>
        /// <author>MXJ</author>
        /// <param name="originalText">�����ַ���</param>
        /// <returns></returns>
        public static string Encrypt(string originalText)
        {
            return Encrypt(originalText, passKey);
        }

        /// <summary>
        /// ��������������
        /// </summary>
        /// <author>MXJ</author>
        /// <param name="originalText">�����ַ���</param>
        /// <param name="secretKey">������Կ</param>
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
        /// ��������������
        /// �޸�˵����2010-4-15 ����
        /// <author>MXJ</author>
        /// </summary>
        /// <param name="cryptograph">�����ַ���</param>
        /// <returns></returns>
        public static string Decrypt(string cryptograph)
        {
            return Decrypt(cryptograph, passKey);
        }

        /// <summary>
        /// ��������������
        /// �޸�˵����2010-4-15 ����
        /// <author>MXJ</author>
        /// </summary>
        /// <param name="cryptograph">�����ַ���</param>
        /// <param name="secretKey">������Կ</param>
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
