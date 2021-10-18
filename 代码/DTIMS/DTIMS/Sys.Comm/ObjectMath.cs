using System;
using System.Data;
using System.Text;
using System.Web.UI;

namespace Sys.Comm.WebTools
{
	/// <summary>
	/// 类中常用的方法集合，一般只提供静态方法，不用实例化
	/// </summary>
	public class ObjectMath
	{
		public ObjectMath()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		/// <summary>
		/// 查看传入的字符串是否超长
		/// </summary>
		/// <param name="str">需要判断的字符串</param>
		/// <param name="t">系统对比的长度</param>
		/// <returns>如果传入的字符串大于此长度则返回TRUE，如果小于或等于则返回FALSE</returns>
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
		/// 排序后再绑定数据
		/// </summary>
		/// <param name="e">排序参数</param>
		/// <param name="sb">页面视图</param>
		/// <param name="dg">DataGrid控件</param>
		/// <param name="dt">数据源</param>
		/// <param name="dgf">过滤控件</param>
		public static void DataBindSort(System.Web.UI.WebControls.DataGridSortCommandEventArgs e,
			System.Web .UI.StateBag sb,
            System.Web.UI.WebControls.DataGrid dg, DataTable dt, Sys.Comm.WebTools.DataGridFilter dgf)
		{
			//实现正反向排序
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
		/// 排定数据
		/// </summary>
		/// <param name="_sort">排序表达式</param>
		/// <param name="dg">DataGrid控件</param>
		/// <param name="dt">数据源</param>
		/// <param name="dgf">过滤条</param>
        public static void DataBind(object _sort, System.Web.UI.WebControls.DataGrid dg, DataTable dt, Sys.Comm.WebTools.DataGridFilter dgf)
		{
			//绑定数据				
        
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

		#region 显示提示信息
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
