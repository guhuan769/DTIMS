<%@ Page Language="c#" Inherits="DTIMS.Web.SystemLog" CodeBehind="SystemLog.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>系统日志</title>
    <script language="javascript" src="../../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
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
        $(document).ready(function () {
            //初始化模态对话框
            cDialog = new customerDialog("系统日志查看", true);
        });

        //查看
        function DoView(url) {
            //打开对话框
            cDialog.open(url, { width: "550px", height: "310", scroll: false });
            return false;
        }
        //删除
        function DeleteAll() {
            $.messager.confirm("确认", "确认删除所有日志?", function (r) {
                if (r == true) {
                    window.document.forms[0].delete1.value = 'a';
                    window.document.forms[0].submit();
                }
            });
            return false;
        }

        function validateTime() {
            if ($("#txtStart").val() == "" && $("#dtpkEndDate").val() == "") {
                return true;
            }
            else {
                if ($("#txtStart").val() != "" && $("#dtpkEndDate").val() != "") {
                    return true;
                }
            }
            $.messager.alert("错误", "请输入起始时间和终止时间", function () {
                $("#txtStart").get(0).focus();
            });
            return false;
        }
    </script>
</head>
<body>
    <form id="Form1" name="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <input type="hidden" name="delete1">
            <input type="hidden" name="actionNo">
            <input type="hidden" name="OperaotrAdd">
            <div class="Search">
                开始时间:<input name="txtStart" type="text" id="txtStart" style="width: 130px" class="Wdate"
                    onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" runat="server" size="18" />
                结束时间:<input name="dtpkEndDate" type="text" id="dtpkEndDate" style="width: 130px"
                    class="Wdate" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" runat="server" size="18" />
                操作员帐号:<asp:TextBox ID="txtUserName" runat="server" MaxLength="20" Width="100"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" class="easyui-linkbutton" iconcls="icon-search" Text="查询"
                    runat="server" OnClientClick="return validateTime();" OnClick="btnSearch_Click" />
                <%if (this.Oper.IsSuper)
                  {%>
                <asp:LinkButton ID="btnClear" class="easyui-linkbutton" iconcls="icon-remove" Text="删除日志"
                    runat="server" OnClientClick="return DeleteAll();" />
                <%} %>
            </div>
            <div class="c-datagrid-container">
                <asp:GridView ID="grdLog" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    OnSorting="grdLog_Sorting" DataKeyNames="LOG_ID" PageSize="15" Width="100%" OnRowDataBound="grdLog_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="LOG_ID" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                            FooterText="LOG_ID" HeaderText="日志ID" />
                        <asp:BoundField DataField="User_Login" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                            FooterText="User_Login" HeaderText="操作员帐号" />
                        <asp:BoundField DataField="LOG_DATETIME" DataFormatString="{0:yyyy-MM-dd  HH:mm:ss}"
                            ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false" FooterText="LOG_DATETIME"
                            HeaderText="操作时间" />
                        <asp:BoundField DataField="LOG_Mode" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                            FooterText="LOG_Mode" HeaderText="操作方式" />
                        <asp:BoundField DataField="LItem_Name" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                            FooterText="LItem_Name" HeaderText="记录项目名称" />
                        <asp:BoundField DataField="LOG_CONTENT" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                            FooterText="LOG_CONTENT" HeaderText="日志内容" />
                        <asp:TemplateField HeaderText="操作" ItemStyle-Width="60" ItemStyle-HorizontalAlign="center">
                            <ItemTemplate>
                            </ItemTemplate>
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
            </div>
            <%--<table width="100%" cellpadding="0" cellspacing="0" class="page-bar">
                                <tbody class="datalist" id="notData" visible="false" runat="server">
                                    <tr>
                                        <td align="center" style="text-align: center; font-size: 15px">
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
            <asp:HiddenField ID="orderbyname" runat="server" Value="LOG_DATETIME" />
            <!--要排序的列字段-->
            <asp:HiddenField ID="orderbytype" runat="server" Value="desc" />
            <!--排序类型：ASC or DESC-->
            <asp:HiddenField ID="maxpage" runat="server" />
            <!--最大页码-->
            <asp:HiddenField ID="filterwords" runat="server" />
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
        $("#btnSearch").linkbutton();
        $("#btnClear").linkbutton();
        $.messager.progress("close");
    }
</script>
<script src="../../Js2/Comm/AjaxTimeout.js" type="text/javascript"></script>
