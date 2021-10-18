using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using BJ.AspxTask;
using BJ.Sys.EasyUiEx;
using BJ.DTIMS.Comm;
using BJ.DTIMS.Comm.Entity;
using BJ.DTIMS.Comm.Data;
using BJ.DTIMS.Comm.Helper;

namespace Inphase.CTQS.App.Test
{
    public partial class DataGrid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request.Params["action"];
            if (!string.IsNullOrEmpty(action))
            {
                switch (action)
                {
                    case "query":
                        this.Query();
                        break;
                    default:
                        break;
                }
            }
        }

        private void Query()
        {
            EasyUiHelper easUi = new EasyUiHelper();
            try
            {
                string mdn = Request.Params["mdn"];
                int pageIndex = Convert.ToInt16(Request.Params["pageIndex"]);
                int pageSize = Convert.ToInt16(Request.Params["pageSize"]);

                string where = "";
                if (!string.IsNullOrEmpty(mdn))
                {
                    where = "HLR_OperationLogInfo.MDN='" + mdn + "'";
                }

                string tableName = @"
                                    HLR_OperationLogInfo 
                                INNER JOIN
                                    HLR_OperationType 
                                ON 
                                    HLR_OperationLogInfo.HLR_OperationTypeID = HLR_OperationType.HLR_OperationTypeID
                                INNER JOIN
                                    C_OperationSource 
                                ON 
                                    HLR_OperationLogInfo.OperationSourceID = C_OperationSource.OperationSourceID 
                                LEFT JOIN
                                    Sys_User 
                                ON 
                                    HLR_OperationLogInfo.User_ID = Sys_User.User_ID";

                string fieldName = @"HLR_OperationLogInfo.HLR_LogInfoID, 
                                                    HLR_OperationLogInfo.MDN,HLR_OperationLogInfo.IMSI,  
                                                    HLR_OperationLogInfo.HLR_OperationStatus,
                                                    HLR_OperationLogInfo.HLR_OperationTime, 
                                                    HLR_OperationType.HLR_OperationTypeName,
                                                    HLR_OperationLogInfo.Client_IP";

                //实例华调用分页存储过程的参数
                ProcParaEntity para = new ProcParaEntity
                {
                    Fileds = fieldName,
                    TableName = tableName,
                    Order = "HLR_OperationLogInfo.HLR_LogInfoID desc",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    PrimaryKey = "HLR_OperationLogInfo.HLR_LogInfoID",
                    Where = where,
                };

                //执行分页存储过程
                ProcResultEntity query = Helper.GetPagingProcData(para);
                if (query.Success)
                {
                    BJ.Sys.EasyUiEx.Data.DataGrid dataGrid = BJ.Sys.EasyUiEx.Data.DataGrid.GetInstance(query.dataTable);
                    dataGrid.Total = query.TotalRows;
                    easUi.ResponseData(true, "", EasyUiTypeEnum.DataGrid, dataGrid);
                }
                else
                {
                    throw new Exception(query.Descr);
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                easUi.Alert(false, ex.Message, BJ.Sys.EasyUiEx.IconEnum.Error);
            }
        }

        #region 生成DataTableJson数据
        public string CreateDataTableJsonData(DataTable dt, int total)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.AppendFormat("total\":{0},", total.ToString());
            jsonBuilder.Append("\"rows");
            jsonBuilder.Append("\":[");
            string dataValue = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");

                    jsonBuilder.Append(dt.Columns[j].ColumnName.Trim());
                    jsonBuilder.Append("\":\"");

                    //处理内容中特殊字符"与\
                    if (dt.Columns[j].DataType.Name == "String")
                    {
                        dataValue = dt.Rows[i][j].ToString().Trim();
                        dataValue = dataValue.ToString().Trim().Replace("\\", "\\" + "\\")
                            .Replace("\"", "\\" + "\"").Replace("\n", "\\n")
                            .Replace("\t", "\\t").Replace("\r", "\\r");

                        jsonBuilder.Append(dataValue);
                    }
                    else
                    {
                        jsonBuilder.Append(dt.Rows[i][j].ToString().Trim());
                    }
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString().Trim();
        }
        #endregion 生成DataGridJson数据

        public static string CreateJsonParameters(DataTable dt)
        {
            /**/
            /* /****************************************************************************
          * Without goingin to the depth of the functioning of this Method, i will try to give an overview
          * As soon as this method gets a DataTable it starts to convert it into JSON String,
          * it takes each row and in each row it grabs the cell name and its data.
          * This kind of JSON is very usefull when developer have to have Column name of the .
          * Values Can be Access on clien in this way. OBJ.HEAD[0].<ColumnName>
          * NOTE: One negative point. by this method user will not be able to call any cell by its index.
         * *************************************************************************/
            StringBuilder JsonString = new StringBuilder();
            //Exception Handling        
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonString.Append("{ ");
                JsonString.Append("\"Head\":[ ");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonString.Append("{ ");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == dt.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + "\"" + dt.Rows[i][j].ToString() + "\"");
                        }
                    }
                    /**/
                    /*end Of String*/
                    if (i == dt.Rows.Count - 1)
                    {
                        JsonString.Append("} ");
                    }
                    else
                    {
                        JsonString.Append("}, ");
                    }
                }
                JsonString.Append("]}");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }


    }
}