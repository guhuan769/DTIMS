/*
 * @(#)ResourcesList.cs    1.0, Mar 19, 2005
 *
 * @author feng xianwen.
 *
 */

using System;
using System.Resources;
using System.Collections;

namespace BJ.AspxTask 
{

   /// <summary>
   /// Resources文件的资源列表
   /// </summary>
   public class ResourcesList
   {
      
      /// <summary>
      /// 输出资源列表到控制台
      /// </summary>
      /// <param name="filename"></param>
      public static void List(String filename)
      {

         ResourceSet rs = new ResourceSet(filename);
         IDictionaryEnumerator id = rs.GetEnumerator();
         while(id.MoveNext()) 
         {
            Console.WriteLine("\n[{0}] --> {1}", id.Key, id.Value);
         }
         rs.Close();

      }
   }

}
