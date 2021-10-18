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
using BJ.Project.Common;
using BJ.DTIMS.Common;
using System.Text;

namespace CTQS.Sys.UserManage
{
    public partial class UserManage : BJ.AspxTask.WebPageBase
    {
        public int data_rows = 0;//������
        public int max_page_num = 0;//βҳҳ��

        protected void Page_Load(object sender, EventArgs e)
        {
            #region ҳ���ʹ��
            if (!Page.IsPostBack)
            {
                Operator oper = (Operator)Session["Operator"];

                TreeNode node = BJ.DTIMS.Common.AreaTree.CreateAreaTree(oper.Area_ID);
                this.TreeView1.Nodes.Add(node);
                if (this.TreeView1.Nodes.Count > 0)
                {
                    this.TreeView1.Nodes[0].Selected = true;
                }
                this.hidAreaID.Value = this.TreeView1.SelectedNode.Value;

                Data();
            }
            #endregion
            //Data();
        }

        #region ˢ������
        protected void RefreshData(object sender, EventArgs e)
        {
            this.Data();
            BJ.DTIMS.Common.Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.FormatScriptString("initDelEvent"));
        }
        #endregion


        #region ��ҳ���ݰ�
        private void Data()
        {
            int curPageIndex = 0;

            BJ.OperatorPersistent.Descent.OperatorPersistent op = new BJ.OperatorPersistent.Descent.OperatorPersistent();
            DataTable table = null;
            try
            {
                string where = GetWhere();
                table = op.GetAreaOperator(this.GridView1.PageSize, CurrentPageIndex(), where, GetOrderBy(), out data_rows, out curPageIndex);
            }
            catch (Exception ex)
            {
                JqueryMessager(this.UpdatePanel1, "����", ex.Message, BJ.Sys.Comm.MessageType.error);
                return;
            }


            this.GridView1.DataSource = table;
            this.GridView1.DataBind();
            //max_page_num = (data_rows - 1) / page_rows + 1;

            ////����ҳ����ʾ������
            //this.maxpage.Value = max_page_num.ToString();
            ////��X������
            //this.labMaxCount.Text = data_rows.ToString();
            ////�жϵ�ǰҳ���Ƿ񳬹��߽�
            //if (Convert.ToInt32(this.pagenum.Text) > max_page_num)
            //{
            //    this.pagenum.Text = max_page_num.ToString();
            //}
            ////����ת�������б�ֵ
            //SetDropNumber();
            //InitPageButton();

            //#region ������������ʱ��ʾ�����ݺͷ�ҳ��ʽ
            //if (Convert.ToInt32(data_rows) == 0)
            //{
            //    this.pageTable.Visible = false;
            //}
            //else
            //{
            //    this.pageTable.Visible = true;
            //}
            //if (Convert.ToInt32(data_rows) == 0)
            //{
            //    this.notData.Visible = true;
            //}
            //else
            //{
            //    this.notData.Visible = false;
            //}
            //if (this.GridView1.Rows.Count == 0)
            //{
            //    Inphase.CTQS.Common.Common.RenderEmptyGridView(this.GridView1);
            //}
            //#endregion

            if (table.Rows.Count == 0)
            {
                Common.RenderEmptyGridView(this.GridView1);
            }

            SetPager(curPageIndex - 1, this.GridView1.PageSize, table.DefaultView.Count, data_rows);
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            Data();
        }

        //#region ����ת��ҳ�������б�
        ///// <summary>
        ///// ����ת��ҳ�������б�
        ///// </summary>
        //private void SetDropNumber()
        //{
        //    string pageIndex = this.pagenum.Text;
        //    string maxPage = this.maxpage.Value;
        //    this.dropGoToPageNumber.Items.Clear();
        //    ListItem item = null;
        //    for (int i = 1; i <= Convert.ToInt32(maxPage); i++)
        //    {
        //        item = new ListItem();
        //        item.Text = i.ToString();
        //        item.Value = i.ToString();
        //        this.dropGoToPageNumber.Items.Add(item);
        //    }

        //    this.dropGoToPageNumber.SelectedValue = pageIndex;
        //}
        //#endregion

        /// <summary>
        /// ��ȡ��ǰҳ��
        /// </summary>
        /// <returns></returns>
        private int CurrentPageIndex()
        {
            //��ǰҳ��
            string pageIndex = this.hidPageIndex.Value;
            if (string.IsNullOrEmpty(pageIndex))
            {
                return 1;
            }
            else
            {
                try
                {
                    return Convert.ToInt32(pageIndex);
                }
                catch
                {
                    return 1;
                }
            }
        }

        //#region �趨 ��ҳ ��һҳ ��һҳ βҳ ��ť�Ƿ����
        ///// <summary>
        ///// �趨 ��ҳ ��һҳ ��һҳ βҳ ��ť�Ƿ����
        ///// </summary>
        //private void InitPageButton()
        //{
        //    //��ȫ����ʼ��
        //    this.btn_first.Enabled = true;
        //    this.btn_pre.Enabled = true;
        //    this.btn_end.Enabled = true;
        //    this.btn_next.Enabled = true;

        //    if (this.pagenum.Text == "1")
        //    {
        //        this.btn_first.Enabled = false;
        //        this.btn_pre.Enabled = false;
        //    }
        //    if (this.pagenum.Text == this.maxpage.Value)
        //    {
        //        this.btn_end.Enabled = false;
        //        this.btn_next.Enabled = false;
        //    }
        //    if (this.pagenum.Text == "1" && this.maxpage.Value == "0")
        //    {
        //        this.btn_end.Enabled = false;
        //        this.btn_next.Enabled = false;
        //    }
        //    if (string.IsNullOrEmpty(this.maxpage.Value) || this.maxpage.Value == "0")
        //    {
        //        this.btn_first.Enabled = false;
        //        this.btn_pre.Enabled = false;
        //        this.btn_end.Enabled = false;
        //        this.btn_next.Enabled = false;
        //    }
        //}
        //#endregion

        //#region �����ҳ
        ///// <summary>
        ///// �����ҳ
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void GotoThePage(object sender, CommandEventArgs e)
        //{
        //    switch (e.CommandArgument.ToString())
        //    {
        //        case "��ҳ":
        //            this.pagenum.Text = "1";
        //            break;
        //        case "��һҳ":
        //            this.pagenum.Text = Convert.ToString(Convert.ToInt32(this.pagenum.Text.Trim()) - 1);
        //            break;
        //        case "��һҳ":
        //            this.pagenum.Text = Convert.ToString(Convert.ToInt32(this.pagenum.Text.Trim()) + 1);
        //            break;
        //        case "ĩҳ":
        //            this.pagenum.Text = this.maxpage.Value;
        //            break;
        //    }

        //    Data();
        //}
        //#endregion

        //#region ת��ָ����ҳ����ȥ
        ///// <summary>
        ///// ת��ָ����ҳ����ȥ
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void GotoThePageNum(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(this.dropGoToPageNumber.SelectedValue))
        //    {
        //        string page_num = this.dropGoToPageNumber.SelectedValue;
        //        try
        //        {
        //            Convert.ToUInt32(this.dropGoToPageNumber.SelectedValue.Trim());
        //            this.pagenum.Text = page_num.Trim();
        //        }
        //        catch (FormatException ex)
        //        {
        //            this.pagenum.Text = "1";
        //            this.dropGoToPageNumber.SelectedValue = "";
        //            Response.Write("<script language='javascript' type='text/javascript'>alert('" + ex + "');</script>");
        //            return;
        //        }
        //        catch (OverflowException ex)
        //        {
        //            this.pagenum.Text = "1";
        //            this.dropGoToPageNumber.SelectedValue = "";
        //            Response.Write("<script language='javascript' type='text/javascript'>alert('" + ex + "');</script>");
        //            return;
        //        }
        //        catch (ArgumentException ex)
        //        {
        //            this.pagenum.Text = "1";
        //            this.dropGoToPageNumber.SelectedValue = "";
        //            Response.Write("<script language='javascript' type='text/javascript'>alert('" + ex + "');</script>");
        //            return;
        //        }

        //    }

        //    Data();
        //}
        //#endregion
        #endregion

        #region where����
        /// <summary>
        /// ��ȡwhere����
        /// </summary>
        /// <returns></returns>
        private string GetWhere()
        {
            StringBuilder where = new StringBuilder();

            string urserName = this.txtUserName.Value.Trim();
            string loginName = this.txtLoginName.Value.Trim();
            string status = this.dropStatus.SelectedValue.Trim();
            string role = this.dropRole.SelectedValue.Trim();

            if (!string.IsNullOrEmpty(urserName))
            {
                where.Append(" User_Name like '%" + urserName + "%' and ");
            }
            if (!string.IsNullOrEmpty(loginName))
            {
                where.Append(" User_Login like '%" + loginName + "%' and ");
            }
            if (!status.Equals("2"))
            {
                where.Append(" User_Status=" + status + " and ");
            }
            if (!role.Equals("0"))
            {
                where.Append(" a.UserRole_ID=" + role + " and ");
            }

            //��ǰѡ��ĵ���
            if (ViewState["UserManage_AreaID"] != null)
            {
                where.Append(" a.MainArea_ID=" + ViewState["UserManage_AreaID"].ToString() + " and ");
            }
            else
            {
                Operator oper = (Operator)Session["Operator"];
                where.Append(" a.MainArea_ID=" + oper.Area_ID + " and ");
            }

            string sql = where.ToString();

            if (!string.IsNullOrEmpty(sql))
            {
                sql = sql.Substring(0, sql.Length - 4);
            }
            return sql;
        }
        #endregion

        #region ��������
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <returns></returns>
        private string GetOrderBy()
        {
            return this.orderbyname.Value + " " + this.orderbytype.Value;
        }
        #endregion

        //protected void dropGoToPageNumber_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    this.pagenum.Text = this.dropGoToPageNumber.SelectedValue;
        //    Data();
        //}

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView data = e.Row.DataItem as DataRowView;
                string userid = data["User_ID"].ToString();
                LinkButton btn = e.Row.FindControl("delBtn") as LinkButton;
                HtmlImage btnEdit = e.Row.FindControl("btnEdit") as HtmlImage;
                HtmlImage btnRoleViews = e.Row.FindControl("btnRoleViews") as HtmlImage;
                btn.CommandArgument = userid;

                //<a href="#" onclick="doView(<%#Eval("User_ID") %>,'<%#Eval("UserRole_ID")%>')">
                //                                   <%#Eval("User_Name")%>
                //                               </a>

                //���õ�һ�е�����
                System.Web.UI.WebControls.HyperLink hl = new HyperLink();
                hl.Text = data["User_Name"].ToString();
                hl.NavigateUrl = "#";
                hl.Attributes.Add("style", "cursor:pointer");
                hl.ToolTip = "�鿴";
                hl.Attributes.Add("onClick", "doView('" + userid.Trim() + "','" + data["UserRole_ID"].ToString().Trim() + "')");
                e.Row.Cells[0].Controls.Add(hl);

                HtmlImage delImg = new HtmlImage();
                delImg.Border = 0;
                if (e.Row.Cells[4].Text.Equals("0"))
                {
                    e.Row.Cells[4].Text = "����";
                    btn.Text = "����";
                    btn.ToolTip = "����";
                    delImg.Attributes.Add("title", "����");
                    delImg.Src = "../../images/enabled.png";
                    btn.Controls.Add(delImg);
                }
                else
                {
                    e.Row.Cells[4].Text = "����";
                    btn.Text = "����";
                    btn.ToolTip = "����";
                    delImg.Attributes.Add("title", "����");
                    delImg.Src = "../../images/disabled.png";

                    btn.Controls.Add(delImg);
                }

                //�����super�û�������ò�����ť
                if (data["UserRole_ID"].ToString().Equals("1"))
                {
                    btn.Visible = false;
                    btnEdit.Attributes.Add("onclick", "editUser(" + userid + ",'ok')");
                }
                else if (userid == this.Oper.OperatorId)//������Լ�����ֻ���޸Ļ�����Ϣ���Ҳ��ܽ����Լ�
                {
                    btn.Visible = false;
                    btnEdit.Attributes.Add("onclick", "editUser(" + userid + ",'ok')");
                }
                else
                {
                    btnEdit.Attributes.Add("onclick", "editUser(" + userid + ")");
                }
                btnRoleViews.Attributes.Add("onclick", "ShowRoleViews(" + userid + ")");
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sort = e.SortExpression;
            this.orderbyname.Value = sort;
            if (string.IsNullOrEmpty(this.orderbytype.Value) || this.orderbytype.Value.ToLower().Equals("desc"))
            {
                this.orderbytype.Value = "asc";
            }
            else
            {
                this.orderbytype.Value = "desc";
            }
            Data();
        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            ViewState["UserManage_AreaID"] = this.TreeView1.SelectedNode.Value;
            this.hidAreaID.Value = this.TreeView1.SelectedNode.Value;

            BJ.DTIMS.Common.Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.FormatScriptString("initDelEvent"));
            Data();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("del"))
            {
                string userid = e.CommandArgument.ToString();

                //���ܽ����Լ�
                Operator oper = (Operator)Session["Operator"];
                if (oper.OperatorId.Equals(userid))
                {
                    Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("��ʾ", "ɾ��ʧ��,���ܽ����Լ�", MessageType.error));
                }
                else
                {
                    try
                    {
                        LinkButton status = e.CommandSource as LinkButton;
                        string temp = null;
                        if (status.Text.Equals("����"))
                        {
                            temp = "1";
                        }
                        else
                        {
                            temp = "0";
                        }
                        BJ.OperatorPersistent.Descent.OperatorPersistent.Delete(userid, temp);
                        Operator user = OperatorFactory.OperatorCreate(userid, 0);

                        //��¼��־
                       BJ.SysLog.Descent.SysLog.Add(oper.OperatorId, BJ.SysLog.Descent.SysLog.LogMode.UpDate, BJ.SysLog.Descent.SysLog.LogItemID.OperatorManage,
                            status.Text.Trim() + "����Ա(" + user.OperLogin + ").", this.Oper.Client_IP);
                        Data();
                        Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.FormatScriptString("initDelEvent"));
                    }
                    catch (Exception ex)
                    {
                        Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("��ʾ", "����ʧ��.ԭ��" + ex.Message, MessageType.error));
                    }
                }
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            Data();
        }

        #region ��ҳЧ������
        /// <summary>
        /// �����ҳ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GotoThePage(object sender, CommandEventArgs e)
        {
            try
            {
                switch (e.CommandArgument.ToString().ToLower())
                {
                    case "first":
                        this.hidPageIndex.Value = "1";
                        break;
                    case "prev":
                        this.hidPageIndex.Value = Convert.ToString(Convert.ToInt32(this.hidPageIndex.Value.Trim()) - 1);
                        break;
                    case "next":
                        this.hidPageIndex.Value = Convert.ToString(Convert.ToInt32(this.hidPageIndex.Value.Trim()) + 1);
                        break;
                    case "last":
                        this.hidPageIndex.Value = lblPageCount.Text;//this.maxpage.Value;
                        break;
                    case "num":
                        int page = 1;
                        if (!int.TryParse(txtPageIndex.Text.Trim(), out page))
                        {
                            throw new Exception("ҳ���Ƿ���");
                        }

                        this.hidPageIndex.Value = page.ToString();

                        break;
                }

                this.Data();
            }
            catch (Exception ex)
            {
                this.JqueryMessager(this.UpdatePanel1, ex.Message, BJ.Sys.Comm.MessageType.error);
            }
        }

        private void SetPager(int pageIndex, int pageSize, int pageRows, int rowCount)
        {
            // ��ҳ��ť��ʽ��
            string normalBtnClass = "c-l-btn c-l-btn-plain";
            string disabledBtnClass = "c-l-btn c-l-btn-plain c-l-btn-disabled";

            // ��ҳ����
            int pageCount = (int)Math.Ceiling((double)rowCount / pageSize);

            // ���õ�һҳ����һҳ��ť�Ƿ���á�
            if (pageIndex == 0)
            {
                lbtnPagingFirst.Enabled = false;
                lbtnPagingPrev.Enabled = false;
                lbtnPagingFirst.CssClass = disabledBtnClass;
                lbtnPagingPrev.CssClass = disabledBtnClass;
            }
            else
            {
                lbtnPagingFirst.Enabled = true;
                lbtnPagingPrev.Enabled = true;
                lbtnPagingFirst.CssClass = normalBtnClass;
                lbtnPagingPrev.CssClass = normalBtnClass;
            }

            // ������һҳ�����һҳ��ť�Ƿ���á�
            if (pageIndex >= pageCount - 1)
            {
                lbtnPagingNext.Enabled = false;
                lbtnPagingLast.Enabled = false;
                lbtnPagingNext.CssClass = disabledBtnClass;
                lbtnPagingLast.CssClass = disabledBtnClass;
            }
            else
            {
                lbtnPagingNext.Enabled = true;
                lbtnPagingLast.Enabled = true;
                lbtnPagingNext.CssClass = normalBtnClass;
                lbtnPagingLast.CssClass = normalBtnClass;
            }

            // ���õ�ǰҳ����ҳ����ֵ��
            txtPageIndex.Text = (pageIndex + 1).ToString();
            hidPageIndex.Value = txtPageIndex.Text;
            lblPageCount.Text = pageCount.ToString();

            lblRowStart.Text = ((pageIndex * pageSize) + 1).ToString();
            lblRowEnd.Text = ((pageIndex * pageSize) + pageRows).ToString();
            lblRowCount.Text = rowCount.ToString();
        }
        #endregion

    }
}
