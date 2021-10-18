using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using BJ.WebTools;
using BJ.SystemWebFormulation;

namespace BJ.Project.Common
{
    /// <summary>
    /// NormalAreaRange 的摘要说明。
    /// </summary>
    [Serializable]
    public class NormalAreaRange : IAreaInfo
    {
        private String operId = null;
        public NormalAreaRange(String id)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            this.operId = id.Trim();
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
           Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.SystemWebFormulation.Parameter("MainDataBaseName"));
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
           Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.SystemWebFormulation.Parameter("MainDataBaseName"));
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
        /// 得到当前人员有直接权限的地区，并由地区之间关系形成上下级关系。
        /// </summary>
        /// <returns>返回DataTable，有三个字段，分别为Area_Id,FatherArea_ID,Area_Name，地区ID，上级地区ID，地区名称</returns>
        public DataTable GetDirectRoleArea(String dataBase)
        {
           // TODO:  添加 NormalAreaRange.GetDirectRoleArea 实现
           DataTable resultTable = GetAllAreaAsDataTable(dataBase).Clone();

           System.Collections.Hashtable htAll = this.GetAllArea(dataBase);
           System.Collections.Hashtable htDir = this.GetDirectArea(this.operId, dataBase);
           foreach (DictionaryEntry de in htDir)
           {
              String[] tempArray = (String[])de.Value;
              String parentId = GetParentKey(tempArray[0].Trim(), htDir, htAll);
              String key = (String)de.Key;

              DataRow dr = resultTable.NewRow();
              dr[0] = key;
              if (parentId == null)
              {
                 dr[1] = System.DBNull.Value;
              }
              else
              {
                 dr[1] = parentId;
              }
              dr[2] = tempArray[1].Trim();
			
              resultTable.Rows.Add(dr);
           }

           return resultTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataBaseDestance">数据库实例名称</param>
        /// <returns></returns>
        public DataTable GetAllRoleArea(String dataBaseInstance)
        {
           // TODO:  添加 NormalAreaRange.GetAllRoleArea 实现
           DataTable resultTable = null;
           DataTable htAll = this.GetAllAreaAsDataTable(dataBaseInstance);
           resultTable = htAll.Clone();

           DataTable tempResult = this.GetHaveRoleArea(dataBaseInstance);
           DataRow[] drs = tempResult.Select("FatherArea_ID IS NULL");
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

        public DataTable GetAllChildArea(String dataBaseInstance, String areaId)
        {
           DataTable resultTable = null;
           DataTable htAll = this.GetAllAreaAsDataTable(dataBaseInstance);
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

        #region 为节点排序提供的工具函数
        /// <summary>
        /// 得到输入的当前KEY值的上级节点的KEY值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ht"></param>
        /// <returns></returns>
        private String GetParentKey(String key, System.Collections.Hashtable htDir, System.Collections.Hashtable htAll)
        {
           if (key == null || key == "")
           {
              return null;
           }
           if (htDir.Contains(key))
           {
              return key;
           }

           String[] tempArray = (String[])htAll[key];
           return GetParentKey(tempArray[0], htDir, htAll);
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
        #endregion

        #region 得到当前系统中的全部地区
        /// <summary>
        /// 得到当前系统中的全部地区
        /// </summary>
        /// <returns>列表中没有包含虚拟的根节点</returns>
        private System.Collections.Hashtable GetAllArea(String dataBaseName)
        {
           System.Collections.Hashtable ht = new System.Collections.Hashtable();

           DbCommand cmdSelect = null;
           Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase =
              DatabaseFactory.CreateDatabase(dataBaseName);
           System.Data.IDataReader iReader = null;
           String sqlstr = "SELECT AREA_ID,FatherArea_ID,AREA_NAME FROM B_C_Area ORDER BY AREA_ID";

           cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
           try
           {
              iReader = mDataBase.ExecuteReader(cmdSelect);
              while (iReader.Read())
              {
                 String[] strArray = { iReader["FatherArea_ID"].ToString().Trim(), iReader["AREA_NAME"].ToString().Trim() };
                 ht.Add(iReader["AREA_ID"].ToString().Trim(), strArray);
              }
           }
           catch (Exception e)
           {
              throw (new Exception("查询系统中全部地区出错，" + e.Message));
           }
           finally
           {
              if (iReader != null)
              {
                 iReader.Close();
                 iReader.Dispose();
              }
           }
           return ht;
        }

        /// <summary>
        /// 得到当前系统中的全部地区
        /// </summary>
        /// <returns>列表中没有包含虚拟的根节点与前不同这里的VALUE表示根节点，VALUE表示当前节点，和名称</returns>
        private DataTable GetAllAreaAsDataTable(String dataBaseName)
        {
           DataTable dt = null;
           DbCommand cmdSelect = null;
           Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(dataBaseName);
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

           return dt;
        }
        #endregion

        #region 得到当前系统中的直接可管理地区
        private System.Collections.Hashtable GetDirectArea(String id, String dataBaseName)
        {
           System.Collections.Hashtable ht = new System.Collections.Hashtable();

           DbCommand cmdSelect = null;
           Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(dataBaseName);
           System.Data.IDataReader iReader = null;
           String sqlstr = "SELECT AREA_ID,FatherArea_ID,AREA_NAME FROM B_C_Area  ORDER BY AREA_ID";

           cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
           mDataBase.AddInParameter(cmdSelect, "@id", System.Data.DbType.String, id.Trim());
           try
           {
              iReader = mDataBase.ExecuteReader(cmdSelect);
              while (iReader.Read())
              {
                 String[] strArray = { iReader["FatherArea_ID"].ToString().Trim(), iReader["AREA_NAME"].ToString().Trim() };
                 ht.Add(iReader["AREA_ID"].ToString().Trim(), strArray);
              }
           }
           catch (Exception e)
           {
              throw (new Exception("查询当前人员可管理的全部地区出错，" + e.Message));
           }
           finally
           {
              if (iReader != null)
              {
                 iReader.Close();
                 iReader.Dispose();
              }
           }
           return ht;
        }
        #endregion

        #region 得到当前人员可管理的地市州级地区
        /// <summary>
        /// 得到当前人员可管理的全部地市州级地区，得到地区的名称，数据库名称等信息
        /// </summary>
        /// <returns>返回数据集，包括地区ID，地区名称，地区数据库名称，长途区号等</returns>
        public DataTable GetMainAreaInfo()
        {
           DataTable dt = null;
           DbCommand cmdSelect = null;
           Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.SystemWebFormulation.Parameter("MainDataBaseName"));
           String sqlstr = "SELECT MainArea_ID,MainArea_Name,MainArea_Database,MainArea_Code FROM S_MainArea " +
                          "WHERE MAINAREA_ID IN(SELECT MAINAREA_ID FROM S_OperatorMainAreaMap WHERE OPER_ID=@id)";

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

        /// <summary>
        /// 查询当前人员在指定地区下有直接管理权限的地区
        /// 为添加每个地市州地区的主地区即只要人员在地市州地区的任一地区权限，
        /// 那么都会显示出地市州地区的节点出来哈。
        /// </summary>
        /// <param name="dataBase">地区名称</param>
        /// <returns>查询当前人员在指定地区下有直接管理权限的地区列表</returns>
        private DataTable GetHaveRoleArea(String dataBase)
        {
           DataTable dt = null;
           DbCommand cmdSelect = null;
           Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(dataBase);
           String sqlstr = "SELECT AREA_ID,NULL AS FATHERAREA_ID,AREA_NAME FROM B_C_Area WHERE AREA_ID IN ("
                         + "SELECT AREA_ID FROM B_C_OperAreaMap WHERE OPER_ID=@id)";

           cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
           mDataBase.AddInParameter(cmdSelect, "@id", System.Data.DbType.String, this.operId);
           try
           {
              dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
           }
           catch (Exception e)
           {
              throw (new Exception("查询当前人员可管理的全部有直接权限的地区出错，" + e.Message));
           }

           return dt;
        }
        #endregion

        /// <summary>
        /// 功能描述:获取当前人员可管理的所有地区信息
        /// 修改说明:2009-3-20 新增
        /// <author>MXJ</author>
        /// </summary>
        /// <returns></returns>
        public DataTable GetOperatorAreaInfo()
        {
            DataTable dt = null;
            DbCommand cmdSelect = null;
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase();
            String sqlstr = "SELECT MainArea_ID,MainArea_Name FROM S_MainArea where FatherMainArea_ID<>'0'";

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

        /// <summary>
        /// 根据地区ID获取所有的子地区
        /// </summary>
        /// <param name="areaID"></param>
        /// <returns></returns>
        public DataTable GetAllAreaByParent(string areaID)
        {
            DataTable dt = null;
            DbCommand cmdSelect = null;
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase();
            String sqlstr = "SELECT MainArea_ID,FatherMainArea_ID,MainArea_Name from  S_MainArea where FatherMainArea_ID=@id ORDER BY MainArea_ID";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@id", System.Data.DbType.String, areaID);
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
