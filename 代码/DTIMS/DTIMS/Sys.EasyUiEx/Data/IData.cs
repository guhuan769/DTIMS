using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.EasyUiEx.Data
{
    /// <summary>
    /// 所有呈现结果的基本数据接口。
    /// </summary>
    public interface IData
    {
        /// <summary>
        /// 获取界面使用数据的 EasyUi 控件类型。
        /// </summary>
        EasyUiTypeEnum EasyUiType
        {
            get;
        }

        /// <summary>
        /// 获取呈现给界面具体结果的数据内容。
        /// </summary>
        string Content
        {
            get;
        }
    }
}
