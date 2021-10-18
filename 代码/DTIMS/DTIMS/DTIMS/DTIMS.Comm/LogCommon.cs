using System;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace BJ.DTIMS.Common
{
    public class LogCommon
    {
        /// <summary>
        /// ��������:������ҵ��дWindows�¼���־(������־)
        /// </summary>
        /// <param name="message"></param>
        public static void WriteError(string message)
        {
            Write(message, Category.Errors, Priority.High, 1, System.Diagnostics.TraceEventType.Error);
        }

        /// <summary>
        /// ��������:������ҵ��дWindows�¼���־(������־)
        /// </summary>
        /// <param name="message"></param>
        public static void WriteWarning(string message)
        {
            Write(message, Category.Warnings, Priority.Normal, 1, System.Diagnostics.TraceEventType.Warning);
        }

        /// <summary>
        /// ��������:������ҵ��дWindows�¼���־(��ʾ�¼���־)
        /// </summary>
        /// <param name="message"></param>
        public static void WriteInformation(string message)
        {
            Write(message, Category.General, Priority.Lowest, 1, System.Diagnostics.TraceEventType.Information);
        }

        private static void Write(object message, string category,
            int priority, int eventId, System.Diagnostics.TraceEventType severity)
        {
            try
            {
                Logger.Write(message, category, priority, eventId, severity);
            }
            catch 
            {
                //�������¼��ϵͳ��һ����־�ļ��С�
            }
            finally
            {
                //
            }
        }
    }//end   public class LogCommon
}//end namespace Inphase.SMS.Common
