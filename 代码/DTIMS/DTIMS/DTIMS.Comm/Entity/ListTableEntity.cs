using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.ComponentModel;

namespace DTIMS.Comm.Entity
{
    public class ListTableEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="NAME"></param>
        public ListTableEntity(string ID,string NAME)
        {
            this.listID = ID;
            this.listName = NAME;
        }

        private string listID = string.Empty;

        /// <summary>
        /// 列表ID
        /// <summary>
        public string ListID
        {
            get
            {
                return this.listID;
            }
            set
            {
                this.listID = value;
            }
        }

        private string listName = string.Empty;

        /// <summary>
        /// 列表名称
        /// </summary>
        public string ListName
        {
            get
            {
                return this.ListName;
            }
            set
            {
                this.ListName = value;
            }
        }

        private object dataSource;

        /// <summary>
        /// 数据源
        /// </summary>
        public object DataSource
        {
            get { return dataSource; }
            set
            {
                if (value != null)
                {
                    if (value is DataTable)
                    {
                        dataSource = new DataView(dataSource as DataTable);
                    }
                    else if (dataSource is IList)
                    {
                        dataSource = dataSource as IList;
                    }
                    else
                        dataSource = dataSource as IEnumerable;
                }
                dataSource = value;
            }
        }

        /// <summary>
        /// 将对象转换为枚举
        /// </summary>
        /// <param name="item"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object ConvertEnumerationItem(object item, string fieldName)
        {
            DataRow row = item as DataRow;
            if (row != null)
            {
                if (!string.IsNullOrEmpty(fieldName))
                {
                    if (row.Table.Columns.Contains(fieldName))
                        return row[fieldName];
                }
                return row[0];
            }
            else
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(item).Find(fieldName, true);
                if (descriptor != null)
                    return (descriptor.GetValue(item) ?? null);
            }
            return null;
        }
    }
}
