using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;
using BJ.Project.Common;

namespace Inphase.Project.CTQS.Descent
{
	/// <summary>
	/// LogInfo 的摘要说明。
	/// </summary>
	public partial class LogInfo : BJ.AspxTask.WebPageBase
	{
      private string SystemInfoId = null;
      private string operMode = null;

      private void Page_Load(object sender, System.EventArgs e)
      {
         // 在此处放置用户代码以初始化页面
         this.SystemInfoId = Request.QueryString["Num"];

         #region 初使化
         if (!Page.IsPostBack)
         {
            try
            {
               //查看					
                BJ.SysLog.Descent.SysLog sysLog = new BJ.SysLog.Descent.SysLog(this.SystemInfoId);
               this.labelLogID.Text = sysLog.LogID.Trim();
               this.labelOprLogin.Text = sysLog.OprLogin.Trim();

               this.labelDate.Text = Convert.ToDateTime(sysLog.Date.ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
               if (sysLog.Mode.Trim() == "A" || sysLog.Mode.Trim() == "a")
               {
                  operMode = "添加";
               }
               else if (sysLog.Mode.Trim() == "D" || sysLog.Mode.Trim() == "d")
               {
                  operMode = "删除";
               }
               else if (sysLog.Mode.Trim() == "U" || sysLog.Mode.Trim() == "u")
               {
                  operMode = "修改";
               }
               this.labeloperMode.Text = this.operMode;
               this.labelItemName.Text = sysLog.ItemName.Trim();
               this.textContent.Value = sysLog.Content.ToString().Trim();
               this.lblLOG_Client_IP.Text = sysLog.LOG_Client_IP;
            }
            catch (Exception et)
            {
               this.ShowMessage(this.Page,et.Message);
            }
         }
         #endregion
      }

      #region Web 窗体设计器生成的代码
      override protected void OnInit(EventArgs e)
      {
         //
         // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
         //
         InitializeComponent();
         base.OnInit(e);
      }

      /// <summary>
      /// 设计器支持所需的方法 - 不要使用代码编辑器修改
      /// 此方法的内容。
      /// </summary>
      private void InitializeComponent()
      {
         

      }
      #endregion
	}
}
