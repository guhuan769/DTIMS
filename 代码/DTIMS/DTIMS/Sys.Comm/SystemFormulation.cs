using System;
using System.Collections;
using System.Data;
using System.Xml;

namespace Sys.Com.Common
{
	#region ϵͳ�ؽ��֣�ֵ��
	/// <summary>
	/// SystemConsent ��ժҪ˵����
	/// </summary>
	public class SystemWebFormulation
	{
        //private static String mMainDataBaseName = null;		//�ܿ����ݿ�ʵ������
		private static Hashtable ht = null;
		private SystemWebFormulation(){}

		/// <summary>
		/// �õ����ƵĹؽ���ֵ�ַ�����
		/// </summary>
		/// <param name="key">�ؽ���</param>
		/// <returns></returns>
		public static String Parameter(String key)
		{
			if(ht == null || ht.Count==0)
			{
				InitHashtable();
			}
			if(ht[key] != null)
			{
				return ht[key].ToString().Trim();
			}
			throw(new Exception("�����ļ��л�δָ��\"" + key + "\"�ؽ��֣�����ӣ�"));
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
					AppDomain.CurrentDomain.SetupInformation.ConfigurationFile) +"\\Config\\SystemConsent.xml" );
				XmlDoc.Load(myReader);
           
				XmlNodeList myNodes = XmlDoc.GetElementsByTagName("parameters");
				int count = myNodes.Item(0).ChildNodes.Count;

				for(int i=0;i<count;i++)
				{
					System.Xml.XmlNode node = myNodes.Item(0).ChildNodes[i];
					String strKey = node.Attributes["parameter"].Value.Trim();
					String strValue = node.Attributes["paravalue"].Value.Trim();
                    ht[strKey] = strValue;
					//ht.Add(strKey , strValue);
				}
			}
			catch(Exception e)
			{
				throw(new Exception("�������ļ������밴�淶���ϵͳĬ��ֵ��Ӧ�������ļ��Ƿ�������ϸԭ��Ϊ��" + e.Message));
			}
			finally
			{
				if(myReader != null)
				{
					myReader.Close();
				}
			}
		}
		#endregion
	}
	#endregion
}

