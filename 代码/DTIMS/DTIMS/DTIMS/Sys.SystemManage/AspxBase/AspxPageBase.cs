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

    #region WebPageBase类
    /// <summary>
    /// 扩展System.Web.UI.Page，应由ASPX的CodeBehind类扩展，为其提供通用属性和方法。
    /// </summary>
    public class WebPageBase : System.Web.UI.Page
    {
        #region 消息窗口(AJAX)
        /// <summary>
        /// 功能描述：弹出对话框
        /// </summary>
        /// <param name="updatePanel">UpdatePanel</param>
        /// <param name="title">标题</param>
        /// <param name="msg">内容</param>
        /// <param name="funName">回调函数名称</param>
        /// <param name="msgType">对话框类型</param>
        /// <returns></returns>
        public void JqueryMessager(UpdatePanel updatePanel, string title, string msg, MessageType msgType, string funName)
        {
            //System.Random rd = new Random();
            //msg = Escape(msg);
            string script = JqueryComm.ShowMessage(title, msg, msgType, funName);
            ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(),
                      "Script" + Guid.NewGuid().ToString(), script, true);
        }

        #region 使用JavaScript方式转换字符串
        /// <summary>
        /// 转换指定的字符串，以使用 % 字符对保留字符进行转义，并以 Unicode 法表示它们。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Escape(string str)
        {
            return GlobalObject.escape(str);
        }

        /// <summary>
        /// 将指定字符串中 % 的已转义字符转换成其原始格式。已转义的字符以 Unicode 表示法表示。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Unescape(string str)
        {
            return GlobalObject.unescape(str);
        }
        #endregion

        /// <summary>
        /// 功能描述：弹出对话框
        /// </summary>
        /// <param name="updatePanel">UpdatePanel</param>
        /// <param name="msg">内容</param>
        /// <param name="msgType">对话框类型</param>
        /// <param name="funName">回调函数名称</param>
        /// <returns></returns>
        public void JqueryMessager(UpdatePanel updatePanel, string msg, MessageType msgType, string funName)
        {
            this.JqueryMessager(updatePanel, "提示", msg, msgType, funName);
        }

        /// <summary>
        /// 功能描述：弹出对话框
        /// </summary>
        /// <param name="updatePanel">UpdatePanel</param>
        /// <param name="msg">内容</param>
        /// <param name="msgType">对话框类型</param>
        /// <returns></returns>
        public void JqueryMessager(UpdatePanel updatePanel, string msg, MessageType msgType)
        {
            if (msgType == MessageType.error)
            {
                this.JqueryMessager(updatePanel, "错误", msg, msgType, null);
            }
            else if (msgType == MessageType.warning)
            {
                this.JqueryMessager(updatePanel, "警告", msg, msgType, null);
            }
            else
            {
                this.JqueryMessager(updatePanel, "提示", msg, msgType, null);
            }
        }

        /// <summary>
        /// 功能描述：弹出对话框
        /// </summary>
        /// <param name="updatePanel">UpdatePanel</param>
        /// <param name="title">标题</param>
        /// <param name="msg">内容</param>
        /// <param name="msgType">对话框类型</param>
        /// <returns></returns>
        public void JqueryMessager(UpdatePanel updatePanel, string title, string msg, MessageType msgType)
        {
            this.JqueryMessager(updatePanel, title, msg, msgType, null);
        }

        /// <summary>
        /// 功能描述：弹出对话框
        /// </summary>
        /// <param name="updatePanel">UpdatePanel</param>
        /// <param name="msg">内容</param>
        /// <returns></returns>
        public void JqueryMessager(UpdatePanel updatePanel, string msg)
        {
            this.JqueryMessager(updatePanel, "提示", msg, MessageType.info, null);
        }
        #endregion

        #region 消息窗口
        /// <summary>
        /// 功能描述：弹出对话框
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="title">标题</param>
        /// <param name="msg">内容</param>
        /// <param name="funName">回调函数名称</param>
        /// <param name="msgType">对话框类型</param>
        /// <returns></returns>
        public void JqueryMessager(Page page, string title, string msg, MessageType msgType, string funName)
        {
            string script = JqueryComm.ShowMessage(title, msg, msgType, funName);
            Page.ClientScript.RegisterStartupScript(page.GetType(), "Script" + Guid.NewGuid(), script, true);
        }

        /// <summary>
        /// 功能描述：弹出对话框
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="msg">内容</param>
        /// <param name="msgType">对话框类型</param>
        /// <param name="funName">回调函数名称</param>
        /// <returns></returns>
        public void JqueryMessager(Page page, string msg, MessageType msgType, string funName)
        {
            this.JqueryMessager(page, "提示", msg, msgType, funName);
        }

        /// <summary>
        /// 功能描述：弹出对话框
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="msg">内容</param>
        /// <param name="msgType">对话框类型</param>
        /// <returns></returns>
        public void JqueryMessager(Page page, string msg, MessageType msgType)
        {
            this.JqueryMessager(page, "提示", msg, msgType, null);
        }

        /// <summary>
        /// 功能描述：弹出对话框
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="title">标题</param>
        /// <param name="msg">内容</param>
        /// <param name="msgType">对话框类型</param>
        /// <returns></returns>
        public void JqueryMessager(Page page, string title, string msg, MessageType msgType)
        {
            this.JqueryMessager(page, title, msg, msgType, null);
        }

        /// <summary>
        /// 功能描述：弹出对话框
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="msg">内容</param>
        /// <returns></returns>
        public void JqueryMessager(Page page, string msg)
        {
            this.JqueryMessager(page, "提示", msg, MessageType.info, null);
        }
        #endregion

        private String mRootNamespace;

        private String mNameSpaceName = null;	//命名空间与类名

        private bool mIsOnlyRead = false;			//是否只仅有查看的权限

        private String mAppPath;

        private ResourceManager mRes;

        private Operator mOper = null;			//用户对象，包含用户的相关信息

        private String mMainDataBaseName = null;//主数据库的实例名称

        /// <summary>
        /// 是否只仅有查看的权限
        /// </summary>
        public bool IsOnlyRead
        {
            get
            {
                return this.mIsOnlyRead;
            }
        }

        /// <summary>
        /// 命名空单与类名
        /// </summary>
        public String NameSpaceName
        {
            get
            {
                return this.mNameSpaceName;
            }
        }

        /// <summary>
        /// 用户登陆名称
        /// </summary>
        public String OperLoginName
        {
            get
            {
                return this.mOper.OperLogin;
            }
        }

        /// <summary>
        /// 用户密码
        /// </summary>
        public String OperPwd
        {
            get
            {
                return this.mOper.OperPwd;
            }
        }

        /// <summary>
        /// 得到主数据库的数据库名称,即数据连接层的实例名称
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
        /// 获取当前项目中的人员对象，由此可提供权限验证等功能
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
        /// 获取应用程序根命名空间名。
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
        /// 获取当前应用的ApplicationPath，即 "/" + 虚拟目录。
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
        /// override 初始化方法。
        /// </summary>
        /// <remarks>
        /// 在 CodeBehind 类的 OnInit(e) 方法中调用 base.OnInit(e) 初始化，
        /// 可以提高后续取值操作的效率。
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
            //读取人员对象
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

            //读取权限相关信息
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

        #region 读取cookies重新实例化人员对象
        private Operator CreateOperatorObj()
        {
            //读取cookies
            Operator oper = null;
            if (Request.Browser.Cookies)
            {
                System.Web.HttpCookie httpc = Request.Cookies[
                   BJ.SystemWebFormulation.SystemWebFormulation.Parameter("OperatorId")];

                if (httpc != null)
                {
                    if (httpc.Value == null)
                    {
                        throw (new Exception("读取Cookies值出错！"));
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
                throw (new Exception("你的浏览器不支持Cookies，可能有些功能不能使用！"));
            }

            return oper;
        }
        #endregion


        #region 权限判断
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
            //    //普通用户
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

            ////存ViewState
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
        /// 获取当前应用的ApplicationPath，即 "/" + 虚拟目录。
        /// </summary>
        /// <returns>应用程序的ApplicationPath</returns>
        private String GetAppPath()
        {
            return Request.ApplicationPath;
        }

        /// <summary>
        /// 从 Web.config 中取 dsnstore 关键字 RootNamespace 的值。
        /// 它表示应用程序的默认根命名空间名。
        /// </summary>
        /// <returns>根命名空间名</returns>
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
        /// 初始化资源管理实例变量。
        /// </summary>
        /// <remarks>
        /// 该方法认为资源的 BaseName 为 <RootNamespace>.bin.Resx.CodeBehindPathAndTypeName。
        /// 这样，如果在VS中编译，对应资源文件应该定义为 ApplicationPath/bin/Resx/CodeBehindPath/CodeBehindTypeName.resx。
        /// </remarks>
        private void InitResource()
        {
            // CodeBehind类的Type。
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
        /// 在默认程序集中获取默认资源文件中指定关键字的值。
        /// 资源初始打开后不会被关闭，直到被垃圾回收。
        /// </summary>
        /// <remarks>
        /// 默认程序集为CodeBehind类的程序集。
        /// 默认资源的 BaseName 为 <RootNamespace>.bin.Resx.CodeBehindPathAndTypeName。
        /// 这样，如果在VS中编译，对应资源文件应该定义为 ApplicationPath/bin/Resx/CodeBehindPath/CodeBehindTypeName.resx。
        /// </remarks>
        /// <param name="key">关键字</param>
        /// <returns>默认资源文件中指定关键字对应的值</returns>
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
        /// 在 CodeBehind 类的程序集中获取指定 BaseName 的资源文件中指定关键字的值。
        /// </summary>
        /// <remarks>
        /// 在调用的时候打开资源，取值后关闭。
        /// </remarks>
        /// <param name="baseName">资源文件的 BaseName</param>
        /// <param name="key">关键字</param>
        /// <returns>关键字对应的值</returns>
        protected String GetString(String baseName, String key)
        {
            return GetString(GetType().BaseType.Assembly, baseName, key);
        }

        /// <summary>
        /// 在指定程序集中获取 BaseName 为 CodeBehindFullTypeName 
        /// 的资源文件中指定关键字的值。
        /// </summary>
        /// <remarks>
        /// 在调用的时候打开资源，取值后关闭。
        /// </remarks>
        /// <param name="a">程序集名称</param>
        /// <param name="key">关键字</param>
        /// <returns>关键字对应的值</returns>
        protected String GetString(Assembly a, String key)
        {
            Type bt = GetType().BaseType;

            return GetString(a, bt.FullName, key);
        }

        /// <summary>
        /// 在指定程序集中获取指定 BaseName 的资源文件中指定关键字的值。
        /// </summary>
        /// <remarks>
        /// 在调用的时候打开资源，取值后关闭。
        /// </remarks>
        /// <param name="a">程序集名称</param>
        /// <param name="baseName">资源文件的 BaseName</param>
        /// <param name="key">关键字</param>
        /// <returns>关键字对应的值</returns>
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

        #region 获取 Web Service 请求头
        /// <summary>
        /// 获取 Web Service 请求头。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetAuthSoapHeader<T>()
            where T : System.Web.Services.Protocols.SoapHeader
        {
            // 获取类型。
            Type type = typeof(T);

            // 调用默认构造函数实例化对象。
            ConstructorInfo constructor = type.GetConstructor(new Type[0]);
            object objT = constructor.Invoke(new object[0]);

            // 获取属性并赋值。
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

        #region EasyUi消息框和数据绑定
        private EasyUiHelper m_EasyUi = null;
        /// <summary>
        /// EasyUi前台AJAX+JSON的消息框与数据的封装对象。
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
    /// 扩展System.Web.UI.Page，应由ASPX的CodeBehind类扩展，为其提供通用属性和方法。
    /// </summary>
    /// <remarks>
    /// AspxPageBase has been deprecated, 建议使用 WebPageBase
    /// </remarks>
    public class AspxPageBase : System.Web.UI.Page
    {

        private String mRootNamespace;

        private String mAppPath;

        //private Int32 mUserID = -1;

        private Object mOper = null;			//用户对象，包含用户的相关信息

        private ResourceManager mRes;

        /// <summary>
        /// 获取应用程序根命名空间名。
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
        /// 获取当前应用的ApplicationPath，即 "/" + 虚拟目录。
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
        /// 获取 OperatorID 值。
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
        /// override 初始化方法。
        /// </summary>
        /// <remarks>
        /// 在 CodeBehind 类的 OnInit(e) 方法中调用 base.OnInit(e) 初始化，
        /// 可以提高后续取值操作的效率。
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
        /// 获取 OperatorID 值。
        /// </summary>
        /// <returns>操作员ID整数值</returns>
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
        /// 获取当前应用的ApplicationPath，即 "/" + 虚拟目录。
        /// </summary>
        /// <returns>应用程序的ApplicationPath</returns>
        private String GetAppPath()
        {
            return Request.ApplicationPath;
        }

        /// <summary>
        /// 从 Web.config 中取 dsnstore 关键字 RootNamespace 的值。
        /// 它表示应用程序的默认根命名空间名。
        /// </summary>
        /// <returns>根命名空间名</returns>
        private String GetRootNamespace()
        {
            return HtmlUtil.getDSN("RootNamespace");
        }

        /// <summary>
        /// 初始化资源管理实例变量。
        /// </summary>
        /// <remarks>
        /// 该方法认为资源BaseName为：RootNamespace.Resx.CodeBehindTypeName。
        /// 这样，如果在VS中编译，对应资源文件应该定义为 ApplicationPath/Resx/CodeBehindTypeName.resx。
        /// </remarks>
        private void InitResource()
        {
            // CodeBehind类的Type。
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
        /// 在默认程序集中获取默认资源文件中指定关键字的值。
        /// 资源初始打开后不会被关闭，直到被垃圾回收。
        /// </summary>
        /// <remarks>
        /// 默认程序集为CodeBehind类的程序集。
        /// 默认资源的 BaseName 为 <RootNamespace>.Resx.CodeBehindTypeName。
        /// 这样，如果在VS中编译，对应资源文件应该定义为 ApplicationPath/Resx/CodeBehindTypeName.resx。
        /// </remarks>
        /// <param name="key">关键字</param>
        /// <returns>默认资源文件中指定关键字对应的值</returns>
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
        /// 在 CodeBehind 类的程序集中获取指定 BaseName 的资源文件中指定关键字的值。
        /// </summary>
        /// <remarks>
        /// 在调用的时候打开资源，取值后关闭。
        /// </remarks>
        /// <param name="baseName">资源文件的 BaseName</param>
        /// <param name="key">关键字</param>
        /// <returns>关键字对应的值</returns>
        protected String GetString(String baseName, String key)
        {
            return GetString(GetType().BaseType.Assembly, baseName, key);
        }

        /// <summary>
        /// 在指定程序集中获取 BaseName 为 RootNamespace.Resx.CodeBehindTypeName 
        /// 的资源文件中指定关键字的值。
        /// </summary>
        /// <remarks>
        /// 在调用的时候打开资源，取值后关闭。
        /// </remarks>
        /// <param name="a">程序集名称</param>
        /// <param name="key">关键字</param>
        /// <returns>关键字对应的值</returns>
        protected String GetString(Assembly a, String key)
        {
            Type bt = GetType().BaseType;

            return GetString(a, RootNamespace + ".Resx." + bt.Name, key);
        }

        /// <summary>
        /// 在指定程序集中获取指定 BaseName 的资源文件中指定关键字的值。
        /// </summary>
        /// <remarks>
        /// 在调用的时候打开资源，取值后关闭。
        /// </remarks>
        /// <param name="a">程序集名称</param>
        /// <param name="baseName">资源文件的 BaseName</param>
        /// <param name="key">关键字</param>
        /// <returns>关键字对应的值</returns>
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
