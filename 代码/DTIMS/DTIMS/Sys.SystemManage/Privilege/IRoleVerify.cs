using System;
using System.Text;
using System.Collections;

namespace Sys.Project.Common
{
	/// <summary>
	/// �ж�Ȩ�޵Ľӿ�
	/// </summary>
   public interface IRoleVerify
   {
      /// <summary>
      /// �����Ƿ���Ȩ��
      /// </summary>
      /// <param name="function">������ID</param>
      /// <returns></returns>
      bool Verify(String function);

      /// <summary>
      /// �õ���ǰ�û�������Ȩ�޵ķ����б�
      /// </summary>
      /// <returns></returns>
      ArrayList GetRoleCategory();
   }
}
