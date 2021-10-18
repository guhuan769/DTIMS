using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Sys.Comm.WebTools;						//为了实现过滤条引用的命名空间
using Sys.Project.Common;
using DTIMS.Comm;

namespace DTIMS.Web
{
    /// <summary>
    /// SystemLog 的摘要说明。
    /// </summary>
    public partial class SystemLog : WebPageBase
    {
        //private Inphase.WebTools.DataGridFilter fi = null;						//声明过滤条的对象
        public int data_rows = 0;//总条数
        public int page_rows = 15;//每页显示条数        
        public int max_page_num = 0;//尾页页码  

        private void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面

            //实现过滤条的代码
            //fi = new DataGridFilter(this.DataGridUserPort);
            //fi.AllowFilter = true;
            //fi.AddColumn("LOG_ID");
            //fi.AddColumn("OPER_Login");
            //fi.AddColumn("LOG_DATETIME");
            //fi.AddColumn("LOG_Mode");
            //fi.AddColumn("LItem_Name");
            //fi.AddColumn("LOG_CONTENT");
            string temp2 = Request.Form["actionNo"];
            Operator oper = (Operator)Session["Operator"];

            if (!Page.IsPostBack || temp2 == "1" || temp2 == "2")
            {
                //数据绑定
                if (temp2 == "2")
                {
                    // this.DataGridUserPort.CurrentPageIndex = this.DataGridUserPort.PageCount - 1;
                    ViewState.Remove("sortOrder");
                }

                this.txtStart.Value = DateTime.Now.ToString("yyyy-MM-dd");
                this.dtpkEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                //  DoBind();
            }
            else
            {
                //得到页面提交的删除的某行的ID
                string temp = Request.Form["delete1"].ToString();
                if (temp != null && temp != "")
                {
                    //做删除方法
                    if (temp == "a")
                    {
                        try
                        {
                            DTIMS.DataAcess.Common.SysLog.Delete();
                          DTIMS.DataAcess.Common.SysLog.Add(this.Oper.OperatorId,
                           DTIMS.DataAcess.Common.SysLog.LogMode.Delete,
                           DTIMS.DataAcess.Common.SysLog.LogItemID.SyslogManage, "管理员：" + this.Oper.OperName + "，删除所有日志信息！", oper.Client_IP);
                            //this.DoBind();
                            return;
                        }
                        catch (Exception et)
                        {
                            //this.DoBind();
                            this.ShowMessage(this.Page, et.Message);
                            return;
                        }
                    }
                }
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
            //this.DataGridUserPort.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.DataGridUserPort_PageIndexChanged);
            //this.DataGridUserPort.PreRender += new System.EventHandler(this.DataGridUserPort_PreRender);
            //this.DataGridUserPort.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.DataGridUserPort_SortCommand);
            //this.DataGridUserPort.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGridUserPort_ItemDataBound);
        }
        #endregion


        private void DoBind()
        {
            #region 判断输入的日期是否正确
            if (!string.IsNullOrEmpty(this.txtStart.Value) && !string.IsNullOrEmpty(this.dtpkEndDate.Value))
            {
                DateTime startTime = Convert.ToDateTime(this.txtStart.Value);
                DateTime endTime = Convert.ToDateTime(this.dtpkEndDate.Value);
                if (endTime < startTime)
                {
                    string msg = JqueryComm.ShowMessage("错误", "起始日期必需小于结束日期", MessageType.error);
                   Common.ScriptManagerRegister(this.UpdatePanel1, msg);
                    //this.DataGridUserPort.DataSource = null;
                    //this.DataGridUserPort.DataBind();
                Common.RenderEmptyGridView(this.grdLog);
                    return;
                }
            }
            #endregion


            int cur_page_index = 0;
            DataTable dt = DTIMS.DataAcess.Common.SysLog.GetDataTable(page_rows, CurrentPageIndex(), GetOrderBy(),
                this.txtStart.Value, this.dtpkEndDate.Value, this.txtUserName.Text.Trim(), out data_rows, out cur_page_index);

        
            //进行绑定前的排序
            if (ViewState["sortOrder"] != null)
            {
                dt.DefaultView.Sort = ViewState["sortOrder"].ToString().Trim();
            }

            this.grdLog.DataSource = dt.DefaultView;
            this.grdLog.DataBind();

            if (dt.Rows.Count == 0)
            {
              Common.RenderEmptyGridView(this.grdLog);
            }

            SetPager(cur_page_index - 1, this.grdLog.PageSize, dt.DefaultView.Count, data_rows);
        }


        protected void grdLog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.Image lb = new System.Web.UI.WebControls.Image();
                lb.ImageUrl = "../../Images/Common/view.gif";
                lb.Attributes.Add("style", "cursor:pointer");
                lb.Attributes.Add("onClick", "return DoView('LogInfo.aspx?num=" + e.Row.Cells[0].Text + "&from=a&rad='+Math.random());");
                lb.ToolTip = "查看";

                e.Row.Cells[this.grdLog.Columns.Count - 1].Controls.Add(lb);

                if (e.Row.Cells[5].Text.Length > 40)
                {
                    e.Row.Cells[5].Text = e.Row.Cells[5].Text.Substring(0, 40);
                }
            }
        }


        protected void grdLog_Sorting(object sender, GridViewSortEventArgs e)
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
            DoBind();
        }


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

        protected void Button2_Click(object sender, EventArgs e)
        {
            //DoBind();
        }

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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DoBind();
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
                this.JqueryMessager(this.UpdatePanel1, ex.Message,   Sys.Comm.MessageType.error);
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

    }
}
