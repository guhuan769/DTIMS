using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using BJ.DTIMS.Comm.Entity;

namespace BJ.DTIMS.Common
{
    #region 公用枚举
    public enum TimeTicksType
    {
        Hours,
        Minutes,
        Seconds,
        MilliSeconds,
    }
    /// <summary>
    /// 查询类型
    /// </summary>
    public enum QueryType
    {
        MDN = 1,
        IMSI = 2,
    }

    /// <summary>
    /// 设备类型
    /// </summary>
    public enum DeviceType
    {
        AAA = 1,
        HLR = 2,
        WAP = 3,
        ANAAA = 4,
    }

    /// <summary>
    /// 功能描述:定义日志类型的枚举
    /// </summary>
    public struct Category
    {
        public const string General = "General";
        public const string Trace = "Trace";
        public const string Errors = "Logging Errors";
        public const string Warnings = "Warnings";
    }

    /// <summary>
    /// 功能描述:定义事件日志严重级别的枚举
    /// </summary>
    public struct Priority
    {
        public const int Lowest = 0;
        public const int Low = 1;
        public const int Normal = 2;
        public const int High = 3;
        public const int Highest = 4;
    }

    #endregion

    public class Common
    {
        private static Hashtable htPhone = new Hashtable();
        /// <summary>
        /// GetTicks
        /// </summary>
        /// <param name="TicksType">Hours,Minutes,Seconds,MilliSecond</param>
        /// <returns></returns>
        public static int GetTimeTicks(TimeTicksType TicksType)
        {
            DateTime dt1 = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());

            DateTime dt2 = System.DateTime.Now;
            TimeSpan ts = dt2 - dt1;

            if (TicksType == TimeTicksType.Hours)
            {
                return Convert.ToInt32(ts.TotalHours);
            }
            else if (TicksType == TimeTicksType.Minutes)
            {
                return Convert.ToInt32(ts.TotalMinutes);
            }
            else if (TicksType == TimeTicksType.MilliSeconds)
            {
                return Convert.ToInt32(ts.TotalMilliseconds);
            }
            else
            {
                return Convert.ToInt32(ts.TotalMinutes);
            }
        }


        /// <summary>
        /// 功能描述：设置GridView的翻页控件
        /// </summary>
        /// <param name="sender"></param>
        public static void GridViewDataBound(object sender)
        {
            GridView theGrid = sender as GridView;
            if (theGrid.Rows.Count == 0)
            {
                return;
            }

            DropDownList ddl = (DropDownList)theGrid.BottomPagerRow.Cells[0].FindControl("ddlPaging");
            for (int cnt = 0; cnt < theGrid.PageCount; cnt++)
            {
                int curr = cnt + 1;
                ListItem item = new ListItem();//curr.ToString()
                item.Value = curr.ToString();
                item.Text = "第" + curr.ToString() + "页";

                //设置当前要选中的页码

                if (cnt == theGrid.PageIndex)
                {
                    item.Selected = true;
                }
                ddl.Items.Add(item);
            }
            //设置总记录数
            Label lbl = (Label)theGrid.BottomPagerRow.Cells[0].FindControl("lblRowsCount");
            lbl.Text = ((DataView)theGrid.DataSource).Count.ToString();


            //如果新页为首页将“上一页”设置为不可用

            if (theGrid.PageIndex == 0)
            {
                (theGrid.BottomPagerRow.FindControl("btnPrev") as WebControl).Enabled = false;
                (theGrid.BottomPagerRow.FindControl("btnFirst") as WebControl).Enabled = false;
            }
            else
            {
                (theGrid.BottomPagerRow.FindControl("btnPrev") as WebControl).Enabled = true;
                (theGrid.BottomPagerRow.FindControl("btnFirst") as WebControl).Enabled = true;
            }

            //如果新页为尾页将“尾页”设置为不可用

            if (theGrid.PageIndex == theGrid.PageCount - 1)
            {
                (theGrid.BottomPagerRow.FindControl("btnNext") as WebControl).Enabled = false;
                (theGrid.BottomPagerRow.FindControl("btnLast") as WebControl).Enabled = false;
            }
            else
            {
                (theGrid.BottomPagerRow.FindControl("btnNext") as WebControl).Enabled = true;
                (theGrid.BottomPagerRow.FindControl("btnLast") as WebControl).Enabled = true;
            }
        }

        #region　绑定空数据

        /// <summary>
        /// 功能描述：在没有数据时显示header
        /// </summary>
        /// <param name="EmptyGridView"></param>
        public static void RenderEmptyGridView(GridView EmptyGridView)
        {
            try
            {
                DataTable dt = new DataTable();
                int columnSpan = 0;
                for (int i = 0; i < EmptyGridView.Columns.Count; i++)
                {
                    if (EmptyGridView.Columns[i] is BoundField)
                    {
                        BoundField field = (BoundField)EmptyGridView.Columns[i];
                        dt.Columns.Add(new DataColumn(field.DataField, typeof(string)));
                    }

                    if (EmptyGridView.Columns[i].Visible)
                    {
                        columnSpan++;
                    }
                }

                DataRow dr = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    dr[col.ColumnName] = DBNull.Value;
                }
                dt.Rows.Add(dr);

                EmptyGridView.DataSource = dt.DefaultView;
                EmptyGridView.DataBind();

                //空数据时加上一个Td
                EmptyGridView.Rows[0].Controls.Clear();

                TableCell td = new TableCell();
                td.Text = "未找到符合条件的数据.";
                td.ColumnSpan = columnSpan;
                td.Attributes.Add("style", "text-align:center;");
                EmptyGridView.Rows[0].Controls.Add(td);
            }
            catch { }
        }
        #endregion

        /// <summary>
        /// 功能描述：通过ScriptManager执行JavaScript方法
        /// </summary>
        /// <param name="updatePanel"></param>
        /// <param name="script"></param>
        public static void ScriptManagerRegister(System.Web.UI.UpdatePanel updatePanel, string script)
        {
            //System.Random rd = new Random();
            System.Web.UI.ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(),
                      "Script" + Guid.NewGuid().ToString(), script, true);
        }

        /// <summary>
        /// 功能描述：通过ScriptManager显示Message
        /// </summary>
        /// <param name="updatePanel"></param>
        /// <param name="message"></param>
        public static void ScriptManagerMessageTemp(System.Web.UI.UpdatePanel updatePanel, string message)
        {
            if (message != null && message.Length > 0)
            {
                message = message.Replace("\"", "");
                message = message.Replace("/", "");
                message = message.Replace("&", "");
                message = message.Replace("<", "");
                message = message.Replace(">", "");
                message = message.Replace("'", "‘");
            }

            //System.Random rd = new Random();
            System.Web.UI.ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(),
                      "Messag" + Guid.NewGuid().ToString(), string.Format("alert('{0}');", message), true);
        }

        /// <summary>
        /// 功能描述：格式化参数字符串(将用户名与密码替换为***)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FormatCommandString(string input)
        {
            Match match = Regex.Match(input, "(?<=ctag:).*?(?=;)");
            if (match.Success)
            {
                input = input.Replace(match.Value.Trim(), "******");
            }

            //将IP地址替换
            match = Regex.Match(input, "(?<=IP=).*?(?=:)");
            if (match.Success)
            {
                input = input.Replace(match.Value.Trim(), "****");
            }
            return input;
        }

        /// <summary>
        /// 获取指定长度字符
        /// </summary>
        /// <returns></returns>
        public static string GetSubstring(string str, int length)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            if (str.Length > length)
            {
                try
                {
                    return str.Substring(0, length) + "...";
                }
                catch
                {
                    return string.Empty;
                }
            }
            else
            {
                return str;
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
                    strTemp += str[i].ToString().Trim();
                }
            }
            return strTemp;
        }
        #endregion


        #region 查找关键字，如果关键字存在则返回True，如果关键字不存在则返回False；

        /// <summary>
        /// 查找关键字，如果关键字存在则返回True，如果关键字不存在则返回False；
        /// </summary>
        /// <param name="regExp">进行字符串比对的正则表达式</param>
        /// <param name="rawStrings">在此字符串中查找与第一个参数所表达正则表达式匹配的字符串</param>
        /// FindTheKey("ABC","TuyongABC") 在"TuyongABC"中查找字符串"ABC",如果存在返回True,如果不存在返回False;
        /// <returns></returns>
        public static bool FindTheKey(string regExp, string rawStrings)
        {
            Regex re = new Regex(regExp, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            try
            {
                MatchCollection Matches = re.Matches(rawStrings);
                if (Matches.Count >= 1)
                {
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
        #endregion

        #region 替换字符串中相应的的字符；

        /*
            string inputString = "利用C#中的<正则表达式>可以很好地处理[字符串]问题！";
            string pattern = "正则表达式|字符串"; //将"正则表达式，字符串"替换掉
            string replacement = "替换一下";
            string output = Regex.Replace(inputString, pattern, replacement);
            Console.WriteLine(output); //输出"利用C#中的<替换一下>可以很好地处理[替换一下]问题！"
         */
        /// <summary>
        /// 替换字符串中相应的的字符；
        /// </summary>
        /// <param name="inputString">原字符串</param>
        /// <param name="pattern">被替换的字符串的正则表达式</param>
        /// <param name="rawRegExp">替换后的字符串</param>
        /// ReplaceTheKey("TuyongABC","ABC","MFR")//把字符串TuyongABC中的ABC替换成MFR，替换后的字符串为：TuyongMFR；

        /// <returns></returns>
        public static string ReplaceTheKey(string inputString, string pattern, string replacement)
        {
            string result = "";
            try
            {
                result = Regex.Replace(inputString, pattern, replacement);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("ReplaceTheKey", ex);
            }
        }
        #endregion

        #region  匹配正则表达式所表示的内容；
        /// <summary>
        /// 匹配正则表达式所表示的内容；
        /// </summary>
        /// <param name="regExp">正则表达式</param>
        /// <param name="rawSingleCommand"></param>
        /// <returns></returns>
        public static string RegMatch(string regExp, string rawSingleCommand)
        {
            try
            {
                Regex re = new Regex(regExp, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                MatchCollection Matches = re.Matches(rawSingleCommand);
                string matchValue = "";
                foreach (Match NextMatch in Matches)
                {
                    matchValue = NextMatch.Value;
                }
                return matchValue;
            }
            catch (Exception ex)
            {
                throw new Exception("RegMatch", ex);
            }
        }
        #endregion

        #region 以正则表达式为分界，切割原始字符串

        public static string[] RegSplit(string regExp, string rawSingleCommand)
        {

            try
            {
                Regex re = new Regex(regExp, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                return re.Split(rawSingleCommand);
            }
            catch (Exception ex)
            {
                throw new Exception("SplitError", ex);
            }
        }

        #endregion

        #region  匹配正则表达式所表示的内容；
        /// <summary>
        /// 匹配正则表达式所表示的内容；
        /// </summary>
        /// <param name="regExp">正则表达式</param>
        /// <param name="rawSingleCommand"></param>
        /// <returns></returns>
        public static string[] RegFindArr(string regExp, string rawSingleCommand)
        {
            try
            {
                Regex re = new Regex(regExp, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                MatchCollection Matches = re.Matches(rawSingleCommand);
                string[] resultStrArr = new string[Matches.Count];
                int i = 0;
                foreach (Match NextMatch in Matches)
                {
                    resultStrArr[i] = NextMatch.Value;
                    i++;
                }
                return resultStrArr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 将所有HTML标签替换成""
        /// <summary>   
        /// 将所有HTML标签替换成""   
        /// </summary>   
        /// <param name="strHtml"></param>   
        /// <returns></returns>   
        public static string ReplaceHTMLToEmpty(string strHtml)
        {
            string[] aryReg ={   
            @"<script[^>]*?>.*?</script>",   
            @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(file://[""'tbnr]|[^/7])*?/7|/w+)|.{0})|/s)*?(///s*)?>",   
            @"<[\s]*?script[^>]*?>[\s\S]*?<[\s]*?\/[\s]*?script[\s]*?>",
            @"([\r\n])[\s]+",   
            @"&(quot|#34);",   
            @"&(amp|#38);",   
            @"&(lt|#60);",   
            @"&(gt|#62);",    
            @"&(nbsp|#160);",    
            @"&(iexcl|#161);",   
            @"&(cent|#162);",   
            @"&(pound|#163);",   
            @"&(copy|#169);",   
            @"&#(\d+);",   
            @"-->",   
            @"<!--.*\n",
            @"<([^>]*)>"
            };

            string[] aryRep = {   
            "",   
            "",   
            "",   
            "",   
            "\"",   
            "&",   
            "<",   
            ">",   
            " ",   
            "\xa1",//chr(161),   
            "\xa2",//chr(162),   
            "\xa3",//chr(163),   
            "\xa9",//chr(169),   
            "",   
            "\r\n",   
            "",
            ""
            };

            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }

            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");


            return strOutput;
        }
        #endregion

        #region 16进制转码
        /// <summary>
        /// <函数：Encode>
        /// 作用：将字符串内容转化为16进制数据编码，其逆过程是Decode
        /// 参数说明：
        /// strEncode 需要转化的原始字符串
        /// 转换的过程是直接把字符转换成Unicode字符,比如数字"3"-->0033,汉字"我"-->U+6211
        /// 函数decode的过程是encode的逆过程.
        /// </summary>
        /// <param name="strEncode"></param>
        /// <returns></returns>
        public static string Encode(string strEncode)
        {
            string strReturn = "";//  存储转换后的编码
            foreach (short shortx in strEncode.ToCharArray())
            {
                strReturn += shortx.ToString("X2");
            }
            return strReturn;
        }
        /// <summary>
        /// <函数：Decode>
        ///作用：将16进制数据编码转化为字符串，是Encode的逆过程
        /// </summary>
        /// <param name="strDecode"></param>
        /// <returns></returns>
        public static string Decode(string strDecode)
        {
            string sResult = "";
            for (int i = 0; i < strDecode.Length / 4; i++)
            {
                sResult += (char)short.Parse(strDecode.Substring(i * 4, 4), global::System.Globalization.NumberStyles.HexNumber);
            }
            return sResult;
        }
        #endregion

    }//end public class Common
}//end namespace Inphase.CTQS.Common
