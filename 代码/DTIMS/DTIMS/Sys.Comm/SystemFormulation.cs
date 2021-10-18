using System;
using System.Collections;
using System.Data;
using System.Xml;

namespace Sys.Com.Common
{
	#region 系统关健字，值对
	/// <summary>
	/// SystemConsent 的摘要说明。
	/// </summary>
	public class SystemWebFormulation
	{
        //private static String mMainDataBaseName = null;		//总库数据库实例名称
		private static Hashtable ht = null;
		private SystemWebFormulation(){}

		/// <summary>
		/// 得到配制的关健字值字符串。
		/// </summary>
		/// <param name="key">关健字</param>
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
			throw(new Exception("配制文件中还未指定\"" + key + "\"关健字，请添加！"));
		}

		#region 初使化Hashtable值。
		/// <summary>
		/// 初使化Hashtable值。
		/// 配制文件路径为根目录的Resource目录中，文件名为：SystemConsent.xml
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
				throw(new Exception("读配制文件出错，请按规范检查系统默认值对应的配制文件是否有误，详细原因为：" + e.Message));
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

