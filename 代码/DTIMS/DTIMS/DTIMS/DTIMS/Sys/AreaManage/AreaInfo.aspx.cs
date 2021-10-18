using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BJ.Project.Common;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Text;
using BJ.DTIMS.Common;

namespace Inphase.Project.CTQS.Descent
{
	/// <summary>
	/// �����������ȴ�SESSION�еõ���Ա��������Ա����õ���ǰ�ɹ����ȫ��������ˢ����������
	/// </summary>
    public partial class AreaInfo : BJ.AspxTask.WebPageBase
    {

       private void Page_Load(object sender, System.EventArgs e)
       {
          // �ڴ˴������û������Գ�ʼ��ҳ��
           if (!IsPostBack)
           {
               InitAreaTree();
           }
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

        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <param name="oper"></param>
       private void InitAreaTree()
       {
           this.trvAreaInfo.Nodes.Clear();
           NormalAreaRange area = new NormalAreaRange(this.Oper.OperatorId);
           DataTable areaTable = area.GetOperatorAreaInfo();
           TreeNode root = new TreeNode("�Ĵ�ʡ", "1");
           root.ImageUrl = "~/images/SystemManage/AreaRoot.ico";
           root.SelectAction = TreeNodeSelectAction.None;
           trvAreaInfo.Nodes.Add(root);
           foreach(DataRow row in areaTable.Rows)
           {
               TreeNode node = new TreeNode(row["MainArea_Name"].ToString(), row["MainArea_ID"].ToString());
               node.ImageUrl = "~/images/SystemManage/AreaSub.ico";
               trvAreaInfo.Nodes[0].ChildNodes.Add(node);
           }
           this.trvAreaInfo.ExpandAll();
           this.textEdit.Text = "";
           if (this.buttonEdit.Enabled) this.buttonEdit.Enabled = false;
       }

        /// <summary>
        /// ѡ��ڵ��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trvAreaInfo_SelectedNodeChanged(object sender, EventArgs e)
        {
            this.textEdit.Text = this.trvAreaInfo.SelectedNode.Text;
            if(!this.buttonEdit.Enabled) this.buttonEdit.Enabled = true;
        }

        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                BJ.Area.Descent.Area area = new BJ.Area.Descent.Area(this.trvAreaInfo.SelectedValue.ToString());
                area.MainArea_Name = this.textEdit.Text.Trim();
                area.Update();
                BJ.DTIMS.Common.Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("��ʾ", "�޸ĳɹ���", MessageType.info));
                string content = "�޸ĵ�����" + this.textEdit.Text.Trim() + "��ԭ��������Ϊ��" + this.trvAreaInfo.SelectedNode.Text + "������IDΪ��" + this.trvAreaInfo.SelectedValue + "����";
                BJ.SysLog.Descent.SysLog.Add(this.Oper.OperatorId, BJ.SysLog.Descent.SysLog.LogMode.UpDate,BJ.SysLog.Descent.SysLog.LogItemID.AreaManage, content, this.Oper.Client_IP);
                this.trvAreaInfo.SelectedNode.Text = this.textEdit.Text.Trim();
                this.trvAreaInfo.SelectedNode.Selected = true;
            }
            catch (Exception ex)
            {
                BJ.DTIMS.Common.Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("��ʾ", "�޸�ʧ�ܣ�" + ex.Message, MessageType.error));
            }
        }
	}
}
