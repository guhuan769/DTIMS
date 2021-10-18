using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using BJ.WebTools;

namespace BJ.Project.Common
{
    /// <summary>
    /// 人员的实际创建类，还有功能就是验证当前登陆的人员是否合法。
    /// </summary>
    public class OperatorFactory
    {
        public static Operator OperatorCreate(String loginName, String password)
        {
            //将登录密码加密
            password = BJ.Sys.Comm.EnDeCrypt.Encrypt(password);

            DbCommand cmdSelect = null;
            String operId = null;
            String operName = null;
            String operAttr = null;
            String operPwd = null;
            String operStatus = null;
            string areaid = null;

            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase();
            System.Data.IDataReader iReader = null;
            string sql = @"SELECT User_ID, UserRole_ID, 
                            MainArea_ID, User_Name, User_Login, 
                            User_Password, User_CreateDate, User_Status
                        FROM         
                            Sys_User
                        WHERE
                            User_Login=@User_Login
                        AND
                            User_Password=@User_Password";

            cmdSelect = mDataBase.GetSqlStringCommand(sql);
            mDataBase.AddInParameter(cmdSelect, "@User_Login", System.Data.DbType.AnsiString, loginName.Trim());
            mDataBase.AddInParameter(cmdSelect, "@User_Password", System.Data.DbType.AnsiString, password.Trim());

            try
            {
                iReader = mDataBase.ExecuteReader(cmdSelect);
                if (iReader.Read())
                {
                    operId = iReader["User_ID"].ToString().Trim();
                    operName = iReader["User_Name"].ToString().Trim();
                    operAttr = iReader["UserRole_ID"].ToString().Trim();
                    operPwd = iReader["User_Password"].ToString().Trim();
                    operStatus = iReader["User_Status"].ToString().Trim();
                    areaid = iReader["MainArea_ID"].ToString().Trim();
                }
                else
                {
                    throw (new Exception("登陆名或密码错误！"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
            finally
            {
                if (iReader != null)
                {
                    iReader.Close();
                    iReader.Dispose();
                }
            }

            Operator oper = null;

            if (operStatus == "0")
            {
                if (operAttr == "1")
                {
                    oper = new Operator(operId, operName, loginName.Trim(), new SuperAreaRange(operId), new SuperVerification(operId), areaid);
                }
                else
                {
                    oper = new Operator(operId, operName, loginName.Trim(), new NormalAreaRange(operId), new NormalVerification(operId), areaid);
                }
            }

            return oper;
        }

        public static Operator OperatorCreate(String operatorId)
        {
            DbCommand cmdSelect = null;
            String operId = null;
            String operLogin = null;
            String operName = null;
            String operAttr = null;
            String operPwd = null;
            String operStatus = null;
            string areaid = null;

            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase =
               DatabaseFactory.CreateDatabase(SystemWebFormulation.SystemWebFormulation.Parameter("MainDataBaseName"));
            System.Data.IDataReader iReader = null;
            string sql = @"SELECT User_ID, UserRole_ID, 
                            MainArea_ID, User_Name, User_Login, 
                            User_Password, User_CreateDate, User_Status
                        FROM         
                            Sys_User
                        WHERE
                            User_ID=@User_ID";
            cmdSelect = mDataBase.GetSqlStringCommand(sql);
            mDataBase.AddInParameter(cmdSelect, "@User_ID", System.Data.DbType.String, operatorId.Trim());

            try
            {
                iReader = mDataBase.ExecuteReader(cmdSelect);
                if (iReader.Read())
                {
                    operId = iReader["User_ID"].ToString().Trim();
                    operName = iReader["User_Name"].ToString().Trim();
                    operAttr = iReader["UserRole_ID"].ToString().Trim();
                    operLogin = iReader["User_Login"].ToString().Trim();
                    operPwd = iReader["User_Password"].ToString().Trim();
                    operStatus = iReader["User_Status"].ToString().Trim();
                    areaid = iReader["MainArea_ID"].ToString().Trim();
                }
                else
                {
                    throw (new Exception("登陆名或密码错误！"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
            finally
            {
                if (iReader != null)
                {
                    iReader.Close();
                    iReader.Dispose();
                }
            }

            Operator oper = null;
            //if (operStatus == "0")
            //{
                if (operAttr == "1")
                {
                    oper = new Operator(operId, operName, operLogin, new SuperAreaRange(operId), new SuperVerification(operId), areaid);
                }
                else
                {
                    oper = new Operator(operId, operName, operLogin, new NormalAreaRange(operId), new NormalVerification(operId), areaid);
                }
            //}
            return oper;
        }

        public static Operator OperatorCreate(String operatorId,int temp)
        {
            DbCommand cmdSelect = null;
            String operId = null;
            String operLogin = null;
            String operName = null;
            String operAttr = null;
            String operPwd = null;
            String operStatus = null;
            string areaid = null;

            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase =
               DatabaseFactory.CreateDatabase(SystemWebFormulation.SystemWebFormulation.Parameter("MainDataBaseName"));
            System.Data.IDataReader iReader = null;
            string sql = @"SELECT User_ID, UserRole_ID, 
                            MainArea_ID, User_Name, User_Login, 
                            User_Password, User_CreateDate, User_Status
                        FROM         
                            Sys_User
                        WHERE
                            User_ID=@User_ID";
            cmdSelect = mDataBase.GetSqlStringCommand(sql);
            mDataBase.AddInParameter(cmdSelect, "@User_ID", System.Data.DbType.String, operatorId.Trim());

            try
            {
                iReader = mDataBase.ExecuteReader(cmdSelect);
                if (iReader.Read())
                {
                    operId = iReader["User_ID"].ToString().Trim();
                    operName = iReader["User_Name"].ToString().Trim();
                    operAttr = iReader["UserRole_ID"].ToString().Trim();
                    operLogin = iReader["User_Login"].ToString().Trim();
                    operPwd = iReader["User_Password"].ToString().Trim();
                    operStatus = iReader["User_Status"].ToString().Trim();
                    areaid = iReader["MainArea_ID"].ToString().Trim();
                }
                else
                {
                    throw (new Exception("登陆名或密码错误！"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
            finally
            {
                if (iReader != null)
                {
                    iReader.Close();
                    iReader.Dispose();
                }
            }

            Operator oper = null;
            if (operAttr == "1")
            {
                oper = new Operator(operId, operName, operLogin, new SuperAreaRange(operId), new SuperVerification(operId), areaid);
            }
            else
            {
                oper = new Operator(operId, operName, operLogin, new NormalAreaRange(operId), new NormalVerification(operId), areaid);
            }
            return oper;
        }
    }
}
