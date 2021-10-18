using System;
using System.Text;
using System.Collections;

namespace Sys.Project.Common
{
	/// <summary>
	/// 判断权限的接口
	/// </summary>
   public interface IRoleVerify
   {
      /// <summary>
      /// 返回是否有权限
      /// </summary>
      /// <param name="function">功能项ID</param>
      /// <returns></returns>
      bool Verify(String function);

      /// <summary>
      /// 得到当前用户所有有权限的分类列表
      /// </summary>
      /// <returns></returns>
      ArrayList GetRoleCategory();
   }
}
