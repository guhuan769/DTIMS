using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;

using Sys.Project.Common;

namespace DTIMS.Comm
{
    /// <summary>
    /// 权限树
    /// </summary>
    public class PrivilegeTree
    {
        #region 生成功能权限树
        private static Hashtable htFuns = new Hashtable(); //当前权限组的权限ID集合

        /// <summary>
        /// 功能描述：生成功能权限树
        /// </summary>
        /// <param name="htFunsCollections">要修改的用户的权限组集合</param>
        /// <param name="oper">登录用户对象</param>
        /// <returns></returns>
        private static TreeNode CreatePrivilegeTree(Hashtable htFunsCollections, Operator oper)
        {
            //如果不是新增则获取当前权限组对应的权限
            htFuns = htFunsCollections;

            return CreateTree(oper);
        }

        /// <summary>
        /// 功能描述：生成功能权限树
        /// </summary>
        /// <param name="privGroup_ID">权限组ID</param>
        /// <param name="oper">登录用户对象</param>
        /// <returns></returns>
        public static TreeNode CreatePrivilegeTree(string privGroup_ID,Operator oper)
        {
            //如果不是新增则获取当前权限组对应的权限
            Sys_PrivilegeGroup group = null;
            if (privGroup_ID == "0")
            {
                htFuns.Clear();
                group = new Sys_PrivilegeGroup();
            }
            else
            {
                group = new Sys_PrivilegeGroup(privGroup_ID);
                htFuns = group.HashtableFunc_ID;
            }

            return CreateTree(oper);
        }

        /// <summary>
        /// 功能描述：生成功能权限树
        /// </summary>
        /// <param name="oper">人员对象</param>
        /// <returns></returns>
        public static TreeNode CreatePrivilegeTree(Operator oper)
        {
            return CreatePrivilegeTree("0", oper);
        }

        private static TreeNode CreateTree(Operator oper)
        {
            //获取当前用户的权限
            DataTable dtUserFunc = Sys_FunctionItem.GetUserFunctionItem(oper);

            TreeNode root = new TreeNode("功能菜单树");
            root.Value = "0";
            root.ShowCheckBox = false;
            root.Checked = false;
            root.Expanded = true;
            root.ImageUrl = "~/images/TreeView/menu1.ico";

            //生成大类菜单
            DataRow[] rowCategory = dtUserFunc.Select("FunCategory_ID=1");
            foreach (DataRow dr in rowCategory)
            {
                TreeNode node = new TreeNode();
                node.Text = dr["Fun_Name"].ToString().Trim();
                node.Value = dr["Fun_ID"].ToString().Trim();
                node.ShowCheckBox = true;
                node.Expanded = false;
                node.ImageUrl = "~/images/TreeView/menu2.ico";

                //判断是否选择了当前功能项
                if (htFuns != null && htFuns.Count > 0
                    && htFuns.ContainsKey(dr["Fun_ID"].ToString().Trim()))
                {
                    node.Checked = true;
                }
                else
                {
                    node.Checked = false;
                }

                //加载子功能节点
                BuildTreeNode(node, dr["Fun_ID"].ToString().Trim(), dtUserFunc);

                root.ChildNodes.Add(node);
            }

            return root;
        }

        private static void BuildTreeNode(TreeNode parent, string parentID, DataTable data)
        {
            DataRow[] rows = data.Select(string.Format("Fun_ParentID='{0}'",parentID));
            foreach (DataRow row in rows)
            {
                TreeNode node = new TreeNode(row["Fun_Name"].ToString());
                node.Value = row["Fun_ID"].ToString().Trim();
                node.ShowCheckBox = true;
                node.Expanded = false;

                //设置Icon
                if (row["FunCategory_ID"].ToString().Trim() == "2")//功能菜单（如用户管理）
                {
                    node.ImageUrl = "~/images/TreeView/icon-folder-open.gif";
                }
                else if (row["FunCategory_ID"].ToString().Trim() == "3")//具体的功能（如修改，删除等）
                {
                    node.ImageUrl = "~/images/TreeView/menu4.ico";
                }

                //判断是否选择了当前功能项
                if (htFuns != null && htFuns.Count > 0
                    && htFuns.ContainsKey(row["Fun_ID"].ToString().Trim()))
                {
                    node.Checked = true;
                }
                else
                {
                    node.Checked = false;
                }
                parent.ChildNodes.Add(node);

                BuildTreeNode(node, row["Fun_ID"].ToString().Trim(), data);
            }
        }
        #endregion

        #region 
        public static TreeNode CreateTree(string userId)
        {
            //获取当前用户的权限
            Operator oper = OperatorFactory.OperatorCreate(userId);
            DataTable dtUserFunc = Sys_FunctionItem.GetUserFunctionItem(oper);

            TreeNode root = new TreeNode("功能菜单树");
            root.Value = "0";
            root.ShowCheckBox = false;
            root.Checked = false;
            root.Expanded = true;
            root.ImageUrl = "~/images/TreeView/menu1.ico";

            //生成大类菜单
            DataRow[] rowCategory = dtUserFunc.Select("FunCategory_ID=1");
            foreach (DataRow dr in rowCategory)
            {
                TreeNode node = new TreeNode();
                node.Text = dr["Fun_Name"].ToString().Trim();
                node.Value = dr["Fun_ID"].ToString().Trim();
                node.ShowCheckBox = false;
                node.Expanded = false;
                node.ImageUrl = "~/images/TreeView/menu2.ico";
                node.SelectAction = TreeNodeSelectAction.Expand;

                //判断是否选择了当前功能项
                if (htFuns != null && htFuns.Count > 0
                    && htFuns.ContainsKey(dr["Fun_ID"].ToString().Trim()))
                {
                    node.Checked = true;
                }
                else
                {
                    node.Checked = false;
                }

                //加载子功能节点
                BuildTreeNode1(node, dr["Fun_ID"].ToString().Trim(), dtUserFunc);

                root.ChildNodes.Add(node);
            }

            root.Expand();
            return root;
        }

        private static void BuildTreeNode1(TreeNode parent, string parentID, DataTable data)
        {
            DataRow[] rows = data.Select(string.Format("Fun_ParentID='{0}'", parentID));
            foreach (DataRow row in rows)
            {
                TreeNode node = new TreeNode(row["Fun_Name"].ToString());
                node.Value = row["Fun_ID"].ToString().Trim();
                node.ShowCheckBox = false;
                node.Expanded = false;
                node.SelectAction = TreeNodeSelectAction.None;

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

                BuildTreeNode1(node, row["Fun_ID"].ToString().Trim(), data);
            }
        }
        #endregion
    }//end public class PrivilegeTree
}//end namespace Inphase.JFIMS.Common
