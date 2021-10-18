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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Sys.Project.Common;
using System.Web.Services;
using System.Net.Sockets;
using PaymentModel;
using System.Data.SqlClient;
using BJ.DTIMS.DataAccess;
using BJ.DTIMS.Comm;
using System.EnterpriseServices;

namespace DTIMS.Web
{
    /// <summary>
    /// LogInfo 的摘要说明。
    /// </summary>
    public partial class hbmdInfo : WebPageBase
    {
        private static string StuId = null;
        private static string Kskm = null;
        private static string NAME = null;
        private string operMode = null;
        private static string sfzmhm = null;
        private static string OperatorId = null;

        private void Page_Load(object sender, System.EventArgs e)
        {
            // 在此处放置用户代码以初始化页面
            StuId = Request.QueryString["Num"];
            Operator oper = Session["Operator"] as Operator;
            OperatorId = oper.OperatorId;
            #region 初使化
            if (!Page.IsPostBack)
            {
                try
                {

                    DataTable dt = SysLog.GetStudentInfo(Convert.ToInt32(StuId));
                    sfzmhm = dt.Rows[0]["SFZMHM"].ToString();
                    Kskm = dt.Rows[0]["Kskm"].ToString();
                    NAME = dt.Rows[0]["NAME"].ToString();
                    lbSfzmhm.Text = dt.Rows[0]["SFZMHM"].ToString();
                    lbKskm.Text = dt.Rows[0]["Kskm"].ToString();
                    lbJfbj.Text = dt.Rows[0]["Jfbj"].ToString();
                    lbKscs.Text = dt.Rows[0]["Kscs"].ToString();
                    lbKzcs.Text = dt.Rows[0]["Kzcs"].ToString();
                }
                catch (Exception et)
                {
                    this.ShowMessage(this.Page, et.Message);
                }
            }
            #endregion
        }

        #region Web 窗体设计器生成的代码
        //override protected void OnInit(EventArgs e)
        //{
        //    //
        //    // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
        //    //
        //    InitializeComponent();
        //    base.OnInit(e);
        //}
        static UdpClient udp = new UdpClient();
        [WebMethod]
        public static string SendInfo()
        {
            //return "true|192.168.1.1|" + "message";
            DataTable dtcon = SysLog.GetConfig(Kskm);
            if (dtcon.Rows.Count == 1)
            {
                try
                {
                    //PASS 1 黑名单 未缴费  1  白名单 0 缴费 0
                    //JFBJ  0 未缴费  1缴费
                    //查询
                    //如果是黑名单，判断是否缴费，如果缴费直接开闸并修改成白名单。
                    //如果是未缴费，提示是否需要继续开闸，开闸修改成白名单，不开闸直接返回。
                    StringBuilder strSql = new StringBuilder();
                    strSql = strSql.Append(" update [dbo].[T_Student] set pass = 0,jfbj = 0 ");
                    //strSql.Append(" where ID=@id");
                    SqlParameter[] parameters = {
                              new SqlParameter("@id",SqlDbType.VarChar,50) };
                    parameters[0].Value = StuId;
                    string ip = dtcon.Rows[0]["IP"].ToString();
                    ResultClass ret = new ResultClass();
                    ret = PaymentClient.InterfaceMethod.PaymentVliadateClient(sfzmhm, NAME, Kskm, "");
                   
                    if (ret.PassReason == PaymentModel.Enum.enumPassReason.Success)
                    {
                        if (ip == "")
                            return "请配置IP!";
                        int prot = Convert.ToInt32(dtcon.Rows[0]["PROT"]);
                        udp.Connect(ip, prot);
                        Byte[] sendByte = Encoding.Default.GetBytes(sfzmhm.Trim() + "|open");
                        udp.Send(sendByte, sendByte.Length);

                        //int row = 0;
                        //row = SqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
                        DTIMS.DataAcess.Common.SysLog.Add(OperatorId, DTIMS.DataAcess.Common.SysLog.LogMode.Insert,
                DTIMS.DataAcess.Common.SysLog.LogItemID.AreaManage, "该" + sfzmhm + "学员" + "开闸成功！", GetIpClass.GetLocalIP());
                        return "该学员已缴费,开闸成功!";
                    }
                    else if (ret.PassReason == PaymentModel.Enum.enumPassReason.NoPay)
                    {
                        return "true|" + ip + "|" + "该学员未缴费!";
                    }
                    else
                    {
                        return "开闸失败!";
                    }
                    //if (ret.result == true)
                    //{
                    //    //if (Convert.ToInt32(ret.PassReason) == 0)
                    //    //{


                    //    //}
                    //}
                    //else
                    //{
                    //    //返回学员所有信息 比如未缴费 黑名单
                    //    return "true|" + ip + "|" + ret.message.ToString();
                    //}
                    //Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    return "配置信息错误(或接口地址不通)!" + ex.ToString();
                }
                //return "发送成功!";
            }
            else
            {
                return "请指定目标主机或者配置了多个目标主机!";
            }

        }

        [WebMethod]
        public static string SendKz(string ip)
        {
            try
            {
                UdpClient udp = new UdpClient();
                udp.Connect(ip, 888);
                Byte[] sendByte = Encoding.Default.GetBytes(ip + "|open");
                udp.Send(sendByte, sendByte.Length);
                DTIMS.DataAcess.Common.SysLog.Add(OperatorId, DTIMS.DataAcess.Common.SysLog.LogMode.Insert,
               DTIMS.DataAcess.Common.SysLog.LogItemID.AreaManage, "该" + sfzmhm + "学员,未缴费开闸" + "开闸成功！", GetIpClass.GetLocalIP());
                return "开闸成功";
            }
            catch (Exception ex)
            {
                return "配置信息错误";
            }

        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {


        }
        #endregion
    }
}
