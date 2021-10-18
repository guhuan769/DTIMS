// IE 浏览器提速。
/*@cc_on
@if (@_jscript_version < 9)
_d = document;
eval('var document=_d');
@end
@*/

// 附加事件。
function addEventHandler(oTarget, sEventType, fnHandler) {
    if (oTarget.attachEvent) {  // IE、Opera
        oTarget.attachEvent("on" + sEventType, fnHandler);
    }
    else if (oTarget.addEventListener) {    // FF、Opera、Chrome
        oTarget.addEventListener(sEventType, fnHandler, false);
    }
    else {
        oTarget["on" + sEventType] = fnHandler;
    }
}

// 给String对象添加trim()方法
String.prototype.trim = function () {
    return this.replace(/(^(\s|\u3000)*)|((\s|\u3000)*$)/g, "");
}

// String 对象添加验证字符串是否为数字的方法。
String.prototype.isDigits = function () {
    return (this.search(/\D/) == -1);
}

// String 对象添加长度范围检查的方法。
String.prototype.lengthRange = function (min, max) {
    return (this.length >= min && this.length <= max);
}


// 将字符串转换为日期，不能转换返回NaN。有效日期格式：yyyy-MM-dd 或 yyyy/MM/dd；有效时间格式：HH:mm:ss，时间可省略。
function parseDate(str) {
    if (typeof (str) != "string" || str.trim() == "") {
        return NaN;
    }
    var pattern = /^(\d{4})(?:-|\/)(\d{1,2})(?:-|\/)(\d{1,2})(?:\s(\d{1,2})(?::)(\d{1,2})(?::)(\d{1,2}))?$/;
    var matchs = pattern.exec(str);
    if (matchs == null) {
        return NaN;
    }

    for (var i = 0; i < matchs.length; i++) {
        if (typeof (matchs[i]) == "undefined") {
            matchs[i] = "";
        }
    }
    var year = parseInt(matchs[1]);
    var month = parseInt(matchs[2].replace(/^0+(?=\d)/, ""));
    var day = parseInt(matchs[3].replace(/^0+(?=\d)/, ""));
    var hour = parseInt(matchs[4].replace(/^0+(?=\d)/, ""));
    var minute = parseInt(matchs[5].replace(/^0+(?=\d)/, ""));
    var second = parseInt(matchs[6].replace(/^0+(?=\d)/, ""));
    if (year == NaN || month == NaN || day == NaN) {
        return NaN;
    }
    if (isNaN(hour) && isNaN(minute) && isNaN(second)) {
        hour = 0;
        minute = 0;
        second = 0;
    }
    if (isNaN(hour) || isNaN(minute) || isNaN(second)) {
        return NaN;
    }
    var date = new Date(year + "/" + month + "/" + day + " " + hour + ":" + minute + ":" + second);
    if (isNaN(date)) {
        return NaN;
    }
    if (date.getFullYear() != year
    || (date.getMonth() + 1) != month
    || date.getDate() != day
    || date.getHours() != hour
    || date.getMinutes() != minute
    || date.getSeconds() != second) {
        return NaN;
    }
    return date;
}

// 是否都为空白字符，字符串全由空白组成返回true
function isBlankSpace(str) {
    return str.search(/\S/) < 0;
}

// 是否为IE浏览器。
function isIE() {
    if (window.attachEvent) {
        return true;
    }
    else {
        return false;
    }
}

// 日期验证，正确的日期格式返回true
function isValidDate(dateStr) {
    if (typeof (dateStr) != "string" || dateStr.trim() == "") {
        return false;
    }
    var pattern = /^(\d{4})(?:-|\/)(\d{1,2})(?:-|\/)(\d{1,2})$/;
    var matchs = pattern.exec(dateStr);
    if (matchs == null) {
        return false;
    }
    for (var i = 0; i < matchs.length; i++) {
        if (typeof (matchs[i]) == "undefined") {
            matchs[i] = "";
        }
    }
    var year = parseInt(matchs[1]);
    var month = parseInt(matchs[2].replace(/^0+(?=\d)/, ""));
    var day = parseInt(matchs[3].replace(/^0+(?=\d)/, ""));
    var currentDate = new Date(year + "/" + month + "/" + day);
    if (isNaN(currentDate)) {
        return false;
    }
    if (currentDate.getFullYear() == year
    && (currentDate.getMonth() + 1) == month
    && currentDate.getDate() == day) {
        return true;
    }
    return false;
}

// 获取浏览器信息。返回：IsIE - 是否为IE浏览器；IEVersion - IE浏览器版本。
function Browser() {
    var ua = window.navigator.userAgent;
    var _isIE = false;
    var _IEVersion;

    if (ua.indexOf("MSIE") > -1 && ua.indexOf("Opera") < 0) {
        _isIE = true;
    }

    if (_isIE) {
        var pattern = /(?:MSIE\s?)(\d\.\d)(?=;)/;
        var match = pattern.exec(ua);
        if (match != null && match.length == 2) {
            _IEVersion = parseFloat(match[1]);
        }
    }

    return { isIE: _isIE, IEVersion: _IEVersion };
}

//特殊字符验证
function SpecialCharValidate(strInput) {
    var myReg = /^\"|\\|\'|<|>|alert|select|=$/;
    if (myReg.exec(strInput)) {
        return true; //包含特殊字符
    }
    else {
        return false; //不包含特殊字符
    }
}

// 跳转到主页面。
function doGoHomePage() {
    var startIndex = location.protocol.length + 2 + location.host.length + 1;
    var endIndex = location.href.indexOf("/", startIndex);
    var url = location.href.substring(0, endIndex > -1 ? endIndex : startIndex);
    window.top.location.replace(url);
}

//自定义模式对话框
//divObj:页面中的div对象
//title:窗口标题
//isCloseButton : 是否有关闭按钮
//onClose:  关闭对话框时的回调函数，参数1 string 值=（ToolCloseButton：右上角关闭按钮被按下；CloseFunction：Close方法被调用）
var customerDialog = function (title, isCloseButton) {
    var oThis = this;
    var divObjID = "dialogDiv-" + new Date().getTime();
    this.id = divObjID;
    this.iframe = { id: null, onload: null };
    var _divObj = "#" + divObjID;
    var _title = title;
    var _disposed = false;
    var _onClose = null;
    var dialogID = "dialog-iframe-" + divObjID;
    var iframeStr = function (url, option) {
        var scroTmp = "auto";
        if (option.scroll == false) {
            scroTmp = "no";
        }
        //return "<iframe src='" + url + "' width='100%' height='100%' frameborder='0' scrolling='" + scroTmp + "' id='" + dialogID + "'></iframe>";
        return "<iframe width='100%' height='100%' frameborder='0' scrolling='" + scroTmp + "' id='" + dialogID + "'></iframe>";
    }
    var divStr = function () {
        return "<div id=\"" + divObjID + "\" icon=\"icon-save\" style=\"width:730px;height:390px;\"></div>";
    }
    document.body.appendChild($(divStr()).get(0));
    //原始宽
    var oldW = $(_divObj).css("width");
    oldW = filterPX(oldW);
    //原始高
    var oldH = $(_divObj).css("height");
    oldH = filterPX(oldH);

    if (isCloseButton) {
        $(_divObj).dialog({
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $(_divObj).dialog('close');
                }
            }],
            title: _title,
            modal: true
        });
    }
    else {
        $(_divObj).dialog({
            title: _title,
            modal: true
        });
    }

    // 点击右上角关闭按钮时引发事件。
    $(".panel.window .panel-tool-close").bind("click", function () {
        if (_disposed) {
            $(_divObj).dialog("destroy");
        }
        if (typeof (_onClose) == "function") {
            // 关闭原因：右上角关闭按钮被点击。
            _onClose("ToolCloseButton");
        }
    }
    );

    //加载模态对话框
    $(_divObj).dialog('close');
    //打开模式窗口
    //url:窗口中页面的路径
    this.open = function (url, option) {
        $(".panel.window .panel-title").html("数据加载中，请稍后...");
        $(_divObj).find(".dialog-content.panel-body.panel-body-noheader.panel-body-noborder").html(iframeStr(url, option));
        setOption(option);
        //隐藏，当页面内容加载完成再显示出来
        //$("#dialog-iframe").fadeTo("fast", 0);
        this.iframe.id = dialogID;
        $("#" + dialogID).bind("load", function () {
            $(".panel.window .panel-title").html(_title);
            if (typeof (oThis.iframe.onload) == "function") {
                oThis.iframe.onload.apply(this, arguments);
            }
        });
        $("#" + dialogID).attr("src", url);
        $(_divObj).dialog('open');
        // 隐藏阴影。
        $(".window-shadow").hide();
        // 获得焦点。
        $(_divObj).focus();
    }
    this.close = function () {
        $(_divObj).dialog('close');
        if (_disposed) {
            $(_divObj).dialog("destroy");
        }
        // 调用关闭方法时引发事件。
        if (typeof (_onClose) == "function") {
            // 关闭原因：Close方法被调用。
            _onClose("CloseFunction");
        }
    }
    this.dispose = function () {
        $(_divObj).dialog("destroy");
    }

    //设置参数
    function setOption(option) {
        if (option == null) {
            return;
        }
        if (option.title != null && option.title != "")//设置标题
        {
            _title = option.title;
        }
        if (option.width != null && option.width != "")//设置宽
        {
            setWidthOrHeight(option.width, null);
        }
        if (option.height != null && option.height != "")//设置高
        {
            setWidthOrHeight(null, option.height);
        }
        if (option.height != null && option.width != null)//如果自定义高，则重新设置窗口位置
        {
            var parentObj = $(_divObj).parent();
            var windowW = $(document).width();
            var windowH = $(document).height();
            parentObj.css("left", parseInt(windowW) / 2 - parseInt(filterPX(option.width)) / 2);
            parentObj.css("top", parseInt(windowH) / 2 - parseInt(filterPX(option.height)) / 2);
        }

        _disposed = option.disposed;
        //if (option.onClose != null && typeof(option.onClose) == "function") // 关联对话框关闭事件。
        //{
        //    _onClose = option.onClose;
        //}
        _onClose = option.onClose;
    }

    function setWidthOrHeight(w, h) {
        var parentObj = $(_divObj).parent();
        if (w != null && h == null) {
            var parentWidth = filterPX(parentObj.css("width"));
            var totalParentWidth = parseInt(oldW) - parentWidth;
            w = filterPX(w);
            parentObj.css("width", parseInt(w) - totalParentWidth);

            parentObj.find("div").each(function () {
                var curW = this.style.width;
                if (curW != null && curW != "") {
                    var t = parseInt(oldW) - parseInt(curW); //获取偏移量宽
                    t = parseInt(w) - parseInt(t);
                    t = (t < 0 ? 0 : t);
                    this.style.width = t + "px";
                }
            });
            oldW = w;
        }
        else {
            h = filterPX(h);
            parentObj.find("div").each(function () {
                var curH = this.style.height;
                if (curH != null && curH != "") {
                    var t = parseInt(oldH) - parseInt(curH); //获取偏移量宽
                    t = parseInt(h) - parseInt(t);
                    t = (t < 0 ? 0 : t);
                    this.style.height = t + "px";
                }
            });
            oldH = h;
        }
    }

    //过滤过px字符
    function filterPX(str) {
        return str.replace('px', '');
    }
}

// 封装Jquery的Ajax调用。初始化或调用send()方法时可以设置或修改参数。
// 参数格式：{ type: "POST", timeout: 30000, success: function(e) { alert(e.message); } }
// 可用参数列表：
// type:    请求类型，默认"POST"。可取值："GET"、"POST"。
// url:     请求的URL，默认空字符串。空字符串时使用当前页面的URL。
// timeout: 请求超时时间（毫秒），默认60000，即1分钟。
// data:    请求附加参数，默认null。
// urlAppendRandom: 是否在请求URL后添加随机数，默认true。
// success: 操作成功的回调函数。
// error:   操作失败的回调函数。
// parseError:  解析错误时的回调函数。
// timeout: 调用超时的回调函数。
// loginTimeout:    登录超时的回调函数。
// complete:    操作完成的回调函数。
// JR = { code: -1, desc: "", success: false, messager: null, responseData: null }; 回调函数参数。
// code:状态码；desc:与状态码相关的描述信息；success:操作成功或失败；messager:消息框参数；responseData:数据绑定参数。
// code值包含不同的意义：-2 解析错误；-1 初始化；0 成功；1 错误；2 请求超时；3 登录超时。
AjaxJson = function (option) {
    // private fields
    var type = "POST";  // HTTP请求方式。
    var url = "";  // 请求URL。
    var timeout = 60000;   // 请求超时（毫秒）
    var data = null; // 请示附加数据。
    var urlAppendRandom = true;    // 是否在请求URL后添加随机数。
    // callback function
    var success = function (JR) { // 操作成功回调函数。
        if (typeof (JR.responseData) == "object" && JR.responseData != null) {
            $.messager.alert("提示", unescape(JR.responseData.message), "info");
        }
    }
    var error = function (JR) { // 操作失败回调函数。
        if (typeof (JR.responseData) == "object" && JR.responseData != null) {
            $.messager.alert("提示", unescape(JR.responseData.message), "error");
        }
    }
    var parseError = function (JR) {    // 解析错误回调函数。
        if (this.jqXHR.responseText.search(/<meta.*?description.*?登录页面.*?>/i) > -1) {
            doGoHomePage();
        }
        else {
            // JR.responseText.search(/<html.*?>/i) > -1
            var oThis = this;
            var __dialog = new customerDialog("错误", false);
            __dialog.iframe.onload = function () {
                try {
                    this.contentWindow.document.write(oThis.jqXHR.responseText);
                } catch (e) {
                    this.contentWindow.document.write(e.ToString());
                }
            }
            var _b = document.body;
            // 如果在frame框架中，则以父frame框架的大小为准。
            if (window.frameElement) {
                _b = window.frameElement;
            }
            var wrate = (_b.offsetWidth < 1000 ? 0.8 : 0.6);
            var hrate = 0.8;
            var w = _b.offsetWidth * wrate;
            var h = _b.offsetHeight * hrate;
            __dialog.open("about:blank", { title: "错误", width: w + "px", height: h + "px", scroll: true, disposed: true });
        }
    }
    var timeout = function (JR) {   // 调用超时回调函数。
        $.messager.alert("提示", "请求超时，请稍后再试。", "warning");
    }
    var loginTimeout = function (JR) {  // 登录超时回调函数。      
        //window.top.$.messager.alert("提示", "登录超时，请重新登录系统！", "warning", function () {
        doGoHomePage();
        //});
        //$(".messager-window .panel-tool-close").one("click", doGoHomePage);
    }
    var complete = null;    // 操作完成回调函数。所有类型回调都会执行。
    // 保存原始方法。
    var base = {
        success: success,
        error: error,
        parseError: parseError,
        timeout: timeout,
        loginTimeout: loginTimeout,
        complete: complete
    }

    // public fields

    // private methods
    var isFunction = function (func) {  // 判断是否为有效的函数。
        if (typeof (func) == "function" && func != null) {
            return true;
        }
        else {
            return false;
        }
    }

    var setOptions = function (option) {    // 参数设置。

        if (option == undefined || option == null) {
            return;
        }

        if (typeof (option.type) == "string") {
            type = option.type;
        }
        if (typeof (option.url) == "string") {
            url = option.url;
        }
        if (typeof (option.timeout) == "number" && option.timeout >= 0) {
            timeout = option.timeout;
        }
        if (typeof (option.data) == "object") {
            data = option.data;
        }
        if (typeof (option.urlAppendRandom) == "boolean") {
            urlAppendRandom = option.urlAppendRandom;
        }
        if (isFunction(option.success)) {
            success = option.success;
        }
        if (isFunction(option.error)) {
            error = option.error;
        }
        if (isFunction(option.parseError)) {
            parseError = option.parseError;
        }
        if (isFunction(option.timeout)) {
            timeout = option.timeout;
        }
        if (isFunction(option.loginTimeout)) {
            loginTimeout = option.loginTimeout;
        }
        if (isFunction(option.complete)) {
            complete = option.complete;
        }
    }

    var getCurrentPageUrl = function () {
        return location.protocol + "//" + location.host + location.pathname;
    }

    function constructor() {    // 构造函数。
        setOptions(option);
    }

    // public methods
    this.send = function (option) {   // 发送Ajax请求。
        setOptions(option);
        var requestUrl = (url ? url : getCurrentPageUrl());
        requestUrl += (urlAppendRandom ? ((requestUrl.indexOf("?") > 0 ? "&" : "?") + "r=" + Math.random()) : "");
        $.ajax({
            type: type,
            timeout: timeout,
            url: requestUrl,
            cache: false,
            data: data,
            complete: function (jqXHR, textStatus) {    // Ajax调用完成时执行。
                // 返回给回调函数的对象。
                var oThis = { jqXHR: jqXHR, textStatus: textStatus, base: base }
                // 设置回调函数的返回参数。
                // code值包含不同的意义：-2 解析错误；-1 初始化；0 成功；1 错误；2 请求超时；3 登录超时。
                var JR = { code: -1, desc: "", success: false, messager: null, responseData: null };

                var json;
                try {
                    json = eval('(' + jqXHR.responseText + ')');    // 尝试由返回的JSON字符串构建JavaScript对象。
                } catch (e) {
                    JR.code = -2;
                    JR.desc = "JSON对象解析错误：" + e.description;
                    JR.success = false;
                    if (isFunction(complete)) {
                        complete.call(oThis, JR);
                    }
                    if (isFunction(parseError)) {
                        parseError.call(oThis, JR);
                    }
                    return;
                }

                if (textStatus == "success" && json.resultType == "Messager") {
                    JR.success = json.success;
                    JR.messager = json.messager;
                    if (JR.success) {
                        JR.code = 0;
                        if (isFunction(complete)) {
                            complete.call(oThis, JR);
                        }
                        if (JR.messager.msg == "loginTimeout") {
                            if (isFunction(loginTimeout)) {
                                loginTimeout.call(oThis, JR);
                            }
                        }
                        else {
                            if (isFunction(success)) {
                                $.messager.alert(unescape(JR.messager.title), unescape(JR.messager.msg), JR.messager.icon, function () { success.call(oThis, JR); });
                            }
                        }
                    }
                    else {
                        JR.code = 1;
                        if (isFunction(complete)) {
                            complete.call(oThis, JR);
                        }
                        if (JR.messager.msg == "loginTimeout") {
                            if (isFunction(loginTimeout)) {
                                loginTimeout.call(oThis, JR);
                            }
                        }
                        else {
                            if (isFunction(error)) {
                                $.messager.alert(unescape(JR.messager.title), unescape(JR.messager.msg), JR.messager.icon, function () { error.call(oThis, JR); });
                            }
                        }
                    }
                }
                else if (textStatus == "success" && json.resultType == "ResponseData") {
                    JR.desc = json.responseData.message;
                    JR.success = json.success;
                    JR.responseData = json.responseData;
                    if (JR.success) {
                        JR.code = 0;
                        if (isFunction(complete)) {
                            complete.call(oThis, JR);
                        }
                        if (JR.responseData.message == "loginTimeout") {
                            if (isFunction(loginTimeout)) {
                                loginTimeout.call(oThis, JR);
                            }
                        }
                        else {
                            if (isFunction(success)) {
                                success.call(oThis, JR);
                            }
                        }
                    }
                    else {
                        JR.code = 1;
                        if (isFunction(complete)) {
                            complete.call(oThis, JR);
                        }
                        if (JR.responseData.message == "loginTimeout") {
                            if (isFunction(loginTimeout)) {
                                loginTimeout.call(oThis, JR);
                            }
                        }
                        else {
                            if (isFunction(error)) {
                                error.call(oThis, JR);
                            }
                        }
                    }
                }
                else if (textStatus = "timeout") {
                    JR.code = 2;
                    JR.desc = "请求超时。";
                    if (isFunction(complete)) {
                        complete.call(oThis, JR);
                    }
                    if (isFunction(timeout)) {
                        timeout.call(oThis, JR);
                    }
                }
                else {
                    JR.code = 1;
                    JR.desc = "未知错误。";
                    if (isFunction(complete)) {
                        complete.call(oThis, JR);
                    }
                    if (isFunction(error)) {
                        error.call(oThis, JR);
                    }
                }
            }
        });

    }

    constructor();  // 调用构造器。
}

function dofreshSession() {
    try {
        var obj = window.top.parent;
        var i = 0;
        while (i <= 10) {
            //alert(obj.name);
            if (obj.name == '_main1') {
                break;
            }
            else {
                obj = obj.opener.top;
            }
            i++;
        }

        if (obj.name == '_main1') {
            obj.dofreshwindow();
        }
        else {
            //alert('请打开引导主页面');
        }
    } catch (e) {
        //alert('error');
    }
}