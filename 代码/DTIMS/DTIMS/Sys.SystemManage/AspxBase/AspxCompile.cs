
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Threading;
using Microsoft.Win32;
using System.Resources;

namespace Sys.Project.Common
{
	/// <summary>
	/// 编译所有aspx文件并且拷贝dll到bin目录，产生文件名对应文件
	/// </summary>
	public class AspxCompile
	{

      /// <summary>
      /// 应用程序虚拟目录
      /// </summary>
      private String appPath = "";

      /// <summary>
      /// 应用程序根目录
      /// </summary>
      private String root = "";

      private String resfile = "";

      private IResourceWriter writer = null;

      /// <summary>
      /// 所有aspx相对Application的URL列表
      /// </summary>
      private ArrayList allfiles = new ArrayList();

      public AspxCompile(String appPath, String root) 
      {
         this.appPath = appPath;
         this.root = root;
         resfile = root + @"bin\aspx.resources";
         Console.WriteLine("Application Path: " + appPath);
         Console.WriteLine("Physical Application Path: " + root);
         Console.WriteLine("Resources File: " + resfile + "\n");
      }

		public void start()
		{

         // 编译aspx文件
         CompileFiles(root);
         Console.WriteLine("Compile complete.");
         Console.WriteLine();

         // 等2秒
         Thread.Sleep(2000);

         // 输出资源文件
         if (File.Exists(resfile)) File.Delete(resfile);
         writer = new ResourceWriter(resfile);

         OutputFile();

         writer.Close();

      }

      public void ClearTmp()
      {
         Directory.Delete(GetAspNetTmpDir(), true);
         Console.WriteLine("Temporary ASP.NET Files Cleared!");
      }

      /// <summary>
      /// 清除旧的aspx程序集
      /// </summary>
      /// <param name="resfile"></param>
      public void ClearRes()
      {
         if (File.Exists(resfile)) 
         {
            ResourceSet rs = new ResourceSet(resfile);
            IDictionaryEnumerator id = rs.GetEnumerator();
            while(id.MoveNext())
            {
               String v = id.Value.ToString();
               String filename = root + @"\bin\" + v.Substring(0, v.IndexOf(":")) + ".dll";
               try
               {
                  if (File.Exists(filename)) File.Delete(filename);
                  Console.WriteLine(filename + " deleted!");
               }
               catch (Exception e)
               {
                  Console.WriteLine("试图删除" + filename + "时出错：" + e.Message);
               }
            }
            rs.Close();
         }
         Console.WriteLine("Clear resources completed!");
      }

      /// <summary>
      ///  通过访问aspx文件的url让系统自动编译一个文件
      /// </summary>
      /// <param name="file"></param>
      private void CompileFile(String file)
      {
         String url = file.Substring(root.Length-1).Replace('\\','/');
         if (url.ToLower().Equals("/zc.aspx")) return;

         allfiles.Add(url.ToLower());

         WebClient myWebClient = new WebClient();
         url = @"http://localhost" + appPath + url;
         try
         {
            myWebClient.DownloadData(url);
            Console.WriteLine("compile: " + url);
         }
         catch (Exception e)
         {
            Console.WriteLine(e.Message);
         }
      }

      /// <summary>
      /// 得到Temporary ASP.NET Files的位置
      /// </summary>
      /// <returns></returns>
      private String GetAspNetTmpDir()
      {
         try
         {
            RegistryKey regKey = Registry.LocalMachine;
            regKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\ASP.NET");
            String v = regKey.GetValue("RootVer").ToString();
            regKey = Registry.LocalMachine;
            regKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\ASP.NET\" + v);
            String frpath = regKey.GetValue("Path").ToString();

            return frpath + @"\Temporary ASP.NET Files\" + appPath.Substring(1);
         }
         catch (Exception e)
         {
            Console.WriteLine(e.StackTrace);
            return null;
         }
      }

      /// <summary>
      /// 获取当前application的Temporary ASP.NET Files位置
      /// </summary>
      /// <returns></returns>
      private String GetAppTmpDir()
      {

         // aspx编译产生的文件存放位置
         String theDir = GetAspNetTmpDir();
         if (theDir == null) return null;

         String[] dirs = Directory.GetDirectories(theDir);
         if (dirs==null || dirs.Length==0) return null;

         DateTime topdt = DateTime.MinValue;
         for (int i=0; i<dirs.Length; i++)
         {
            DateTime dt = Directory.GetLastWriteTime(dirs[i]);
            if (DateTime.Compare(dt, topdt) > 0)
            {
               topdt = dt;
               theDir = dirs[i];
            }
         }

         dirs = Directory.GetDirectories(theDir);
         if (dirs==null || dirs.Length==0) return null;

         topdt = DateTime.MinValue;
         for (int i=0; i<dirs.Length; i++)
         {
            DateTime dt = Directory.GetLastWriteTime(dirs[i]);
            if (DateTime.Compare(dt, topdt) > 0)
            {
               topdt = dt;
               theDir = dirs[i];
            }
         }

         Console.WriteLine("App TempDir: " + theDir + "\n");

         return theDir;

      }


      /// <summary>
      /// 通过获取编译后的文件产生文件名对文件
      /// </summary>
      private void OutputFile()
      {
         try
         {
            String theDir = GetAppTmpDir();
            if (theDir == null) return;

            string[] files = Directory.GetFiles(theDir, "*.xml");
            if (files==null || files.Length==0) return;

            int j = 0;
            for (int i=0; i<files.Length; i++)
            {
               StreamReader sr = new StreamReader(files[i]);
               String theLine = sr.ReadLine();
               String flag = "assem=\"";
               theLine = theLine.Substring(theLine.IndexOf(flag) + flag.Length);

               // aspx文件编译后产生的程序集名:类名
               String theValue = "";

               theValue = theLine.Substring(0, theLine.IndexOf("\""));

               while ((theLine=sr.ReadLine()) != null)
               {
                  if (theLine.ToLower().IndexOf(".aspx\"") > 0) break;
               }
               sr.Close();
               if (theLine == null) continue;

               Int32 idx1 = theLine.IndexOf("\"");
               Int32 idx2 = theLine.LastIndexOf("\"");

               if (idx1<0 || idx2<0 || idx2<=idx1) continue;

               // aspx文件相对Application的URL
               String theName = "";

               theName = theLine.Substring(idx1+1, idx2-idx1-1);
               theName = theName.Substring(root.Length-1).Replace('\\', '/');

               if (!allfiles.Contains(theName.ToLower())) continue;

               // 拷贝dll
               File.Copy(theDir + "\\" + theValue + ".dll", root + @"\bin\" + theValue + ".dll", true);

               theValue += ":ASP." + theName.Substring(theName.LastIndexOf("/") + 1).Replace('.', '_');

               // 输出theName:theValue到resources文件
               writer.AddResource(theName.ToLower(), theValue);

               Console.WriteLine("res: " + theName.ToLower() + " --> " + theValue);
               j++;
            }
            Console.WriteLine();
            Console.WriteLine("共有" + j + "个aspx程序集。");
         }
         catch (Exception e)
         {
            Console.WriteLine(e.StackTrace);
         }

      }

      /// <summary>
      /// 编译目录及其子目录内的所有aspx文件
      /// </summary>
      /// <param name="path"></param>
      private void CompileFiles(String path)
      {
         string[] dirs = Directory.GetDirectories(path);
         string[] files = Directory.GetFiles(path, "*.aspx");
         if (files != null)
         {
            for (int i=0; i<files.Length; i++)
            {
               CompileFile(files[i]);
            }
         }
         if (dirs != null)
         {
            for (int i=0; i<dirs.Length; i++)
            {
               if (dirs[i].Equals(root+"bin")) continue;
               CompileFiles(dirs[i]);
            }
         }
      } // end CompileFiles()

   }
}
