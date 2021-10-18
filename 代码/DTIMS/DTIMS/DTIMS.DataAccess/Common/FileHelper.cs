using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Inphase.Common
{
    /// <summary>
    /// 文件操作类by TYMFR
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 要读取的文件完全限定名

        /// </summary>
        /// <param name="path"></param>
        public FileHelper(string path)
        {
            this.FilePath = path;
        }
        /// <summary>
        /// 要读取的文件完全限定名

        /// </summary>
        /// <param name="path"></param>
        public FileHelper()
        {
            //
        }

        /// <summary>
        /// 属性 文件路径
        /// </summary>
        string filePath;
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        /// <summary>
        /// 得到文件的完全名字 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string FullFileName(string path)
        {
            return Path.GetFileName(path);//path不存在就返回path
        }

        /// <summary>
        /// 写文件

        /// </summary>
        /// <param name="context"></param>
        public void WriteFile(string context)/*写入内容*/
        {
            try
            {
                CreateDirectory();//创建文件目录

                if (!File.Exists(this.FilePath)) /*判断文件是否存在，存在true不存在false*/
                {
                    using (StreamWriter sw = File.CreateText(this.FilePath))
                    {
                        TextWriter tw = TextWriter.Synchronized(sw);
                        tw.Write(context);
                        tw.Close();
                    }
                }
                else
                {
                    FileStream fs = File.Open(this.filePath, FileMode.Append, FileAccess.Write);
                    StreamWriter streamWriter = new StreamWriter(fs);
                    streamWriter.Flush();
                    streamWriter.Write(context);
                    //关闭此文件

                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 写文件

        /// </summary>
        /// <param name="context"></param>
        public void WriteFile(string context,FileMode fm)/*写入内容*/
        {
            try
            {
                CreateDirectory();//创建文件目录

                if (!File.Exists(this.FilePath)) /*判断文件是否存在，存在true不存在false*/
                {
                    using (StreamWriter sw = File.CreateText(this.FilePath))
                    {
                        TextWriter tw = TextWriter.Synchronized(sw);
                        tw.Write(context);
                        tw.Close();
                    }
                }
                else
                {
                    FileStream fs = File.Open(this.filePath, fm ,FileAccess.Write);
                    StreamWriter streamWriter = new StreamWriter(fs);
                    streamWriter.Flush();
                    streamWriter.Write(context);
                    //关闭此文件

                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            catch 
            {
            }
        }

        /// <summary>
        /// 读文件
        /// </summary>
        /// <returns></returns>
        public string ReadFile()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                using (StreamReader sr = File.OpenText(this.FilePath))
                {
                    string str = string.Empty;
                    while ((str = sr.ReadLine()) != null)
                    {
                        sb.Append(str);
                    }
                }
                return sb.ToString();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <returns></returns>
        public bool DeleteFile()
        {
            try
            {
                if (File.Exists(this.FilePath))
                {
                    File.Delete(this.FilePath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 复制文件,将我们的文件复制到一个新文件,新文件如果已经存在那么就挂了
        /// </summary>
        /// <param name="path"></param>
        public bool CopyFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.Copy(this.FilePath/*源文件*/ , path/*目标文件*/);
                    return true;//复制成功了

                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        private void CreateDirectory()
        {
            if (!Directory.Exists(filePath.Substring(0, filePath.LastIndexOf(@"\"))))
            {
                Directory.CreateDirectory(filePath.Substring(0, filePath.LastIndexOf(@"\")));
            }
        }
    }
}


