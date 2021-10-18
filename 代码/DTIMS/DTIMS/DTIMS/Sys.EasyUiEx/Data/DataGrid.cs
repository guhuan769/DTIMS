using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using BJ.Sys.EasyUiEx.Helper;

namespace BJ.Sys.EasyUiEx.Data
{
    /// <summary>
    /// Jquery EasyUi DataGrid 的基本数据类。
    /// </summary>
    public class DataGrid : IData
    {
        #region 全局变量
        // 生成的数据列表字符串。
        private string m_rowsData = "";
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置数据的总数。
        /// </summary>
        public int Total { get; set; }

        #endregion

        #region IData接口成员
        /// <summary>
        /// 获取界面使用数据的 EasyUi 控件类型。
        /// </summary>
        public virtual EasyUiTypeEnum EasyUiType
        {
            get
            {
                return EasyUiTypeEnum.DataGrid;
            }
        }

        /// <summary>
        /// 获取呈现给界面具体结果的数据内容。
        /// </summary>
        public virtual string Content
        {
            get
            {
                if (string.IsNullOrEmpty(m_rowsData) || m_rowsData.Trim().Length == 0)
                {
                    return "{ \"total\": 0, \"rows\": [] }";
                }
                else
                {
                    return ("{\"total\":" + this.Total + ",\"rows\":" + this.m_rowsData + "}");
                }
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 实例化。
        /// </summary>
        /// <param name="data">数据。</param>
        /// <param name="total">数据总条数。</param>
        private DataGrid(string data, int total)
        {
            m_rowsData = data;
            Total = total;
        }
        #endregion

        #region 创建实例
        /// <summary>
        /// 获取 DataGrid 对象实例。
        /// </summary>
        /// <returns></returns>
        public static DataGrid GetInstance()
        {
            return new DataGrid(null, 0);
        }

        /// <summary>
        /// 获取 DataGrid 对象实例。
        /// </summary>
        /// <returns></returns>
        public static DataGrid GetInstance(DataTable table)
        {
            string content = (table == null ? null : JsonHelper.JsonSerializer(table));
            return new DataGrid(content, table.Rows.Count);
        }

        /// <summary>
        /// 获取 DataGrid 对象实例。
        /// </summary>
        /// <returns></returns>
        public static DataGrid GetInstance(ICollection items)
        {
            string content = (items == null ? null : JsonHelper.JsonSerializer(items));
            return new DataGrid(content, items.Count);
        }

        /// <summary>
        /// 获取 DataGrid 对象实例。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataGrid GetInstance<T>(ICollection<T> items)
        {
            string content = (items == null ? null : JsonHelper.JsonSerializer(items));
            return new DataGrid(content, items.Count);
        }

        /// <summary>
        /// 获取 DataGrid 对象实例。
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataGrid GetInstance(System.Collections.IEnumerable items, int total)
        {
            string content = (items == null ? null : JsonHelper.JsonSerializer(items));
            return new DataGrid(content, (items == null ? 0 : total));
        }

        /// <summary>
        /// 获取 DataGrid 对象实例。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataGrid GetInstance<T>(IEnumerable<T> items, int total)
        {
            string content = (items == null ? null : JsonHelper.JsonSerializer(items));
            return new DataGrid(content, (items == null ? 0 : total));
        }

        #endregion

    }
}
