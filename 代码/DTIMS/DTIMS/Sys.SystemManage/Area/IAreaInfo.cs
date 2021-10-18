using System;
using System.Text;
using System.Data;
using System.Collections;

namespace Sys.Project.Common
{
	/// <summary>
	/// 由人员类得到相关地区信息的接口
	/// </summary>
   public interface IAreaInfo
   {
      /// <summary>
      /// 返回有直接权限的地区列表
      /// </summary>
      /// <returns>ArrayList,由地区ID组成的集合</returns>
      DataTable GetDirectRoleArea(String dataBaseInstance);

      /// <summary>
      /// 返回人员有权限的全部地区列表，主要是用作地区管理和人员管理
      /// </summary>
      /// <returns>ArrayList,由地区ID组成的集合</returns>
      DataTable GetAllRoleArea(String dataBaseInstance);

      /// <summary>
      /// 返回指定地区下的全部子节点地区
      /// </summary>
      /// <param name="dataBaseInstance">数据库实例名称</param>
      /// <param name="areaId">地区ID</param>
      /// <returns></returns>
      DataTable GetAllChildArea(String dataBaseInstance, String areaId);

      /// <summary>
      /// 添加一个接口，得到全部的有权限的地市州地区信息哈
      /// </summary>
      /// <returns></returns>
      DataTable GetMainAreaInfo();

      /// <summary>
      /// 得到指定地区下的全部YZF地区集合，
      /// 是以“，”号分隔
      /// </summary>
      /// <param name="dataBaseName">数据库实例名称</param>
      /// <param name="areaId">地区ID</param>
      /// <returns></returns>
      String GetYZFAreaList(String dataBaseName, String areaId);

      /// <summary>
      /// 得到指定地区下的全部YZF地区集合，
      /// 是以“，”号分隔
      /// </summary>
      /// <param name="dataBaseName">数据库实例名称</param>
      /// <returns></returns>
      String GetYZFAreaList(String dataBaseName);
   }
}
