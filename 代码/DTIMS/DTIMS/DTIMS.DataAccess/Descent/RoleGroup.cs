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
    /// Ȩ�����࣬������Ȩ������صķ�����
    /// ��Ҫ����Ȩ���������ɾ���ģ����о���Ϊ��Ա���ṩ����������Ա�࣬
    /// ������Ա������ص�Ȩ�޹����
    /// </summary>
    public class RoleGroup
    {
        private Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = null;
        public ArrayList FunctionItems = null;						//�������
        private String mId = null;						//Ȩ����ID��Ψһ��ʾ
        private String mRoleGroupName = null;						//Ȩ��������
        private String mRoleGroupDes = null;						//Ȩ����˵��

        #region ��������
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
                            throw (new Exception("����˵�����ܰ��������ַ���"));
                        }
                        if (ObjectMath.IsOverLong(value, 50))
                        {
                            throw (new Exception("����˵�����ܳ���50���ַ���25�����֣�"));
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
                    throw (new Exception("������������Ʋ���Ϊ�գ�"));
                }
                if (value.Trim() == "")
                {
                    throw (new Exception("������������Ʋ���Ϊ�գ�"));
                }
                if (Sys.Comm.Secuirty.CheckString.HaveSpecialChar(value))
                {
                    throw (new Exception("���������Ʋ��ܰ��������ַ���"));
                }
                if (ObjectMath.IsOverLong(value, 20))
                {
                    throw (new Exception("�������Ʋ��ܳ���20���ַ���10�����֣�"));
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

        #region ���캯��
        public RoleGroup()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
            FunctionItems = new ArrayList();
            this.mId = null;
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="id">����ID</param>
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

                    //��ȡ��Ȩ�޷����Ӧ��ȫ��������
                    this.FunctionItems = GetFunctionList(this.Id);
                }
                else
                {
                    throw (new Exception("�޴�Ȩ�޷��飡"));
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
        /// ��Ȩ����õ���Ӧ��ȫ���������
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

        #region ��ӻ��޸�
        public void Update()
        {
            //�ж������Ƿ��ظ�
            IsGroupNameDuplic(this.RoleGroupName, this.Id);

            if (this.FunctionItems.Count == 0)
            {
                throw (new Exception("Ȩ�޷�������Ӧ����һ�������"));
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

                    //�޸Ļ����Ȩ�޷���
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

                    //�޸Ļ���ӷ����빦�����ϵ
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

        #region �жϷ��������Ƿ��ظ�
        /// <summary>
        /// �жϵ�ǰ���������Ƿ����ظ�
        /// </summary>
        /// <param name="group">��������</param>
        /// <param name="groupId">��ǰ�����ID�����������ӷ��飬��Ȼ����IDΪNULL</param>
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
                    throw (new Exception("��������������Ȩ�޷����ظ���"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
        }
        #endregion

        #region ɾ������
        /// <summary>
        /// ɾ��Ȩ�޷��飬ϵͳ������˷����Ƿ�����Աӵ�У�������������ʾ�û�
        /// </summary>
        /// <param name="groupId">����ID</param>
        public static void Delete(string groupId)
        {
            DTIMS.OperatorPersistent.Descent.OperatorPersistent oper = new DTIMS.OperatorPersistent.Descent.OperatorPersistent();
            if (oper.IsHaveRoleGroup(groupId))
            {
                throw (new Exception("������Ա���ڴ˷��飬�㲻��ɾ����"));
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

        #region ��ѯȫ��Ȩ����
        /// <summary>
        /// ��ѯȫ��Ȩ����
        /// </summary>
        /// <param name="isPublic">�Ƿ��ǹ���Ȩ����</param>
        /// <param name="oper">��ǰ�û�</param>
        /// <returns></returns>
        public DataTable ListAll(bool isPublic,  Sys.Project.Common.Operator oper)
        {
           
            DataTable dt = null;
            string isPublicstr = isPublic ? "1" : "0";
            DbCommand cmdSelect;
            Database dataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));

            String sqlstr = "";
            if (oper.IsSuper)//����ǳ����û����ѯ�Ĵ�ʡ����Ȩ����
            {
                sqlstr = "SELECT * FROM Sys_PrivilegeGroup where PrivGroup_IsPrivate=" + isPublicstr + " and  User_ID in (select User_ID from Sys_User where MainArea_ID=1)";
            }
            else//����ֻ��ѯ�Լ�������Ȩ����
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

        #region ��ѯ�û����е�Ȩ����
        /// <summary>
        /// ��ѯ�û����е�Ȩ����
        /// </summary>
        /// <param name="userid">�û�id</param>
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

        #region �г�ϵͳ��ȫ���������
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
                throw (new Exception("��ѯ���������,ԭ��:" + e.Message));
            }

            return dt;
        }
        #endregion

        #region �г�ϵͳ��ȫ��BigCategory����

        /// <summary>
        /// �г�ϵͳ��ȫ��BigCategory����
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
                throw (new Exception("��ѯ���ܴ������,ԭ��:" + e.Message));
            }

            return dt;
        }
        #endregion

        #region �г�ϵͳ��ָ����FunctionCategory����

        /// <summary>
        /// �г�ϵͳ��ָ����FunctionCategory����
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
                throw (new Exception("��ѯFunctionCategory����,ԭ��:" + e.Message));
            }

            return dt;
        }
        #endregion

        #region �г�ϵͳ��ָ����FunctionItem����

        /// <summary>
        /// �г�ϵͳ��ָ����FunctionItem����
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
                throw (new Exception("��ѯFunctionItem����,ԭ��:" + e.Message));
            }

            return dt;
        }
        #endregion

        #region ��ȡ��ɫ

        /// <summary>
        /// ��ȡ��ɫ
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
                throw (new Exception("��ѯFunctionItem����,ԭ��:" + e.Message));
            }

            return dt;
        }
        #endregion
    }
}
