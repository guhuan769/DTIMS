<%@ Page Language="c#" Inherits="DTIMS.Web.LogInfo" CodeBehind="LogInfo.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>日志详细信息</title>
    <link href="<%=Request.ApplicationPath%>/css/main.css.aspx" type="text/css" rel="stylesheet">

    <script src="<%=Request.ApplicationPath%>/js/common.js.aspx"></script>

    <script language="javascript" src="../../Js/jquery-1.4.2.min.js" type="text/javascript"></script>

</head>
<body>
    <form method="post" enctype="multipart/form-data" id="Form1" style="margin-left: -8px" runat="server">
        <div>
            <table visible="false" border="0" width="100%" align="center" cellpadding="0" cellspacing="0"
                class="table_Dialog" id="SelectTeleNum">
                <tr>
                    <td class="header" style="width: 200px" align="center">   &nbsp;日志ID：</td>
                    <td>
                        <asp:Label ID="labelLogID" runat="server" Width="156px"></asp:Label></td>
                    <td class="header" align="center" style="width: 150px" nowrap>记录项目名称：</td>
                    <td style="width: 200px">
                        <asp:Label ID="labelItemName" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="header" align="center">人员名称：</td>
                    <td>
                        <asp:Label ID="labelOprLogin" runat="server"></asp:Label></td>
                    <td class="header" align="center">操作时间：</td>
                    <td>
                        <asp:Label ID="labelDate" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td height="26" align="center" class="header">操作方式：</td>
                    <td>
                        <asp:Label ID="labeloperMode" runat="server"></asp:Label></td>
                    <td align="center" class="header">  IP地址：</td>
                    <td>
                        <asp:Label ID="lblLOG_Client_IP" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="header" style="height: 56px" align="center">日志内容：</td>
                    <td style="height: 156px" colspan="3">
                        <textarea id="textContent" readonly rows="10" cols="40" style="width: 99%" name="textContent" runat="server"></textarea></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
<script src="../../Js2/Comm/AjaxTimeout.js" type="text/javascript"></script>
