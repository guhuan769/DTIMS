using System;
using System.Collections.Generic;
using System.Text;

namespace DTIMS.Comm.Entity
{
    /// <summary>
    /// 分页存储过程参数实体
    /// </summary>
    public class ProcParaEntity
    {
        public ProcParaEntity()
        {
            Where = "";
        }

        /// <summary>
        /// 查询的表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 关键字，多个关键字用,号分开
        /// </summary>
        public string PrimaryKey{ get; set; }

        /// <summary>
        /// 每页显示数据个数，默认为10行数据
        /// </summary>
        public int PageSize{ get; set; }

        /// <summary>
        /// 要查询的页号
        /// </summary>
        public int PageIndex{ get; set; }

        /// <summary>
        /// 关键字，多个关键字用,号分开
        /// </summary>
        public string Fileds{ get; set; }

        /// <summary>
        /// Where条件，不包括Where关键字
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// 排序条件，不包括order by 关键字
        /// </summary>
        public string Order { get; set; }
    }
}
