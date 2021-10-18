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
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Text;
using Sys.Project.Common;
using Sys.EasyUiEx;
using DTIMS.Comm;
using Sys.Area.Descent;

namespace DTIMS.Web
{
	/// <summary>
	/// 地区管理，首先从SESSION中得到人员对象，由人员对象得到当前可管理的全部地区，刷出地区树。
	/// </summary>
    public partial class AreaInfo : WebPageBase
    {

       private void Page_Load(object sender, System.EventArgs e)
       {
          // 在此处放置用户代码以初始化页面
           if (!IsPostBack)
           {
               InitAreaTree();
           }
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

        /// <summary>
        /// 初始化地区
        /// </summary>
        /// <param name="oper"></param>
       private void InitAreaTree()
       {
           this.trvAreaInfo.Nodes.Clear();
           NormalAreaRange area = new NormalAreaRange(this.Oper.OperatorId);
           DataTable areaTable = area.GetOperatorAreaInfo();
           TreeNode root = new TreeNode("内江市", "1");
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
        /// 选择节点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void trvAreaInfo_SelectedNodeChanged(object sender, EventArgs e)
        {
            this.textEdit.Text = this.trvAreaInfo.SelectedNode.Text;
            if(!this.buttonEdit.Enabled) this.buttonEdit.Enabled = true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                 Area area = new Area(this.trvAreaInfo.SelectedValue.ToString());
                area.MainArea_Name = this.textEdit.Text.Trim();
                area.Update();
                DTIMS.Comm.Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("提示", "修改成功！", MessageType.info));
                string content = "修改地区：" + this.textEdit.Text.Trim() + "，原地区名称为：" + this.trvAreaInfo.SelectedNode.Text + "（地区ID为：" + this.trvAreaInfo.SelectedValue + "）！";
              DTIMS.DataAcess.Common.SysLog.Add(this.Oper.OperatorId, DTIMS.DataAcess.Common.SysLog.LogMode.UpDate, DTIMS.DataAcess.Common.SysLog.LogItemID.AreaManage, content, this.Oper.Client_IP);
                this.trvAreaInfo.SelectedNode.Text = this.textEdit.Text.Trim();
                this.trvAreaInfo.SelectedNode.Selected = true;
            }
            catch (Exception ex)
            {
                DTIMS.Comm.Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("提示", "修改失败！" + ex.Message, MessageType.error));
            }
        }
	}
}
