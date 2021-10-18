using System;
using System.Collections.Generic;
using System.Text;

using BJ.Sys.EasyUiEx.Helper;

namespace BJ.Sys.EasyUiEx.Data
{
    /// <summary>
    /// Jquery EasyUi 通用的对象数据类。
    /// </summary>
    public class ObjectInstance : IData
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
                return EasyUiTypeEnum.None;
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
                    return "{}";
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
        private ObjectInstance(string data)
        {
            m_data = data;
        }
        #endregion

        #region 创建实例
        /// <summary>
        /// 获取 ObjectInstance 对象实例。
        /// </summary>
        /// <returns></returns>
        public static ObjectInstance GetInstance()
        {
            return new ObjectInstance(null);
        }

        /// <summary>
        /// 获取 ObjectInstance 对象实例。
        /// </summary>
        /// <returns></returns>
        public static ObjectInstance GetInstance(object obj)
        {
            string content = (obj == null ? null : JsonHelper.JsonSerializer(obj));
            return new ObjectInstance(content);
        }

        /// <summary>
        /// 获取 ObjectInstance 对象实例。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ObjectInstance GetInstance<T>(T obj)
        {
            string content = (obj == null ? null : JsonHelper.JsonSerializer(obj));
            return new ObjectInstance(content);
        }

        #endregion

    }
}
