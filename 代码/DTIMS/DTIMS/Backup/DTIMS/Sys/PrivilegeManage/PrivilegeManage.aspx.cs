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
        public int data_rows = 0;//总条数
        public int max_page_num = 0;//尾页页码
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //加载地区树
                    this.trvAreaInfo.Nodes.Add(AreaTree.CreateAreaTree(this.Oper.Area_ID));

                    //设置默认选中节点
                    if (this.trvAreaInfo.Nodes.Count > 0)
                    {
                        this.trvAreaInfo.Nodes[0].Selected = true;
                        this.DoBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("错误", "初始化页面出错，详细:" + ex.Message, MessageType.info));
            }
        }

        #region DataGrid事件
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

                //在名称上添加超链接
                HyperLink hl = new HyperLink();
                hl.Text = data["PrivGroup_Name"].ToString();
                hl.NavigateUrl = "#";
                hl.Attributes.Add("style", "cursor:pointer");
                hl.Attributes.Add("onClick", string.Format("return OpenWindow('{0}','{1}');",
                    data["PrivGroup_ID"].ToString().Trim(), data["User_ID"].ToString().Trim()));
                hl.ToolTip = "查看详细";
                e.Row.Cells[1].Controls.Add(hl);

                ////添加操作的几个按钮
                //Image img = new Image();
                //img.ImageUrl = "../../Images/Common/edit.png";
                //img.Attributes.Add("style", "cursor:hand");
                //img.Attributes.Add("onClick", string.Format("return OpenWindow('{0}');", data["PrivGroup_ID"].ToString().Trim()));
                //img.ToolTip = "修改";
                //e.Row.Cells[this.grdPrivilegeGroup.Columns.Count - 1].Controls.Add(img);

                //////删除
                ////LinkButton delBtn = new LinkButton();
                ////delBtn.CommandName = "del";
                ////delBtn.ID = "delBtn";
                ////delBtn.Text = "删除";
                ////delBtn.CommandArgument = data["PrivGroup_ID"].ToString().Trim();
                ////img = new Image();
                ////img.ImageUrl = "../../Images/Common/delete.gif";
                ////img.Attributes.Add("style", "cursor:hand");
                //////img.ToolTip = "删除";
                ////delBtn.Controls.Add(img);
                ////e.Row.Cells[this.grdPrivilegeGroup.Columns.Count - 1].Controls.Add(delBtn);

                ////查看用户
                //img = new Image();
                //img.ImageUrl = "../../Images/Common/users.ico";
                //img.Attributes.Add("style", "cursor:hand");
                //img.Attributes.Add("onClick", string.Format("return ShowUserList('{0}');", data["PrivGroup_ID"].ToString().Trim()));
                //img.ToolTip = "查看当前组下的所有用户";
                //e.Row.Cells[this.grdPrivilegeGroup.Columns.Count - 1].Controls.Add(img);
            }
        }

        protected void grdPrivilegeGroup_SortCommand(object source, GridViewSortEventArgs e)
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
            int cur_page_index = 0;
            DTIMS.DataAccess.Common comm = new DTIMS.DataAccess.Common();
            string inner = @"Sys_PrivilegeGroup INNER JOIN
                      Sys_User ON Sys_PrivilegeGroup.User_ID = Sys_User.User_ID INNER JOIN
                      S_MainArea ON Sys_User.MainArea_ID = S_MainArea.MainArea_ID";
            string fields = @"Sys_PrivilegeGroup.PrivGroup_ID, Sys_PrivilegeGroup.User_ID, Sys_PrivilegeGroup.PrivGroup_Name, Sys_PrivilegeGroup.PrivGroup_Desc, 
                      Sys_User.User_Name, Sys_User.User_Login, S_MainArea.MainArea_Name";

            //条件
            string fifter = "";

            if (this.Oper.IsSuper)//超级用户查询当前地区所有权限组，否则只查询自己建议的
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

            //进行绑定前的排序
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
            //if (this.grdPrivilegeGroup.Rows.Count == 0)
            //{
            //    Inphase.CTQS.Common.Common.RenderEmptyGridView(this.grdPrivilegeGroup);
            //}
            //this.labMaxCount.Text = data_rows.ToString();
            //InitPageButton();
        }
        #endregion

        //#region 分页相关方法
        //#region 设置转向页面下拉列表
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
        //#endregion

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

        //#region 设定 首页 下一页 上一页 尾页 按钮是否可用
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
        //#endregion

        //#region 处理分页
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
        //#endregion

        //#region 转到指定的页码上去
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
        //#endregion

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
        //    DropDownList ddl = (DropDownList)grdPrivilegeGroup.BottomPagerRow.Cells[0].FindControl("ddlPaging");
        //    grdPrivilegeGroup.PageIndex = ddl.SelectedIndex;
        //    this.DoBind();
        //}
        //#endregion 分页相关方法

        #region 选中树节点后刷新数据
        protected void trvAreaInfo_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (this.trvAreaInfo.SelectedNode != null)
            {
                this.RefreshData(null, null);
            }
        }
        #endregion 选中树节点后刷新数据

        #region 刷新数据
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

                    Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("提示", "删除成功", MessageType.info, "initDelEvent"));

                    //记录日志
                    DTIMS.DataAcess.Common.SysLog.Add(this.Oper.OperatorId, DTIMS.DataAcess.Common.SysLog.LogMode.UpDate, DTIMS.DataAcess.Common.SysLog.LogItemID.RightManage,
                          string.Format("删除权限组：{0}", group.PrivGroup_Name), this.Oper.Client_IP);

                    //重新绑定数据
                    this.RefreshData(null, null);
                }
                catch (Exception ex)
                {
                    Common.ScriptManagerRegister(this.UpdatePanel1, JqueryComm.ShowMessage("错误", ex.Message, MessageType.error, "initDelEvent"));
                }
            }
        }

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

    }//end public partial class PrivilegeManage : System.Web.UI.Page
}
