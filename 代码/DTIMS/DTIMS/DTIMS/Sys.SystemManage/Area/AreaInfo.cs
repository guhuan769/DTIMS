using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace BJ.Project.Common
{
    /// <summary>
    /// ����������������Ϣ��
    /// </summary>
    [Serializable]
    public class AreaInfo
    {
        private string mMainArea_ID = null;
        private string mFatherMainArea_ID = null;
        private string mMainArea_Name = null;

        #region ��������
        /// <summary>
        /// ����ID
        /// </summary>
        public string MainArea_ID
        {
            get
            {
                return this.mMainArea_ID;
            }
        }

        /// <summary>
        /// ������ID
        /// </summary>
        public string FatherMainArea_ID
        {
            get
            {
                return this.mFatherMainArea_ID;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string MainArea_Name
        {
            get
            {
                return this.mMainArea_Name;
            }
        }
        #endregion

        #region ���캯��(��ȡָ������ID����Ϣ
        /// <summary>
        /// �������������캯��(��ȡָ������ID����Ϣ)
        /// </summary>
        /// <param name="areaID"></param>
        public AreaInfo(string areaID)
        {
            DbCommand cmdSelect = null;

            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase();
            System.Data.IDataReader iReader = null;
            String sql = "SELECT MainArea_ID,FatherMainArea_ID,MainArea_Name,MainArea_Remark FROM S_MainArea WHERE MainArea_ID=@MainArea_ID";

            cmdSelect = mDataBase.GetSqlStringCommand(sql);
            mDataBase.AddInParameter(cmdSelect, "@MainArea_ID", System.Data.DbType.String, areaID);

            try
            {
                iReader = mDataBase.ExecuteReader(cmdSelect);
                if (iReader.Read())
                {
                    this.mMainArea_ID = iReader["MainArea_ID"].ToString().Trim();
                    this.mFatherMainArea_ID = iReader["FatherMainArea_ID"].ToString().Trim();
                    this.mMainArea_Name = iReader["MainArea_Name"].ToString().Trim();
                }
                else
                {
                    throw (new Exception("δ�ҵ�������Ϣ��"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception("��ѯ������Ϣ����,ԭ��:" + e.Message));
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
        #endregion

        #region ��ȡ���е�����Ϣ
        /// <summary>
        /// ������������ȡ������Ϣ
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllAreaInfo()
        {
            DataTable dt = null;
            DbCommand cmdSelect = null;
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = DatabaseFactory.CreateDatabase();
            String sqlstr = "SELECT MainArea_ID,FatherMainArea_ID,MainArea_Name,MainArea_Remark FROM S_MainArea ORDER BY MainArea_ID";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            try
            {
                dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
            }
            catch (Exception e)
            {
                throw (new Exception("��ѯ������Ϣʱ����" + e.Message));
            }

            return dt;
        }
        #endregion
    }//end public class AreaInfo
}//end namespace Operator
