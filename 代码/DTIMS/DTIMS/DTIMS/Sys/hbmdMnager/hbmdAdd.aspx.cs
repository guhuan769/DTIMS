using BJ.DTIMS.Comm;
using BJ.DTIMS.DataAccess;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentModel;
using Sys.Project.Common;
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

namespace BJ.DTIMS.Sys.hbmdMnager
{
    public partial class hbmdAdd : System.Web.UI.Page
    {
        static string type = "";
        private static string OperatorId = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            Operator oper = Session["Operator"] as Operator;
            OperatorId = oper.OperatorId;
            string edit = Request["type"].ToString();
            if (edit == "edit")
            {
                string s = "?type = edit & num = 41 & from = a & rad = 0.04523484080715923";

                string id = Request["num"].ToString();
                btnAdd.Text = "修   改";
                StringBuilder strSql = new StringBuilder();
                strSql = strSql.Append("     SELECT * FROM [T_Student] ");
                strSql.Append(" where id=@id ");
                SqlParameter[] parameters = {
                              new SqlParameter("@id",SqlDbType.VarChar,50)};
                parameters[0].Value = id;
                DataTable dt = SqlHelper.ExecuteDataTable(strSql.ToString(), parameters);
                txtCardId.Text = dt.Rows[0]["SFZMHM"].ToString();
                txtName.Text = dt.Rows[0]["NAME"].ToString();
                txtLsh.Text = dt.Rows[0]["lsh"].ToString();
                ddkscs.SelectedValue = dt.Rows[0]["kscs"].ToString();
                IsJf.SelectedValue = dt.Rows[0]["JFBJ"].ToString();
                ddMd.SelectedValue = dt.Rows[0]["PASS"].ToString();
                //ddkm.SelectedValue = dt.Rows[0]["KSKM"].ToString();
                ddkm.SelectedIndex =Convert.ToInt32(dt.Rows[0]["KSKM"].ToString())-1;
            }
            type = edit;
        }
        [WebMethod]
        public static string StudentLoadInfo(string type, string id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql = strSql.Append("     SELECT * FROM [T_Student] ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                          new SqlParameter("@id",SqlDbType.VarChar,50)};
            parameters[0].Value = id;
            DataTable dt = SqlHelper.ExecuteDataTable(strSql.ToString(), parameters);
            return Convert.ToDateTime(dt.Rows[0]["TESTDATE"]).ToString("yyyy-MM-dd");
        }
        [WebMethod]
        public static string AddStudent(string jsonvar, string id)//, string txtName, string txtDate, string txtLsh, string ddkscs, string IsJf, string ddMd, string ddkm)
        {
            //string jsonText = "{\"zone\":\"海淀\",\"zone_en\":\"haidian\"}";
            JObject jo = (JObject)JsonConvert.DeserializeObject(jsonvar);
            //JObject jo1 = (JObject)JsonConvert.DeserializeObject(jsonvar);
            string txtCardId = jo["txtCardId"].ToString();
            string txtName = jo["txtName"].ToString();
            string txtDate = jo["txtDate"].ToString();
            string txtLsh = jo["txtLsh"].ToString();
            string ddkscs = jo["ddkscs"].ToString();
            string IsJf = jo["IsJf"].ToString();
            string ddMd = jo["ddMd"].ToString();
            string ddkm = jo["ddkm"].ToString();
            if (type == "add")
            {
                ResultClass ret = new ResultClass();
                ret = PaymentClient.InterfaceMethod.PaymentVliadateClient(txtCardId, txtName, ddkm, "");
                if (ret.PassReason == PaymentModel.Enum.enumPassReason.Success)
                {
                    //int row = 0;
                    //row = SqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
            //        DTIMS.DataAcess.Common.SysLog.Add(OperatorId, DTIMS.DataAcess.Common.SysLog.LogMode.Insert,
            //DTIMS.DataAcess.Common.SysLog.LogItemID.AreaManage, "该" + txtName + "学员" + "开闸成功！", GetIpClass.GetLocalIP());
                    return "该学员已缴费,添加成功!";
                }
                else if (ret.PassReason == PaymentModel.Enum.enumPassReason.NoPay)
                {
                    return "该学员未缴费,添加成功!";
                }
                else
                {
                    return "添加失败!";
                }
                //    StringBuilder strSql = new StringBuilder();
                //    strSql = strSql.Append(" INSERT INTO [dbo].[T_Student] ");
                //    strSql.Append("  ([SFZMHM],[NAME],[TESTDATE] ,[LSH],[KSKM],[JFBJ],[KZCS],[PASS])  ");
                //    strSql.Append(" VALUES ");
                //    strSql.Append(" (@txtCardId,@txtName,@txtDate,@txtLsh,@ddkm,@IsJf ,@ddkscs,@ddMd) ");
                //    SqlParameter[] parameters = {
                //                  new SqlParameter("@txtCardId",SqlDbType.VarChar,50),
                //                  new SqlParameter("@txtName",SqlDbType.VarChar,50),
                //                  new SqlParameter("@txtDate",SqlDbType.DateTime),
                //                  new SqlParameter("@txtLsh",SqlDbType.VarChar,50),
                //                  new SqlParameter("@ddkscs",SqlDbType.VarChar,50),
                //                  new SqlParameter("@IsJf",SqlDbType.VarChar,50),
                //                  new SqlParameter("@ddMd",SqlDbType.VarChar,50),
                //                  new SqlParameter("@ddkm",SqlDbType.VarChar,50)
                //};
                //    parameters[0].Value = txtCardId;
                //    parameters[1].Value = txtName;
                //    parameters[2].Value = txtDate;
                //    parameters[3].Value = txtLsh;
                //    parameters[4].Value = ddkscs;
                //    parameters[5].Value = IsJf;
                //    parameters[6].Value = ddMd;
                //    parameters[7].Value = ddkm;
                //    int row = 0;
                //    row = SqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
                //    if (row > 0)
                //        return "添加成功!";
                //    else
                //        return "添加失败!";
            }
            else if (type == "edit")
            {
                StringBuilder strSql = new StringBuilder();
                strSql = strSql.Append(" UPDATE [dbo].[T_Student] ");
                strSql.Append("  SET [SFZMHM] = @txtCardId,[NAME] = @txtName,[TESTDATE] = @txtDate,[KSKM] = @ddkm  ");
                strSql.Append("  WHERE ID =@id ");//,[LSH] = @txtLsh,[KSKM] = @ddkm,[JFBJ] = @IsJf,[KSCS] = @ddkscs,[PASS] = @ddMd 
                SqlParameter[] parameters = {
                              new SqlParameter("@txtCardId",SqlDbType.VarChar,50),
                              new SqlParameter("@txtName",SqlDbType.VarChar,50),
                              new SqlParameter("@txtDate",SqlDbType.DateTime),
                              new SqlParameter("@txtLsh",SqlDbType.VarChar,50),
                              new SqlParameter("@ddkscs",SqlDbType.VarChar,50),
                              new SqlParameter("@IsJf",SqlDbType.VarChar,50),
                              new SqlParameter("@ddMd",SqlDbType.VarChar,50),
                              new SqlParameter("@ddkm",SqlDbType.VarChar,50),
                              new SqlParameter("@id",SqlDbType.VarChar,50)
            };
                parameters[0].Value = txtCardId;
                parameters[1].Value = txtName;
                parameters[2].Value = txtDate;
                parameters[3].Value = txtLsh;
                parameters[4].Value = ddkscs;
                parameters[5].Value = IsJf;
                parameters[6].Value = ddMd;
                parameters[7].Value = ddkm;
                parameters[8].Value = id;
                int row = 0;
                row = SqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
                if (row > 0)
                    return "修改成功!";
                else
                    return "修改失败!";
            }
            return "";
        }
    }
}