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
    /// 功能描述：地区信息类
    /// </summary>
    [Serializable]
    public class AreaInfo
    {
        private string mMainArea_ID = null;
        private string mFatherMainArea_ID = null;
        private string mMainArea_Name = null;

        #region 公用属性
        /// <summary>
        /// 地区ID
        /// </summary>
        public string MainArea_ID
        {
            get
            {
                return this.mMainArea_ID;
            }
        }

        /// <summary>
        /// 地区父ID
        /// </summary>
        public string FatherMainArea_ID
        {
            get
            {
                return this.mFatherMainArea_ID;
            }
        }

        /// <summary>
        /// 地区名称
        /// </summary>
        public string MainArea_Name
        {
            get
            {
                return this.mMainArea_Name;
            }
        }
        #endregion

        #region 构造函数(获取指定地区ID的信息
        /// <summary>
        /// 功能描述：构造函数(获取指定地区ID的信息)
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
                    throw (new Exception("未找到地区信息！"));
                }
            }
            catch (Exception e)
            {
                throw (new Exception("查询地区信息出错,原因:" + e.Message));
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

        #region 获取所有地区信息
        /// <summary>
        /// 功能描述：获取地区信息
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
                throw (new Exception("查询地区信息时出错，" + e.Message));
            }

            return dt;
        }
        #endregion
    }//end public class AreaInfo
}//end namespace Operator
