<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrivilegeUserList.aspx.cs"
    Inherits="DTIMS.Web.PrivilegeUserList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户列表</title>
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/icon.css" />
    <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/Comm.css" />
    <script type="text/javascript" src="../../Js2/Jquery/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../../Js2/Comm/Comm.js"></script>
    <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/GridView.css" />
    <script type="text/javascript" src="../../Js2/Comm/GridView.js"></script>
    <script type="text/javascript" language="javascript">
        function closeWindow(r) {
            window.parent.CloseWindow(r);
        }
    </script>
</head>
<body style="margin: 0px; padding: 0px;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="c-datagrid-container">
                <asp:GridView ID="grdUserList" runat="server" CssClass="datalist" BorderStyle="None"
                    BorderWidth="1px" CellPadding="3" GridLines="Horizontal" AllowSorting="True"
                    PageSize="11" AutoGenerateColumns="False" OnSorting="grdUserList_SortCommand"
                    Width="100%">
                    <Columns>
                        <asp:BoundField DataField="User_Name" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                            SortExpression="User_Name" FooterText="User_Name" HeaderText="姓名" ItemStyle-Width="90px">
                        </asp:BoundField>
                        <asp:BoundField DataField="User_Login" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                            SortExpression="User_Login" FooterText="User_Login" HeaderText="登录名" ItemStyle-Width="100px">
                        </asp:BoundField>
                        <asp:BoundField DataField="PrivGroup_Name" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                            SortExpression="PrivGroup_Name" FooterText="PrivGroup_Name" HeaderText="权限组名称"
                            ItemStyle-Width="130px"></asp:BoundField>
                        <asp:BoundField DataField="MainArea_Name" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                            SortExpression="MainArea_Name" FooterText="MainArea_Name" HeaderText="地区" ItemStyle-Width="80px">
                        </asp:BoundField>
                        <asp:BoundField DataField="UserRole_Name" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                            SortExpression="UserRole_Name" FooterText="UserRole_Name" HeaderText="角色" ItemStyle-Width="70px">
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="orderbyname" runat="server" Value="Sys_PrivilegeGroup.PrivGroup_ID" />
                <!--要排序的列字段-->
                <asp:HiddenField ID="orderbytype" runat="server" Value="desc" />
                <!--排序类型：ASC or DESC-->
                <asp:HiddenField ID="maxpage" runat="server" />
                <!--最大页码-->
                <asp:HiddenField ID="filterwords" runat="server" />
                <div class="c-datagrid-pager c-pagination">
                    <table cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lbtnPagingFirst" Enabled="false" runat="server" CommandName="Page"
                                        CommandArgument="First" OnCommand="GotoThePage" icon="pagination-first" class="c-l-btn c-l-btn-plain c-l-btn-disabled">
                                        <span class="c-l-btn-left"><span class="c-l-btn-text"><span class="c-l-btn-empty c-pagination-first">
                                            &nbsp;</span></span></span></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnPagingPrev" Enabled="false" runat="server" CommandName="Page"
                                        CommandArgument="Prev" OnCommand="GotoThePage" icon="pagination-prev" class="c-l-btn c-l-btn-plain c-l-btn-disabled">
                                        <span class="c-l-btn-left"><span class="c-l-btn-text"><span class="c-l-btn-empty c-pagination-prev">
                                            &nbsp;</span></span></span></asp:LinkButton>
                                </td>
                                <td>
                                    <div class="c-pagination-btn-separator">
                                    </div>
                                </td>
                                <td>
                                    <span style="padding-left: 6px;">第</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPageIndex" runat="server" Text="1" size="2" CssClass="easyui-numberbox c-pagination-num" />
                                    <asp:LinkButton ID="lbtnPageIndex" CommandName="Page" CommandArgument="Num" Text=""
                                        runat="server" OnCommand="GotoThePage" Style="display: none;" />
                                    <asp:HiddenField ID="hidPageIndex" runat="server" Value="1" />
                                </td>
                                <td>
                                    <span style="padding-right: 6px;">共<asp:Label ID="lblPageCount" Text="0" runat="server" />页</span>
                                </td>
                                <td>
                                    <div class="c-pagination-btn-separator">
                                    </div>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnPagingNext" Enabled="false" runat="server" CommandName="Page"
                                        CommandArgument="Next" OnCommand="GotoThePage" icon="pagination-next" class="c-l-btn c-l-btn-plain c-l-btn-disabled">
                                        <span class="c-l-btn-left"><span class="c-l-btn-text"><span class="c-l-btn-empty c-pagination-next">
                                            &nbsp;</span></span></span></asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbtnPagingLast" Enabled="false" runat="server" CommandName="Page"
                                        CommandArgument="Last" OnCommand="GotoThePage" icon="pagination-last" class="c-l-btn c-l-btn-plain c-l-btn-disabled">
                                        <span class="c-l-btn-left"><span class="c-l-btn-text"><span class="c-l-btn-empty c-pagination-last">
                                            &nbsp;</span></span></span></asp:LinkButton>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="c-pagination-info">
                        显示<asp:Label ID="lblRowStart" Text="1" runat="server" />到<asp:Label ID="lblRowEnd"
                            Text="0" runat="server" />,共<asp:Label ID="lblRowCount" Text="0" runat="server" />记录</div>
                    <div style="clear: both;">
                    </div>
                </div>
                <%--<table width="100%" border="0" cellpadding="0" cellspacing="0" style="text-align: center;"
                    class="datalist">
                    <tbody id="pageTable" runat="server">
                        <tr>
                            <td style="text-align: right; color: #ff6600;" colspan="7">
                                <asp:Button ID="btn_first" runat="server" Text="" ToolTip="首页" CommandArgument="首页"
                                    OnCommand="GotoThePage" CssClass="pagination_first" Width="19px" />
                                <asp:Button ID="btn_pre" runat="server" Text="" ToolTip="上一页" CommandArgument="上一页"
                                    OnCommand="GotoThePage" CssClass="pagination_prev" Width="19px" />
                                <asp:Button ID="btn_next" runat="server" Text="" ToolTip="下一页" CommandArgument="下一页"
                                    OnCommand="GotoThePage" CssClass="pagination_next" Width="19px" />
                                <asp:Button ID="btn_end" runat="server" Text="" ToolTip="末页" CommandArgument="末页"
                                    OnCommand="GotoThePage" CssClass="pagination_last" Width="19px" />
                                &nbsp;&nbsp;&nbsp;&nbsp; 当前是第<asp:Literal ID="pagenum" runat="server" Text="1"></asp:Literal>页
                                转到<asp:DropDownList ID="dropGoToPageNumber" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropGoToPageNumber_SelectedIndexChanged">
                                </asp:DropDownList>
                                页 共有<%=max_page_num %>页
                                <%=page_rows%>
                                条/页 共<asp:Label ID="labMaxCount" runat="server" Text="0"></asp:Label>
                                条数据
                            </td>
                        </tr>
                    </tbody>
                </table>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
<script type="text/javascript">
    function msAjaxBeginRequest(sender, args) {
        $.messager.progress({ title: '请稍等', msg: '提交数据中...' });
    }

    // 重新绑定 Jquery easyUi 控件样式。
    function msAjaxEndRequest(sender, args) {
        $.messager.progress("close");
    }
</script>
<script src="../../Js2/Comm/AjaxTimeout.js" type="text/javascript"></script>
