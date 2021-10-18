using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Sys.Comm.WebTools;

namespace Sys.Project.Common
{
	/// <summary>
	/// ��ͨ�����û���Ȩ����֤��
	/// ���ݲ����û���Ȩ�޷���Ķ�Ӧ��ϵ�õ���ǰ�������Ƿ���Ȩ��
	/// </summary>
    [Serializable]
	public class NormalVerification : IRoleVerify
	{
		private String operId = null;

		/// <summary>
		/// ���캯��������ԱID�����г־û�����õ���ǰ��ԱȨ���飬
		/// ��Ȩ����õ�ǰȨ���б��ɼ�鵱ǰ�������Ƿ���Ȩ��
		/// </summary>
		/// <param name="id"></param>
		public NormalVerification(String id)
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
			this.operId = id.Trim();
		}

      #region IRoleVerify ��Ա
      /// <summary>
      /// �ж���ͨ�û��Ƿ����Ȩ��
      /// </summary>
      /// <param name="function">��Ҫ�жϵĹ�����ID</param>
      /// <returns>����TRUE��ʾ��Ȩ�ޣ�����FALSE��ʾ��Ȩ��</returns>
      public bool Verify(String function)
      {
         // TODO:  ��� NormalVerification.Verify ʵ��
         bool ret = false;
         System.Data.IDataReader idr = null;
         DbCommand cmdSelect;
         Database dataBase = DatabaseFactory.CreateDatabase(Sys.Com.Common.SystemWebFormulation.Parameter("MainDataBaseName"));

         //String sqlstr = "SELECT COUNT(*) FROM S_FunctionItem WHERE FUNN_ID=@function AND FUNN_ID IN(";
         //sqlstr += "SELECT FUNN_ID FROM S_FunctionMap WHERE ROLE_ID=(";
         //sqlstr += "SELECT ROLE_ID FROM S_Operator WHERE OPER_ID=@operId))";

         string sql = @"SELECT     Sys_User.User_ID, Sys_PrivilegeFunctionMap.Fun_ID
                        FROM         Sys_User INNER JOIN
                              Sys_UserPrivilegeMap ON Sys_User.User_ID = Sys_UserPrivilegeMap.User_ID INNER JOIN
                              Sys_PrivilegeFunctionMap ON Sys_UserPrivilegeMap.PrivGroup_ID = Sys_PrivilegeFunctionMap.PrivGroup_ID
                        WHERE     (Sys_User.User_ID = @operId) AND (Sys_PrivilegeFunctionMap.Fun_ID = @function)";
         cmdSelect = dataBase.GetSqlStringCommand(sql);
         dataBase.AddInParameter(cmdSelect, "@function", System.Data.DbType.String, function.Trim());
         dataBase.AddInParameter(cmdSelect, "@operId", System.Data.DbType.String, this.operId.Trim());

         try
         {
            idr = dataBase.ExecuteReader(cmdSelect);
            if (idr.Read())
            {
               if (Int32.Parse(idr.GetValue(0).ToString().Trim()) > 0)
               {
                  ret = true;
               }
            }
         }
         catch (Exception e)
         {
            throw (new Exception(e.Message));
         }
         finally
         {
            if (idr != null)
            {
               idr.Close();
               idr.Dispose();
            }
         }

         return ret;
      }

      /// <summary>
      /// �õ���ǰ�û���Ȩ�޵�ȫ�����ܷ���
      /// </summary>
      /// <returns></returns>
      public ArrayList GetRoleCategory()
      {
         ArrayList al = new ArrayList();
         DbCommand cmdSelect;
         Database dataBase = DatabaseFactory.CreateDatabase(Sys.Com.Common.SystemWebFormulation.Parameter("MainDataBaseName"));

         String sqlstr = "select FUNCN_ID,Funcn_Name from S_FunctionCategory where FUNCN_ID in(" +
            "select distinct FUNCN_ID from S_FunctionItem where FUNN_ID in(" +
            "select FUNN_ID from S_FunctionMap where ROLE_ID in(select ROLE_ID from S_Operator where OPER_ID=@operId)))";
         cmdSelect = dataBase.GetSqlStringCommand(sqlstr);
         dataBase.AddInParameter(cmdSelect, "@operId", System.Data.DbType.String, this.operId.Trim());

         try
         {
            DataTable dt = dataBase.ExecuteDataSet(cmdSelect).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
               al.Add(dr["FUNCN_ID"].ToString().Trim());
            }
         }
         catch (Exception e)
         {
            throw (new Exception("��ѯ��Ȩ�޵Ĺ���������ԭ��" + e.Message));
         }

         return al;
      }
      #endregion

      #region �ᶨҳ���Ƿ��й������
      /// <summary>
      /// �ᶨҳ���Ƿ��й������
      /// </summary>
      /// <param name="className">����ռ����������ɵ��ַ���</param>
      /// <returns></returns>
      public bool IsHaveFunctionItem(String className)
      {
         bool ret = false;
         System.Data.IDataReader idr = null;
         DbCommand cmdSelect;
         Database dataBase = DatabaseFactory.CreateDatabase(Sys.Com.Common.SystemWebFormulation.Parameter("MainDataBaseName"));
         String sqlstr = "select count(*) from S_FUNCTIONWEBMap where FWM_Name=@name";
         cmdSelect = dataBase.GetSqlStringCommand(sqlstr);
         dataBase.AddInParameter(cmdSelect, "@name", System.Data.DbType.String, className.Trim());

         try
         {
            idr = dataBase.ExecuteReader(cmdSelect);
            if (idr.Read())
            {
               if (Int32.Parse(idr.GetValue(0).ToString().Trim()) > 0)
               {
                  ret = true;
               }
            }
         }
         catch (Exception e)
         {
            throw (new Exception(e.Message));
         }
         finally
         {
            if (idr != null)
            {
               idr.Close();
               idr.Dispose();
            }
         }

         return ret;
      }
      #endregion

      #region �õ���ǰҳ����Ȩ�޵�ȫ���������
      /// <summary>
      /// �õ���ǰҳ����Ȩ�޵�ȫ���������
      /// </summary>
      /// <param name="className">����ռ����������ɵ��ַ���</param>
      /// <returns></returns>
      public DataTable GetRoleFunction(String className)
      {
         DataTable dt = null;
         DbCommand cmdSelect;
         Database dataBase = DatabaseFactory.CreateDatabase(Sys.Com.Common.SystemWebFormulation.Parameter("MainDataBaseName"));
         StringBuilder sb = new StringBuilder();
         sb.Append("select m.FUNN_ID,FWM_NAME,FWM_URL,FWM_Key,Funn_Description,FUNN_Type from S_FUNCTIONWEBMap m ");
         sb.Append("left join S_FunctionItem f on m.FUNN_ID=f.FUNN_ID ");
         sb.Append("where m.FWM_NAME=@name and m.FUNN_ID in(select FUNN_ID from S_FunctionMap where ROLE_ID ");
         sb.Append("in(select ROLE_ID from S_Operator where OPER_ID=@id)) order by FWM_URL desc");

         cmdSelect = dataBase.GetSqlStringCommand(sb.ToString());
         dataBase.AddInParameter(cmdSelect, "@name", System.Data.DbType.String, className.Trim());
         dataBase.AddInParameter(cmdSelect, "@id", System.Data.DbType.Int32, Int32.Parse(this.operId));

         try
         {
            dt = dataBase.ExecuteDataSet(cmdSelect).Tables[0];
         }
         catch (Exception e)
         {
            throw (new Exception("��ѯ��ǰҳ����Ȩ�޵Ĺ�������Ϣ����ԭ��" + e.Message));
         }

         return dt;
      }
      #endregion
	}
}
