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
    /// ChangePassword 的摘要说明。
    /// </summary>
    public partial class ChangePassword : BJ.AspxTask.WebPageBase
    {

        protected void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            //Operator oper = (Operator)Session["Operator"];
            //String temp = Request.Form["actionNo"];

            //try
            //{
            //    if(temp == "1")
            //    {
            //        //开始修改
            //        String oldPwd = this.textBoxOldPwd.Value.Trim();
            //        String newPwd = this.textBoxNewPwd.Value.Trim();
            //        String newCon = this.textBoxConPwd.Value.Trim();

            //        OperatorPersistent opper = new OperatorPersistent(oper.OperatorId);
            //        if(opper.OPER_Password != oldPwd)
            //        {
            //            throw(new Exception("你的旧密码不正确！"));
            //        }
            //        if(newPwd != newCon)
            //        {
            //            throw(new Exception("新密码和确认密码不一致！"));
            //        }
            //        opper.OPER_Password = newPwd;
            //        opper.ChangePassword(this.MainDataBaseName,newPwd);
            //        this.ShowMessage(this.Page,"修改成功！");
            //    }
            //}
            //catch(Exception et)
            //{
            //    this.ShowMessage(this.Page,et.Message);
            //}
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

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                //开始修改
                String oldPwd = this.textBoxOldPwd.Value.Trim();
                String newPwd = this.textBoxNewPwd.Value.Trim();
                String newCon = this.textBoxConPwd.Value.Trim();

                oldPwd = BJ.Sys.Comm.EnDeCrypt.Encrypt(oldPwd);//将旧密码加密
                BJ.OperatorPersistent.Descent.OperatorPersistent opper = new BJ.OperatorPersistent.Descent.OperatorPersistent(this.Oper.OperatorId);
                if (opper.OPER_Password != oldPwd)
                {
                    throw (new Exception("你的旧密码不正确！"));
                }
                if (newPwd != newCon)
                {
                    throw (new Exception("新密码和确认密码不一致！"));
                }

                newPwd = BJ.Sys.Comm.EnDeCrypt.Encrypt(newPwd);//将新密码加密
                opper.OPER_Password = newPwd;
                opper.ChangePassword(this.MainDataBaseName, newPwd);

                BJ.SysLog.Descent.SysLog.Add(this.Oper.OperatorId, BJ.SysLog.Descent.SysLog.LogMode.UpDate, BJ.SysLog.Descent.SysLog.LogItemID.OperatorManage, "密码修改成功！", this.Oper.Client_IP);

                BJ.DTIMS.Common.Common.ScriptManagerRegister(this.UpdatePanel1,
                    JqueryComm.ShowMessage("提示", "修改成功", MessageType.info));
            }
            catch (Exception et)
            {
                BJ.SysLog.Descent.SysLog.Add(this.Oper.OperatorId, BJ.SysLog.Descent.SysLog.LogMode.UpDate, BJ.SysLog.Descent.SysLog.LogItemID.OperatorManage, string.Concat("密码修改失败！", et.Message), this.Oper.Client_IP);
                BJ.DTIMS.Common.Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("错误", "修改错误，详细：" + et.Message, MessageType.error));
            }
        }
    }
}
