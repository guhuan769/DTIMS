<%@ Page Language="c#" Inherits="Inphase.Project.CTQS.Descent.AreaInfo" CodeBehind="AreaInfo.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>地区管理</title>
    <%--<link href="../../Css/main.css.aspx" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../../Css/tablestyle.css" />
    <script src="../../Js/common.js.aspx" type="text/javascript"></script>
    <link href="../../Css/CellStyle.css" type="text/css" rel="stylesheet" />
    <script language="javascript" src="../../Js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script language="javascript" src="../../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../../Css/Common/Common.css" type="text/css" rel="stylesheet" />
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
    <style type="text/css">
        <!--
        .STYLE1 {color: #FF0000}
        body {
            margin-left: 0px;
            margin-top: 5px;
            margin-right: 0px;
            margin-bottom: 0px;
        }
        -->
    </style>
    <script type="text/javascript">

        //限制地区名称不能为空
        function editCheck() {
            if (($("#textEdit").val()) == "") {
                $.messager.alert('警告', '地区名称不能为空！', 'info', function () { $("#textEdit").focus(); });
                return false;
            }
        }
    </script>
</head>
<body class="area_mainbg">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--  <div id="searchHead">
                    地区管理</div>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="height: 500" cellspacing="0" cellpadding="0" width="100%">
                <tr valign="top">
                    <td style="width: 250">
                        <div style="overflow: auto; height: 100%; margin: 0px; padding: 0px;">
                            <asp:TreeView ID="trvAreaInfo" runat="server" ParentNodeStyle-ImageUrl="~/Images/TreeView/folder.gif"
                                RootNodeStyle-ImageUrl="~/images/TreeView/control_panel.gif" LeafNodeStyle-ImageUrl="~/images/TreeView/word.gif"
                                ExpandImageUrl="~/images/TreeView/folder_open.gif" ShowLines="true" CssClass="TreeView"
                                SelectedNodeStyle-CssClass="SelectedTreeNode" HoverNodeStyle-CssClass="HoverTreeNode"
                                Height="480px" OnSelectedNodeChanged="trvAreaInfo_SelectedNodeChanged">
                                <ParentNodeStyle ImageUrl="~/Images/TreeView/folder.gif" />
                                <HoverNodeStyle CssClass="HoverTreeNode" />
                                <SelectedNodeStyle CssClass="SelectedTreeNode" ForeColor="Red" />
                                <RootNodeStyle ImageUrl="~/images/TreeView/control_panel.gif" />
                                <LeafNodeStyle ImageUrl="~/images/TreeView/word.gif" />
                            </asp:TreeView>
                        </div>
                    </td>
                    <td style="width: 75%; vertical-align: top">
                        <img alt="地区管理" src="../../images/SystemManage/areasc.gif" width="250" height="64" />
                        <div class="AreaInfo_table">
                            <asp:Label ID="Label1" runat="server" Text="地区名称："></asp:Label>
                            <asp:TextBox ID="textEdit" CssClass="inputField" MaxLength="15" runat="server"></asp:TextBox>
                            <%--<asp:Button ID="buttonEdit" CssClass="button2d" runat="server" Text="修改名称" OnClientClick="return editCheck()"
                                OnClick="buttonEdit_Click" Enabled="false" />--%>
                            <asp:LinkButton ID="buttonEdit" class="easyui-linkbutton" iconcls="icon-edit" Text="修改"
                                runat="server" Enabled="false" OnClientClick="return editCheck();" OnClick="buttonEdit_Click"/>
                        </div>
                        <hr style="text-align: center; size: 1; color: #98B9E7" />
                        <p class="keyinfo">
                            <span class="STYLE1">说明：</span>如果你要修改选中的地区，请在输入地区名称后点击“修改名称”！</p>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
<script type="text/javascript">
    // 重新绑定 Jquery easyUi 控件样式。
    function msAjaxEndRequest(sender, args) {
        $("#buttonEdit").linkbutton();
    }
</script>
<script src="../../Js2/Comm/AjaxTimeout.js" type="text/javascript"></script>
