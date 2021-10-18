<%@ Page Language="c#" Inherits="DTIMS.Web.hbmdQuery" CodeBehind="hbmdQuery.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>系统日志</title>
    <script language="javascript" src="../../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/icon.css" />
    <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/Comm.css" />
    <link rel="Stylesheet" type="text/css" href="../../Css2/Excel/base.css"  />
    <link rel="Stylesheet" type="text/css" href="../../Css2/Excel/edp-nav.css"  />
    <script type="text/javascript" src="../../Js2/Excel/jquery.js"></script>
    <script type="text/javascript" src="../../Js/xlsx.full.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../../Js2/Comm/Comm.js"></script>
    <script type="text/javascript" src="../../Js/ExcelJs.js"></script>
    <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/GridView.css" />
    <script type="text/javascript" src="../../Js2/Comm/GridView.js"></script>

    <script type="text/javascript" src="../../Js2/Excel/JsonExportExcel.min.js"></script>
    <script type="text/javascript">
        var cDialog = null;
        $(document).ready(function () {
            //初始化模态对话框
            cDialog = new customerDialog("系统日志查看", true);
        });

        //查看
        function DoView(url) {
            cDialog = new customerDialog("开闸", true);
            //打开对话框
            cDialog.open(url, { width: "550px", height: "310", scroll: false });
            return false;
        }

        //修改
        function Edit(url) {
            cDialog = new customerDialog("修改", true);
            //打开对话框
            cDialog.open(url, { width: "370px", height: "445px", scroll: false });
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

        function kz() {
            var flag = isValidIP($("#txtIPs").val());
            if (flag == false) {
                $.messager.alert("提示", "您的IP输入错误!", "info", function () {
                    $("#txtIPs").get(0).focus();
                }); return false;
            }
            var ip = $("#txtIPs").val();
            $.ajax({ //调用的静态方法，所以下面必须参数按照下面来
                url: 'hbmdQuery.aspx/SendInfos',
                type: 'post',
                contentType: "application/json",
                dataType: 'json',
                data: "{ip:'" + ip + "'}", //必须的，为空的话也必须是json字符串 
                success: function (data) {//这边返回的是个对象
                    // $.messager.progress({ title: '提示', msg: data });
                    if (data != null)
                        alert(data.d);
                }
            });
        }
        function isValidIP(ip) {
            var reg = /^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/
            return reg.test(ip);
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

        function addStudent() {
            cDialog = new customerDialog("新增学员信息", true);
            var url = 'hbmdAdd.aspx?type=add';
            //cDialog.open(url, { width: "370px", height: "445px", scroll: false });
            cDialog.open(url, { width: "370px", height: "445px", scroll: false });
            return false;
        }


        //导出Excel


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
                        onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd' })" runat="server" size="18" />
                    结束时间:<input name="dtpkEndDate" type="text" id="dtpkEndDate" style="width: 130px"
                        class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd' })" runat="server" size="18" />
                    身份证:<asp:TextBox ID="txt_sfz" runat="server"></asp:TextBox>
                    <%--黑白名单:--%>
                    <asp:DropDownList Visible="false" ID="hbMdBtn" runat="server">
                        <asp:ListItem Text="白名单" Value="0"></asp:ListItem>
                        <asp:ListItem Text="黑名单" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:LinkButton ID="btnSearch" class="easyui-linkbutton" iconcls="icon-search" Text="查询"
                        runat="server" OnClientClick="return validateTime();" OnClick="btnSearch_Click" />
                    IP：<%--<asp:TextBox ID="txtIPs" class="inputField"  style="width: 100px" runat="server"></asp:TextBox>--%><%-- <input class="inputField" id="txtIPs" type="text" />--%><asp:DropDownList ID="txtIPs" Style="width: 179px; height: 21px; color: #ff6600;" runat="server">
                    </asp:DropDownList>
                    <asp:LinkButton ID="lbKz" class="easyui-linkbutton" OnClientClick="return kz();" iconcls="icon-search" Text="开闸" runat="server" />
                    <asp:LinkButton ID="btnAdd" class="easyui-linkbutton" iconcls="icon-add" Text="添加" runat="server" OnClientClick="return addStudent();" />
                    <%--<asp:LinkButton ID="lbDerive" class="easyui-linkbutton" Text="导 出" runat="server" OnClientClick="return exportExcel();" />--%>
                    <asp:LinkButton ID="lbDerive" class="easyui-linkbutton" Text="导 出" runat="server" OnClientClick="return exportExcel();" />
                    <a href="" download="学员信息.xlsx" id="downloadA"></a>
                    <%-- javascript:HtmlExportToExcel('datareport') --%>
                    <%if (this.Oper.IsSuper)
                        {%>

                    <%} %>
                </div>
                <div class="c-datagrid-container">
                    <asp:GridView ID="grdLog" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                        OnSorting="grdLog_Sorting" DataKeyNames="ID" PageSize="15" Width="100%" OnRowDataBound="grdLog_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="ID" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                                FooterText="ID" HeaderText="ID" />
                            <asp:BoundField DataField="SFZMHM" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                                FooterText="SFZMHM" HeaderText="身份证" />
                            <asp:BoundField DataField="name" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                                FooterText="NAME" HeaderText="姓名" />
                            <asp:BoundField DataField="TESTDATE" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                                FooterText="TESTDATE" HeaderText="日期" />
                            <asp:BoundField DataField="LSH" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                                FooterText="LSH" HeaderText="流水号" />
                            <asp:BoundField DataField="KSKM" Visible="false" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                                FooterText="KSKM" HeaderText="考试科目" />
                            <asp:BoundField DataField="JFBJ" Visible="false" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                                FooterText="JFBJ" HeaderText="缴费标记" />
                            <asp:BoundField DataField="KSCS" Visible="false" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                                FooterText="KSCS" HeaderText="考试次数" />
                            <asp:BoundField DataField="KZCS" Visible="false" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                                FooterText="KZCS" HeaderText="开闸次数" />
                            <asp:BoundField DataField="PASS" Visible="false" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                                FooterText="PASS" HeaderText="PASS" />
                            <asp:TemplateField HeaderText="操作" ItemStyle-Width="60" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <%--  <img alt="" src="../../Images/Common/edit.gif" onclick="Edit(<%#Eval("ID") %>)" />--%>
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
                                Text="0" runat="server" />,共<asp:Label ID="lblRowCount" Text="0" runat="server" />记录
                        </div>
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
        $("#lbDerive").linkbutton();
        $("#btnSearch").linkbutton();
        $("#btnClear").linkbutton();
        $("#lbKz").linkbutton();
        $("#btnAdd").linkbutton();
        $.messager.progress("close");
    }
</script>
<script src="../../Js2/Comm/AjaxTimeout.js" type="text/javascript"></script>
