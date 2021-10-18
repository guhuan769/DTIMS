using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

namespace Inphase.CTQS
{
    public partial class Main : BJ.AspxTask.WebPageBase
    {
        #region 全局变量
        private readonly static Regex m_replaceUrlReg = new Regex(@"\.\./");
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // 禁止注销登录后，IE后退按钮访问注销前页面。
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (this.Oper == null)
            {
                Response.Redirect("Login.aspx");
            }
        }

        #region 创建用户菜单
        /// <summary>
        /// 创建用户菜单。
        /// </summary>
        /// <returns></returns>
        protected string CreateUserMenu()
        {
            StringBuilder build = new StringBuilder();
            build.Append("<div class=\"content-header\">");
            build.Append("<div class=\"marquee-container\">");
            build.Append("<div class=\"marquee-inner\">");
            build.Append("<div id=\"inphase_marquee\">");
            //build.Append("&nbsp;");
            build.Append("</div>");
            build.Append("</div>");
            build.Append("</div>");
            build.Append("<div class=\"menu-container\">");
            build.Append(string.Format("<span>您好，{0}</span>&nbsp;&nbsp;", (Oper != null ? Oper.OperName : "访客")));
            if (Oper != null)
            {
                build.Append("<a class=\"menuAnchor\" href=\"javascript:void(0);\" onclick=\"javascript:return userOper(this.innerHTML);\" title=\"注销登录\">注销</a>&nbsp;");
                build.Append("<a class=\"menuAnchor\" href=\"javascript:void(0);\" onclick=\"javascript:return userOper(this.innerHTML);\" title=\"退出系统\">退出</a>&nbsp;");
            }
            else
            {
                build.Append("<a class=\"menuAnchor\" href=\"javascript:void(0);\" onclick=\"javascript:return userOper(this.innerHTML);\" title=\"登录系统\">登录</a>&nbsp;");
            }
            build.Append("</div>");
            build.Append("<div class=\"float: none; clear: both;\"></div>");
            build.Append("</div>");
            return build.ToString();
        }
        #endregion

        #region 创建左边菜单
        /// <summary>
        /// 创建左边菜单。
        /// </summary>
        /// <returns></returns>
        protected string CreateLeftMenu()
        {
            if (Oper == null)
            {
                return "";
            }

            StringBuilder build = new StringBuilder();  // 生成菜单的HTML字符串。
            DataTable dtFunc = BJ.Project.Common.Sys_FunctionItem.GetUserFunctionItem(Oper);

            //生成菜单
            DataRow[] rowMenu = dtFunc.Select("FunCategory_ID=1");
            foreach (DataRow dr in rowMenu)
            {
                //根据当前的大类查找子功能菜单
                DataRow[] rowSub = dtFunc.Select("Fun_Url <> '' AND Fun_ParentID=" + dr["Fun_ID"].ToString().Trim(), "Fun_Sort ASC");

                //如果有子功能菜单则加载主菜单
                if (rowSub.Length > 0)
                {
                    build.Append("<div title=\"");
                    build.Append(dr["Fun_Name"].ToString().Trim());
                    build.Append("\"");
                    build.Append(" style=\"padding: 0;\">");
                }
                else
                {
                    continue;
                }

                //生成子菜单
                object ulCssClass = dr["Fun_CssClass"] = "leftMenu";   // 菜单主样式。
                object liCssClass = null;
                foreach (DataRow drSub in rowSub)
                {
                    liCssClass = drSub["Fun_CssClass"];     // 具体菜单项的样式。
                    build.Append("<ul");
                    if (ulCssClass != null && !string.IsNullOrEmpty(ulCssClass.ToString()))
                    {
                        build.Append(" class=\"");
                        build.Append(ulCssClass.ToString());
                        build.Append("\"");
                    }
                    build.Append(">");
                    build.Append("<li url=\"");
                    build.Append(ReplaceUrlPrefix(drSub["Fun_Url"].ToString().Trim()));
                    build.Append("\" title=\"");
                    build.Append(drSub["Fun_Desc"].ToString().Trim());
                    build.Append("\"");
                    if (liCssClass != null && !string.IsNullOrEmpty(liCssClass.ToString()))
                    {
                        build.Append(" class=\"");
                        build.Append(liCssClass.ToString());
                        build.Append("\"");
                    }
                    build.Append(">");
                    build.Append(drSub["Fun_Name"].ToString().Trim());
                    build.Append("</li>");
                    build.Append("</ul>");
                }
                build.Append("</div>");
            }
            return build.ToString();
        }
        #endregion

        #region 替换数据库菜单项URL的“../”前缀。
        /// <summary>
        /// 替换数据库菜单项URL的“../”前缀，除了系统菜单都加上“App/”前缀返回。
        /// </summary>
        /// <param name="url">原始URL。</param>
        /// <returns>处理后的URL。</returns>
        private string ReplaceUrlPrefix(string url)
        {
            string temp = m_replaceUrlReg.Replace(url, "");
            temp = temp.StartsWith("sys/", StringComparison.OrdinalIgnoreCase) ? temp : "App/" + temp;
            return temp;
        }
        #endregion
    }
}