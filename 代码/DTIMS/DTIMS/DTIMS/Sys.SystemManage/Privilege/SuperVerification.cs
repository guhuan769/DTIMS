using System;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace BJ.Project.Common
{
	/// <summary>
	/// 超级用户权限验证，任务功能项都返回TRUE，即有任务功能项的权限
	/// </summary>
    [Serializable]
    public class SuperVerification : IRoleVerify
	{
		private String operId = null;

		public SuperVerification(String id)
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
			this.operId = id.Trim();
		}

		#region IRoleVerify 成员
		/// <summary>
		/// 超级用户，无论验证任何功能项都返回TRUE
		/// </summary>
		/// <param name="function"></param>
		/// <returns></returns>
		public bool Verify(String function)
		{
			// TODO:  添加 SuperVerification.Verify 实现
			return true;
		}

		/// <summary>
		/// 得到当前用户有权限的全部功能分类
		/// </summary>
		/// <returns></returns>
		public ArrayList GetRoleCategory()
		{
			ArrayList al = new ArrayList();
			DbCommand cmdSelect;
            Database dataBase = DatabaseFactory.CreateDatabase(SystemWebFormulation.SystemWebFormulation.Parameter("MainDataBaseName"));

         String sqlstr = "select FUNCN_ID,Funcn_Name from S_FunctionCategory";
			cmdSelect = dataBase.GetSqlStringCommand (sqlstr);

			try
			{
				DataTable dt = dataBase.ExecuteDataSet(cmdSelect).Tables[0];
				foreach(DataRow dr in dt.Rows)
				{
					al.Add(dr["FUNCN_ID"].ToString().Trim());
				}
			}
			catch(Exception e)
			{
				throw(new Exception("查询有权限的功能类别出错，原因：" + e.Message));
			}

			return al;
		}
		#endregion
	}
}
