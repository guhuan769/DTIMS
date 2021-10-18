<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DTIMS.Web.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>通用信息管理系统V1.0</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="description" content="登录页面" /><%--元信息禁止删除，Comm.js文件将使用该值。--%>
    <link rel="stylesheet" type="text/css" href="Css2/Jquery/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="Css2/Jquery/themes/icon.css" />
    <script type="text/javascript" src="Js2/Jquery/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="Js2/Jquery/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="Js2/Jquery/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="Js2/Comm/Comm.js"></script>
    <style type="text/css">
        body
        {
	margin-top: 100px;
	padding: 0px;
	background-repeat: repeat-x;
	background-image: url(Images/Login/login_bg.jpg);
        }
        .input-label
        {
            font: 14px Arial,Tahoma,Helvetica, "宋体" ,sans-serif;
        }
        .input-text-login
        {
            color: #131313;
            font: 12px Arial,Tahoma,Helvetica, "宋体" ,sans-serif;
        }
    .STYLE2 {
	font: 14px Arial,Tahoma,Helvetica, "宋体" ,sans-serif;
	color: #FFFFFF;
	font-weight: bold;
}
.STYLE4 {
	color: #FFFFFF;
	font-weight: bold;
}
    </style>
    <script type="text/javascript">
        // 是否正在提交数据。
        var isSubmit = false;

        //特殊字符验证
        function charValidate(strInput) {
            var myReg = /^\"|\\|\'|<|>|alert|select|=$/;
            if (myReg.exec(strInput)) {
                return true; //包含特殊字符
            }
            else {
                return false; //不包含特殊字符
            }
        }

        function checkData() {
            // 有消息框显示时，若使用Enter键提交数据，则先关闭消息框，并取消提交。
            if ($(".panel-tool-close").length > 0) {
                $(".panel-tool-close").click();
                return false;
            }
            if (isSubmit) {
                return false;
            }
            if ($('#textUserName').val() == '') {
                $.messager.alert('错误', '登录名不能为空！', 'error', function () { $('#textUserName').focus(); });
                return false;
            }
            if ($('#textPwd').val() == '') {
                $.messager.alert('错误', '请输入登录密码！', 'error', function () { $('#textPwd').focus(); });
                return false;
            }

            if (charValidate($('#textUserName').val())) {
                $.messager.alert('错误', '登录名包含特殊字符！', 'error', function () { $('#textUserName').focus(); });
                return false;
            }

            if (charValidate($('#textPwd').val())) {
                $.messager.alert('错误', '密码包含特殊字符！', 'error', function () { $('#textPwd').focus(); });
                return false;
            }

            //document.Form1.style.cursor = 'wait';
            //$('#aLogin').focus();
            //$('#action').val('login');
            //document.Form1.submit();
            return true;
        }

        function doSubmit() {
            if (checkData()) {
                // 先关闭所有消息框。
                $(".panel-tool-close").click();
                $.messager.progress({ title: '请稍等', msg: '正在验证用户...' });
                isSubmit = true;
                // Ajax方式登录系统。
                var aj = new AjaxJson({ data: { action: "login", user: escape($("#textUserName").val()), pwd: escape($("#textPwd").val())} });
                aj.send({
                    complete: function (JR) {
                        if (!JR.success) {
                            isSubmit = false;
                            $.messager.progress("close");
                        }
                    },
                    success: function (JR) {
                        $(".messager-p-msg").html("正在登录系统...");
                        // 登录成功，跳转到主界面。
                        window.top.location.replace("Main.aspx");
                    },
                    error: function (JR) {
                        setPwdFocus();
                    },
                    timeout: function (JR) {
                        $.messager.alert("提示", unescape(JR.message), "error", setPwdFocus);
                    }
                });
            }
            return false;
        }

        function exitSystem() {
            //window.opener = null;
            //window.open('', '_self');
            //window.close();
            if (window == window.top) {
                window.opener = null;
                window.open('about:blank', '_self', ''); // about:blank 使FF下window.close() 无效时转到空白页。
                window.close();
            }
            else {
                window.top.opener = null;
                window.top.open('about:blank', '_top', ''); // about:blank 使FF、chrome下window.top.close() 无效时转到空白页。
                window.top.close();
            }
        }

        function setPwdFocus() {
            $("#textPwd").focus();
            $("#textPwd").select();
        }

        $(document).ready(function () {
            var $username = $("#textUserName");
            var $pwd = $("#textPwd");

            $username.bind("keypress", function (e) {
                switch (e.keyCode) {
                    case 9:
                    case 13:
                        $("#textPwd").focus();
                        e.preventDefault();
                        e.stopPropagation();
                        break;
                    default:
                        break;
                }
            });

            $pwd.bind("keypress", function (e) {
                switch (e.keyCode) {
                    case 13:
                        doSubmit();
                        e.preventDefault();
                        e.stopPropagation();
                        break;
                    default:
                        break;
                }
            });

            if ($username.val() == "") {
                $username.focus();
            }
            else {
                $pwd.focus();
            }
        });
    </script>
</head>
<body>
    <form id="Form1" runat="server" method="post" name="Form1">
    <input type="hidden" name="action" id="action" />
    <table width="750" height="100px" border="0" align="center" cellpadding="0" cellspacing="0">
      <tr>
        <td><img src="Images/Login/login_01.gif" width="985" height="120" /></td>
      </tr>
    </table>
    <table width="985" height="179px" border="0" align="center" cellpadding="0" cellspacing="0" background="Images/Login/login_02.gif">
      <tr>
        <td width="250">&nbsp;</td>
        <td width="500"><table width="280" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td height="60" colspan="2" valign="bottom"><p> <span class="STYLE2">登录名：</span><span style="text-align: left;">
                <input name="textUserName" id="textUserName" class="input-text-login" runat="server"
                                        type="text" size="20" tabindex="0" style="width: 175px; height: 15px;color:#ff6600;" />
            </span> </p></td>
          </tr>
          <tr>
            <td height="40" colspan="2"><span class="input-label"><span style="text-align: left;">
              <span class="STYLE4">密&nbsp;&nbsp;&nbsp;&nbsp;码：</span>
              <input name="textPwd" id="textPwd" runat="server" class="input-text-login" type="password"
                                    size="20" tabindex="0" style="width: 175px; height: 15px; color:#ff6600;" />
            </span></span> </td>
          </tr>
          <tr>
            <td width="180" height="40" align="right">&nbsp;</td>
            <td width="220" align="center"><a href="javascript:void(0);" id="aLogin" class="easyui-linkbutton" onclick="return doSubmit();"
                                icon="icon-login">登 录</a></td>
          </tr>
        </table></td>
      </tr>
    </table>
    <table width="985" height="120px" border="0" align="center" cellpadding="0" cellspacing="0" background="Images/Login/login_03.gif">
      <tr>
        <td>&nbsp;</td>
      </tr>
    </table>
    </form>
</body>
</html>
