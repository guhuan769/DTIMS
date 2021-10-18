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
using BJ.DTIMS.Common;

namespace CTQS.Sys.UserManage
{
    public partial class RoleView : BJ.AspxTask.WebPageBase
    {
        private Hashtable htFuns = new Hashtable(); //当前权限组的权限ID集合

        protected void Page_Load(object sender, EventArgs e)
        {
            string userId = Request.Params["userId"];
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    TreeNode node = PrivilegeTree.CreateTree(userId);
                    node.SelectAction = TreeNodeSelectAction.Expand;
                    trvFunctionItem.Nodes.Add(node);
                    trvFunctionItem.DataBind();

                    // 构建权限组树。
                    trvGroup.Nodes.Add(GetGroupsNode(userId));
                }
            }
        }

        private TreeNode GetGroupsNode(string userId)
        {
            TreeNode root = new TreeNode("所属权限组");
            root.Value = "0";
            root.ShowCheckBox = false;
            root.Checked = false;
            //root.Expanded = true;
            root.SelectAction = TreeNodeSelectAction.Expand;
            root.ImageUrl = "~/images/TreeView/menu1.ico";

            //Inphase.Project.CTQS.Descent.OperatorPersistent editOper = new Inphase.Project.CTQS.Descent.OperatorPersistent(userId);

            //TreeNode privNode = null;
            //foreach (string item in editOper.RoleList)
            //{
            //    TreeNode node = CreateGroupNode(item.Split('|')[0]);
            //    if (node.Text == "私有权限")
            //    {
            //        privNode = node;
            //    }
            //    else
            //    {
            //        root.ChildNodes.Add(node);
            //    }
            //}

            //if (privNode != null)
            //{
            //    root.ChildNodes.Add(privNode);
            //}

            DataTable group = BJ.Project.Common.Sys_PrivilegeGroup.GetUserGroup(userId);
            TreeNode privNode = null;
            foreach (DataRow item in group.Rows)
            {
                TreeNode node = new TreeNode();
                node.ImageUrl = "~/images/TreeView/menu2.ico";
                node.SelectAction = TreeNodeSelectAction.Expand;
                node.Value = item["PrivGroup_ID"].ToString().Trim();
                node.CollapseAll();

                if (item["PrivGroup_IsPrivate"].ToString().Trim() == "0")
                {
                    node.Text = "私有权限";
                    privNode = node;
                }
                else
                {
                    node.Text = item["PrivGroup_Name"].ToString().Trim();
                    root.ChildNodes.Add(node);
                }

                DataTable dt = BJ.Project.Common.Sys_FunctionItem.GetGroupFunctions(node.Value);
                CreateTree(node, dt);
            }

            if (privNode != null)
            {
                root.ChildNodes.Add(privNode);
            }

            return root;
        }

        private TreeNode CreateGroupNode(string privGroup_ID)
        {
            BJ.Project.Common.Sys_PrivilegeGroup privGroup = new BJ.Project.Common.Sys_PrivilegeGroup(privGroup_ID);
            TreeNode node = new TreeNode();
            node.ImageUrl = "~/images/TreeView/menu2.ico";
            node.SelectAction = TreeNodeSelectAction.Expand;
            node.Value = privGroup.PrivGroup_ID;
            if (privGroup.PrivGroup_IsPrivate)
            {
                node.Text = "私有权限";
            }
            else
            {
                node.Text = privGroup.PrivGroup_Name;
            }

            DataTable dt = BJ.Project.Common.Sys_FunctionItem.GetGroupFunctions(privGroup_ID);

            CreateTree(node, dt);

            return node;
        }

        private void CreateTree(TreeNode parent, DataTable dt)
        {
            //生成大类菜单
            DataRow[] rowCategory = dt.Select("FunCategory_ID=1");
            foreach (DataRow dr in rowCategory)
            {
                TreeNode node = new TreeNode();
                node.Text = dr["Fun_Name"].ToString().Trim();
                node.Value = dr["Fun_ID"].ToString().Trim();
                node.ShowCheckBox = false;
                node.Expanded = false;
                node.ImageUrl = "~/images/TreeView/icon-folder-open.gif";
                node.SelectAction = TreeNodeSelectAction.Expand;

                //加载子功能节点
                BuildTreeNode(node, dr["Fun_ID"].ToString().Trim(), dt);

                parent.ChildNodes.Add(node);
            }
        }

        private void BuildTreeNode(TreeNode parent, string parentID, DataTable data)
        {
            DataRow[] rows = data.Select(string.Format("Fun_ParentID='{0}'", parentID));
            foreach (DataRow row in rows)
            {
                TreeNode node = new TreeNode(row["Fun_Name"].ToString());
                node.Value = row["Fun_ID"].ToString().Trim();
                node.ShowCheckBox = false;
                node.Expanded = false;
                node.SelectAction = TreeNodeSelectAction.Expand;

                //设置Icon
                if (row["FunCategory_ID"].ToString().Trim() == "2")//功能菜单（如用户管理）
                {
                    node.ImageUrl = "~/images/TreeView/icon-folder-open.gif";
                }
                else if (row["FunCategory_ID"].ToString().Trim() == "3")//具体的功能（如修改，删除等）
                {
                    node.ImageUrl = "~/images/TreeView/menu4.ico";
                }
                parent.ChildNodes.Add(node);

                BuildTreeNode(node, row["Fun_ID"].ToString().Trim(), data);
            }
        }

    }//end public partial class RoleView : Inphase.AspxTask.WebPageBase
}//end namespace CTQS.Sys.UserManage
