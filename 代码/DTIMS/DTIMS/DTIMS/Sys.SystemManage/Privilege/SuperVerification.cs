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
	/// �����û�Ȩ����֤�������������TRUE���������������Ȩ��
	/// </summary>
    [Serializable]
    public class SuperVerification : IRoleVerify
	{
		private String operId = null;

		public SuperVerification(String id)
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
			this.operId = id.Trim();
		}

		#region IRoleVerify ��Ա
		/// <summary>
		/// �����û���������֤�κι��������TRUE
		/// </summary>
		/// <param name="function"></param>
		/// <returns></returns>
		public bool Verify(String function)
		{
			// TODO:  ��� SuperVerification.Verify ʵ��
			return true;
		}

		/// <summary>
		/// �õ���ǰ�û���Ȩ�޵�ȫ�����ܷ���
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
				throw(new Exception("��ѯ��Ȩ�޵Ĺ���������ԭ��" + e.Message));
			}

			return al;
		}
		#endregion
	}
}
