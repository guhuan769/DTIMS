
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
	/// ��������aspx�ļ����ҿ���dll��binĿ¼�������ļ�����Ӧ�ļ�
	/// </summary>
	public class AspxCompile
	{

      /// <summary>
      /// Ӧ�ó�������Ŀ¼
      /// </summary>
      private String appPath = "";

      /// <summary>
      /// Ӧ�ó����Ŀ¼
      /// </summary>
      private String root = "";

      private String resfile = "";

      private IResourceWriter writer = null;

      /// <summary>
      /// ����aspx���Application��URL�б�
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

         // ����aspx�ļ�
         CompileFiles(root);
         Console.WriteLine("Compile complete.");
         Console.WriteLine();

         // ��2��
         Thread.Sleep(2000);

         // �����Դ�ļ�
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
      /// ����ɵ�aspx����
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
                  Console.WriteLine("��ͼɾ��" + filename + "ʱ����" + e.Message);
               }
            }
            rs.Close();
         }
         Console.WriteLine("Clear resources completed!");
      }

      /// <summary>
      ///  ͨ������aspx�ļ���url��ϵͳ�Զ�����һ���ļ�
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
      /// �õ�Temporary ASP.NET Files��λ��
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
      /// ��ȡ��ǰapplication��Temporary ASP.NET Filesλ��
      /// </summary>
      /// <returns></returns>
      private String GetAppTmpDir()
      {

         // aspx����������ļ����λ��
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
      /// ͨ����ȡ�������ļ������ļ������ļ�
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

               // aspx�ļ����������ĳ�����:����
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

               // aspx�ļ����Application��URL
               String theName = "";

               theName = theLine.Substring(idx1+1, idx2-idx1-1);
               theName = theName.Substring(root.Length-1).Replace('\\', '/');

               if (!allfiles.Contains(theName.ToLower())) continue;

               // ����dll
               File.Copy(theDir + "\\" + theValue + ".dll", root + @"\bin\" + theValue + ".dll", true);

               theValue += ":ASP." + theName.Substring(theName.LastIndexOf("/") + 1).Replace('.', '_');

               // ���theName:theValue��resources�ļ�
               writer.AddResource(theName.ToLower(), theValue);

               Console.WriteLine("res: " + theName.ToLower() + " --> " + theValue);
               j++;
            }
            Console.WriteLine();
            Console.WriteLine("����" + j + "��aspx���򼯡�");
         }
         catch (Exception e)
         {
            Console.WriteLine(e.StackTrace);
         }

      }

      /// <summary>
      /// ����Ŀ¼������Ŀ¼�ڵ�����aspx�ļ�
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
