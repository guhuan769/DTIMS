using System;

namespace Sys.Comm.WebTools
{
	/// <summary>
	/// Net 的摘要说明。
	/// </summary>
	public class Net
	{
		/// <summary>
		/// 
		/// </summary>
		public Net()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
		/// <summary>
		/// 转换IP为整型数据，转换前： 192.1.1.240   
		///转换后： -1073675792 
		/// </summary>
		/// <param name="ip">要转换的IP</param>
		/// <returns></returns>
		public int ConvertIP(string ip) {
				return IPConvertToInt(ip);
		}
		/// <summary>
		/// 转换IP为整型数据，转换前： 192.1.1.240   
		///转换后： -1073675792 
		/// </summary>
		/// <param name="ip">要转换的IP</param>
		/// <returns></returns>
		public static int IPConvertToInt(string ip) {
			string [] aryIP = ip.Split('.');

			if(aryIP.Length != 4) {
				return 0;
			}
			int iRet = Convert.ToInt32(aryIP[0]) * 256 * 256 * 256;
			iRet += Convert.ToInt32(aryIP[1]) * 256 * 256;
			iRet += Convert.ToInt32(aryIP[2]) * 256;
			iRet += Convert.ToInt32(aryIP[3]) ;
			return iRet;
		} 

		/// <summary>
		/// 将转换的整数,转换为IP
		/// </summary>
		/// <param name="ip_Int">要转换的数</param>
		/// <returns></returns>
		public static string IntConvertToIP(long ip_Int) {
			long seg1 = (ip_Int & 0xff000000) >> 24;
			if (seg1 < 0)
				seg1 += 0x100;
			long seg2 = (ip_Int & 0x00ff0000) >> 16;
			if (seg2 < 0)
				seg2 += 0x100;
			long seg3 = (ip_Int & 0x0000ff00) >> 8;
			if (seg3 < 0)
				seg3 += 0x100;
			long seg4 = (ip_Int & 0x000000ff);
			if (seg4 < 0)
				seg4 += 0x100;
			string ip = seg1.ToString() + "." + seg2.ToString() + "." + seg3.ToString() + "." + seg4.ToString();

			return ip;
		}

	}
}
