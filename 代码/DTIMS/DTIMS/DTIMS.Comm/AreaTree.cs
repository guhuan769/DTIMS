using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using Sys.Project.Common;

namespace DTIMS.Comm
{
    /// <summary>
    /// ����������������
    /// </summary>
    public class AreaTree
    {
       
        #region �ݹ����ɵ�����
        /// <summary>
        /// �����������ݹ����ɵ�����
        /// </summary>
        /// <param name="areaID"></param>
        /// <returns></returns>
        public static TreeNode CreateAreaTree(string areaID)
        {
            Sys.Project.Common.AreaInfo areaInfo = new Sys.Project.Common.AreaInfo(areaID);
            DataTable data = Sys.Project.Common.AreaInfo.GetAllAreaInfo();   //��ȡ���е�����

            //�����������ڵ�
            TreeNode root = new TreeNode();
            root.Text = areaInfo.MainArea_Name;
            root.Value = areaInfo.MainArea_ID;
            root.ImageUrl = "~/images/SystemManage/AreaRoot.ico";

            //�����ӽڵ�
            BuildTreeNode(root, areaID, data);
            return root;
        }

        private static void BuildTreeNode(TreeNode parent, string parentID, DataTable data)
        {
            DataRow[] rows = data.Select("FatherMainArea_ID=" + parentID);
            foreach (DataRow row in rows)
            {
                TreeNode node = new TreeNode(row["MainArea_Name"].ToString());
                node.Value = row["MainArea_ID"].ToString().Trim();
                node.ImageUrl = "~/images/SystemManage/AreaSub.ico";
                parent.ChildNodes.Add(node);
                BuildTreeNode(node, row["MainArea_ID"].ToString().Trim(), data);
            }
        }
        #endregion �ݹ����ɵ�����
    }//end public class AreaTree
}//end namespace Inphase.JFIMS.Common
