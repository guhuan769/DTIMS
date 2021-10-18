using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Sys.Project.Common
{
    /// <summary>
    /// 功能描述：功能项表类
    /// </summary>
    public class Sys_FunctionItem
    {
        #region 获取指定人员的功能权限
        /// <summary>
        /// 功能描述：获取指定人员的功能权限
        /// </summary>
        /// <param name="oper"></param>
        /// <returns></returns>
        public static DataTable GetUserFunctionItem(Operator oper)
        {
            string sql = @"SELECT DISTINCT Sys_FunctionItem.Fun_ID, Sys_FunctionItem.FunCategory_ID, 
                                Sys_FunctionItem.Fun_ParentID, Sys_FunctionItem.Fun_Name, 
                                Sys_FunctionItem.Fun_Desc, Sys_FunctionItem.Fun_Sort, 
                                Sys_FunctionItem.Fun_Url, Sys_FunctionItem.Fun_Image,Sys_FunctionItem.Fun_CssClass
                           FROM         
                                Sys_FunctionItem";

            //如果是超级管理员不需要加条件，获取所有功能项
            if (!oper.IsSuper)
            {
                sql += @" INNER JOIN
                            Sys_PrivilegeFunctionMap 
                        ON 
                            Sys_FunctionItem.Fun_ID = Sys_PrivilegeFunctionMap.Fun_ID 
                        INNER JOIN
                            Sys_PrivilegeGroup 
                        ON 
                            Sys_PrivilegeFunctionMap.PrivGroup_ID = Sys_PrivilegeGroup.PrivGroup_ID 
                        INNER JOIN
                            Sys_UserPrivilegeMap 
                        ON 
                            Sys_PrivilegeGroup.PrivGroup_ID = Sys_UserPrivilegeMap.PrivGroup_ID
                        INNER JOIN
                            Sys_User 
                        ON 
                            Sys_User.User_ID = Sys_UserPrivilegeMap.User_ID
                        WHERE 
                            Sys_User.User_ID=@User_ID";
            }
            sql += " ORDER BY Sys_FunctionItem.Fun_Sort";

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand cmdSelect = db.GetSqlStringCommand(sql);
                if (!oper.IsSuper)
                {
                    db.AddInParameter(cmdSelect, "@User_ID", System.Data.DbType.String, oper.OperatorId);
                }
                return db.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("获取功能权限时出错:详细信息"+ ex.Message);
            }
        }

        /// <summary>
        /// 功能描述：获取指定人员的功能权限
        /// </summary>
        /// <param name="oper"></param>
        /// <returns></returns>
        public static Hashtable GetUserFunctionItem(string userID)
        {
            Hashtable ht = new Hashtable();
            string sql = @"SELECT DISTINCT Sys_FunctionItem.Fun_ID
                           FROM         
                                Sys_FunctionItem";
                sql += @" INNER JOIN
                            Sys_PrivilegeFunctionMap 
                        ON 
                            Sys_FunctionItem.Fun_ID = Sys_PrivilegeFunctionMap.Fun_ID 
                        INNER JOIN
                            Sys_PrivilegeGroup 
                        ON 
                            Sys_PrivilegeFunctionMap.PrivGroup_ID = Sys_PrivilegeGroup.PrivGroup_ID 
                        INNER JOIN
                            Sys_UserPrivilegeMap 
                        ON 
                            Sys_PrivilegeGroup.PrivGroup_ID = Sys_UserPrivilegeMap.PrivGroup_ID
                        INNER JOIN
                            Sys_User 
                        ON 
                            Sys_User.User_ID = Sys_UserPrivilegeMap.User_ID
                        WHERE 
                            Sys_User.User_ID=@User_ID";
            //sql += " ORDER BY Sys_FunctionItem.Fun_Sort";

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand cmdSelect = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmdSelect, "@User_ID", System.Data.DbType.String, userID);
                DataTable dt = db.ExecuteDataSet(cmdSelect).Tables[0];
                foreach(DataRow dr in dt.Rows)
                {
                    if (!ht.ContainsKey(dr["Fun_ID"].ToString().Trim()))
                    {
                        ht.Add(dr["Fun_ID"].ToString().Trim(), dr["Fun_ID"].ToString().Trim());
                    }
                }
                return ht;
            }
            catch (Exception ex)
            {
                throw new Exception("获取功能权限时出错:详细信息" + ex.Message);
            }
        }
        #endregion

        #region 获取指定权限组的功能权限ID集合
        public static Hashtable GetFuncCollectionsID(string privGroup_ID)
        {
            DbCommand cmdSelect = null;
            Database mDataBase = DatabaseFactory.CreateDatabase();
            IDataReader iReader = null;
            Hashtable htFuncID = new Hashtable();
            string sql = "SELECT Fun_ID, PrivGroup_ID FROM Sys_PrivilegeFunctionMap WHERE (PrivGroup_ID = @PrivGroup_ID)";

            cmdSelect = mDataBase.GetSqlStringCommand(sql);
            mDataBase.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, privGroup_ID);

            try
            {
                iReader = mDataBase.ExecuteReader(cmdSelect);
                if (iReader.Read())
                {
                    if (!htFuncID.ContainsKey(iReader["Fun_ID"].ToString().Trim()))
                    {
                        htFuncID.Add(iReader["Fun_ID"].ToString().Trim(), iReader["Fun_ID"].ToString().Trim());
                    }
                }
                else
                {
                    throw (new Exception("未找到权限信息！"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception("查询权限信息出错,原因:" + e.Message));
            }
            finally
            {
                if (iReader != null)
                {
                    iReader.Close();
                    iReader.Dispose();
                }
            }
            return htFuncID;
        }
        #endregion

        #region 获取指定权限组的功能权限组集合
        public static DataTable GetGroupFunctions(string privGroup_ID)
        {
            DbCommand cmdSelect = null;
            Database mDataBase = DatabaseFactory.CreateDatabase();
            string sql = @"SELECT  Sys_FunctionItem.Fun_ID ,
                                    Sys_FunctionItem.FunCategory_ID ,
                                    Sys_FunctionItem.Fun_ParentID ,
                                    Sys_FunctionItem.Fun_Name ,
                                    Sys_FunctionItem.Fun_Sort
                            FROM    Sys_FunctionItem
                                    INNER JOIN Sys_PrivilegeFunctionMap ON Sys_FunctionItem.Fun_ID = Sys_PrivilegeFunctionMap.Fun_ID
                            WHERE   ( Sys_PrivilegeFunctionMap.PrivGroup_ID = @PrivGroup_ID )
                            ORDER BY Sys_FunctionItem.Fun_Sort";

            cmdSelect = mDataBase.GetSqlStringCommand(sql);
            mDataBase.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, privGroup_ID);

            try
            {
                return mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception e)
            {
                throw (new Exception("查询权限信息出错,原因:" + e.Message));
            }
            
        }
        #endregion
    }//end public class Sys_FunctionItem
}//end namespace Inphase.Project.Common
