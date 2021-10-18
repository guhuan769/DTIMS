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
        /// �����Rar
        /// </summary>
        /// <param name="patch">�ļ�·��</param>
        /// <param name="rarPatch">ѹ�����·��</param>
        /// <param name="rarName">�ļ�����</param>
        /// <returns>ִ�гɹ����ؿ�,�񷵻ش���ԭ��</returns>
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
                    //�������
                    the_Info = " a " + rarName + " " + patch + " -ep"; ;
                    the_StartInfo = new ProcessStartInfo();
                    the_StartInfo.FileName = the_rar;
                    the_StartInfo.Arguments = the_Info;
                    the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    //����ļ����Ŀ¼
                    the_StartInfo.WorkingDirectory = rarPatch;
                    the_Process = new Process();
                    the_Process.StartInfo = the_StartInfo;
                    the_Process.Start();
                    the_Process.WaitForExit();
                    the_Process.Close();
                }
                catch (Exception ex)
                {
                    return rarName + "����ΪRAR�ļ���������ԭ��" + ex.Message;
                }
                return null;
            }
            return "�ü����δ��װWinRAR,�밲װ���ٲ���!";
        }

        /// <summary>
        /// ��ѹ
        /// </summary>
        /// <param name="unRarPatch">�ļ�·��</param>
        /// <param name="rarPatch">��ѹ�ļ����·��</param>
        /// <param name="rarName">�ļ�����</param>
        /// <returns>ִ�гɹ����ؿ�,�񷵻ش���ԭ��</returns>
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
                    //��������
                    the_Info = "x " + rarName + " " + unRarPatch + " -y";

                    ProcessStartInfo the_StartInfo = new ProcessStartInfo();
                    the_StartInfo.FileName = the_rar;
                    the_StartInfo.Arguments = the_Info;
                    the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    the_StartInfo.WorkingDirectory = rarPatch;//��ȡѹ����·��

                    //��ѹ�ļ�
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
            return "�ü����δ��װWinRAR,�밲װ���ٲ���!";
        }
    }//end public class ExecRAR
}//end namespace Inphase.CTQS.Common
