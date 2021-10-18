/*
 * @(#)AspxFilter.cs    1.0, Jun 2, 2005
 *
 * @author feng xianwen.
 *
 */

using System;

using BJ.WebTools;

namespace BJ.AspxTask
{

   /// <summary>
   /// Aspx 过滤器，进行页面过滤和登录控制。
   /// </summary>
   public class AspxFilter : System.Web.UI.Page, System.Web.SessionState.IRequiresSessionState
   {

      /// <summary>
      /// 配置节点名称
      /// </summary>
      public const String C_ConfigSection  = "loginInfo";

      /// <summary>
      /// 字符串值各节的分隔符
      /// </summary>
      public const Char   C_ValueNodeSep   = ':';

      /// <summary>
      /// 字符串值中各相关值的分隔符
      /// </summary>
      public const Char   C_ValueClassSep = ',';

      /// <summary>
      /// 默认登录页 URL，相对 ApplicationPath
      /// </summary>
      public const String C_DefLoginPage   = "/Login.aspx";

      /// <summary>
      /// 默认的登录后存在的 Session 变量
      /// </summary>
      public const String C_DefSessionName = "UserID";

      /// <summary>
      /// 默认过期页面 URL，相对 ApplicationPath
      /// </summary>
      public const String C_DefExpirePage  = "/Expire.html";

      /// <summary>
      /// 指定登录存在的 Session 变量的配置节名称
      /// </summary>
      public const String C_SessionNameSec = "sessionNames";

      /// <summary>
      /// 指定忽略登录的页面的配置节名称
      /// </summary>
      public const String C_ExludePagesSec = "exludePages";

      /// <summary>
      /// 指定登录页的配置节名称
      /// </summary>
      public const String C_LoginPageSec   = "loginPage";

      /// <summary>
      /// 指定过期页面的配置节名称
      /// </summary>
      public const String C_ExpirePageSec  = "expirePage";

      /// <summary>
      /// 指定映射的配置节名称
      /// </summary>
      public const String C_MappingSec     = "mappings";

      /// <summary>
      /// 指示 Session 过期的参数名
      /// </summary>
      public const String C_TmpExpireName  = "_AF_IsExpired";

      /// <summary>
      /// 指示 Session 过期的参数值
      /// </summary>
      public const String C_TmpExpireValue = "1";

      private AspxFilter() 
      {
      }

      /// <summary>
      /// 判断当前登录 Session 是否已经过期。
      /// </summary>
      /// <remarks>
      /// 默认判断的 Session 变量是 UserID。
      /// 可以在 Web.config 中设置 sessionNames 的值为需要判断的 Session 变量，
      /// 以 (:) 分隔多个值。
      /// </remarks>
      private Boolean IsExpired 
      {
         get 
         {
            String s = HtmlUtil.getConfigSection(
               AspxFilter.C_ConfigSection, AspxFilter.C_SessionNameSec);

            if (s == null || s.Length == 0) 
            {
               s = AspxFilter.C_DefSessionName;
            }

            String[] ss = s.Split(AspxFilter.C_ValueNodeSep);

            for (int i=0; i<ss.Length; i++) 
            {
               if (this.Session[ss[i]] == null) 
               {
                  return true;
               }
            }

            return false;
         }
      }

      /// <summary>
      /// 当前请求页的 URL （相对 ApplicationPath）。
      /// </summary>
      private String RequestPath 
      {
         get 
         {
            String rt = Request.CurrentExecutionFilePath.Substring(Request.ApplicationPath.Length);
            if (!rt.StartsWith("/")) 
            {
               rt = "/" + rt;
            }
            return rt;
         }
      }

      /// <summary>
      /// 判断请求的页是否允许忽略登录。
      /// 可以在 Web.config 中设置 exludePages 的值为忽略登录的页，
      /// 以 (:) 分隔多个值，
      /// 各值是相对 ApplicationPath 的。
      /// </summary>
      private Boolean IsExcludePage
      {
         get 
         {
            String s = HtmlUtil.getConfigSection(
               AspxFilter.C_ConfigSection, AspxFilter.C_ExludePagesSec);
            if (s == null || s.Length == 0) 
            {
               return false;
            }

            String[] ss = s.Split(AspxFilter.C_ValueNodeSep);

            String cPath = this.RequestPath.ToLower();

            for (int i=0; i<ss.Length; i++) 
            {
               if (ss[i].ToLower().Equals(cPath)) 
               {
                  return true;
               }
            }

            return false;
         }
      }

      /// <summary>
      /// 判断请求的页是否为登录页。
      /// </summary>
      private Boolean IsLoginPage
      {
         get 
         {
            if (this.RequestPath.ToLower().Equals(this.LoginPage.ToLower())) 
            {
               return true;
            }

            return false;
         }
      }

		private Boolean IsLoginOutRequest
		{
			get
			{
				String loginOutPage = "/main2.aspx";
				String requestStr   = "url";
				String loginOutStr  = "login.aspx?actionNo=logout";
				String exitStr      = "login.aspx?actionNo=exit";
				if(this.RequestPath.ToLower().IndexOf(loginOutPage.ToLower()) != -1)
				{
					String tmpSession = HtmlUtil.queryString(Request, requestStr);
					if (tmpSession.IndexOf(loginOutStr)>0 || tmpSession.IndexOf(exitStr)>0)
					{
						return true;
					}
				}
				
				return false;
			}
		}

      /// <summary>
      /// 获取登录页面的URL（相对ApplicationPath）。
      /// 默认登录页是 /Login.aspx。
      /// 可以在 Web.config 中设置 loginPage 的值为登录页。
      /// </summary>
      private String LoginPage
      {
         get 
         {
            String s = HtmlUtil.getConfigSection(
               AspxFilter.C_ConfigSection, AspxFilter.C_LoginPageSec);
            if (s == null || s.Equals("")) 
            {
               s = AspxFilter.C_DefLoginPage;
            }

            return s;
         }
      }

      /// <summary>
      /// 从配置文件获取当前请求页的处理对象，
      /// 如果不存在返回 null。
      /// </summary>
      /// <remarks>
      /// 可以在 Web.config 中为 mappings 设置值指定 URL 映射的处理类及其所属程序集，
      /// 忽略空格和回车换行符
      /// 以 (:) 分隔多个映射，每个映射中以 (,) 分隔 URL 、处理类和程序集。
      /// URL 是相对 ApplicationPath 的
      /// 类要指定全名
      /// </remarks>
      /// <returns>null 或 System.Web.UI.Page 对象</returns>
      private Object GetMapping()
      {

         // 取 mappings 值
         String s = HtmlUtil.getConfigSection(
            AspxFilter.C_ConfigSection, AspxFilter.C_MappingSec);
         if (s == null || s.Length == 0) 
         {
            return null;
         }

         // 忽略空格和回车换行符
         s = s.Replace(" ", "");
         s = s.Replace("\r", "");
         s = s.Replace("\n", "");

         // 为 mappings 值加前导分隔符
         if (!s.StartsWith(""+AspxFilter.C_ValueNodeSep)) 
         {
            s = AspxFilter.C_ValueNodeSep + s;
         }

         // 为 mappings 值加后缀分隔符
         if (!s.EndsWith(""+AspxFilter.C_ValueNodeSep)) 
         {
            s += AspxFilter.C_ValueNodeSep;
         }

         // 获取映射位置
         String pathNode = AspxFilter.C_ValueNodeSep + this.RequestPath + AspxFilter.C_ValueClassSep;
         int idx = s.IndexOf(pathNode);
         if (idx < 0) 
         {
            return null;
         }

         // 截去处理类前的串
         s = s.Substring(idx + pathNode.Length);

         // 获取处理类的结束位置
         idx = s.IndexOf(AspxFilter.C_ValueClassSep);
         if (idx < 0) 
         {
            return null;
         }

         // 处理类的全名
         String typeName = s.Substring(0, idx);

         // 截去处理类
         s = s.Substring(idx + 1);

         // 获取程序集的结束位置
         idx = s.IndexOf(AspxFilter.C_ValueNodeSep);
         if (idx < 0) 
         {
            return null;
         }

         // 程序集名
         String aName = s.Substring(0, idx);

         // 获取并返回请求页的处理类的实例，该实例使用默认构建器创建。
         Object o = Activator.CreateInstance(aName, typeName);
         return ((System.Runtime.Remoting.ObjectHandle)o).Unwrap();

      }

      /// <summary>
      /// 入口
      /// </summary>
      protected override void FrameworkInitialize() 
      {
         // 如果当前页既不是登录页，又不是忽略登录的页，并且 Session 已过期，则转到登录页。
         if (!this.IsLoginPage && 
            !this.IsExcludePage && 
            this.IsExpired && 
				!IsLoginOutRequest) 
         {
            ToLoginPage();
         } 
         else 
         {
            ProcessRequest();
         }

      }

      /// <summary>
      /// 执行正常的页面请求。
      /// </summary>
      private void ProcessRequest() 
      {

         // 如果页面已过期，转向的过期提示页面。
         // 可以在 Web.config 中设置 expirePage 的值为要转向的页面。
         if (this.IsLoginPage) 
         {

            // 获取过期指示参数
            String tmpSession = HtmlUtil.queryString(Request, AspxFilter.C_TmpExpireName);
            if (tmpSession.Equals(AspxFilter.C_TmpExpireValue)) 
            {
               // 从配置文件获取过期输出页 URL
               String expirePage = HtmlUtil.getConfigSection(
                  AspxFilter.C_ConfigSection, AspxFilter.C_ExpirePageSec);

               // 如果没有配置过期页面，使用默认过期页面
               if (expirePage == null || expirePage.Equals("")) 
               {
                  expirePage = AspxFilter.C_DefExpirePage;
               }

               // 转向过期页面
               this.Redirect(expirePage);
            } 
            else 
            {
               // 输出正常的页面。
               this.GetToPage().ProcessRequest(this.Context);
            }
         } 
         else 
         {
            // 输出正常的页面。
            this.GetToPage().ProcessRequest(this.Context);
         }

      }

      /// <summary>
      /// 获取对当前请求响应的页的实例。
      /// </summary>
      /// <returns></returns>
      private System.Web.UI.Page GetToPage() 
      {
         // 如果定义了请求页的处理类，返回处理类的实例
         Object o = this.GetMapping();
         if (o != null) 
         {
            return (System.Web.UI.Page)o;
         }

         // 获取并返回请求页的实例
         System.Web.IHttpHandler ihh = System.Web.UI.PageParser.GetCompiledPageInstance(
            Request.CurrentExecutionFilePath, 
            Request.PhysicalApplicationPath + this.RequestPath.Substring(1).Replace('/', '\\'), 
            this.Context);

         return (System.Web.UI.Page)ihh;
      }

      /// <summary>
      /// 重定向到登录页。
      /// 添加 URL 参数 _AF_IsExpired=1 指示 Session 过期
      /// </summary>
      private void ToLoginPage() 
      {
         Redirect(this.LoginPage + "?" + AspxFilter.C_TmpExpireName + "=" + AspxFilter.C_TmpExpireValue);
      }

      /// <summary>
      /// 重定向页面
      /// </summary>
      /// <param name="page">重定向到的页面 URL（相对 ApplicationPath）</param>
      private void Redirect(String page) 
      {
         Response.Redirect(Request.ApplicationPath + page, true);
      }

   }
}
