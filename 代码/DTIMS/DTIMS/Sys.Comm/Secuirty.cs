using System;
using System.Text.RegularExpressions ;

namespace Sys.Comm.Secuirty
{
   public class CheckString
   {
      public CheckString()
      {
         //
         // TODO: �ڴ˴���ӹ��캯���߼�
         //
      }   

      /// <summary>
      /// ������ʽƥ��
      /// </summary>
      /// <param name="regex">������ʽ</param>
      /// <param name="strValue">������ַ���</param>
      /// <returns>ƥ����</returns>
      public static System.Text .RegularExpressions .Match ExecMatch(string regex,string strValue)
      {
         return Regex.Match(strValue,regex);
      }

      /// <summary>
      /// ��֤�����IP��ַ�Ƿ���ȷ
      /// </summary>
      /// <param name="ip">IP�ַ���</param>
      /// <returns>��֤���</returns>
      public static bool IsIP(string ip)
      {
         return ExecMatch(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$",
            ip).Success;
      }
      
      /// <summary>
      /// ��֤���������
      /// </summary>
      /// <param name="Number">���������</param>
      /// <returns>��֤���</returns>
      public static bool IsNumeric(string Number)
      {
          if (Number == null || Number.Trim() == "" || Number == string.Empty)
          {
              return false;
          }

         if(Number=="0")
         {
            return true;
         }
         else
         {
            return ExecMatch(@"^((\d?)|(([-+]?\d+\.?\d*)|([-+]?\d*\.?\d+))|(([-+]?\d+\.?\d*\,\ ?)*([-+]?\d+\.?\d*))|(([-+]?\d*\.?\d+\,\ ?)*([-+]?\d*\.?\d+))|(([-+]?\d+\.?\d*\,\ ?)*([-+]?\d*\.?\d+))|(([-+]?\d*\.?\d+\,\ ?)*([-+]?\d+\.?\d*)))$",Number).Success ;
         }
      }

      /// <summary>
      /// ��֤�����Ƿ�Ϊ��
      /// </summary>
      /// <param name="obj">����Ķ���</param>
      /// <returns>��֤���</returns>
      public static bool IsNull(object obj)
      {
         if(obj==null)
         {
            return true;
         }
         return false;
      }

      /// <summary>
      /// ��֤�ո�
      /// </summary>
      /// <param name="chekString">������ַ���</param>
      /// <returns>���ز���</returns>
      public static bool IsSpace(string chekString)
      {
         if(chekString.Trim()=="")
         {
            return true;
         }
         return false;
      }
      /// <summary>
      /// �����ַ��б� {'<','>','=',';','\'','\"','&','*','%'}
      /// </summary>
      /// <param name="checkString">������ַ���</param>
      /// <returns>������֤���</returns>
      public static bool HaveSpecialChar(string checkString)
      {
         int index = checkString.IndexOfAny(new char[]{'<','>','=',';','\'','\"','&','*','%'});
         if(index!=-1)
         {
            return true;
         }
         return false;                                                                                              
      }

      /// <summary>
      /// ��֤�Ƿ���ָ���ַ���
      /// </summary>
      /// <param name="checkString">������ַ���</param>
      /// <param name="special">�����ַ���</param>
      /// <returns>���ؽ��</returns>
      public static bool HaveSpecialChar(string checkString,char[] special)
      {
         int index = checkString.IndexOfAny(special);
         if(index!=-1)
         {
            return true;
         }
         return false;     
      }
      
      #region ������ʱ�����֤
      /// <summary>
      /// 
      /// </summary>
      /// <param name="dateTime"></param>
      /// <returns></returns>
      public static bool IsDate(string dateTime)
      {
         return true;
      }
      public static bool IsTime(string dateTime)
      {
         return true;
      }

      /// <summary>
      /// ��֤����ʱ��ؼ�
      /// </summary>
      /// <param name="dateTime">���������ʱ��</param>
      /// <returns>��֤���</returns>
      public static bool IsDateTime(string dateTime)
      {
         return ExecMatch(@"^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?"+
            @"((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))"+
            @"[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}"+
            @"(([02468][1235679])|([13579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])"+
            @"|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?"+
            @"((0?[1-9])|(1[0-9])|(2[0-8]))))))(\s(((0?[1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(\:([0-5][0-9])\s))"+
            @"([AM|PM|am|pm]{2,2})))?$",dateTime).Success;

      }
      #endregion

   }

}
