/*
 * @(#)AspxPageBase.cs    1.0, Mar 21, 2005
 *
 * @author feng xianwen.
 *
 */

using System;
using System.Reflection;
using System.Resources;
using BJ.WebTools;
using System.Web.UI;
using BJ.Project.Common;
using System.Data;
using System.Web.Services;

using BJ.Sys.Comm;
using Microsoft.JScript;

namespace BJ.AspxTask
{

    #region WebPageBase��
    /// <summary>
    /// ��չSystem.Web.UI.Page��Ӧ��ASPX��CodeBehind����չ��Ϊ���ṩͨ�����Ժͷ�����
    /// </summary>
    public class WebPageBase : System.Web.UI.Page
    {
        #region ��Ϣ����(AJAX)
        /// <summary>
        /// ���������������Ի���
        /// </summary>
        /// <param name="updatePanel">UpdatePanel</param>
        /// <param name="title">����</param>
        /// <param name="msg">����</param>
        /// <param name="funName">�ص���������</param>
        /// <param name="msgType">�Ի�������</param>
        /// <returns></returns>
        public void JqueryMessager(UpdatePanel updatePanel, string title, string msg, MessageType msgType, string funName)
        {
            //System.Random rd = new Random();
            //msg = Escape(msg);
            string script = JqueryComm.ShowMessage(title, msg, msgType, funName);
            ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(),
                      "Script" + Guid.NewGuid().ToString(), script, true);
        }

        #region ʹ��JavaScript��ʽת���ַ���
        /// <summary>
        /// ת��ָ�����ַ�������ʹ�� % �ַ��Ա����ַ�����ת�壬���� Unicode ����ʾ���ǡ�
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Escape(string str)
        {
            return GlobalObject.escape(str);
        }

        /// <summary>
        /// ��ָ���ַ����� % ����ת���ַ�ת������ԭʼ��ʽ����ת����ַ��� Unicode ��ʾ����ʾ��
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Unescape(string str)
        {
            return GlobalObject.unescape(str);
        }
        #endregion

        /// <summary>
        /// ���������������Ի���
        /// </summary>
        /// <param name="updatePanel">UpdatePanel</param>
        /// <param name="msg">����</param>
        /// <param name="msgType">�Ի�������</param>
        /// <param name="funName">�ص���������</param>
        /// <returns></returns>
        public void JqueryMessager(UpdatePanel updatePanel, string msg, MessageType msgType, string funName)
        {
            this.JqueryMessager(updatePanel, "��ʾ", msg, msgType, funName);
        }

        /// <summary>
        /// ���������������Ի���
        /// </summary>
        /// <param name="updatePanel">UpdatePanel</param>
        /// <param name="msg">����</param>
        /// <param name="msgType">�Ի�������</param>
        /// <returns></returns>
        public void JqueryMessager(UpdatePanel updatePanel, string msg, MessageType msgType)
        {
            if (msgType == MessageType.error)
            {
                this.JqueryMessager(updatePanel, "����", msg, msgType, null);
            }
            else if (msgType == MessageType.warning)
            {
                this.JqueryMessager(updatePanel, "����", msg, msgType, null);
            }
            else
            {
                this.JqueryMessager(updatePanel, "��ʾ", msg, msgType, null);
            }
        }

        /// <summary>
        /// ���������������Ի���
        /// </summary>
        /// <param name="updatePanel">UpdatePanel</param>
        /// <param name="title">����</param>
        /// <param name="msg">����</param>
        /// <param name="msgType">�Ի�������</param>
        /// <returns></returns>
        public void JqueryMessager(UpdatePanel updatePanel, string title, string msg, MessageType msgType)
        {
            this.JqueryMessager(updatePanel, title, msg, msgType, null);
        }

        /// <summary>
        /// ���������������Ի���
        /// </summary>
        /// <param name="updatePanel">UpdatePanel</param>
        /// <param name="msg">����</param>
        /// <returns></returns>
        public void JqueryMessager(UpdatePanel updatePanel, string msg)
        {
            this.JqueryMessager(updatePanel, "��ʾ", msg, MessageType.info, null);
        }
        #endregion

        #region ��Ϣ����
        /// <summary>
        /// ���������������Ի���
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="title">����</param>
        /// <param name="msg">����</param>
        /// <param name="funName">�ص���������</param>
        /// <param name="msgType">�Ի�������</param>
        /// <returns></returns>
        public void JqueryMessager(Page page, string title, string msg, MessageType msgType, string funName)
        {
            string script = JqueryComm.ShowMessage(title, msg, msgType, funName);
            Page.ClientScript.RegisterStartupScript(page.GetType(), "Script" + Guid.NewGuid(), script, true);
        }

        /// <summary>
        /// ���������������Ի���
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="msg">����</param>
        /// <param name="msgType">�Ի�������</param>
        /// <param name="funName">�ص���������</param>
        /// <returns></returns>
        public void JqueryMessager(Page page, string msg, MessageType msgType, string funName)
        {
            this.JqueryMessager(page, "��ʾ", msg, msgType, funName);
        }

        /// <summary>
        /// ���������������Ի���
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="msg">����</param>
        /// <param name="msgType">�Ի�������</param>
        /// <returns></returns>
        public void JqueryMessager(Page page, string msg, MessageType msgType)
        {
            this.JqueryMessager(page, "��ʾ", msg, msgType, null);
        }

        /// <summary>
        /// ���������������Ի���
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="title">����</param>
        /// <param name="msg">����</param>
        /// <param name="msgType">�Ի�������</param>
        /// <returns></returns>
        public void JqueryMessager(Page page, string title, string msg, MessageType msgType)
        {
            this.JqueryMessager(page, title, msg, msgType, null);
        }

        /// <summary>
        /// ���������������Ի���
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="msg">����</param>
        /// <returns></returns>
        public void JqueryMessager(Page page, string msg)
        {
            this.JqueryMessager(page, "��ʾ", msg, MessageType.info, null);
        }
        #endregion

        private String mRootNamespace;

        private String mNameSpaceName = null;	//�����ռ�������

        private bool mIsOnlyRead = false;			//�Ƿ�ֻ���в鿴��Ȩ��

        private String mAppPath;

        private ResourceManager mRes;

        private Operator mOper = null;			//�û����󣬰����û��������Ϣ

        private String mMainDataBaseName = null;//�����ݿ��ʵ������

        /// <summary>
        /// �Ƿ�ֻ���в鿴��Ȩ��
        /// </summary>
        public bool IsOnlyRead
        {
            get
            {
                return this.mIsOnlyRead;
            }
        }

        /// <summary>
        /// �����յ�������
        /// </summary>
        public String NameSpaceName
        {
            get
            {
                return this.mNameSpaceName;
            }
        }

        /// <summary>
        /// �û���½����
        /// </summary>
        public String OperLoginName
        {
            get
            {
                return this.mOper.OperLogin;
            }
        }

        /// <summary>
        /// �û�����
        /// </summary>
        public String OperPwd
        {
            get
            {
                return this.mOper.OperPwd;
            }
        }

        /// <summary>
        /// �õ������ݿ�����ݿ�����,���������Ӳ��ʵ������
        /// </summary>
        protected String MainDataBaseName
        {
            get
            {
                if (mMainDataBaseName == null)
                {
                    mMainDataBaseName = BJ.SystemWebFormulation.SystemWebFormulation.Parameter("MainDataBaseName");
                }

                return mMainDataBaseName;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ��Ŀ�е���Ա�����ɴ˿��ṩȨ����֤�ȹ���
        /// </summary>
        protected Operator Oper
        {
            get
            {
                if (mOper == null)
                {
                    mOper = (Operator)Session["Operator"];
                }

                return mOper;
            }
        }

        protected void GoHome(Page page, String msg)
        {
            ShowMessage(page, msg);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "Error", "<script language='javascript'>doGoHome();</script>");
            ClientScript.RegisterStartupScript(this.GetType(), "Error", "<script language='javascript'>doGoHome();</script>");
        }

        protected void Close(Page page, String msg)
        {
            if (msg.Trim().Length > 0)
            {
                msg = System.Web.HttpUtility.UrlEncode(msg);
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "close", "<script language='javascript'>alert('" + msg + "');window.close();</script>");
                ClientScript.RegisterStartupScript(this.GetType(), "close", "<script language='javascript'>alert('" + msg + "');window.close();</script>");
            }
            else
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "close", "<script language='javascript'>window.close();</script>");
                ClientScript.RegisterStartupScript(this.GetType(), "close", "<script language='javascript'>window.close();</script>");
            }
        }

        protected void ShowMessage(Page page, String msg)
        {
            msg = CheckSpecialString(msg);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "Error", "<script language='javascript'>alert('" + msg + "');</script>");
            ClientScript.RegisterStartupScript(this.GetType(), "Error", "<script language='javascript'>alert('" + msg + "');</script>");
        }

        protected String CheckSpecialString(String str)
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

        /// <summary>
        /// ��ȡӦ�ó���������ռ�����
        /// </summary>
        protected String RootNamespace
        {
            get
            {
                if (mRootNamespace == null)
                {
                    mRootNamespace = GetRootNamespace();
                }

                return mRootNamespace;
            }
        }

        /// <summary>
        /// ��ȡ��ǰӦ�õ�ApplicationPath���� "/" + ����Ŀ¼��
        /// </summary>
        protected String AppPath
        {
            get
            {
                if (mAppPath == null)
                {
                    mAppPath = GetAppPath();
                }

                return mAppPath;
            }
        }

        /// <summary>
        /// override ��ʼ��������
        /// </summary>
        /// <remarks>
        /// �� CodeBehind ��� OnInit(e) �����е��� base.OnInit(e) ��ʼ����
        /// ������ߺ���ȡֵ������Ч�ʡ�
        /// </remarks>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            mAppPath = GetAppPath();

            //mOper    = (Operator)Session["Operator"];

            mRootNamespace = GetRootNamespace();

            InitResource();
        }

        protected override void OnLoad(EventArgs e)
        {
            //��ȡ��Ա����
            if (Session["Operator"] == null)
            {
                mOper = CreateOperatorObj();
                Session["Operator"] = mOper;
                Session.Timeout = 120;
            }
            else
            {
                mOper = (Operator)Session["Operator"];
            }

            //��ȡȨ�������Ϣ
            if (ViewState["_RoleIndex"] == null)
            {
                this.SetRole();
            }
            else
            {
                this.mIsOnlyRead = (bool)ViewState["_RoleIndex"];
            }

            string functionName = "";
            if (this.IsOnlyRead)
            {
                //functionName = "<script language='javascript'>dofreshSession();function setContorlRole(controlId){document.getElementById(controlId).disabled = true;}</script>";
                functionName = "<script language='javascript'>function setContorlRole(controlId){document.getElementById(controlId).disabled = true;}</script>";
            }
            else
            {
                //functionName = "<script language='javascript'>dofreshSession();function setContorlRole(controlId){;}</script>";
                functionName = "<script language='javascript'>function setContorlRole(controlId){;}</script>";
            }
            //this.RegisterClientScriptBlock("load", functionName);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "load", functionName);

            base.OnLoad(e);
        }

        #region ��ȡcookies����ʵ������Ա����
        private Operator CreateOperatorObj()
        {
            //��ȡcookies
            Operator oper = null;
            if (Request.Browser.Cookies)
            {
                System.Web.HttpCookie httpc = Request.Cookies[
                   BJ.SystemWebFormulation.SystemWebFormulation.Parameter("OperatorId")];

                if (httpc != null)
                {
                    if (httpc.Value == null)
                    {
                        throw (new Exception("��ȡCookiesֵ����"));
                    }
                    string strId = httpc.Value.ToString().Trim();

                    try
                    {
                        oper = OperatorFactory.OperatorCreate(strId);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else
            {
                throw (new Exception("����������֧��Cookies��������Щ���ܲ���ʹ�ã�"));
            }

            return oper;
        }
        #endregion


        #region Ȩ���ж�
        private void SetRole()
        {
            //if(this.Oper == null)
            //{
            //    this.mIsOnlyRead = false;ViewState["_RoleIndex"] = this.mIsOnlyRead;return;
            //}
            //if(this.Oper.IsSuper)
            //{
            //    this.mIsOnlyRead = false;
            //}
            //else
            //{
            //    //��ͨ�û�
            //    this.mIsOnlyRead = false;
            //    NormalVerification verify = new NormalVerification(this.Oper.OperatorId);
            //    if(!verify.IsHaveFunctionItem(this.NameSpaceName))
            //    {
            //        ViewState["_RoleIndex"] = this.mIsOnlyRead;return;
            //    }

            //    DataTable dt = verify.GetRoleFunction(this.NameSpaceName);
            //    if(dt.Rows.Count == 0)
            //    {
            //        this.mIsOnlyRead = true;ViewState["_RoleIndex"] = this.mIsOnlyRead;return;
            //    }

            //    bool ret = false;
            //    foreach(DataRow dr in dt.Rows)
            //    {
            //        string urlKey		= dr["FWM_URL"].ToString().Trim();
            //        string urlValue	= dr["FWM_Key"].ToString().Trim();
            //        string type			= dr["FUNN_Type"].ToString().Trim();

            //        if(IsRightPara(urlKey , urlValue))
            //        {
            //            ret = true;
            //            if(type == "1")
            //            {
            //                this.mIsOnlyRead = false;break;
            //            }
            //            else
            //            {
            //                this.mIsOnlyRead = true;
            //            }
            //        }
            //    }

            //    if(!ret)
            //    {
            //        this.mIsOnlyRead = true;
            //    }
            //}

            ////��ViewState
            //ViewState["_RoleIndex"] = this.mIsOnlyRead;
        }

        private bool IsRightPara(string urlKey, string strValue)
        {
            if (urlKey == "")
            {
                return true;
            }
            if (Request.QueryString[urlKey] == strValue)
            {
                return true;
            }
            return false;
        }
        #endregion

        /// <summary>
        /// ��ȡ��ǰӦ�õ�ApplicationPath���� "/" + ����Ŀ¼��
        /// </summary>
        /// <returns>Ӧ�ó����ApplicationPath</returns>
        private String GetAppPath()
        {
            return Request.ApplicationPath;
        }

        /// <summary>
        /// �� Web.config ��ȡ dsnstore �ؼ��� RootNamespace ��ֵ��
        /// ����ʾӦ�ó����Ĭ�ϸ������ռ�����
        /// </summary>
        /// <returns>�������ռ���</returns>
        private String GetRootNamespace()
        {
            String rt = HtmlUtil.getDSN("RootNamespace");

            if (rt == null)
            {
                rt = this.AppPath;
            }

            return rt;
        }

        /// <summary>
        /// ��ʼ����Դ����ʵ��������
        /// </summary>
        /// <remarks>
        /// �÷�����Ϊ��Դ�� BaseName Ϊ <RootNamespace>.bin.Resx.CodeBehindPathAndTypeName��
        /// �����������VS�б��룬��Ӧ��Դ�ļ�Ӧ�ö���Ϊ ApplicationPath/bin/Resx/CodeBehindPath/CodeBehindTypeName.resx��
        /// </remarks>
        private void InitResource()
        {
            // CodeBehind���Type��
            Type bt = GetType().BaseType;

            this.mNameSpaceName = bt.Namespace + "." + bt.Name;

            //			String baseName = bt.FullName.Insert(RootNamespace.Length, ".bin.Resx");
            String baseName = RootNamespace + ".Resx." + bt.Name;

            if (mRes != null)
            {
                mRes.ReleaseAllResources();
            }

            try
            {
                mRes = new ResourceManager(baseName, bt.Assembly);
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// ��Ĭ�ϳ����л�ȡĬ����Դ�ļ���ָ���ؼ��ֵ�ֵ��
        /// ��Դ��ʼ�򿪺󲻻ᱻ�رգ�ֱ�����������ա�
        /// </summary>
        /// <remarks>
        /// Ĭ�ϳ���ΪCodeBehind��ĳ��򼯡�
        /// Ĭ����Դ�� BaseName Ϊ <RootNamespace>.bin.Resx.CodeBehindPathAndTypeName��
        /// �����������VS�б��룬��Ӧ��Դ�ļ�Ӧ�ö���Ϊ ApplicationPath/bin/Resx/CodeBehindPath/CodeBehindTypeName.resx��
        /// </remarks>
        /// <param name="key">�ؼ���</param>
        /// <returns>Ĭ����Դ�ļ���ָ���ؼ��ֶ�Ӧ��ֵ</returns>
        protected String GetString(String key)
        {
            if (mRes == null)
            {
                InitResource();
            }

            if (mRes == null)
            {
                return null;
            }

            return mRes.GetString(key);
        }

        /// <summary>
        /// �� CodeBehind ��ĳ����л�ȡָ�� BaseName ����Դ�ļ���ָ���ؼ��ֵ�ֵ��
        /// </summary>
        /// <remarks>
        /// �ڵ��õ�ʱ�����Դ��ȡֵ��رա�
        /// </remarks>
        /// <param name="baseName">��Դ�ļ��� BaseName</param>
        /// <param name="key">�ؼ���</param>
        /// <returns>�ؼ��ֶ�Ӧ��ֵ</returns>
        protected String GetString(String baseName, String key)
        {
            return GetString(GetType().BaseType.Assembly, baseName, key);
        }

        /// <summary>
        /// ��ָ�������л�ȡ BaseName Ϊ CodeBehindFullTypeName 
        /// ����Դ�ļ���ָ���ؼ��ֵ�ֵ��
        /// </summary>
        /// <remarks>
        /// �ڵ��õ�ʱ�����Դ��ȡֵ��رա�
        /// </remarks>
        /// <param name="a">��������</param>
        /// <param name="key">�ؼ���</param>
        /// <returns>�ؼ��ֶ�Ӧ��ֵ</returns>
        protected String GetString(Assembly a, String key)
        {
            Type bt = GetType().BaseType;

            return GetString(a, bt.FullName, key);
        }

        /// <summary>
        /// ��ָ�������л�ȡָ�� BaseName ����Դ�ļ���ָ���ؼ��ֵ�ֵ��
        /// </summary>
        /// <remarks>
        /// �ڵ��õ�ʱ�����Դ��ȡֵ��رա�
        /// </remarks>
        /// <param name="a">��������</param>
        /// <param name="baseName">��Դ�ļ��� BaseName</param>
        /// <param name="key">�ؼ���</param>
        /// <returns>�ؼ��ֶ�Ӧ��ֵ</returns>
        protected String GetString(Assembly a, String baseName, String key)
        {
            String rt = "";

            if (a == null || baseName == null || key == null)
            {
                return rt;
            }

            ResourceManager tmpRes;
            try
            {
                tmpRes = new ResourceManager(baseName, a);
            }
            catch
            {
                return null;
            }

            if (tmpRes == null)
            {
                return null;
            }

            rt = tmpRes.GetString(key);
            tmpRes.ReleaseAllResources();
            return rt;
        }

        #region ��ȡ Web Service ����ͷ
        /// <summary>
        /// ��ȡ Web Service ����ͷ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetAuthSoapHeader<T>()
            where T : System.Web.Services.Protocols.SoapHeader
        {
            // ��ȡ���͡�
            Type type = typeof(T);

            // ����Ĭ�Ϲ��캯��ʵ��������
            ConstructorInfo constructor = type.GetConstructor(new Type[0]);
            object objT = constructor.Invoke(new object[0]);

            // ��ȡ���Բ���ֵ��
            // OperId
            PropertyInfo property = type.GetProperty("OperId");
            property.SetValue(objT, Oper.OperatorId, null);

            // OperIp
            property = type.GetProperty("OperIp");
            property.SetValue(objT, Oper.Client_IP, null);

            // OperName
            property = type.GetProperty("OperName");
            property.SetValue(objT, Oper.OperName, null);

            // ServiceId
            property = type.GetProperty("ServiceId");
            property.SetValue(objT, 20, null);

            // OperLogin
            property = type.GetProperty("OperLogin");
            if (property != null)
            {
                property.SetValue(objT, Oper.OperLogin, null);
            }

            return (T)objT;
        }

        #endregion

        #region EasyUi��Ϣ������ݰ�
        private EasyUiHelper m_EasyUi = null;
        /// <summary>
        /// EasyUiǰ̨AJAX+JSON����Ϣ�������ݵķ�װ����
        /// </summary>
        public EasyUiHelper EasyUi
        {
            get
            {
                m_EasyUi = (m_EasyUi == null ? new EasyUiHelper() : m_EasyUi);
                return m_EasyUi;
            }
        }

        #endregion

    } // End of class WebPageBase.
    #endregion

    #region AspxPageBase
    /// <summary>
    /// ��չSystem.Web.UI.Page��Ӧ��ASPX��CodeBehind����չ��Ϊ���ṩͨ�����Ժͷ�����
    /// </summary>
    /// <remarks>
    /// AspxPageBase has been deprecated, ����ʹ�� WebPageBase
    /// </remarks>
    public class AspxPageBase : System.Web.UI.Page
    {

        private String mRootNamespace;

        private String mAppPath;

        //private Int32 mUserID = -1;

        private Object mOper = null;			//�û����󣬰����û��������Ϣ

        private ResourceManager mRes;

        /// <summary>
        /// ��ȡӦ�ó���������ռ�����
        /// </summary>
        protected String RootNamespace
        {
            get
            {
                if (mRootNamespace == null)
                {
                    mRootNamespace = GetRootNamespace();
                }

                return mRootNamespace;
            }
        }

        /// <summary>
        /// ��ȡ��ǰӦ�õ�ApplicationPath���� "/" + ����Ŀ¼��
        /// </summary>
        protected String AppPath
        {
            get
            {
                if (mAppPath == null)
                {
                    mAppPath = GetAppPath();
                }

                return mAppPath;
            }
        }

        protected Object Oper
        {
            get
            {
                if (mOper == null)
                {
                    mOper = Session["Operator"];
                }

                return mOper;
            }
        }

        /// <summary>
        /// ��ȡ OperatorID ֵ��
        /// </summary>
        //      protected Int32 UserID 
        //      {
        //         get 
        //         { 
        //            if (mUserID < 0) 
        //            {
        //               mUserID = GetUserID();
        //            }
        //
        //            return mUserID;
        //         }
        //      }

        /// <summary>
        /// override ��ʼ��������
        /// </summary>
        /// <remarks>
        /// �� CodeBehind ��� OnInit(e) �����е��� base.OnInit(e) ��ʼ����
        /// ������ߺ���ȡֵ������Ч�ʡ�
        /// </remarks>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            mAppPath = GetAppPath();

            mRootNamespace = GetRootNamespace();

            InitResource();
        }

        /// <summary>
        /// ��ȡ OperatorID ֵ��
        /// </summary>
        /// <returns>����ԱID����ֵ</returns>
        private Int32 GetUserID()
        {
            Object o = Session["UserID"];
            if (o == null)
            {
                return -1;
            }

            return (Int32)o;
        }

        /// <summary>
        /// ��ȡ��ǰӦ�õ�ApplicationPath���� "/" + ����Ŀ¼��
        /// </summary>
        /// <returns>Ӧ�ó����ApplicationPath</returns>
        private String GetAppPath()
        {
            return Request.ApplicationPath;
        }

        /// <summary>
        /// �� Web.config ��ȡ dsnstore �ؼ��� RootNamespace ��ֵ��
        /// ����ʾӦ�ó����Ĭ�ϸ������ռ�����
        /// </summary>
        /// <returns>�������ռ���</returns>
        private String GetRootNamespace()
        {
            return HtmlUtil.getDSN("RootNamespace");
        }

        /// <summary>
        /// ��ʼ����Դ����ʵ��������
        /// </summary>
        /// <remarks>
        /// �÷�����Ϊ��ԴBaseNameΪ��RootNamespace.Resx.CodeBehindTypeName��
        /// �����������VS�б��룬��Ӧ��Դ�ļ�Ӧ�ö���Ϊ ApplicationPath/Resx/CodeBehindTypeName.resx��
        /// </remarks>
        private void InitResource()
        {
            // CodeBehind���Type��
            Type bt = GetType().BaseType;

            String baseName = RootNamespace + ".Resx." + bt.Name;

            if (mRes != null)
            {
                mRes.ReleaseAllResources();
            }

            try
            {
                mRes = new ResourceManager(baseName, bt.Assembly);
            }
            catch
            {
                return;
            }
        }

        protected Object GetObject(String key)
        {
            if (mRes == null)
            {
                InitResource();
            }

            if (mRes == null)
            {
                return null;
            }

            return mRes.GetObject(key);
        }

        /// <summary>
        /// ��Ĭ�ϳ����л�ȡĬ����Դ�ļ���ָ���ؼ��ֵ�ֵ��
        /// ��Դ��ʼ�򿪺󲻻ᱻ�رգ�ֱ�����������ա�
        /// </summary>
        /// <remarks>
        /// Ĭ�ϳ���ΪCodeBehind��ĳ��򼯡�
        /// Ĭ����Դ�� BaseName Ϊ <RootNamespace>.Resx.CodeBehindTypeName��
        /// �����������VS�б��룬��Ӧ��Դ�ļ�Ӧ�ö���Ϊ ApplicationPath/Resx/CodeBehindTypeName.resx��
        /// </remarks>
        /// <param name="key">�ؼ���</param>
        /// <returns>Ĭ����Դ�ļ���ָ���ؼ��ֶ�Ӧ��ֵ</returns>
        protected String GetString(String key)
        {
            if (mRes == null)
            {
                InitResource();
            }

            if (mRes == null)
            {
                return null;
            }

            return mRes.GetString(key);
        }

        /// <summary>
        /// �� CodeBehind ��ĳ����л�ȡָ�� BaseName ����Դ�ļ���ָ���ؼ��ֵ�ֵ��
        /// </summary>
        /// <remarks>
        /// �ڵ��õ�ʱ�����Դ��ȡֵ��رա�
        /// </remarks>
        /// <param name="baseName">��Դ�ļ��� BaseName</param>
        /// <param name="key">�ؼ���</param>
        /// <returns>�ؼ��ֶ�Ӧ��ֵ</returns>
        protected String GetString(String baseName, String key)
        {
            return GetString(GetType().BaseType.Assembly, baseName, key);
        }

        /// <summary>
        /// ��ָ�������л�ȡ BaseName Ϊ RootNamespace.Resx.CodeBehindTypeName 
        /// ����Դ�ļ���ָ���ؼ��ֵ�ֵ��
        /// </summary>
        /// <remarks>
        /// �ڵ��õ�ʱ�����Դ��ȡֵ��رա�
        /// </remarks>
        /// <param name="a">��������</param>
        /// <param name="key">�ؼ���</param>
        /// <returns>�ؼ��ֶ�Ӧ��ֵ</returns>
        protected String GetString(Assembly a, String key)
        {
            Type bt = GetType().BaseType;

            return GetString(a, RootNamespace + ".Resx." + bt.Name, key);
        }

        /// <summary>
        /// ��ָ�������л�ȡָ�� BaseName ����Դ�ļ���ָ���ؼ��ֵ�ֵ��
        /// </summary>
        /// <remarks>
        /// �ڵ��õ�ʱ�����Դ��ȡֵ��رա�
        /// </remarks>
        /// <param name="a">��������</param>
        /// <param name="baseName">��Դ�ļ��� BaseName</param>
        /// <param name="key">�ؼ���</param>
        /// <returns>�ؼ��ֶ�Ӧ��ֵ</returns>
        protected String GetString(Assembly a, String baseName, String key)
        {
            String rt = "";

            if (a == null || baseName == null || key == null)
            {
                return rt;
            }

            ResourceManager tmpRes;
            try
            {
                tmpRes = new ResourceManager(baseName, a);
            }
            catch
            {
                return null;
            }

            if (tmpRes == null)
            {
                return null;
            }

            rt = tmpRes.GetString(key);

            tmpRes.ReleaseAllResources();

            return rt;
        }

    } // End of class AspxPageBase.
    #endregion
}
