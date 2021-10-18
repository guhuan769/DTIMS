<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleView.aspx.cs" Inherits="DTIMS.Web.RoleView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <script language="javascript" src="../../Js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script language="javascript" src="../../Js/jquery.easyui.min.js" type="text/javascript"
        charset="gb2312"></script>
    <script type="text/javascript" src="<%=Request.ApplicationPath%>/js/common.js.aspx"></script>
    <link href="../../Css/main.css.aspx" type="text/css" rel="stylesheet" />
    <link href="../../Css/customer.ui.css" type="text/css" rel="stylesheet" />
    <link href="../../Css/Common/Common.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">

        function closeDia() {
            window.parent.closeDialog("false");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="divPanel" runat="server" class="easyui-tabs" style="background-color: #ddeeff;">
        <div title="用户权限" style="display: none;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:TreeView ID="trvFunctionItem" runat="server" ShowLines="true" CssClass="TreeView"
                        SelectedNodeStyle-CssClass="SelectedTreeNode" HoverNodeStyle-CssClass="HoverTreeNode"
                        Height="270px" Width="370px" EnableClientScript="false">
                    </asp:TreeView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div title="用户所属权限组" style="background-color: #ddeeff;">
            <asp:TreeView ID="trvGroup" runat="server" ShowLines="true" CssClass="TreeView"
                SelectedNodeStyle-CssClass="SelectedTreeNode" HoverNodeStyle-CssClass="HoverTreeNode"
                Height="270px" Width="370px">
            </asp:TreeView>
        </div>
    </div>
    </form>
</body>
</html>
<script src="../../Js2/Comm/AjaxTimeout.js" type="text/javascript"></script>
