using System;
using System.Collections.Generic;
using System.Text;

namespace DTIMS.Comm
{
    #region Jquery����ö����ṹ��
    /// <summary>
    /// ��Ϣ������½ǵ�������
    /// </summary>
    public struct ShowType
    {
        /// <summary>
        /// �������󵯳�
        /// </summary>
        public const string show = "show";

        /// <summary>
        /// �������ϵ���
        /// </summary>
        public const string slide = "slide";

        /// <summary>
        /// ֱ�ӵ���
        /// </summary>
        public const string fade = "fade";
    }

    /// <summary>
    /// ��Ϣ��ʾ������
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// ��������
        /// </summary>
         error,

        /// <summary>
        /// ��ʾ����
        /// </summary>
         info,

        /// <summary>
        /// ��������
        /// </summary>
         question,

        /// <summary>
        /// ��������
        /// </summary>
         warning,

        /// <summary>
        /// ȷ�϶Ի�������
        /// </summary>
        confirm,

        /// <summary>
        /// ȷ�϶Ի���(�������)����
        /// </summary>
         prompt
    }

    #endregion Jquery����ö����ṹ��

    /// <summary>
    /// ��������JQUERY���÷�����ȡ
    /// </summary>
    public class JqueryComm
    {
        #region ��Ļ���½ǵ����Ի���
        /// <summary>
        /// ��������������Ļ���½ǵ�����Ϣ��
        /// </summary>
        /// <param name="title">��Ϣ�����</param>
        /// <param name="msg">��Ϣ����</param>
        /// <param name="timeout">�Զ��ر�ʱ��(����)</param>
        /// <param name="showType">��Ϣ�򵯳�����</param>
        /// <returns></returns>
        public static string ShowMessage(string title, string msg, int timeout, ShowType showType)
        {
            //��ʽ�������е�hmtl���
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
        /// ��������������Ļ���½ǵ�����Ϣ��
        /// </summary>
        /// <param name="title">��Ϣ�����</param>
        /// <param name="msg">��Ϣ����</param>
        /// <param name="showType">��Ϣ�򵯳�����</param>
        /// <returns></returns>
        public static string ShowMessage(string title, string msg, ShowType showType)
        {
            return ShowMessage(title, msg, 4000, showType);
        }
        #endregion ��Ļ���½ǵ����Ի���

        #region �����Ի���
        /// <summary>
        /// ���������������Ի���
        /// </summary>
        /// <param name="title">����</param>
        /// <param name="msg">����</param>
        /// <param name="funName">�ص���������</param>
        /// <param name="msgType">�Ի�������</param>
        /// <returns></returns>
        public static string ShowMessage(string title,string msg, MessageType msgType,string funName)
        {
            //�ж��Ƿ��лص�����
            if (string.IsNullOrEmpty(funName))
            {
                funName = "";
            }
            else
            {
                funName = "," + funName;
            }

            //�ж���ȷ�Ͽ��ʱ�򽫱����Ϊ"ȷ��"
            if ((msgType == MessageType.confirm || msgType == MessageType.prompt) && (title=="��ʾ" || title==null)) 
            {
                title = "ȷ��";
            }
            
            //��ʽ�������е�hmtl���
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
        /// ���������������Ի���
        /// </summary>
        /// <param name="msg">����</param>
        /// <param name="msgType">�Ի�������</param>
        /// <param name="funName">�ص���������</param>
        /// <returns></returns>
        public static string ShowMessage(string msg, MessageType msgType, string funName)
        {
            return ShowMessage("��ʾ", msg, msgType, funName);
        }

        /// <summary>
        /// ���������������Ի���
        /// </summary>
        /// <param name="msg">����</param>
        /// <param name="msgType">�Ի�������</param>
        /// <returns></returns>
        public static string ShowMessage(string msg, MessageType msgType)
        {
            return ShowMessage("��ʾ", msg, msgType, null);
        }

        /// <summary>
        /// ���������������Ի���
        /// </summary>
        /// <param name="title">����</param>
        /// <param name="msg">����</param>
        /// <param name="msgType">�Ի�������</param>
        /// <returns></returns>
        public static string ShowMessage(string title,string msg, MessageType msgType)
        {
            return ShowMessage(title, msg, msgType, null);
        }

        /// <summary>
        /// ���������������Ի���
        /// </summary>
        /// <param name="msg">����</param>
        /// <returns></returns>
        public static string ShowMessage(string msg)
        {
            return ShowMessage("��ʾ", msg, MessageType.info, null);
        }
        #endregion

        #region ��ʽ��ִ��ǰ̨ҳ��Ľű�����
        /// <summary>
        /// ������������ʽ��ִ��ǰ̨ҳ��Ľű�����
        /// </summary>
        /// <param name="funName">��������</param>
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
        #endregion ��ʽ��ִ��ǰ̨ҳ��Ľű�����
    }
}
