using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;
using System.IO;

namespace BJ.DTIMS.Common
{
    public static class IMSConfigure
    {
        private static readonly XmlDocument xmlDoc; //IMS配置文档

        static IMSConfigure()
        {
            xmlDoc = new XmlDocument();
        }

        /// <summary>
        /// 加载配置文件。
        /// </summary>
        public static void Load()
        {
            string path = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile), "Config\\IMSConfig.xml");
            xmlDoc.Load(path);
        }

        /// <summary>
        /// 获取配置文件中的字段值列表。
        /// </summary>
        /// <param name="typeName">类型名。</param>
        /// <param name="fieldName">字段名。</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetFieldValues(string typeName, string fieldName)
        {
            // 获取指定类型名下的字段名节点。
            XmlNode fieldNode = xmlDoc.SelectSingleNode(string.Format("/HSS/{0}/Fields/{1}", typeName, fieldName));
            if (fieldNode == null)
            {
                throw new ArgumentException(string.Format("读取配置文件(IMSConfig.xml)出错，配置文件中不包含指定类型名({0})下的字段名({1})节点，请检查配置文件。", typeName, fieldName));
            }

            // 获取字段名节点下的所有 Item 节。
            XmlNodeList itemsNode = fieldNode.SelectNodes("./Item");
          
            // 保存 Item 节列表。
            Dictionary<string, string> returnList = new Dictionary<string, string>(itemsNode.Count);
            try
            {
                foreach (XmlNode node in itemsNode)
                {
                    // 以 Value 属性为键、Name 属性为值，存入字典。
                    if (!returnList.ContainsKey(node.Attributes["ID"].Value))
                    {
                        returnList.Add(node.Attributes["ID"].Value, node.Attributes["Value"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return returnList;
        }
    }
}
