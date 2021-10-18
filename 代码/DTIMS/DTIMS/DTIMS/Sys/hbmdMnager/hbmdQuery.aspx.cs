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
using Sys.Comm.WebTools;						//Ϊ��ʵ�ֹ��������õ������ռ�
using Sys.Project.Common;
using DTIMS.Comm;
using System.Net.Sockets;
using System.Text;
using System.Web.Services;
using BJ.DTIMS.Comm;
using System.IO;
using CommonFunction;
using System.Web.Script.Serialization;
using System.Collections.Generic;
//using Sys.Comm;

namespace DTIMS.Web
{
    /// <summary>
    /// SystemLog ��ժҪ˵����
    /// </summary>
    public partial class hbmdQuery : WebPageBase
    {
        //private Inphase.WebTools.DataGridFilter fi = null;						//�����������Ķ���
        public int data_rows = 0;//������
        public int page_rows = 15;//ÿҳ��ʾ����        
        public int max_page_num = 0;//βҳҳ��  
        private void Page_Load(object sender, System.EventArgs e)
        {
            // �ڴ˴������û������Գ�ʼ��ҳ��

            txt_sfzs = txt_sfz.Text.Trim().ToString();
            page_rowss = page_rows;
            CurrentPageIndexs = CurrentPageIndex();
            txtStarts = this.txtStart.Value;
            dtpkEndDates = this.dtpkEndDate.Value;

            //ʵ�ֹ������Ĵ���
            //fi = new DataGridFilter(this.DataGridUserPort);
            //fi.AllowFilter = true;
            //fi.AddColumn("LOG_ID");
            //fi.AddColumn("OPER_Login");
            //fi.AddColumn("LOG_DATETIME");
            //fi.AddColumn("LOG_Mode");
            //fi.AddColumn("LItem_Name");
            //fi.AddColumn("LOG_CONTENT");
            DataTable dtcon = SysLog.GetConfig();
            txtIPs.DataSource = dtcon;
            txtIPs.DataTextField = "IP";//������Ҫ��ȡ�����ݱ��������
            txtIPs.DataBind();//���ݰ�
            string temp2 = Request.Form["actionNo"];
            Operator oper = (Operator)Session["Operator"];
            //if (Page.IsPostBack)
            //{
            //    txt_sfzs = txt_sfz.Text.Trim().ToString();
            //    page_rowss = page_rows;
            //    CurrentPageIndexs = CurrentPageIndex();
            //    txtStarts = this.txtStart.Value;
            //    dtpkEndDates = this.dtpkEndDate.Value;
            //    ExportByWeb("ExportDemo.xlsx");
            //}
            if (!Page.IsPostBack || temp2 == "1" || temp2 == "2")
            {
                //���ݰ�
                if (temp2 == "2")
                {
                    // this.DataGridUserPort.CurrentPageIndex = this.DataGridUserPort.PageCount - 1;
                    ViewState.Remove("sortOrder");
                }

                this.txtStart.Value = DateTime.Now.ToString("yyyy-MM-dd");
                this.dtpkEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                DoBind();
            }
            else
            {
                //�õ�ҳ���ύ��ɾ����ĳ�е�ID
                string temp = Request.Form["delete1"].ToString();
                if (temp != null && temp != "")
                {
                    //��ɾ������
                    if (temp == "a")
                    {
                        try
                        {
                            DTIMS.DataAcess.Common.SysLog.Delete();
                            DTIMS.DataAcess.Common.SysLog.Add(this.Oper.OperatorId,
                             DTIMS.DataAcess.Common.SysLog.LogMode.Delete,
                             DTIMS.DataAcess.Common.SysLog.LogItemID.SyslogManage, "����Ա��" + this.Oper.OperName + "��ɾ��������־��Ϣ��", oper.Client_IP);
                            this.DoBind();
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
            //this.DataGridUserPort.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.DataGridUserPort_PageIndexChanged);
            //this.DataGridUserPort.PreRender += new System.EventHandler(this.DataGridUserPort_PreRender);
            //this.DataGridUserPort.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.DataGridUserPort_SortCommand);
            //this.DataGridUserPort.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGridUserPort_ItemDataBound);
        }
        #endregion


        private void DoBind()
        {
            #region �ж�����������Ƿ���ȷ
            if (!string.IsNullOrEmpty(this.txtStart.Value) && !string.IsNullOrEmpty(this.dtpkEndDate.Value))
            {
                DateTime startTime = Convert.ToDateTime(this.txtStart.Value);
                DateTime endTime = Convert.ToDateTime(this.dtpkEndDate.Value);
                if (endTime < startTime)
                {
                    string msg = JqueryComm.ShowMessage("����", "��ʼ���ڱ���С�ڽ�������", MessageType.error);
                    Common.ScriptManagerRegister(this.UpdatePanel1, msg);
                    //this.DataGridUserPort.DataSource = null;
                    //this.DataGridUserPort.DataBind();
                    Common.RenderEmptyGridView(this.grdLog);
                    return;
                }
            }
            #endregion
            //if(txt_sfz.Text != null || txt_sfz.Text != "")


            int cur_page_index = 0;
            DataTable dt = DTIMS.DataAcess.Common.SysLog.GetDataHbmdTable(txt_sfz.Text.Trim().ToString(), page_rows, CurrentPageIndex(), " ID DESC",
               this.txtStart.Value, this.dtpkEndDate.Value, Convert.ToInt32(this.hbMdBtn.SelectedValue), out data_rows, out cur_page_index);

            //���а�ǰ������
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
                lb.Attributes.Add("onClick", "return DoView('hbmdInfo.aspx?num=" + e.Row.Cells[0].Text + "&from=a&rad='+Math.random());");
                lb.ToolTip = "�鿴";

                System.Web.UI.WebControls.ImageButton lb1 = new System.Web.UI.WebControls.ImageButton();
                lb1.ImageUrl = "../../Images/Common/edit.png";
                lb1.Attributes.Add("style", "cursor:pointer");//hbmdAdd
                lb1.Attributes.Add("onClick", "return Edit('hbmdAdd.aspx?type=edit&num=" + e.Row.Cells[0].Text + "&from=a&rad='+Math.random());");
                lb1.ToolTip = "�޸�";
                e.Row.Cells[this.grdLog.Columns.Count - 1].Controls.Add(lb);
                e.Row.Cells[this.grdLog.Columns.Count - 1].Controls.Add(lb1);
                //GH
                //if (e.Row.Cells[5].Text.Length > 40)
                //{
                //    e.Row.Cells[5].Text = e.Row.Cells[5].Text.Substring(0, 40);
                //}
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

        protected void Button2_Click(object sender, EventArgs e)
        {
            //DoBind();
        }

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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DoBind();
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


        [WebMethod]
        public static string SendInfos(string ip)
        {
            try
            {
                UdpClient udp = new UdpClient();
                udp.Connect(ip, 888);
                Byte[] sendByte = Encoding.Default.GetBytes(ip + "|open");
                udp.Send(sendByte, sendByte.Length);
                //DTIMS.DataAcess.Common.SysLog.Add(OperatorId, DTIMS.DataAcess.Common.SysLog.LogMode.Insert,
                //DTIMS.DataAcess.Common.SysLog.LogItemID.AreaManage, "��" + sfzmhm + "ѧԱ" + "��բ�ɹ���", GetIpClass.GetLocalIP());
                return "���ͳɹ�";
            }
            catch (Exception ex)
            {
                return "������Ϣ����" + ex.ToString();
            }

        }



        //private static string txt_sfzs = null;
        //private static int page_rowss = 0;
        //private static int CurrentPageIndexs = 0;
        //private static string txtStarts = null;
        //private static string dtpkEndDates = null;
        public static MemoryStream ExcelStream()
        {
            DataTable dtTable = //GetDtTable();
             DTIMS.DataAcess.Common.SysLog.GetDataHbmdTable(txt_sfzs, page_rowss, CurrentPageIndexs, " ID DESC",
            txtStarts, dtpkEndDates, 0, out data_rowss, out cur_page_indexs);

            //GetDtTable();//gh
            return ExcelHelper.ExportDT(dtTable, "HeaderText");

        }

        private static DataTable GetDtTable()
        {

            string path = HttpContext.Current.Request.MapPath("~/App_Data/excel2007.xlsx");
            //����ZK��ExcelHelper
            DataTable dtTable = ExcelHelper.ImportExceltoDt(path);
            return dtTable;
        }

        public static void ExportByWeb(string strFileName)
        {
            HttpContext curContext = HttpContext.Current;

            // ���ñ���͸�����ʽ
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));
            curContext.Response.BinaryWrite(ExcelStream().GetBuffer());
            //curContext.Response.Redirect("hbmdQuery.aspx", false);
            curContext.Response.End();
        }
        private static string txt_sfzs = null;
        private static int page_rowss = 0;
        private static int CurrentPageIndexs = 0;
        private static string txtStarts = null;
        private static string dtpkEndDates = null;
        private static int data_rowss = 0;
        private static int cur_page_indexs = 0;
        protected void lbDerive_Click(object sender, EventArgs e)
        {
            try
            {

                //ExportByWeb("ExportDemo.xlsx");


            }
            catch (Exception ex)
            {

            }
        }

        [WebMethod]
        public static string ExcelJson(string starttime, string endtime)
        {
            //https://blog.csdn.net/liguoqingxjxcc/article/details/81630498
            DataTable dt = //GetDtTable();
            DTIMS.DataAcess.Common.SysLog.GetDataHbmdTable(txt_sfzs, 999999999, 1, " ID DESC",
           starttime, endtime, 0, out data_rowss, out cur_page_indexs);

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //ȡ�������ֵ
            ArrayList arrayList = new ArrayList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ϵͳ���", "ϵͳ���");
            dictionary.Add("���֤", "���֤");
            dictionary.Add("����", "����");
            dictionary.Add("����", "����");
            dictionary.Add("���Կ�Ŀ", "���Կ�Ŀ");
            dictionary.Add("���Դ���", "���Դ���");
            dictionary.Add("�Ƿ�ɷ�", "�Ƿ�ɷ�");
            arrayList.Add(dictionary); //ArrayList��������Ӽ�ֵ
            foreach (DataRow dataRow in dt.Rows)
            {
                dictionary = new Dictionary<string, object>();  //ʵ����һ����������
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    string ColunName = string.Empty;
                    string dataRowColunName = string.Empty;
                    if (dataColumn.ColumnName.Equals("ID"))
                    {
                        ColunName = "ϵͳ���";
                        dataRowColunName = dataRow[dataColumn.ColumnName].ToString();
                    }
                    if (dataColumn.ColumnName.Equals("SFZMHM"))
                    {
                        ColunName = "���֤";
                        dataRowColunName = dataRow[dataColumn.ColumnName].ToString();
                    }
                    if (dataColumn.ColumnName.Equals("NAME"))
                    {
                        ColunName = "����";
                        dataRowColunName = dataRow[dataColumn.ColumnName].ToString();
                    }
                    if (dataColumn.ColumnName.Equals("TESTDATE"))
                    {
                        ColunName = "����";
                        dataRowColunName = dataRow[dataColumn.ColumnName].ToString();
                    }
                    if (dataColumn.ColumnName.Equals("KSKM"))
                    {
                        ColunName = "���Կ�Ŀ";
                        dataRowColunName = dataRow[dataColumn.ColumnName].ToString();
                    }
                    if (dataColumn.ColumnName.Equals("KSCS"))
                    {
                        ColunName = "���Դ���";
                        dataRowColunName = dataRow[dataColumn.ColumnName].ToString();
                    }
                    if (dataColumn.ColumnName.Equals("PASS"))
                    {
                        ColunName = "�Ƿ�ɷ�";
                        if (dataRow[dataColumn.ColumnName].ToString().Equals("1"))
                        {
                            dataRowColunName = "δ�ɷ�";
                        }
                        else
                        {
                            dataRowColunName = "�ѽɷ�";
                        }

                    }

                    if (ColunName != string.Empty)
                    {
                        dictionary.Add(ColunName, dataRowColunName);
                        ColunName = string.Empty;
                    }
                }
                arrayList.Add(dictionary); //ArrayList��������Ӽ�ֵ
            }

            return javaScriptSerializer.Serialize(arrayList);
        }

        //public static string ToJson(this DataTable dt)
        //{
        //    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        //    javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //ȡ�������ֵ
        //    ArrayList arrayList = new ArrayList();
        //    foreach (DataRow dataRow in dt.Rows)
        //    {
        //        Dictionary<string, object> dictionary = new Dictionary<string, object>();  //ʵ����һ����������
        //        foreach (DataColumn dataColumn in dt.Columns)
        //        {
        //            dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
        //        }
        //        arrayList.Add(dictionary); //ArrayList��������Ӽ�ֵ
        //    }

        //    return javaScriptSerializer.Serialize(arrayList);  //����һ��json�ַ���
        //}

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        udp.Connect(txtIPs.Text.Trim(), 888);
        //        Byte[] sendByte = Encoding.Default.GetBytes(txtIPs.Text.Trim() + "|OPEN");
        //        // Console.WriteLine(DateTime.Now.ToLongDateString() + "��ĿԤ��,�˵��д������");
        //        udp.Send(sendByte, sendByte.Length);
        //        string message = "���ͳɹ�!";
        //        Response.Write("<script>alert(\"" + message + "\")</script>");
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = "������Ϣ����";
        //        Response.Write("<script>alert(\"" + message + "\")</script>");
        //    }
        //}

    }
}
