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
    /// NormalAreaRange ��ժҪ˵����
    /// </summary>
    [Serializable]
    public class NormalAreaRange : IAreaInfo
    {
        private String operId = null;
        public NormalAreaRange(String id)
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
            this.operId = id.Trim();
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
           Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.SystemWebFormulation.Parameter("MainDataBaseName"));
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
           Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.SystemWebFormulation.Parameter("MainDataBaseName"));
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
        /// �õ���ǰ��Ա��ֱ��Ȩ�޵ĵ��������ɵ���֮���ϵ�γ����¼���ϵ��
        /// </summary>
        /// <returns>����DataTable���������ֶΣ��ֱ�ΪArea_Id,FatherArea_ID,Area_Name������ID���ϼ�����ID����������</returns>
        public DataTable GetDirectRoleArea(String dataBase)
        {
           // TODO:  ��� NormalAreaRange.GetDirectRoleArea ʵ��
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
        /// <param name="dataBaseDestance">���ݿ�ʵ������</param>
        /// <returns></returns>
        public DataTable GetAllRoleArea(String dataBaseInstance)
        {
           // TODO:  ��� NormalAreaRange.GetAllRoleArea ʵ��
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

              //���õݹ����������µ�ȫ���ӽڵ㣬����ӽ�resultTable��
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

        #region Ϊ�ڵ������ṩ�Ĺ��ߺ���
        /// <summary>
        /// �õ�����ĵ�ǰKEYֵ���ϼ��ڵ��KEYֵ
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

        #region �õ���ǰϵͳ�е�ȫ������
        /// <summary>
        /// �õ���ǰϵͳ�е�ȫ������
        /// </summary>
        /// <returns>�б���û�а�������ĸ��ڵ�</returns>
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
              throw (new Exception("��ѯϵͳ��ȫ����������" + e.Message));
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
        /// �õ���ǰϵͳ�е�ȫ������
        /// </summary>
        /// <returns>�б���û�а�������ĸ��ڵ���ǰ��ͬ�����VALUE��ʾ���ڵ㣬VALUE��ʾ��ǰ�ڵ㣬������</returns>
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
              throw (new Exception("��ѯ��ǰ��Ա�ɹ����ȫ����������" + e.Message));
           }

           return dt;
        }
        #endregion

        #region �õ���ǰϵͳ�е�ֱ�ӿɹ������
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
              throw (new Exception("��ѯ��ǰ��Ա�ɹ����ȫ����������" + e.Message));
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

        #region �õ���ǰ��Ա�ɹ���ĵ����ݼ�����
        /// <summary>
        /// �õ���ǰ��Ա�ɹ����ȫ�������ݼ��������õ����������ƣ����ݿ����Ƶ���Ϣ
        /// </summary>
        /// <returns>�������ݼ�����������ID���������ƣ��������ݿ����ƣ���;���ŵ�</returns>
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
              throw (new Exception("��ѯ��ǰ��Ա�ɹ����ȫ�������ݼ�������Ϣ����" + e.Message));
           }

           return dt;
        }

        /// <summary>
        /// ��ѯ��ǰ��Ա��ָ����������ֱ�ӹ���Ȩ�޵ĵ���
        /// Ϊ���ÿ�������ݵ�������������ֻҪ��Ա�ڵ����ݵ�������һ����Ȩ�ޣ�
        /// ��ô������ʾ�������ݵ����Ľڵ��������
        /// </summary>
        /// <param name="dataBase">��������</param>
        /// <returns>��ѯ��ǰ��Ա��ָ����������ֱ�ӹ���Ȩ�޵ĵ����б�</returns>
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
              throw (new Exception("��ѯ��ǰ��Ա�ɹ����ȫ����ֱ��Ȩ�޵ĵ�������" + e.Message));
           }

           return dt;
        }
        #endregion

        /// <summary>
        /// ��������:��ȡ��ǰ��Ա�ɹ�������е�����Ϣ
        /// �޸�˵��:2009-3-20 ����
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
                throw (new Exception("��ѯ��ǰ��Ա�ɹ����ȫ�������ݼ�������Ϣ����" + e.Message));
            }

            return dt;
        }

        /// <summary>
        /// ���ݵ���ID��ȡ���е��ӵ���
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
                throw (new Exception("��ѯ��ǰ��Ա�ɹ����ȫ�������ݼ�������Ϣ����" + e.Message));
            }

            return dt;
        }

        #endregion
    }
}
