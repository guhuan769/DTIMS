<%@ Page Language="c#" Inherits="DTIMS.Web.ChangePassword" CodeBehind="ChangePassword.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>�޸�����</title>
    <%--<link href="<%=Request.ApplicationPath%>/css/main.css.aspx" type="text/css" rel="stylesheet" />
    <script src="<%=Request.ApplicationPath%>/js/common.js.aspx" type="text/javascript"></script>
    <link href="<%=Request.ApplicationPath%>/css/CellStyle.css" type="text/css rel=stylesheet" />
    <link href="../../Css/main.css.aspx" type="text/css" rel="stylesheet" />
    <script language="javascript" src="../../Js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script language="javascript" src="../../Js/jquery.easyui.min.js" type="text/javascript"
        charset="gb2312"></script>
    <link href="../../Css/customer.ui.css" type="text/css" rel="stylesheet" />--%>

    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/icon.css" />
    <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/Comm.css" />
    <script type="text/javascript" src="../../Js2/Jquery/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../../Js2/Comm/Comm.js"></script>
    <base target="_self" />
    <script language="javascript" type="text/javascript">
        function docheck() {
            if (document.Form1.textBoxOldPwd.value.trim() == '') {
                $.messager.alert('����', '��������ľ����룡', 'error', function () { document.Form1.textBoxOldPwd.focus(); });
                return false;
            }
            if (document.Form1.textBoxNewPwd.value.trim() == '') {
                $.messager.alert('����', '��������������룡', 'error', function () { document.Form1.textBoxNewPwd.focus(); });
                return false;
            }
            if (document.Form1.textBoxConPwd.value.trim() == '') {
                $.messager.alert('����', '���������ȷ�����룡', 'error', function () { document.Form1.textBoxConPwd.focus(); });
                return false;
            }
            if (document.Form1.textBoxNewPwd.value.trim() != document.Form1.textBoxConPwd.value.trim()) {
                $.messager.alert('����', '�������ȷ�����벻һ�£�', 'error', function () { document.Form1.textBoxNewPwd.focus(); });
                return false;
            }

            if (charValidate(document.getElementById('textBoxOldPwd').value)) {
                $.messager.alert('����', '��������������ַ���', 'error');
                return false;
            }

            if (charValidate(document.getElementById('textBoxNewPwd').value)) {
                $.messager.alert('����', '��������������ַ���', 'error');
                return false;
            }

            if (charValidate(document.getElementById('textBoxConPwd').value)) {
                $.messager.alert('����', 'ȷ��������������ַ���', 'error');
                return false;
            }

            //document.Form1.actionNo.value = '1';
            //document.Form1.submit();
            //return false;
        }

        function dokeydown() {
            var obj = event.keyCode;
            if (obj == '13') {
                docheck();
                return false;
            }
        }

        function dokeydowOld() {
            var obj = event.keyCode;
            if (obj == '13') {
                document.getElementById('textBoxNewPwd').focus();
                return false;
            }
        }

        function dokeydowConfirm() {
            var obj = event.keyCode;
            if (obj == '13') {
                document.getElementById('textBoxConPwd').focus();
                return false;
            }
        }
    </script>
    <style type="text/css">
<!--
.style1 {color: #ff0000}
-->
/*DIV*/
.table2
{
   cursor:default;
   width:98%;
}
.table2_left
{
   color:#333333;
   TEXT-ALIGN:right;
   width:100px;
   float:left;
   padding-top:18px;
   
}
.table2_right
{
   color:#333333;
   TEXT-ALIGN:left;
   border:1px #3578CA solid;
   padding-top:5px;
   padding-bottom:5px;
   margin-top:2px;
}

TABLE.table2 {
   cursor:default;
   BACKGROUND-COLOR: #DFDFDF;
   border:#3578CA 1px solid;
   margin-left:10px;
}

TABLE.table2 TD {
   padding:5px;
   BACKGROUND-COLOR: #FFFFFF;
   border-bottom:#D5E3EE 1px solid;
}

TABLE.table2 TD.header
{
   padding:5px;
   TEXT-ALIGN: right;
   font-weight: bold;
   BACKGROUND-COLOR: #D8EDFF;
}

TABLE.table2 TD.header1{
   padding:5px;
   TEXT-ALIGN: left;
   font-weight: bold;
   BACKGROUND-COLOR: #EAF7E2;
}
        </style>
</head>
<body class="normal_mainbg">
    <form id="Form1" method="post" defaultfocus="textBoxOldPwd" runat="server" name="Form1">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <input type="hidden" name="actionNo">
            <%--<div id="searchHead">
                    �޸�����</div>--%>
            <table width="780" border="0" cellspacing="0" cellpadding="0" class="table2" style="margin-top: 20px;">
                <tr>
                    <td width="29%" height="50">
                    </td>
                    <td width="71%" height="30">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td height="30" align="right">
                        ����<span class="STYLE1">��</span>���룺
                    </td>
                    <td height="30">
                        <input class="inputField" id="textBoxOldPwd" type="password" size="20" name="textBoxOldPwd"
                            onkeydown="dokeydowOld();" runat="server">
                    </td>
                </tr>
                <tr>
                    <td height="30" align="right">
                        <span class="STYLE1">��</span>���룺
                    </td>
                    <td height="30">
                        <input class="inputField" id="textBoxNewPwd" type="password" size="20" name="textBoxNewPwd"
                            onkeydown="dokeydowConfirm();" runat="server">
                    </td>
                </tr>
                <tr>
                    <td height="30" align="right">
                        ȷ��<span class="STYLE1">��</span>���룺
                    </td>
                    <td height="30">
                        <input class="inputField" id="textBoxConPwd" type="password" size="20" name="textBoxConPwd"
                            runat="server" onkeydown="dokeydown();">
                    </td>
                </tr>
                <tr>
                    <td height="50">
                        &nbsp;
                    </td>
                    <td height="30">
                        <%--   <asp:Button ID="btnOk" CssClass="button2d" OnClientClick="return docheck();" runat="server"
                            Text=" ȷ  �� " OnClick="btnOk_Click" />--%>
                        <asp:LinkButton ID="btnOk" class="easyui-linkbutton" iconcls="icon-save" Text="ȷ��"
                            runat="server" OnClientClick="return docheck();" OnClick="btnOk_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
<script type="text/javascript">
    // ���°� Jquery easyUi �ؼ���ʽ��
    function msAjaxEndRequest(sender, args) {
        $("#btnOk").linkbutton();
    }
</script>
<script src="../../Js2/Comm/AjaxTimeout.js" type="text/javascript"></script>
