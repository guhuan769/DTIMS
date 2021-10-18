using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Sys.Project.Common;
using System.Text.RegularExpressions;
using DTIMS.Comm.Entity;

namespace DTIMS.Comm.Data
{
    /// <summary>
    /// 数据操作类的公用
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// 调用分页存储过程查询数据
        /// </summary>
        /// <param name="para">参数对象</param>
        /// <returns></returns>
        public static ProcResultEntity GetPagingProcData(ProcParaEntity para)
        {
            return GetPagingProcData(para, null);
        }

        /// <summary>
        /// 调用分页存储过程查询数据
        /// </summary>
        /// <param name="para">参数对象</param>
        /// <param name="connName">Web.config文件中 connectionStrings 配置节中的连接名称。</param>
        /// <returns></returns>
        public static ProcResultEntity GetPagingProcData(ProcParaEntity para, string connName)
        {
            DataTable data = null;
            ProcResultEntity resutl = new ProcResultEntity();
            try
            {
                Database db = string.IsNullOrEmpty(connName) ? DatabaseFactory.CreateDatabase() : DatabaseFactory.CreateDatabase(connName);
                DbCommand dbcomm = db.GetStoredProcCommand("proc_GetRecordFromPage");
                db.AddInParameter(dbcomm, "@tblName", System.Data.DbType.String, para.TableName);
                db.AddInParameter(dbcomm, "@primarykey", System.Data.DbType.String, para.PrimaryKey);
                db.AddInParameter(dbcomm, "@PageSize", System.Data.DbType.Int32, para.PageSize);
                db.AddInParameter(dbcomm, "@PageIndex", System.Data.DbType.Int32, para.PageIndex);
                db.AddInParameter(dbcomm, "@fileds", System.Data.DbType.String, para.Fileds);
                db.AddInParameter(dbcomm, "@strWhere", System.Data.DbType.String, para.Where);
                db.AddInParameter(dbcomm, "@order", System.Data.DbType.String, para.Order);
                db.AddOutParameter(dbcomm, "@dataCount", System.Data.DbType.Int32, 100);
                db.AddOutParameter(dbcomm, "@curPageIndex", System.Data.DbType.Int32, 100);

                //执行数据查询
                data = db.ExecuteDataSet(dbcomm).Tables[0];

                //获取返回的数据
                object outParam_dataCount = db.GetParameterValue(dbcomm, "dataCount");
                object outParam_curPageIndex = db.GetParameterValue(dbcomm, "curPageIndex");

                //获取总数据条数
                if (outParam_dataCount != null)
                {
                    resutl.TotalRows = Convert.ToInt32(outParam_dataCount.ToString());
                }
                else
                {
                    resutl.TotalRows = 0;
                }

                if (outParam_curPageIndex != null)
                {
                    resutl.CurPageIndex = Convert.ToInt32(outParam_curPageIndex.ToString());
                }
                else
                {
                    resutl.CurPageIndex = 0;
                }

                resutl.dataTable = data;
                resutl.Success = true;
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.Descr = ex.Message;
            }

            return resutl;
        }

        #region 分权分域判断
 
 
        private static bool IsQueryVerify(string Number, NumberType type, Operator oper)
        {
            Database mDataBase = DatabaseFactory.CreateDatabase();
            DbCommand cmdSelect;
            string sql = "";

            if (type == NumberType.MobileNo)
            {
                sql = @"SELECT COUNT(1) FROM TelNumAreaSegment WHERE TNAS_StartNum <= @Number AND TNAS_EndNum >= @Number";
                if (oper.Area_ID.Trim() != "1")//非省级用户需要加地区ID
                {
                    sql += " AND MainArea_ID = @MainArea_ID ";
                }
            }
            else
            {
                sql = @"SELECT  COUNT(1) FROM IMSINumAreaSegment WHERE INAS_StartIMSINum <= @Number AND INAS_EndIMSINum >= @Number";
                if (oper.Area_ID.Trim() != "1")//非省级用户需要加地区ID
                {
                    sql += " AND MainArea_ID = @MainArea_ID ";
                }
            }

            cmdSelect = mDataBase.GetSqlStringCommand(sql);

            if (oper.Area_ID.Trim() != "1")
            {
                mDataBase.AddInParameter(cmdSelect, "@MainArea_ID", DbType.String, oper.Area_ID);
            }

            if (type == NumberType.MobileNo)
            {
                mDataBase.AddInParameter(cmdSelect, "@Number", DbType.String, Number.Substring(2));
            }
            else
            {
                mDataBase.AddInParameter(cmdSelect, "@Number", DbType.String, Number);
            }

            if (Convert.ToInt16(mDataBase.ExecuteScalar(cmdSelect)) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsContain(NumberType[] targetType, NumberType numberType)
        {
            foreach (NumberType item in targetType)
            {
                if (item == numberType)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion 分权分域判断
    }//end public class Helper
}//end namespace Inphase.JFIMS.Comm.Data
