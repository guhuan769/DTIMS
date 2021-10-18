using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DTIMS.Comm.Entity
{
    /// <summary>
    /// 分页存储过程返回结果实体
    /// </summary>
    public class ProcResultEntity
    {
        public bool mSuccess = false;

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success
        {
            get
            {
                return this.mSuccess;
            }

            set
            {
                this.mSuccess = value;
            }
        }

        /// <summary>
        /// 详细信息，错误时返回所有错误信息
        /// </summary>
        public string Descr { get; set; }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        public DataSet dataSet { get; set; }

        /// <summary>
        /// 返回DataTable
        /// </summary>
        public DataTable dataTable { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurPageIndex { get; set; }

        /// <summary>
        /// 返回总行数
        /// </summary>
        public int TotalRows { get; set; }
    }
}
