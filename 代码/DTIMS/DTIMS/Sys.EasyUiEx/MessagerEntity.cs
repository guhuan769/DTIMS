using System;
using System.Collections.Generic;
using System.Text;
using Sys.EasyUiEx;

namespace Sys.EasyUiEx
{
    /// <summary>
    /// 消息框实体。
    /// <remarks>ResultType 属性为 ResultTypeEnum.Messager 有效。</remarks>
    /// </summary>
    public class MessagerEntity
    {
        #region 全局变量
        // 消息框类型。
        private IconEnum m_Icon = IconEnum.None;

        #endregion

        #region 属性&字段
        /// <summary>
        /// 获取或设置消息框的标题。
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 获取或设置消息框的内容。
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 获取或设置消息框类型。
        /// </summary>
        public IconEnum Icon
        {
            get { return m_Icon; }
            set { m_Icon = value; }
        }

        #endregion

    }
}
