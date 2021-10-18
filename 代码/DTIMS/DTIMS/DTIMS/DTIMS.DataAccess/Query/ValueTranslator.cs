using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Web.Caching;
using System.Web;

namespace Inphase.CTQS.DataAccess.Query
{
    public sealed class ValueTranslator
    {
        private static ValueTranslator translator;

        private static readonly object syncRoot = new object();

        private Hashtable nameValuePair;

        public Hashtable NameValuePair
        {
            get { return nameValuePair; }
            set { nameValuePair = value; }
        }

        private ValueTranslator()
        {
            nameValuePair = new Hashtable();
            GetNameValuePair();

        }

        public static ValueTranslator GetTranslator()
        {
            if (translator == null)
            {
                lock (syncRoot)
                {
                    if (translator == null)
                    {
                        translator = new ValueTranslator();
                    }
                }
            }
            return translator;
        }

        private void GetNameValuePair()
        {
            try
            {
                Database mDataBase = DatabaseFactory.CreateDatabase();
                DbCommand cmdSelect;
                DataTable dtInfo = null;

                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT FT.[FT_TableName]+'_'+FT.[FT_Name]+'_'+FVT.[FVT_Value] AS CacheName,MAX([FVT_ExplainMeaning]) AS CacheValue ");
                sb.Append("FROM [FieldValueTranslate] AS FVT LEFT JOIN [FieldTranscribe] AS FT ON FVT.[FT_ID] = FT.[FT_ID]");
                sb.Append("GROUP BY FT.[FT_TableName],FT.[FT_Name],FVT.[FVT_Value]");

                cmdSelect = mDataBase.GetSqlStringCommand(sb.ToString());

                dtInfo = mDataBase.ExecuteDataSet(cmdSelect).Tables[0];

                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    nameValuePair.Add(dtInfo.Rows[i][0].ToString(), dtInfo.Rows[i][1].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
