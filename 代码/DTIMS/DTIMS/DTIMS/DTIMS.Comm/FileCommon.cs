using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BJ.DTIMS.Common
{
    /// <summary>
    /// ��������:�ļ�������(�����ļ����ƶ�,��ȡ�ļ�����Ϣ��)
    /// </summary>
    public class FileCommon
    {
        #region �ļ��ƶ�
        /// <summary>
        /// ��������:�ļ��ƶ�(��ָ���ļ��ƶ���ָ��Ŀ¼)
        /// �޸�˵��:2009-3-25 ����
        /// <author>MXJ</author>
        /// </summary>
        /// <param name="sourceFile"></param>
        public static void FileMove(string sourceFile, string targetDir)
        {
            try
            {
                //�ж�Դ�ļ��Ƿ����
                if (!File.Exists(sourceFile.Trim()))
                {
                    throw new Exception("δ�ҵ�Ҫ�ƶ���Դ�ļ�!");
                }

                //�ж�Ŀ��Ŀ¼�Ƿ����
                string dir = Path.GetDirectoryName(targetDir);
                if (!Directory.Exists(dir))
                {
                    throw new Exception(string.Format("δ�ҵ��ļ�Ŀ¼({0})!",targetDir));
                }

                //���Ŀ���ļ�����,��ɾ��Ŀ���ļ�
                if (File.Exists(targetDir))
                {
                    File.Delete(targetDir);
                }

                //�ƶ��ļ�
                File.Move(sourceFile, targetDir);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region �ļ�ɾ��
        /// <summary>
        /// ��������:��ָ�����ļ�ɾ��
        /// </summary>
        /// <param name="fileName"></param>
        public static void FileDelete(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region ��ȡSms��־�������ļ�����Ϣ(*.log)
        /// <summary>
        /// ��������:��ȡSms��־�������ļ�����Ϣ
        /// </summary>
        public static Hashtable GetLogFileNames()
        {
            Hashtable htLogFileNames = new Hashtable(); //��������ļ�����
            string fileIdentifier = BJ.DTIMS.Common.Configure.Parameter("SmsIdentifier");

            //�ж�ָ����Դ�ļ�Ŀ¼�Ƿ����
            string smsLog = BJ.DTIMS.Common.Configure.Parameter("SmsLog");
            if (!Directory.Exists(smsLog))
            {
                throw new Exception(string.Format("δ�ҵ��ļ�Ŀ¼({0})!", smsLog));
            }

            //��Դ�ļ�Ŀ¼�е������ļ�����ӵ��ļ��б���
            foreach (string fileName in System.IO.Directory.GetFiles(smsLog, fileIdentifier + "*.*"))
            {
                if(!htLogFileNames.ContainsKey(fileName.Trim()))
                {
                    htLogFileNames.Add(fileName.Trim(), fileName.Trim());
                }
            }//end foreach
            return htLogFileNames;
        }
        #endregion

        #region ��ȡBCP�ļ�Ŀ¼�µ������ļ�����Ϣ(*.bcp)
        /// <summary>
        /// ��������:��ȡSms��־�������ļ�����Ϣ
        /// </summary>
        public static Hashtable GetBcpFileNames()
        {
            Hashtable htBcpFileNames = new Hashtable(); //��������ļ�����

            //�ж�ָ����Դ�ļ�Ŀ¼�Ƿ����
            string bcpDir = BJ.DTIMS.Common.Configure.Parameter("BcpFile");
            if (!Directory.Exists(bcpDir))
            {
                throw new Exception(string.Format("δ�ҵ��ļ�Ŀ¼({0})!", bcpDir));
            }

            //��Դ�ļ�Ŀ¼�е������ļ�����ӵ��ļ��б���
            foreach (string fileName in System.IO.Directory.GetFiles(bcpDir, "*.bcp"))
            {
                if (!htBcpFileNames.ContainsKey(fileName.Trim()))
                {
                    htBcpFileNames.Add(fileName.Trim(), fileName.Trim());
                }
            }//end foreach
            return htBcpFileNames;
        }
        #endregion
       
    }
}
