using System;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Inphase.Common.DBParameter
{
   /// <summary>
   /// SystemParameter ��ժҪ˵����
   /// ȡ���ݿ��е����ò���
   /// </summary>

   public struct ParameterObject
   {
      public string dbKey;
      public DataTable table;
      public DateTime createTime;
   }

   public class SystemParameter
   {
      protected string mTableName = "SystemParameter";
      protected string mKey = "SYSP_Key";
      protected string mValue = "SYSP_Value";

      protected string mDbName;
      protected bool mIsCache;
      protected int mCacheTime = 30; //����
      protected static ArrayList mParameterList = new ArrayList();
      protected DataTable mTable;	//ȡ����ǰ���ݱ�

      public SystemParameter(string dbName, bool isCache)
      {
         this.mDbName = dbName;
         this.mIsCache = isCache;
         SetTableInfo();
         Init();
      }

      protected virtual void SetTableInfo()
      {

      }

      protected void Init()
      {
         int i = 0;
         ParameterObject po = new ParameterObject();
         for (; i < mParameterList.Count; i++)
         {
            po = (ParameterObject)mParameterList[i];
            if (po.dbKey == this.mDbName)
            {
               break;
            }
         }

         if (i == mParameterList.Count)
         {
            //��ѯ����,������������
            mTable = GetParameterTable();
         }
         else if (this.mIsCache == false)
         {
            //��ѯ����,���û���
            mTable = GetParameterTable();
         }
         else if (DateTime.Now.CompareTo(po.createTime.AddMinutes(mCacheTime)) > 0)
         {
            //��ѯ����,�������
            mTable = GetParameterTable();
         }
         else
         {
            //ʹ�û�������
            mTable = po.table;
         }

         if (this.mIsCache)
         {
            //���滺������
            if (i == mParameterList.Count)
            {
               //�������
               ParameterObject newPo = new ParameterObject();
               newPo.createTime = DateTime.Now;
               newPo.dbKey = this.mDbName;
               newPo.table = mTable;

               mParameterList.Insert(i, newPo);

            }
            else
            {
               //��������
               ParameterObject newPo = new ParameterObject();
               newPo.createTime = DateTime.Now;
               newPo.dbKey = this.mDbName;
               newPo.table = mTable;

               mParameterList[i] = newPo;

            }
         }

      }

      private DataTable GetParameterTable()
      {
         string sbSql = "SELECT * FROM " + mTableName;

         Database db = DatabaseFactory.CreateDatabase(this.mDbName);
         DbCommand cmd = db.GetSqlStringCommand(sbSql);
         cmd.CommandTimeout = 0;

         return db.ExecuteDataSet(cmd).Tables[0];
      }



      public string GetParameter(string key)
      {
         DataRow[] drs = mTable.Select(mKey + "='" + key + "'");

         if (drs.Length == 1)
         {
            return drs[0][mValue].ToString();
         }
         else if (drs.Length < 1)
         {
            throw new Exception("����������!");
         }
         else
         {
            throw new Exception("�����������ظ�,�����������!");
         }

      }
      public void SetParameter(string key, string keyValue)
      {
         string strUpdate = "";
         string strSelect = "select 1 from " + mTableName + " where " + mKey + "='" + key + "'";
         Database db = DatabaseFactory.CreateDatabase(this.mDbName);
         DbCommand cmd = db.GetSqlStringCommand(strSelect);
         cmd.CommandTimeout = 0;

         DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
         int rowCount = dt.Rows.Count;
         if (rowCount > 0)
         {
            strUpdate = "update " + mTableName + " set " + mValue + "=@keyValue where " + mKey + "=@key";
         }
         else
         {
            strUpdate = "insert into  " + mTableName + "(" + mKey + "," + mValue + ") values(@key,@keyValue)";
         }
         DbCommand updateCmd = db.GetSqlStringCommand(strUpdate);
         db.AddInParameter(updateCmd, "@key", System.Data.DbType.String, key);
         db.AddInParameter(updateCmd, "@keyValue", System.Data.DbType.String, keyValue);
         updateCmd.CommandTimeout = 0;

         db.ExecuteNonQuery(updateCmd);

         mTable = GetParameterTable();

         int i = 0;
         ParameterObject po = new ParameterObject();
         for (; i < mParameterList.Count; i++)
         {
            po = (ParameterObject)mParameterList[i];
            if (po.dbKey == this.mDbName)
            {
               break;
            }
         }

         if (this.mIsCache)
         {
            //���滺������
            if (i == mParameterList.Count)
            {
               //�������
               ParameterObject newPo = new ParameterObject();
               newPo.createTime = DateTime.Now;
               newPo.dbKey = this.mDbName;
               newPo.table = mTable;

               mParameterList.Insert(i, newPo);

            }
            else
            {
               //��������
               ParameterObject newPo = new ParameterObject();
               newPo.createTime = DateTime.Now;
               newPo.dbKey = this.mDbName;
               newPo.table = mTable;

               mParameterList[i] = newPo;

            }
         }

      }
   }
}
