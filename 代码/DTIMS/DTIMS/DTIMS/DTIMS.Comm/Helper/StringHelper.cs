using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Microsoft.JScript;

namespace BJ.DTIMS.Comm.Helper
{
    /// <summary>
    /// String 帮助类。
    /// </summary>
    public static class StringHelper
    {
        #region 使用JavaScript方式转换字符串
        /// <summary>
        /// 转换指定的字符串，以使用 % 字符对保留字符进行转义，并以 Unicode 法表示它们。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Escape(string str)
        {
            return GlobalObject.escape(str);
        }

        /// <summary>
        /// 将指定字符串中 % 的已转义字符转换成其原始格式。已转义的字符以 Unicode 表示法表示。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Unescape(string str)
        {
            return GlobalObject.unescape(str);
        }
        #endregion

        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        /// <param name="value">要测试的字符串。</param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(string value)
        {
            return string.IsNullOrEmpty(value) || value.Trim().Length == 0;
        }

        /// <summary>
        /// 指示输入的字符串是否全由十进制数字组成。
        /// </summary>
        /// <param name="value">检测的字符串。</param>
        /// <returns></returns>
        public static bool IsDigits(string value)
        {
            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// XML序列化对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="t">将要序列化的对象实例。</param>
        /// <returns></returns>
        public static string XmlSerialize<T>(T t)
        {
            try
            {
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");

                XmlSerializer ser = new XmlSerializer(typeof(T));
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(stream, Encoding.UTF8))
                    {
                        ser.Serialize(writer, t, namespaces);
                        writer.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "XML Serialize Error: " + ex.ToString();
            }
        }

        /// <summary>
        /// XML反序列化对象。
        /// </summary>
        /// <typeparam name="T">返回的对象。</typeparam>
        /// <param name="xmlString">XML字符串。</param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string xmlString)
        {
            T desT = default(T);
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (StringReader sr = new StringReader(xmlString))
            {
                desT = (T)ser.Deserialize(sr);
            }
            return desT;
        }

    }
}
