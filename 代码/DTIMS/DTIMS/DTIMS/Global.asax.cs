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
    /// 功能描述:全局程序应用类,如处理Sql注入等就在些类完成
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
                //重新获取SESSION,通过cookie中的sessionid来获取
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
                //Membership的FormsAuthentication验证,把AUTHID也按照SessionID的方法进行处理
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

        #region SQL注入式攻击代码分析
        ///  <summary> 
        /// 功能描述:处理用户提交的请求 
        ///  </summary> 
        private void StartProcessRequest()
        {
            try
            {
                string getkeys = "";
                string sqlErrorPage = "App/Common/error.aspx";//转向的错误提示页面 
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
                // 错误处理: 处理用户提交信息! 
            }
        }
        ///  <summary> 
        /// 功能描述:分析用户请求是否正常 '
        ///  </summary> 
        ///  <param name="Str">传入用户提交数据 </param> 
        ///  <returns>返回是否含有SQL注入式攻击代码 </returns> 
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