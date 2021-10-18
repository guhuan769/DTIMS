using System;
using System.Collections.Generic;
using System.Text;

using BJ.Sys.EasyUiEx.Helper;

namespace BJ.Sys.EasyUiEx.Data
{
    /// <summary>
    /// Jquery EasyUi Combobox 的基本数据类。
    /// </summary>
    public class Combobox : IData
    {
        #region 全局变量
        // 生成的数据字符串。
        private string m_data = "";
        #endregion

        #region IData接口成员
        /// <summary>
        /// 获取界面使用数据的 EasyUi 控件类型。
        /// </summary>
        public virtual EasyUiTypeEnum EasyUiType
        {
            get
            {
                return EasyUiTypeEnum.Combobox;
            }
        }

        /// <summary>
        /// 获取呈现给界面具体结果的数据内容。
        /// </summary>
        public virtual string Content
        {
            get
            {
                if (string.IsNullOrEmpty(m_data) || m_data.Trim().Length == 0)
                {
                    return "[]";
                }
                else
                {
                    return m_data;
                }
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 实例化。
        /// </summary>
        /// <param name="data">数据。</param>
        private Combobox(string data)
        {
            m_data = data;
        }
        #endregion

        /// <summary>
        /// 获取 Combobox 对象实例。
        /// </summary>
        /// <returns></returns>
        public static Combobox GetInstance()
        {
            return new Combobox(null);
        }

        /// <summary>
        /// 获取 Combobox 对象实例。
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static Combobox GetInstance(System.Collections.IEnumerable items)
        {
            string content = (items == null ? null : JsonHelper.JsonSerializer(items));
            return new Combobox(content);
        }

        /// <summary>
        /// 获取 Combobox 对象实例。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Combobox GetInstance<T>(IEnumerable<T> items)
        {
            string content = (items == null ? null : JsonHelper.JsonSerializer(items));
            return new Combobox(content);
        }
    }
}
