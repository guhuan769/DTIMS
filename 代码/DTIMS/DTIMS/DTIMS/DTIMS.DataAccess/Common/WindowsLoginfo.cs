using System;
using System.Data;
using System.Diagnostics;

namespace Inphase.Common.Log
{
   #region 事件定义
   /// <summary>
   /// 日志事件
   /// </summary>
   public delegate void WindowsLogHeader(string logInfo, string Source, EventLogEntryType type);
   #endregion

   /// <summary>	
   /// 模块名：日志管理类
   /// 功能：把系统产生的日常写入Windwos系统日志中
   /// 作者：曹志平
   /// 日期：2005-01-26
   /// </summary>
   public class WindowsLoginfo
   {
      /// <summary>
      /// 构造函数     
      /// </summary>		
      public WindowsLoginfo(string name)
         : this(name, "")
      {
      }

      public WindowsLoginfo(string name, string tag)
      {
         this.logTag = tag;
         this.logName = name;
      }

      #region 属性
      /// <summary>
      /// 日志后缀
      /// </summary>
      protected string logTag;
      public string LogTag
      {
         set { logTag = value; }
      }

      protected string logName;
      public string LogName
      {
         set { LogName = value; }
      }

      #endregion

      /// <summary>
      /// 创建日志源
      /// </summary>
      /// <param name="LogSourceName"></param>
      private void CreateDDCSLogSource(string logSourceName)
      {
         try
         {
            string eventName = logName + logTag;
            if (!EventLog.Exists(eventName))
            {
               //建事件源
               if (!EventLog.SourceExists(eventName))
               {
                  EventLog.CreateEventSource(eventName, eventName);
               }
               else
               {
                  EventLog.DeleteEventSource(eventName);
                  EventLog.CreateEventSource(eventName, eventName);
               }
            }

            //创建日志源
            string logItem = logSourceName;
            if (!EventLog.SourceExists(logItem))
            {
               EventLog.CreateEventSource(logItem, eventName);
            }

         }
         catch (Exception ee)
         {
            throw new Exception("创建日志源时出错：" + ee.Message);
         }
      }

      /// <summary>
      ///写系统日志
      /// </summary>
      /// <param name="logInfo">日志信息</param>
      /// <param name="Source">日志源</param>
      /// <param name="elt">日志类型</param>
      public void WriteLog(string msg, string source, EventLogEntryType logType)
      {
         string strSource = source + logTag;
         try
         {

            if (EventLog.SourceExists(strSource))
            {
               EventLog.WriteEntry(strSource, msg, logType);
            }
            else
            {
               CreateDDCSLogSource(strSource);
               EventLog.WriteEntry(strSource, msg, logType);
            }
         }
         catch (Exception ee)
         {
            EventLog.WriteEntry(strSource.ToString(), "写日志时异常！" + ee.Message + msg);
         }
      }

   }
}
