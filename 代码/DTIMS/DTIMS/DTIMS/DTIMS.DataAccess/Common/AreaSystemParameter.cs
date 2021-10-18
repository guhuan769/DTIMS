using System;

namespace Inphase.Common.DBParameter
{
   /// <summary>
   /// �������еĲ���
   /// </summary>
   public class AreaSystemParameter : SystemParameter
   {

      public AreaSystemParameter(string dbName, bool isCache)
         : base(dbName, isCache)
      {
      }

      protected override void SetTableInfo()
      {
         base.mTableName = "AreaSystemParameter";
         base.mKey = "ASYSP_Key";
         base.mValue = "ASYSP_Value";
      }
   }
}
