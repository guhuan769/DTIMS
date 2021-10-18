using System;
using System.Collections.Generic;
using System.Text;
using Sys.EasyUiEx;
using Sys.EasyUiEx.Data;

namespace Sys.Project.Common
{
    /// <summary>
    /// EasyUi前台AJAX+JSON的消息框与数据的封装类。
    /// </summary>
    public class EasyUiHelper
    {
        #region 构造函数
        public EasyUiHelper()
        {
        }

        #endregion

        #region 消息框

        /// <summary>
        /// 显示消息框。
        /// </summary>
        /// <param name="message">内容。</param>
        /// <exception cref="System.Threading.ThreadAbortException"></exception>
        public void Alert(string message)
        {
            Alert(true, "提示", message, IconEnum.Info);
        }

        /// <summary>
        /// 显示消息框。
        /// </summary>
        /// <param name="message">内容。</param>
        /// <exception cref="System.Threading.ThreadAbortException"></exception>
        public void Alert(string message, IconEnum icon)
        {
            switch (icon)
            {
                case IconEnum.Error:
                case IconEnum.Warning:
                    Alert(false, "提示", message, icon);
                    break;
                case IconEnum.None:
                case IconEnum.Info:
                case IconEnum.Question:
                default:
                    Alert(true, "提示", message, icon);
                    break;
            }
        }

        /// <summary>
        /// 显示消息框。
        /// </summary>
        /// <param name="success">操作是否成功。</param>
        /// <param name="message">内容。</param>
        /// <exception cref="System.Threading.ThreadAbortException"></exception>
        public void Alert(bool success, string message)
        {
            Alert(success, "提示", message, IconEnum.Info);
        }

        /// <summary>
        /// 显示消息框。
        /// </summary>
        /// <param name="success">操作是否成功。</param>
        /// <param name="title">标题。</param>
        /// <param name="message">内容。</param>
        /// <exception cref="System.Threading.ThreadAbortException"></exception>
        public void Alert(bool success, string title, string message)
        {
            Alert(success, title, message, IconEnum.Info);
        }

        /// <summary>
        /// 显示消息框。
        /// </summary>
        /// <param name="success">操作是否成功。</param>
        /// <param name="message">内容。</param>
        /// <param name="icon">消息框类型。</param>
        /// <exception cref="System.Threading.ThreadAbortException"></exception>
        public void Alert(bool success, string message, IconEnum icon)
        {
            Alert(success, "提示", message, icon);
        }

        /// <summary>
        /// 显示消息框。
        /// </summary>
        /// <param name="success">操作是否成功。</param>
        /// <param name="title">标题。</param>
        /// <param name="message">内容。</param>
        /// <param name="icon">消息框类型。</param>
        /// <exception cref="System.Threading.ThreadAbortException"></exception>
        public void Alert(bool success, string title, string message, IconEnum icon)
        {
            try
            {
                JsonResult jr = new JsonResult()
                {
                    ResultType = ResultTypeEnum.Messager,
                    Success = success,
                    Messager =
                    {
                        Title = title,
                        Msg = message,
                        Icon = icon
                    }
                };
                System.Web.HttpContext.Current.Response.Write(jr.Json);
                System.Web.HttpContext.Current.Response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
                //System.Threading.Thread.ResetAbort();
                System.Web.HttpContext.Current.ClearError();
            }
        }

        #endregion

        #region 创建消息框
        /// <summary>
        /// 创建消息框。
        /// </summary>
        /// <param name="success">操作是否成功。</param>
        /// <param name="message">内容。</param>
        /// <returns></returns>
        public string CreateAlert(bool success, string message)
        {
            return CreateAlert(success, "提示", message, IconEnum.Info);
        }

        /// <summary>
        /// 创建消息框。
        /// </summary>
        /// <param name="success">操作是否成功。</param>
        /// <param name="title">标题。</param>
        /// <param name="message">内容。</param>
        /// <returns></returns>
        public string CreateAlert(bool success, string title, string message)
        {
            return CreateAlert(success, title, message, IconEnum.Info);
        }

        /// <summary>
        /// 创建消息框。
        /// </summary>
        /// <param name="success">操作是否成功。</param>
        /// <param name="message">内容。</param>
        /// <param name="icon">消息框类型。</param>
        /// <returns></returns>
        public string CreateAlert(bool success, string message, IconEnum icon)
        {
            return CreateAlert(success, "提示", message, icon);
        }

        /// <summary>
        /// 创建消息框。
        /// </summary>
        /// <param name="success">操作是否成功。</param>
        /// <param name="title">标题。</param>
        /// <param name="message">内容。</param>
        /// <param name="icon">消息框类型。</param>
        /// <returns></returns>
        public string CreateAlert(bool success, string title, string message, IconEnum icon)
        {
            JsonResult jr = new JsonResult()
            {
                ResultType = ResultTypeEnum.Messager,
                Success = success,
                Messager =
                {
                    Title = title,
                    Msg = message,
                    Icon = icon
                }
            };
            return jr.Json;
        }

        #endregion

        #region 数据绑定
        /// <summary>
        /// 向前台AJAX返回操作成功的数据。
        /// </summary>
        /// <param name="data">返回的数据。</param>
        /// <exception cref="System.Threading.ThreadAbortException"></exception>
        public void ResponseData(IData data)
        {
            ResponseData(EasyUiTypeEnum.None, data);
        }

        /// <summary>
        /// 向前台AJAX返回操作成功的数据。
        /// </summary>
        /// <param name="easyUiType">前台使用该数据的EasyUi控件类型。</param>
        /// <exception cref="System.Threading.ThreadAbortException"></exception>
        public void ResponseData(EasyUiTypeEnum easyUiType)
        {
            ResponseData(easyUiType, null);
        }

        /// <summary>
        /// 向前台AJAX返回操作成功的数据。
        /// </summary>
        /// <param name="easyUiType">前台使用该数据的EasyUi控件类型。</param>
        /// <param name="data">返回的数据。</param>
        /// <exception cref="System.Threading.ThreadAbortException"></exception>
        public void ResponseData(EasyUiTypeEnum easyUiType, IData data)
        {
            ResponseData(true, "", easyUiType, data);
        }

        /// <summary>
        /// 向前台AJAX返回空数据。
        /// </summary>
        /// <param name="success">操作是否成功。</param>
        /// <param name="message">有关操作成功或失败的信息。</param>
        /// <exception cref="System.Threading.ThreadAbortException"></exception>
        public void ResponseData(bool success, string message)
        {
            ResponseData(success, message, EasyUiTypeEnum.None);
        }

        /// <summary>
        /// 向前台AJAX返回指定控件类型的空数据。
        /// </summary>
        /// <param name="success">操作是否成功。</param>
        /// <param name="message">有关操作成功或失败的信息。</param>
        /// <param name="easyUiType">前台使用该数据的EasyUi控件类型。</param>
        /// <exception cref="System.Threading.ThreadAbortException"></exception>
        public void ResponseData(bool success, string message, EasyUiTypeEnum easyUiType)
        {
            ResponseData(success, message, easyUiType, null);
        }

        /// <summary>
        /// 向前台AJAX返回数据。
        /// </summary>
        /// <param name="success">操作是否成功。</param>
        /// <param name="message">有关操作成功或失败的信息。</param>
        /// <param name="easyUiType">前台使用该数据的EasyUi控件类型。</param>
        /// <param name="data">返回的数据，用作数据绑定。</param>
        /// <exception cref="System.Threading.ThreadAbortException"></exception>
        public void ResponseData(bool success, string message, EasyUiTypeEnum easyUiType, IData data)
        {
            try
            {
                string json = CreateResponseData(success, message, easyUiType, data);
                System.Web.HttpContext.Current.Response.Write(json);
                System.Web.HttpContext.Current.Response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
                //System.Threading.Thread.ResetAbort();
                System.Web.HttpContext.Current.ClearError();
            }
        }

        #endregion

        #region 创建数据绑定
        /// <summary>
        /// 向前台AJAX返回空数据。
        /// </summary>
        /// <param name="success">操作是否成功。</param>
        /// <param name="message">有关操作成功或失败的信息。</param>
        /// <returns></returns>
        public string CreateResponseData(bool success, string message)
        {
            return CreateResponseData(success, message, EasyUiTypeEnum.None, null);
        }

        /// <summary>
        /// 向前台AJAX返回指定控件类型的空数据。
        /// </summary>
        /// <param name="success">操作是否成功。</param>
        /// <param name="message">有关操作成功或失败的信息。</param>
        /// <param name="easyUiType">前台使用该数据的EasyUi控件类型。</param>
        /// <returns></returns>
        public string CreateResponseData(bool success, string message, EasyUiTypeEnum easyUiType)
        {
            return CreateResponseData(success, message, easyUiType, null);
        }

        /// <summary>
        /// 向前台AJAX返回数据。
        /// </summary>
        /// <param name="success">操作是否成功。</param>
        /// <param name="message">有关操作成功或失败的信息。</param>
        /// <param name="easyUiType">前台使用该数据的EasyUi控件类型。</param>
        /// <param name="data">返回的数据，用作数据绑定。</param>
        /// <returns></returns>
        public string CreateResponseData(bool success, string message, EasyUiTypeEnum easyUiType, IData data)
        {
            if (data == null)
            {
                switch (easyUiType)
                {
                    case EasyUiTypeEnum.Tree:
                    case EasyUiTypeEnum.None:
                        data = ObjectInstance.GetInstance();
                        break;
                    case EasyUiTypeEnum.DataGrid:
                        data = DataGrid.GetInstance();
                        break;
                    case EasyUiTypeEnum.Combobox:
                        data = Combobox.GetInstance();
                        break;
                    default:
                        break;
                }
            }

            JsonResult jr = new JsonResult()
            {
                ResultType = ResultTypeEnum.ResponseData,
                Success = success,
                ResponseData =
                {
                    Message = message,
                    Data = data
                }
            };
            return jr.Json;
        }

        #endregion

        #region 登录超时
        /// <summary>
        /// 向前台页面注册登录超时。
        /// </summary>
        /// <exception cref="System.Threading.ThreadAbortException"></exception>
        public void LoginTimeout()
        {
            try
            {
                string loginTimeout = CreateLoginTimeout();
                System.Web.HttpContext.Current.Response.Write(loginTimeout);
                System.Web.HttpContext.Current.Response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
                System.Web.HttpContext.Current.ClearError();
            }
        }

        #endregion

        #region 创建登录超时
        /// <summary>
        /// 向前台页面注册登录超时。
        /// </summary>
        /// <returns></returns>
        public string CreateLoginTimeout()
        {
            JsonResult jr = new JsonResult()
            {
                ResultType = ResultTypeEnum.Messager,
                Success = false,
                Messager =
                {
                    Title = "",
                    Msg = "loginTimeout",
                    Icon = IconEnum.Error
                }
            };
            return jr.Json;
        }

        #endregion

    } // End of class EasyUiHelper
}
