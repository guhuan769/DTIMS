<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IPEdit.aspx.cs" Inherits="BJ.DTIMS.Sys.IPMnager.IPEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>配置信息</title>
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/icon.css" />
    <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/Comm.css" />
    <script type="text/javascript" src="../../Js2/Jquery/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../../Js2/Comm/Comm.js"></script>
    <script type="text/javascript">
        function validateForm() {
            //$.messager.alert("提示", "您的IP输入错误!", "info", function () { }); return false;
            var flag = isValidIP($("#txtIP").val());
            if (flag == false) {
                $.messager.alert("提示", "您的IP输入错误!", "info", function () {
                    $("#txtIP").get(0).focus();
                }); return false;
            }
            if ($("#txtIP").val() == "") {
                $.messager.alert("提示", "IP不能为空!", "info", function () {
                    $("#txtIP").get(0).focus();
                }); return false;
            }

            if ($("#txtProt").val() == "") {
                $.messager.alert("提示", "端口不能为空!", "info", function () {
                    $("#txtProt").get(0).focus();
                }); return false;
            }

           //$.ajax({ //调用的静态方法，所以下面必须参数按照下面来
           //     url: 'IPEdit.aspx/IPUpdate',
           //     type: 'post',
           //     contentType: "application/json",
           //     dataType: 'json',
           //     data: "{}", //必须的，为空的话也必须是json字符串
           //    success: function (data) {//这边返回的是个对象
           //         if (data != null)
           //             $.messager.progress({ title: '提示', msg: data });
           //     }   
           // });
        }

        function isValidIP(ip) {
            var reg = /^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/
            return reg.test(ip);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
      <table class="table_Dialog" style="width: 100%; height: 345px;">
            <tr>
                <td class="header" style="width: 80px; border-left: 0px">IP：
                </td>
                <td>
                    <asp:TextBox ID="txtIP" runat="server" Width="200" MaxLength="20"></asp:TextBox>
                    <span style="color: red">*</span>
                </td>
            </tr>
            <tr id="pwd1" runat="server">
                <td class="header">端口：
                </td>
                <td>
                    <asp:TextBox ID="txtProt" runat="server" Width="200" MaxLength="10"></asp:TextBox>
                    <span style="color: red">*</span>
                </td>
            </tr>
            <tr>
                <td class="header">科目：
                </td>
                <td>
                    <asp:DropDownList ID="ddkm" Width="200" runat="server">
                        <asp:ListItem Text="科目一" Value="1"></asp:ListItem>
                        <asp:ListItem Text="科目二" Value="2"></asp:ListItem>
                        <asp:ListItem Text="科目三" Value="3"></asp:ListItem>
                        <asp:ListItem Text="科目四" Value="4"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="header"></td>
                <td style="text-align: center">
                    <%--<asp:Button ID="btnUpdate" class="easyui-linkbutton" runat="server" Text="修   改" OnClientClick="javascript:return validateForm();" />--%>
                    <asp:LinkButton ID="btnUpdate" class="easyui-linkbutton" Text="修   改" runat="server" OnClientClick="return validateForm();" OnClick="btnUpdate_Click" />
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
<script type="text/javascript">
    function msAjaxBeginRequest(sender, args) {
        $.messager.progress({ title: '请稍等', msg: '提交数据中...' });
    }

    // 重新绑定 Jquery easyUi 控件样式。
    function msAjaxEndRequest(sender, args) {
         $("#btnAdd").linkbutton();$("#btnUpdate").linkbutton();
        $.messager.progress("close");
    }
</script>
