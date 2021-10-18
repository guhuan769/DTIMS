using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

namespace BJ.DTIMS.Common
{
    public class ExecRAR
    {
         private bool the_Exists = false;
        private RegistryKey the_Reg;
        public ExecRAR()
        {
            the_Reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
            if (the_Reg == null)
            {
                the_Exists = false;
            }
            else
            {
                the_Exists = true;
            }
        }

        /// <summary>
        /// 打包成Rar
        /// </summary>
        /// <param name="patch">文件路径</param>
        /// <param name="rarPatch">压缩存放路径</param>
        /// <param name="rarName">文件名称</param>
        /// <returns>执行成功返回空,否返回错误原因</returns>
        public string CompressRAR(string patch, string rarPatch, string rarName)
        {
            if (the_Exists)
            {
                string the_rar;
                object the_Obj;
                string the_Info;
                ProcessStartInfo the_StartInfo;
                Process the_Process;
                try
                {
                    the_Obj = the_Reg.GetValue("");
                    the_rar = the_Obj.ToString();
                    the_Reg.Close();
                    //the_rar = the_rar.Substring(1, the_rar.Length - 7);
                    Directory.CreateDirectory(rarPatch);
                    //命令参数
                    the_Info = " a " + rarName + " " + patch + " -ep"; ;
                    the_StartInfo = new ProcessStartInfo();
                    the_StartInfo.FileName = the_rar;
                    the_StartInfo.Arguments = the_Info;
                    the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    //打包文件存放目录
                    the_StartInfo.WorkingDirectory = rarPatch;
                    the_Process = new Process();
                    the_Process.StartInfo = the_StartInfo;
                    the_Process.Start();
                    the_Process.WaitForExit();
                    the_Process.Close();
                }
                catch (Exception ex)
                {
                    return rarName + "保存为RAR文件出错，错误原因：" + ex.Message;
                }
                return null;
            }
            return "该计算机未安装WinRAR,请安装后再操作!";
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="unRarPatch">文件路径</param>
        /// <param name="rarPatch">解压文件存放路径</param>
        /// <param name="rarName">文件名称</param>
        /// <returns>执行成功返回空,否返回错误原因</returns>
        public string unCompressRAR(string unRarPatch, string rarPatch, string rarName)
        {
            if (the_Exists)
            {
                string the_rar;
                object the_Obj;
                string the_Info;
                try
                {
                    the_Obj = the_Reg.GetValue("");
                    the_rar = the_Obj.ToString();
                    the_Reg.Close();
                    //the_rar = the_rar.Substring(1, the_rar.Length - 7);

                    if (Directory.Exists(unRarPatch) == false)
                    {
                        Directory.CreateDirectory(unRarPatch);
                    }
                    //参数命令
                    the_Info = "x " + rarName + " " + unRarPatch + " -y";

                    ProcessStartInfo the_StartInfo = new ProcessStartInfo();
                    the_StartInfo.FileName = the_rar;
                    the_StartInfo.Arguments = the_Info;
                    the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    the_StartInfo.WorkingDirectory = rarPatch;//获取压缩包路径

                    //解压文件
                    Process the_Process = new Process();
                    the_Process.StartInfo = the_StartInfo;
                    the_Process.Start();
                    the_Process.WaitForExit();
                    the_Process.Close();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                return null;
            }
            return "该计算机未安装WinRAR,请安装后再操作!";
        }
    }//end public class ExecRAR
}//end namespace Inphase.CTQS.Common
