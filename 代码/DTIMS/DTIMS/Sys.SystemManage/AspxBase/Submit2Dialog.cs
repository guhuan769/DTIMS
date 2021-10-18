

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Resources;

using Sys.Comm.WebTools;

namespace Sys.Project.Common
{

   /// <summary>
   /// 在需要提交 Form 并打开模式窗口输出的时候提供中间处理。
   /// </summary>
   /// <remarks>
   /// 使用者为了达到上述目的，调用ShowModalDialog()方法必须遵照如下约定：
   /// 其URL（ShowModalDialog的第一个参数）为本类的映射，
   /// 同时以?_Submit2URL=参数传递用户真正需要输出的URL（需要对该URL编码）；
   /// ShowModalDialog的第二个参数传递提交的Form对象。
   /// </remarks>
	public class Submit2Dialog : AspxPageBase, System.Web.SessionState.IRequiresSessionState
	{
		private String mBaseName;

		/// <summary>
		/// 入口
		/// </summary>
		protected override void FrameworkInitialize() 
		{

			this.OnInit(new EventArgs());
			mBaseName = this.GetType().Namespace + ".Resx." + this.GetType().Name;

			if (Request.HttpMethod.ToUpper().Equals("POST"))
			{
				String url = HtmlUtil.queryString(Request, "_Submit2URL");
				if (!url.Equals(""))
				{
					StringBuilder sb = new StringBuilder();
					sb.Append("<html><head><title></title>\n");
					sb.Append("<base target='_self'>\n");
					sb.Append("<script>\n");
	
					sb.Append("function dosubmit(){\n");
					sb.Append("   _xdiv.innerHTML = window.dialogArguments.innerHTML;\n");
//					sb.Append("alert(_xdiv.innerHTML); \n");
					sb.Append("   var _formObj = document.forms[0];\n");
					sb.Append("   if (typeof(_formObj) == 'undefined') { \n");
					sb.Append("      alert('" + GetString(mBaseName, "msg.errAlert") + "'); \n");
					sb.Append("      window.close(); \n");
					sb.Append("      return; \n");
					sb.Append("   }\n");
					sb.Append("		var obj1 = document.getElementsByName('__EVENTTARGET');   \n");
					sb.Append("		var obj2 = document.getElementsByName('__EVENTARGUMENT');   \n");
					sb.Append("		var obj3 = document.getElementsByName('__VIEWSTATE');   \n");
					sb.Append("    removeobj(obj1); \n");
					sb.Append("    removeobj(obj2); \n");
					sb.Append("    removeobj(obj3); \n");
			
					//移出__EVENTTARGET，__EVENTARGUMENT控件，使系统认为是第一次刷新
					//使用提交的方式更新页面，可保证提交页面的一小部分，以提高效率
//					sb.Append("    alert(obj1);alert(obj2); \n");
					sb.Append("		if(typeof(obj1) != 'undefined')   \n");
					sb.Append("		{   \n");
					sb.Append("        try{ \n");
					sb.Append("			_formObj.removeChild(obj1);   \n");
					sb.Append("			_formObj.removeChild(obj2);   \n");
					sb.Append("        }catch(e){}\n");
					sb.Append("		}   \n");
//					sb.Append("     alert('ok'); \n");
					sb.Append("		_formObj.action = '" + url + "'; \n");
					sb.Append("		_formObj.method = 'post';\n");
					sb.Append("		var viewState =  _formObj.__VIEWSTATE;\n");
					sb.Append("		if ('undefined' != typeof(viewState)) viewState.disabled = true;\n");
					sb.Append("		_formObj.submit();\n");
					sb.Append("		_formObj.action = '';\n");
					sb.Append("		if ('undefined' != typeof(viewState)) viewState.disabled = false;\n");
					sb.Append("}\n");

					sb.Append("function removeobj(objs)\n");
					sb.Append("{\n");
					sb.Append("	for(var i=0;i<objs.length;i++)\n");
					sb.Append("	{\n");
					sb.Append("		try\n");
					sb.Append("		{\n");
					sb.Append("			_xdiv.removeChild(objs[i]);\n");
					sb.Append("		}catch(e){}\n");
					sb.Append("	}\n");
					sb.Append("}\n");
					sb.Append("</script>\n");
					sb.Append("</head>\n");
					sb.Append("<body onload='dosubmit()'>\n");
					sb.Append("<FORM id=Form1 name=Form1>\n");
					sb.Append("<div id='_xdiv' style='display:none'></div>\n");
					sb.Append("</FORM>\n");
					sb.Append("</body>\n");
					sb.Append("</html>\n");

					Response.Output.WriteLine(sb.ToString());
				}
				else
				{
					// 使用者没有正确的设置URL参数
					Response.Write(GetString(mBaseName, "msg.errLabel"));
				}
			}
			else
			{
				OutputTmpPage();
			}
		}


		/// <summary>
		/// 输出中间处理页面，
		/// 该页获取提交的Form数据并提交到用户指定的URL。
		/// </summary>
		private void OutputTmpPage()
		{
			String url = HtmlUtil.queryString(Request, "_Submit2URL");

			StringBuilder sb = new StringBuilder();
			sb.Append("<html><head><title></title>\n");
			sb.Append("<base target='_self'>\n");
			sb.Append("<script>\n");
			sb.Append("function _doSubmit() {\n");
			sb.Append("   _xdiv.innerHTML = window.dialogArguments.innerHTML;\n");
//			sb.Append("   alert(window.dialogArguments.outerHTML); \n");
			sb.Append("   var _formObj = document.forms[0];\n");
//			sb.Append("   alert(_formObj); \n");
			sb.Append("   if (typeof(_formObj) == 'undefined') { \n");
			sb.Append("      alert('" + GetString(mBaseName, "msg.errAlert") + "'); \n");
			sb.Append("      window.close(); \n");
			sb.Append("      return; \n");
			sb.Append("   }\n");

			sb.Append("   _formObj.method = 'post';\n");
			sb.Append("   _formObj.action = '';\n");
			sb.Append("   _formObj.submit();\n");
			sb.Append("}\n");

			sb.Append("</script>\n");
			sb.Append("</head>\n");
			sb.Append("<body onload='_doSubmit()'>\n");
			sb.Append("<FORM id=Form1 name=Form1>\n");
			sb.Append("<div id='_xdiv' style='display:none'></div>\n");
			sb.Append("</FORM>\n");
			sb.Append("</body>\n");
			sb.Append("</html>\n");

			Response.Output.WriteLine(sb.ToString());
		}
	}
}
