using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace JFIMS
{
    /// <summary>
    /// ��������:ȫ�ֳ���Ӧ����,�紦��Sqlע��Ⱦ���Щ�����
    /// </summary>
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //StartProcessRequest();
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            try
            {
                //���»�ȡSESSION,ͨ��cookie�е�sessionid����ȡ
                string session_param_name = "ASPSESSID";
                string session_cookie_name = "ASP.NET_SESSIONID";

                if (HttpContext.Current.Request.Form[session_param_name] != null)
                {
                    UpdateCookie(session_cookie_name, HttpContext.Current.Request.Form[session_param_name]);
                }
                else if (HttpContext.Current.Request.QueryString[session_param_name] != null)
                {
                    UpdateCookie(session_cookie_name, HttpContext.Current.Request.QueryString[session_param_name]);
                }
            }
            catch (Exception)
            {
            }

            try  
            {
                //Membership��FormsAuthentication��֤,��AUTHIDҲ����SessionID�ķ������д���
                string auth_param_name = "AUTHID";  
                string auth_cookie_name = FormsAuthentication.FormsCookieName;  
  
                if (HttpContext.Current.Request.Form[auth_param_name] != null)  
                {  
                    UpdateCookie(auth_cookie_name, HttpContext.Current.Request.Form[auth_param_name]);  
                }  
                else if (HttpContext.Current.Request.QueryString[auth_param_name] != null)  
                {  
                    UpdateCookie(auth_cookie_name, HttpContext.Current.Request.QueryString[auth_param_name]);  
                }  
 
           }  
            catch (Exception)  
            {  
            }  

            StartProcessRequest();
          
        }

        #region SQLע��ʽ�����������
        ///  <summary> 
        /// ��������:�����û��ύ������ 
        ///  </summary> 
        private void StartProcessRequest()
        {
            try
            {
                string getkeys = "";
                string sqlErrorPage = "App/Common/error.aspx";//ת��Ĵ�����ʾҳ�� 
                if (System.Web.HttpContext.Current.Request.QueryString != null)
                {

                    for (int i = 0; i < System.Web.HttpContext.Current.Request.QueryString.Count; i++)
                    {
                        getkeys = System.Web.HttpContext.Current.Request.QueryString.Keys[i];
                        if (!ProcessSqlStr(System.Web.HttpContext.Current.Request.QueryString[getkeys]))
                        {
                            System.Web.HttpContext.Current.Response.Redirect(sqlErrorPage);
                            System.Web.HttpContext.Current.Response.End();
                        }
                    }
                }
                if (System.Web.HttpContext.Current.Request.Form != null)
                {
                    for (int i = 0; i < System.Web.HttpContext.Current.Request.Form.Count; i++)
                    {
                        getkeys = System.Web.HttpContext.Current.Request.Form.Keys[i];
                        if (getkeys == "__VIEWSTATE") continue;
                        if (!ProcessSqlStr(System.Web.HttpContext.Current.Request.Form[getkeys]))
                        {
                            System.Web.HttpContext.Current.Response.Redirect(sqlErrorPage);
                            System.Web.HttpContext.Current.Response.End();
                        }
                    }
                }
            }
            catch
            {
                // ������: �����û��ύ��Ϣ! 
            }
        }
        ///  <summary> 
        /// ��������:�����û������Ƿ����� '
        ///  </summary> 
        ///  <param name="Str">�����û��ύ���� </param> 
        ///  <returns>�����Ƿ���SQLע��ʽ�������� </returns> 
        private bool ProcessSqlStr(string Str)
        {
            bool ReturnValue = true;
            try
            {
                if (Str.Trim() != "")
                {
                    string SqlStr = "and |exec |insert |select |delete |update |count |* |chr |mid |master |truncate |char |declare";

                    string[] anySqlStr = SqlStr.Split('|');
                    foreach (string ss in anySqlStr)
                    {
                        if (Str.ToLower().IndexOf(ss) >= 0)
                        {
                            ReturnValue = false;
                            break;
                        }
                    }
                }
            }
            catch
            {
                ReturnValue = false;
            }
            return ReturnValue;
        }


        void UpdateCookie(string cookie_name, string cookie_value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
            if (cookie == null)
            {
                HttpCookie cookie1 = new HttpCookie(cookie_name, cookie_value);
                Response.Cookies.Add(cookie1);
            }
            else
            {
                cookie.Value = cookie_value;
                HttpContext.Current.Request.Cookies.Set(cookie);
            }

        }
        #endregion
    }
}