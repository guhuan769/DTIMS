using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DTIMS.Comm;
using Sys.Comm.WebTools;
using Sys.Project.Common;

namespace DTIMS.Web
{
    public partial class UserManageEdit :WebPageBase
    {
        private string type = null;
        Operator oper = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                type = Request.QueryString["type"];
                oper = (Operator)Session["Operator"];

                #region 页面初使化
                if (!Page.IsPostBack)
                {
                    ViewState.Add("ROLE_ID", "2");

                    InitRoleGroup(null);
                    InitAreaList();
                    #region 如果是修改，则初始化数据
                    if (type.Equals("add"))
                    {
                        this.spanMsg.InnerText = "，新增用户默认密码123456";
                        this.hidPwd.Value = "123456";
                        string areaId = Request.QueryString["areaId"];
                        if (!string.IsNullOrEmpty(areaId))
                        {
                            this.dropAreaLists.SelectedValue = areaId;
                        }
                    }
                    //设置角色
                    InitRule();
                    if (type.Equals("edit"))
                    {
                        //this.spanMsg.InnerText = "，密码为空，则不修改密码";
                        InitEdit();
                    }
                    if (type.Equals("view"))
                    {
                        InitEdit();
                        this.pwd1.Visible = false;
                        this.btnAdd.Visible = false;
                        this.showDis.Visible = false;
                    }

                    InitPrivateRole();
                    #endregion
                }
                this.trvFunctionItem.Attributes.Add("onclick", "CheckEvent()");
                #endregion

                Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.FormatScriptString("InitPage"));
            }
            catch (Exception ex)
            {
                this.JqueryMessager(this.UpdatePanel1, ex.Message);
            }
        }

        #region 初始化地区树
        private void InitAreaList()
        {
            TreeNode node = AreaTree.CreateAreaTree(oper.Area_ID);
            ListItem item = null;

            //根地区
            item = new ListItem();
            item.Text = node.Text;
            item.Value = node.Value;
            this.dropAreaLists.Items.Add(item);

            //如果有子地区，则继续添加
            foreach (TreeNode t in node.ChildNodes)
            {
                item = new ListItem();
                item.Text = "　├" + t.Text;
                item.Value = t.Value;
                this.dropAreaLists.Items.Add(item);
            }
        }
        #endregion

        #region 初始化添加用户的权限组
        private void InitRoleGroup(string tempAreaId)
        {
            string areaId = null;
            if (string.IsNullOrEmpty(tempAreaId))
            {
                areaId = Request.QueryString["areaId"];
            }
            else
            {
                areaId = tempAreaId;
            }

            if (string.IsNullOrEmpty(areaId))
                areaId = oper.Area_ID;
            DTIMS.Role.Descent.RoleGroup role = new DTIMS.Role.Descent.RoleGroup();
            DataTable table = role.ListAll(true, oper);
            this.checkListRole.DataSource = table;
            this.checkListRole.DataTextField = "PrivGroup_Name";
            this.checkListRole.DataValueField = "PrivGroup_ID";
            this.checkListRole.DataBind();
        }
        #endregion

        #region 初始化用户数据
        private void InitEdit()
        {
            string userid = Request.QueryString["id"];
            DTIMS.OperatorPersistent.Descent.OperatorPersistent editOper = new DTIMS.OperatorPersistent.Descent.OperatorPersistent(userid);

            this.TextBox1.Text = editOper.OPER_Login;
            this.txtUserName.Text = editOper.OPER_Name;
            ViewState.Add("ROLE_ID", editOper.ROLE_ID);
            string pwd = editOper.OPER_Password;

            if (editOper.OPER_Login.Equals("super"))
            {
                this.isSuperNo.Visible = false;
            }

            try
            {
                pwd = Sys.Comm.EnDeCrypt.Decrypt(pwd);
            }
            catch
            {
                pwd = "123456";
            }
            this.hidPwd.Value = pwd;

            this.TextBox2.Value = pwd;
            this.dropRule.SelectedValue = editOper.ROLE_ID;
            if (editOper.OPER_Status.Equals("0"))
            {
                this.ok.Checked = true;
            }
            else
            {
                this.no.Checked = true;
            }
            this.TextBox4.Text = editOper.OPER_Remark;

            #region 如果用户为super，则只能修改用户名称
            if (editOper.ROLE_ID.Equals("1") || userid == this.Oper.OperatorId)
            {
                this.TextBox1.Enabled = false;
                this.dropAreaLists.Enabled = false;
                this.ok.Enabled = false;
                this.no.Enabled = false;
                this.tt.Visible = false;
                DTIMS.Role.Descent.RoleGroup role = new DTIMS.Role.Descent.RoleGroup();
                DataTable table = role.GetSys_UserRole();
                dropRule.Items.Clear();
                dropRule.DataTextField = "UserRole_Name";
                dropRule.DataValueField = "UserRole_ID";
                dropRule.DataSource = table;
                dropRule.DataBind();
                this.dropRule.Enabled = false;
            }
            #endregion

            //地区
            this.dropAreaLists.SelectedValue = editOper.MainArea_ID;

            //私有组id
            string privateRoleID = null;
            foreach (string id in editOper.RoleList)
            {
                string[] split = id.Split('|');
                for (int i = 0; i < this.checkListRole.Items.Count; i++)
                {
                    if (this.checkListRole.Items[i].Value.Equals(split[0]))
                    {
                        this.checkListRole.Items[i].Selected = true;
                    }
                }
                if (split[1].Equals("0"))
                {
                    privateRoleID = split[0];
                }
            }

            this.hidPrivateID.Value = privateRoleID;

        }
        #endregion

        #region 设置用户私有组
        private void InitPrivateRole()
        {
            string privateRoleID = this.hidPrivateID.Value;

            /****************************************************
             ******************2012-03-28增加*******************
             ****************************************************/
            if (string.IsNullOrEmpty(privateRoleID))
            {
                privateRoleID = "0";
            }

            //设置用户私有组
            try
            {
                TreeNode node = null;
                if (type.Equals("add"))
                {
                    node = PrivilegeTree.CreatePrivilegeTree(this.oper);
                }
                else
                {
                    node =PrivilegeTree.CreatePrivilegeTree(privateRoleID, this.oper);
                }

                this.trvFunctionItem.Nodes.Add(node);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private void CheckUserFun(TreeNode node, Hashtable ht, ref Hashtable htRoleFunID)
        {
            //TreeNode parentNode = new TreeNode();
            foreach (TreeNode no in node.ChildNodes)
            {
                if (no.Checked)
                {
                    htRoleFunID.Remove(no.Value.Trim());
                }

                if (ht.ContainsKey(no.Value))
                {
                    no.Checked = true;
                }
                CheckUserFun(no, ht, ref htRoleFunID);
            }
            //return parentNode;
        }
        #endregion

        #region 保存数据
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            #region 用户数据
            try
            {
                DTIMS.OperatorPersistent.Descent.OperatorPersistent addOper = new DTIMS.OperatorPersistent.Descent.OperatorPersistent();
                addOper.OPER_Login = this.TextBox1.Text;
                addOper.OPER_Name = this.txtUserName.Text;
                addOper.MainArea_ID = this.dropAreaLists.SelectedValue;
                string pwd = this.TextBox2.Value.Replace('', ' ').Trim();
                if (string.IsNullOrEmpty(pwd))
                {
                    pwd = this.hidPwd.Value;
                }

                //验证密码长度
                if (Sys.Comm.WebTools.ObjectMath.IsOverLong(pwd, 20))
                {
                    throw new Exception("登陆密码最长不能超过20字符，或10个中文字符！");
                }

                pwd = Sys.Comm.EnDeCrypt.Encrypt(pwd);

                addOper.OPER_Password = pwd;//将密码加密

                addOper.ROLE_ID = this.dropRule.SelectedValue;
                addOper.MainArea_ID = dropAreaLists.SelectedValue;//使用父地区
                addOper.OPER_Status = this.ok.Checked ? "0" : "1";
                addOper.OPER_Remark = this.TextBox4.Text;

                ArrayList roleList = new ArrayList();
                foreach (ListItem item in this.checkListRole.Items)
                {
                    if (item.Selected)
                    {
                        roleList.Add(item.Value);
                    }
                }

                if (roleList.Count == 0 && ViewState["ROLE_ID"].ToString() != "1" && Request.QueryString["id"] != this.Oper.OperatorId)//必须选择一项权限
                {
                    DTIMS.Comm.Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("操作失败", "请至少选择一个权限组", MessageType.error));
                    return;
                }

                ArrayList funList = new ArrayList();
                TreeNodeCollection nodeList = this.trvFunctionItem.CheckedNodes;
                foreach (TreeNode node in nodeList)
                {
                    funList.Add(node.Value);
                }

                string type = Request.QueryString["type"];
                if (type.Equals("add"))
                {
                    try
                    {
                        addOper.Update(roleList, funList, null, false);
                      DTIMS.DataAcess.Common.SysLog.Add(oper.OperatorId, DTIMS.DataAcess.Common.SysLog.LogMode.Insert,
                       DTIMS.DataAcess.Common.SysLog.LogItemID.OperatorManage, "添加操作员：" + addOper.OPER_Name, this.Oper.Client_IP);
                        DTIMS.Comm.Common.ScriptManagerRegister(this.UpdatePanel1, string.Format("prompt('{0}')", "添加用户成功"));
                    }
                    catch (Exception ex)
                    {
                        DTIMS.Comm.Common.ScriptManagerRegister(this.UpdatePanel1, string.Format("error('{0}')", "添加用户失败。" + ex.Message));
                        //设置角色
                        InitRule();
                        InitRoleGroup(null);
                    }
                }
                else if (type.Equals("edit"))
                {
                    try
                    {
                        addOper.OPER_ID = Request.QueryString["id"];
                        //addOper.ROLE_ID = ViewState["ROLE_ID"].ToString();
                        if (addOper.OPER_ID == this.Oper.OperatorId)
                        {
                            addOper.Update(roleList, funList, this.hidPrivateID.Value, true);
                        }
                        else
                        {
                            addOper.Update(roleList, funList, this.hidPrivateID.Value, false);
                        }

                      DTIMS.DataAcess.Common.SysLog.Add(oper.OperatorId, DTIMS.DataAcess.Common.SysLog.LogMode.UpDate, DTIMS.DataAcess.Common.SysLog.LogItemID.OperatorManage, "修改操作员：" + addOper.OPER_Name, this.Oper.Client_IP);

                        DTIMS.Comm.Common.ScriptManagerRegister(this.UpdatePanel1, string.Format("prompt('{0}')", "修改用户成功"));
                        //Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("提示", "修改用户 \"" + addOper.OPER_Name + "\" 成功", MessageType.info, "editSuccess"));
                    }
                    catch (Exception ex)
                    {
                        DTIMS.Comm.Common.ScriptManagerRegister(this.UpdatePanel1, string.Format("error('{0}')", "修改用户失败。" + ex.Message));
                        //设置角色
                        InitRule();
                        InitRoleGroup(null);
                    }
                }
            }
            catch (Exception ex)
            {
                this.JqueryMessager(this.UpdatePanel1, "操作失败，详细：" + ex.Message, Sys.Comm.MessageType.error);
            }

            #endregion
        }
        #endregion

        #region 初始化权限组
        private void InitRule()
        {
            DTIMS.Role.Descent.RoleGroup role = new DTIMS.Role.Descent.RoleGroup();
            DataTable table = role.GetSys_UserRole();
            DataRow row = null;
            this.dropRule.Items.Clear();
            ListItem item = new ListItem();
            //item.Text = "请选择";
            //item.Value = "";
            //this.dropRule.Items.Add(item);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                row = table.Rows[i];
                if (row["UserRole_ID"].ToString().Equals("1") || row["UserRole_Name"].ToString().Equals("超级管理员"))
                {
                    if (!this.TextBox1.Text.Equals("super"))
                    {
                        continue;
                    }
                }
                item = new ListItem();
                item.Text = row["UserRole_Name"].ToString();
                item.Value = row["UserRole_ID"].ToString();
                this.dropRule.Items.Add(item);
            }
        }
        #endregion

        protected void dfadsfs_Click(object sender, EventArgs e)
        {

        }

        protected void dropAreaLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            string areaId = this.dropAreaLists.SelectedValue;
            InitRoleGroup(areaId);
        }
    }
}
