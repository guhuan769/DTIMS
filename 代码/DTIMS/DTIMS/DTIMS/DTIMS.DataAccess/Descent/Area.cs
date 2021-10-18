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
    /// �����࣬�ṩ������ع���
    /// </summary>
    public class Area
    {
        private Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = null;
        private String mAreaId = null;			//����ID		
        private String mFatherMainArea_ID = null;	//������ID	
        private String mMainArea_Name = null;		//����������
        private string opertorid;                    //������ԱID

        #region ��������
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
                    throw (new Exception("�����ڵ�����������Ϊ�գ�"));
                }
                if (value.Trim() == "")
                {
                    throw (new Exception("�����ڵ�����������Ϊ�գ�"));
                }
                if (BJ.Secuirty.CheckString.HaveSpecialChar(value))
                {
                    throw (new Exception("�����ڵ����������ܰ��������ַ���"));
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
                    throw (new Exception("�������ڵ㲻��Ϊ�գ�"));
                }
                if (value.Trim() == "")
                {
                    throw (new Exception("�������ڵ㲻��Ϊ�գ�"));
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

        #region ���캯��
        public Area()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        /// <summary>
        /// ʵ��������
        /// </summary>
        /// <param name="areaId">����ID</param>
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
                    throw (new Exception("ϵͳ�в����ڴ˵�����"));
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

        #region ���/�޸�
        /// <summary>
        /// �޸Ļ���ӵ�����Ϣ
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

                    //�޸Ļ���ӵ���
                    int areaID = 1;
                    if (this.AreaId == null)//����
                    {
                        sb.Append("INSERT INTO S_MainArea (MainArea_ID,FatherMainArea_ID,MainArea_Name) VALUES(@MainArea_ID,@father,@name)");

                        //��ѯ����ӵ�����ID��
                        cmdSelect = mDataBase.GetSqlStringCommand("SELECT TOP 1 MainArea_ID FROM S_MainArea ORDER BY MainArea_ID DESC");
                        DataTable dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            areaID = Convert.ToInt16(dt.Rows[0]["MainArea_ID"]) + 1;//���ID��+1
                        }
                    }
                    else//�޸�
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

                    if (this.AreaId != null)//�޸�
                    {
                        mDataBase.AddInParameter(cmdSelect, "@id", System.Data.DbType.String, this.AreaId);
                    }
                    else
                    {
                        mDataBase.AddInParameter(cmdSelect, "@MainArea_ID", System.Data.DbType.Int16, areaID);
                    }

                    mDataBase.ExecuteNonQuery(cmdSelect, trans);

                    //���������,���µ��������е�ǰ��Ա�Ĺ���Ȩ��
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

        #region ɾ������
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

                //ɾ������������Ա��Ĺ�����ϵ
                string sql = "DELETE S_OperatorMainAreaMap WHERE MainArea_ID=@id";
                cmdSelect = mDataBase.GetSqlStringCommand(sql);
                mDataBase.AddInParameter(cmdSelect, "@id", DbType.String, this.AreaId);
                mDataBase.ExecuteNonQuery(cmdSelect, trans);

                //ɾ���������е�����
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

        #region �г�ȫ������
        /// <summary>
        /// ���ȫ�������б�
        /// </summary>
        /// <returns>����DataTable</returns>
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
                throw (new Exception("��ѯȫ����������" + e.Message));
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
                throw (new Exception("��ѯȫ����������" + e.Message));
            }

            return dt;
        }
        #endregion

        #region �жϵ��������Ƿ��ظ�
        /// <summary>
        /// �жϵ�ǰ���������Ƿ����ظ�
        /// </summary>
        /// <param name="group">��������</param>
        /// <param name="groupId">��ǰ�����ID�����������ӷ��飬��Ȼ����IDΪNULL</param>
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
                    throw (new Exception("�������Ʋ����ظ���"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
        }
        #endregion

        #region �жϵ�ǰ�����Ƿ����ӽڵ�
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
                    throw (new Exception("�õ��������ӽڵ㲻��ɾ����"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
        }
        #endregion

        #region  �õ��е�ǰ�����豸����Ȩ�޵�ȫ����Ա�б�
        /// <summary>
        /// �õ��е�ǰ�����豸����Ȩ�޵�ȫ����Ա�б�
        /// </summary>
        /// <param name="areaId">��ǰ����ID</param>
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

        #region  ��ȡָ�������г��Լ���ӵ��Ȩ�޵�ȫ����Ա�б�
        /// <summary>
        /// ��������:��ȡָ�������г��Լ���ӵ��Ȩ�޵�ȫ����Ա�б�
        /// �޸�˵��:2009��3��23�� ����
        /// <author>MXJ</author>
        /// </summary>
        /// <param name="areaId">��ǰ����ID</param>
        /// <param name="operid">��ԱID</param>
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

        #region  �õ���ǰ�����µ�ȫ���豸����
        /// <summary>
        /// �õ���ǰ�����µ�ȫ���豸����
        /// </summary>
        /// <param name="areaId">��ǰ����ID</param>
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

        #region  �õ���ǰ�����µĹ�������
        /// <summary>
        /// �õ���ǰ�����µĹ�������
        /// </summary>
        /// <param name="areaId">��ǰ����ID</param>
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

        #region  �õ���ǰ�����µ�״̬���Ʒ�ʽ����
        /// <summary>
        /// �õ���ǰ�����µ�״̬���Ʒ�ʽ����
        /// </summary>
        /// <param name="areaId">��ǰ����ID</param>
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

        #region �ж�ָ���������Ƿ���MDN�Ŷ���Ϣ��
        /// <summary>
        /// ��������:�ж�ָ���������Ƿ���MDN�Ŷ���Ϣ��
        /// �޸�˵��:2009-3-23 ����
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

        #region �ж�ָ���������Ƿ���IMSI�Ŷ���Ϣ��
        /// <summary>
        /// ��������:�ж�ָ���������Ƿ���IMSI�Ŷ���Ϣ��
        /// �޸�˵��:2009-3-23 ����
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
