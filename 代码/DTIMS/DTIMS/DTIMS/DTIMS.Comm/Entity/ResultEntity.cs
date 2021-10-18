using System;
using System.Collections.Generic;
using System.Text;

namespace BJ.DTIMS.Comm.Entity
{
    public class ResultEntity
    {
        //private bool? mResult = null;
        //private string mResultCode = "";
        //private string mDescr = "";

        /// <summary>
        /// 成功，失败
        /// </summary>
        public bool? Result { get; set; }

        /// <summary>
        /// 操作结果代码
        /// </summary>
        public string ResultCode { get; set; }


        /// <summary>
        /// 操作结果描述
        /// </summary>
        public string Descr { get; set; }
    }
}
