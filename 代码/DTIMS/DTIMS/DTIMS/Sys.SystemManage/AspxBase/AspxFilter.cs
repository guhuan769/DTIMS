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
   /// Aspx ������������ҳ����˺͵�¼���ơ�
   /// </summary>
   public class AspxFilter : System.Web.UI.Page, System.Web.SessionState.IRequiresSessionState
   {

      /// <summary>
      /// ���ýڵ�����
      /// </summary>
      public const String C_ConfigSection  = "loginInfo";

      /// <summary>
      /// �ַ���ֵ���ڵķָ���
      /// </summary>
      public const Char   C_ValueNodeSep   = ':';

      /// <summary>
      /// �ַ���ֵ�и����ֵ�ķָ���
      /// </summary>
      public const Char   C_ValueClassSep = ',';

      /// <summary>
      /// Ĭ�ϵ�¼ҳ URL����� ApplicationPath
      /// </summary>
      public const String C_DefLoginPage   = "/Login.aspx";

      /// <summary>
      /// Ĭ�ϵĵ�¼����ڵ� Session ����
      /// </summary>
      public const String C_DefSessionName = "UserID";

      /// <summary>
      /// Ĭ�Ϲ���ҳ�� URL����� ApplicationPath
      /// </summary>
      public const String C_DefExpirePage  = "/Expire.html";

      /// <summary>
      /// ָ����¼���ڵ� Session ���������ý�����
      /// </summary>
      public const String C_SessionNameSec = "sessionNames";

      /// <summary>
      /// ָ�����Ե�¼��ҳ������ý�����
      /// </summary>
      public const String C_ExludePagesSec = "exludePages";

      /// <summary>
      /// ָ����¼ҳ�����ý�����
      /// </summary>
      public const String C_LoginPageSec   = "loginPage";

      /// <summary>
      /// ָ������ҳ������ý�����
      /// </summary>
      public const String C_ExpirePageSec  = "expirePage";

      /// <summary>
      /// ָ��ӳ������ý�����
      /// </summary>
      public const String C_MappingSec     = "mappings";

      /// <summary>
      /// ָʾ Session ���ڵĲ�����
      /// </summary>
      public const String C_TmpExpireName  = "_AF_IsExpired";

      /// <summary>
      /// ָʾ Session ���ڵĲ���ֵ
      /// </summary>
      public const String C_TmpExpireValue = "1";

      private AspxFilter() 
      {
      }

      /// <summary>
      /// �жϵ�ǰ��¼ Session �Ƿ��Ѿ����ڡ�
      /// </summary>
      /// <remarks>
      /// Ĭ���жϵ� Session ������ UserID��
      /// ������ Web.config ������ sessionNames ��ֵΪ��Ҫ�жϵ� Session ������
      /// �� (:) �ָ����ֵ��
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
      /// ��ǰ����ҳ�� URL ����� ApplicationPath����
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
      /// �ж������ҳ�Ƿ�������Ե�¼��
      /// ������ Web.config ������ exludePages ��ֵΪ���Ե�¼��ҳ��
      /// �� (:) �ָ����ֵ��
      /// ��ֵ����� ApplicationPath �ġ�
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
      /// �ж������ҳ�Ƿ�Ϊ��¼ҳ��
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
      /// ��ȡ��¼ҳ���URL�����ApplicationPath����
      /// Ĭ�ϵ�¼ҳ�� /Login.aspx��
      /// ������ Web.config ������ loginPage ��ֵΪ��¼ҳ��
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
      /// �������ļ���ȡ��ǰ����ҳ�Ĵ������
      /// ��������ڷ��� null��
      /// </summary>
      /// <remarks>
      /// ������ Web.config ��Ϊ mappings ����ֵָ�� URL ӳ��Ĵ����༰���������򼯣�
      /// ���Կո�ͻس����з�
      /// �� (:) �ָ����ӳ�䣬ÿ��ӳ������ (,) �ָ� URL ��������ͳ��򼯡�
      /// URL ����� ApplicationPath ��
      /// ��Ҫָ��ȫ��
      /// </remarks>
      /// <returns>null �� System.Web.UI.Page ����</returns>
      private Object GetMapping()
      {

         // ȡ mappings ֵ
         String s = HtmlUtil.getConfigSection(
            AspxFilter.C_ConfigSection, AspxFilter.C_MappingSec);
         if (s == null || s.Length == 0) 
         {
            return null;
         }

         // ���Կո�ͻس����з�
         s = s.Replace(" ", "");
         s = s.Replace("\r", "");
         s = s.Replace("\n", "");

         // Ϊ mappings ֵ��ǰ���ָ���
         if (!s.StartsWith(""+AspxFilter.C_ValueNodeSep)) 
         {
            s = AspxFilter.C_ValueNodeSep + s;
         }

         // Ϊ mappings ֵ�Ӻ�׺�ָ���
         if (!s.EndsWith(""+AspxFilter.C_ValueNodeSep)) 
         {
            s += AspxFilter.C_ValueNodeSep;
         }

         // ��ȡӳ��λ��
         String pathNode = AspxFilter.C_ValueNodeSep + this.RequestPath + AspxFilter.C_ValueClassSep;
         int idx = s.IndexOf(pathNode);
         if (idx < 0) 
         {
            return null;
         }

         // ��ȥ������ǰ�Ĵ�
         s = s.Substring(idx + pathNode.Length);

         // ��ȡ������Ľ���λ��
         idx = s.IndexOf(AspxFilter.C_ValueClassSep);
         if (idx < 0) 
         {
            return null;
         }

         // �������ȫ��
         String typeName = s.Substring(0, idx);

         // ��ȥ������
         s = s.Substring(idx + 1);

         // ��ȡ���򼯵Ľ���λ��
         idx = s.IndexOf(AspxFilter.C_ValueNodeSep);
         if (idx < 0) 
         {
            return null;
         }

         // ������
         String aName = s.Substring(0, idx);

         // ��ȡ����������ҳ�Ĵ������ʵ������ʵ��ʹ��Ĭ�Ϲ�����������
         Object o = Activator.CreateInstance(aName, typeName);
         return ((System.Runtime.Remoting.ObjectHandle)o).Unwrap();

      }

      /// <summary>
      /// ���
      /// </summary>
      protected override void FrameworkInitialize() 
      {
         // �����ǰҳ�Ȳ��ǵ�¼ҳ���ֲ��Ǻ��Ե�¼��ҳ������ Session �ѹ��ڣ���ת����¼ҳ��
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
      /// ִ��������ҳ������
      /// </summary>
      private void ProcessRequest() 
      {

         // ���ҳ���ѹ��ڣ�ת��Ĺ�����ʾҳ�档
         // ������ Web.config ������ expirePage ��ֵΪҪת���ҳ�档
         if (this.IsLoginPage) 
         {

            // ��ȡ����ָʾ����
            String tmpSession = HtmlUtil.queryString(Request, AspxFilter.C_TmpExpireName);
            if (tmpSession.Equals(AspxFilter.C_TmpExpireValue)) 
            {
               // �������ļ���ȡ�������ҳ URL
               String expirePage = HtmlUtil.getConfigSection(
                  AspxFilter.C_ConfigSection, AspxFilter.C_ExpirePageSec);

               // ���û�����ù���ҳ�棬ʹ��Ĭ�Ϲ���ҳ��
               if (expirePage == null || expirePage.Equals("")) 
               {
                  expirePage = AspxFilter.C_DefExpirePage;
               }

               // ת�����ҳ��
               this.Redirect(expirePage);
            } 
            else 
            {
               // ���������ҳ�档
               this.GetToPage().ProcessRequest(this.Context);
            }
         } 
         else 
         {
            // ���������ҳ�档
            this.GetToPage().ProcessRequest(this.Context);
         }

      }

      /// <summary>
      /// ��ȡ�Ե�ǰ������Ӧ��ҳ��ʵ����
      /// </summary>
      /// <returns></returns>
      private System.Web.UI.Page GetToPage() 
      {
         // �������������ҳ�Ĵ����࣬���ش������ʵ��
         Object o = this.GetMapping();
         if (o != null) 
         {
            return (System.Web.UI.Page)o;
         }

         // ��ȡ����������ҳ��ʵ��
         System.Web.IHttpHandler ihh = System.Web.UI.PageParser.GetCompiledPageInstance(
            Request.CurrentExecutionFilePath, 
            Request.PhysicalApplicationPath + this.RequestPath.Substring(1).Replace('/', '\\'), 
            this.Context);

         return (System.Web.UI.Page)ihh;
      }

      /// <summary>
      /// �ض��򵽵�¼ҳ��
      /// ��� URL ���� _AF_IsExpired=1 ָʾ Session ����
      /// </summary>
      private void ToLoginPage() 
      {
         Redirect(this.LoginPage + "?" + AspxFilter.C_TmpExpireName + "=" + AspxFilter.C_TmpExpireValue);
      }

      /// <summary>
      /// �ض���ҳ��
      /// </summary>
      /// <param name="page">�ض��򵽵�ҳ�� URL����� ApplicationPath��</param>
      private void Redirect(String page) 
      {
         Response.Redirect(Request.ApplicationPath + page, true);
      }

   }
}
