using System;
using System.Collections.Generic;
using System.Text;

namespace DTIMS.Comm.Entity
{
    public enum NumberType
    {
        /// <summary>
        /// 其他号码
        /// </summary>
        None,

        /// <summary>
        /// 手机号码
        /// </summary>
        MobileNo,

        /// <summary>
        /// 固定号码
        /// </summary>
        FixedNo,

        /// <summary>
        /// IMSI
        /// </summary>
        IMSI,

        /// <summary>
        /// IMS，TEL方式
        /// </summary>
        IMS_TEL,

        /// <summary>
        /// IMS，SIP方式
        /// </summary>
        IMS_SIP,

    }

    /// <summary>
    /// 分权分域实体
    /// </summary>
    public class CheckAuthEntity
    {
        /// <summary>
        /// 是否有权限
        /// </summary>
        public bool IsHave { get; set; }

        /// <summary>
        /// 判断权限时返回的信息
        /// </summary>
        public string Descr { get;set;}

        /// <summary>
        /// 格式后的号码
        /// </summary>
        public string FormatedNumber { get; set; }


        /// <summary>
        /// 号码类型
        /// </summary>
        public NumberType Type { get; set; }
    }//end public class CheckAuthorityEntity
}//end namespace Inphase.JFIMS.Comm.Entity
