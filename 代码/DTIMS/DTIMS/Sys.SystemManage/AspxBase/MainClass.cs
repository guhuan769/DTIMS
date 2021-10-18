using System;

namespace Sys.Project.Common
{
	/// <summary>
	/// AspxTask Ӧ�ó��������
	/// </summary>
	public class MainClass
	{

      /// <summary>
      /// Ӧ�ó������
      /// </summary>
      /// <param name="args"></param>
      public static void Main(String[] args) 
      {
         Console.WriteLine();

         if (args.Length < 2) 
         {
            Console.WriteLine("Usage: AspxTask.exe <ApplicationPath> <PhysicalApplicationPath> [option]");
				Console.Read();
            return;
         }
         AspxCompile ar = new AspxCompile(args[0], args[1]);
         
         if (args.Length == 2) 
         {
            ar.start();
            return;
         }

         if (args[2].Equals("clear")) 
         {
            ar.ClearRes();
         } 
         else if (args[2].Equals("clearTmp")) 
         {
            ar.ClearTmp();
         }

      }

      public static void Help() 
      {
         Console.WriteLine();

         Console.WriteLine("Usage: AspxTask.exe [option] [resourcefilename]");

         Console.WriteLine();

         Console.WriteLine("Options:");
         Console.WriteLine("-a<ApplicationPath>             Ӧ�ó�������Ŀ¼����Ҫ");
         Console.WriteLine("-p<PhysicalApplicationPath>     Ӧ�ó����������·������Ҫ");
         Console.WriteLine("-c                              ����aspx������dll��binĿ¼");
         Console.WriteLine("-clear                          ���binĿ¼��aspx��Ӧ��dll");
         Console.WriteLine("-clearTmp                       ���Ӧ�ó�����ʱ�ļ�");
         Console.WriteLine("-l                              �ڿ���̨�г���Դ�ļ��е���Դ");

         Console.WriteLine();

      }

	}
}
