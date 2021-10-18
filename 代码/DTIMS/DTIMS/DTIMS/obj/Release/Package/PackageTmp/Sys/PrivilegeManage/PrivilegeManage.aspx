<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrivilegeManage.aspx.cs"
    Inherits="DTIMS.Web.PrivilegeManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>权限组管理</title>
    <%--  <link href="../../Css/main.css.aspx" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../../Css/tablestyle.css" />
    <script src="<%=Request.ApplicationPath%>/js/common.js.aspx" type="text/javascript"></script>
    <link href="../../css/treestyle.css" type="text/css" rel="stylesheet"/>
    <script language="javascript" src="../../Js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script language="javascript" src="../../Js/jquery.easyui.min.js" type="text/javascript" charset="gb2312"></script>
    <link href="../../Css/customer.ui.css" type="text/css" rel="stylesheet" />--%>
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/icon.css" />
    <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/Comm.css" />
    <script type="text/javascript" src="../../Js2/Jquery/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../../Js2/Comm/Comm.js"></script>
    <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/GridView.css" />
    <script type="text/javascript" src="../../Js2/Comm/GridView.js"></script>
    <script type="text/javascript">
        var cDialog = null;
        var cDialogUser = null;
        $(document).ready(function () {
            //初始化模态对话框
            cDialog = new customerDialog("权限组属性", false);
            cDialogUser = new customerDialog("用户查看", true);
            initDelEvent();
        });

        function initDelEvent() {
            //停止当前删除操作,加上确认提示框
            $(".imgClass").each(function () {
                var href = $(this).attr("href");
                href = href.replace("javascript:__doPostBack('", "");
                href = href.replace("','')", "");
                $(this).attr("href", "#");
                //添加点击事件
                $(this).click(function () {
                    $.messager.confirm('确认', '确定删除吗?', function (r) {
                        if (r) {
                            __doPostBack(href, "");
                        }
                    });
                });
            });
        }

        function ConfirmDel() {
            $.messager.confirm('确认', '确定删除吗?', function (r) {
                if (r) {
                    return true;
                }
                else {
                    return false;
                }
            });
        }

        //打开对话框
        function OpenWindow(id, createUserID) {
            var url = 'PrivilegeEdit.aspx?PrivGroup_ID=' + id + '&createUserID=' + createUserID + '&rad=' + Math.random()

            //打开对话框
            cDialog.open(url, { width: "570px", height: "385px", scroll: false });
            return false;
        }

        //显示用户列表
        function ShowUserList(id) {
            var url = 'PrivilegeUserList.aspx?PrivGroup_ID=' + id + '&rad=' + Math.random()

            //打开对话框
            cDialogUser.open(url, { width: "618px", height: "380px", scroll: false });
            return false;
        }

        function CloseWindow(r) {
            if (r == 'succ') {
                $('#refresh').click();
            }
            cDialog.close();
        }
    </script>
</head>
<body style="margin: 0px; padding: 0px; overflow: auto;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%-- <div id="searchHead">
                    权限组管理</div>--%>
            <table>
                <tr>
                    <td>
                        <fieldset style="height: 500px; width: 150px">
                            <legend>地区</legend>
                            <div style="overflow: auto; height: 480; margin: 0px; padding: 0px;">
                                <asp:TreeView ID="trvAreaInfo" runat="server" ParentNodeStyle-ImageUrl="~/Images/TreeView/folder.gif"
                                    RootNodeStyle-ImageUrl="~/images/TreeView/control_panel.gif" LeafNodeStyle-ImageUrl="~/images/TreeView/word.gif"
                                    ExpandImageUrl="~/images/TreeView/folder_open.gif" ShowLines="true" CssClass="TreeView"
                                    SelectedNodeStyle-CssClass="SelectedTreeNode" HoverNodeStyle-CssClass="HoverTreeNode"
                                    OnSelectedNodeChanged="trvAreaInfo_SelectedNodeChanged" Height="480px">
                                </asp:TreeView>
                            </div>
                        </fieldset>
                    </td>
                    <td>
                        <fieldset style="height: 500px; width: 700px;">
                            <legend>权限组</legend>
                            <div class="Search" <%--style="margin: 0px; padding-top: 5px; padding-bottom: 5px; background: #efefef;
                                width:100%;"--%>>
                                <%--<input type="button" id="btnNew" onclick="return OpenWindow(0);" value="添加权限组" class="button2d"/>--%>
                                <asp:LinkButton ID="btnNew" class="easyui-linkbutton" iconcls="icon-add" Text="添加权限组"
                                    runat="server" OnClientClick="return OpenWindow(0);" />
                            </div>
                            <div class="c-datagrid-container">
                                <asp:GridView ID="grdPrivilegeGroup" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    OnSorting="grdPrivilegeGroup_SortCommand" DataKeyNames="PrivGroup_ID" PageSize="15"
                                    Width="100%" OnRowCommand="grdPrivilegeGroup_RowCommand" OnRowDataBound="grdPrivilegeGroup_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="PrivGroup_ID" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                                            Visible="false" FooterText="PrivGroup_ID" HeaderText="权限组ID" />
                                        <asp:BoundField DataField="PrivGroup_Name" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                                            FooterText="PrivGroup_Name" HeaderText="权限组名称" />
                                        <asp:BoundField DataField="MainArea_Name" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                                            SortExpression="MainArea_Name" FooterText="MainArea_Name" HeaderText="地区" ItemStyle-Width="100px">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="User_Name" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                                            SortExpression="User_Name" FooterText="User_Name" HeaderText="创建人" ItemStyle-Width="90px">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PrivGroup_Desc" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                                            SortExpression="PrivGroup_Desc" FooterText="PrivGroup_Desc" HeaderText="权限说明"
                                            ItemStyle-Width="160px"></asp:BoundField>
                                        <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="center" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <img alt="" style="margin-right: 8px; cursor:pointer;" src="../../Images/Common/edit.png" title="修改"
                                                     onclick="return OpenWindow(<%#Eval("PrivGroup_ID") %>,<%#Eval("User_ID") %>)" />
                                                <asp:LinkButton ID="delBtn" Style="margin-right: 8px;" runat="server" class="imgClass"
                                                    Text="删除" CommandName="del"><img style="border:0;" alt=""  src="../../Images/Common/delete.gif" /></asp:LinkButton>
                                                <img alt="" src="../../Images/Common/users.ico" title="查看当前组下的所有用户" style="cursor: pointer;"
                                                    onclick="return ShowUserList(<%#Eval("PrivGroup_ID") %>)" />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="False"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="User_ID" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                                            Visible="false" FooterText="User_ID" HeaderText="User_ID" />
                                    </Columns>
                                </asp:GridView>
                                <asp:HiddenField ID="orderbyname" runat="server" Value="PrivGroup_ID" />
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
                                                    OnCommand="GotoThePage" CssClass="pagination_first" />
                                                <asp:Button ID="btn_pre" runat="server" Text="" ToolTip="上一页" CommandArgument="上一页"
                                                    OnCommand="GotoThePage" CssClass="pagination_prev" />
                                                <asp:Button ID="btn_next" runat="server" Text="" ToolTip="下一页" CommandArgument="下一页"
                                                    OnCommand="GotoThePage" CssClass="pagination_next" />
                                                <asp:Button ID="btn_end" runat="server" Text="" ToolTip="末页" CommandArgument="末页"
                                                    OnCommand="GotoThePage" CssClass="pagination_last" />
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
                        </fieldset>
                    </td>
                </tr>
            </table>
            <asp:Button ID="refresh" runat="server" OnClick="RefreshData" Style="display: none;" />
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
        $("#btnNew").linkbutton();
        $.messager.progress("close");
    }
</script>
<script src="../../Js2/Comm/AjaxTimeout.js" type="text/javascript"></script>
