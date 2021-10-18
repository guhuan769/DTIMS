using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BJ.DTIMS.Common
{
    /// <summary>
    /// 功能描述:文件操作类(包括文件的移动,读取文件名信息等)
    /// </summary>
    public class FileCommon
    {
        #region 文件移动
        /// <summary>
        /// 功能描述:文件移动(将指定文件移动到指定目录)
        /// 修改说明:2009-3-25 新增
        /// <author>MXJ</author>
        /// </summary>
        /// <param name="sourceFile"></param>
        public static void FileMove(string sourceFile, string targetDir)
        {
            try
            {
                //判断源文件是否存在
                if (!File.Exists(sourceFile.Trim()))
                {
                    throw new Exception("未找到要移动的源文件!");
                }

                //判断目标目录是否存在
                string dir = Path.GetDirectoryName(targetDir);
                if (!Directory.Exists(dir))
                {
                    throw new Exception(string.Format("未找到文件目录({0})!",targetDir));
                }

                //如果目标文件存在,先删除目标文件
                if (File.Exists(targetDir))
                {
                    File.Delete(targetDir);
                }

                //移动文件
                File.Move(sourceFile, targetDir);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 文件删除
        /// <summary>
        /// 功能描述:将指定的文件删除
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

        #region 讨取Sms日志的所有文件名信息(*.log)
        /// <summary>
        /// 功能描述:讨取Sms日志的所有文件名信息
        /// </summary>
        public static Hashtable GetLogFileNames()
        {
            Hashtable htLogFileNames = new Hashtable(); //存放所有文件名字
            string fileIdentifier = BJ.DTIMS.Common.Configure.Parameter("SmsIdentifier");

            //判断指定的源文件目录是否存在
            string smsLog = BJ.DTIMS.Common.Configure.Parameter("SmsLog");
            if (!Directory.Exists(smsLog))
            {
                throw new Exception(string.Format("未找到文件目录({0})!", smsLog));
            }

            //将源文件目录中的所有文件名添加到文件列表中
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

        #region 讨取BCP文件目录下的所有文件名信息(*.bcp)
        /// <summary>
        /// 功能描述:讨取Sms日志的所有文件名信息
        /// </summary>
        public static Hashtable GetBcpFileNames()
        {
            Hashtable htBcpFileNames = new Hashtable(); //存放所有文件名字

            //判断指定的源文件目录是否存在
            string bcpDir = BJ.DTIMS.Common.Configure.Parameter("BcpFile");
            if (!Directory.Exists(bcpDir))
            {
                throw new Exception(string.Format("未找到文件目录({0})!", bcpDir));
            }

            //将源文件目录中的所有文件名添加到文件列表中
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
