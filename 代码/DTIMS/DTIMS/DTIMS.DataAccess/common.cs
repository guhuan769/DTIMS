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
    /// common ��ժҪ˵����
    /// </summary>

    #region �ֶ����Թ���
    public class Field
    {
        #region ����
        private String mName = null;					//�ֶ�����,һ������DataGrid�İ����ֶ�
        private String mTitle = null;					//�ֶα���,һ������DataGrid���б���
        private Int32 mAlign = 0;						//���뷽ʽ,Ĭ��Ϊ���Ҷ���.���Ϊ0��ʾ���Ҷ���,Ϊ1��ʾ�������,Ϊ2ʱ��ʾ���ж���
        public bool Visible = true;					//�Ƿ�����
        public bool IsKey = false;				//�Ƿ�������

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
        /// ���Ϊ0��ʾ���Ҷ���,Ϊ1��ʾ�������,Ϊ2ʱ��ʾ���ж���
        /// </summary>
        public Int32 Align
        {
            get
            {
                return this.mAlign;
            }
        }
        #endregion

        #region ���캯��
        /// <summary>
        /// �ֶ����Թ���
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="title">�б���</param>
        /// <param name="isVisible">�Ƿ�����</param>
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
                throw (new Exception("�ֶ����ƺ�˵������Ϊ�գ�"));
            }
        }

        /// <summary>
        /// �ֶ����Թ���
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="title">�б���</param>
        /// <param name="isVisible">�Ƿ�����</param>
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
                throw (new Exception("�ֶ����ƺ�˵������Ϊ�գ�"));
            }
        }

        /// <summary>
        /// �ֶ����Թ���
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="title">�б���</param>
        /// <param name="isVisible">�Ƿ�����</param>
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
                throw (new Exception("�ֶ����ƺ�˵������Ϊ�գ�"));
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

        #region ���캯��
        public Fields()
        {
            this.fieldList = new ArrayList();
        }
        #endregion

        #region IEnumerable ��Ա

        public IEnumerator GetEnumerator()
        {
            // TODO:  ��� Fields.GetEnumerator ʵ��
            return this.fieldList.GetEnumerator();
        }

        #endregion

        #region ICollection ��Ա

        public bool IsSynchronized
        {
            get
            {
                // TODO:  ��� Fields.IsSynchronized getter ʵ��
                return this.fieldList.IsSynchronized;
            }
        }

        public int Count
        {
            get
            {
                // TODO:  ��� Fields.Count getter ʵ��
                return this.fieldList.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            // TODO:  ��� Fields.CopyTo ʵ��
            this.fieldList.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                // TODO:  ��� Fields.SyncRoot getter ʵ��
                return this.fieldList.SyncRoot;
            }
        }

        public Field this[Int32 index]
        {
            get
            {
                if (index >= this.fieldList.Count)
                {
                    throw (new Exception("��Ų��ܴ��ڼ��ϴ�С!"));
                }
                return (Field)this.fieldList[index];
            }
            set
            {
                if (value == null)
                {
                    throw (new Exception("Ԫ�ز���Ϊ��!"));
                }
                this.fieldList[index] = value;
            }
        }
        #endregion
    }
    #endregion

    #region ��ťģ����
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

    #region ����ѡ��Ļ���
    public abstract class MultipleSelect
    {
        #region ����
        private String mDataBaseName = null;
        private Fields mDataFields = null;			//ȫ���������ֶ�
        private Fields mDataKeyFields = null;		//���������ֶ�

        /// <summary>
        /// ���ݿ�ʵ������
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
        /// �ֶζ��󼯺�
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
        /// �ֶιؽ��ֶ��󼯺�
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

        #region ����
        /// <summary>
        /// �õ�ȫ�����ݼ�
        /// </summary>
        /// <param name="key">�ؽ���,�������ؽ�������:�ŷָ�</param>
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
        /// �õ�ȫ�����ݼ�
        /// </summary>
        /// <returns>��Ĭ�Ϸ�ʽ�õ�ȫ��</returns>
        public virtual DataTable ListAll()
        {
            return null;
        }
        public virtual DataTable ListAllDetail(String areaid, String type)
        {
            return null;
        }
        #endregion

        #region ���췽��
        /// <summary>
        /// ���췽��
        /// </summary>
        /// <param name="dataBaseName">���ݿ�����,�����о����Ƿ�Ϊ�ܿ�,���Ϊ�ܿ�ʱ�����Լ�ȡ��</param>
        public MultipleSelect(String dataBaseName)
        {
            this.mDataFields = new Fields();
            this.mDataKeyFields = new Fields();
            this.mDataBaseName = dataBaseName;
        }

        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>
        public MultipleSelect()
        {
            this.mDataFields = new Fields();
            this.mDataKeyFields = new Fields();
        }
        #endregion
    }
    #endregion

    #region ���صĻ���

    public class UpLoadObj
    {
        #region ����
        private String mDataBaseName = null;
        private Fields mDataFields = null;			//ȫ���������ֶ�
        private Fields mDataKeyFields = null;		//���������ֶ�
        protected String mBoardFilePath = null;		//ģ���ļ�·��
        protected Int32 mColsNum = 0;					//������ֶ���
        protected Int32 mStartCols = 0;					//��ʼ��
        protected Int32 mStartRows = 0;					//��ʼ��
        protected Int32 mPrimayColIndex = 0;			//���������
        protected String mBatchNo = null;				//���κ�
        protected String mAreaIdList = null;			//ӵ��Ȩ�޵�ȫ������ID

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
        /// ���ݿ�ʵ������
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
        /// �ֶζ��󼯺�
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
        /// �ֶιؽ��ֶ��󼯺�
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

        #region ����
        /// <summary>
        /// �õ�ȫ�����ݼ�
        /// </summary>
        /// <returns>��Ĭ�Ϸ�ʽ�õ�ȫ��</returns>
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
                throw (new Exception("���浼�����ݳ���,ԭ��:" + ex.Message));
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
                throw (new Exception("ɾ���������ʱ���ݳ���" + e.Message));
            }
        }
        #endregion

        #region ���췽��
        /// <summary>
        /// ���췽��
        /// </summary>
        /// <param name="dataBaseName">���ݿ�����,�����о����Ƿ�Ϊ�ܿ�,���Ϊ�ܿ�ʱ�����Լ�ȡ��</param>
        public UpLoadObj(String dataBaseName)
        {
            this.mDataFields = new Fields();
            this.mDataKeyFields = new Fields();
            this.mDataBaseName = dataBaseName;
        }

        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>
        public UpLoadObj()
        {
            this.mDataFields = new Fields();
            this.mDataKeyFields = new Fields();
        }
        #endregion
    }
    #endregion

    #region �������״̬

    public enum BillBackFlag
    {
        NoBack = 1,//δ�ص�
        Succeed = 2,//�سɹ�
        Fail = 3	//��ʧ��

    }

    /// <summary>
    /// �ص�ģʽ
    /// </summary>
    public enum BackMode
    {
        Manual = 1,
        Succeed = 2,
        Fail = 3
    }

    /// <summary>
    /// ִ�н��
    /// </summary>
    public enum ResultFlag
    {
        /*
         *  1:���ڴ�����(UT��ִ����Ҫ����м�ȡֵ��¼״̬)
           2:�ɹ�
           3:ʧ��
           null:��ִ��		 
         * */
        Process = 1,
        Succeed = 2,
        Fail = 3
    }

    /// <summary>
    /// �ɵ��ͻص�����ģʽ
    /// </summary>
    public enum ProcessMode
    {
        Auto = 2, //�Զ�����
        Manual = 3 //�˹�����
    }

    /// <summary>
    /// ����״̬
    /// </summary>
    public enum BillStatus
    {
        /// <summary>
        /// δ����
        /// </summary>
        UnAnalysis = 1,
        /// <summary>
        /// �����������
        /// </summary>
        AnalysisEnd = 2,
        /// <summary>
        /// ���з�����ɣ��ȴ�ִ��
        /// </summary>
        WaitExecut = 15,
        /// <summary>
        /// ִ�����
        /// </summary>
        Executed = 17,
        /// <summary>
        /// �ȴ��ص�
        /// </summary>
        WaitBack = 18,
        /// <summary>
        /// �ص�����
        /// </summary>
        Back = 19,
        /// <summary>
        /// �ص����
        /// </summary>
        BackEnd = 20,
        /// <summary>
        /// ��Դ����
        /// </summary>
        UpdateResource = 21
    }

    /// <summary>
    /// �˿�״̬
    /// </summary>
    public enum PortStatus
    {
        UnUse = 1,
        Used = 2
    }

    /// <summary>
    /// �豸״̬
    /// </summary>
    public enum ControlStatus
    {
        Normal = 1,//����
        Pause = 2,//��ͣ
        Fail = 3,//ʧ��
        Succeed = 4,//�ɹ�	
        Delayed = 5 //��ʱ�����ڣ�
    }

    /// <summary>
    /// ȷ�������ģʽ
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
    /// Common ��ժҪ˵����
    /// </summary>
    public class Common
    {
        #region ��DataTableת��ΪArrayList����
        /// <summary>
        /// ��DataTableת��ΪArrayList����
        /// </summary>
        /// <param name="dt">���ݱ��</param>
        /// <param name="rowName">�ֶ�����</param>
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

        #region ��DataTableת��Ϊ�ַ���
        /// <summary>
        /// ��DataTableת��Ϊ�ַ���,�ַ�����","�ŷָ�
        /// </summary>
        /// <param name="dt">Ҫת�������ݱ��</param>
        /// <param name="rowName">�ֶ�����</param>
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
        /// ��DataTableת��Ϊ�ַ���,�ַ�����","�ŷָ�
        /// </summary>
        /// <param name="dt">Ҫת�������ݱ��</param>
        /// <param name="rowIndex">�ֶ����</param>
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

        #region �����������ļ�
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

        #region дCookies
        /// <summary>
        /// дCookies���ͻ�����
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="strValue">ֵ</param>
        public static void WriteCookies(string key, string strValue)
        {
            //дcookies
            if (System.Web.HttpContext.Current.Request.Browser.Cookies)
            {
                string t = HttpUtility.UrlEncode(strValue);
                System.Web.HttpCookie htck = new HttpCookie(key, t);
                htck.Expires = DateTime.Now.AddDays(90);
                System.Web.HttpContext.Current.Response.Cookies.Add(htck);
            }
            else
            {
                throw (new Exception("�������֧��Cookies����ѱ�վ������Ϊ������վ��!"));
            }
        }
        #endregion

        #region ��Cookies
        /// <summary>
        /// дCookies���ͻ�����
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="strValue">ֵ</param>
        public static string ReadCookies(string key)
        {
            //дcookies
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
                throw (new Exception("����������֧��Cookies��������Щ���ܲ���ʹ�ã�"));
            }

            return str;
        }
        #endregion

        #region ��Դ�����и������ر�״̬

        /// <summary>
        /// �޸�״̬,�ύ��̨���´���
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
        ///  �޸�״̬,�ύ��̨���´���
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
        /// �޸�״̬,�ύ��̨���´���
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
        /// �޸�״̬,�ύ��̨���´���
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

        #region ������Ҫ��ҳ����ʾ����ʾ��Ϣ
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


        #region Excel����
        /// <summary>
        /// ɾ����ǰ��ǰ����ʱ�ļ�
        /// </summary>
        /// <param name="txtDir"></param>
        public void DeleteFile(String txtDir)
        {
            try
            {
                //�ж�ָ����Դ�ļ�Ŀ¼�Ƿ����
                //string txtDir = Request.MapPath(Request.ApplicationPath) + "\\temp";
                if (!System.IO.Directory.Exists(txtDir))
                {
                    throw new Exception(string.Format("δ�ҵ��ļ�Ŀ¼({0})!", txtDir));
                }

                //ɾ��������ǰ���ļ�
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

        #region Text����
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

        #region ִ�з�ҳ�洢����

        /// <summary>
        /// ִ�з�ҳ�洢����
        /// </summary>
        /// <param name="PageSize">ÿҳ��ʾ���ݸ���</param>
        /// <param name="page">Ҫ��ѯ��ҳ��</param>
        /// <param name="tabName">��ѯ�ı���</param>
        /// <param name="primarykey">�ؼ��֣�����ؼ�����,�ŷֿ�</param>
        /// <param name="fileds">Ҫ��ѯ����</param>
        /// <param name="where">where������������where�ؼ���</param>
        /// <param name="orderby">����������������order by �ؼ���</param>
        /// <param name="dataCount">��������������</param>
        /// <param name="curPageIndex">���ص�ǰҳ��</param>
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
                //ִ�����ݲ�ѯ
                table = db.ExecuteDataSet(dbcomm).Tables[0];
                object outParam_dataCount = db.GetParameterValue(dbcomm, "dataCount");
                object outParam_curPageIndex = db.GetParameterValue(dbcomm, "curPageIndex");
                //��ȡ����������
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

        #region ��ȡ���м�¼

        /// <summary>
        /// ��ȡ���м�¼
        /// </summary>
        /// <param name="table">����</param>
        /// <param name="where">����</param>
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
                throw (new Exception("��ȡ��¼����,ԭ��:" + ex.Message));
            }
            return dt;
        }

        #endregion
    }

    #region ������Ϣö��

    /// <summary>
    /// ��Դ���´���״̬
    /// </summary>
    public enum DnStatus
    {
        Download = 1,
        Disposing = 2,
        WaitConfirm = 3,
        Complete = 4
    }

    /// <summary>
    /// ��Դ���´���״̬
    /// </summary>
    public enum DnFlag
    {
        Fail = 1,
        AutoSucc = 2,
        ManuelSucc = 3
    }

    #endregion
}
