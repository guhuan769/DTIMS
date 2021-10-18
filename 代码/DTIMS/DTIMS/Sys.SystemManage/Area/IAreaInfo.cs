using System;
using System.Text;
using System.Data;
using System.Collections;

namespace Sys.Project.Common
{
	/// <summary>
	/// ����Ա��õ���ص�����Ϣ�Ľӿ�
	/// </summary>
   public interface IAreaInfo
   {
      /// <summary>
      /// ������ֱ��Ȩ�޵ĵ����б�
      /// </summary>
      /// <returns>ArrayList,�ɵ���ID��ɵļ���</returns>
      DataTable GetDirectRoleArea(String dataBaseInstance);

      /// <summary>
      /// ������Ա��Ȩ�޵�ȫ�������б���Ҫ�����������������Ա����
      /// </summary>
      /// <returns>ArrayList,�ɵ���ID��ɵļ���</returns>
      DataTable GetAllRoleArea(String dataBaseInstance);

      /// <summary>
      /// ����ָ�������µ�ȫ���ӽڵ����
      /// </summary>
      /// <param name="dataBaseInstance">���ݿ�ʵ������</param>
      /// <param name="areaId">����ID</param>
      /// <returns></returns>
      DataTable GetAllChildArea(String dataBaseInstance, String areaId);

      /// <summary>
      /// ���һ���ӿڣ��õ�ȫ������Ȩ�޵ĵ����ݵ�����Ϣ��
      /// </summary>
      /// <returns></returns>
      DataTable GetMainAreaInfo();

      /// <summary>
      /// �õ�ָ�������µ�ȫ��YZF�������ϣ�
      /// ���ԡ������ŷָ�
      /// </summary>
      /// <param name="dataBaseName">���ݿ�ʵ������</param>
      /// <param name="areaId">����ID</param>
      /// <returns></returns>
      String GetYZFAreaList(String dataBaseName, String areaId);

      /// <summary>
      /// �õ�ָ�������µ�ȫ��YZF�������ϣ�
      /// ���ԡ������ŷָ�
      /// </summary>
      /// <param name="dataBaseName">���ݿ�ʵ������</param>
      /// <returns></returns>
      String GetYZFAreaList(String dataBaseName);
   }
}
