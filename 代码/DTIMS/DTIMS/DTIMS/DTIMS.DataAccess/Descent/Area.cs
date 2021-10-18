using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections;
using BJ.Secuirty;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using BJ.WebTools;

namespace BJ.Area.Descent
{
    /// <summary>
    /// 地区类，提供地区相关功能
    /// </summary>
    public class Area
    {
        private Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = null;
        private String mAreaId = null;			//地区ID		
        private String mFatherMainArea_ID = null;	//父地区ID	
        private String mMainArea_Name = null;		//地区中文名
        private string opertorid;                    //操作人员ID

        #region 公有属性
        public String MainArea_Name
        {
            get
            {
                return this.mMainArea_Name;
            }
            set
            {
                if (value == null)
                {
                    throw (new Exception("地区节点中文名不能为空！"));
                }
                if (value.Trim() == "")
                {
                    throw (new Exception("地区节点中文名不能为空！"));
                }
                if (BJ.Secuirty.CheckString.HaveSpecialChar(value))
                {
                    throw (new Exception("地区节点中文名不能包含特殊字符！"));
                }
                this.mMainArea_Name = value.Trim();
            }
        }
        public String FatherMainArea_ID
        {
            get
            {
                return this.mFatherMainArea_ID;
            }
            set
            {
                if (value == null)
                {
                    throw (new Exception("地区根节点不能为空！"));
                }
                if (value.Trim() == "")
                {
                    throw (new Exception("地区根节点不能为空！"));
                }
                this.mFatherMainArea_ID = value.Trim();
            }
        }
        public String AreaId
        {
            get
            {
                return this.mAreaId;
            }
        }

        public string OpertorID
        {
            set
            {
                opertorid = value;
            }
        }
        #endregion

        #region 构造函数
        public Area()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 实例化地区
        /// </summary>
        /// <param name="areaId">地区ID</param>
        public Area(string areaId)
        {
            DbCommand cmdSelect;
            System.Data.IDataReader reader = null;
            mDataBase = DatabaseFactory.CreateDatabase();
            String sqlstr = "SELECT MainArea_ID,FatherMainArea_ID,MainArea_Name FROM S_MainArea WHERE MainArea_ID=@id";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, areaId);
            try
            {
                reader = mDataBase.ExecuteReader(cmdSelect);
                if (reader.Read())
                {
                    this.mAreaId = reader["MainArea_ID"].ToString().Trim();
                    this.mFatherMainArea_ID = reader["FatherMainArea_ID"].ToString().Trim();
                    this.mMainArea_Name = reader["MainArea_Name"].ToString().Trim();
                }
                else
                {
                    throw (new Exception("系统中不存在此地区！"));
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
                }
            }
        }
        #endregion

        #region 添加/修改
        /// <summary>
        /// 修改或添加地区信息
        /// </summary>
        public void Update()
        {
            IsAreaNameDuplic(this.MainArea_Name, this.AreaId);

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

                    //修改或添加地区
                    int areaID = 1;
                    if (this.AreaId == null)//新增
                    {
                        sb.Append("INSERT INTO S_MainArea (MainArea_ID,FatherMainArea_ID,MainArea_Name) VALUES(@MainArea_ID,@father,@name)");

                        //查询新添加地区的ID号
                        cmdSelect = mDataBase.GetSqlStringCommand("SELECT TOP 1 MainArea_ID FROM S_MainArea ORDER BY MainArea_ID DESC");
                        DataTable dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            areaID = Convert.ToInt16(dt.Rows[0]["MainArea_ID"]) + 1;//最大ID号+1
                        }
                    }
                    else//修改
                    {
                        sb.Append("UPDATE S_MainArea SET FatherMainArea_ID=@father,MainArea_Name=@name WHERE MainArea_ID=@id");
                    }
                    cmdSelect = mDataBase.GetSqlStringCommand(sb.ToString());

                    if (this.FatherMainArea_ID == "" || this.FatherMainArea_ID == null)
                    {
                        mDataBase.AddInParameter(cmdSelect, "@father", System.Data.DbType.Int64, System.DBNull.Value);
                    }
                    else
                    {
                        mDataBase.AddInParameter(cmdSelect, "@father", System.Data.DbType.String, this.FatherMainArea_ID);
                    }
                    mDataBase.AddInParameter(cmdSelect, "@name", System.Data.DbType.String, this.MainArea_Name);

                    if (this.AreaId != null)//修改
                    {
                        mDataBase.AddInParameter(cmdSelect, "@id", System.Data.DbType.String, this.AreaId);
                    }
                    else
                    {
                        mDataBase.AddInParameter(cmdSelect, "@MainArea_ID", System.Data.DbType.Int16, areaID);
                    }

                    mDataBase.ExecuteNonQuery(cmdSelect, trans);

                    //如果是新增,将新地区设置有当前人员的管理权限
                    if (this.AreaId == null)
                    {
                        cmdSelect = mDataBase.GetSqlStringCommand("INSERT INTO S_OperatorMainAreaMap (OPER_ID, MainArea_ID) VALUES (@OPER_ID,@MainArea_ID)");
                        mDataBase.AddInParameter(cmdSelect, "@OPER_ID", System.Data.DbType.Int64, opertorid);
                        mDataBase.AddInParameter(cmdSelect, "@MainArea_ID", System.Data.DbType.Int16, areaID);
                        mDataBase.ExecuteNonQuery(cmdSelect, trans);
                    }

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
                    connection.Close();
                }
            }
        }
        #endregion

        #region 删除地区
        public void Delete()
        {
            IsAreaChild(this.AreaId);

            DbCommand cmdSelect;
            DbTransaction trans = null;
            mDataBase = DatabaseFactory.CreateDatabase();
            DbConnection connection = mDataBase.CreateConnection();
            try
            {
                connection.Open();
                trans = connection.BeginTransaction();

                //删除地区表与人员表的关联关系
                string sql = "DELETE S_OperatorMainAreaMap WHERE MainArea_ID=@id";
                cmdSelect = mDataBase.GetSqlStringCommand(sql);
                mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, this.AreaId);
                mDataBase.ExecuteNonQuery(cmdSelect, trans);

                //删除地区表中的数据
                sql = "DELETE S_MainArea WHERE MainArea_ID=@id";
                cmdSelect = mDataBase.GetSqlStringCommand(sql);
                mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, this.AreaId);
                mDataBase.ExecuteNonQuery(cmdSelect, trans);
                trans.Commit();
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
        #endregion

        #region 列出全部地区
        /// <summary>
        /// 表出全部地区列表
        /// </summary>
        /// <returns>返回DataTable</returns>
        public DataTable GetAreaTable()
        {
            DataTable dt = null;

            DbCommand cmdSelect;
            mDataBase = DatabaseFactory.CreateDatabase(BJ.SystemWebFormulation.SystemWebFormulation.Parameter("MainDataBaseName"));
            String sqlstr = "SELECT MainArea_ID,FatherMainArea_ID,MainArea_Name FROM S_MainArea ORDER BY MainArea_ID";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            try
            {
                dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception e)
            {
                throw (new Exception("查询全部地区出错，" + e.Message));
            }

            return dt;
        }

        public DataTable GetAreaTable(string dbName)
        {
            DataTable dt = null;

            DbCommand cmdSelect;
            mDataBase = DatabaseFactory.CreateDatabase(dbName);
            String sqlstr = "SELECT MainArea_ID,FatherMainArea_ID,MainArea_Name FROM S_MainArea ORDER BY MainArea_ID";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            try
            {
                dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception e)
            {
                throw (new Exception("查询全部地区出错，" + e.Message));
            }

            return dt;
        }
        #endregion

        #region 判断地区名称是否重复
        /// <summary>
        /// 判断当前分组名称是否有重复
        /// </summary>
        /// <param name="group">分组名称</param>
        /// <param name="groupId">当前分组的ID，如果是新添加分组，当然分组ID为NULL</param>
        private void IsAreaNameDuplic(string name, string areaId)
        {
            DbCommand cmdSelect;
            mDataBase = DatabaseFactory.CreateDatabase();
            String sqlstr = "SELECT COUNT(*) FROM S_MainArea WHERE MainArea_Name=@name AND MainArea_ID<>@id";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            if (areaId != null)
            {
                mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, areaId);
            }
            else
            {
                mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, "");
            }
            mDataBase.AddInParameter(cmdSelect, "@name", DbType.String, name);

            try
            {
                if (Int32.Parse(mDataBase.ExecuteDataSet(cmdSelect).Tables[0].Rows[0][0].ToString()) > 0)
                {
                    throw (new Exception("地区名称不能重复！"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
        }
        #endregion

        #region 判断当前地区是否有子节点
        private void IsAreaChild(string areaId)
        {
            DbCommand cmdSelect;
            mDataBase = DatabaseFactory.CreateDatabase();
            String sqlstr = "SELECT COUNT(*) FROM S_MainArea WHERE FatherMainArea_ID=@id";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, areaId.Trim());

            try
            {
                if (Int32.Parse(mDataBase.ExecuteDataSet(cmdSelect).Tables[0].Rows[0][0].ToString()) > 0)
                {
                    throw (new Exception("该地区还有子节点不能删除！"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
        }
        #endregion

        #region  得到有当前地区设备管理权限的全部人员列表
        /// <summary>
        /// 得到有当前地区设备管理权限的全部人员列表
        /// </summary>
        /// <param name="areaId">当前地区ID</param>
        /// <returns></returns>
        public static ArrayList GetOperatorList(string areaId)
        {
            ArrayList al = new ArrayList();
            DbCommand cmdSelect;
            System.Data.IDataReader idr = null;
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase();
            String sqlstr = "SELECT OPER_ID FROM S_OperatorMainAreaMap WHERE MainArea_ID=@id";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, areaId.Trim());

            try
            {
                idr = mDataBase.ExecuteReader(cmdSelect);
                while (idr.Read())
                {
                    al.Add(idr["OPER_ID"].ToString().Trim());
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

            return al;
        }
        #endregion

        #region  获取指定地区中除自己外拥有权限的全部人员列表
        /// <summary>
        /// 功能描述:获取指定地区中除自己外拥有权限的全部人员列表
        /// 修改说明:2009年3月23日 新增
        /// <author>MXJ</author>
        /// </summary>
        /// <param name="areaId">当前地区ID</param>
        /// <param name="operid">人员ID</param>
        /// <returns></returns>
        public static ArrayList GetOperatorList(string areaId,string operid)
        {
            ArrayList al = new ArrayList();
            DbCommand cmdSelect;
            System.Data.IDataReader idr = null;
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase();
            String sqlstr = "SELECT OPER_ID FROM S_OperatorMainAreaMap WHERE MainArea_ID=@id AND OPER_ID <> @OPER_ID";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, areaId.Trim());
            mDataBase.AddInParameter(cmdSelect, "@OPER_ID", DbType.String, operid);

            try
            {
                idr = mDataBase.ExecuteReader(cmdSelect);
                while (idr.Read())
                {
                    al.Add(idr["OPER_ID"].ToString().Trim());
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

            return al;
        }
        #endregion

        #region  得到当前地区下的全部设备集合
        /// <summary>
        /// 得到当前地区下的全部设备集合
        /// </summary>
        /// <param name="areaId">当前地区ID</param>
        /// <returns></returns>
        public static ArrayList GetDeviceList(string areaId, string areaDatabase)
        {
            ArrayList al = new ArrayList();
            DbCommand cmdSelect;
            System.Data.IDataReader idr = null;
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase(areaDatabase);
            String sqlstr = "SELECT D_ID FROM B_R_Device WHERE MainArea_ID=@id";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, areaId.Trim());

            try
            {
                idr = mDataBase.ExecuteReader(cmdSelect);
                while (idr.Read())
                {
                    al.Add(idr["D_ID"].ToString().Trim());
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

            return al;
        }
        #endregion

        #region  得到当前地区下的工单数量
        /// <summary>
        /// 得到当前地区下的工单数量
        /// </summary>
        /// <param name="areaId">当前地区ID</param>
        /// <returns></returns>
        public static int GetBillListCount(string areaId, string areaDatabase)
        {
            int billCount = 0;
            DbCommand cmdSelect;
            Database mDataBase = DatabaseFactory.CreateDatabase(areaDatabase);
            String sqlstr = "SELECT COUNT(*) FROM B_B_BillAnalysis WHERE MainArea_ID=@id";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, areaId.Trim());

            try
            {
                billCount = Convert.ToInt32(mDataBase.ExecuteScalar(cmdSelect));
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }

            return billCount;
        }
        #endregion

        #region  得到当前地区下的状态控制方式数量
        /// <summary>
        /// 得到当前地区下的状态控制方式数量
        /// </summary>
        /// <param name="areaId">当前地区ID</param>
        /// <returns></returns>
        public static int GetStatusControlModeCount(string areaId, string areaDatabase)
        {
            int billCount = 0;
            DbCommand cmdSelect;
            Database mDataBase = DatabaseFactory.CreateDatabase(areaDatabase);
            String sqlstr = "SELECT COUNT(*) FROM B_B_BillAreaStatusControlMode WHERE MainArea_ID=@id";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, areaId.Trim());

            try
            {
                billCount = Convert.ToInt32(mDataBase.ExecuteScalar(cmdSelect));
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }

            return billCount;
        }
        #endregion

        #region 判断指定地区下是否有MDN号段信息表
        /// <summary>
        /// 功能描述:判断指定地区下是否有MDN号段信息表
        /// 修改说明:2009-3-23 新增
        /// <author>MXJ</author>
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public static ArrayList GetAreaMDN(string areaId)
        {
            ArrayList al = new ArrayList();
            DbCommand cmdSelect;
            System.Data.IDataReader idr = null;
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase();
            String sqlstr = "SELECT TNAS_ID FROM TelNumAreaSegment WHERE MainArea_ID=@id";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, areaId.Trim());

            try
            {
                idr = mDataBase.ExecuteReader(cmdSelect);
                while (idr.Read())
                {
                    al.Add(idr["TNAS_ID"].ToString().Trim());
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
            return al;
        }
        #endregion

        #region 判断指定地区下是否有IMSI号段信息表
        /// <summary>
        /// 功能描述:判断指定地区下是否有IMSI号段信息表
        /// 修改说明:2009-3-23 新增
        /// <author>MXJ</author>
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        public static ArrayList GetAreaIMSI(string areaId)
        {
            ArrayList al = new ArrayList();
            DbCommand cmdSelect;
            System.Data.IDataReader idr = null;
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase();
            String sqlstr = "SELECT INAS_ID FROM IMSINumAreaSegment WHERE MainArea_ID=@id";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, areaId.Trim());

            try
            {
                idr = mDataBase.ExecuteReader(cmdSelect);
                while (idr.Read())
                {
                    al.Add(idr["INAS_ID"].ToString().Trim());
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
            return al;
        }
        #endregion
    }
}
