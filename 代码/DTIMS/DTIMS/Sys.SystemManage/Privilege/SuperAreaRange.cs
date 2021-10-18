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
    /// SuperAreaRange ��ժҪ˵����
    /// </summary>
    [Serializable]
    public class SuperAreaRange : IAreaInfo
    {
        private String operId = null;
        public SuperAreaRange(String id)
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
            operId = id.Trim();
        }

        #region IAreaInfo ��Ա
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
              throw (new Exception("��ѯ��ǰ������Ȩ�޵�ȫ���ۺ�Ӫ�ʵ�������" + e.Message));
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
              throw (new Exception("��ѯ��ǰ������Ȩ�޵�ȫ���ۺ�Ӫ�ʵ�������" + e.Message));
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
        /// �����û��������ݿ��е�ȫ�������б���Hashtableʵ��
        /// </summary>
        /// <returns>����DataTable���������ֶΣ��ֱ�ΪArea_Id,FatherArea_ID,Area_Name������ID���ϼ�����ID����������</returns>
        public DataTable GetDirectRoleArea(String dataBaseInstance)
        {
           // TODO:  ��� SuperAreaRange.GetDirectRoleArea ʵ��
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
                 throw (new Exception("��ѯ��ǰ��Ա�ɹ����ȫ����������" + e.Message));
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
           // TODO:  ��� SuperAreaRange.GetAllRoleArea ʵ��
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
              throw (new Exception("ϵͳ�в���û�д˵���ID��\n����ĵ���IDΪ��" + areaId));
           }
           foreach (DataRow dr in drs)
           {
              resultTable.Rows.Add(dr.ItemArray);
           }

           foreach (DataRow dr in drs)
           {

              //���õݹ����������µ�ȫ���ӽڵ㣬����ӽ�resultTable��
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
        /// �õ���ǰ��Ա�ɹ����ȫ�������ݼ��������õ����������ƣ����ݿ����Ƶ���Ϣ
        /// </summary>
        /// <returns>�������ݼ�����������ID���������ƣ��������ݿ����ƣ���;���ŵ�</returns>
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
              throw (new Exception("��ѯ��ǰ��Ա�ɹ����ȫ�������ݼ�������Ϣ����" + e.Message));
           }

           return dt;
        }
        #endregion
    }
}
