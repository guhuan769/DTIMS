using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using BJ.DTIMS.Comm.Entity;
using BJ.Project.Common;
using System.Text.RegularExpressions;

namespace BJ.DTIMS.Comm.Data
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
        public static CheckAuthEntity CheckAuthority(string Number, Operator oper, params NumberType[] targetType)
        {
            CheckAuthEntity result = new CheckAuthEntity();
            result.IsHave = false;
            result.Type = NumberType.None;
            result.Descr = "你没有查询该号码的权限!";

            try
            {
                //判断是否是空号码，或者非数字的号码
                if (string.IsNullOrEmpty(Number) || Number.Trim().Length == 0)
                {
                    throw new ArgumentException("号码非法");
                }

                Regex regex;
                Match match;
                //C网号码
                if (targetType.Length == 0 || IsContain(targetType, NumberType.MobileNo))
                {
                    regex = new Regex("^(?:86)?(1(?:89|33|53|80|81)[0-9]{8})$");
                    match = regex.Match(Number);
                    if (match.Success)//C网号码
                    {
                        result.FormatedNumber = "86" + match.Groups[1].Value.Trim();
                        result.Type = NumberType.MobileNo;

                        //验证权限
                        if (IsQueryVerify(result.FormatedNumber, NumberType.MobileNo, oper))
                        {
                            result.IsHave = true;
                            result.Descr = "成功";
                        }
                        else
                        {
                            result.IsHave = false;
                        }
                        return result;
                    }
                }

                //IMSI号码
                if (targetType.Length == 0 || IsContain(targetType, NumberType.IMSI))
                {
                    regex = new Regex("^4600[0-9]{11}$");
                    if (regex.IsMatch(Number))
                    {
                        result.FormatedNumber = Number;
                        result.Type = NumberType.IMSI;

                        //验证权限
                        if (IsQueryVerify(Number, NumberType.IMSI, oper))
                        {
                            result.IsHave = true;
                            result.Descr = "成功";
                        }
                        else
                        {
                            result.IsHave = false;
                        }
                        return result;
                    }
                }

                //固网号码（11位）
                if (targetType.Length == 0 || IsContain(targetType, NumberType.FixedNo))
                {
                    regex = new Regex("^(?:0)?(2|8)([0-9]{9})$");
                    match = regex.Match(Number);
                    if (match.Success)
                    {
                        result.Type = NumberType.FixedNo;

                        //若前面有0，则去掉0后进行判断
                        result.FormatedNumber = match.Value;
                        if (result.FormatedNumber.Substring(0, 1) == "0")
                        {
                            result.FormatedNumber = result.FormatedNumber.Substring(1);
                        }

                        if (oper.Area_ID.Equals("1"))//省级用户
                        {
                            result.IsHave = true;
                            result.Descr = "成功";
                        }
                        else if (oper.Area_ID.Equals("2"))//成都用户（比较区号）
                        {
                            if (result.FormatedNumber.Substring(0, 3) == "282" || result.FormatedNumber.Substring(0, 3) == "283")
                            {
                                //不包括眉山与资阳
                            }
                            else if (BJ.DTIMS.Comm.Config.AreaInfo.Parameter(oper.Area_ID.Trim()) == result.FormatedNumber.Substring(0, 2))
                            {
                                result.IsHave = true;
                                result.Descr = "成功";
                            }
                        }
                        else//非成都（比较区号）
                        {
                            if (BJ.DTIMS.Comm.Config.AreaInfo.Parameter(oper.Area_ID.Trim()) == result.FormatedNumber.Substring(0, 3))
                            {
                                result.IsHave = true;
                                result.Descr = "成功";
                            }
                        }
                        return result;
                    }
                }

                //IMS Tel
                if (targetType.Length == 0 || IsContain(targetType, NumberType.IMS_TEL))
                {
                    regex = new Regex("^(?:86|\\+86)?((?:2|8)(?:([0-9]{9})|([0-9]{12})))$"); 
                    match = regex.Match(Number);
                    if (match.Success)
                    {
                        result.Type = NumberType.IMS_TEL;
                        result.FormatedNumber = "+86" + match.Groups[1].Value.Trim(); //将号码格式化：+862864020033

                        if (oper.Area_ID.Equals("1"))//省级用户
                        {
                            result.IsHave = true;
                            result.Descr = "成功";
                        }
                        else if (oper.Area_ID.Equals("2"))//成都用户（比较区号）
                        {
                            if (BJ.DTIMS.Comm.Config.AreaInfo.Parameter(oper.Area_ID.Trim()) == result.FormatedNumber.Substring(3, 2))
                            {
                                result.IsHave = true;
                                result.Descr = "成功";
                            }
                        }
                        else//非成都（比较区号）
                        {
                            if (BJ.DTIMS.Comm.Config.AreaInfo.Parameter(oper.Area_ID.Trim()) == result.FormatedNumber.Substring(3, 3))
                            {
                                result.IsHave = true;
                                result.Descr = "成功";
                            }
                        }
                        return result;
                    }
                }

                //IMS Sip
                if (targetType.Length == 0 || IsContain(targetType, NumberType.IMS_SIP))
                {
                    regex = new Regex(BJ.DTIMS.Common.Configure.Parameter("ImsSipRegex"));
                    match = regex.Match(Number);
                    if (match.Success)
                    {
                        result.Type = NumberType.IMS_SIP;
                        result.FormatedNumber = "+86" + match.Groups[1].Value.Trim(); //将号码格式化：+862864020033@sc.ctcims.cn

                        if (oper.Area_ID.Equals("1"))//省级用户
                        {
                            result.IsHave = true;
                            result.Descr = "成功";
                        }
                        else if (oper.Area_ID.Equals("2"))//成都用户（比较区号）
                        {
                            if (BJ.DTIMS.Comm.Config.AreaInfo.Parameter(oper.Area_ID.Trim()) == result.FormatedNumber.Substring(3, 2))
                            {
                                result.IsHave = true;
                                result.Descr = "成功";
                            }
                        }
                        else//非成都（比较区号）
                        {
                            if (BJ.DTIMS.Comm.Config.AreaInfo.Parameter(oper.Area_ID.Trim()) == result.FormatedNumber.Substring(3, 3))
                            {
                                result.IsHave = true;
                                result.Descr = "成功";
                            }
                        }
                        return result;
                    }
                }

                throw new ArgumentException("号码非法");
            }
            catch (Exception ex)
            {
                result.FormatedNumber = Number;
                result.Descr = ex.Message;
            }
            return result;
        }

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
}//end namespace Inphase.CTQS.Comm.Data
