using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace BJ.Project.Common
{
    /// <summary>
    /// 人员类。
    /// </summary>
    [Serializable]
    public class Operator
    {
     
        private String mOperatorId = null;			//人员ID
        private String mOperName = null;			//人员姓名
        private String mOperLogin = null;			//人员登陆名称
        private String mOperPwd = null;			//人员密码
        public IAreaInfo AreaRoleInfo = null;		//地区信息接口
        public IRoleVerify RoleVerify = null;		//权限验证接口
        private string mArea_ID = null;             //地区ID
        private string client_IP = null;

        #region 公有属性
        public bool IsSuper
        {
            get
            {
                if (this.RoleVerify is SuperVerification)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string Client_IP
        {
            get
            {
                return client_IP;
            }
            set
            {
                client_IP = value;
            }
        }

        /// <summary>
        /// 地区ID
        /// </summary>
        public string Area_ID
        {
            get
            {
                return this.mArea_ID;
            }
        }

        /// <summary>
        /// 人员登陆名称
        /// </summary>
        public String OperLogin
        {
            get
            {
                return this.mOperLogin;
            }
        }

        /// <summary>
        /// 人员密码
        /// </summary>
        public String OperPwd
        {
            get
            {
                return this.mOperPwd;
            }
        }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public String OperName
        {
            get
            {
                return this.mOperName;
            }
        }

        /// <summary>
        /// 人员ID
        /// </summary>
        public String OperatorId
        {
            get
            {
                return this.mOperatorId;
            }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="operId">人员ID</param>
        /// <param name="operName">人员姓名</param>
        /// <param name="operLogin">人员登陆名称</param>
        /// <param name="IAreaInfo">人员地区接口</param>
        /// <param name="IRoleVerify">人员权限验证接口</param>
        public Operator(String operId, String operName, String operLogin, IAreaInfo areaInfo, IRoleVerify verify, string areaID)
        {
            this.mOperatorId = operId.Trim();
            this.mOperName = operName.Trim();
            this.mOperLogin = operLogin.Trim();

            this.AreaRoleInfo = areaInfo;
            this.RoleVerify = verify;
            this.mArea_ID = areaID;

            GetPwd();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="operId">人员ID</param>
        /// <param name="operName">人员姓名</param>
        /// <param name="operLogin">人员登陆名称</param>
        /// <param name="IAreaInfo">人员地区接口</param>
        /// <param name="IRoleVerify">人员权限验证接口</param>
        public Operator(String operId, String operName, String operLogin, String pwd,
              IAreaInfo areaInfo, IRoleVerify verify,string areaID)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //

            this.mOperatorId = operId.Trim();
            this.mOperName = operName.Trim();
            this.mOperLogin = operLogin.Trim();
            this.mOperPwd = pwd.Trim();

            this.AreaRoleInfo = areaInfo;
            this.RoleVerify = verify;
            this.mArea_ID = areaID;
        }

        /// <summary>
        /// 得到当前人员的登陆密码
        /// </summary>
        /// <returns></returns>
        public void GetPwd()
        {
            DbCommand cmdSelect = null;
            //String pwd = null;

            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase();
            System.Data.IDataReader iReader = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT User_Password FROM Sys_User WHERE User_ID=@id");

            cmdSelect = mDataBase.GetSqlStringCommand(sb.ToString());
            mDataBase.AddInParameter(cmdSelect, "@id", System.Data.DbType.String, this.OperatorId.Trim());

            try
            {
                iReader = mDataBase.ExecuteReader(cmdSelect);
                if (iReader.Read())
                {
                    this.mOperPwd = iReader["User_Password"].ToString().Trim();
                }
                else
                {
                    throw (new Exception("系统中无此人员信息！"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception("查询人员相关信息出错,原因:" + e.Message));
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

        /// <summary>
        /// 验证当前人员是否有权限
        /// </summary>
        /// <param name="function">功能项ID</param>
        /// <returns>如果有权限则返回TRUE，如果没有权限则返回FALSE</returns>
        public bool Verify(String function)
        {
            return this.RoleVerify.Verify(function);
        }

        /// <summary>
        /// 返回有直接权限的地区列表
        /// </summary>
        /// <returns>ArrayList,由地区ID组成的集合</returns>
        public DataTable GetDirectRoleArea(String dataBaseInstance)
        {
            return this.AreaRoleInfo.GetDirectRoleArea(dataBaseInstance);
        }

        /// <summary>
        /// 返回人员有权限的全部地区列表，主要是用作地区管理和人员管理
        /// </summary>
        /// <returns>ArrayList,由地区ID组成的集合</returns>
        public DataTable GetAllRoleArea(String dataBaseInstance)
        {
            return this.AreaRoleInfo.GetAllRoleArea(dataBaseInstance);
        }

        /// <summary>
        /// 返回指定地区下包括输入地区和它以下的全部子地区的集合
        /// </summary>
        /// <param name="dataBaseInstance">数据库实例名称</param>
        /// <param name="areaId">地区ID</param>
        /// <returns></returns>
        public DataTable GetAllChildArea(String dataBaseInstance, String areaId)
        {
            return this.AreaRoleInfo.GetAllChildArea(dataBaseInstance, areaId);
        }

        /// <summary>
        /// 返回当前人员可以权限的全部地市州级地区
        /// </summary>
        /// <returns>DataTable.包含地区ID，地区名称，地区数据库名称</returns>
        public DataTable GetMainArea()
        {
            return this.AreaRoleInfo.GetMainAreaInfo();
        }

        /// <summary>
        /// 根据传入的数据库实例名称和地区ID,得到它和它以下全部地区对应的yzf地区集合,
        /// 和ID之间用","号分隔
        /// </summary>
        /// <param name="dataBaseName">数据库实例名称</param>
        /// <param name="areaId">地区ID</param>
        /// <returns></returns>
        public String GetYZFAreaList(String dataBaseName, String areaId)
        {
            return this.AreaRoleInfo.GetYZFAreaList(dataBaseName, areaId);
        }

        /// <summary>
        /// 根据传入的数据库实例名称和地区ID,得到它和它以下全部地区对应的yzf地区集合,
        /// 和ID之间用","号分隔
        /// </summary>
        /// <param name="dataBaseName">数据库实例名称</param>
        /// <returns></returns>
        public String GetYZFAreaList(String dataBaseName)
        {
            return this.AreaRoleInfo.GetYZFAreaList(dataBaseName);
        }
        /// <summary>
        /// 获取所有权限组
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllRoleGroup()
        {
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase =
            DatabaseFactory.CreateDatabase(SystemWebFormulation.SystemWebFormulation.Parameter("MainDataBaseName"));
            DataTable dt = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ROLE_ID,ROLE_NAME FROM S_Role Order by ROLE_NAME ASC");
            DbCommand cmdSelect = mDataBase.GetSqlStringCommand(sb.ToString());
            try
            {
                dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
                return dt;
            }
            catch (Exception e)
            {
                throw (new Exception("获取权限组失败:" + e.Message));
            }
        }

        /// <summary>
        /// 获取所有权限组和用户
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllUsersAndPrivileges()
        {
            DataTable dt = null;
            DbCommand cmd = null;
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder sb = new StringBuilder();
            sb.Append("select Sys_UserPrivilegeMap.User_ID,User_Name,Sys_PrivilegeGroup.PrivGroup_ID,PrivGroup_Name,MainArea_ID ");//需要的字段
            sb.Append("from Sys_UserPrivilegeMap left join Sys_User on Sys_UserPrivilegeMap.User_ID=Sys_User.User_ID left join Sys_PrivilegeGroup on Sys_UserPrivilegeMap.PrivGroup_ID=Sys_PrivilegeGroup.PrivGroup_ID ");//需要的表
            sb.Append("where PrivGroup_IsPrivate=1");//需要的条件
            cmd = db.GetSqlStringCommand(sb.ToString());
            try
            {
                dt = db.ExecuteDataSet(cmd).Tables[0];
            }
            catch (Exception ex)
            {
                throw (new Exception("查询用户与权限信息出错,原因:" + ex.Message));
            }
            return dt;
        }
    }
}
