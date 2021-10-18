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
    /// Ȩ����
    /// </summary>
    public class PrivilegeTree
    {
        #region ���ɹ���Ȩ����
        private static Hashtable htFuns = new Hashtable(); //��ǰȨ�����Ȩ��ID����

        /// <summary>
        /// �������������ɹ���Ȩ����
        /// </summary>
        /// <param name="htFunsCollections">Ҫ�޸ĵ��û���Ȩ���鼯��</param>
        /// <param name="oper">��¼�û�����</param>
        /// <returns></returns>
        private static TreeNode CreatePrivilegeTree(Hashtable htFunsCollections, Operator oper)
        {
            //��������������ȡ��ǰȨ�����Ӧ��Ȩ��
            htFuns = htFunsCollections;

            return CreateTree(oper);
        }

        /// <summary>
        /// �������������ɹ���Ȩ����
        /// </summary>
        /// <param name="privGroup_ID">Ȩ����ID</param>
        /// <param name="oper">��¼�û�����</param>
        /// <returns></returns>
        public static TreeNode CreatePrivilegeTree(string privGroup_ID,Operator oper)
        {
            //��������������ȡ��ǰȨ�����Ӧ��Ȩ��
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
        /// �������������ɹ���Ȩ����
        /// </summary>
        /// <param name="oper">��Ա����</param>
        /// <returns></returns>
        public static TreeNode CreatePrivilegeTree(Operator oper)
        {
            return CreatePrivilegeTree("0", oper);
        }

        private static TreeNode CreateTree(Operator oper)
        {
            //��ȡ��ǰ�û���Ȩ��
            DataTable dtUserFunc = Sys_FunctionItem.GetUserFunctionItem(oper);

            TreeNode root = new TreeNode("���ܲ˵���");
            root.Value = "0";
            root.ShowCheckBox = false;
            root.Checked = false;
            root.Expanded = true;
            root.ImageUrl = "~/images/TreeView/menu1.ico";

            //���ɴ���˵�
            DataRow[] rowCategory = dtUserFunc.Select("FunCategory_ID=1");
            foreach (DataRow dr in rowCategory)
            {
                TreeNode node = new TreeNode();
                node.Text = dr["Fun_Name"].ToString().Trim();
                node.Value = dr["Fun_ID"].ToString().Trim();
                node.ShowCheckBox = true;
                node.Expanded = false;
                node.ImageUrl = "~/images/TreeView/menu2.ico";

                //�ж��Ƿ�ѡ���˵�ǰ������
                if (htFuns != null && htFuns.Count > 0
                    && htFuns.ContainsKey(dr["Fun_ID"].ToString().Trim()))
                {
                    node.Checked = true;
                }
                else
                {
                    node.Checked = false;
                }

                //�����ӹ��ܽڵ�
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

                //����Icon
                if (row["FunCategory_ID"].ToString().Trim() == "2")//���ܲ˵������û�����
                {
                    node.ImageUrl = "~/images/TreeView/icon-folder-open.gif";
                }
                else if (row["FunCategory_ID"].ToString().Trim() == "3")//����Ĺ��ܣ����޸ģ�ɾ���ȣ�
                {
                    node.ImageUrl = "~/images/TreeView/menu4.ico";
                }

                //�ж��Ƿ�ѡ���˵�ǰ������
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
            //��ȡ��ǰ�û���Ȩ��
            Operator oper = OperatorFactory.OperatorCreate(userId);
            DataTable dtUserFunc = Sys_FunctionItem.GetUserFunctionItem(oper);

            TreeNode root = new TreeNode("���ܲ˵���");
            root.Value = "0";
            root.ShowCheckBox = false;
            root.Checked = false;
            root.Expanded = true;
            root.ImageUrl = "~/images/TreeView/menu1.ico";

            //���ɴ���˵�
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

                //�ж��Ƿ�ѡ���˵�ǰ������
                if (htFuns != null && htFuns.Count > 0
                    && htFuns.ContainsKey(dr["Fun_ID"].ToString().Trim()))
                {
                    node.Checked = true;
                }
                else
                {
                    node.Checked = false;
                }

                //�����ӹ��ܽڵ�
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

                //����Icon
                if (row["FunCategory_ID"].ToString().Trim() == "2")//���ܲ˵������û�����
                {
                    node.ImageUrl = "~/images/TreeView/icon-folder-open.gif";
                }
                else if (row["FunCategory_ID"].ToString().Trim() == "3")//����Ĺ��ܣ����޸ģ�ɾ���ȣ�
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
