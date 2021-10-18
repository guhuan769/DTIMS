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
	/// LogInfo ��ժҪ˵����
	/// </summary>
	public partial class LogInfo : BJ.AspxTask.WebPageBase
	{
      private string SystemInfoId = null;
      private string operMode = null;

      private void Page_Load(object sender, System.EventArgs e)
      {
         // �ڴ˴������û������Գ�ʼ��ҳ��
         this.SystemInfoId = Request.QueryString["Num"];

         #region ��ʹ��
         if (!Page.IsPostBack)
         {
            try
            {
               //�鿴					
                BJ.SysLog.Descent.SysLog sysLog = new BJ.SysLog.Descent.SysLog(this.SystemInfoId);
               this.labelLogID.Text = sysLog.LogID.Trim();
               this.labelOprLogin.Text = sysLog.OprLogin.Trim();

               this.labelDate.Text = Convert.ToDateTime(sysLog.Date.ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
               if (sysLog.Mode.Trim() == "A" || sysLog.Mode.Trim() == "a")
               {
                  operMode = "���";
               }
               else if (sysLog.Mode.Trim() == "D" || sysLog.Mode.Trim() == "d")
               {
                  operMode = "ɾ��";
               }
               else if (sysLog.Mode.Trim() == "U" || sysLog.Mode.Trim() == "u")
               {
                  operMode = "�޸�";
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

      #region Web ������������ɵĴ���
      override protected void OnInit(EventArgs e)
      {
         //
         // CODEGEN: �õ����� ASP.NET Web ���������������ġ�
         //
         InitializeComponent();
         base.OnInit(e);
      }

      /// <summary>
      /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
      /// �˷��������ݡ�
      /// </summary>
      private void InitializeComponent()
      {
         

      }
      #endregion
	}
}
