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
    /// SysLog 的摘要说明。
    /// </summary>
    public class SysLog
    {
        #region 属性
        private string mLogId;
        private string mOprId;
        private string mOprLogin;
        private System.DateTime mDateTime;
        private string mMode;
        //private string mItemId;   保留，暂未使用。
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
                        throw (new Exception("日志内容不能包含特殊字符！"));
                    }
                    else
                    {
                        byte[] bytes = System.Text.Encoding.Default.GetBytes(value);
                        if (bytes.Length > 520)
                        {
                            throw (new Exception("日志内容最长不能超过520个字符！"));
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

        #region 构造函数
        public SysLog()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public SysLog(string id)
        {
            Database mDataBase = DatabaseFactory.CreateDatabase(BJ.SystemWebFormulation.SystemWebFormulation.Parameter("MainDataBaseName"));

            #region 查询设备基本信息（日志ID,人员ID,人员登录名,操作时间,操作方式,日志内容,记录项目）
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
                throw (new Exception("系统找不到指定的日志！"));
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
        /// 公用枚举
        /// </summary>
        public enum LogItemID
        {
            /// <summary>
            /// 操作员管理
            /// </summary>
            OperatorManage = '1',

            /// <summary>
            /// 权限管理
            /// </summary>
            RightManage = '2',

            /// <summary>
            /// 地区管理
            /// </summary>
            AreaManage = '3',

            /// <summary>
            /// 工单管理
            /// </summary>
            BillManage = '4',

            /// <summary>
            /// 资源管理
            /// </summary>
            ResourceManage = '5',

            /// <summary>
            /// 系统日志管理
            /// </summary>
            SyslogManage = '6',

            /// <summary>
            /// 故障综合管理
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

            //判断是否需要按帐号查询
            if (User_Login.Length > 0)
            {
                where = " O.User_Login='" + User_Login + "' AND " + where;
            }

            DataTable dt = null;
            try
            {
                string field = "F.LOG_ID,F.User_ID,convert(varchar(25),F.LOG_DATETIME,120) AS LOG_DATETIME,(CASE F.LOG_Mode WHEN 'U' THEN '修改' WHEN 'A' THEN '添加' WHEN 'D' THEN '删除' END) AS LOG_Mode,F.LOG_CONTENT,I.LItem_Name,O.User_Login ";
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


        #region 添加
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

        #region 删除
        /// <summary>
        /// 删除一条日志
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
                throw (new Exception("删除该条日志出错，原因：" + e.Message));
            }
        }
        /// <summary>
        /// 删除所有日志
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
                throw (new Exception("删除日志出错，原因：" + e.Message));
            }
        }
        #endregion

        #region 获取操作类型中文名称
        public static string GetLogModeName(string str)
        {
            string strName = null;
            switch (str)
            {
                case "A":
                    strName = "添加";
                    break;
                case "D":
                    strName = "删除";
                    break;
                case "U":
                    strName = "修改";
                    break;
                default:
                    strName = "未知类型";
                    break;
            }
            return strName;
        }
        #endregion
    }
}
