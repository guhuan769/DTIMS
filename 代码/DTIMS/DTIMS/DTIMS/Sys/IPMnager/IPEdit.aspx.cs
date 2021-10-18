using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BJ.DTIMS.DataAccess;
using DTIMS.Comm;
using Sys.Project.Common;

namespace BJ.DTIMS.Sys.IPMnager
{
    public partial class IPEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtProt.Text = "888";
                try
                {

                    string edit = Request["type"].ToString();
                    if (edit == "edit")
                    {
                        string id = Request["id"].ToString();
                        StringBuilder strSql = new StringBuilder();
                        strSql = strSql.Append("     SELECT * FROM [IPConifg] ");
                        strSql.Append(" where id=@id ");
                        SqlParameter[] parameters = {
                              new SqlParameter("@id",SqlDbType.VarChar,50)};
                        parameters[0].Value = id;
                        DataTable dt = SqlHelper.ExecuteDataTable(strSql.ToString(), parameters);
                        txtIP.Text = dt.Rows[0]["IP"].ToString();
                        txtProt.Text = dt.Rows[0]["prot"].ToString();
                        string km = dt.Rows[0]["km"].ToString();
                        ddkm.SelectedIndex = Convert.ToInt32(km) - 1;
                    }
                    else
                    {
                        btnUpdate.Text = "保   存";
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        [WebMethod]
        public static string IPUpdate()
        {
            //if(txtIP.Text =="")
            //    return "IP不能为空!";
            //if(txtProt.Text == "")
            //    return "端口不能为空!";

            return "11";
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text;
            string prot = txtProt.Text;
            string km = ddkm.Text;
            string edit = Request["type"].ToString();
            if (edit == "edit")
            {
                //开始修改
                string id = Request["id"].ToString();

                StringBuilder strSql = new StringBuilder();

                //strSql = strSql.Append("update sys_usertable set USER_PASSWORD=:newPwd");
                //strSql.Append(" where USER_LOGIN=:presentUser");

                strSql = strSql.Append("update IPConifg set IP =@ip, prot =@prot, KM = @km");
                strSql.Append(" where ID=@id");

                SqlParameter[] parameters = {
                              new SqlParameter("@ip",SqlDbType.VarChar,50),
                              new SqlParameter("@prot",SqlDbType.VarChar,50),
                              new SqlParameter("@km",SqlDbType.VarChar,50),
                              new SqlParameter("@id",SqlDbType.VarChar,50)
            };
                parameters[0].Value = ip;
                parameters[1].Value = prot;
                parameters[2].Value = km;
                parameters[3].Value = id;
                int row = 0;
                row = SqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
                if (row >= 1)
                {
                    string message = "修改成功!";
                    Response.Write("<script>alert(\"" + message + "\")</script>");
                }
                else
                {
                    string message = "修改失败或网络堵塞!";
                    Response.Write("<script>alert(\"" + message + "\")</script>");
                }
            }
            else if (edit == "add")
            {
                StringBuilder strSql = new StringBuilder();

                //strSql = strSql.Append("update sys_usertable set USER_PASSWORD=:newPwd");
                //strSql.Append(" where USER_LOGIN=:presentUser");

                strSql = strSql.Append("INSERT INTO [JFIMS].[dbo].[IPConifg]");
                strSql.Append(" ([ip] ,[prot] ,[KM])VALUES ");
                strSql.Append(" (@ip,@prot ,@km) ");

                SqlParameter[] parameters = {
                              new SqlParameter("@ip",SqlDbType.VarChar,50),
                              new SqlParameter("@prot",SqlDbType.VarChar,50),
                              new SqlParameter("@km",SqlDbType.VarChar,50)
            };
                parameters[0].Value = ip;
                parameters[1].Value = prot;
                parameters[2].Value = km;
                int row = 0;
                row = SqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
                if (row >= 1)
                {


                    string message = "保存成功!";
                    Response.Write("<script>alert(\"" + message + "\")</script>");
                    //Response.Write("<script>$.messager.alert(\"提示\", \"您的IP输入错误!\", \"info\", function () { }); return false;</script>");
                }
                else
                {
                    string message = "保存失败或网络堵塞!";
                    Response.Write("<script>alert(\"" + message + "\")</script>");
                }
            }
        }
    }
}