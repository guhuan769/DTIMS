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
   /// Resources�ļ�����Դ�б�
   /// </summary>
   public class ResourcesList
   {
      
      /// <summary>
      /// �����Դ�б�����̨
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
