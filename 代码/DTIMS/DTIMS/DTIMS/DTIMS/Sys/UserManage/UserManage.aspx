<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserManage.aspx.cs" Inherits="CTQS.Sys.UserManage.UserManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户管理</title>
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

        function initDelEvent() {
            //停止当前删除操作,加上确认提示框
            $(".imgClass[title='禁用']").each(function () {
                var href = $(this).attr("href");
                if (href == null) return;
                href = href.replace("javascript:__doPostBack('", "");
                href = href.replace("','')", "");
                $(this).attr("href", "#");
                //添加点击事件
                $(this).click(function () {
                    $.messager.confirm('确认', '确定要禁用此用户?', function (r) {
                        if (r) {
                            __doPostBack(href, "");
                        }
                    });
                });
            });

            //如果是ie6则设置高度
            if ($.browser.msie && $.browser.version == "6.0") {
                $("#ie6Heigth").css("height", "480px");
            }
        }
        var cDialog = null;
        var cDialog1 = null;
        $(document).ready(function () {
            //加载模态对话框    
            cDialog = new customerDialog("用户信息", false);
            initDelEvent("禁用");
        });

        function GetSelectArea() {
            //$("#TreeView1").
        }

        //查看
        function doView(id, login) {
            var url = 'UserManageEdit.aspx?id=' + id + '&type=view&time=' + new Date().getDate();
            if (login != null && login == "1") {
                cDialog.open(url, { width: "370px", height: "445px", scroll: false });
            }
            else {
                cDialog.open(url, { width: "580px", height: "445px", scroll: false });
            }
            return false;
        }

        //添加
        function addUser() {
            var areaId = $("#hidAreaID").val();
            var url = 'UserManageEdit.aspx?type=add&areaId=' + areaId + '&time=' + new Date().getDate();
            cDialog.open(url, { width: "580px", height: "440px", scroll: false });
            return false;
        }

        //修改
        function editUser(id, isSuper) {
            var url = 'UserManageEdit.aspx?id=' + id + '&type=edit&time=' + new Date().getDate();
            if (isSuper != null && isSuper == "ok") {
                cDialog.open(url, { width: "370px", height: "445px", scroll: false });
            }
            else {
                cDialog.open(url, { width: "580px", height: "445px", scroll: false });
            }
            return false;
        }

        function ShowRoleViews(id) {
            var url = 'RoleView.aspx?userId=' + id;
            cDialog1 = new customerDialog("用户权限信息", true);
            cDialog1.open(url, { width: "390px", height: "360px", scroll: false });
        }

        //关闭
        function closeDialog(isUpdate) {
            if (isUpdate == "succ") {
                $("#refreshPage").click();
            }
            cDialog.close();
        }
    </script>
</head>
<body style="overflow: auto;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <div id="progressBar" runat="server">
                        <img alt="" src="../../Images/Common/loading.gif" /><span>正在操作，请稍候…</span></div>
                </ProgressTemplate>
            </asp:UpdateProgress>--%>
            <%--<div id="searchHead">
                    用户管理</div>--%>
            <table style="width: 900px">
                <tr>
                    <td>
                        <asp:HiddenField ID="hidAreaID" runat="server" />
                        <fieldset style="height: 500px; width: 150px">
                            <legend>地区</legend>
                            <%--  <asp:TreeView ID="TreeView1" runat="server" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged"
                                    ParentNodeStyle-ImageUrl="~/Images/TreeView/folder.gif" RootNodeStyle-ImageUrl="~/images/TreeView/control_panel.gif"
                                    LeafNodeStyle-ImageUrl="~/images/TreeView/word.gif" ExpandImageUrl="~/images/TreeView/folder_open.gif"
                                    ShowLines="true" CssClass="TreeView" SelectedNodeStyle-CssClass="SelectedTreeNode"
                                    HoverNodeStyle-CssClass="HoverTreeNode" Height="450px">
                                </asp:TreeView>--%>
                            <asp:TreeView ID="TreeView1" runat="server" ParentNodeStyle-ImageUrl="~/Images/TreeView/folder.gif"
                                RootNodeStyle-ImageUrl="~/images/TreeView/control_panel.gif" LeafNodeStyle-ImageUrl="~/images/TreeView/word.gif"
                                ExpandImageUrl="~/images/TreeView/folder_open.gif" ShowLines="true" CssClass="TreeView"
                                SelectedNodeStyle-CssClass="SelectedTreeNode" HoverNodeStyle-CssClass="HoverTreeNode"
                                OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" Height="480px">
                            </asp:TreeView>
                        </fieldset>
                    </td>
                    <td valign="top">
                        <fieldset id="ie6Heigth" style="height: 500px; width: 750px">
                            <legend>用户列表</legend>
                            <div id="searchBody" class="Search">
                                用户名：<input id="txtUserName" type='text' maxlength="20" class='inputField' style='width: 100px;'
                                    runat="server" />
                                登录名：<input id="txtLoginName" type='text' maxlength="15" class='inputField' style='width: 100px;'
                                    runat="server" />
                                角色：<asp:DropDownList ID="dropRole" runat="server">
                                    <asp:ListItem Text="全部" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="超级管理员" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="系统管理员" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="普通用户" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                                状态：<asp:DropDownList ID="dropStatus" runat="server">
                                    <asp:ListItem Text="全部" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="正常" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="禁用" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                                <%--<asp:Button ID="btnQuery" runat="server" CssClass="button2d" Text="查  询" OnClick="btnQuery_Click" />
                                <input type="button" class="button2d" onclick="addUser()" value="添  加" />--%>
                                <asp:LinkButton ID="btnQuery" class="easyui-linkbutton" iconcls="icon-search" Text="查询"
                                    OnClick="btnQuery_Click" runat="server" />
                                <asp:LinkButton ID="btnAdd" class="easyui-linkbutton" iconcls="icon-add" Text="添加"
                                    runat="server" OnClientClick="return addUser();" />
                            </div>
                            <asp:HiddenField ID="orderbyname" runat="server" Value="User_ID" />
                            <!--要排序的列字段-->
                            <asp:HiddenField ID="orderbytype" runat="server" Value="desc" />
                            <!--排序类型：ASC or DESC-->
                            <asp:HiddenField ID="maxpage" runat="server" />
                            <!--最大页码-->
                            <asp:HiddenField ID="filterwords" runat="server" />
                            <div class="c-datagrid-container">
                                <asp:GridView DataKeyNames="User_ID" ID="GridView1" AutoGenerateColumns="false" PageSize="15"
                                    Width="100%" runat="server" CellPadding="4" GridLines="None" AllowSorting="True"
                                    OnRowDataBound="GridView1_RowDataBound" OnSorting="GridView1_Sorting" OnRowCommand="GridView1_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="用户名" SortExpression="User_Name" HeaderStyle-Width="128">
                                            <ItemTemplate>
                                               <%-- <a href="#" onclick="doView(<%#Eval("User_ID") %>,'<%#Eval("UserRole_ID")%>')">
                                                    <%#Eval("User_Name")%>
                                                </a>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="角色" HeaderStyle-Width="100" DataField="UserRole_Name"
                                            SortExpression="UserRole_Name" />
                                        <asp:BoundField HeaderText="登录名" HeaderStyle-Width="120" DataField="User_Login" SortExpression="User_Login" />
                                        <asp:BoundField HeaderText="创建日期" HeaderStyle-Width="120" DataField="User_CreateDate"
                                            SortExpression="User_CreateDate" />
                                        <asp:BoundField HeaderText="状态" HeaderStyle-Width="50" DataField="User_Status" SortExpression="User_Status" />
                                        <asp:TemplateField HeaderText="权限组" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="ruleList" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="操作" ItemStyle-Width="60" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%if (GridView1.Rows.Count > 0)
                                                  {  %>
                                                  <div style="margin:0px; padding:0px; text-align:left; vertical-align:middle;">
                                                <img alt="" src="../../Images/Common/users.ico" title="查看用户权限" id="btnRoleViews"
                                                    runat="server" style="cursor: pointer;" />
                                                <img alt="" src="../../Images/Common/edit.png" title="修改" id="btnEdit" runat="server"
                                                    style="cursor: pointer;" />
                                                <asp:LinkButton ID="delBtn" runat="server" class="imgClass" CommandName="del">
                                                </asp:LinkButton>
                                                </div>
                                                <%} %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="35" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
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
                                <%--<table width="100%" cellpadding="0" cellspacing="0" class="page-bar">
                                    <tbody class="datalist" id="notData" visible="false" runat="server">
                                        <tr>
                                            <td align="center" style="text-align: left;" colspan="10">
                                                未找到符合条件的数据.
                                            </td>
                                        </tr>
                                    </tbody>
                                    <tbody id="pageTable" runat="server">
                                        <tr>
                                            <td style="text-align: right; width: 70%; padding: 3px; color: #ff6600;">
                                                <asp:Button ID="btn_first" runat="server" Text=" " ToolTip="首页" CommandArgument="首页"
                                                    OnCommand="GotoThePage" CssClass="pagination_first" />
                                                <asp:Button ID="btn_pre" runat="server" Text=" " ToolTip="上一页" CommandArgument="上一页"
                                                    OnCommand="GotoThePage" CssClass="pagination_prev" />
                                                <asp:Button ID="btn_next" runat="server" Text=" " ToolTip="下一页" CommandArgument="下一页"
                                                    OnCommand="GotoThePage" CssClass="pagination_next" />
                                                <asp:Button ID="btn_end" runat="server" Text=" " ToolTip="末页" CommandArgument="末页"
                                                    OnCommand="GotoThePage" CssClass="pagination_last" />
                                                当前是第<asp:Literal ID="pagenum" runat="server" Text="1"></asp:Literal>页 转到<asp:DropDownList
                                                    ID="dropGoToPageNumber" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropGoToPageNumber_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                页
                                            </td>
                                            <td style="width: 30%; color: #ff6600; text-align: center; padding-top: 5px;">
                                                共有<%=max_page_num %>页
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
            <asp:Button ID="refreshPage" Text="fffffff" runat="server" OnClick="RefreshData"
                Style="display: none;" />
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
        $("#btnQuery").linkbutton();
        $("#btnAdd").linkbutton();
        $.messager.progress("close");
    }
</script>
<script src="../../Js2/Comm/AjaxTimeout.js" type="text/javascript"></script>
