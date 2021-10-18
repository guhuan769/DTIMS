using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DTIMS.Comm;
using Sys.Project.Common;

namespace DTIMS.Web
{
    public partial class PrivilegeEdit :WebPageBase
    {
        private string privGroup_ID = "0";            //＝＝０表示新增
        private Hashtable htFuns = new Hashtable(); //当前权限组的权限ID集合
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //绑定地区信息
                    Sys.Project.Common.AreaInfo area = new Sys.Project.Common.AreaInfo(this.Oper.Area_ID);
                    this.lblAreaName.Text = area.MainArea_Name;

                    htFuns = new Hashtable();
                    privGroup_ID = Request.QueryString["PrivGroup_ID"];
                    LoadFuncTree();

                    //this.trvFunctionItem.Attributes.Add("onclick", "postBackByObject()");

                    this.trvFunctionItem.Attributes.Add("onclick", "CheckEvent()");

                    if (privGroup_ID != "0")
                    {
                        //判断是否是自己添加的权限组,并且设置能否修改
                        string createUserID = Request.QueryString["createUserID"];
                        if (this.Oper.IsSuper || this.Oper.OperatorId.Trim() == createUserID.Trim())
                        {
                            this.btnAdd.Visible = true;
                            this.txtPrivGroup_Desc.Enabled = true;
                            this.txtPrivGroup_Name.Enabled = true;
                        }
                        else
                        {
                            this.btnAdd.Visible = false;
                            this.txtPrivGroup_Desc.Enabled = false;
                            this.txtPrivGroup_Name.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DTIMS.Comm.Common.ScriptManagerRegister(this.UpdatePanel1,
                    JqueryComm.ShowMessage("错误", "初始化页面出错,详细:" + ex.Message, MessageType.error));
            }
        }

        #region 加载当前用户的所有权限
        private void LoadFuncTree()
        {
            if (privGroup_ID == "0")//新增
            {
                this.trvFunctionItem.Nodes.Add(PrivilegeTree.CreatePrivilegeTree(this.Oper));
            }
            else//修改权限组
            {
                this.trvFunctionItem.Nodes.Add(PrivilegeTree.CreatePrivilegeTree(privGroup_ID, this.Oper));

                Sys_PrivilegeGroup group = new Sys_PrivilegeGroup(privGroup_ID);
                this.txtPrivGroup_Name.Text = group.PrivGroup_Name;
                this.txtPrivGroup_Desc.Text = group.PrivGroup_Desc;
            }
        }

        #endregion

        #region 新增或者修改
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string prompt = "";
            try
            {
                privGroup_ID = Request.QueryString["PrivGroup_ID"];
                Sys_PrivilegeGroup group = new Sys_PrivilegeGroup();
                if (privGroup_ID == "0")//新增
                {
                    prompt = "新增权限组";
                    group.PrivGroup_ID = null;
                }
                else
                {
                    prompt = "修改权限组";
                    group.PrivGroup_ID = privGroup_ID;
                }
                group.PrivGroup_Name = this.txtPrivGroup_Name.Text.Trim();
                group.PrivGroup_Desc = this.txtPrivGroup_Desc.Text.Trim();

                //获取勾选的功能项
                Hashtable htSelectedFunc = new Hashtable();
                TreeNodeCollection selectedNodes = this.trvFunctionItem.CheckedNodes;
                foreach (TreeNode node in selectedNodes)
                {
                    htSelectedFunc.Add(node.Value.Trim(), node.Value);
                }

                group.Update(htSelectedFunc, this.Oper);

                DTIMS.Comm.Common.ScriptManagerRegister(this.UpdatePanel1, string.Format("prompt('{0}')", prompt + "成功"));

                //写成功日志
                prompt += "：" + this.txtPrivGroup_Name.Text.Trim() ;
              DTIMS.DataAcess.Common.SysLog.Add(this.Oper.OperatorId, DTIMS.DataAcess.Common.SysLog.LogMode.UpDate, DTIMS.DataAcess.Common.SysLog.LogItemID.RightManage, prompt + "成功", this.Oper.Client_IP);
            }
            catch (Exception ex)
            {
                //记录错误日志
              DTIMS.DataAcess.Common.SysLog.Add(this.Oper.OperatorId, DTIMS.DataAcess.Common.SysLog.LogMode.UpDate, DTIMS.DataAcess.Common.SysLog.LogItemID.RightManage, prompt + "失败", this.Oper.Client_IP);
                DTIMS.Comm.Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("错误", prompt + "出错," + ex.Message, MessageType.error));
            }
        }
        #endregion 
    }//end public partial class PrivilegeEdit : Inphase.AspxTask.WebPageBase
}//end namespace CTQS.Sys.PrivilegeManage
