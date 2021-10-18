using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace Sys.Comm
{
    public class Common
    {
        #region 写Cookies
        /// <summary>
        /// 写Cookies到客户机上
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="strValue">值</param>
        public static void WriteCookies(string key, string strValue)
        {
            //写cookies
            if (System.Web.HttpContext.Current.Request.Browser.Cookies)
            {
                string t = HttpUtility.UrlEncode(strValue);
                System.Web.HttpCookie htck = new HttpCookie(key, t);
                htck.Expires = DateTime.Now.AddDays(90);
                System.Web.HttpContext.Current.Response.Cookies.Add(htck);
            }
            else
            {
                throw (new Exception("浏览器不支持Cookies，请把本站点设置为受信任站点!"));
            }
        }

        #region 过滤掉字符串中的HMTL标记
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
        /// 判断字符串是全为数字
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
        /// 是否为日期型字符串  
        /// </summary>  
        /// <param name="StrSource">日期字符串(2008-05-08)</param>  
        /// <returns></returns>  
        public static bool IsDate(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
        }

        /// <summary>  
        /// 是否为时间型字符串  
        /// </summary>  
        /// <param name="source">时间字符串(15:00:00)</param>  
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
        /// 是否为日期+时间型字符串  
        /// </summary>  
        /// <param name="source"></param>  
        /// <returns></returns>  
        public static bool IsDateTime(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
        }

        /// <summary>
        /// 是否是SIP格式
        /// </summary>
        /// <param name="StrSource"></param>
        /// <returns></returns>
        public static bool isSipNumber(string StrSource)
        {
            string str = StrSource.ToLower();
            return Regex.IsMatch(str, @"^sip:|sip:+(\w+)@+(\w+)$");
        }

        /// <summary>
        /// 是否是TEL格式
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
