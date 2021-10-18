
using System;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Collections;
using System.Diagnostics;
using System.Configuration;

namespace Sys.Comm.WebTools
{

    /// <summary>
    /// 一些在页面上常用的方法
    /// </summary>
    public class HtmlUtil
    {

        private static String DELIM = "U";

        /// <summary>
        /// 执行控制台命令,不会抛出异常,但可以通过返回值判断是否执行成功
        /// </summary>
        /// <param name="cmd">命令行,或者是一个EXE文件</param>
        /// <returns>如果返回0表示执行成功,如果返回1表示执行失败</returns>
        public static int ExecCommand(String cmd)
        {
            int rt = 0;
            Process p = null;

            try
            {
                p = new Process();
                p.StartInfo.ErrorDialog = false;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = "/c \"" + cmd + "\"";
                p.Start();
                string output = p.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
                p.WaitForExit();

                rt = p.ExitCode;
            }
            catch (Exception e)
            {
                Console.WriteLine("执行控制台命令 " + cmd + " 失败：");
                Console.WriteLine(e);
                rt = 1;
            }
            finally
            {
                try
                {
                    if (p != null)
                    {
                        p.Kill();
                        p.Close();
                    }
                }
                catch
                {
                }
            }

            return rt;
        }

        /// <summary>
        /// 取得数据表中指定行,指定列的数据
        /// </summary>
        /// <param name="row">数据表中的指定行</param>
        /// <param name="idx">数据表中的指定列</param>
        /// <returns>把结果转化为字符串返回</returns>
        public static String GetField(DataRow row, int idx)
        {
            if (row == null)
            {
                return "";
            }

            if (row.Table.Columns.Count <= idx)
            {
                return "";
            }

            Object rt = row[idx];

            if (rt == null)
            {
                return "";
            }
            else
            {
                return rt.ToString().Trim();
            }
        }

        /// <summary>
        /// 复制ARRAYLIST,得到一个全新的集合.
        /// </summary>
        /// <param name="a">需要复制的集合</param>
        /// <returns>得到一个集合,与原来的集合无关,如果输入为NULL,结果为实例化后的一个空集合对象</returns>
        public static ArrayList Array2ArrayList(Array a)
        {
            ArrayList rt = new ArrayList();

            if (a == null || a.Length == 0)
            {
                return rt;
            }

            for (int i = 0; i < a.Length; i++)
            {
                rt.Add(a.GetValue(i));
            }

            return rt;
        }

        /// <summary>
        /// 由字符串得到集合
        /// </summary>
        /// <param name="str">当前的字符串</param>
        /// <param name="split">分隔串</param>
        /// <returns></returns>
        public static ArrayList GetArrayList(String str, char split)
        {
            String[] strArray = str.Split(split);
            ArrayList al = new ArrayList();

            foreach (String str1 in strArray)
            {
                if (str1.Trim() != "")
                {
                    al.Add(str1);
                }
            }

            return al;
        }

        /// <summary>
        /// 复制一个数据表格.得到一个全新的数据表格对象
        /// </summary>
        /// <param name="a">需要复制的数据表格对象</param>
        /// <returns>得到的一个全新的数据表格,如果输入数据表格对象为NULL,也返回NULL值.</returns>
        public static DataTable DataTable2DataTable(DataTable a)
        {
            DataTable dt = new DataTable();
            if (a == null)
            {
                return null;
            }
            dt = a.Clone();
            foreach (DataRow dr in a.Rows)
            {
                dt.Rows.Add(dr.ItemArray);
            }
            return dt;
        }

        /// <summary>
        /// 得到IDataParameter对象,一般用作输入或输出参数对象.
        /// </summary>
        /// <param name="name">输入或输出参数名称</param>
        /// <param name="t">输入或输出参数类型</param>
        /// <param name="v">值</param>
        /// <returns></returns>
        public static IDataParameter GenP(String name, SqlDbType t, Object v)
        {
            SqlParameter sp1 = new SqlParameter(name, t);
            sp1.Value = v;
            return sp1;
        }

        /// <summary>
        /// 得到保存在WEBCONFIG文件中的连接字符串,关健字由输入参数决定.
        /// 默认保存在WEBCONFIG文件中的system.web/dsnstore节中
        /// </summary>
        /// <param name="jndiName">保存连接字符串的关健字</param>
        /// <returns>实例化连接字符串并返回</returns>
        public static SqlConnection getConnection(String jndiName)
        {
            return new SqlConnection(getDSN(jndiName));
        }

        /// <summary>
        /// 以','号分隔字符串,得到整数数组
        /// </summary>
        /// <param name="p">输入需要分隔的字符串</param>
        /// <returns>整数数组</returns>
        public static Int32[] ArrInt32(String p)
        {
            String[] s = ArrString(p);
            Int32[] rt = new Int32[s.Length];
            for (Int32 i = 0; i < s.Length; i++)
            {
                if (s[i].Trim() != "")
                {
                    rt[i] = Int32.Parse(s[i]);
                }
            }
            return rt;
        }

        /// <summary>
        /// 以','分隔字符串得到字符串数组
        /// </summary>
        /// <param name="p">需要分隔的字符串</param>
        /// <returns>数组集合</returns>
        public static String[] ArrString(String p)
        {
            return p.Split(new Char[] { ',' });
        }

        /// <summary>
        /// 建立Form验证
        /// </summary>
        /// <param name="page">当前页面对象</param>
        /// <param name="userName">登陆用户名称</param>
        /// <param name="createPersistentCookie">是否保存Cookies</param>
        /// <param name="timeout">超时时间</param>
        public static void FormsAuthenticate(Page page, String userName, Boolean createPersistentCookie, Int32 timeout)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(userName, createPersistentCookie, timeout);
            String encTicket = FormsAuthentication.Encrypt(ticket);
            page.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
        }

        public static void FormsSignOut(HttpCookieCollection cookies)
        {
            cookies.Remove(FormsAuthentication.FormsCookieName);
        }

        /// <summary>
        /// 写日志文件.默认日志文件保存路径为F盘根目录的dotnetlog.txt文件,
        /// 注意需要预先建立此文件.文件最大数据为2048字节,如果超过此长度会重新建立此文件
        /// </summary>
        /// <param name="input"></param>
        public static void Log(String input)
        {
            string fname = "f:\\dotnetlog.txt";
            FileInfo finfo = new FileInfo(fname);

            // Only allow a file length of 2k
            if (finfo.Exists && finfo.Length > 2048)
            {
                finfo.Delete();
            }

            using (FileStream fs = finfo.OpenWrite())
            {
                StreamWriter w = new StreamWriter(fs);
                w.BaseStream.Seek(0, SeekOrigin.End);

                w.Write("{0}\t{1}\r\n", DateTime.Now.ToString(), input);

                w.Flush();
                w.Close();
            }
        }

        /// <summary>
        /// 得到config文件中定义的dsn字符串
        /// </summary>
        /// <param name="jndiName">关健字</param>
        /// <returns></returns>
        public static String getDSN(String jndiName)
        {
            //Object o = HttpContext.GetAppConfig("configuration/appSettings");
            //if (o == null) return null;

            //String temp =  (String)((NameValueCollection)o)[jndiName];
            //return temp;
            //return ConfigurationSettings.AppSettings[jndiName];
            return System.Configuration.ConfigurationManager.AppSettings[jndiName];
        }

        /// <summary>
        ///  得到config文件的section信息。
        /// </summary>
        /// <param name="sectionName">WebConfig中节点名称</param>
        /// <param name="key">节点中关健字名称</param>
        /// <returns>字符串</returns>
        public static String getConfigSection(String sectionName, String key)
        {
            //Object o = HttpContext.GetAppConfig("system.web/" + sectionName);
            object o = System.Web.Configuration.WebConfigurationManager.GetWebApplicationSection("system.web/" + sectionName);
            if (o == null) return null;

            return (String)((NameValueCollection)o)[key];
        }

        /// <summary>
        /// 返回NULL对应的字符串,空串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String null2none(String s)
        {
            if (s == null) return "";
            return s;
        }

        public static Int32 parseInt(String s)
        {
            if (s == null || s.Equals("")) return 0;
            return Int32.Parse(s);
        }

        public static String trim(String s)
        {
            if (s == null) return "";
            return s.Trim();
        }

        public static String form(HttpRequest request, String s)
        {
            String rt = request.Form[s];
            if (rt == null) rt = "";
            return rt;
        }

        public static String queryString(HttpRequest request, String s)
        {
            String rt = request.QueryString[s];
            if (rt == null) rt = "";
            return rt;
        }

        // 从 System.Data.SqlClient.SqlDataReader 中取数据
        public static String sdrString(IDataReader sdr, String s)
        {
            String rt = sdr[s].ToString();
            if (rt == null) rt = "";
            return rt;
        }

        public static String sdrString(IDataReader sdr, Int32 i)
        {
            String rt = sdr[i].ToString();
            if (rt == null) rt = "";
            return rt;
        }

        public static DateTime sdrDateTime(IDataReader sdr, String s)
        {
            try
            {
                return sdr.GetDateTime(sdr.GetOrdinal(s));
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime sdrDateTime(IDataReader sdr, Int32 i)
        {
            try
            {
                return sdr.GetDateTime(i);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static Boolean sdrBoolean(IDataReader sdr, String s)
        {
            try
            {
                return sdr.GetBoolean(sdr.GetOrdinal(s));
            }
            catch
            {
                return false;
            }
        }

        public static Boolean sdrBoolean(IDataReader sdr, Int32 i)
        {
            try
            {
                return sdr.GetBoolean(i);
            }
            catch
            {
                return false;
            }
        }

        public static Int32 sdrInt32(IDataReader sdr, String s)
        {
            try
            {
                return sdr.GetInt32(sdr.GetOrdinal(s));
            }
            catch
            {
                return 0;
            }
        }

        public static Int32 sdrInt32(IDataReader sdr, Int32 i)
        {
            try
            {
                return sdr.GetInt32(i);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 根据文本产生可以用做html控件的name的字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String getAsName(String text)
        {
            String rt = "";
            if (text == null || text.Length == 0) return DELIM;

            for (int i = 0; i < text.Length; i++)   // unicode string.
                rt += align(((Int32)text[i]).ToString("X"));
            return DELIM + rt;
        }

        /// <summary>
        /// 获取任一字符串（包括单一的白空）在HTML中对应的字符串。
        /// </summary>
        /// <param name="text">源字符串</param>
        /// <returns>结果字符串</returns>
        public static String getAsValue(String text)
        {
            return getHTMLEntity(text, false);
        }

        /// <summary>
        /// 获取字符串在HTML中对应的字符串
        /// </summary>
        /// <param name="text">源字符串</param>
        /// <param name="ignoreWS">是否不处理单一的白空(&nbsp;)</param>
        /// <returns>结果字符串</returns>
        public static String getAsValue(String text, Boolean ignoreWS)
        {
            return getHTMLEntity(text, ignoreWS);
        }

        /// <summary>
        /// 获取任一字符串（包括单一的白空）在HTML中对应的字符串。
        /// </summary>
        /// <param name="text">源字符串</param>
        /// <returns>结果字符串</returns>
        public static String getHTMLEntity(String text)
        {
            return getHTMLEntity(text, false);
        }

        /// <summary>
        /// 获取字符串在HTML中对应的字符串
        /// </summary>
        /// <param name="text">源字符串</param>
        /// <param name="ignoreWS">是否不处理单一的白空(&nbsp;)</param>
        /// <returns>结果字符串</returns>
        public static String getHTMLEntity(String text, Boolean ignoreWS)
        {
            if (ignoreWS && text.Equals("&nbsp;"))
            {
                return text;
            }

            String rt = "";
            if (text == null) return rt;

            String m = "\u0000~!@#$%^&*()_+`-=|:\"<>?\\;',./ ";
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (m.IndexOf(c) == -1)
                {
                    rt += c;
                    continue;
                }
                int code = (int)c;
                if (code == 0) code = (int)' ';
                rt += "&#" + code + ";";          // HTML entity format.
            }
            return rt;
        }

        /// <summary>
        /// 转换文本为javascript中可用的字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String getAsScriptValue(String text)
        {
            String rt = "";
            if (text == null) return rt;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c == '\r') { rt += "\\r"; continue; }
                if (c == '\n') { rt += "\\n"; continue; }
                if (c == '\'' || c == '"' || c == '\\' || c == '/') rt += '\\';
                rt += c;
            }
            return rt;
        }

        public static int ALIGN_LEFT = 0;
        public static int ALIGN_RIGHT = 1;

        public static String align(String s)
        {
            return align(s, 4);
        }

        public static String align(String s, int radius)
        {
            return align(s, radius, ALIGN_RIGHT);
        }

        public static String align(String s, int radius, int flag)
        {
            return align(s, radius, flag, '0');
        }

        /**
         * 通过填充指定的字符对齐文本到指定的长度
         * @param radius  文本对齐后的长度
         * @param flag    对齐方向
         * @param fill    填充字符
         */
        public static String align(String s, Int32 radius, Int32 flag, char fill)
        {
            char[] ch = new char[radius];
            for (int i = 0; i < radius; i++) ch[i] = fill;

            int len = s.Length;
            int max = radius > len ? len : radius;

            if (flag == ALIGN_LEFT)
                for (int i = 0; i < max; i++) ch[i] = s[i];
            else if (flag == ALIGN_RIGHT)
                for (int i = 0; i < max; i++) ch[radius - 1 - i] = s[len - 1 - i];
            else return s;

            return new String(ch);
        }

    }

}