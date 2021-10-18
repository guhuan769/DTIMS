using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections;
using Sys.Comm.Secuirty;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Sys.Comm.WebTools;
using Sys.Com.Common;

namespace DTIMS.Role.Descent
{
    /// <summary>
    /// 权限组类，负责处理权限组相关的方法，
    /// 主要就是权限组的增，删，改，还有就是为人员类提供方法，供人员类，
    /// 得以人员对象相关的权限功能项。
    /// </summary>
    public class RoleGroup
    {
        private Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = null;
        public ArrayList FunctionItems = null;						//功能项集合
        private String mId = null;						//权限组ID，唯一标示
        private String mRoleGroupName = null;						//权限组名称
        private String mRoleGroupDes = null;						//权限组说明

        #region 公有属性
        public String RoleGroupDes
        {
            get
            {
                return this.mRoleGroupDes;
            }
            set
            {
                if (value == null)
                {
                    this.mRoleGroupDes = null;
                }
                else
                {
                    if (value.Trim() == "")
                    {
                        this.mRoleGroupDes = null;
                    }
                    else
                    {
                        if (Sys.Comm.Secuirty.CheckString.HaveSpecialChar(value))
                        {
                            throw (new Exception("分项说明不能包含特殊字符！"));
                        }
                        if (ObjectMath.IsOverLong(value, 50))
                        {
                            throw (new Exception("分项说明不能超过50个字符或25个汉字！"));
                        }
                        this.mRoleGroupDes = value.Trim();
                    }
                }
            }
        }
        public String RoleGroupName
        {
            get
            {
                return this.mRoleGroupName;
            }
            set
            {
                if (value == null)
                {
                    throw (new Exception("功能项分组名称不能为空！"));
                }
                if (value.Trim() == "")
                {
                    throw (new Exception("功能项分组名称不能为空！"));
                }
                if (Sys.Comm.Secuirty.CheckString.HaveSpecialChar(value))
                {
                    throw (new Exception("功能项分组称不能包含特殊字符！"));
                }
                if (ObjectMath.IsOverLong(value, 20))
                {
                    throw (new Exception("分组名称不能超过20个字符或10个汉字！"));
                }
                this.mRoleGroupName = value.Trim();
            }
        }
        public String Id
        {
            get
            {
                return this.mId;
            }
        }
        #endregion

        #region 构造函数
        public RoleGroup()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            FunctionItems = new ArrayList();
            this.mId = null;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">分组ID</param>
        public RoleGroup(string id)
        {
            FunctionItems = new ArrayList();
            this.mId = id;

            DbCommand cmdSelect;
            System.Data.IDataReader reader = null;

            mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            String sqlstr = "SELECT ROLE_ID,ROLE_NAME,ROLE_DESCRIPTION FROM S_ROLE WHERE ROLE_ID=@id";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, mId);
            try
            {
                reader = mDataBase.ExecuteReader(cmdSelect);
                if (reader.Read())
                {
                    this.mRoleGroupName = reader["ROLE_NAME"].ToString().Trim();
                    this.mRoleGroupDes = reader["ROLE_DESCRIPTION"].ToString().Trim();

                    //读取此权限分组对应的全部功能项
                    this.FunctionItems = GetFunctionList(this.Id);
                }
                else
                {
                    throw (new Exception("无此权限分组！"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
        }

        /// <summary>
        /// 由权限组得到对应的全部功能项集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ArrayList GetFunctionList(string id)
        {
            ArrayList al = new ArrayList();

            DbCommand cmdSelect;
            System.Data.IDataReader reader = null;
            mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            String sqlstr = "select FUNN_ID from S_FunctionMap where ROLE_ID=@id";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, mId);
            try
            {
                reader = mDataBase.ExecuteReader(cmdSelect);
                while (reader.Read())
                {
                    al.Add(reader["FUNN_ID"].ToString().Trim());
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            return al;
        }
        #endregion

        #region 添加或修改
        public void Update()
        {
            //判断名称是否重复
            IsGroupNameDuplic(this.RoleGroupName, this.Id);

            if (this.FunctionItems.Count == 0)
            {
                throw (new Exception("权限分组至少应该有一个功能项！"));
            }

            DbCommand cmdSelect;
            DbConnection connection;
            DbTransaction trans;
            mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            connection = mDataBase.CreateConnection();

            try
            {
                connection.Open();
                trans = connection.BeginTransaction();
                try
                {
                    StringBuilder sb = new StringBuilder();

                    //修改或添加权限分组
                    if (this.Id == null)
                    {
                        sb.Append("INSERT INTO S_ROLE (ROLE_NAME,ROLE_DESCRIPTION) VALUES(@name,@des)");
                    }
                    else
                    {
                        sb.Append("UPDATE S_ROLE SET ROLE_NAME=@name,ROLE_DESCRIPTION=@des WHERE ROLE_ID=@id");
                    }
                    cmdSelect = mDataBase.GetSqlStringCommand(sb.ToString());
                    mDataBase.AddInParameter(cmdSelect, "@name", System.Data.DbType.String, this.RoleGroupName);
                    if (this.RoleGroupDes != null)
                    {
                        mDataBase.AddInParameter(cmdSelect, "@des", System.Data.DbType.String, this.RoleGroupDes);
                    }
                    else
                    {
                        mDataBase.AddInParameter(cmdSelect, "@des", System.Data.DbType.String, "");
                    }
                    if (this.Id != null)
                    {
                        mDataBase.AddInParameter(cmdSelect, "@id", System.Data.DbType.String, this.Id);
                    }
                    mDataBase.ExecuteNonQuery(cmdSelect, trans);

                    if (this.Id == null)
                    {
                        cmdSelect = mDataBase.GetSqlStringCommand("SELECT ROLE_ID FROM S_ROLE WHERE ROLE_NAME=@name");
                        mDataBase.AddInParameter(cmdSelect, "@name", System.Data.DbType.String, this.RoleGroupName);
                        this.mId = mDataBase.ExecuteDataSet(cmdSelect, trans).Tables[0].Rows[0][0].ToString().Trim();
                    }

                    //修改或添加分组与功能项关系
                    sb = new StringBuilder();
                    sb.Append("DELETE FROM S_FunctionMap WHERE ROLE_ID='" + this.Id + "' \n");

                    int count = FunctionItems.Count;
                    for (int i = 0; i < count; i++)
                    {
                        sb.Append("INSERT INTO S_FunctionMap (ROLE_ID,FUNN_ID) VALUES('" + this.Id + "','" + FunctionItems[i].ToString().Trim() + "') \n ");
                    }
                    cmdSelect = mDataBase.GetSqlStringCommand(sb.ToString());
                    mDataBase.ExecuteNonQuery(cmdSelect, trans);
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw (new Exception(e.Message));
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close(); connection.Dispose();
                }
            }
        }
        #endregion

        #region 判断分组名称是否重复
        /// <summary>
        /// 判断当前分组名称是否有重复
        /// </summary>
        /// <param name="group">分组名称</param>
        /// <param name="groupId">当前分组的ID，如果是新添加分组，当然分组ID为NULL</param>
        private void IsGroupNameDuplic(string group, string groupId)
        {
            DbCommand cmdSelect;
            mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            String sqlstr = "SELECT COUNT(*) FROM S_ROLE WHERE ROLE_NAME=@name AND ROLE_ID<>@id";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            if (groupId != null)
            {
                mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, groupId);
            }
            else
            {
                mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, "0");
            }
            mDataBase.AddInParameter(cmdSelect, "@name", DbType.String, group);

            try
            {
                if (Int32.Parse(mDataBase.ExecuteDataSet(cmdSelect).Tables[0].Rows[0][0].ToString()) > 0)
                {
                    throw (new Exception("分组名称与已有权限分组重复！"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
        }
        #endregion

        #region 删除分组
        /// <summary>
        /// 删除权限分组，系统必须检查此分组是否还有人员拥有，如果有则必须提示用户
        /// </summary>
        /// <param name="groupId">分组ID</param>
        public static void Delete(string groupId)
        {
            DTIMS.OperatorPersistent.Descent.OperatorPersistent oper = new DTIMS.OperatorPersistent.Descent.OperatorPersistent();
            if (oper.IsHaveRoleGroup(groupId))
            {
                throw (new Exception("还有人员属于此分组，你不能删除！"));
            }

            DbCommand cmdSelect;
            Database dataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            String sqlstr = "DELETE S_FunctionMap WHERE ROLE_ID=@id \n";
            sqlstr += "DELETE S_ROLE WHERE ROLE_ID=@id \n ";
            cmdSelect = dataBase.GetSqlStringCommand(sqlstr);

            try
            {
                dataBase.AddInParameter(cmdSelect, "@id", System.Data.DbType.String, groupId.Trim());
                dataBase.ExecuteNonQuery(cmdSelect);
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
        }
        #endregion

        #region 查询全部权限组
        /// <summary>
        /// 查询全部权限组
        /// </summary>
        /// <param name="isPublic">是否是公有权限组</param>
        /// <param name="oper">当前用户</param>
        /// <returns></returns>
        public DataTable ListAll(bool isPublic,  Sys.Project.Common.Operator oper)
        {
           
            DataTable dt = null;
            string isPublicstr = isPublic ? "1" : "0";
            DbCommand cmdSelect;
            Database dataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));

            String sqlstr = "";
            if (oper.IsSuper)//如果是超级用户则查询四川省地区权限组
            {
                sqlstr = "SELECT * FROM Sys_PrivilegeGroup where PrivGroup_IsPrivate=" + isPublicstr + " and  User_ID in (select User_ID from Sys_User where MainArea_ID=1)";
            }
            else//否则只查询自己建立的权限组
            {
                sqlstr = "SELECT * FROM Sys_PrivilegeGroup where PrivGroup_IsPrivate="
                    + isPublicstr + " and  User_ID=" + oper.OperatorId;
            }

            cmdSelect = dataBase.GetSqlStringCommand(sqlstr);

            try
            {
                dt = dataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }

            return dt;
        }
        #endregion

        #region 查询用户所有的权限组
        /// <summary>
        /// 查询用户所有的权限组
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        public DataTable GetRoleByUser(string userid)
        {
            DataTable dt = null;
            DbCommand cmdSelect;
            Database dataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            String sqlstr = @"SELECT b.PrivGroup_ID,b.User_ID,PrivGroup_Name,PrivGroup_Desc,PrivGroup_IsPrivate FROM Sys_UserPrivilegeMap a left join 
                                    Sys_PrivilegeGroup b on a.PrivGroup_ID=b.PrivGroup_ID
                                    where a.User_ID=" + userid;
            cmdSelect = dataBase.GetSqlStringCommand(sqlstr);

            try
            {
                dt = dataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }

            return dt;
        }
        #endregion

        #region 列出系统中全部功能项集合
        public DataTable GetFunctionData()
        {
            DataTable dt = null;

            DbCommand cmdSelect;
            mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            String sqlstr = "select FUNN_ID,FUNN_FatherID,FUNN_DESCRIPTION,c.FUNCN_ID,FUNCN_Name,b.FUNBC_ID,FUNBC_Name " +
               "from S_FunctionMap f left join S_FunctionCategory c on f.FUNCN_ID=c.FUNCN_ID " +
               "left join S_FunctionBigCategory b on c.FUNBC_ID=b.FUNBC_ID " +
               "order by b.FUNBC_ID,cast(c.FUNCN_ID as int),FUNN_ID";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            try
            {
                dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception e)
            {
                throw (new Exception("查询功能项出错,原因:" + e.Message));
            }

            return dt;
        }
        #endregion

        #region 列出系统中全部BigCategory集合

        /// <summary>
        /// 列出系统中全部BigCategory集合
        /// </summary>
        /// <returns></returns>
        public DataTable GetBigCategory()
        {
            DataTable dt = null;

            DbCommand cmdSelect;
            mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            String sqlstr = "select * from S_FunctionBigCategory";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            try
            {
                dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception e)
            {
                throw (new Exception("查询功能大项出错,原因:" + e.Message));
            }

            return dt;
        }
        #endregion

        #region 列出系统中指定的FunctionCategory集合

        /// <summary>
        /// 列出系统中指定的FunctionCategory集合
        /// </summary>
        /// <returns></returns>
        public DataTable GetFunctionCategory(string id)
        {
            DataTable dt = null;

            DbCommand cmdSelect;
            mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            String sqlstr = "select * from S_FunctionCategory where FUNBC_ID=@id";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, id);
            try
            {
                dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception e)
            {
                throw (new Exception("查询FunctionCategory出错,原因:" + e.Message));
            }

            return dt;
        }
        #endregion

        #region 列出系统中指定的FunctionItem集合

        /// <summary>
        /// 列出系统中指定的FunctionItem集合
        /// </summary>
        /// <returns></returns>
        public DataTable GetFunctionItem(string id)
        {
            DataTable dt = null;

            DbCommand cmdSelect;
            mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            String sqlstr = "select * from S_FunctionItem where FUNCN_ID=@id ORDER BY FUNN_Sort";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, id);
            try
            {
                dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception e)
            {
                throw (new Exception("查询FunctionItem出错,原因:" + e.Message));
            }

            return dt;
        }
        #endregion

        #region 获取角色

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <returns></returns>
        public DataTable GetSys_UserRole()
        {
            DataTable dt = null;

            DbCommand cmdSelect;
            mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            String sqlstr = "select * from Sys_UserRole";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            try
            {
                dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception e)
            {
                throw (new Exception("查询FunctionItem出错,原因:" + e.Message));
            }

            return dt;
        }
        #endregion
    }
}
