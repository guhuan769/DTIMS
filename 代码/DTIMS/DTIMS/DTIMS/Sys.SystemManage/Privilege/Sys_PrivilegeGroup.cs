using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace BJ.Project.Common
{
    /// <summary>
    /// 功能描述：用户权限组
    /// 修改说明：2010-4-7　新增
    /// <author>MXJ</author>
    /// </summary>
    public class Sys_PrivilegeGroup
    {
        private string mPrivGroup_ID = null;
        private string mPrivGroup_Name = null;
        private string mPrivGroup_Desc = null;
        private bool mPrivGroup_IsPrivate = false;
        private Hashtable mHashtableFunc_ID = null;

        #region 公用属性
        /// <summary>
        /// PrivGroup_ID
        /// </summary>
        public string PrivGroup_ID
        {
            get
            {
                return this.mPrivGroup_ID;
            }

            set
            {
                this.mPrivGroup_ID = value;
            }
        }

        /// <summary>
        /// 权限组名
        /// </summary>
        public string PrivGroup_Name
        {
            get
            {
                return this.mPrivGroup_Name;
            }

            set
            {
                this.mPrivGroup_Name = value;
            }
        }

        /// <summary>
        /// 权限说明
        /// </summary>
        public string PrivGroup_Desc
        {
            get
            {
                return this.mPrivGroup_Desc;
            }

            set
            {
                this.mPrivGroup_Desc = value;
            }
        }

        /// <summary>
        /// 标识是否是用户私有组
        /// </summary>
        public bool PrivGroup_IsPrivate
        {
            get
            {
                return this.mPrivGroup_IsPrivate;
            }

            set
            {
                this.mPrivGroup_IsPrivate = value;
            }
        }

        /// <summary>
        /// 功能项ID集合
        /// </summary>
        public Hashtable HashtableFunc_ID
        {
            get
            {
                return mHashtableFunc_ID;
            }
        }
        
        #endregion

        #region 构造函数
        public Sys_PrivilegeGroup()
        {
        }

        public Sys_PrivilegeGroup(string privGroup_ID)
        {
            DbCommand cmdSelect = null;
            Database mDataBase = DatabaseFactory.CreateDatabase();
            IDataReader iReader = null;

            //if (string.IsNullOrEmpty(privGroup_ID))
            //{
            //    privGroup_ID = "0";
            //}

            try
            {
                //读取权限组信息
                String sql = @"SELECT Sys_PrivilegeGroup.PrivGroup_ID, Sys_PrivilegeGroup.PrivGroup_Name, Sys_PrivilegeGroup.PrivGroup_Desc, 
	                                Sys_PrivilegeGroup.PrivGroup_IsPrivate
                                FROM
	                                Sys_PrivilegeGroup
                                WHERE     
	                                (Sys_PrivilegeGroup.PrivGroup_ID = @PrivGroup_ID)";
                cmdSelect = mDataBase.GetSqlStringCommand(sql);
                mDataBase.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, privGroup_ID);

                iReader = mDataBase.ExecuteReader(cmdSelect);
                if (iReader.Read())
                {
                        this.mPrivGroup_ID = iReader["PrivGroup_ID"].ToString().Trim();
                        this.mPrivGroup_Name = iReader["PrivGroup_Name"].ToString().Trim();
                        this.mPrivGroup_Desc = iReader["PrivGroup_Desc"].ToString().Trim();
                        if (iReader["PrivGroup_IsPrivate"].ToString().Trim() == "0")
                        {
                            this.mPrivGroup_IsPrivate = true;
                        }
                        else
                        {
                            this.mPrivGroup_IsPrivate = false;
                        }
                }
                else
                {
                    throw (new Exception("未找到权限组信息！"));
                }

                //读取对应的权限ID信息
                sql = "SELECT Fun_ID, PrivGroup_ID FROM Sys_PrivilegeFunctionMap WHERE (PrivGroup_ID = @PrivGroup_ID)";
                cmdSelect = mDataBase.GetSqlStringCommand(sql);
                mDataBase.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, privGroup_ID);
                DataTable dtFunc = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];

                //设置当前权限组的信息
                this.mHashtableFunc_ID = new Hashtable();
                foreach (DataRow dr in dtFunc.Rows)
                {
                    if (!this.mHashtableFunc_ID.ContainsKey(dr["Fun_ID"].ToString().Trim()))
                    {
                        mHashtableFunc_ID.Add(dr["Fun_ID"].ToString().Trim(), dr["Fun_ID"].ToString().Trim());
                    }
                }
            }
            catch (Exception e)
            {
                throw (new Exception("查询权限组信息出错,原因:" + e.Message));
            }
            finally
            {
                if (iReader != null)
                {
                    iReader.Close();
                    iReader.Dispose();
                }
            }
        }
        #endregion 构造函数

        #region 修改

        private void IsExistGroupName(Operator oper)
        {
            string sql = @"SELECT     
	                            COUNT(1)
                            FROM         
	                            S_MainArea 
                            INNER JOIN
	                            Sys_User 
                            ON 
	                            S_MainArea.MainArea_ID = Sys_User.MainArea_ID 
                            INNER JOIN
	                            Sys_PrivilegeGroup 
                            ON 
	                            Sys_User.User_ID = Sys_PrivilegeGroup.User_ID
                            WHERE     
                                (Sys_User.User_ID = @User_ID) 
                            AND
                                Sys_PrivilegeGroup.PrivGroup_Name=@PrivGroup_Name";

            if (this.mPrivGroup_ID != null && this.mPrivGroup_ID.Length > 0)//修改时不需要与自己比较
            {
                sql += " AND Sys_PrivilegeGroup.PrivGroup_ID<>" + this.mPrivGroup_ID;
            }

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cmdSelect;
            cmdSelect = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmdSelect, "@User_ID", DbType.String, oper.OperatorId);
            db.AddInParameter(cmdSelect, "@PrivGroup_Name", DbType.String, this.mPrivGroup_Name);
            object ret = db.ExecuteScalar(cmdSelect);
            if (ret != null && Convert.ToInt16(ret) > 0)
            {
                throw new Exception("权限组【"+this.mPrivGroup_Name+"】已经存在,请重新输入。");
            }
        }

        /// <summary>
        /// 新增或者修改权限组
        /// </summary>
        /// <param name="htFunc">功能项ID集合</param>
        /// <param name="oper">登录用户对象</param>
        public void Update(Hashtable htFunc,Operator oper)
        {
            try
            {
                //权限组名称判断是否相关
                this.IsExistGroupName(oper);

                string sql = "";
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand cmdSelect;
                using (DbConnection conn = db.CreateConnection())
                {
                    //打开连接
                    conn.Open();
                    //创建事务
                    DbTransaction trans = conn.BeginTransaction();
                    try
                    {
                        //新增修改修权限分组
                        if (this.mPrivGroup_ID == null)
                        {
                            sql = @"INSERT INTO Sys_PrivilegeGroup
                                        (User_ID, PrivGroup_Name, PrivGroup_Desc, PrivGroup_IsPrivate)
                                    VALUES     
                                        (@User_ID,@PrivGroup_Name,@PrivGroup_Desc,1);SELECT @@identity AS identityID
                                    ";
                        }
                        else
                        {
                            sql = @"UPDATE    
                                        Sys_PrivilegeGroup
                                    SET              
                                        PrivGroup_Name =@PrivGroup_Name, 
                                        PrivGroup_Desc =@PrivGroup_Desc
                                    WHERE     
                                        (PrivGroup_ID  = @PrivGroup_ID )
                                    ";
                        }

                        //执行新增或者修改
                        cmdSelect = db.GetSqlStringCommand(sql);
                        db.AddInParameter(cmdSelect, "@PrivGroup_Name", DbType.String, this.mPrivGroup_Name);
                        db.AddInParameter(cmdSelect, "@PrivGroup_Desc", DbType.String, this.mPrivGroup_Desc);

                        string groupID = "";
                        if (this.mPrivGroup_ID == null)
                        {
                            db.AddInParameter(cmdSelect, "@User_ID", DbType.String, oper.OperatorId);
                            groupID = db.ExecuteScalar(cmdSelect,trans).ToString().Trim();//执行新增并获取新增的自增长ID
                        }
                        else
                        {
                            db.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, this.mPrivGroup_ID);
                            db.ExecuteNonQuery(cmdSelect, trans);
                            groupID = this.mPrivGroup_ID;
                        }

                        if (groupID == null || groupID == "")
                        {
                            throw new Exception("新增权限时出错.");
                        }

                        //删除权限项
                        if (this.mPrivGroup_ID != null)
                        {
                            sql = "DELETE FROM Sys_PrivilegeFunctionMap WHERE (PrivGroup_ID = @PrivGroup_ID)";
                            cmdSelect = db.GetSqlStringCommand(sql);
                            db.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, this.mPrivGroup_ID);
                            db.ExecuteNonQuery(cmdSelect, trans);
                        }

                        //新增权限项
                        sql = @"INSERT INTO 
                                    Sys_PrivilegeFunctionMap
                                        (Fun_ID, PrivGroup_ID)
                                    VALUES     
                                        (@Fun_ID,@PrivGroup_ID)
                                    ";
                        IDictionaryEnumerator ie = htFunc.GetEnumerator();
                        while (ie.MoveNext())
                        {
                            cmdSelect = db.GetSqlStringCommand(sql);
                            db.AddInParameter(cmdSelect, "@Fun_ID", DbType.String, ie.Key.ToString().Trim());
                            db.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, groupID);
                            db.ExecuteNonQuery(cmdSelect,trans);
                        }

                        //删除当前组下面对应的用户建立的权限组权限
                        if (this.mPrivGroup_ID != null)
                        {
                            DeleteFunc(this.mPrivGroup_ID, trans, db);
                        }

                        //都执行成功则提交事务
                        trans.Commit();
                    }
                    catch (Exception ee)
                    {
                        trans.Rollback();//发生异常，事务回滚
                        throw (new Exception(ee.Message));
                    }
                    finally
                    {
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();//关闭连接
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }

        private void DeleteFunc(string groupID, DbTransaction trans,Database db)
        {
            /*1:将当前权限组的功能权限与当前权限组下的用户功能项进行比较，将差异的取出来
             * 2:将比较的结果与当前组下的用户来自于其他权限组的功能比较
             * 3:删除多余的功能权限
             */
            DbCommand cmdSelect;
            string sql = @"
                            /*指定权限组下的用户建立的权限组的权限*/
                            SELECT DISTINCT Sys_PrivilegeFunctionMap.Fun_ID, 
                            Sys_PrivilegeFunctionMap.PrivGroup_ID
                            FROM         Sys_PrivilegeGroup INNER JOIN
                                                  Sys_PrivilegeFunctionMap ON Sys_PrivilegeGroup.PrivGroup_ID = Sys_PrivilegeFunctionMap.PrivGroup_ID
                            AND Sys_PrivilegeGroup.User_ID IN(
                            /*当前组下的用户*/
                            SELECT     Sys_UserPrivilegeMap.User_ID
                            FROM         Sys_PrivilegeGroup INNER JOIN
                                                  Sys_UserPrivilegeMap ON Sys_PrivilegeGroup.PrivGroup_ID = Sys_UserPrivilegeMap.PrivGroup_ID
                            WHERE     (Sys_PrivilegeGroup.PrivGroup_ID = @PrivGroup_ID))
                            AND Sys_PrivilegeFunctionMap.Fun_ID NOT IN( /*将当前权限组下的用户建立的权限与当前权限组的权限进行比较*/
                            /*指定权限组的最新功能权限*/
                            SELECT     Fun_ID
                            FROM         Sys_PrivilegeFunctionMap
                            WHERE     (PrivGroup_ID = @PrivGroup_ID))
                            AND

                            /*判断当前权限组下比较后多余的功能项是否来自于其他组*/
                            Sys_PrivilegeFunctionMap.Fun_ID NOT IN
                            (
	                            /*判断比较多出的权限在其他组中是否存在*/
	                            SELECT DISTINCT Sys_PrivilegeFunctionMap.Fun_ID
	                            FROM         Sys_UserPrivilegeMap INNER JOIN
		                            Sys_PrivilegeGroup ON Sys_UserPrivilegeMap.PrivGroup_ID = Sys_PrivilegeGroup.PrivGroup_ID INNER JOIN
		                            Sys_PrivilegeFunctionMap ON Sys_PrivilegeGroup.PrivGroup_ID = Sys_PrivilegeFunctionMap.PrivGroup_ID
	                            AND Sys_UserPrivilegeMap.User_ID IN(
	                            /*当前组下的用户*/
	                            SELECT     Sys_UserPrivilegeMap.User_ID
	                            FROM         Sys_PrivilegeGroup INNER JOIN
						                              Sys_UserPrivilegeMap ON Sys_PrivilegeGroup.PrivGroup_ID = Sys_UserPrivilegeMap.PrivGroup_ID
	                            WHERE     (Sys_PrivilegeGroup.PrivGroup_ID = @PrivGroup_ID))
	                            AND Sys_PrivilegeGroup.PrivGroup_ID <> @PrivGroup_ID
                            )";
            cmdSelect = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, groupID);
            DataTable dtFunc = db.ExecuteDataSet(cmdSelect, trans).Tables[0];

            //删除多余的功能项
            foreach (DataRow dr in dtFunc.Rows)
            {
                sql = "DELETE FROM Sys_PrivilegeFunctionMap WHERE (PrivGroup_ID = {0}) AND (Fun_ID = '{1}')";
                cmdSelect = db.GetSqlStringCommand(string.Format(sql, dr["PrivGroup_ID"].ToString().Trim(), dr["Fun_ID"].ToString().Trim()));
                db.ExecuteNonQuery(cmdSelect, trans);
            }
        }
        #endregion 修改

        #region 删除
        /// <summary>
        /// 功能描述：判断指定权限组下是否还有用户
        /// </summary>
        /// <param name="privGroupID"></param>
        /// <returns></returns>
        public static bool IsHaveUser(string privGroupID)
        {
            bool ret = true;

            DbCommand cmdSelect;

            Database db = DatabaseFactory.CreateDatabase();
            String sqlstr = "SELECT COUNT(*) FROM Sys_UserPrivilegeMap WHERE PrivGroup_ID=@PrivGroup_ID";

            cmdSelect = db.GetSqlStringCommand(sqlstr);
            db.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, privGroupID);

            try
            {
                if (Int32.Parse(db.ExecuteDataSet(cmdSelect).Tables[0].Rows[0][0].ToString()) == 0)
                {
                    ret = false;
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
            return ret;
        }

        /// <summary>
        /// 删除权限组
        /// </summary>
        public void Delete()
        {
            //判断是否有用户属于该组（如果有则返回错误）
            if (Sys_PrivilegeGroup.IsHaveUser(this.PrivGroup_ID))
            {
                throw (new Exception("还有人员属于此分组，您不能删除！"));
            }

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cmdSelect;
            using (DbConnection conn = db.CreateConnection())
            {
                //打开连接
                conn.Open();
                //创建事务
                DbTransaction trans = conn.BeginTransaction();

                try
                {
                    //删除权限组与功能项对应关系
                    string sql = "DELETE FROM Sys_PrivilegeFunctionMap WHERE (PrivGroup_ID = @PrivGroup_ID)";
                    cmdSelect = db.GetSqlStringCommand(sql);
                    db.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, this.mPrivGroup_ID);
                    db.ExecuteNonQuery(cmdSelect, trans);

                    //删除用户权限组
                    sql = "DELETE FROM Sys_PrivilegeGroup WHERE (PrivGroup_ID = @PrivGroup_ID)";
                    cmdSelect = db.GetSqlStringCommand(sql);
                    db.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, this.mPrivGroup_ID);
                    db.ExecuteNonQuery(cmdSelect, trans);

                    //提交事务
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("删除权限组时出错,详细:" + ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }
        #endregion

        #region 获取

        /// <summary>
        /// 获取所有非私有的权限组
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllPrivileges()
        {
            DataTable dt = null;
            DbCommand cmdSelect = null;
            Database mDataBase = DatabaseFactory.CreateDatabase();
            String sql = @"SELECT  S_MainArea.MainArea_ID, Sys_PrivilegeGroup.PrivGroup_Name, Sys_PrivilegeGroup.PrivGroup_ID,
	                            Sys_PrivilegeGroup.PrivGroup_IsPrivate
                            FROM	
                                S_MainArea 
                            INNER JOIN
	                            Sys_User 
                            ON 
	                            S_MainArea.MainArea_ID = Sys_User.MainArea_ID 
                            INNER JOIN
	                            Sys_PrivilegeGroup 
                            ON 
	                            Sys_User.User_ID = Sys_PrivilegeGroup.User_ID
                            WHERE     
	                            (Sys_PrivilegeGroup.PrivGroup_IsPrivate = 1)";
            cmdSelect = mDataBase.GetSqlStringCommand(sql);
            try
            {
                dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception ex)
            {
                throw (new Exception("查询权限组信息出错,原因:" + ex.Message));
            }
            return dt;
        }
        #endregion

        #region 获取指定用户的权限组
        public static DataTable GetUserGroup(string userID)
        {
            DataTable dt = null;
            DbCommand cmdSelect = null;
            Database mDataBase = DatabaseFactory.CreateDatabase();
            String sql = @"SELECT  Sys_UserPrivilegeMap.User_ID ,
                                    Sys_PrivilegeGroup.PrivGroup_ID ,
                                    Sys_PrivilegeGroup.PrivGroup_Name ,
                                    Sys_PrivilegeGroup.PrivGroup_IsPrivate
                            FROM    Sys_UserPrivilegeMap
                                    INNER JOIN Sys_PrivilegeGroup ON Sys_UserPrivilegeMap.PrivGroup_ID = Sys_PrivilegeGroup.PrivGroup_ID
                            WHERE   ( Sys_UserPrivilegeMap.User_ID = @User_ID )";
            cmdSelect = mDataBase.GetSqlStringCommand(sql);
            mDataBase.AddInParameter(cmdSelect, "@User_ID", DbType.String, userID);
            try
            {
                dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception ex)
            {
                throw (new Exception("获取指定用户的权限组,原因:" + ex.Message));
            }
            return dt;
        }
        #endregion

    }//end  public class Sys_PrivilegeGroup
}//end namespace Inphase.Project.Common
