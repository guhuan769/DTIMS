<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="Inphase.CTQS.Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>通用信息管理系统V1.0</title>
    <link rel="stylesheet" type="text/css" href="Css2/Jquery/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="Css2/Jquery/themes/icon.css" />
    <link rel="Stylesheet" type="text/css" href="Css2/Comm/Comm.css" />
    <link rel="Stylesheet" type="text/css" href="Css2/Frame.css" />
    <script type="text/javascript" src="Js2/Jquery/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="Js2/Jquery/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="Js2/Jquery/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="Js2/Comm/Comm.js"></script>
    <script type="text/javascript">
        var currentTabTitleOnMouse = "";  // 当前鼠标所在选项卡标题的名称。

        var init = function () {
            // 设置显示用户信息区域的格式。同时添加以兼容IE各版本和其它浏览器。
            $(".panel-title .menu-container").css("text-align", "right");
            $(".panel-title .menu-container").get(0).parentNode.style.textAlign = "right";

            // 绑定左边菜单鼠标单击/进入/移出/按下/释放事件。
            $(".leftMenu li").each(function () {
                $(this).bind("click", function () {
                    activeTab(this.attributes["url"].value, this.innerHTML);
                });

                $(this).bind("mouseover", function () {
                    this.className = "lmmouseover";
                });

                $(this).bind("mouseout", function () {
                    this.className = "lmmouseout";
                });

                $(this).bind("mousedown", function () {
                    this.className = "lmmousedown";
                });

                $(this).bind("mouseup", function () {
                    this.className = "lmmouseup";
                });

            });

            $("#tt").tabs({
                onAdd: function (title) { // 添加选项卡时，重新绑定所有选项卡标题的鼠标右键上下文菜单事件。
                    bindTabs();
                },
                onSelect: function (title) { // 切换选项卡时，若有EasyUi对话框则使其居中显示。
                    var tab = $("#tt").tabs("getTab", title);
                    var cIframe = tab.find("iframe")[0];
                    var c$ = cIframe.contentWindow.$;
                    if (typeof (c$) == "function") {
                        var dialog = c$(".panel.window.messager-window");
                        var shadow = c$(".window-shadow");
                        var mask = c$(".window-mask");
                        var cw, ch;
                        cw = cIframe.offsetWidth;
                        ch = cIframe.offsetHeight;
                        var pattern = /\d+/;
                        var dw, dh;
                        dw = parseInt(pattern.exec(dialog.css("width")));
                        dh = parseInt(pattern.exec(dialog.css("height")));
                        if (!isNaN(dw) && !isNaN(dh)) {
                            //shadow.css("display", "none");
                            mask.css("width", cIframe.scrollWidth);
                            mask.css("height", cIframe.scrollHeight);
                            var b = new Browser();
                            if (b.isIE) {
                                c$(".messager-body.panel-body.panel-body-noborder.window-body").css("width", dw - 22);
                            }
                            dialog.css("left", (cw - dw) / 2);
                            dialog.css("top", (ch - dh) / 2);
                            shadow.css("left", (cw - dw) / 2);
                            shadow.css("top", (ch - dh) / 2);
                        }
                    }
                }
            });

            // 进行第一次选项卡标题鼠标右键上下文菜单事件。
            bindTabs();

            // 绑定选项卡右键菜单。
            $(".tabs-wrap").bind("contextmenu", function (e) {
                // 绑定菜单显示事件，初始菜单状态。
                $("#mm").menu({
                    onShow: function () {
                        if (currentTabTitleOnMouse != "") {
                            $("#menuCloseMe").css("color", "#000");
                            $("#menuCloseExcMe").css("color", "#000");
                        }
                        else {
                            $("#menuCloseMe").css("color", "#BBB");
                            $("#menuCloseExcMe").css("color", "#BBB");
                        }
                    }
                });
                // 绑定菜单关闭事件，清空鼠标指针所在的选项卡标题。
                $("#mm").menu({
                    onHide: function () {
                        currentTabTitleOnMouse = "";
                    }
                });
                // 显示上下文右键菜单。
                $("#mm").menu("show", { left: e.pageX, top: e.pageY });
                return false;
            });

            // 初始化右上角隐藏顶部Logo按钮。
            $("#topNorthHide").css("opacity", 0.2);
            $("#topInnerBox").bind("dblclick", function () {
                $("#topNorthHide").click();
            });
            $("#topNorthHide").bind("mouseover", function () {
                $(this).animate({ opacity: 1 }, "fast");
            });
            $("#topNorthHide").bind("mouseout", function () {
                $(this).animate({ opacity: 0.2 }, "fast");
            });
            $("#topNorthHide").bind("click", function () {
                $("body").layout("collapse", "north");
                // 设置右上角展开按钮的工具提示属性。
                $(".panel-tool .layout-button-down").attr("title", "展开顶部");
            });

        }

        // 文档初始化。
        $(document).ready(function () {
            init();
        });

        // 打开登录对话框。
        function login(fn) {
            window.top.location.replace("login.aspx");
        }

        // 用户操作，登录/注销/退出等。
        // 参数：name - 操作类型。
        function userOper(name) {
            if (name == "登录") {
                login(null);
            }
            else if (name == "注销") {
                $.messager.confirm('确认', '确定注销登录吗？', function (r) {
                    if (r) {
                        window.top.location.replace("Login.aspx?action=logout");
                    }
                });
            }
            else if (name == "退出") {
                $.messager.confirm('确认', '确定退出系统吗？', function (r) {
                    if (r) {
                        window.top.location.replace("Login.aspx?action=exit");
                    }
                });
            }
         
            else if (name == "帮助") {
                //$.messager.alert('提示', "帮助信息正在建设中...", 'info');
                window.open("Help/Help.html", "CTQS_V2_帮助");
            }
            else {
                $.messager.alert('提示', name, 'info');
            }
            return false;
        }

        // 在选项卡中打开页面。
        // 参数：pageUrl - 页面地址；title - 选项卡标题；reload - 是否重新打开（当已经打开时）。
        function activeTab(pageUrl, title, reload) {
            var exists = $("#tt").tabs("exists", title);
            if (exists && !reload) {
                $("#tt").tabs("select", title);
            }
            else {
                if (exists) {
                    $("#tt").tabs("close", title);
                }
                // 显示进度条。
                $.messager.progress({ title: '请稍等', msg: '读取数据中...' });
                var iframeId = "iframe" + Math.random().toString().replace("0.", ""); // 生成iframe的ID。
                $("#tt").tabs("add", {
                    title: title,
                    closable: true,
                    content: '<iframe id="' + iframeId + '" src="' + pageUrl + '" width="100%" height="100%" frameborder="0" scrolling="auto" marginwidth="0" marginheight="0" onload="javascript:closeProgress(this);"></iframe>'
                });
            }
        }

        // 隐藏进度条。
        function closeProgress(iframe) {
            //setTimeout(function () { $.messager.progress("close"); }, 500);
            $.messager.progress("close");
            iframe.focus();
        }

        // 选项卡标题鼠标右键上下文菜单事件的具体函数。
        function contextmenuBindTab(e) {
            currentTabTitleOnMouse = $(this).find("span.tabs-title.tabs-closable").get(0).innerHTML;
        }

        // 绑定选项卡标题的鼠标右键上下文菜单事件。
        function bindTabs() {
            var $tabs_inner = $("a.tabs-inner");
            $tabs_inner.unbind("contextmenu", contextmenuBindTab);
            $tabs_inner.bind("contextmenu", contextmenuBindTab);
        }

        // 关闭选项卡。
        function closeTab() {
            if (typeof (currentTabTitleOnMouse) == "string" && currentTabTitleOnMouse != "") {
                $("#tt").tabs("close", currentTabTitleOnMouse);
            }
        }

        // 关闭除此之外的所有选项卡。
        function closeAllTabExclusiveMe() {
            $($("#tt").tabs("tabs")).each(function () {
                if (typeof (currentTabTitleOnMouse) == "string" && currentTabTitleOnMouse != "" && this.panel("options").title != currentTabTitleOnMouse) {
                    $("#tt").tabs("close", this.panel("options").title);
                }
            });
        }

        // 关闭所有选项卡。
        function closeAllTab() {
            $($("#tt").tabs("tabs")).each(function () {
                $("#tt").tabs("close", this.panel("options").title);
            });
        }
    </script>
</head>
<body class="easyui-layout">
    <script type="text/javascript">        //加载页面时，显示提示信息。
        document.body.style.overflow = "hidden";    // 先隐藏滚动条，防止页面加载时出现滚动条。
        var divObj = document.createElement("div"); // 创建外层div元素节点，页面未加载完成时，遮罩整个页面。
        var divCss = divObj.style;  // 得到样式对象。
        divObj.id = "SplashScreen"; // 设置id属性。
        divCss.position = "absolute";   // 设置样式。
        divCss.top = "0";
        divCss.left = "0";
        divCss.width = "100%";
        divCss.height = "100%";
        divCss.cursor = "wait";
        divCss.backgroundColor = "#CCC";
        divCss.textAlign = "center";
        divCss.zIndex = "9999";
        var divSubObj = document.createElement("div");  // 创建内层div元素节点，包含提示文本。
        var divSubCss = divSubObj.style;    // 得到样式对象。
        divSubCss.position = "absolute";    // 设置样式。
        divSubCss.top = "50%";
        divSubCss.left = "50%";
        divSubCss.width = "200px";
        divSubCss.height = "20px";
        divSubCss.lineHeight = "20px";
        divSubCss.margin = "-10px 0px 0px -100px";
        divSubCss.textAlign = "center";
        divSubCss.zIndex = "9999";
        var textObj = document.createTextNode("系统载入中，请稍等...");  // 创建文本元素节点，显示页面加载前的提示信息。
        document.body.appendChild(divObj);  // 将外层div元素追加到页面body元素下。
        divObj.appendChild(divSubObj);  // 将内层div元素追加到外层div元素下。
        divSubObj.appendChild(textObj); // 将文本节点追加到内层div元素下。
        document.onreadystatechange = function () { // 页面加载完成后隐藏外层div元素。
            if (this.readyState == 4 || this.readyState == "loaded" || this.readyState == "complete") {
                divObj.style.display = "none";
                document.body.removeChild(divObj);
                //document.body.style.overflow = "";
            }
        }
    </script>
    <div region="north" border="false" style="height: 65px; overflow: hidden; background-image: url(Images/Main/main_topbg.jpg);
        background-repeat: repeat-x;">
        <div id="topInnerBox" style="overflow: hidden;">
            <img src="Images/Main/logo.jpg" width="998" height="65" alt="logo" />
            <div id="topNorthHide" title="折叠顶部" class="layout-button-up" style="position: absolute;
                top: 6px; right: 6px; width: 16px; height: 16px; cursor: pointer; z-index: 500;">
            </div>
        </div>
    </div>
    <div region="west" split="true" title="菜单" style="width: 150px; padding: 0;">
        <div class="easyui-accordion" fit="true" animate="false" border="false">
            <%-- 生成左边菜单项 --%>
            <%= this.CreateLeftMenu() %>
        </div>
    </div>

    <div region="center" id="MainContainer" title='<%= this.CreateUserMenu() %>'>
        <div id="tt" class="easyui-tabs" fit="true" border="false" plain="true" style="background: url(Images2/Comm/index_bg.jpg) no-repeat left bottom;">
            
        </div>
    </div>
    <div id="mm" class="easyui-menu" style="width: 140px;">
        <div id="menuCloseMe" onclick="javascript:closeTab();">
            关闭</div>
        <div id="menuCloseExcMe" onclick="javascript:closeAllTabExclusiveMe();">
            关闭其他窗口</div>
        <div class="menu-sep">
        </div>
        <div onclick="javascript:closeAllTab();">
            全部关闭</div>
    </div>
</body>
</html>
