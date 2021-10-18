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
    /// ��Ա��ĳ־û�����
    /// ��Ա����ҳ������ص��ṩ�ķ���,
    /// ��Ա�����ɾ�ĵȲ���
    /// </summary>
    public class OperatorPersistent
    {
        private Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = null;
        private String mOPER_ID = null;				//��ԱID
        private String mROLE_ID = null;				//Ȩ����ID
        private String mOPER_Name = null;			//��Ա����
        private String mOPER_Login = null;			//��Ա��½��
        private String mOPER_Password = null;		//��Ա��½����
        private String mOPER_CreateDate = null;	//��Ա�������ڣ�ʱ��
        private String mOPER_Attribute = null;		//��Ա����
        private String mOPER_Status = null;		   //��Ա����
        private String mOPER_Remark = null;			//��Ա��ע
        public ArrayList AreaList = null;			//��������
        public ArrayList RoleList = null;			//Ȩ���鼯��
        private string areaid = null; //����

        #region ��������
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
                        throw (new Exception("��ԱȨ���鲻��Ϊ�գ�"));
                    }
                    else
                    {
                        this.mROLE_ID = value.Trim();
                    }
                }
                else
                {
                    throw (new Exception("��ԱȨ���鲻��Ϊ�գ�"));
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
                        throw (new Exception("��Ա��½���Ʋ���Ϊ�գ�"));
                    }
                    else
                    {
                        if (ObjectMath.IsOverLong(value, 20))
                        {
                            throw (new Exception("��Ա��½��������ܳ���20�ַ�����10�������ַ���"));
                        }
                        else
                        {
                            this.mOPER_Login = value.Trim();
                        }
                    }
                }
                else
                {
                    throw (new Exception("��Ա��½���Ʋ���Ϊ�գ�"));
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
                        throw (new Exception("��Ա���Ʋ���Ϊ�գ�"));
                    }
                    else
                    {
                        if (ObjectMath.IsOverLong(value, 20))
                        {
                            throw (new Exception("��Ա��������ܳ���20�ַ�����10�������ַ���"));
                        }
                        else
                        {
                            this.mOPER_Name = value.Trim();
                        }
                    }
                }
                else
                {
                    throw (new Exception("��Ա���Ʋ���Ϊ�գ�"));
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
                        throw (new Exception("��½���벻��Ϊ�գ�"));
                    }
                    else
                    {
                        //if (ObjectMath.IsOverLong(value, 20))
                        //{
                        //    throw (new Exception("��½��������ܳ���20�ַ�����10�������ַ���"));
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
                    throw (new Exception("��½���벻��Ϊ�գ�"));
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
                            throw (new Exception("��Ա״̬����ܳ���4�ַ�����2�������ַ���"));
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
                            throw (new Exception("��Ա��ע����ܳ���50�ַ�����25�������ַ���"));
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

        #region ���캯��
        public OperatorPersistent()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
            AreaList = new ArrayList();
        }

        public OperatorPersistent(string id)
        {
            this.mOPER_ID = id.Trim();
            AreaList = new ArrayList();

            //��ȡ��ǰ�û�����Ϣ
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
                    throw (new Exception("�޴���Ա��Ϣ��"));
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

        #region ��ȡָ����Ա�������Ա�б�
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
                throw (new Exception("��ѯ��ǰ��Ա�ɹ����ȫ����������" + e.Message));
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

        #region ��Ա��������޸�
        /// <summary>
        /// ��Ա����ӻ��޸�
        /// </summary>
        public void Update(ArrayList groupList, ArrayList funList, string privateGroupID, bool isme)
        {
            //�ж������Ƿ��ظ�
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

                    #region ���������޸��û���Ϣ
                    if (this.OPER_ID == null)//�����û���Ϣ
                    {
                        sb.Append("INSERT INTO Sys_User(UserRole_ID,MainArea_ID,User_Name,User_Login,User_Password,User_CreateDate,User_Status,User_Remark)");
                        sb.Append(" VALUES(@UserRole_ID,@MainArea_ID,");
                        sb.Append("@User_Name,@User_Login,@User_Password,getdate(),0,@User_Remark);select @@identity");
                    }
                    else//�޸��û���Ϣ
                    {
                        OperatorPersistent oper = new OperatorPersistent(this.OPER_ID);
                        sb.Append("UPDATE Sys_User SET UserRole_ID=@UserRole_ID,MainArea_ID=@MainArea_ID,User_Name=@User_Name,");
                        sb.Append("User_Login=@User_Login,User_Password=@User_Password,User_Status=@status,User_Remark=@User_Remark WHERE User_ID=@User_ID");
                    }
                    cmdSelect = mDataBase.GetSqlStringCommand(sb.ToString());
                    mDataBase.AddInParameter(cmdSelect, "@UserRole_ID", System.Data.DbType.String, this.ROLE_ID);

                    mDataBase.AddInParameter(cmdSelect, "@MainArea_ID", System.Data.DbType.String, this.areaid);
                    //������޸ģ�������״̬
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
                        //�������,��ȡ��ǰ�����û���ID,��������������û�������ĵ���
                        tempOper_id = mDataBase.ExecuteScalar(cmdSelect).ToString();
                    }
                    else
                    {
                        mDataBase.ExecuteNonQuery(cmdSelect, trans);
                        tempOper_id = this.OPER_ID;
                    }

                    #endregion ���������޸��û���Ϣ

                    if (!string.IsNullOrEmpty(ROLE_ID) && !ROLE_ID.Equals("1"))
                    {
                        //������������򴴽��û�˽����
                        string sql = null;
                        if (this.OPER_ID == null)
                        {
                            sql = @"INSERT INTO Sys_PrivilegeGroup(User_ID,PrivGroup_Name,PrivGroup_Desc,PrivGroup_IsPrivate) VALUES (@User_ID,@PrivGroup_Name,@PrivGroup_Desc,0);select @@identity";
                            cmdSelect = mDataBase.GetSqlStringCommand(sql);
                            mDataBase.AddInParameter(cmdSelect, "@User_ID", System.Data.DbType.Int32, tempOper_id);
                            mDataBase.AddInParameter(cmdSelect, "@PrivGroup_Name", System.Data.DbType.String, this.OPER_Name + "��");
                            mDataBase.AddInParameter(cmdSelect, "@PrivGroup_Desc", System.Data.DbType.String, "�û� '" + this.OPER_Name + "' ��˽����");
                            privateGroupID = mDataBase.ExecuteScalar(cmdSelect, trans).ToString();//ִ�и���
                        }

                        if (!isme)//������Լ��޸��Լ�����Ϣ����ʲô������
                        {
                            //������޸���ɾ��˽���鹦��Ȩ�����Ӧ��Ȩ����
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
                                //���˽����Ȩ��
                                sqltemp = "INSERT INTO Sys_PrivilegeFunctionMap(Fun_ID,PrivGroup_ID) VALUES({0},@PrivGroup_ID)";

                                foreach (string s in funList)
                                {
                                    sqlList.Append(string.Format(sqltemp, s));
                                }

                                cmdSelect = mDataBase.GetSqlStringCommand(sqlList.ToString());
                                mDataBase.AddInParameter(cmdSelect, "@PrivGroup_ID", System.Data.DbType.String, privateGroupID.Trim());
                                mDataBase.ExecuteNonQuery(cmdSelect, trans);
                            }

                            //������Ա˽�����Ӧ��ϵ
                            sql = "INSERT INTO Sys_UserPrivilegeMap(PrivGroup_ID,User_ID) VALUES (@PrivGroup_ID,@mOPER_ID);";
                            cmdSelect = mDataBase.GetSqlStringCommand(sql);
                            mDataBase.AddInParameter(cmdSelect, "@PrivGroup_ID", System.Data.DbType.Int32, privateGroupID);
                            mDataBase.AddInParameter(cmdSelect, "@mOPER_ID", System.Data.DbType.Int32, tempOper_id);
                            mDataBase.ExecuteNonQuery(cmdSelect, trans);//ִ�и���

                            if (groupList.Count > 0)
                            {
                                //���Ȩ����
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

                    //ɾ����ǰ�û�������Ȩ�����Ȩ��
                    if (this.OPER_ID != null)
                    {
                        //GetDeleteGroupId(this.OPER_ID);//��ʱ��ɾ��
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

        #region ������ԱȨ����
        /// <summary>
        /// ������ԱȨ����
        /// </summary>
        /// <param name="operId">��ԱID</param>
        /// <param name="list">Ȩ�����б�</param>
        public void SetRoleGroup(string operId, ArrayList list)
        {
            //��ɾ��Ȩ�ޣ����������
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

        #region ɾ����ԱȨ����
        /// <summary>
        /// ɾ����ԱȨ����
        /// </summary>
        /// <param name="operId">��ԱID</param>
        public void DelRoleGroup(string operId)
        {
            DbConnection connection;
            DbTransaction trans;
            DbCommand cmdSelect;
            Database dataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.Parameter("MainDataBaseName"));
            connection = dataBase.CreateConnection();
            connection.Open();
            trans = connection.BeginTransaction();
            //��ɾ��˽����
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

        #region �߼�ɾ����Ա
        /// <summary>
        /// �߼�ɾ����Ա
        /// </summary>
        /// <param name="operId">��ԱID</param>
        /// <param name="state">״̬ 1=���ã�0=����</param>
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

        #region ��ȡ�ܵ���
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
                throw (new Exception("��ѯ��ǰ��Ա�ɹ����ȫ�������ݼ�������Ϣ����" + e.Message));
            }

            return dt;
        }
        #endregion

        #endregion

        #region ɾ����Ա˽���鹦����
        /// <summary>
        /// ɾ����Ա˽���鹦����
        /// </summary>
        /// <param name="operId">��ԱID</param>
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

        #region �����Ա˽���鹦����
        /// <summary>
        /// �����Ա˽���鹦����
        /// </summary>
        /// <param name="operId">��ԱID</param>
        ///  <param name="privateId">˽����ID</param>
        /// <param name="funList">������ID����</param>
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

        #region �ж���Ա���Ƿ�����Ա���ڴ�Ȩ�޷��飬������ɾ��Ȩ�޷���ʱ��Ȩ�޷�����õ��ô˷�����
        /// <summary>
        /// �ж���Ա���Ƿ�����Ա���ڴ�Ȩ�޷���
        /// ������ɾ��Ȩ�޷���ʱ��Ȩ�޷�����õ��ô˷�����
        /// </summary>
        /// <param name="groupId">Ȩ�޷���ID</param>
        /// <returns>�������Ա���ڴ˷��鷵��TRUE�����˷���FALSE</returns>
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

        #region �ж���Ա��½�����Ƿ��ظ�
        /// <summary>
        /// �жϵ�ǰ���������Ƿ����ظ�
        /// </summary>
        /// <param name="name">��Ա��½����</param>
        /// <param name="operId">��ǰ��Ա��ID��������������Ա����Ȼ��ԱIDΪNULL</param>
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
                    throw (new Exception("�˵�½���Ѿ����ڣ�"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
        }
        #endregion

        #region �õ���ǰ�û���Ȩ�޵�ȫ�������µ���Ա��Ϣ��
        /// <summary>
        /// �õ���ǰ�û���Ȩ�޵�ȫ�������µ���Ա��Ϣ��
        /// </summary>
        /// <param name="dtArea">��ǰ��Ȩ�޵ĵ����б�</param>
        /// <returns>��Ա��Ϣ����</returns>
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

        #region ��ȡָ�������µĹ���Ա��Ϣ�б�
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

        #region �õ�ȫ������
        /// <summary>
        /// �õ���ǰ��Ա�µ�ȫ�������б�
        /// </summary>
        /// <returns>ARRAYLIST����</returns>
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
                throw (new Exception("��ѯ��ǰ��Ա�ɹ����ȫ����������" + e.Message));
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

        #region �ж�ɾ���û��ĵ���Ȩ���Ƿ���ڵ�ǰ��¼�û�
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

        #region �ж�ɾ���û��ĵ���Ȩ���Ƿ���ڵ�ǰ��¼�û�
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
                throw (new Exception("�鿴�û����Ƿ������־��Ϣ����" + ex.Message));
            }

            return rt;
        }

        #endregion

        #region  �����޸�
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
                throw (new Exception("�޸��������" + ex.Message));
            }
        }
        #endregion

        /// <summary>
        /// ��ȡҪɾ���û�����Ĺ�����
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

                #region ������޸ģ����ж��û��Ĺ������Ƿ���٣�������٣���ɾ�����������������

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
                    //ִ��ɾ��
                    delSql = "delete Sys_PrivilegeFunctionMap where " + delSql;
                    cmdSelect = mDataBase.GetSqlStringCommand(delSql);
                    mDataBase.ExecuteNonQuery(cmdSelect, trans);//ִ�и���
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
