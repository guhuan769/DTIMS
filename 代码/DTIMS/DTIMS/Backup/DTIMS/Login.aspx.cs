using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Sys.Project.Common;
using Sys.EasyUiEx;
using DTIMS.Comm.Helper;
using Sys.Com.Common;
using DTIMS.Web;

namespace DTIMS.Web
{
    public partial class Login  :WebPageBase
    {
        private const string cookiesName = "DTIMS_UserName";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpCookie ck = Request.Cookies[cookiesName];
                if (ck != null)
                {
                    textUserName.Value = HttpUtility.UrlDecode(ck.Value.Trim());
                    textPwd.Focus(); //将输入焦点设置到密码框
                }
                else
                {
                    textUserName.Focus();
                }
            }
            string temp = Request.Params["action"];
            switch (temp)
            {
                case "login":
                    LoginStart();
                    break;
                case "logout":
                    LoginOut();
                    break;
                case "exit":
                    Exit();
                    break;
                //default:
                //    // 如果用户已经登录，则跳转到首页。
                //    if (this.Oper != null)
                //    {
                //        this.Form1.Style[HtmlTextWriterStyle.Display] = "none";
                //        this.ClientScript.RegisterStartupScript(this.GetType(), "returnHome" + Guid.NewGuid(), "top.location.replace('" + Request.ApplicationPath + "');", true);
                //    }
                //    break;
            }
        }

        private void LoginStart()
        {
            try
            {
                string name = StringHelper.Unescape(Request.Params["user"]);
                string pwd = StringHelper.Unescape(Request.Params["pwd"]);
                Operator oper = OperatorFactory.OperatorCreate(name, pwd);

                if (oper != null)
                {
                    oper.Client_IP = Request.UserHostAddress;
                    Session["Operator"] = oper;

                    Sys.Comm.Common.WriteCookies(SystemWebFormulation.Parameter("OperatorId"),
                                        oper.OperatorId);
                    FormsAuthentication.SetAuthCookie(name, false);

                    //登录成功并设置Cookies
                    HttpCookie newcookie = new HttpCookie(cookiesName);
                    newcookie.Value = HttpUtility.UrlEncode(name);
                    newcookie.Expires = DateTime.Now.AddDays(365); //默认有效期为一年
                    Response.AppendCookie(newcookie);

                    DTIMS.DataAcess.Common.SysLog.Add(oper.OperatorId, DTIMS.DataAcess.Common.SysLog.LogMode.Insert,
                     DTIMS.DataAcess.Common.SysLog.LogItemID.AreaManage, "登录成功！", oper.Client_IP);
                    return;
                }
                else
                {
                    this.EasyUi.Alert(false, "管理员不存在，或者已经被禁用！", IconEnum.Warning);
                    return;
                }
            }
            catch (System.Threading.ThreadAbortException)
            {

            }
            catch (Exception ex)
            {
                this.EasyUi.Alert(false, ex.Message, IconEnum.Error);
                return;
            }
        }

        private void LoginOut()
        {
            Session.RemoveAll();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("~");
        }

        private void Exit()
        {
            this.Form1.Style[HtmlTextWriterStyle.Display] = "none";
            Session.RemoveAll();
            Session.Abandon();
            FormsAuthentication.SignOut();
            //string script = "$.messager.alert('提示','您已经退出系统。','info',exitSystem);";//$(\".messager-window .panel-tool-close\").one(\"click\", function () {exitSystem();});";
            string script = "exitSystem();";
            script = this.CreateReadyScript(script);
            this.ClientScript.RegisterStartupScript(this.GetType(), "script" + Guid.NewGuid(), script, true);
        }

        /// <summary>
        /// 创建Jquery ready方法脚本。
        /// </summary>
        /// <param name="script">文档加载完成时执行的脚本。</param>
        /// <returns></returns>
        private string CreateReadyScript(string script)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("$(document).ready(");
            sb.Append("function(){");
            sb.Append(script);
            sb.Append("});");
            return sb.ToString();
        }

    }
}