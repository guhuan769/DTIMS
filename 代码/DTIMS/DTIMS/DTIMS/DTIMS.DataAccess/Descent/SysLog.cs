using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;
using BJ.WebTools;


namespace BJ.SysLog.Descent
{
    /// <summary>
    /// SysLog ��ժҪ˵����
    /// </summary>
    public class SysLog
    {
        #region ����
        private string mLogId;
        private string mOprId;
        private string mOprLogin;
        private System.DateTime mDateTime;
        private string mMode;
        //private string mItemId;   ��������δʹ�á�
        private string mItemName;
        private string mContent;
        private string mLOG_Client_IP;
        //private Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase = null;

        public string LogID
        {
            get
            {
                return this.mLogId;
            }
        }
        //public string ItemID
        //{
        //    get
        //    {
        //        return this.mItemId;
        //    }
        //}
        public string ItemName
        {
            get
            {
                return this.mItemName;
            }
        }
        public string Mode
        {
            get
            {
                return this.mMode;
            }
        }
        public DateTime Date
        {
            get
            {
                return this.mDateTime;
            }
        }
        public string OprID
        {
            get
            {
                return this.mOprId;
            }
        }
        public string OprLogin
        {
            get
            {
                return this.mOprLogin;
            }
        }
        public string Content
        {
            get
            {
                return this.mContent;
            }
            set
            {
                if (value == null || value == "")
                {
                    this.mContent = null;
                }
                else
                {
                    if (BJ.Secuirty.CheckString.HaveSpecialChar(value))
                    {
                        throw (new Exception("��־���ݲ��ܰ��������ַ���"));
                    }
                    else
                    {
                        byte[] bytes = System.Text.Encoding.Default.GetBytes(value);
                        if (bytes.Length > 520)
                        {
                            throw (new Exception("��־��������ܳ���520���ַ���"));
                        }
                        else
                        {
                            this.mContent = value.Trim();
                        }
                    }
                }
            }
        }

        public string LOG_Client_IP
        {
            get
            {
                return this.mLOG_Client_IP;
            }
        }

        #endregion

        #region ���캯��
        public SysLog()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }

        public SysLog(string id)
        {
            Database mDataBase = DatabaseFactory.CreateDatabase(BJ.SystemWebFormulation.SystemWebFormulation.Parameter("MainDataBaseName"));

            #region ��ѯ�豸������Ϣ����־ID,��ԱID,��Ա��¼��,����ʱ��,������ʽ,��־����,��¼��Ŀ��
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT F.LOG_ID,F.User_ID,F.LOG_DATETIME,F.LOG_Mode,F.LOG_CONTENT,I.LItem_Name,O.User_Login,F.LOG_Client_IP FROM S_LogFile AS F ");
            sb.Append("LEFT JOIN S_LogItem AS I ON F.LItem_ID=I.LItem_ID ");
            sb.Append("LEFT JOIN Sys_User AS O ON F.User_ID=O.User_ID ");
            sb.Append("WHERE F.LOG_ID=@id ORDER BY F.LOG_ID");

            DbCommand cmdSelect = mDataBase.GetSqlStringCommand(sb.ToString());
            mDataBase.AddInParameter(cmdSelect, "@id", System.Data.DbType.String, id.Trim());
            DataTable dt = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];
            if (dt.Rows.Count == 0)
            {
                throw (new Exception("ϵͳ�Ҳ���ָ������־��"));
            }
            this.mLogId = dt.Rows[0]["LOG_ID"].ToString().Trim();
            this.mOprId = dt.Rows[0]["User_ID"].ToString().Trim();
            this.mOprLogin = dt.Rows[0]["User_Login"].ToString().Trim();
            this.mDateTime = (System.DateTime)dt.Rows[0]["LOG_DATETIME"];
            this.mMode = dt.Rows[0]["LOG_Mode"].ToString().Trim();
            this.mItemName = dt.Rows[0]["LItem_Name"].ToString().Trim();
            this.mContent = dt.Rows[0]["LOG_CONTENT"].ToString().Trim();
            this.mLOG_Client_IP = dt.Rows[0]["LOG_Client_IP"].ToString().Trim();
            #endregion
        }
        #endregion

        /// <summary>
        /// ����ö��
        /// </summary>
        public enum LogItemID
        {
            /// <summary>
            /// ����Ա����
            /// </summary>
            OperatorManage = '1',

            /// <summary>
            /// Ȩ�޹���
            /// </summary>
            RightManage = '2',

            /// <summary>
            /// ��������
            /// </summary>
            AreaManage = '3',

            /// <summary>
            /// ��������
            /// </summary>
            BillManage = '4',

            /// <summary>
            /// ��Դ����
            /// </summary>
            ResourceManage = '5',

            /// <summary>
            /// ϵͳ��־����
            /// </summary>
            SyslogManage = '6',

            /// <summary>
            /// �����ۺϹ���
            /// </summary>
            TroubleManage = '7',
        }


        public enum LogMode
        {
            UpDate = 'U',
            Insert = 'A',
            Delete = 'D'
        }

        public static DataTable GetDataTable(int pageSize, int page, string orderby, string startTime, string endTime, string User_Login, out int row_data, out int cur_page_index)
        {
            string where = "";
            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                where = string.Format("LOG_DATETIME between '{0} 00:00:00' and '{1} 23:59:59'", startTime, endTime);
            }

            //�ж��Ƿ���Ҫ���ʺŲ�ѯ
            if (User_Login.Length > 0)
            {
                where = " O.User_Login='" + User_Login + "' AND " + where;
            }

            DataTable dt = null;
            try
            {
                string field = "F.LOG_ID,F.User_ID,convert(varchar(25),F.LOG_DATETIME,120) AS LOG_DATETIME,(CASE F.LOG_Mode WHEN 'U' THEN '�޸�' WHEN 'A' THEN '���' WHEN 'D' THEN 'ɾ��' END) AS LOG_Mode,F.LOG_CONTENT,I.LItem_Name,O.User_Login ";
                string tableName = "S_LogFile AS F LEFT JOIN S_LogItem AS I ON F.LItem_ID=I.LItem_ID LEFT JOIN Sys_User AS O ON F.User_ID=O.User_ID";
                if (string.IsNullOrEmpty(orderby))
                {
                    orderby = " F.LOG_ID DESC ";
                }
                Inphase.Project.CTQS.Common com = new Inphase.Project.CTQS.Common();
                dt = com.Query(pageSize, page, tableName, "F.LOG_ID", field, where, orderby, out row_data, out cur_page_index);
            }
            catch (Exception e)
            {
                throw (new Exception(e.Message));
            }
            return dt;
        }


        #region ���
        public static void Add(string operId, LogMode mode, LogItemID itemId, string content, string client_IP)
        {
            StringBuilder sb = new StringBuilder();
            DbConnection connection;
            DbTransaction trans;
            Database mDataBase = DatabaseFactory.CreateDatabase(BJ.SystemWebFormulation.SystemWebFormulation.Parameter("MainDataBaseName"));
            connection = mDataBase.CreateConnection();
            try
            {
                connection.Open();
                trans = connection.BeginTransaction();
                try
                {
                    sb.Append("INSERT INTO S_LogFile (User_ID,LOG_DATETIME,LOG_Mode,LOG_CONTENT,LItem_ID,LOG_Client_IP) VALUES(");
                    sb.Append("@operid,getdate(),@mode,@content,@itemid,@Client_IP)");
                    if (content == null)
                    {
                        sb.Replace("@content", "NULL");
                    }
                    DbCommand cmdSelect = mDataBase.GetSqlStringCommand(sb.ToString());
                    if (content != null)
                    {
                        mDataBase.AddInParameter(cmdSelect, "@content", System.Data.DbType.String, content.Trim());
                    }
                    mDataBase.AddInParameter(cmdSelect, "@operid", System.Data.DbType.String, operId.Trim());
                    mDataBase.AddInParameter(cmdSelect, "@mode", System.Data.DbType.String, ((char)mode).ToString());
                    mDataBase.AddInParameter(cmdSelect, "@itemid", System.Data.DbType.String, ((char)itemId).ToString());
                    mDataBase.AddInParameter(cmdSelect, "@Client_IP", System.Data.DbType.String, client_IP);
                    mDataBase.ExecuteDataSet(cmdSelect, trans);

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
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close(); connection.Dispose();
                }
            }
        }
        #endregion

        #region ɾ��
        /// <summary>
        /// ɾ��һ����־
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteItem(string id)
        {
            DbConnection connection;
            DbTransaction trans;
            Database mDataBase = DatabaseFactory.CreateDatabase(BJ.SystemWebFormulation.SystemWebFormulation.Parameter("MainDataBaseName"));
            connection = mDataBase.CreateConnection();
            connection.Open();
            trans = connection.BeginTransaction();

            string sqlstr = "delete S_LogFile where LOG_ID=@id";

            try
            {
                DbCommand cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
                mDataBase.AddInParameter(cmdSelect, "@id", System.Data.DbType.String, id.Trim());
                mDataBase.ExecuteDataSet(cmdSelect);

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw (new Exception("ɾ��������־����ԭ��" + e.Message));
            }
        }
        /// <summary>
        /// ɾ��������־
        /// </summary>		
        public static void Delete()
        {
            DbConnection connection;
            DbTransaction trans;
            Database mDataBase = DatabaseFactory.CreateDatabase(BJ.SystemWebFormulation.SystemWebFormulation.Parameter("MainDataBaseName"));
            connection = mDataBase.CreateConnection();
            connection.Open();
            trans = connection.BeginTransaction();

            string sqlstr = "delete S_LogFile";

            try
            {
                DbCommand cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
                mDataBase.ExecuteDataSet(cmdSelect);

                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw (new Exception("ɾ����־����ԭ��" + e.Message));
            }
        }
        #endregion

        #region ��ȡ����������������
        public static string GetLogModeName(string str)
        {
            string strName = null;
            switch (str)
            {
                case "A":
                    strName = "���";
                    break;
                case "D":
                    strName = "ɾ��";
                    break;
                case "U":
                    strName = "�޸�";
                    break;
                default:
                    strName = "δ֪����";
                    break;
            }
            return strName;
        }
        #endregion
    }
}
