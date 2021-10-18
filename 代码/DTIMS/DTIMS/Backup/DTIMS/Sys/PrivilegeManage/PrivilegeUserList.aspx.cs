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
using Sys.Com.Common;


namespace DTIMS.Web
{
    public partial class PrivilegeUserList : WebPageBase
    {
        
        public int data_rows = 0;//������
        public int max_page_num = 0;//βҳҳ��
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //������
                    DoBind();
                }
            }
            catch (Exception ex)
            {
                Common.ScriptManagerRegister(this.UpdatePanel1,
                   JqueryComm.ShowMessage("����", "��ʼ��ҳ�����,��ϸ:" + ex.Message, MessageType.error));
            }
        }

        #region DataGrid�¼�
        protected void grdUserList_SortCommand(object source, GridViewSortEventArgs e)
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
            string privGroup_ID = Request.QueryString["PrivGroup_ID"];
            int cur_page_index = 0;
            DTIMS.DataAccess.Common comm = new DTIMS.DataAccess.Common();
            string inner = @"Sys_PrivilegeGroup 
                            INNER JOIN
	                            Sys_UserPrivilegeMap 
                            ON 
	                            Sys_PrivilegeGroup.PrivGroup_ID = Sys_UserPrivilegeMap.PrivGroup_ID  and PrivGroup_IsPrivate = 1
                            INNER JOIN
	                            Sys_User 
                            ON 
	                            Sys_UserPrivilegeMap.User_ID = Sys_User.User_ID 
                            INNER JOIN
	                            S_MainArea 
                            ON 
	                            Sys_User.MainArea_ID = S_MainArea.MainArea_ID 
                            INNER JOIN
	                            Sys_UserRole 
                            ON 
	                            Sys_User.UserRole_ID = Sys_UserRole.UserRole_ID";
            string fields = @"Sys_User.User_ID,Sys_PrivilegeGroup.PrivGroup_Name, S_MainArea.MainArea_Name, 
	                            Sys_User.User_Name, Sys_User.User_Login,Sys_UserRole.UserRole_Name";
            string fifter = "Sys_PrivilegeGroup.PrivGroup_ID=" + privGroup_ID;
            DataTable dt = comm.Query(this.grdUserList.PageSize, CurrentPageIndex(), inner, "Sys_User.User_ID",
                fields, fifter, GetOrderBy(), out data_rows, out cur_page_index);

            //���а�ǰ������
            if (ViewState["sortOrder"] != null)
            {
                dt.DefaultView.Sort = ViewState["sortOrder"].ToString().Trim();
            }

            this.grdUserList.DataSource = dt.DefaultView;
            this.grdUserList.DataBind();

            if (dt.Rows.Count == 0)
            {
                 Common.RenderEmptyGridView(this.grdUserList);
            }

            SetPager(cur_page_index - 1, this.grdUserList.PageSize, dt.DefaultView.Count, data_rows);

            //max_page_num = (data_rows - 1) / this.grdUserList.PageSize + 1;

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
            //if (this.grdUserList.Rows.Count == 0)
            //{
            //    Inphase.CTQS.Common.Common.RenderEmptyGridView(this.grdUserList);
            //}
            //this.labMaxCount.Text = data_rows.ToString();
            //InitPageButton();
        }
        #endregion

        #region ��ҳ��ط���
        #region ����ת��ҳ�������б�
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
        #endregion

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

        #region �趨 ��ҳ ��һҳ ��һҳ βҳ ��ť�Ƿ����
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
        #endregion

        #region �����ҳ
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
        #endregion

        #region ת��ָ����ҳ����ȥ
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
        //    DoBind();
        //}

        //protected void ddlPaging_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DropDownList ddl = (DropDownList)grdUserList.BottomPagerRow.Cells[0].FindControl("ddlPaging");
        //    grdUserList.PageIndex = ddl.SelectedIndex;
        //    this.DoBind();
        //}
        #endregion ��ҳ��ط���

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

    }//end public partial class PrivilegeUserList : Inphase.AspxTask.WebPageBase
}//end namespace CTQS.Sys.PrivilegeManage
