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
    /// �����������û�Ȩ����
    /// �޸�˵����2010-4-7������
    /// <author>MXJ</author>
    /// </summary>
    public class Sys_PrivilegeGroup
    {
        private string mPrivGroup_ID = null;
        private string mPrivGroup_Name = null;
        private string mPrivGroup_Desc = null;
        private bool mPrivGroup_IsPrivate = false;
        private Hashtable mHashtableFunc_ID = null;

        #region ��������
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
        /// Ȩ������
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
        /// Ȩ��˵��
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
        /// ��ʶ�Ƿ����û�˽����
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
        /// ������ID����
        /// </summary>
        public Hashtable HashtableFunc_ID
        {
            get
            {
                return mHashtableFunc_ID;
            }
        }
        
        #endregion

        #region ���캯��
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
                //��ȡȨ������Ϣ
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
                    throw (new Exception("δ�ҵ�Ȩ������Ϣ��"));
                }

                //��ȡ��Ӧ��Ȩ��ID��Ϣ
                sql = "SELECT Fun_ID, PrivGroup_ID FROM Sys_PrivilegeFunctionMap WHERE (PrivGroup_ID = @PrivGroup_ID)";
                cmdSelect = mDataBase.GetSqlStringCommand(sql);
                mDataBase.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, privGroup_ID);
                DataTable dtFunc = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];

                //���õ�ǰȨ�������Ϣ
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
                throw (new Exception("��ѯȨ������Ϣ����,ԭ��:" + e.Message));
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
        #endregion ���캯��

        #region �޸�

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

            if (this.mPrivGroup_ID != null && this.mPrivGroup_ID.Length > 0)//�޸�ʱ����Ҫ���Լ��Ƚ�
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
                throw new Exception("Ȩ���顾"+this.mPrivGroup_Name+"���Ѿ�����,���������롣");
            }
        }

        /// <summary>
        /// ���������޸�Ȩ����
        /// </summary>
        /// <param name="htFunc">������ID����</param>
        /// <param name="oper">��¼�û�����</param>
        public void Update(Hashtable htFunc,Operator oper)
        {
            try
            {
                //Ȩ���������ж��Ƿ����
                this.IsExistGroupName(oper);

                string sql = "";
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand cmdSelect;
                using (DbConnection conn = db.CreateConnection())
                {
                    //������
                    conn.Open();
                    //��������
                    DbTransaction trans = conn.BeginTransaction();
                    try
                    {
                        //�����޸���Ȩ�޷���
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

                        //ִ�����������޸�
                        cmdSelect = db.GetSqlStringCommand(sql);
                        db.AddInParameter(cmdSelect, "@PrivGroup_Name", DbType.String, this.mPrivGroup_Name);
                        db.AddInParameter(cmdSelect, "@PrivGroup_Desc", DbType.String, this.mPrivGroup_Desc);

                        string groupID = "";
                        if (this.mPrivGroup_ID == null)
                        {
                            db.AddInParameter(cmdSelect, "@User_ID", DbType.String, oper.OperatorId);
                            groupID = db.ExecuteScalar(cmdSelect,trans).ToString().Trim();//ִ����������ȡ������������ID
                        }
                        else
                        {
                            db.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, this.mPrivGroup_ID);
                            db.ExecuteNonQuery(cmdSelect, trans);
                            groupID = this.mPrivGroup_ID;
                        }

                        if (groupID == null || groupID == "")
                        {
                            throw new Exception("����Ȩ��ʱ����.");
                        }

                        //ɾ��Ȩ����
                        if (this.mPrivGroup_ID != null)
                        {
                            sql = "DELETE FROM Sys_PrivilegeFunctionMap WHERE (PrivGroup_ID = @PrivGroup_ID)";
                            cmdSelect = db.GetSqlStringCommand(sql);
                            db.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, this.mPrivGroup_ID);
                            db.ExecuteNonQuery(cmdSelect, trans);
                        }

                        //����Ȩ����
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

                        //ɾ����ǰ�������Ӧ���û�������Ȩ����Ȩ��
                        if (this.mPrivGroup_ID != null)
                        {
                            DeleteFunc(this.mPrivGroup_ID, trans, db);
                        }

                        //��ִ�гɹ����ύ����
                        trans.Commit();
                    }
                    catch (Exception ee)
                    {
                        trans.Rollback();//�����쳣������ع�
                        throw (new Exception(ee.Message));
                    }
                    finally
                    {
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();//�ر�����
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
            /*1:����ǰȨ����Ĺ���Ȩ���뵱ǰȨ�����µ��û���������бȽϣ��������ȡ����
             * 2:���ȽϵĽ���뵱ǰ���µ��û�����������Ȩ����Ĺ��ܱȽ�
             * 3:ɾ������Ĺ���Ȩ��
             */
            DbCommand cmdSelect;
            string sql = @"
                            /*ָ��Ȩ�����µ��û�������Ȩ�����Ȩ��*/
                            SELECT DISTINCT Sys_PrivilegeFunctionMap.Fun_ID, 
                            Sys_PrivilegeFunctionMap.PrivGroup_ID
                            FROM         Sys_PrivilegeGroup INNER JOIN
                                                  Sys_PrivilegeFunctionMap ON Sys_PrivilegeGroup.PrivGroup_ID = Sys_PrivilegeFunctionMap.PrivGroup_ID
                            AND Sys_PrivilegeGroup.User_ID IN(
                            /*��ǰ���µ��û�*/
                            SELECT     Sys_UserPrivilegeMap.User_ID
                            FROM         Sys_PrivilegeGroup INNER JOIN
                                                  Sys_UserPrivilegeMap ON Sys_PrivilegeGroup.PrivGroup_ID = Sys_UserPrivilegeMap.PrivGroup_ID
                            WHERE     (Sys_PrivilegeGroup.PrivGroup_ID = @PrivGroup_ID))
                            AND Sys_PrivilegeFunctionMap.Fun_ID NOT IN( /*����ǰȨ�����µ��û�������Ȩ���뵱ǰȨ�����Ȩ�޽��бȽ�*/
                            /*ָ��Ȩ��������¹���Ȩ��*/
                            SELECT     Fun_ID
                            FROM         Sys_PrivilegeFunctionMap
                            WHERE     (PrivGroup_ID = @PrivGroup_ID))
                            AND

                            /*�жϵ�ǰȨ�����±ȽϺ����Ĺ������Ƿ�������������*/
                            Sys_PrivilegeFunctionMap.Fun_ID NOT IN
                            (
	                            /*�жϱȽ϶����Ȩ�������������Ƿ����*/
	                            SELECT DISTINCT Sys_PrivilegeFunctionMap.Fun_ID
	                            FROM         Sys_UserPrivilegeMap INNER JOIN
		                            Sys_PrivilegeGroup ON Sys_UserPrivilegeMap.PrivGroup_ID = Sys_PrivilegeGroup.PrivGroup_ID INNER JOIN
		                            Sys_PrivilegeFunctionMap ON Sys_PrivilegeGroup.PrivGroup_ID = Sys_PrivilegeFunctionMap.PrivGroup_ID
	                            AND Sys_UserPrivilegeMap.User_ID IN(
	                            /*��ǰ���µ��û�*/
	                            SELECT     Sys_UserPrivilegeMap.User_ID
	                            FROM         Sys_PrivilegeGroup INNER JOIN
						                              Sys_UserPrivilegeMap ON Sys_PrivilegeGroup.PrivGroup_ID = Sys_UserPrivilegeMap.PrivGroup_ID
	                            WHERE     (Sys_PrivilegeGroup.PrivGroup_ID = @PrivGroup_ID))
	                            AND Sys_PrivilegeGroup.PrivGroup_ID <> @PrivGroup_ID
                            )";
            cmdSelect = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, groupID);
            DataTable dtFunc = db.ExecuteDataSet(cmdSelect, trans).Tables[0];

            //ɾ������Ĺ�����
            foreach (DataRow dr in dtFunc.Rows)
            {
                sql = "DELETE FROM Sys_PrivilegeFunctionMap WHERE (PrivGroup_ID = {0}) AND (Fun_ID = '{1}')";
                cmdSelect = db.GetSqlStringCommand(string.Format(sql, dr["PrivGroup_ID"].ToString().Trim(), dr["Fun_ID"].ToString().Trim()));
                db.ExecuteNonQuery(cmdSelect, trans);
            }
        }
        #endregion �޸�

        #region ɾ��
        /// <summary>
        /// �����������ж�ָ��Ȩ�������Ƿ����û�
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
        /// ɾ��Ȩ����
        /// </summary>
        public void Delete()
        {
            //�ж��Ƿ����û����ڸ��飨������򷵻ش���
            if (Sys_PrivilegeGroup.IsHaveUser(this.PrivGroup_ID))
            {
                throw (new Exception("������Ա���ڴ˷��飬������ɾ����"));
            }

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cmdSelect;
            using (DbConnection conn = db.CreateConnection())
            {
                //������
                conn.Open();
                //��������
                DbTransaction trans = conn.BeginTransaction();

                try
                {
                    //ɾ��Ȩ�����빦�����Ӧ��ϵ
                    string sql = "DELETE FROM Sys_PrivilegeFunctionMap WHERE (PrivGroup_ID = @PrivGroup_ID)";
                    cmdSelect = db.GetSqlStringCommand(sql);
                    db.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, this.mPrivGroup_ID);
                    db.ExecuteNonQuery(cmdSelect, trans);

                    //ɾ���û�Ȩ����
                    sql = "DELETE FROM Sys_PrivilegeGroup WHERE (PrivGroup_ID = @PrivGroup_ID)";
                    cmdSelect = db.GetSqlStringCommand(sql);
                    db.AddInParameter(cmdSelect, "@PrivGroup_ID", DbType.String, this.mPrivGroup_ID);
                    db.ExecuteNonQuery(cmdSelect, trans);

                    //�ύ����
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("ɾ��Ȩ����ʱ����,��ϸ:" + ex.Message);
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

        #region ��ȡ

        /// <summary>
        /// ��ȡ���з�˽�е�Ȩ����
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
                throw (new Exception("��ѯȨ������Ϣ����,ԭ��:" + ex.Message));
            }
            return dt;
        }
        #endregion

        #region ��ȡָ���û���Ȩ����
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
                throw (new Exception("��ȡָ���û���Ȩ����,ԭ��:" + ex.Message));
            }
            return dt;
        }
        #endregion

    }//end  public class Sys_PrivilegeGroup
}//end namespace Inphase.Project.Common
