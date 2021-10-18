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
    /// LogInfo ��ժҪ˵����
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
            // �ڴ˴������û������Գ�ʼ��ҳ��
            StuId = Request.QueryString["Num"];
            Operator oper = Session["Operator"] as Operator;
            OperatorId = oper.OperatorId;
            #region ��ʹ��
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

        #region Web ������������ɵĴ���
        //override protected void OnInit(EventArgs e)
        //{
        //    //
        //    // CODEGEN: �õ����� ASP.NET Web ���������������ġ�
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
                    //PASS 1 ������ δ�ɷ�  1  ������ 0 �ɷ� 0
                    //JFBJ  0 δ�ɷ�  1�ɷ�
                    //��ѯ
                    //����Ǻ��������ж��Ƿ�ɷѣ�����ɷ�ֱ�ӿ�բ���޸ĳɰ�������
                    //�����δ�ɷѣ���ʾ�Ƿ���Ҫ������բ����բ�޸ĳɰ�����������բֱ�ӷ��ء�
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
                            return "������IP!";
                        int prot = Convert.ToInt32(dtcon.Rows[0]["PROT"]);
                        udp.Connect(ip, prot);
                        Byte[] sendByte = Encoding.Default.GetBytes(sfzmhm.Trim() + "|open");
                        udp.Send(sendByte, sendByte.Length);

                        //int row = 0;
                        //row = SqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
                        DTIMS.DataAcess.Common.SysLog.Add(OperatorId, DTIMS.DataAcess.Common.SysLog.LogMode.Insert,
                DTIMS.DataAcess.Common.SysLog.LogItemID.AreaManage, "��" + sfzmhm + "ѧԱ" + "��բ�ɹ���", GetIpClass.GetLocalIP());
                        return "��ѧԱ�ѽɷ�,��բ�ɹ�!";
                    }
                    else if (ret.PassReason == PaymentModel.Enum.enumPassReason.NoPay)
                    {
                        return "true|" + ip + "|" + "��ѧԱδ�ɷ�!";
                    }
                    else
                    {
                        return "��բʧ��!";
                    }
                    //if (ret.result == true)
                    //{
                    //    //if (Convert.ToInt32(ret.PassReason) == 0)
                    //    //{


                    //    //}
                    //}
                    //else
                    //{
                    //    //����ѧԱ������Ϣ ����δ�ɷ� ������
                    //    return "true|" + ip + "|" + ret.message.ToString();
                    //}
                    //Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    return "������Ϣ����(��ӿڵ�ַ��ͨ)!" + ex.ToString();
                }
                //return "���ͳɹ�!";
            }
            else
            {
                return "��ָ��Ŀ���������������˶��Ŀ������!";
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
               DTIMS.DataAcess.Common.SysLog.LogItemID.AreaManage, "��" + sfzmhm + "ѧԱ,δ�ɷѿ�բ" + "��բ�ɹ���", GetIpClass.GetLocalIP());
                return "��բ�ɹ�";
            }
            catch (Exception ex)
            {
                return "������Ϣ����";
            }

        }

        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
        /// �˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {


        }
        #endregion
    }
}
