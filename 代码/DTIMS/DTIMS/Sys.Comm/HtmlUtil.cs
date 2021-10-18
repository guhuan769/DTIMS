
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
    /// һЩ��ҳ���ϳ��õķ���
    /// </summary>
    public class HtmlUtil
    {

        private static String DELIM = "U";

        /// <summary>
        /// ִ�п���̨����,�����׳��쳣,������ͨ������ֵ�ж��Ƿ�ִ�гɹ�
        /// </summary>
        /// <param name="cmd">������,������һ��EXE�ļ�</param>
        /// <returns>�������0��ʾִ�гɹ�,�������1��ʾִ��ʧ��</returns>
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
                Console.WriteLine("ִ�п���̨���� " + cmd + " ʧ�ܣ�");
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
        /// ȡ�����ݱ���ָ����,ָ���е�����
        /// </summary>
        /// <param name="row">���ݱ��е�ָ����</param>
        /// <param name="idx">���ݱ��е�ָ����</param>
        /// <returns>�ѽ��ת��Ϊ�ַ�������</returns>
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
        /// ����ARRAYLIST,�õ�һ��ȫ�µļ���.
        /// </summary>
        /// <param name="a">��Ҫ���Ƶļ���</param>
        /// <returns>�õ�һ������,��ԭ���ļ����޹�,�������ΪNULL,���Ϊʵ�������һ���ռ��϶���</returns>
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
        /// ���ַ����õ�����
        /// </summary>
        /// <param name="str">��ǰ���ַ���</param>
        /// <param name="split">�ָ���</param>
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
        /// ����һ�����ݱ��.�õ�һ��ȫ�µ����ݱ�����
        /// </summary>
        /// <param name="a">��Ҫ���Ƶ����ݱ�����</param>
        /// <returns>�õ���һ��ȫ�µ����ݱ��,����������ݱ�����ΪNULL,Ҳ����NULLֵ.</returns>
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
        /// �õ�IDataParameter����,һ����������������������.
        /// </summary>
        /// <param name="name">����������������</param>
        /// <param name="t">����������������</param>
        /// <param name="v">ֵ</param>
        /// <returns></returns>
        public static IDataParameter GenP(String name, SqlDbType t, Object v)
        {
            SqlParameter sp1 = new SqlParameter(name, t);
            sp1.Value = v;
            return sp1;
        }

        /// <summary>
        /// �õ�������WEBCONFIG�ļ��е������ַ���,�ؽ����������������.
        /// Ĭ�ϱ�����WEBCONFIG�ļ��е�system.web/dsnstore����
        /// </summary>
        /// <param name="jndiName">���������ַ����Ĺؽ���</param>
        /// <returns>ʵ���������ַ���������</returns>
        public static SqlConnection getConnection(String jndiName)
        {
            return new SqlConnection(getDSN(jndiName));
        }

        /// <summary>
        /// ��','�ŷָ��ַ���,�õ���������
        /// </summary>
        /// <param name="p">������Ҫ�ָ����ַ���</param>
        /// <returns>��������</returns>
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
        /// ��','�ָ��ַ����õ��ַ�������
        /// </summary>
        /// <param name="p">��Ҫ�ָ����ַ���</param>
        /// <returns>���鼯��</returns>
        public static String[] ArrString(String p)
        {
            return p.Split(new Char[] { ',' });
        }

        /// <summary>
        /// ����Form��֤
        /// </summary>
        /// <param name="page">��ǰҳ�����</param>
        /// <param name="userName">��½�û�����</param>
        /// <param name="createPersistentCookie">�Ƿ񱣴�Cookies</param>
        /// <param name="timeout">��ʱʱ��</param>
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
        /// д��־�ļ�.Ĭ����־�ļ�����·��ΪF�̸�Ŀ¼��dotnetlog.txt�ļ�,
        /// ע����ҪԤ�Ƚ������ļ�.�ļ��������Ϊ2048�ֽ�,��������˳��Ȼ����½������ļ�
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
        /// �õ�config�ļ��ж����dsn�ַ���
        /// </summary>
        /// <param name="jndiName">�ؽ���</param>
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
        ///  �õ�config�ļ���section��Ϣ��
        /// </summary>
        /// <param name="sectionName">WebConfig�нڵ�����</param>
        /// <param name="key">�ڵ��йؽ�������</param>
        /// <returns>�ַ���</returns>
        public static String getConfigSection(String sectionName, String key)
        {
            //Object o = HttpContext.GetAppConfig("system.web/" + sectionName);
            object o = System.Web.Configuration.WebConfigurationManager.GetWebApplicationSection("system.web/" + sectionName);
            if (o == null) return null;

            return (String)((NameValueCollection)o)[key];
        }

        /// <summary>
        /// ����NULL��Ӧ���ַ���,�մ�
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

        // �� System.Data.SqlClient.SqlDataReader ��ȡ����
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
        /// �����ı�������������html�ؼ���name���ַ���
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
        /// ��ȡ��һ�ַ�����������һ�İ׿գ���HTML�ж�Ӧ���ַ�����
        /// </summary>
        /// <param name="text">Դ�ַ���</param>
        /// <returns>����ַ���</returns>
        public static String getAsValue(String text)
        {
            return getHTMLEntity(text, false);
        }

        /// <summary>
        /// ��ȡ�ַ�����HTML�ж�Ӧ���ַ���
        /// </summary>
        /// <param name="text">Դ�ַ���</param>
        /// <param name="ignoreWS">�Ƿ񲻴���һ�İ׿�(&nbsp;)</param>
        /// <returns>����ַ���</returns>
        public static String getAsValue(String text, Boolean ignoreWS)
        {
            return getHTMLEntity(text, ignoreWS);
        }

        /// <summary>
        /// ��ȡ��һ�ַ�����������һ�İ׿գ���HTML�ж�Ӧ���ַ�����
        /// </summary>
        /// <param name="text">Դ�ַ���</param>
        /// <returns>����ַ���</returns>
        public static String getHTMLEntity(String text)
        {
            return getHTMLEntity(text, false);
        }

        /// <summary>
        /// ��ȡ�ַ�����HTML�ж�Ӧ���ַ���
        /// </summary>
        /// <param name="text">Դ�ַ���</param>
        /// <param name="ignoreWS">�Ƿ񲻴���һ�İ׿�(&nbsp;)</param>
        /// <returns>����ַ���</returns>
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
        /// ת���ı�Ϊjavascript�п��õ��ַ���
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
         * ͨ�����ָ�����ַ������ı���ָ���ĳ���
         * @param radius  �ı������ĳ���
         * @param flag    ���뷽��
         * @param fill    ����ַ�
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