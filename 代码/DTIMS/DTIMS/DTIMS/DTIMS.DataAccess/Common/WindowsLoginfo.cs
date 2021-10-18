using System;
using System.Data;
using System.Diagnostics;

namespace Inphase.Common.Log
{
   #region �¼�����
   /// <summary>
   /// ��־�¼�
   /// </summary>
   public delegate void WindowsLogHeader(string logInfo, string Source, EventLogEntryType type);
   #endregion

   /// <summary>	
   /// ģ��������־������
   /// ���ܣ���ϵͳ�������ճ�д��Windwosϵͳ��־��
   /// ���ߣ���־ƽ
   /// ���ڣ�2005-01-26
   /// </summary>
   public class WindowsLoginfo
   {
      /// <summary>
      /// ���캯��     
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

      #region ����
      /// <summary>
      /// ��־��׺
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
      /// ������־Դ
      /// </summary>
      /// <param name="LogSourceName"></param>
      private void CreateDDCSLogSource(string logSourceName)
      {
         try
         {
            string eventName = logName + logTag;
            if (!EventLog.Exists(eventName))
            {
               //���¼�Դ
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

            //������־Դ
            string logItem = logSourceName;
            if (!EventLog.SourceExists(logItem))
            {
               EventLog.CreateEventSource(logItem, eventName);
            }

         }
         catch (Exception ee)
         {
            throw new Exception("������־Դʱ����" + ee.Message);
         }
      }

      /// <summary>
      ///дϵͳ��־
      /// </summary>
      /// <param name="logInfo">��־��Ϣ</param>
      /// <param name="Source">��־Դ</param>
      /// <param name="elt">��־����</param>
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
            EventLog.WriteEntry(strSource.ToString(), "д��־ʱ�쳣��" + ee.Message + msg);
         }
      }

   }
}
