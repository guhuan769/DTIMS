using System;
using System.Collections.Generic;
using DTIMS.Comm;
using DTIMS.Comm.Entity;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sys.Project.Common;
using Sys.EasyUiEx;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace DTIMS.Web
{
    public partial class SysLog : WebPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string act = Request.Params["act"];
                if (act != null && act.Equals("Log"))
                {
                    int index = int.Parse(Request.Params["index"]);
                    int size = int.Parse(Request.Params["size"]);
                    string User = Request.Form["User"].ToString().Trim();
                    string startTime = Request.Form["startTime"].ToString().Trim();
                    string endTime = Request.Form["endTime"].ToString().Trim();

                    string where = "1=1 ";
                    if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
                    {
                        where += string.Format(" AND LOG_DATETIME between '{0} 00:00:00' and '{1} 23:59:59'", startTime, endTime);
                    }

                    //判断是否需要按帐号查询
                    if (User.Length > 0)
                    {
                        where += " AND O.User_Login='" + User + "'";
                    }

                    ProcParaEntity para = new ProcParaEntity()
                    {
                        Fileds = "F.LOG_ID,F.LOG_Client_IP AS Client_IP,F.User_ID,convert(varchar(25),F.LOG_DATETIME,120) AS LOG_DATETIME,(CASE F.LOG_Mode WHEN 'U' THEN '修改' WHEN 'A' THEN '添加' WHEN 'D' THEN '删除' END) AS LOG_Mode,F.LOG_CONTENT,I.LItem_Name,O.User_Login ",
                        TableName = "S_LogFile AS F LEFT JOIN S_LogItem AS I ON F.LItem_ID=I.LItem_ID LEFT JOIN Sys_User AS O ON F.User_ID=O.User_ID",
                        PrimaryKey = "LOG_ID",
                        Order = "F.LOG_ID DESC ",
                        PageIndex = index,
                        PageSize = size,
                        Where = where
                    };

                    ProcResultEntity result = DTIMS.Comm.Data.Helper.GetPagingProcData(para);
                    if (result.Success)
                    {
                        Sys.EasyUiEx.Data.DataGrid grid = Sys.EasyUiEx.Data.DataGrid.GetInstance(result.dataTable);
                        //Inphase.Sys.Comm.EasyUi.Data.DataGrid grid = Sys.Comm.EasyUi.Data.DataGrid.GetInstance(result.dataTable);
                        grid.Total = result.TotalRows;
                        EasyUi.ResponseData(grid);
                    }
                    else
                    {
                        EasyUi.Alert(false, result.Descr, IconEnum.Error);
                    }
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                // 一旦捕获了其他异常，那么这个捕获不能省，因为 EasyUi.Alert()、EasyUi.ResponseData() 方法内部必定会抛出该异常。
            }
            catch (Exception ex)
            {
                EasyUi.Alert(false, ex.Message, IconEnum.Error);
            }
        }

        #region
        public static DataTable GetStudentInfo(int StuId)
        {
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase =
                DatabaseFactory.CreateDatabase(Sys.Com.Common.SystemWebFormulation.Parameter("MainDataBaseName"));
            string sqlstr = @"SELECT [ID],[SFZMHM],[NAME] ,[TESTDATE],[LSH] ,[KSKM] ,CASE WHEN JFBJ = 1 THEN '未缴费' when JFBJ = 0 THEN '已缴费'  end JFBJ ,[KSCS] ,[KZCS],[PASS]
                            FROM [JFIMS].[dbo].[T_Student] WHERE ID = " + StuId+"";
            DataTable dt = null;
            DbCommand cmdSelect = null;
            //Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(dataBaseName);
            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            try
            {
                dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception e)
            {
            }

            return dt;
        }


        public static DataTable GetConfig(string kskm)
        {
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase =
                DatabaseFactory.CreateDatabase(Sys.Com.Common.SystemWebFormulation.Parameter("MainDataBaseName"));
            string sqlstr = @"SELECT *
                            FROM IPConifg where KM = " + kskm+"";
            DataTable dt = null;
            DbCommand cmdSelect = null;
            //Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(dataBaseName);
            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            try
            {
                dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception e)
            {
            }

            return dt;
        }

        public static DataTable GetConfig()
        {
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase =
                DatabaseFactory.CreateDatabase(Sys.Com.Common.SystemWebFormulation.Parameter("MainDataBaseName"));
            string sqlstr = @"SELECT IP
                            FROM IPConifg";
            DataTable dt = null;
            DbCommand cmdSelect = null;
            //Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(dataBaseName);
            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            try
            {
                dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception e)
            {
            }

            return dt;
        }
        #endregion
    }//end public partial class SysLog : Inphase.AspxTask.WebPageBase
}//end namespace Inphase.Project.Sys.SystemLog