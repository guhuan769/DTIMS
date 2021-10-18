using System;
using System.Text.RegularExpressions ;

namespace Sys.Comm.Secuirty
{
   public class CheckString
   {
      public CheckString()
      {
         //
         // TODO: 在此处添加构造函数逻辑
         //
      }   

      /// <summary>
      /// 正则表达式匹配
      /// </summary>
      /// <param name="regex">正则表达式</param>
      /// <param name="strValue">输入的字符串</param>
      /// <returns>匹配结果</returns>
      public static System.Text .RegularExpressions .Match ExecMatch(string regex,string strValue)
      {
         return Regex.Match(strValue,regex);
      }

      /// <summary>
      /// 验证输入的IP地址是否正确
      /// </summary>
      /// <param name="ip">IP字符串</param>
      /// <returns>验证结果</returns>
      public static bool IsIP(string ip)
      {
         return ExecMatch(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$",
            ip).Success;
      }
      
      /// <summary>
      /// 验证输入的数字
      /// </summary>
      /// <param name="Number">输入的数字</param>
      /// <returns>验证结果</returns>
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
      /// 验证对像是否为空
      /// </summary>
      /// <param name="obj">输入的对像</param>
      /// <returns>验证结果</returns>
      public static bool IsNull(object obj)
      {
         if(obj==null)
         {
            return true;
         }
         return false;
      }

      /// <summary>
      /// 验证空格
      /// </summary>
      /// <param name="chekString">输入的字符串</param>
      /// <returns>返回参数</returns>
      public static bool IsSpace(string chekString)
      {
         if(chekString.Trim()=="")
         {
            return true;
         }
         return false;
      }
      /// <summary>
      /// 特殊字符列表 {'<','>','=',';','\'','\"','&','*','%'}
      /// </summary>
      /// <param name="checkString">输入的字符串</param>
      /// <returns>返回验证结果</returns>
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
      /// 验证是否含有指定字符集
      /// </summary>
      /// <param name="checkString">输入的字符串</param>
      /// <param name="special">特殊字符集</param>
      /// <returns>返回结果</returns>
      public static bool HaveSpecialChar(string checkString,char[] special)
      {
         int index = checkString.IndexOfAny(special);
         if(index!=-1)
         {
            return true;
         }
         return false;     
      }
      
      #region 日期与时间的验证
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
      /// 验证日期时间控件
      /// </summary>
      /// <param name="dateTime">输入的日期时间</param>
      /// <returns>验证结果</returns>
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
