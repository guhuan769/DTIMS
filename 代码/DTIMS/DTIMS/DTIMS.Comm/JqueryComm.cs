using System;
using System.Collections.Generic;
using System.Text;

namespace DTIMS.Comm
{
    #region Jquery公用枚举与结构体
    /// <summary>
    /// 消息框从右下角弹出类型
    /// </summary>
    public struct ShowType
    {
        /// <summary>
        /// 从右向左弹出
        /// </summary>
        public const string show = "show";

        /// <summary>
        /// 从下往上弹出
        /// </summary>
        public const string slide = "slide";

        /// <summary>
        /// 直接弹出
        /// </summary>
        public const string fade = "fade";
    }

    /// <summary>
    /// 消息提示框类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 错误类型
        /// </summary>
         error,

        /// <summary>
        /// 提示类型
        /// </summary>
         info,

        /// <summary>
        /// 疑问类型
        /// </summary>
         question,

        /// <summary>
        /// 警告类型
        /// </summary>
         warning,

        /// <summary>
        /// 确认对话框类型
        /// </summary>
        confirm,

        /// <summary>
        /// 确认对话框(带输入框)类型
        /// </summary>
         prompt
    }

    #endregion Jquery公用枚举与结构体

    /// <summary>
    /// 功能描述JQUERY常用方法抽取
    /// </summary>
    public class JqueryComm
    {
        #region 屏幕右下角弹出对话框
        /// <summary>
        /// 功能描述：在屏幕右下角弹出消息框
        /// </summary>
        /// <param name="title">消息框标题</param>
        /// <param name="msg">消息内容</param>
        /// <param name="timeout">自动关闭时间(毫秒)</param>
        /// <param name="showType">消息框弹出类型</param>
        /// <returns></returns>
        public static string ShowMessage(string title, string msg, int timeout, ShowType showType)
        {
            //格式化内容中的hmtl标记
            msg = Common.CheckSpecialString(msg);

            string jsString = "$.messager.alert('{0}','{1}', '{2}',{3});";
            StringBuilder sb = new StringBuilder();
            sb.Append("$(document).ready(");
            sb.Append("function(){");
            sb.Append(string.Format(jsString, title, msg, timeout.ToString(), showType.ToString()));
            sb.Append("});");
            return sb.ToString();
        }

        /// <summary>
        /// 功能描述：在屏幕右下角弹出消息框
        /// </summary>
        /// <param name="title">消息框标题</param>
        /// <param name="msg">消息内容</param>
        /// <param name="showType">消息框弹出类型</param>
        /// <returns></returns>
        public static string ShowMessage(string title, string msg, ShowType showType)
        {
            return ShowMessage(title, msg, 4000, showType);
        }
        #endregion 屏幕右下角弹出对话框

        #region 弹出对话框
        /// <summary>
        /// 功能描述：弹出对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">内容</param>
        /// <param name="funName">回调函数名称</param>
        /// <param name="msgType">对话框类型</param>
        /// <returns></returns>
        public static string ShowMessage(string title,string msg, MessageType msgType,string funName)
        {
            //判断是否有回调函数
            if (string.IsNullOrEmpty(funName))
            {
                funName = "";
            }
            else
            {
                funName = "," + funName;
            }

            //判断是确认框的时候将标题改为"确认"
            if ((msgType == MessageType.confirm || msgType == MessageType.prompt) && (title=="提示" || title==null)) 
            {
                title = "确认";
            }
            
            //格式化内容中的hmtl标记
            msg = Common.CheckSpecialString(msg);

            string type = "alert";
            string icon = ",'info'";
            switch (msgType)
            {
                case MessageType.error:
                    icon = ",'error'";
                    break;
                case MessageType.info:
                    icon = ",'info'";
                    break;
                case MessageType.question:
                    icon = ",'question'";
                    break;
                case MessageType.warning:
                    icon = ",'warning'";
                    break;
                case MessageType.confirm:
                    type = "confirm";
                    icon = "";
                    break;
                case MessageType.prompt:
                    type = "prompt";
                    icon = "";
                    break;
            }
            string temp = "$.messager.{4}('{0}','{1}' {2} {3});";
            StringBuilder sb = new StringBuilder();
            sb.Append("$(document).ready(");
            sb.Append("function(){");
            sb.Append(string.Format(temp, title, msg, icon, funName, type));
            sb.Append("});");
            return sb.ToString();
        }

        /// <summary>
        /// 功能描述：弹出对话框
        /// </summary>
        /// <param name="msg">内容</param>
        /// <param name="msgType">对话框类型</param>
        /// <param name="funName">回调函数名称</param>
        /// <returns></returns>
        public static string ShowMessage(string msg, MessageType msgType, string funName)
        {
            return ShowMessage("提示", msg, msgType, funName);
        }

        /// <summary>
        /// 功能描述：弹出对话框
        /// </summary>
        /// <param name="msg">内容</param>
        /// <param name="msgType">对话框类型</param>
        /// <returns></returns>
        public static string ShowMessage(string msg, MessageType msgType)
        {
            return ShowMessage("提示", msg, msgType, null);
        }

        /// <summary>
        /// 功能描述：弹出对话框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">内容</param>
        /// <param name="msgType">对话框类型</param>
        /// <returns></returns>
        public static string ShowMessage(string title,string msg, MessageType msgType)
        {
            return ShowMessage(title, msg, msgType, null);
        }

        /// <summary>
        /// 功能描述：弹出对话框
        /// </summary>
        /// <param name="msg">内容</param>
        /// <returns></returns>
        public static string ShowMessage(string msg)
        {
            return ShowMessage("提示", msg, MessageType.info, null);
        }
        #endregion

        #region 格式化执行前台页面的脚本方法
        /// <summary>
        /// 功能描述：格式化执行前台页面的脚本方法
        /// </summary>
        /// <param name="funName">方法名称</param>
        /// <returns></returns>
        public static string FormatScriptString(string funName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("$(document).ready(");
            sb.Append("function(){");
            sb.Append("if(jQuery.isFunction(" + funName + ")){");
            sb.Append(funName + "();");
            sb.Append("}});");
            return sb.ToString();
        }
        #endregion 格式化执行前台页面的脚本方法
    }
}
