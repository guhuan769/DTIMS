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

using Sys.Project.Common;
using DTIMS.Comm;

namespace DTIMS.Web
{
    public partial class PrivilegeManage : WebPageBase
    {
        public int data_rows = 0;//������
        public int max_page_num = 0;//βҳҳ��
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //���ص�����
                    this.trvAreaInfo.Nodes.Add(AreaTree.CreateAreaTree(this.Oper.Area_ID));

                    //����Ĭ��ѡ�нڵ�
                    if (this.trvAreaInfo.Nodes.Count > 0)
                    {
                        this.trvAreaInfo.Nodes[0].Selected = true;
                        this.DoBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("����", "��ʼ��ҳ�������ϸ:" + ex.Message, MessageType.info));
            }
        }

        #region DataGrid�¼�
        protected void grdPrivilegeGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView data = e.Row.DataItem as DataRowView;
                LinkButton btn = e.Row.FindControl("delBtn") as LinkButton;
                btn.CommandArgument = data["PrivGroup_ID"].ToString().Trim();

                int index = this.grdPrivilegeGroup.Columns.Count - 3;
                if (e.Row.Cells[index].Text.Length > 10)
                {
                    e.Row.Cells[index].ToolTip = e.Row.Cells[index].Text;
                    e.Row.Cells[index].Text = e.Row.Cells[index].Text.Substring(0, 10);
                }

                //����������ӳ�����
                HyperLink hl = new HyperLink();
                hl.Text = data["PrivGroup_Name"].ToString();
                hl.NavigateUrl = "#";
                hl.Attributes.Add("style", "cursor:pointer");
                hl.Attributes.Add("onClick", string.Format("return OpenWindow('{0}','{1}');",
                    data["PrivGroup_ID"].ToString().Trim(), data["User_ID"].ToString().Trim()));
                hl.ToolTip = "�鿴��ϸ";
                e.Row.Cells[1].Controls.Add(hl);

                ////��Ӳ����ļ�����ť
                //Image img = new Image();
                //img.ImageUrl = "../../Images/Common/edit.png";
                //img.Attributes.Add("style", "cursor:hand");
                //img.Attributes.Add("onClick", string.Format("return OpenWindow('{0}');", data["PrivGroup_ID"].ToString().Trim()));
                //img.ToolTip = "�޸�";
                //e.Row.Cells[this.grdPrivilegeGroup.Columns.Count - 1].Controls.Add(img);

                //////ɾ��
                ////LinkButton delBtn = new LinkButton();
                ////delBtn.CommandName = "del";
                ////delBtn.ID = "delBtn";
                ////delBtn.Text = "ɾ��";
                ////delBtn.CommandArgument = data["PrivGroup_ID"].ToString().Trim();
                ////img = new Image();
                ////img.ImageUrl = "../../Images/Common/delete.gif";
                ////img.Attributes.Add("style", "cursor:hand");
                //////img.ToolTip = "ɾ��";
                ////delBtn.Controls.Add(img);
                ////e.Row.Cells[this.grdPrivilegeGroup.Columns.Count - 1].Controls.Add(delBtn);

                ////�鿴�û�
                //img = new Image();
                //img.ImageUrl = "../../Images/Common/users.ico";
                //img.Attributes.Add("style", "cursor:hand");
                //img.Attributes.Add("onClick", string.Format("return ShowUserList('{0}');", data["PrivGroup_ID"].ToString().Trim()));
                //img.ToolTip = "�鿴��ǰ���µ������û�";
                //e.Row.Cells[this.grdPrivilegeGroup.Columns.Count - 1].Controls.Add(img);
            }
        }

        protected void grdPrivilegeGroup_SortCommand(object source, GridViewSortEventArgs e)
        {
            //ʵ������������
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

            this.DoBind();
        }
        #endregion

        #region ���ݰ�
        private void DoBind()
        {
            int cur_page_index = 0;
            DTIMS.DataAccess.Common comm = new DTIMS.DataAccess.Common();
            string inner = @"Sys_PrivilegeGroup INNER JOIN
                      Sys_User ON Sys_PrivilegeGroup.User_ID = Sys_User.User_ID INNER JOIN
                      S_MainArea ON Sys_User.MainArea_ID = S_MainArea.MainArea_ID";
            string fields = @"Sys_PrivilegeGroup.PrivGroup_ID, Sys_PrivilegeGroup.User_ID, Sys_PrivilegeGroup.PrivGroup_Name, Sys_PrivilegeGroup.PrivGroup_Desc, 
                      Sys_User.User_Name, Sys_User.User_Login, S_MainArea.MainArea_Name";

            //����
            string fifter = "";

            if (this.Oper.IsSuper)//�����û���ѯ��ǰ��������Ȩ���飬����ֻ��ѯ�Լ������
            {
                fifter = "Sys_PrivilegeGroup.PrivGroup_IsPrivate<>0 AND S_MainArea.MainArea_ID=" + this.trvAreaInfo.SelectedNode.Value.ToString();
            }
            else
            {
                fifter = "Sys_PrivilegeGroup.PrivGroup_IsPrivate<>0 AND S_MainArea.MainArea_ID=" + this.trvAreaInfo.SelectedNode.Value.ToString();
                //fifter += " AND Sys_PrivilegeGroup.User_ID =" + this.Oper.OperatorId;
            }

            DataTable dt = comm.Query(this.grdPrivilegeGroup.PageSize, CurrentPageIndex(), inner, "Sys_PrivilegeGroup.PrivGroup_ID",
                fields, fifter, GetOrderBy(), out data_rows, out cur_page_index);

            //���а�ǰ������
            if (ViewState["sortOrder"] != null)
            {
                dt.DefaultView.Sort = ViewState["sortOrder"].ToString().Trim();
            }

            this.grdPrivilegeGroup.DataSource = dt.DefaultView;
            this.grdPrivilegeGroup.DataBind();

            if (dt.Rows.Count == 0)
            {
                Common.RenderEmptyGridView(this.grdPrivilegeGroup);
            }

            SetPager(cur_page_index - 1, this.grdPrivilegeGroup.PageSize, dt.DefaultView.Count, data_rows);

            //max_page_num = (data_rows - 1) / page_rows + 1;

            ////����ҳ����ʾ������
            //this.maxpage.Value = max_page_num.ToString();

            ////�жϵ�ǰҳ���Ƿ񳬹��߽�
            //if (Convert.ToInt32(this.pagenum.Text) > max_page_num)
            //{
            //    this.pagenum.Text = max_page_num.ToString();
            //}

            ////�����Ƿ���ʾ��ҳ�İ�ť
            //if (Convert.ToInt32(data_rows) == 0)
            //{
            //    this.pageTable.Visible = false;
            //}
            //else
            //{
            //    this.pageTable.Visible = true;
            //}

            ////����ת�������б�ֵ
            //SetDropNumber();

            ////���������Ϊ0������ʾ�յ�
            //if (this.grdPrivilegeGroup.Rows.Count == 0)
            //{
            //    Inphase.CTQS.Common.Common.RenderEmptyGridView(this.grdPrivilegeGroup);
            //}
            //this.labMaxCount.Text = data_rows.ToString();
            //InitPageButton();
        }
        #endregion

        //#region ��ҳ��ط���
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

        //    DoBind();
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

        //    DoBind();
        //}
        //#endregion

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
        //    DoBind();
        //}

        //protected void ddlPaging_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DropDownList ddl = (DropDownList)grdPrivilegeGroup.BottomPagerRow.Cells[0].FindControl("ddlPaging");
        //    grdPrivilegeGroup.PageIndex = ddl.SelectedIndex;
        //    this.DoBind();
        //}
        //#endregion ��ҳ��ط���

        #region ѡ�����ڵ��ˢ������
        protected void trvAreaInfo_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (this.trvAreaInfo.SelectedNode != null)
            {
                this.RefreshData(null, null);
            }
        }
        #endregion ѡ�����ڵ��ˢ������

        #region ˢ������
        protected void RefreshData(object sender, EventArgs e)
        {
            this.DoBind();
        }
        #endregion

        protected void grdPrivilegeGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("del"))
            {
                string privID = e.CommandArgument.ToString();

                try
                {
                    Sys_PrivilegeGroup group = new Sys_PrivilegeGroup(privID);
                    group.Delete();

                    Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("��ʾ", "ɾ���ɹ�", MessageType.info, "initDelEvent"));

                    //��¼��־
                    DTIMS.DataAcess.Common.SysLog.Add(this.Oper.OperatorId, DTIMS.DataAcess.Common.SysLog.LogMode.UpDate, DTIMS.DataAcess.Common.SysLog.LogItemID.RightManage,
                          string.Format("ɾ��Ȩ���飺{0}", group.PrivGroup_Name), this.Oper.Client_IP);

                    //���°�����
                    this.RefreshData(null, null);
                }
                catch (Exception ex)
                {
                    Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("����", ex.Message, MessageType.error, "initDelEvent"));
                }
            }
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

                this.DoBind();
            }
            catch (Exception ex)
            {
                this.JqueryMessager(this.UpdatePanel1, ex.Message, Sys.Comm.MessageType.error);
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

    }//end public partial class PrivilegeManage : System.Web.UI.Page
}
