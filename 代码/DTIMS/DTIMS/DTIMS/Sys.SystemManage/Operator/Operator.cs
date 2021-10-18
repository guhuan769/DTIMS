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
    /// ��Ա�ࡣ
    /// </summary>
    [Serializable]
    public class Operator
    {
     
        private String mOperatorId = null;			//��ԱID
        private String mOperName = null;			//��Ա����
        private String mOperLogin = null;			//��Ա��½����
        private String mOperPwd = null;			//��Ա����
        public IAreaInfo AreaRoleInfo = null;		//������Ϣ�ӿ�
        public IRoleVerify RoleVerify = null;		//Ȩ����֤�ӿ�
        private string mArea_ID = null;             //����ID
        private string client_IP = null;

        #region ��������
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
        /// �ͻ���IP
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
        /// ����ID
        /// </summary>
        public string Area_ID
        {
            get
            {
                return this.mArea_ID;
            }
        }

        /// <summary>
        /// ��Ա��½����
        /// </summary>
        public String OperLogin
        {
            get
            {
                return this.mOperLogin;
            }
        }

        /// <summary>
        /// ��Ա����
        /// </summary>
        public String OperPwd
        {
            get
            {
                return this.mOperPwd;
            }
        }

        /// <summary>
        /// ��Ա����
        /// </summary>
        public String OperName
        {
            get
            {
                return this.mOperName;
            }
        }

        /// <summary>
        /// ��ԱID
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
        /// ���캯��
        /// </summary>
        /// <param name="operId">��ԱID</param>
        /// <param name="operName">��Ա����</param>
        /// <param name="operLogin">��Ա��½����</param>
        /// <param name="IAreaInfo">��Ա�����ӿ�</param>
        /// <param name="IRoleVerify">��ԱȨ����֤�ӿ�</param>
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
        /// ���캯��
        /// </summary>
        /// <param name="operId">��ԱID</param>
        /// <param name="operName">��Ա����</param>
        /// <param name="operLogin">��Ա��½����</param>
        /// <param name="IAreaInfo">��Ա�����ӿ�</param>
        /// <param name="IRoleVerify">��ԱȨ����֤�ӿ�</param>
        public Operator(String operId, String operName, String operLogin, String pwd,
              IAreaInfo areaInfo, IRoleVerify verify,string areaID)
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
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
        /// �õ���ǰ��Ա�ĵ�½����
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
                    throw (new Exception("ϵͳ���޴���Ա��Ϣ��"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception("��ѯ��Ա�����Ϣ����,ԭ��:" + e.Message));
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
        /// ��֤��ǰ��Ա�Ƿ���Ȩ��
        /// </summary>
        /// <param name="function">������ID</param>
        /// <returns>�����Ȩ���򷵻�TRUE�����û��Ȩ���򷵻�FALSE</returns>
        public bool Verify(String function)
        {
            return this.RoleVerify.Verify(function);
        }

        /// <summary>
        /// ������ֱ��Ȩ�޵ĵ����б�
        /// </summary>
        /// <returns>ArrayList,�ɵ���ID��ɵļ���</returns>
        public DataTable GetDirectRoleArea(String dataBaseInstance)
        {
            return this.AreaRoleInfo.GetDirectRoleArea(dataBaseInstance);
        }

        /// <summary>
        /// ������Ա��Ȩ�޵�ȫ�������б���Ҫ�����������������Ա����
        /// </summary>
        /// <returns>ArrayList,�ɵ���ID��ɵļ���</returns>
        public DataTable GetAllRoleArea(String dataBaseInstance)
        {
            return this.AreaRoleInfo.GetAllRoleArea(dataBaseInstance);
        }

        /// <summary>
        /// ����ָ�������°�����������������µ�ȫ���ӵ����ļ���
        /// </summary>
        /// <param name="dataBaseInstance">���ݿ�ʵ������</param>
        /// <param name="areaId">����ID</param>
        /// <returns></returns>
        public DataTable GetAllChildArea(String dataBaseInstance, String areaId)
        {
            return this.AreaRoleInfo.GetAllChildArea(dataBaseInstance, areaId);
        }

        /// <summary>
        /// ���ص�ǰ��Ա����Ȩ�޵�ȫ�������ݼ�����
        /// </summary>
        /// <returns>DataTable.��������ID���������ƣ��������ݿ�����</returns>
        public DataTable GetMainArea()
        {
            return this.AreaRoleInfo.GetMainAreaInfo();
        }

        /// <summary>
        /// ���ݴ�������ݿ�ʵ�����ƺ͵���ID,�õ�����������ȫ��������Ӧ��yzf��������,
        /// ��ID֮����","�ŷָ�
        /// </summary>
        /// <param name="dataBaseName">���ݿ�ʵ������</param>
        /// <param name="areaId">����ID</param>
        /// <returns></returns>
        public String GetYZFAreaList(String dataBaseName, String areaId)
        {
            return this.AreaRoleInfo.GetYZFAreaList(dataBaseName, areaId);
        }

        /// <summary>
        /// ���ݴ�������ݿ�ʵ�����ƺ͵���ID,�õ�����������ȫ��������Ӧ��yzf��������,
        /// ��ID֮����","�ŷָ�
        /// </summary>
        /// <param name="dataBaseName">���ݿ�ʵ������</param>
        /// <returns></returns>
        public String GetYZFAreaList(String dataBaseName)
        {
            return this.AreaRoleInfo.GetYZFAreaList(dataBaseName);
        }
        /// <summary>
        /// ��ȡ����Ȩ����
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
                throw (new Exception("��ȡȨ����ʧ��:" + e.Message));
            }
        }

        /// <summary>
        /// ��ȡ����Ȩ������û�
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllUsersAndPrivileges()
        {
            DataTable dt = null;
            DbCommand cmd = null;
            Database db = DatabaseFactory.CreateDatabase();
            StringBuilder sb = new StringBuilder();
            sb.Append("select Sys_UserPrivilegeMap.User_ID,User_Name,Sys_PrivilegeGroup.PrivGroup_ID,PrivGroup_Name,MainArea_ID ");//��Ҫ���ֶ�
            sb.Append("from Sys_UserPrivilegeMap left join Sys_User on Sys_UserPrivilegeMap.User_ID=Sys_User.User_ID left join Sys_PrivilegeGroup on Sys_UserPrivilegeMap.PrivGroup_ID=Sys_PrivilegeGroup.PrivGroup_ID ");//��Ҫ�ı�
            sb.Append("where PrivGroup_IsPrivate=1");//��Ҫ������
            cmd = db.GetSqlStringCommand(sb.ToString());
            try
            {
                dt = db.ExecuteDataSet(cmd).Tables[0];
            }
            catch (Exception ex)
            {
                throw (new Exception("��ѯ�û���Ȩ����Ϣ����,ԭ��:" + ex.Message));
            }
            return dt;
        }
    }
}
