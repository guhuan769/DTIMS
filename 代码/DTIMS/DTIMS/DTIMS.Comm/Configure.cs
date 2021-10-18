using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Data.Common;
using System.Text.RegularExpressions;

using Microsoft.Practices.EnterpriseLibrary.Data;

namespace DTIMS.Comm
{
    public class Configure
    {
        private static Hashtable ht = null;    //��ȡ�����ļ��е�����������
        private static Hashtable htDataBaseInfo = null;
        private static Hashtable htDescChs = null;//����ԭ��
        private static Hashtable htHandleChs = null;//������

        public Configure()
        {
        }

        /// <summary>
        /// �õ����ƵĹؽ���ֵ�ַ�����
        /// </summary>
        /// <param name="key">�ؽ���</param>
        /// <returns></returns>
        public static String Parameter(String key)
        {
            if (ht == null || ht.Count == 0)
            {
                InitHashtable();
            }

            if (ht[key.Trim()] != null)
            {
                return ht[key.Trim()].ToString().Trim();
            }
            throw (new Exception("�����ļ��л�δָ��\"" + key + "\"�ؽ��֣�����ӣ�"));
        }

        /// <summary>
        /// ��������:�����ڴ��нڵ�(truncateDate)��ֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetParameter(string key, string value)
        {
            if (ht.ContainsKey(key.Trim()))
            {
                ht[key.Trim()] = value.Trim();
            }
            else
            {
                throw (new Exception("�����ļ��л�δָ��\"" + key + "\"�ؽ��֣�����ӣ�"));
            }
        }

        /// <summary>
        /// �������������������ļ��е�ֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetValue(string key, string value)
        {
            try
            {
                string config = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile) ;
                config = Path.Combine(config, "SystemConsent.xml");
                XmlDocument xml = new XmlDocument();
                xml.Load(config);

                XmlNode node = xml.SelectSingleNode(string.Format("descendant::item[@parameter='{0}']",key));
                if (node != null)
                {
                    node.Attributes["paravalue"].Value = value.Trim();
                    xml.Save(config);
                }
                else
                {
                    throw new Exception("δ�ҵ��ڵ�[TruncateDate]");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("���������ļ�ʱ����," + ex.Message);
            }
        }

        #region ��ʹ��Hashtableֵ��
        /// <summary>
        /// ��ʹ��Hashtableֵ��
        /// �����ļ�·��Ϊ��Ŀ¼��ResourceĿ¼�У��ļ���Ϊ��SystemConsent.xml
        /// </summary>
        private static void InitHashtable()
        {
            ht = new Hashtable();
            XmlDocument XmlDoc = new XmlDocument();
            XmlTextReader myReader = null;
            try
            {
                myReader =
                    new XmlTextReader(System.IO.Path.GetDirectoryName(
                    AppDomain.CurrentDomain.SetupInformation.ConfigurationFile) + "\\Config\\SystemConsent.xml");
                XmlDoc.Load(myReader);

                XmlNodeList myNodes = XmlDoc.GetElementsByTagName("parameters");
                int count = myNodes.Item(0).ChildNodes.Count;

                for (int i = 0; i < count; i++)
                {
                    System.Xml.XmlNode node = myNodes.Item(0).ChildNodes[i];
                    String strKey = node.Attributes["parameter"].Value.Trim();
                    String strValue = node.Attributes["paravalue"].Value.Trim();

                    if (ht.ContainsKey(strKey))
                    {
                        continue;
                    }
                    ht.Add(strKey, strValue);
                }
            }
            catch (Exception e)
            {
                throw (new Exception("�������ļ������밴�淶���ϵͳĬ��ֵ��Ӧ�������ļ��Ƿ�������ϸԭ��Ϊ��" + e.Message));
            }
            finally
            {
                if (myReader != null)
                {
                    myReader.Close();
                }
            }
        }
        #endregion

        public static String GetDataInfo(String key)
        {
            if (htDataBaseInfo == null || htDataBaseInfo.Count == 0)
            {
                htDataBaseInfo = new Hashtable();
                Database db = DatabaseFactory.CreateDatabase();
                
                //�������ݿ�������Ϣ
                using (DbConnection conn = db.CreateConnection())
                {
                    htDataBaseInfo.Add("Database", conn.Database.Trim());
                    htDataBaseInfo.Add("Server", conn.DataSource.Trim());

                    string connString = conn.ConnectionString.Trim();
                    Match match = Regex.Match(connString, "(?<=User ID=).*?(?=;)");
                    if (match.Success)
                    {
                        htDataBaseInfo.Add("User ID", match.Value.Trim());
                    }
                    else
                    {
                        throw new Exception("��ȡ���ݿ�������ϢUser ID�ڵ�ʱ����");
                    }

                    match = Regex.Match(connString, "(?<=Password=).*?(?=;)");
                    if (match.Success)
                    {
                        htDataBaseInfo.Add("Password", match.Value.Trim());
                    }
                    else
                    {
                        throw new Exception("��ȡ���ݿ�������ϢPassword�ڵ�ʱ����");
                    }
                }
            }

            if (htDataBaseInfo[key.Trim()] != null)
            {
                return htDataBaseInfo[key.Trim()].ToString().Trim();
            }
            throw (new Exception("��ȡ�����ļ�ʱ����\"" + key + "\"�ؽ��֣�����ӣ�"));
        }

        #region ��ȡ����ԭ��
        public static string GetErrorCodeDescChs(string errorCode)
        {
            if (htDescChs == null || htDescChs.Count == 0)
            {
                InitErrorCodeDescChs();
            }

            if (htDescChs[errorCode.Trim()] != null)
            {
                return htDescChs[errorCode.Trim()].ToString().Trim();
            }
            else
            {
                return "δ֪����";
            }
        }

        private static void InitErrorCodeDescChs()
        {
            htDescChs = new Hashtable();
            XmlDocument XmlDoc = new XmlDocument();
            XmlTextReader myReader = null;
            try
            {
                myReader =
                    new XmlTextReader(System.IO.Path.GetDirectoryName(
                    AppDomain.CurrentDomain.SetupInformation.ConfigurationFile) + "\\Config\\ErrorCode.xml");
                XmlDoc.Load(myReader);

                XmlNode errorNode = XmlDoc.SelectSingleNode("/Root/SMS/ErrorCode");
                foreach (XmlNode node in errorNode.ChildNodes)
                {
                    String strKey = node.Attributes["ErrorCode"].Value.Trim();
                    String strValue = node.Attributes["DescChs"].Value.Trim();
                    if (htDescChs.ContainsKey(strKey))
                    {
                        continue;
                    }
                    htDescChs.Add(strKey, strValue);
                }
            }
            catch (Exception e)
            {
                throw (new Exception("�������ļ�������ϸԭ��Ϊ��" + e.Message));
            }
            finally
            {
                if (myReader != null)
                {
                    myReader.Close();
                }
            }
        }
        #endregion

        #region ��ȡ������
        public static string GetErrHandleChs(string errorCode)
        {
            if (htHandleChs == null || htHandleChs.Count == 0)
            {
                InitErrHandleChs();
            }

            if (htHandleChs[errorCode.Trim()] != null)
            {
                return htHandleChs[errorCode.Trim()].ToString().Trim();
            }
            else
            {
                return "δ֪����";
            }
        }

        private static void InitErrHandleChs()
        {
            htHandleChs = new Hashtable();
            XmlDocument XmlDoc = new XmlDocument();
            XmlTextReader myReader = null;
            try
            {
                myReader =
                    new XmlTextReader(System.IO.Path.GetDirectoryName(
                    AppDomain.CurrentDomain.SetupInformation.ConfigurationFile) + "\\Config\\ErrorCode.xml");
                XmlDoc.Load(myReader);

                XmlNode errorNode = XmlDoc.SelectSingleNode("/Root/SMS/ErrorCode");
                foreach (XmlNode node in errorNode.ChildNodes)
                {
                    String strKey = node.Attributes["ErrorCode"].Value.Trim();
                    String strValue = node.Attributes["HandleChs"].Value.Trim();
                    if (htHandleChs.ContainsKey(strKey))
                    {
                        continue;
                    }
                    htHandleChs.Add(strKey, strValue);
                }
            }
            catch (Exception e)
            {
                throw (new Exception("�������ļ�������ϸԭ��Ϊ��" + e.Message));
            }
            finally
            {
                if (myReader != null)
                {
                    myReader.Close();
                }
            }
        }
        #endregion


    }//end public class Configure
}//end namespace Inphase.SMS.Common
