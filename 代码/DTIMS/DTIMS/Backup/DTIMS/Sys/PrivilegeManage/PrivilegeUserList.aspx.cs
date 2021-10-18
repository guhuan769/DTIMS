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
        
        public int data_rows = 0;//总条数
        public int max_page_num = 0;//尾页页码
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //绑定数据
                    DoBind();
                }
            }
            catch (Exception ex)
            {
                Common.ScriptManagerRegister(this.UpdatePanel1,
                   JqueryComm.ShowMessage("错误", "初始化页面出错,详细:" + ex.Message, MessageType.error));
            }
        }

        #region DataGrid事件
        protected void grdUserList_SortCommand(object source, GridViewSortEventArgs e)
        {
            //实现正反向排序
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

        #region 数据绑定
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

            //进行绑定前的排序
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

            ////设置页面显示总条数
            //this.maxpage.Value = max_page_num.ToString();

            ////判断当前页码是否超过边界
            //if (Convert.ToInt32(this.pagenum.Text) > max_page_num)
            //{
            //    this.pagenum.Text = max_page_num.ToString();
            //}

            ////设置是否显示分页的按钮
            //if (Convert.ToInt32(data_rows) == 0)
            //{
            //    this.pageTable.Visible = false;
            //}
            //else
            //{
            //    this.pageTable.Visible = true;
            //}

            ////设置转向下拉列表值
            //SetDropNumber();

            ////如果数据行为0，则显示空的
            //if (this.grdUserList.Rows.Count == 0)
            //{
            //    Inphase.CTQS.Common.Common.RenderEmptyGridView(this.grdUserList);
            //}
            //this.labMaxCount.Text = data_rows.ToString();
            //InitPageButton();
        }
        #endregion

        #region 分页相关方法
        #region 设置转向页面下拉列表
        ///// <summary>
        ///// 设置转向页面下增列表
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
        /// 获取当前页码
        /// </summary>
        /// <returns></returns>
        private int CurrentPageIndex()
        {
            //当前页码
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

        #region 设定 首页 下一页 上一页 尾页 按钮是否可用
        ///// <summary>
        ///// 设定 首页 下一页 上一页 尾页 按钮是否可用
        ///// </summary>
        //private void InitPageButton()
        //{
        //    //先全部初始化
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

        #region 处理分页
        ///// <summary>
        ///// 处理分页
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void GotoThePage(object sender, CommandEventArgs e)
        //{
        //    switch (e.CommandArgument.ToString())
        //    {
        //        case "首页":
        //            this.pagenum.Text = "1";
        //            break;
        //        case "上一页":
        //            this.pagenum.Text = Convert.ToString(Convert.ToInt32(this.pagenum.Text.Trim()) - 1);
        //            break;
        //        case "下一页":
        //            this.pagenum.Text = Convert.ToString(Convert.ToInt32(this.pagenum.Text.Trim()) + 1);
        //            break;
        //        case "末页":
        //            this.pagenum.Text = this.maxpage.Value;
        //            break;
        //    }

        //    DoBind();
        //}
        #endregion

        #region 转到指定的页码上去
        ///// <summary>
        ///// 转到指定的页码上去
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

        #region 排序条件
        /// <summary>
        /// 获取排序条件
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
        #endregion 分页相关方法

        #region 分页效果处理
        /// <summary>
        /// 处理分页
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
                            throw new Exception("页数非法。");
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
            // 翻页按钮样式。
            string normalBtnClass = "c-l-btn c-l-btn-plain";
            string disabledBtnClass = "c-l-btn c-l-btn-plain c-l-btn-disabled";

            // 总页数。
            int pageCount = (int)Math.Ceiling((double)rowCount / pageSize);

            // 设置第一页、上一页按钮是否可用。
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

            // 设置下一页、最后一页按钮是否可用。
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

            // 设置当前页、总页数的值。
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
