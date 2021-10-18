using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace BJ.DTIMS.Comm.Config
{
    public class AreaInfo
    {
        private static Hashtable ht = null;
        public static String Parameter(String key)
        {
            if (ht == null || ht.Count == 0)
            {
                InitHashtable();
            }
            if (ht[key] != null)
            {
                return ht[key].ToString().Trim();
            }
            throw (new Exception("配置文件中还未指定\"" + key + "\"关健字，请添加！"));
        }

        private static void InitHashtable()
        {
            ht = new Hashtable();
            XmlDocument XmlDoc = new XmlDocument();
            XmlTextReader myReader = null;
            try
            {
                myReader =
                    new XmlTextReader(System.IO.Path.GetDirectoryName(
                    AppDomain.CurrentDomain.SetupInformation.ConfigurationFile) + "\\Config\\AreaInfo.xml");
                XmlDoc.Load(myReader);

                XmlNodeList areaInfo = XmlDoc.GetElementsByTagName("Area");
                foreach (XmlNode node in areaInfo)
                {
                    if (!ht.ContainsKey(node.Attributes["ID"].Value.Trim()))
                    {
                        ht.Add(node.Attributes["ID"].Value.Trim(), node.Attributes["NO"].Value.Trim());
                    }
                }
            }
            catch (Exception e)
            {
                throw (new Exception("读配置文件出错，请按规范检查系统默认值对应的配制文件是否有误，详细原因为：" + e.Message));
            }
            finally
            {
                if (myReader != null)
                {
                    myReader.Close();
                }
            }
        }
    }
}
