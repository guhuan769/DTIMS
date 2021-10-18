using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Sys.Comm.WebTools;
using Sys.Com.Common;

namespace DTIMS.OperatorPersistent.Descent
{
    /// <summary>
    /// 人员类的持久化操作
    /// 人员类与页面上相关的提供的方法,
    /// 人员类的增删改等操作
    /// </summary>
    public class OperatorPersistent
    {
        private Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = null;
        private String mOPER_ID = null;				//人员ID
        private String mROLE_ID = null;				//权限组ID
        private String mOPER_Name = null;			//人员名称
        private String mOPER_Login = null;			//人员登陆名
        private String mOPER_Password = null;		//人员登陆密码
        private String mOPER_CreateDate = null;	//人员建立日期，时间
        private String mOPER_Attribute = null;		//人员属性
        private String mOPER_Status = null;		   //人员属性
        private String mOPER_Remark = null;			//人员备注
        public ArrayList AreaList = null;			//地区集合
        public ArrayList RoleList = null;			//权限组集合
        private string areaid = null; //地区

        #region 公有属性
        public String OPER_ID
        {
            get
            {
                return this.mOPER_ID;
            }
            set { this.mOPER_ID = value; }
        }
        public String ROLE_ID
        {
            get
            {
                return this.mROLE_ID;
            }
            set
            {
                if (value != null)
                {
                    if (value.Trim() == "")
                    {
                        throw (new Exception("人员权限组不能为空！"));
                    }
                    else
                    {
                        this.mROLE_ID = value.Trim();
                    }
                }
                else
                {
                    throw (new Exception("人员权限组不能为空！"));
                }
            }
        }
        public String OPER_Login
        {
            get
            {
                return this.mOPER_Login;
            }
            set
            {
                if (value != null)
                {
                    if (value.Trim() == "")
                    {
                        throw (new Exception("人员登陆名称不能为空！"));
                    }
                    else
                    {
                        if (ObjectMath.IsOverLong(value, 20))
                        {
                            throw (new Exception("人员登陆名称最长不能超过20字符，或10个中文字符！"));
                        }
                        else
                        {
                            this.mOPER_Login = value.Trim();
                        }
                    }
                }
                else
                {
                    throw (new Exception("人员登陆名称不能为空！"));
                }
            }
        }
        public String OPER_Name
        {
            get
            {
                return this.mOPER_Name;
            }
            set
            {
                if (value != null)
                {
                    if (value.Trim() == "")
                    {
                        throw (new Exception("人员名称不能为空！"));
                    }
                    else
                    {
                        if (ObjectMath.IsOverLong(value, 20))
                        {
                            throw (new Exception("人员名称最长不能超过20字符，或10个中文字符！"));
                        }
                        else
                        {
                            this.mOPER_Name = value.Trim();
                        }
                    }
                }
                else
                {
                    throw (new Exception("人员名称不能为空！"));
                }
            }
        }
        public String OPER_Password
        {
            get
            {
                return this.mOPER_Password;
            }
            set
            {
                if (value != null)
                {
                    if (value.Trim() == "")
                    {
                        throw (new Exception("登陆密码不能为空！"));
                    }
                    else
                    {
                        //if (ObjectMath.IsOverLong(value, 20))
                        //{
                        //    throw (new Exception("登陆密码最长不能超过20字符，或10个中文字符！"));
                        //}
                        //else
                        //{
                        //    this.mOPER_Password = value.Trim();
                        //}
                        this.mOPER_Password = value.Trim();
                    }
                }
                else
                {
                    throw (new Exception("登陆密码不能为空！"));
                }
            }
        }

        public string MainArea_ID
        {
            get { return areaid; }
            set { this.areaid = value; }
        }

        public String OPER_CreateDate
        {
            get
            {
                return this.mOPER_CreateDate;
            }
        }
        public String OPER_Status
        {
            get
            {
                return this.mOPER_Status;
            }
            set
            {
                if (value != null)
                {
                    if (value.Trim() == "")
                    {
                        this.mOPER_Status = null;
                    }
                    else
                    {
                        if (ObjectMath.IsOverLong(value, 50))
                        {
                            throw (new Exception("人员状态最长不能超过4字符，或2个中文字符！"));
                        }
                        else
                        {
                            this.mOPER_Status = value.Trim();
                        }
                    }
                }
                else
                {
                    this.mOPER_Status = value;
                }
            }
        }
        public String OPER_Remark
        {
            get
            {
                return this.mOPER_Remark;
            }
            set
            {
                if (value != null)
                {
                    if (value.Trim() == "")
                    {
                        this.mOPER_Remark = null;
                    }
                    else
                    {
                        if (ObjectMath.IsOverLong(value, 50))
                        {
                            throw (new Exception("人员备注最长不能超过50字符，或25个中文字符！"));
                        }
                        else
                        {
                            this.mOPER_Remark = value.Trim();
                        }
                    }
                }
                else
                {
                    this.mOPER_Remark = value;
                }
            }
        }
        #endregion

        #region 构造函数
        public OperatorPersistent()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            AreaList = new ArrayList();
        }

        public OperatorPersistent(string id)
        {
            this.mOPER_ID = id.Trim();
            AreaList = new ArrayList();

            //获取当前用户的信息
            DbCommand cmdSelect;
            System.Data.IDataReader reader = null;
            mDataBase = DatabaseFactory.CreateDatabase();
            String sqlstr = @"SELECT *
                              FROM         
                                Sys_User
                              WHERE    
                                 (User_ID = @User_ID)";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@User_ID", DbType.String, id.Trim());
            try
            {
                reader = mDataBase.ExecuteReader(cmdSelect);
                if (reader.Read())
                {
                    this.mOPER_ID = reader["User_ID"].ToString().Trim();
                    this.mROLE_ID = reader["UserRole_ID"].ToString().Trim();
                    this.mOPER_Name = reader["User_Name"].ToString().Trim();
                    this.mOPER_Login = reader["User_Login"].ToString().Trim();
                    this.mOPER_Password = reader["User_Password"].ToString().Trim();
                    this.mOPER_CreateDate = reader["User_CreateDate"].ToString().Trim();
                    this.mOPER_Status = reader["User_Status"].ToString().Trim();
                    this.mOPER_Remark = reader["User_Remark"].ToString().Trim();
                    this.areaid = reader["MainArea_ID"].ToString().Trim();

                    DTIMS.Role.Descent.RoleGroup role = new DTIMS.Role.Descent.RoleGroup();
                    DataTable table = role.GetRoleByUser(this.mOPER_ID);

                    RoleList = new ArrayList();
                    DataRow row = null;
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        row = table.Rows[i];
                        RoleList.Add(row["PrivGroup_ID"].ToString() + "|" + row["PrivGroup_IsPrivate"].ToString());
                    }
                }
                else
                {
                    throw (new Exception("无此人员信息！"));
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
        #endregion

        #region 获取指定人员管理的人员列表
        public static ArrayList GetManageAreaList(String operId)
        {
            ArrayList al = new ArrayList();

            DbCommand cmdSelect = null;
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase();
            System.Data.IDataReader iReader = null;
            string sqlstr = "SELECT MainArea_ID FROM S_OperatorMainAreaMap WHERE OPER_ID=@operId";
            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@operId", System.Data.DbType.String, operId.Trim());
            try
            {
                iReader = mDataBase.ExecuteReader(cmdSelect);
                while (iReader.Read())
                {
                    al.Add(iReader["MainArea_ID"].ToString().Trim());
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
            return al;
        }
        #endregion

        #region 人员的添加与修改
        /// <summary>
        /// 人员的添加或修改
        /// </summary>
        public void Update(ArrayList groupList, ArrayList funList, string privateGroupID, bool isme)
        {
            //判断名称是否重复
            IsLoginNameDuplic(this.OPER_Login, this.OPER_ID);

            DbCommand cmdSelect;
            DbConnection connection;
            DbTransaction trans;
            mDataBase = DatabaseFactory.CreateDatabase();
            connection = mDataBase.CreateConnection();

            try
            {
                connection.Open();
                trans = connection.BeginTransaction();
                try
                {
                    StringBuilder sb = new StringBuilder();

                    #region 新增或者修改用户信息
                    if (this.OPER_ID == null)//新增用户信息
                    {
                        sb.Append("INSERT INTO Sys_User(UserRole_ID,MainArea_ID,User_Name,User_Login,User_Password,User_CreateDate,User_Status,User_Remark)");
                        sb.Append(" VALUES(@UserRole_ID,@MainArea_ID,");
                        sb.Append("@User_Name,@User_Login,@User_Password,getdate(),0,@User_Remark);select @@identity");
                    }
                    else//修改用户信息
                    {
                        OperatorPersistent oper = new OperatorPersistent(this.OPER_ID);
                        sb.Append("UPDATE Sys_User SET UserRole_ID=@UserRole_ID,MainArea_ID=@MainArea_ID,User_Name=@User_Name,");
                        sb.Append("User_Login=@User_Login,User_Password=@User_Password,User_Status=@status,User_Remark=@User_Remark WHERE User_ID=@User_ID");
                    }
                    cmdSelect = mDataBase.GetSqlStringCommand(sb.ToString());
                    mDataBase.AddInParameter(cmdSelect, "@UserRole_ID", System.Data.DbType.String, this.ROLE_ID);

                    mDataBase.AddInParameter(cmdSelect, "@MainArea_ID", System.Data.DbType.String, this.areaid);
                    //如果是修改，则设置状态
                    if (this.OPER_ID != null)
                    {
                        mDataBase.AddInParameter(cmdSelect, "@status", System.Data.DbType.String, this.OPER_Status);
                    }
                    mDataBase.AddInParameter(cmdSelect, "@User_Name", System.Data.DbType.String, this.OPER_Name);
                    mDataBase.AddInParameter(cmdSelect, "@User_Login", System.Data.DbType.String, this.OPER_Login);
                    mDataBase.AddInParameter(cmdSelect, "@User_Password", System.Data.DbType.String, this.OPER_Password);
                    if (this.OPER_Remark != null)
                    {
                        mDataBase.AddInParameter(cmdSelect, "@User_Remark", System.Data.DbType.String, this.OPER_Remark);
                    }
                    else
                    {
                        mDataBase.AddInParameter(cmdSelect, "@User_Remark", System.Data.DbType.String, "");
                    }
                    if (this.OPER_ID != null)
                    {
                        mDataBase.AddInParameter(cmdSelect, "@User_ID", System.Data.DbType.String, this.OPER_ID);
                    }
                    string tempOper_id = null;
                    if (this.OPER_ID == null)
                    {
                        //如果新增,获取当前新增用户的ID,方便接下来设置用户所管理的地区
                        tempOper_id = mDataBase.ExecuteScalar(cmdSelect).ToString();
                    }
                    else
                    {
                        mDataBase.ExecuteNonQuery(cmdSelect, trans);
                        tempOper_id = this.OPER_ID;
                    }

                    #endregion 新增或者修改用户信息

                    if (!string.IsNullOrEmpty(ROLE_ID) && !ROLE_ID.Equals("1"))
                    {
                        //如果是新增，则创建用户私有组
                        string sql = null;
                        if (this.OPER_ID == null)
                        {
                            sql = @"INSERT INTO Sys_PrivilegeGroup(User_ID,PrivGroup_Name,PrivGroup_Desc,PrivGroup_IsPrivate) VALUES (@User_ID,@PrivGroup_Name,@PrivGroup_Desc,0);select @@identity";
                            cmdSelect = mDataBase.GetSqlStringCommand(sql);
                            mDataBase.AddInParameter(cmdSelect, "@User_ID", System.Data.DbType.Int32, tempOper_id);
                            mDataBase.AddInParameter(cmdSelect, "@PrivGroup_Name", System.Data.DbType.String, this.OPER_Name + "组");
                            mDataBase.AddInParameter(cmdSelect, "@PrivGroup_Desc", System.Data.DbType.String, "用户 '" + this.OPER_Name + "' 的私有组");
                            privateGroupID = mDataBase.ExecuteScalar(cmdSelect, trans).ToString();//执行更新
                        }

                        if (!isme)//如果是自己修改自己的信息，就什么都不做
                        {
                            //如果是修改先删除私有组功能权限与对应的权限组
                            string sqltemp = "";
                            if (this.OPER_ID != null)
                            {
                                sqltemp = "DELETE Sys_PrivilegeFunctionMap WHERE PrivGroup_ID={0}; DELETE Sys_UserPrivilegeMap WHERE User_ID={1}";
                                sqltemp = string.Format(sqltemp, privateGroupID.Trim(), tempOper_id.Trim());
                                cmdSelect = mDataBase.GetSqlStringCommand(sqltemp);
                                mDataBase.ExecuteNonQuery(cmdSelect, trans);
                            }
                            StringBuilder sqlList = new StringBuilder();
                            if (funList.Count > 0)
                            {
                                //添加私有组权限
                                sqltemp = "INSERT INTO Sys_PrivilegeFunctionMap(Fun_ID,PrivGroup_ID) VALUES({0},@PrivGroup_ID)";

                                foreach (string s in funList)
                                {
                                    sqlList.Append(string.Format(sqltemp, s));
                                }

                                cmdSelect = mDataBase.GetSqlStringCommand(sqlList.ToString());
                                mDataBase.AddInParameter(cmdSelect, "@PrivGroup_ID", System.Data.DbType.String, privateGroupID.Trim());
                                mDataBase.ExecuteNonQuery(cmdSelect, trans);
                            }

                            //新增人员私有组对应关系
                            sql = "INSERT INTO Sys_UserPrivilegeMap(PrivGroup_ID,User_ID) VALUES (@PrivGroup_ID,@mOPER_ID);";
                            cmdSelect = mDataBase.GetSqlStringCommand(sql);
                            mDataBase.AddInParameter(cmdSelect, "@PrivGroup_ID", System.Data.DbType.Int32, privateGroupID);
                            mDataBase.AddInParameter(cmdSelect, "@mOPER_ID", System.Data.DbType.Int32, tempOper_id);
                            mDataBase.ExecuteNonQuery(cmdSelect, trans);//执行更新

                            if (groupList.Count > 0)
                            {
                                //添加权限组
                                sqltemp = "INSERT INTO Sys_UserPrivilegeMap(PrivGroup_ID,User_ID) VALUES({0},@userid);";
                                sqlList = new StringBuilder();
                                foreach (string s in groupList)
                                {
                                    sqlList.Append(string.Format(sqltemp, s));
                                }
                                cmdSelect = mDataBase.GetSqlStringCommand(sqlList.ToString());
                                mDataBase.AddInParameter(cmdSelect, "@userid", System.Data.DbType.String, tempOper_id.Trim());
                                mDataBase.ExecuteNonQuery(cmdSelect, trans);
                            }
                        }
                    }

                    trans.Commit();

                    //删除当前用户建立的权限组的权限
                    if (this.OPER_ID != null)
                    {
                        //GetDeleteGroupId(this.OPER_ID);//暂时不删除
                    }
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
                    connection.Close();
                }
            }
        }
        #endregion

        #region 设置人员权限组
        /// <summary>
        /// 设置人员权限组
        /// </summary>
        /// <param name="operId">人员ID</param>
        /// <param name="list">权限组列表</param>
        public void SetRoleGroup(string operId, ArrayList list)
        {
            //先删除权限，再重新添加
            DelRoleGroup(operId);
            DbConnection connection;
            DbTransaction trans;
            DbCommand cmdSelect;
            Database dataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            connection = dataBase.CreateConnection();
            connection.Open();
            trans = connection.BeginTransaction();

            StringBuilder sqlList = new StringBuilder();
            string sqlstr = "INSERT INTO Sys_UserPrivilegeMap(PrivGroup_ID,User_ID) VALUES({0},@userid);";
            foreach (string s in list)
            {
                sqlList.Append(string.Format(sqlstr, s));
            }
            cmdSelect = dataBase.GetSqlStringCommand(sqlList.ToString());
            try
            {
                dataBase.AddInParameter(cmdSelect, "@userid", System.Data.DbType.String, operId.Trim());
                dataBase.ExecuteNonQuery(cmdSelect);

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw (new Exception(e.Message));
            }
        }
        #endregion

        #region 删除人员权限组
        /// <summary>
        /// 删除人员权限组
        /// </summary>
        /// <param name="operId">人员ID</param>
        public void DelRoleGroup(string operId)
        {
            DbConnection connection;
            DbTransaction trans;
            DbCommand cmdSelect;
            Database dataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            connection = dataBase.CreateConnection();
            connection.Open();
            trans = connection.BeginTransaction();
            //不删除私有组
            string sqlstr = @"delete Sys_UserPrivilegeMap where User_ID=@User_ID and PrivGroup_ID not in(
                                    select PrivGroup_ID from Sys_PrivilegeGroup where User_ID=@User_ID and PrivGroup_IsPrivate=0)";
            cmdSelect = dataBase.GetSqlStringCommand(sqlstr);
            try
            {
                dataBase.AddInParameter(cmdSelect, "@User_ID", System.Data.DbType.String, operId.Trim());
                dataBase.ExecuteNonQuery(cmdSelect);
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw (new Exception(e.Message));
            }
        }
        #endregion

        #region 逻辑删除人员
        /// <summary>
        /// 逻辑删除人员
        /// </summary>
        /// <param name="operId">人员ID</param>
        /// <param name="state">状态 1=禁用，0=正常</param>
        public static void Delete(string operId, string state)
        {
            DbConnection connection;
            DbTransaction trans;
            DbCommand cmdSelect;
            Database dataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            connection = dataBase.CreateConnection();
            connection.Open();
            trans = connection.BeginTransaction();

            string sqlstr = "update Sys_User set User_Status=@status WHERE User_ID=@id;";
            cmdSelect = dataBase.GetSqlStringCommand(sqlstr);

            try
            {
                dataBase.AddInParameter(cmdSelect, "@status", System.Data.DbType.String, state.Trim());
                dataBase.AddInParameter(cmdSelect, "@id", System.Data.DbType.String, operId.Trim());
                dataBase.ExecuteNonQuery(cmdSelect);

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw (new Exception(e.Message));
            }
        }

        #region 获取总地区
        private static DataTable GetMainAreaInfo()
        {
            DataTable dt = null;
            DbCommand cmdSelect = null;
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            String sqlstr = "SELECT MainArea_ID,MainArea_Name,MainArea_Database,MainArea_Code FROM S_MainArea";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
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

        #endregion

        #region 删除人员私有组功能项
        /// <summary>
        /// 删除人员私有组功能项
        /// </summary>
        /// <param name="operId">人员ID</param>
        public void DelRoleGroupFunction(string operId)
        {
            DbConnection connection;
            DbTransaction trans;
            DbCommand cmdSelect;
            Database dataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            connection = dataBase.CreateConnection();
            connection.Open();
            trans = connection.BeginTransaction();

            string sqlstr = @"delete Sys_PrivilegeFunctionMap where PrivGroup_ID in(
                                    select PrivGroup_ID from Sys_PrivilegeGroup where User_ID=@User_ID and PrivGroup_IsPrivate=0)";

            cmdSelect = dataBase.GetSqlStringCommand(sqlstr);
            try
            {
                dataBase.AddInParameter(cmdSelect, "@User_ID", System.Data.DbType.String, operId.Trim());
                dataBase.ExecuteNonQuery(cmdSelect);

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw (new Exception(e.Message));
            }
        }
        #endregion

        #region 添加人员私有组功能项
        /// <summary>
        /// 添加人员私有组功能项
        /// </summary>
        /// <param name="operId">人员ID</param>
        ///  <param name="privateId">私有组ID</param>
        /// <param name="funList">功能项ID集合</param>
        public void EditRoleGroupFunction(string operId, string privateId, ArrayList funList)
        {
            DelRoleGroupFunction(operId);
            DbConnection connection;
            DbTransaction trans;
            DbCommand cmdSelect;
            Database dataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            connection = dataBase.CreateConnection();
            connection.Open();
            trans = connection.BeginTransaction();

            string sqlstr = "INSERT INTO Sys_PrivilegeFunctionMap(Fun_ID,PrivGroup_ID) values({0},@PrivGroup_ID)";

            StringBuilder sqlList = new StringBuilder();
            foreach (string s in funList)
            {
                sqlList.Append(string.Format(sqlstr, s));
            }

            cmdSelect = dataBase.GetSqlStringCommand(sqlList.ToString());
            try
            {
                dataBase.AddInParameter(cmdSelect, "@PrivGroup_ID", System.Data.DbType.String, privateId.Trim());
                dataBase.ExecuteNonQuery(cmdSelect);

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw (new Exception(e.Message));
            }
        }
        #endregion

        #region 判断人员中是否还有人员属于此权限分组，作用是删除权限分组时，权限分组类得调用此方法。
        /// <summary>
        /// 判断人员中是否还有人员属于此权限分组
        /// 作用是删除权限分组时，权限分组类得调用此方法。
        /// </summary>
        /// <param name="groupId">权限分组ID</param>
        /// <returns>如果有人员属于此分组返回TRUE，反此返回FALSE</returns>
        public bool IsHaveRoleGroup(string groupId)
        {
            bool ret = true;
            DbCommand cmdSelect;
            mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            String sqlstr = "SELECT COUNT(*) FROM S_Operator WHERE ROLE_ID=@roleId";
            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@roleId", DbType.String, groupId.Trim());
            try
            {
                if (Int32.Parse(mDataBase.ExecuteDataSet(cmdSelect).Tables[0].Rows[0][0].ToString()) == 0)
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
        #endregion

        #region 判断人员登陆名称是否重复
        /// <summary>
        /// 判断当前分组名称是否有重复
        /// </summary>
        /// <param name="name">人员登陆名称</param>
        /// <param name="operId">当前人员的ID，如果是新添加人员，当然人员ID为NULL</param>
        private void IsLoginNameDuplic(string name, string operId)
        {
            DbCommand cmdSelect;
            mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            String sqlstr = "SELECT COUNT(*) FROM Sys_User WHERE User_Login=@name AND User_ID<>@id";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            if (operId != null)
            {
                mDataBase.AddInParameter(cmdSelect, "@id", DbType.Int16, operId);
            }
            else
            {
                mDataBase.AddInParameter(cmdSelect, "@id", DbType.Int16, 0);
            }
            mDataBase.AddInParameter(cmdSelect, "@name", DbType.String, name);

            try
            {
                if (Int32.Parse(mDataBase.ExecuteDataSet(cmdSelect).Tables[0].Rows[0][0].ToString()) > 0)
                {
                    throw (new Exception("此登陆名已经存在！"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
        }
        #endregion

        #region 得到当前用户有权限的全部地区下的人员信息。
        /// <summary>
        /// 得到当前用户有权限的全部地区下的人员信息。
        /// </summary>
        /// <param name="dtArea">当前有权限的地区列表</param>
        /// <returns>人员信息集合</returns>
        public DataTable ListAll(String areaDatabase)
        {
            DbCommand cmdSelect;
            mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            StringBuilder sb = new StringBuilder();
            DataTable dt = null;

            sb.Append("SELECT OPER_ID,O.ROLE_ID,ROLE_NAME,OPER_NAME,OPER_LOGIN,OPER_PASSWORD,");
            sb.Append("convert(varchar(25),OPER_CREATEDATE,120) AS OPER_CREATEDATE,OPER_Status,OPER_REMARK FROM S_Operator O LEFT JOIN S_ROLE R ON O.ROLE_ID=R.ROLE_ID ");
            sb.Append("WHERE OPER_ATTRIBUTE='G'	AND OPER_ID IN(SELECT OPER_ID FROM ");
            sb.Append("S_OperatorMainAreaMap WHERE ");
            sb.Append("MainArea_ID IN(SELECT MainArea_ID  FROM S_MainArea  WHERE MainArea_Database=@databasename))  ");

            cmdSelect = mDataBase.GetSqlStringCommand(sb.ToString());
            mDataBase.AddInParameter(cmdSelect, "@databasename", System.Data.DbType.String, areaDatabase);
            try
            {
                dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }

            return dt;
        }
        #endregion

        #region 获取指定地区下的管理员信息列表
        public DataTable GetAreaOperator(int pageSize, int page, string where, string orderby, out int data_row, out int cur_page_index)
        {
            DataTable dt = null;
            string tableName = "Sys_User as a left join Sys_UserRole as b on a.UserRole_ID=b.UserRole_ID left join S_MainArea c on a.MainArea_ID=c.MainArea_ID";
            DTIMS.DataAccess.Common com = new DTIMS.DataAccess.Common();
            dt = com.Query(pageSize, page, tableName, "User_ID", "*", where, orderby, out data_row, out cur_page_index);

            return dt;
        }

        public DataTable GetIPList(int pageSize, int page, string where, string orderby, out int data_row, out int cur_page_index)
        {
            DataTable dt = null;
            string tableName = " IPConifg ";
            DTIMS.DataAccess.Common com = new DTIMS.DataAccess.Common();
            dt = com.Query(pageSize, page, tableName, "ID", "*", where, orderby, out data_row, out cur_page_index);
            return dt;
        }
        #endregion

        #region 得到全部地区
        /// <summary>
        /// 得到当前人员下的全部地区列表
        /// </summary>
        /// <returns>ARRAYLIST集合</returns>
        public static ArrayList GetAreaList(String operId)
        {
            ArrayList al = new ArrayList();

            DbCommand cmdSelect = null;
            DbCommand cmdAreaDatabase = null;
            DbCommand cmdSelectDiqu = null;
            string areaDatabase = null;
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBaseDiqu = null;
            System.Data.IDataReader iReader = null;
            System.Data.IDataReader diquReader = null;
            string sqlstr = "SELECT MainArea_ID FROM S_OperatorMainAreaMap WHERE OPER_ID=@operId";
            string sqlAreaDatabase = "SELECT MainArea_Database FROM S_MainArea WHERE MainArea_id=@MainAreaID";
            string sqlStrDiqu = "SELECT Area_ID  from B_C_OperAreaMap where OPER_ID=@operId";
            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@operId", System.Data.DbType.String, operId.Trim());
            try
            {
                iReader = mDataBase.ExecuteReader(cmdSelect);
                while (iReader.Read())
                {
                    cmdAreaDatabase = mDataBase.GetSqlStringCommand(sqlAreaDatabase);
                    mDataBase.AddInParameter(cmdAreaDatabase, "@MainAreaID", System.Data.DbType.String, iReader["MainArea_ID"].ToString().Trim());
                    areaDatabase = mDataBase.ExecuteScalar(cmdAreaDatabase).ToString();
                    mDataBaseDiqu = DatabaseFactory.CreateDatabase(areaDatabase);
                    cmdSelectDiqu = mDataBaseDiqu.GetSqlStringCommand(sqlStrDiqu);
                    mDataBaseDiqu.AddInParameter(cmdSelectDiqu, "@operId", System.Data.DbType.String, operId.Trim());
                    diquReader = mDataBaseDiqu.ExecuteReader(cmdSelectDiqu);
                    while (diquReader.Read())
                    {
                        al.Add(areaDatabase + ":" + diquReader["AREA_ID"].ToString().Trim());
                    }

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
                    if (diquReader != null)
                    {
                        diquReader.Close();
                        diquReader.Dispose();
                    }
                }
            }

            return al;
        }
        #endregion

        #region 判断删除用户的地区权限是否大于当前登录用户
        public bool IsAreaRangeOut(string operid)
        {
            bool rt = false;
            ArrayList al = GetAreaList(operid);
            if (this.mOPER_Attribute == "G")
            {
                foreach (string areaid in al)
                {
                    if (AreaList.IndexOf(areaid) < 0)
                    {
                        return true;
                    }
                }
            }
            return rt;
        }

        #endregion

        #region 判断删除用户的地区权限是否大于当前登录用户
        public bool IsSysLog(string operid)
        {
            bool rt = false;

            DbConnection connection;
            DbTransaction trans;
            mDataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            connection = mDataBase.CreateConnection();
            connection.Open();
            trans = connection.BeginTransaction();

            string strSql = "SELECT * FROM S_LogFile WHERE  OPER_ID=@operid";
            DbCommand cmdUpdate = mDataBase.GetSqlStringCommand(strSql);
            try
            {
                mDataBase.AddInParameter(cmdUpdate, "@operid", DbType.String, operid);
                DataTable dt = mDataBase.ExecuteDataSet(cmdUpdate).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    rt = true;
                }
                else
                {
                    rt = false;
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw (new Exception("查看用户下是否存在日志信息出错，" + ex.Message));
            }

            return rt;
        }

        #endregion

        #region  密码修改
        public void ChangePassword(string dbName, string newPwd)
        {
            DbConnection connection;
            DbTransaction trans;
            mDataBase = DatabaseFactory.CreateDatabase(dbName);
            connection = mDataBase.CreateConnection();
            connection.Open();
            trans = connection.BeginTransaction();

            string strSql = "UPDATE Sys_User SET User_Password=@newPwd WHERE User_ID=@User_ID";
            DbCommand cmdUpdate = mDataBase.GetSqlStringCommand(strSql);
            try
            {
                mDataBase.AddInParameter(cmdUpdate, "@newPwd", DbType.String, newPwd);
                mDataBase.AddInParameter(cmdUpdate, "@User_ID", DbType.String, this.OPER_ID);
                mDataBase.ExecuteNonQuery(cmdUpdate);

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw (new Exception("修改密码出错，" + ex.Message));
            }
        }
        #endregion

        /// <summary>
        /// 获取要删除用户下面的功能项
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public void GetDeleteGroupId(string userid)
        {
            string sql = @"
                            SELECT DISTINCT Sys_PrivilegeFunctionMap.Fun_ID, 
                            Sys_PrivilegeFunctionMap.PrivGroup_ID
                            FROM         Sys_PrivilegeGroup INNER JOIN
                                                  Sys_PrivilegeFunctionMap ON Sys_PrivilegeGroup.PrivGroup_ID = Sys_PrivilegeFunctionMap.PrivGroup_ID
                            WHERE     (Sys_PrivilegeGroup.User_ID = {0})
                            AND
                            Sys_PrivilegeFunctionMap.Fun_ID
                            NOT IN(
                            SELECT DISTINCT Sys_PrivilegeFunctionMap.Fun_ID
                            FROM         Sys_UserPrivilegeMap INNER JOIN
                                                  Sys_PrivilegeGroup ON Sys_UserPrivilegeMap.PrivGroup_ID = Sys_PrivilegeGroup.PrivGroup_ID INNER JOIN
                                                  Sys_PrivilegeFunctionMap ON Sys_PrivilegeGroup.PrivGroup_ID = Sys_PrivilegeFunctionMap.PrivGroup_ID
                            WHERE     (Sys_UserPrivilegeMap.User_ID = {0}))";

            sql = string.Format(sql, userid);
            DbConnection connection;
            DbTransaction trans;
            DbCommand cmdSelect;
            Database dataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            connection = dataBase.CreateConnection();
            connection.Open();
            trans = connection.BeginTransaction();

            try
            {
                cmdSelect = dataBase.GetSqlStringCommand(sql);

                DataTable dt = dataBase.ExecuteDataSet(cmdSelect).Tables[0];

                #region 如果是修改，则判断用户的功能项是否减少，如果减少，则删除其下面的所有子项

                DataTable table = dt;
                StringBuilder delIdList = new StringBuilder();
                if (table != null && table.Rows.Count > 0)
                {
                    DataRow row = null;
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        row = table.Rows[i];
                        delIdList.Append(string.Format(" (Fun_ID={0} and PrivGroup_ID={1}) or", row["Fun_ID"].ToString(), row["PrivGroup_ID"].ToString()));
                    }
                }
                string delSql = delIdList.ToString();
                if (!string.IsNullOrEmpty(delSql))
                {
                    delSql = delSql.Substring(0, delSql.Length - 3);
                    //执行删除
                    delSql = "delete Sys_PrivilegeFunctionMap where " + delSql;
                    cmdSelect = mDataBase.GetSqlStringCommand(delSql);
                    mDataBase.ExecuteNonQuery(cmdSelect, trans);//执行更新
                    trans.Commit();
                }

                #endregion
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw (new Exception(e.Message));
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}
