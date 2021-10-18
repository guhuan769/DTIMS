using System;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Sys.Comm.WebTools;

namespace Sys.Project.Common
{
    /// <summary>
    /// SuperAreaRange 的摘要说明。
    /// </summary>
    [Serializable]
    public class SuperAreaRange : IAreaInfo
    {
        private String operId = null;
        public SuperAreaRange(String id)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            operId = id.Trim();
        }

        #region IAreaInfo 成员
        public String GetYZFAreaList(String dataBaseName)
        {
           StringBuilder sb = new StringBuilder();
           String areaList = this.GetSqlString(GetDirectRoleArea(dataBaseName), "Area_ID");

           sb.Append("SELECT YZF_CityCode From S_YZFArea Where MainArea_ID IN(");
           sb.Append("Select MainArea_ID From S_MainArea Where MainArea_Database=@dataBase) And ");
           sb.Append("Area_ID IN(" + areaList + ")");

           DataTable dt = null;
           DbCommand cmdSelect = null;
           Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(Sys.Com.Common.SystemWebFormulation.Parameter("MainDataBaseName"));
           cmdSelect = mDataBase.GetSqlStringCommand(sb.ToString());
           mDataBase.AddInParameter(cmdSelect, "@dataBase", System.Data.DbType.String, dataBaseName);
           try
           {
              dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
           }
           catch (Exception e)
           {
              throw (new Exception("查询当前地区有权限的全部综合营帐地区出错，" + e.Message));
           }

           return this.GetSqlString(dt, "YZF_CityCode");
        }

        public String GetYZFAreaList(String dataBaseName, String areaId)
        {
           StringBuilder sb = new StringBuilder();
           String areaList = this.GetSqlString(GetAllChildArea(dataBaseName, areaId), "Area_ID");

           sb.Append("SELECT YZF_CityCode From S_YZFArea Where MainArea_ID IN(");
           sb.Append("Select MainArea_ID From S_MainArea Where MainArea_Database=@dataBase) And ");
           sb.Append("Area_ID IN(" + areaList + ")");

           DataTable dt = null;
           DbCommand cmdSelect = null;
           Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(Sys.Com.Common.SystemWebFormulation.Parameter("MainDataBaseName"));
           cmdSelect = mDataBase.GetSqlStringCommand(sb.ToString());
           mDataBase.AddInParameter(cmdSelect, "@dataBase", System.Data.DbType.String, dataBaseName);
           try
           {
              dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
           }
           catch (Exception e)
           {
              throw (new Exception("查询当前地区有权限的全部综合营帐地区出错，" + e.Message));
           }

           return this.GetSqlString(dt, "YZF_CityCode");
        }

        private String GetSqlString(DataTable dt, String filedName)
        {
           StringBuilder sb = new StringBuilder();
           foreach (DataRow dr in dt.Rows)
           {
              sb.Append("'" + dr[filedName].ToString().Trim() + "',");
           }

           sb = sb.Remove(sb.Length - 1, 1);
           return sb.ToString();
        }

        /// <summary>
        /// 超级用户返回数据库中的全部地区列表，由Hashtable实现
        /// </summary>
        /// <returns>返回DataTable，有三个字段，分别为Area_Id,FatherArea_ID,Area_Name，地区ID，上级地区ID，地区名称</returns>
        public DataTable GetDirectRoleArea(String dataBaseInstance)
        {
           // TODO:  添加 SuperAreaRange.GetDirectRoleArea 实现
           DataTable dt = null;
           DataTable dd = null;

           DbCommand cmdSelect = null;
           Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(Sys.Com.Common.SystemWebFormulation.Parameter("MainDataBaseName"));
           cmdSelect = mDataBase.GetSqlStringCommand("select count(*) from master.dbo.sysdatabases where name=@name");
           mDataBase.AddInParameter(cmdSelect, "@name", System.Data.DbType.String, dataBaseInstance);

           if (Int32.Parse(mDataBase.ExecuteDataSet(cmdSelect).Tables[0].Rows[0][0].ToString()) > 0)
           {
              mDataBase = DatabaseFactory.CreateDatabase(dataBaseInstance);
              String sqlstr = "SELECT AREA_ID,FatherArea_ID,AREA_NAME FROM B_C_Area ORDER BY AREA_ID";

              cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
              try
              {
                 dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
              }
              catch (Exception e)
              {
                 throw (new Exception("查询当前人员可管理的全部地区出错，" + e.Message));
              }

              dd = dt.Clone();
              foreach (DataRow dr in dt.Rows)
              {
                 dd.Rows.Add(dr.ItemArray);
              }
           }

           return dd;
        }

        public DataTable GetAllRoleArea(String dataBaseInstance)
        {
           // TODO:  添加 SuperAreaRange.GetAllRoleArea 实现
           return GetDirectRoleArea(dataBaseInstance);
        }

        public DataTable GetAllChildArea(String dataBaseInstance, String areaId)
        {
           DataTable resultTable = null;
           DataTable htAll = this.GetDirectRoleArea(dataBaseInstance);
           resultTable = htAll.Clone();

           DataRow[] drs = htAll.Select("Area_ID=" + areaId);
           if (drs.Length == 0)
           {
              throw (new Exception("系统中不还没有此地区ID！\n输入的地区ID为：" + areaId));
           }
           foreach (DataRow dr in drs)
           {
              resultTable.Rows.Add(dr.ItemArray);
           }

           foreach (DataRow dr in drs)
           {

              //调用递归计算出它以下的全部子节点，并添加进resultTable中
              GetKeyTree(dr["AREA_ID"].ToString().Trim(), ref resultTable, htAll);
           }

           return resultTable;
        }

        private void GetKeyTree(String key, ref DataTable htResult, DataTable htAll)
        {
           DataRow[] drs = htAll.Select("FatherArea_ID = " + key.Trim());
           foreach (DataRow dr in drs)
           {
              DataRow[] drss = htResult.Select("AREA_ID = " + dr["AREA_ID"].ToString().Trim());
              if (drss.Length > 0)
              {
                 foreach (DataRow ddr in drss)
                 {
                    ddr["FatherArea_ID"] = key;
                 }
              }
              else
              {
                 htResult.Rows.Add(dr.ItemArray);
              }

              GetKeyTree(dr["AREA_ID"].ToString().Trim(), ref htResult, htAll);
           }
        }


        /// <summary>
        /// 得到当前人员可管理的全部地市州级地区，得到地区的名称，数据库名称等信息
        /// </summary>
        /// <returns>返回数据集，包括地区ID，地区名称，地区数据库名称，长途区号等</returns>
        public DataTable GetMainAreaInfo()
        {
           DataTable dt = null;
           DbCommand cmdSelect = null;
           Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(Sys.Com.Common.SystemWebFormulation.Parameter("MainDataBaseName"));
           String sqlstr = "SELECT MainArea_ID,MainArea_Name,MainArea_Database,MainArea_Code FROM S_MainArea";

           cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
           mDataBase.AddInParameter(cmdSelect, "@id", System.Data.DbType.String, this.operId);
           try
           {
              dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
           }
           catch (Exception e)
           {
              throw (new Exception("查询当前人员可管理的全部地市州级地区信息出错，" + e.Message));
           }

           return dt;
        }
        #endregion
    }
}
