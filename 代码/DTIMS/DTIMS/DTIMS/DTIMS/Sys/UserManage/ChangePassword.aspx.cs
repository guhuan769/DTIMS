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

using BJ.Project.Common;
using BJ.DTIMS.Common;

namespace Inphase.Project.CTQS.Descent
{
    /// <summary>
    /// ChangePassword ��ժҪ˵����
    /// </summary>
    public partial class ChangePassword : BJ.AspxTask.WebPageBase
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��
            //Operator oper = (Operator)Session["Operator"];
            //String temp = Request.Form["actionNo"];

            //try
            //{
            //    if(temp == "1")
            //    {
            //        //��ʼ�޸�
            //        String oldPwd = this.textBoxOldPwd.Value.Trim();
            //        String newPwd = this.textBoxNewPwd.Value.Trim();
            //        String newCon = this.textBoxConPwd.Value.Trim();

            //        OperatorPersistent opper = new OperatorPersistent(oper.OperatorId);
            //        if(opper.OPER_Password != oldPwd)
            //        {
            //            throw(new Exception("��ľ����벻��ȷ��"));
            //        }
            //        if(newPwd != newCon)
            //        {
            //            throw(new Exception("�������ȷ�����벻һ�£�"));
            //        }
            //        opper.OPER_Password = newPwd;
            //        opper.ChangePassword(this.MainDataBaseName,newPwd);
            //        this.ShowMessage(this.Page,"�޸ĳɹ���");
            //    }
            //}
            //catch(Exception et)
            //{
            //    this.ShowMessage(this.Page,et.Message);
            //}
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

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                //��ʼ�޸�
                String oldPwd = this.textBoxOldPwd.Value.Trim();
                String newPwd = this.textBoxNewPwd.Value.Trim();
                String newCon = this.textBoxConPwd.Value.Trim();

                oldPwd = BJ.Sys.Comm.EnDeCrypt.Encrypt(oldPwd);//�����������
                BJ.OperatorPersistent.Descent.OperatorPersistent opper = new BJ.OperatorPersistent.Descent.OperatorPersistent(this.Oper.OperatorId);
                if (opper.OPER_Password != oldPwd)
                {
                    throw (new Exception("��ľ����벻��ȷ��"));
                }
                if (newPwd != newCon)
                {
                    throw (new Exception("�������ȷ�����벻һ�£�"));
                }

                newPwd = BJ.Sys.Comm.EnDeCrypt.Encrypt(newPwd);//�����������
                opper.OPER_Password = newPwd;
                opper.ChangePassword(this.MainDataBaseName, newPwd);

                BJ.SysLog.Descent.SysLog.Add(this.Oper.OperatorId, BJ.SysLog.Descent.SysLog.LogMode.UpDate, BJ.SysLog.Descent.SysLog.LogItemID.OperatorManage, "�����޸ĳɹ���", this.Oper.Client_IP);

                BJ.DTIMS.Common.Common.ScriptManagerRegister(this.UpdatePanel1,
                    JqueryComm.ShowMessage("��ʾ", "�޸ĳɹ�", MessageType.info));
            }
            catch (Exception et)
            {
                BJ.SysLog.Descent.SysLog.Add(this.Oper.OperatorId, BJ.SysLog.Descent.SysLog.LogMode.UpDate, BJ.SysLog.Descent.SysLog.LogItemID.OperatorManage, string.Concat("�����޸�ʧ�ܣ�", et.Message), this.Oper.Client_IP);
                BJ.DTIMS.Common.Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("����", "�޸Ĵ�����ϸ��" + et.Message, MessageType.error));
            }
        }
    }
}
