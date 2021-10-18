using System;

namespace Sys.Project.Common
{
	/// <summary>
	/// AspxTask 应用程序入口类
	/// </summary>
	public class MainClass
	{

      /// <summary>
      /// 应用程序入口
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
         Console.WriteLine("-a<ApplicationPath>             应用程序虚拟目录，必要");
         Console.WriteLine("-p<PhysicalApplicationPath>     应用程序绝对物理路径，必要");
         Console.WriteLine("-c                              编译aspx，拷贝dll到bin目录");
         Console.WriteLine("-clear                          清除bin目录下aspx对应的dll");
         Console.WriteLine("-clearTmp                       清除应用程序临时文件");
         Console.WriteLine("-l                              在控制台列出资源文件中的资源");

         Console.WriteLine();

      }

	}
}
