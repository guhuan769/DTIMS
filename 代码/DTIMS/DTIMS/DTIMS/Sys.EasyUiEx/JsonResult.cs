using System;
using System.Collections.Generic;
using System.Text;

namespace BJ.Sys.EasyUiEx
{
    /// <summary>
    /// 表示页面调用后台操作，返回给页面Ajax的通用JSON结果对象。
    /// </summary>
    public class JsonResult
    {
        #region 全局变量
        // 结果类型。
        private ResultTypeEnum m_ResultType = ResultTypeEnum.Messager;

        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置JSON结果的类型。
        /// </summary>
        public ResultTypeEnum ResultType
        {
            get { return m_ResultType; }
            set { m_ResultType = value; }
        }
        /// <summary>
        /// 获取或设置操作是否成功。
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 获取JSON结果字符串。
        /// </summary>
        public string Json
        {
            get { return this.ToString(); }
        }

        public MessagerEntity Messager { get; set; }
        public ResponseDataEntity ResponseData { get; set; }

        #endregion

        #region 构造函数
        public JsonResult() : this(ResultTypeEnum.Messager) { }

        public JsonResult(ResultTypeEnum resultType)
        {
            ResultType = resultType;
            Messager = new MessagerEntity();
            ResponseData = new ResponseDataEntity();
        }

        #endregion

        #region 方法重写
        public override string ToString()
        {
            string returnValue = "";
            switch (this.ResultType)
            {
                case ResultTypeEnum.Messager:
                    returnValue = "{{\"resultType\": \"{0}\", \"success\": {1}, \"messager\": {{\"title\": \"{2}\", \"msg\": \"{3}\", \"icon\": \"{4}\" }} }}";
                    returnValue = string.Format(returnValue, new object[]{
                        ResultType.ToString(),
                        Success.ToString().ToLower(),
                        Microsoft.JScript.GlobalObject.escape(Messager.Title),
                        Microsoft.JScript.GlobalObject.escape(Messager.Msg),
                        Messager.Icon.ToString().ToLower()
                    });
                    break;
                case ResultTypeEnum.ResponseData:
                    returnValue = "{{\"resultType\": \"{0}\", \"success\": {1}, \"responseData\": {{\"message\": \"{2}\", \"data\": {3} }} }}";
                    returnValue = string.Format(returnValue, new object[]{
                        ResultType.ToString(),
                        Success.ToString().ToLower(),
                        Microsoft.JScript.GlobalObject.escape(ResponseData.Message),
                        ResponseData.DataContent
                    });
                    break;
                default:
                    break;
            }
            return returnValue;
        }

        #endregion
    }

}
