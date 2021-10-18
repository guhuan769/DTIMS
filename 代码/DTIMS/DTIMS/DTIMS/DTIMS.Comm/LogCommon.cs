using System;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace BJ.DTIMS.Common
{
    public class LogCommon
    {
        /// <summary>
        /// 功能描述:调用企业库写Windows事件日志(错误日志)
        /// </summary>
        /// <param name="message"></param>
        public static void WriteError(string message)
        {
            Write(message, Category.Errors, Priority.High, 1, System.Diagnostics.TraceEventType.Error);
        }

        /// <summary>
        /// 功能描述:调用企业库写Windows事件日志(警告日志)
        /// </summary>
        /// <param name="message"></param>
        public static void WriteWarning(string message)
        {
            Write(message, Category.Warnings, Priority.Normal, 1, System.Diagnostics.TraceEventType.Warning);
        }

        /// <summary>
        /// 功能描述:调用企业库写Windows事件日志(提示事件日志)
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
                //将错误记录在系统的一个日志文件中。
            }
            finally
            {
                //
            }
        }
    }//end   public class LogCommon
}//end namespace Inphase.SMS.Common
