using System;
using System.Collections.Generic;
using System.Text;

using Sys.EasyUiEx.Data;

namespace Sys.EasyUiEx
{
    /// <summary>
    /// 返回数据实体。
    /// <remarks>ResultType 属性为 ResultTypeEnum.ResponseData 有效。</remarks>
    /// </summary>
    public class ResponseDataEntity
    {
        #region 全局变量
        // 数据内容。
        private string m_DataContent;

        #endregion

        #region 属性&字段
        /// <summary>
        /// 获取或设置有关操作成功或失败的信息。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 获取或设置数据绑定操作返回的具体JSON数据。
        /// </summary>
        public IData Data { get; set; }

        /// <summary>
        /// 获取数据内容。
        /// </summary>
        public string DataContent
        {
            get
            {
                if (Data != null)
                {
                    m_DataContent = Data.Content;
                }
                else
                {
                    m_DataContent = "{}";
                }
                return m_DataContent;
            }
            private set { m_DataContent = value; }
        }

        #endregion

    }
}
