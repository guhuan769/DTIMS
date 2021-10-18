using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Xml;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Web;

namespace DTIMS.DataAccess
{
    public delegate void LogHeader(object obj1, string name);
    /// <summary>
    /// common 的摘要说明。
    /// </summary>

    #region 字段属性管理
    public class Field
    {
        #region 属性
        private String mName = null;					//字段名称,一般用作DataGrid的绑定列字段
        private String mTitle = null;					//字段标题,一般用作DataGrid的列标题
        private Int32 mAlign = 0;						//对齐方式,默认为靠右对齐.如果为0表示靠右对齐,为1表示靠左对齐,为2时表示居中对齐
        public bool Visible = true;					//是否隐藏
        public bool IsKey = false;				//是否是主键

        public String Name
        {
            get
            {
                return mName.Trim();
            }
        }
        public String Title
        {
            get
            {
                return mTitle.Trim();
            }
        }
        /// <summary>
        /// 如果为0表示靠右对齐,为1表示靠左对齐,为2时表示居中对齐
        /// </summary>
        public Int32 Align
        {
            get
            {
                return this.mAlign;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 字段属性管理
        /// </summary>
        /// <param name="name">列名称</param>
        /// <param name="title">列标题</param>
        /// <param name="isVisible">是否隐藏</param>
        public Field(string name, string title, bool isVisible)
        {
            if (name != "" && name != null && title != "" && title != null)
            {
                this.mName = name.Trim();
                this.mTitle = title.Trim();
                this.Visible = isVisible;
            }
            else
            {
                throw (new Exception("字段名称和说明不能为空！"));
            }
        }

        /// <summary>
        /// 字段属性管理
        /// </summary>
        /// <param name="name">列名称</param>
        /// <param name="title">列标题</param>
        /// <param name="isVisible">是否隐藏</param>
        public Field(string name, string title, bool isVisible, Int32 align)
        {
            if (name != "" && name != null && title != "" && title != null)
            {
                this.mName = name.Trim();
                this.mTitle = title.Trim();
                this.Visible = isVisible;
                this.mAlign = align;
            }
            else
            {
                throw (new Exception("字段名称和说明不能为空！"));
            }
        }

        /// <summary>
        /// 字段属性管理
        /// </summary>
        /// <param name="name">列名称</param>
        /// <param name="title">列标题</param>
        /// <param name="isVisible">是否隐藏</param>
        public Field(string name, string title, bool isVisible, bool IsKey, Int32 align)
        {
            if (name != "" && name != null && title != "" && title != null)
            {
                this.mName = name.Trim();
                this.mTitle = title.Trim();
                this.Visible = isVisible;
                this.mAlign = align;
                this.IsKey = IsKey;
            }
            else
            {
                throw (new Exception("字段名称和说明不能为空！"));
            }
        }
        #endregion
    }

    public class Fields : ICollection, IEnumerable
    {
        #region Fields
        private ArrayList fieldList;
        #endregion

        #region Properties
        public DataTable Table
        {
            get
            {
                return this.GetDataTable();
            }
        }
        #endregion

        #region Methods
        private DataTable GetDataTable()
        {
            DataTable dt = new DataTable();
            int index = fieldList.Count;
            dt.Columns.Add("Name", System.Type.GetType("System.String"));
            dt.Columns.Add("Title", System.Type.GetType("System.String"));
            dt.Columns.Add("Length", System.Type.GetType("System.String"));
            for (int i = 0; i < index; i++)
            {
                DataRow dr = dt.NewRow();
                Field temp = (Field)fieldList[i];
                dr[0] = temp.Name;
                dr[1] = temp.Title;
                if (temp.Visible)
                {
                    dr[2] = "1";
                }
                else
                {
                    dr[2] = "0";
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int index = fieldList.Count;
            for (int i = 0; i < index; i++)
            {
                Field temp = (Field)fieldList[i];
                sb.Append(temp.Title + ",");
            }
            if (index > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString().Trim();
        }

        public void Add(Field field)
        {
            this.fieldList.Add(field);
        }
        #endregion

        #region 构造函数
        public Fields()
        {
            this.fieldList = new ArrayList();
        }
        #endregion

        #region IEnumerable 成员

        public IEnumerator GetEnumerator()
        {
            // TODO:  添加 Fields.GetEnumerator 实现
            return this.fieldList.GetEnumerator();
        }

        #endregion

        #region ICollection 成员

        public bool IsSynchronized
        {
            get
            {
                // TODO:  添加 Fields.IsSynchronized getter 实现
                return this.fieldList.IsSynchronized;
            }
        }

        public int Count
        {
            get
            {
                // TODO:  添加 Fields.Count getter 实现
                return this.fieldList.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            // TODO:  添加 Fields.CopyTo 实现
            this.fieldList.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                // TODO:  添加 Fields.SyncRoot getter 实现
                return this.fieldList.SyncRoot;
            }
        }

        public Field this[Int32 index]
        {
            get
            {
                if (index >= this.fieldList.Count)
                {
                    throw (new Exception("序号不能大于集合大小!"));
                }
                return (Field)this.fieldList[index];
            }
            set
            {
                if (value == null)
                {
                    throw (new Exception("元素不能为空!"));
                }
                this.fieldList[index] = value;
            }
        }
        #endregion
    }
    #endregion

    #region 接钮模板列
    public class ButtonTemplate : System.Web.UI.ITemplate
    {
        string funName = null;
        string itemName = null;

        public ButtonTemplate(string name, string func)
        {
            itemName = name;
            funName = func.Trim();
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            System.Web.UI.WebControls.LinkButton lb = new System.Web.UI.WebControls.LinkButton();
            lb.Text = itemName;
            lb.Attributes.Add("onClick", "return " + funName + "();");
            lb.CssClass = "button2d";
            container.Controls.Add(lb);
        }
    }

    public class CheckBoxTemplate : System.Web.UI.ITemplate
    {
        string funName = null;

        public CheckBoxTemplate()
        {

        }

        public CheckBoxTemplate(string func)
        {
            funName = func.Trim();
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            System.Web.UI.WebControls.CheckBox lb = new System.Web.UI.WebControls.CheckBox();
            lb.ID = "CheckBoxNet";
            if (this.funName != null)
            {
                lb.Attributes.Add("onClick", "return " + funName + "();");
            }
            lb.CssClass = "button2d";
            container.Controls.Add(lb);
        }
    }

    public class RadioButtonTemplate : System.Web.UI.ITemplate
    {
        string funName = null;

        public RadioButtonTemplate()
        {

        }

        public RadioButtonTemplate(string func)
        {
            funName = func.Trim();
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            System.Web.UI.WebControls.RadioButton lb = new System.Web.UI.WebControls.RadioButton();
            lb.GroupName = "check";
            if (this.funName != null)
            {
                lb.Attributes.Add("onClick", "return " + funName + "();");
            }
            lb.CssClass = "button2d";
            container.Controls.Add(lb);
        }
    }

    public class LiteralControlTemplate : System.Web.UI.ITemplate
    {
        string name = null;

        public LiteralControlTemplate(string name)
        {
            this.name = name.Trim();
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            System.Web.UI.LiteralControl lb = new System.Web.UI.LiteralControl(name);
            container.Controls.Add(lb);
        }
    }

    #endregion

    #region 多项选择的基类
    public abstract class MultipleSelect
    {
        #region 属性
        private String mDataBaseName = null;
        private Fields mDataFields = null;			//全部的属性字段
        private Fields mDataKeyFields = null;		//主键属性字段

        /// <summary>
        /// 数据库实例名称
        /// </summary>
        public String DataBaseName
        {
            get
            {
                return this.mDataBaseName;
            }
            set
            {
                if (value != null && value != "")
                {
                    this.mDataBaseName = value.Trim();
                }
                else
                {
                    this.mDataBaseName = null;
                }
            }
        }

        /// <summary>
        /// 字段对象集合
        /// </summary>
        public Fields DataFields
        {
            get
            {
                return this.mDataFields;
            }
            set
            {
                this.mDataFields = value;
            }
        }

        /// <summary>
        /// 字段关健字对象集合
        /// </summary>
        public Fields DataKeyFields
        {
            get
            {
                return this.mDataKeyFields;
            }
            set
            {
                this.mDataKeyFields = value;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 得到全部数据集
        /// </summary>
        /// <param name="key">关健字,如果多个关健字则以:号分隔</param>
        /// <returns></returns>
        public virtual DataTable ListAll(String key)
        {
            return null;
        }
        public virtual DataTable ListAllDetail(String key, String areaid, String type)
        {
            return null;
        }
        /// <summary>
        /// 得到全部数据集
        /// </summary>
        /// <returns>以默认方式得到全部</returns>
        public virtual DataTable ListAll()
        {
            return null;
        }
        public virtual DataTable ListAllDetail(String areaid, String type)
        {
            return null;
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dataBaseName">数据库名称,由类中决定是否为总库,如果为总库时由类自己取得</param>
        public MultipleSelect(String dataBaseName)
        {
            this.mDataFields = new Fields();
            this.mDataKeyFields = new Fields();
            this.mDataBaseName = dataBaseName;
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MultipleSelect()
        {
            this.mDataFields = new Fields();
            this.mDataKeyFields = new Fields();
        }
        #endregion
    }
    #endregion

    #region 下载的基类

    public class UpLoadObj
    {
        #region 属性
        private String mDataBaseName = null;
        private Fields mDataFields = null;			//全部的属性字段
        private Fields mDataKeyFields = null;		//主键属性字段
        protected String mBoardFilePath = null;		//模板文件路径
        protected Int32 mColsNum = 0;					//导入的字段数
        protected Int32 mStartCols = 0;					//开始列
        protected Int32 mStartRows = 0;					//开始行
        protected Int32 mPrimayColIndex = 0;			//主健列序号
        protected String mBatchNo = null;				//批次号
        protected String mAreaIdList = null;			//拥有权限的全部地区ID

        public Int32 PrimayColIndex
        {
            get
            {
                return this.mPrimayColIndex;
            }
            set
            {
                this.mPrimayColIndex = value;
            }
        }
        public String AreaIdList
        {
            get
            {
                return this.mAreaIdList;
            }
            set
            {
                this.mAreaIdList = value.Trim();
            }
        }
        public String BoardFilePath
        {
            get
            {
                return this.mBoardFilePath;
            }
        }
        public String BatchNo
        {
            get
            {
                return this.mBatchNo;
            }
        }
        public Int32 StartCols
        {
            get
            {
                return this.mStartCols;
            }
            set
            {
                this.mStartCols = value;
            }
        }
        public Int32 StartRows
        {
            get
            {
                return this.mStartRows;
            }
            set
            {
                this.mStartRows = value;
            }
        }
        public Int32 ColsNum
        {
            get
            {
                return this.mColsNum;
            }
            set
            {
                this.mColsNum = value;
            }
        }
        /// <summary>
        /// 数据库实例名称
        /// </summary>
        public String DataBaseName
        {
            get
            {
                return this.mDataBaseName;
            }
            set
            {
                if (value != null && value != "")
                {
                    this.mDataBaseName = value.Trim();
                }
                else
                {
                    this.mDataBaseName = null;
                }
            }
        }

        /// <summary>
        /// 字段对象集合
        /// </summary>
        public Fields DataFields
        {
            get
            {
                return this.mDataFields;
            }
            set
            {
                this.mDataFields = value;
            }
        }

        /// <summary>
        /// 字段关健字对象集合
        /// </summary>
        public Fields DataKeyFields
        {
            get
            {
                return this.mDataKeyFields;
            }
            set
            {
                this.mDataKeyFields = value;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 得到全部数据集
        /// </summary>
        /// <returns>以默认方式得到全部</returns>
        public virtual DataTable ListAll(String batchNo)
        {
            return null;
        }

        public virtual DataTable ListAll()
        {
            return null;
        }

        public virtual void InsertData(String[,] data, String batchNo)
        {
        }

        public virtual void Delete(String batchNo)
        {

        }

        public virtual void UpLoad(String[,] data)
        {
            String batchNo = GetBatchNo();
            try
            {
                InsertData(data, batchNo);

                mBatchNo = batchNo.Trim();
            }
            catch (Exception ex)
            {
                Delete(batchNo);
                throw (new Exception("保存导入数据出错,原因:" + ex.Message));
            }
        }

        private String GetBatchNo()
        {
            String loadNo = DateTime.Now.Ticks.ToString();
            if (loadNo.Length > 11)
            {
                loadNo = loadNo.Substring(0, 11);
            }
            return loadNo;
        }

        public static void Delete(String batchNo, String dataBaseName)
        {
            DbCommand cmdSelect;
            Microsoft.Practices.EnterpriseLibrary.Data.Database mDataBase =
                DatabaseFactory.CreateDatabase(dataBaseName);
            String sqlstr = "delete CollectionLoadDataMap where LOAD_No=@no";

            cmdSelect = mDataBase.GetSqlStringCommand(sqlstr);
            mDataBase.AddInParameter(cmdSelect, "@no", System.Data.DbType.String, batchNo);

            try
            {
                mDataBase.ExecuteNonQuery(cmdSelect);
            }
            catch (Exception e)
            {
                throw (new Exception("删除导入的临时数据出错，" + e.Message));
            }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dataBaseName">数据库名称,由类中决定是否为总库,如果为总库时由类自己取得</param>
        public UpLoadObj(String dataBaseName)
        {
            this.mDataFields = new Fields();
            this.mDataKeyFields = new Fields();
            this.mDataBaseName = dataBaseName;
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public UpLoadObj()
        {
            this.mDataFields = new Fields();
            this.mDataKeyFields = new Fields();
        }
        #endregion
    }
    #endregion

    #region 工单相关状态

    public enum BillBackFlag
    {
        NoBack = 1,//未回单
        Succeed = 2,//回成功
        Fail = 3	//回失败

    }

    /// <summary>
    /// 回单模式
    /// </summary>
    public enum BackMode
    {
        Manual = 1,
        Succeed = 2,
        Fail = 3
    }

    /// <summary>
    /// 执行结果
    /// </summary>
    public enum ResultFlag
    {
        /*
         *  1:正在处理中(UT的执行需要这个中间取值记录状态)
           2:成功
           3:失败
           null:待执行		 
         * */
        Process = 1,
        Succeed = 2,
        Fail = 3
    }

    /// <summary>
    /// 派单和回单处理模式
    /// </summary>
    public enum ProcessMode
    {
        Auto = 2, //自动处理
        Manual = 3 //人工处理
    }

    /// <summary>
    /// 工单状态
    /// </summary>
    public enum BillStatus
    {
        /// <summary>
        /// 未分析
        /// </summary>
        UnAnalysis = 1,
        /// <summary>
        /// 分析过程完成
        /// </summary>
        AnalysisEnd = 2,
        /// <summary>
        /// 所有分析完成，等待执行
        /// </summary>
        WaitExecut = 15,
        /// <summary>
        /// 执行完成
        /// </summary>
        Executed = 17,
        /// <summary>
        /// 等待回单
        /// </summary>
        WaitBack = 18,
        /// <summary>
        /// 回单处理
        /// </summary>
        Back = 19,
        /// <summary>
        /// 回单完成
        /// </summary>
        BackEnd = 20,
        /// <summary>
        /// 资源更新
        /// </summary>
        UpdateResource = 21
    }

    /// <summary>
    /// 端口状态
    /// </summary>
    public enum PortStatus
    {
        UnUse = 1,
        Used = 2
    }

    /// <summary>
    /// 设备状态
    /// </summary>
    public enum ControlStatus
    {
        Normal = 1,//正常
        Pause = 2,//暂停
        Fail = 3,//失败
        Succeed = 4,//成功	
        Delayed = 5 //延时（帐期）
    }

    /// <summary>
    /// 确定机框的模式
    /// </summary>
    public enum FrameConfirmMode
    {
        BureauProjectATUC = 1,
        BureauATUC = 2,
        ProjectATUC = 3,
        ATUC = 4

    }
    #endregion

    /// <summary>
    /// Common 的摘要说明。
    /// </summary>
    public class Common
    {
        #region 由DataTable转化为ArrayList集合
        /// <summary>
        /// 由DataTable转化为ArrayList集合
        /// </summary>
        /// <param name="dt">数据表格</param>
        /// <param name="rowName">字段名称</param>
        /// <returns></returns>
        public static ArrayList ToArrayList(DataTable dt, String rowName)
        {
            ArrayList al = new ArrayList();
            foreach (DataRow dr in dt.Rows)
            {
                al.Add(dr[rowName].ToString().Trim());
            }
            return al;
        }
        #endregion

        #region 由DataTable转化为字符串
        /// <summary>
        /// 由DataTable转化为字符串,字符串以","号分隔
        /// </summary>
        /// <param name="dt">要转化的数据表格</param>
        /// <param name="rowName">字段名称</param>
        /// <returns></returns>
        public static String ToStringList(DataTable dt, String rowName)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(dr[rowName].ToString().Trim() + ",");
            }

            if (sb.Length > 0)
            {
                return sb.Remove(sb.Length - 1, 1).ToString();
            }
            return sb.ToString();
        }

        /// <summary>
        /// 由DataTable转化为字符串,字符串以","号分隔
        /// </summary>
        /// <param name="dt">要转化的数据表格</param>
        /// <param name="rowIndex">字段序号</param>
        /// <returns></returns>
        public static String ToStringList(DataTable dt, Int32 rowIndex)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(dr[rowIndex].ToString().Trim() + ",");
            }

            if (sb.Length > 0)
            {
                return sb.Remove(sb.Length - 1, 1).ToString();
            }
            return sb.ToString();
        }
        #endregion

        #region 导出给定的文件
        public static void UpLoadFile(System.Web.UI.Page page, string fileName, bool ret)
        {
            string fileName1 = page.Server.MapPath(fileName);
            page.Response.WriteFile(fileName1, true);
            String name = System.IO.Path.GetFileName(fileName1);
            if (ret)
            {
                try
                {
                    System.IO.File.Delete(fileName1);
                }
                catch
                {
                }
            }
            page.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + System.Web.HttpUtility.UrlEncode(name) + "\"");
            page.Response.ContentType = "application/octet-stream";
            page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            page.Response.End();
            page.Response.Close();
        }
        #endregion

        #region 写Cookies
        /// <summary>
        /// 写Cookies到客户机上
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="strValue">值</param>
        public static void WriteCookies(string key, string strValue)
        {
            //写cookies
            if (System.Web.HttpContext.Current.Request.Browser.Cookies)
            {
                string t = HttpUtility.UrlEncode(strValue);
                System.Web.HttpCookie htck = new HttpCookie(key, t);
                htck.Expires = DateTime.Now.AddDays(90);
                System.Web.HttpContext.Current.Response.Cookies.Add(htck);
            }
            else
            {
                throw (new Exception("浏览器不支持Cookies，请把本站点设置为受信任站点!"));
            }
        }
        #endregion

        #region 读Cookies
        /// <summary>
        /// 写Cookies到客户机上
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="strValue">值</param>
        public static string ReadCookies(string key)
        {
            //写cookies
            string str = null;
            if (System.Web.HttpContext.Current.Request.Browser.Cookies)
            {
                System.Web.HttpCookie httpc = System.Web.HttpContext.Current.Request.Cookies[key];

                if (httpc != null)
                {
                    str = HttpUtility.UrlDecode(httpc.Value);
                }
            }
            else
            {
                throw (new Exception("你的浏览器不支持Cookies，可能有些功能不能使用！"));
            }

            return str;
        }
        #endregion

        #region 资源更新中更新下载表状态

        /// <summary>
        /// 修改状态,提交后台重新处理
        /// </summary>
        /// <param name="dbName"></param>
        public static void ModifyStatus(string dbName, string tableName, string MainCol, DnStatus status, string id)
        {
            try
            {
                Database mDataBase = DatabaseFactory.CreateDatabase(dbName);
                string str = "update " + tableName + " set Dn_Status=@Dn_Status where " + MainCol + " in (" + id + ")";
                DbCommand dbw = mDataBase.GetSqlStringCommand(str);

                mDataBase.AddInParameter(dbw, "@Dn_Status", DbType.Int16, status);

                mDataBase.ExecuteDataSet(dbw);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  修改状态,提交后台重新处理
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tableName"></param>
        /// <param name="MainCol"></param>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <param name="loginName"></param>
        /// <param name="time"></param>
        public static void ModifyStatus(string dbName, string tableName, string MainCol, DnStatus status, string id, string loginName, DateTime time)
        {
            try
            {
                Database mDataBase = DatabaseFactory.CreateDatabase(dbName);
                string str = "update " + tableName + " set Dn_Status=@Dn_Status,OPER_ID=@OPER_LOGIN,Dn_TakeDate=@Dn_TakeDate where " + MainCol + " in (" + id + ")";
                DbCommand dbw = mDataBase.GetSqlStringCommand(str);

                mDataBase.AddInParameter(dbw, "@Dn_Status", DbType.Int16, status);
                mDataBase.AddInParameter(dbw, "@OPER_LOGIN", DbType.String, loginName);
                mDataBase.AddInParameter(dbw, "@Dn_TakeDate", DbType.DateTime, time);

                mDataBase.ExecuteDataSet(dbw);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改状态,提交后台重新处理
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="trans"></param>
        /// <param name="status"></param>
        /// <param name="id"></param>
        public static void ModifyStatus(string dbName, IDbTransaction trans, string tableName, string MainCol, DnStatus status, string id)
        {
            try
            {
                Database mDataBase = DatabaseFactory.CreateDatabase(dbName);
                string str = "update " + tableName + " set Dn_Status=@Dn_Status where " + MainCol + " in (" + id + ")";
                DbCommand dbw = mDataBase.GetSqlStringCommand(str);

                mDataBase.AddInParameter(dbw, "@Dn_Status", DbType.Int16, status);

                mDataBase.ExecuteDataSet(dbw);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改状态,提交后台重新处理
        /// </summary>
        /// <param name="mDataBase"></param>
        /// <param name="trans"></param>
        /// <param name="status"></param>
        /// <param name="id"></param>
        public static void ModifyStatus(Database mDataBase, IDbTransaction trans, string tableName, string MainCol, DnStatus status, DnFlag flag, string loginName, string id)
        {
            try
            {
                string str = "update " + tableName + " set Dn_Status=@Dn_Status,Dn_Flag=@Dn_Flag,OPER_ID=@OPER_LOGIN where " + MainCol + " in (" + id + ")";
                DbCommand dbw = mDataBase.GetSqlStringCommand(str);

                mDataBase.AddInParameter(dbw, "@Dn_Status", DbType.Int16, status);
                mDataBase.AddInParameter(dbw, "@Dn_Flag", DbType.Int16, flag);
                mDataBase.AddInParameter(dbw, "@OPER_LOGIN", DbType.String, loginName);

                mDataBase.ExecuteDataSet(dbw);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ModifyStatus(Database mDataBase, IDbTransaction trans, string tableName, string MainCol, DnStatus status, DnFlag flag, string loginName, string id, DateTime time)
        {
            try
            {
                string str = "update " + tableName + " set Dn_Status=@Dn_Status,Dn_Flag=@Dn_Flag,OPER_ID=@OPER_LOGIN,Dn_TakeDate=@Dn_TakeDate where " + MainCol + " in (" + id + ")";
                DbCommand dbw = mDataBase.GetSqlStringCommand(str);

                mDataBase.AddInParameter(dbw, "@Dn_Status", DbType.Int16, status);
                mDataBase.AddInParameter(dbw, "@Dn_Flag", DbType.Int16, flag);
                mDataBase.AddInParameter(dbw, "@OPER_LOGIN", DbType.String, loginName);
                mDataBase.AddInParameter(dbw, "@Dn_TakeDate", DbType.DateTime, time);

                mDataBase.ExecuteDataSet(dbw);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 整理需要在页面显示的提示信息
        public static string CheckSpecialString(string str)
        {
            string strTemp = null;
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i].ToString().Trim() != "<") && (str[i].ToString().Trim() != ">")
                   && (str[i].ToString().Trim() != "/") && (str[i].ToString().Trim() != ":") && (str[i].ToString().Trim() != "'"))
                {
                    strTemp += str[i].ToString().Trim();
                }
            }
            return strTemp;
        }
        #endregion


        #region Excel导出
        /// <summary>
        /// 删除当前以前的临时文件
        /// </summary>
        /// <param name="txtDir"></param>
        public void DeleteFile(String txtDir)
        {
            try
            {
                //判断指定的源文件目录是否存在
                //string txtDir = Request.MapPath(Request.ApplicationPath) + "\\temp";
                if (!System.IO.Directory.Exists(txtDir))
                {
                    throw new Exception(string.Format("未找到文件目录({0})!", txtDir));
                }

                //删除当天以前的文件
                foreach (string fileName in System.IO.Directory.GetFiles(txtDir, "*.*"))
                {
                    if (!fileName.Contains(DateTime.Now.ToString("yyyyMMdd")))
                    {
                        if (System.IO.File.Exists(fileName))
                        {
                            System.IO.File.Delete(fileName);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public void WriteToExcel(DataTable dtData, String[] headArray, String[] DataArray, String FileName)
        {
            DeleteFile(FileName.Substring(0, FileName.LastIndexOf("\\")));
            System.Data.DataTable dt = dtData;
            System.IO.FileStream objFileStream;
            System.IO.StreamWriter objStreamWriter;
            string strLine = "";
            objFileStream = new System.IO.FileStream(FileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            objStreamWriter = new System.IO.StreamWriter(objFileStream, System.Text.Encoding.Unicode);
            for (int i = 0; i < headArray.Length; i++)
            {
                strLine = strLine + headArray[i].ToString() + Convert.ToChar(9);
            }
            objStreamWriter.WriteLine(strLine);
            strLine = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                strLine = strLine + (i + 1) + Convert.ToChar(9);
                for (int j = 1; j < DataArray.Length; j++)
                {
                    strLine = strLine + dt.Rows[i][DataArray[j].ToString()].ToString() + Convert.ToChar(9);

                }
                objStreamWriter.WriteLine(strLine);
                strLine = "";
            }
            objStreamWriter.Close();
            objFileStream.Close();

        }

        public void WriteToExcel(DataTable dtData, String[] headArray, String[] DataArray, String FileName, string temp)
        {
            DeleteFile(FileName.Substring(0, FileName.LastIndexOf("\\")));
            System.Data.DataTable dt = dtData;
            System.IO.FileStream objFileStream;
            System.IO.StreamWriter objStreamWriter;
            string strLine = "";
            objFileStream = new System.IO.FileStream(FileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            objStreamWriter = new System.IO.StreamWriter(objFileStream, System.Text.Encoding.Unicode);
            for (int i = 0; i < headArray.Length; i++)
            {
                strLine = strLine + headArray[i].ToString() + Convert.ToChar(9);
            }
            objStreamWriter.WriteLine(strLine);
            strLine = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                //strLine = strLine + (i + 1) + Convert.ToChar(9);
                for (int j = 0; j < DataArray.Length; j++)
                {
                    //                   strLine = strLine + dt.Rows[i][DataArray[j].ToString()].ToString() + Convert.ToChar(9);
                    strLine += dt.Rows[i][DataArray[j].ToString()].ToString() + Convert.ToChar(9);
                }
                objStreamWriter.WriteLine(strLine);
                strLine = "";
            }
            objStreamWriter.Close();
            objFileStream.Close();

        }

        #endregion

        #region Text导出
        public void WriteToText(DataTable dtData, String[] headArray, String[] DataArray, String FileName)
        {
            DeleteFile(FileName.Substring(0, FileName.LastIndexOf("\\")));
            System.Data.DataTable dt = dtData;
            System.IO.FileStream objFileStream;
            System.IO.StreamWriter objStreamWriter;
            string strLine = "";
            objFileStream = new System.IO.FileStream(FileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            objStreamWriter = new System.IO.StreamWriter(objFileStream, System.Text.Encoding.Unicode);
            for (int i = 0; i < headArray.Length; i++)
            {
                strLine = strLine + headArray[i].ToString() + "\t";
            }
            objStreamWriter.WriteLine(strLine);
            strLine = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                for (int j = 0; j < DataArray.Length; j++)
                {
                    strLine += dt.Rows[i][DataArray[j].ToString()].ToString() + "\t";// Convert.ToChar(9);

                }
                objStreamWriter.WriteLine(strLine);
                strLine = "";
            }
            objStreamWriter.Close();
            objFileStream.Close();

        }
        #endregion

        #region 执行分页存储过程

        /// <summary>
        /// 执行分页存储过程
        /// </summary>
        /// <param name="PageSize">每页显示数据个数</param>
        /// <param name="page">要查询的页号</param>
        /// <param name="tabName">查询的表名</param>
        /// <param name="primarykey">关键字，多个关键字用,号分开</param>
        /// <param name="fileds">要查询的列</param>
        /// <param name="where">where条件，不包括where关键字</param>
        /// <param name="orderby">排序条件，不包括order by 关键字</param>
        /// <param name="dataCount">返回总数据条数</param>
        /// <param name="curPageIndex">返回当前页码</param>
        /// <returns></returns>
        public DataTable Query(int PageSize, int page, string tabName, string primarykey, string fileds, string where, string orderby, out int dataCount, out int curPageIndex)
        {
            DataTable table = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                DbCommand dbcomm = db.GetStoredProcCommand("proc_GetRecordFromPage");
                db.AddInParameter(dbcomm, "@tblName", System.Data.DbType.String, tabName);
                db.AddInParameter(dbcomm, "@primarykey", System.Data.DbType.String, primarykey);
                db.AddInParameter(dbcomm, "@PageSize", System.Data.DbType.Int32, PageSize);
                db.AddInParameter(dbcomm, "@PageIndex", System.Data.DbType.Int32, page);
                db.AddInParameter(dbcomm, "@fileds", System.Data.DbType.String, fileds);
                db.AddInParameter(dbcomm, "@strWhere", System.Data.DbType.String, where);
                db.AddInParameter(dbcomm, "@order", System.Data.DbType.String, orderby);
                db.AddOutParameter(dbcomm, "@dataCount", System.Data.DbType.Int32, 100);
                db.AddOutParameter(dbcomm, "@curPageIndex", System.Data.DbType.Int32, 100);
                //执行数据查询
                table = db.ExecuteDataSet(dbcomm).Tables[0];
                object outParam_dataCount = db.GetParameterValue(dbcomm, "dataCount");
                object outParam_curPageIndex = db.GetParameterValue(dbcomm, "curPageIndex");
                //获取总数据条数
                if (outParam_dataCount != null)
                {
                    dataCount = Convert.ToInt32(outParam_dataCount.ToString());
                }
                else
                {
                    dataCount = 0;
                }
                if (outParam_curPageIndex != null)
                {
                    curPageIndex = Convert.ToInt32(outParam_curPageIndex.ToString());
                }
                else
                {
                    curPageIndex = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return table;
        }

        #endregion

        #region 获取所有记录

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public DataTable QueryAll(string table,string where)
        {
            DataTable dt = null;
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cmd = null;
            string sql = "select * from " + table + " where " + where;
            cmd = db.GetSqlStringCommand(sql);
            try
            {
                dt = db.ExecuteDataSet(cmd).Tables[0];
            }
            catch (Exception ex)
            {
                throw (new Exception("获取记录出错,原因:" + ex.Message));
            }
            return dt;
        }

        #endregion
    }

    #region 下载信息枚举

    /// <summary>
    /// 资源更新处理状态
    /// </summary>
    public enum DnStatus
    {
        Download = 1,
        Disposing = 2,
        WaitConfirm = 3,
        Complete = 4
    }

    /// <summary>
    /// 资源更新处理状态
    /// </summary>
    public enum DnFlag
    {
        Fail = 1,
        AutoSucc = 2,
        ManuelSucc = 3
    }

    #endregion
}
