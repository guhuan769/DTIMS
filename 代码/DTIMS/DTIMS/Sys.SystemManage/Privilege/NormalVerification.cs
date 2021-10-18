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
	/// 普通操作用户的权限验证，
	/// 根据操作用户与权限分组的对应关系得到当前功能项是否有权限
	/// </summary>
    [Serializable]
	public class NormalVerification : IRoleVerify
	{
		private String operId = null;

		/// <summary>
		/// 构造函数，由人员ID，进行持久化构造得到当前人员权限组，
		/// 由权限组得当前权限列表，可检查当前功能项是否有权限
		/// </summary>
		/// <param name="id"></param>
		public NormalVerification(String id)
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
			this.operId = id.Trim();
		}

      #region IRoleVerify 成员
      /// <summary>
      /// 判断普通用户是否具体权限
      /// </summary>
      /// <param name="function">需要判断的功能项ID</param>
      /// <returns>返回TRUE表示有权限，返回FALSE表示无权限</returns>
      public bool Verify(String function)
      {
         // TODO:  添加 NormalVerification.Verify 实现
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
      /// 得到当前用户有权限的全部功能分类
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
            throw (new Exception("查询有权限的功能类别出错，原因：" + e.Message));
         }

         return al;
      }
      #endregion

      #region 提定页面是否有功能项定义
      /// <summary>
      /// 提定页面是否有功能项定义
      /// </summary>
      /// <param name="className">命令空间包含类名组成的字符串</param>
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

      #region 得到当前页面有权限的全部功能项定义
      /// <summary>
      /// 得到当前页面有权限的全部功能项定义
      /// </summary>
      /// <param name="className">命令空间包含类名组成的字符串</param>
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
            throw (new Exception("查询当前页面有权限的功能项信息出错，原因：" + e.Message));
         }

         return dt;
      }
      #endregion
	}
}
