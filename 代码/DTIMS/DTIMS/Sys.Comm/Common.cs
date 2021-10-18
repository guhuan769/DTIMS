using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace Sys.Comm
{
    public class Common
    {
        #region дCookies
        /// <summary>
        /// дCookies���ͻ�����
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="strValue">ֵ</param>
        public static void WriteCookies(string key, string strValue)
        {
            //дcookies
            if (System.Web.HttpContext.Current.Request.Browser.Cookies)
            {
                string t = HttpUtility.UrlEncode(strValue);
                System.Web.HttpCookie htck = new HttpCookie(key, t);
                htck.Expires = DateTime.Now.AddDays(90);
                System.Web.HttpContext.Current.Response.Cookies.Add(htck);
            }
            else
            {
                throw (new Exception("�������֧��Cookies����ѱ�վ������Ϊ������վ��!"));
            }
        }

        #region ���˵��ַ����е�HMTL���
        public static string CheckSpecialString(String str)
        {
            string strTemp = null;
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i].ToString().Trim() != "<") && (str[i].ToString().Trim() != ">")
                    && (str[i].ToString().Trim() != "/") && (str[i].ToString().Trim() != ":") && (str[i].ToString().Trim() != "'"))
                {
                    strTemp += str[i].ToString();
                }
            }
            return strTemp;
        }
        #endregion
        #endregion

        /// <summary>
        /// �ж��ַ�����ȫΪ����
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(string text)
        {
            Regex r1 = new Regex("^[0-9]+$");
            Match m1 = r1.Match(text);
            if (m1.Success)
            {
                return true; 
            }

            return false;
        }

        /// <summary>  
        /// �Ƿ�Ϊ�������ַ���  
        /// </summary>  
        /// <param name="StrSource">�����ַ���(2008-05-08)</param>  
        /// <returns></returns>  
        public static bool IsDate(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
        }

        /// <summary>  
        /// �Ƿ�Ϊʱ�����ַ���  
        /// </summary>  
        /// <param name="source">ʱ���ַ���(15:00:00)</param>  
        /// <returns></returns>  
        public static bool IsTime(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
        }

        public static bool IsAcsTime(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((20|21|22|23|[0-1]?\d):[0-5]?\d)$");
        }

        /// <summary>  
        /// �Ƿ�Ϊ����+ʱ�����ַ���  
        /// </summary>  
        /// <param name="source"></param>  
        /// <returns></returns>  
        public static bool IsDateTime(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
        }

        /// <summary>
        /// �Ƿ���SIP��ʽ
        /// </summary>
        /// <param name="StrSource"></param>
        /// <returns></returns>
        public static bool isSipNumber(string StrSource)
        {
            string str = StrSource.ToLower();
            return Regex.IsMatch(str, @"^sip:|sip:+(\w+)@+(\w+)$");
        }

        /// <summary>
        /// �Ƿ���TEL��ʽ
        /// </summary>
        /// <param name="StrSource"></param>
        /// <returns></returns>
        public static bool isTelNumber(string StrSource)
        {
            string str = StrSource.ToLower();
            return Regex.IsMatch(str, @"^tel:|tel:+(\w+)$");
        }
    }
}
