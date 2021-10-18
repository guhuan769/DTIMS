if (typeof (Sys) != "undefined") {
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    if (prm != null) {
        function BeginRequestHandler(sender, args) {
            if (typeof (msAjaxBeginRequest) == "function") {
                msAjaxBeginRequest.call(null, sender, args);
            }
        }

        function EndRequestHandler(sender, args) {
            // 执行回调函数，重新绑定EasyUi控件样式。
            if (typeof (msAjaxEndRequest) == "function") {
                msAjaxEndRequest.call(null, sender, args);
            }

            // 判断页面From身份验证是否超时。
            if (args.get_error() != undefined) {
                if (args.get_error().message.substring(0, 51) == "Sys.WebForms.PageRequestManagerParserErrorException") {
                    window.top.location.reload();   //出现Session丢失时的错误处理，可以自己定义。
                    args.set_errorHandled(true);
                }
                //else {
                //    alert("发生错误！原因可能是数据不完整，或网络延迟。");   //其他错误的处理。
                //}
                //args.set_errorHandled(true);
            }
        }

        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
    }
}