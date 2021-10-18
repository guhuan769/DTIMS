using System;
using System.Data;
using System.Text;
using System.Web.UI;

namespace Sys.Comm.WebTools
{
	/// <summary>
	/// ���г��õķ������ϣ�һ��ֻ�ṩ��̬����������ʵ����
	/// </summary>
	public class ObjectMath
	{
		public ObjectMath()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		/// <summary>
		/// �鿴������ַ����Ƿ񳬳�
		/// </summary>
		/// <param name="str">��Ҫ�жϵ��ַ���</param>
		/// <param name="t">ϵͳ�Աȵĳ���</param>
		/// <returns>���������ַ������ڴ˳����򷵻�TRUE�����С�ڻ�����򷵻�FALSE</returns>
		public static bool IsOverLong(string str , Int32 t)
		{
			System.Byte [] bytes = System.Text.Encoding.Default.GetBytes(str);
			if(bytes.Length > t)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// ������ٰ�����
		/// </summary>
		/// <param name="e">�������</param>
		/// <param name="sb">ҳ����ͼ</param>
		/// <param name="dg">DataGrid�ؼ�</param>
		/// <param name="dt">����Դ</param>
		/// <param name="dgf">���˿ؼ�</param>
		public static void DataBindSort(System.Web.UI.WebControls.DataGridSortCommandEventArgs e,
			System.Web .UI.StateBag sb,
            System.Web.UI.WebControls.DataGrid dg, DataTable dt, Sys.Comm.WebTools.DataGridFilter dgf)
		{
			//ʵ������������
			sb["sortOrder"] = e.SortExpression.ToString().Trim();
			if(sb["SortExp"]==null)
			{
				sb["sortOrder"] = sb["sortOrder"].ToString().Trim().Substring(0,
					sb["sortOrder"].ToString().Trim().IndexOf(' ')+1)+" DESC";
				sb["SortExp"] = sb["sortOrder"].ToString().Trim();				
			}
			else
			{
				if(sb["SortExp"].ToString().Trim() == sb["sortOrder"].ToString().Trim())
				{
					sb["sortOrder"] = sb["sortOrder"].ToString().Trim().Substring(0,
						sb["sortOrder"].ToString().Trim().IndexOf(' ')+1)+" DESC";
				}
				sb["SortExp"] = sb["sortOrder"].ToString().Trim();
			}

			ObjectMath.DataBind(sb["SortExp"],
				dg ,
				dt ,
				dgf );	
						

		}

		/// <summary>
		/// �Ŷ�����
		/// </summary>
		/// <param name="_sort">������ʽ</param>
		/// <param name="dg">DataGrid�ؼ�</param>
		/// <param name="dt">����Դ</param>
		/// <param name="dgf">������</param>
        public static void DataBind(object _sort, System.Web.UI.WebControls.DataGrid dg, DataTable dt, Sys.Comm.WebTools.DataGridFilter dgf)
		{
			//������				
        
			int pagecount = dt.Rows.Count/dg .PageSize;
			if(pagecount*dg.PageSize<dt.Rows .Count)
			{
				pagecount ++;
			}
         
			if(dg.CurrentPageIndex >= pagecount&&pagecount>0 )
			{
				dg .CurrentPageIndex =pagecount-1;
			}
     
			if(_sort != null)
			{
				dt.DefaultView.Sort =_sort.ToString ();
			}
         
			dg.DataSource =  dt;
			dg.DataBind();  
			dgf.Filter();      
		} 

		#region ��ʾ��ʾ��Ϣ
		public static void Close(Page page, string msg)
		{
			if(msg.Trim ().Length >0)
			{
				msg = System.Web .HttpUtility .UrlEncode(msg);
				page.Response.Write("<script language='javascript'>alert('"+msg+"');</script>");				
			}
			page.Response.Write("<script language='javascript'>window.close();</script>");
		}

		public static void ShowMessage(Page page, string msg)
		{
			msg = CheckSpecialString(msg);
			//Page.ClientScript.RegisterStartupScript(this.GetType(), "Error","<script language='javascript'>alert('"+msg+"');</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), "Error", "<script language='javascript'>alert('" + msg + "');</script>");
		}

		private static string CheckSpecialString(string str)
		{
			string strTemp = null;
			for(int i=0;i<str.Length;i++)
			{
				if((str[i].ToString().Trim()!="<")&&(str[i].ToString().Trim()!=">")
					&&(str[i].ToString().Trim()!="/")&&(str[i].ToString().Trim()!=":")&&(str[i].ToString().Trim()!="'"))
				{
					strTemp += str[i].ToString().Trim();
				}
			}
			return strTemp;
		}
		#endregion
	}
}
