<%@ Page Language="c#" Inherits="DTIMS.Web.SystemLog" CodeBehind="SystemLog.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
    <title>ϵͳ��־</title>
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
            //��ʼ��ģ̬�Ի���
            cDialog = new customerDialog("ϵͳ��־�鿴", true);
        });

        //�鿴
        function DoView(url) {
            //�򿪶Ի���
            cDialog.open(url, { width: "550px", height: "310", scroll: false });
            return false;
        }
        //ɾ��
        function DeleteAll() {
            $.messager.confirm("ȷ��", "ȷ��ɾ��������־?", function (r) {
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
            $.messager.alert("����", "��������ʼʱ�����ֹʱ��", function () {
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
                ��ʼʱ��:<input name="txtStart" type="text" id="txtStart" style="width: 130px" class="Wdate"
                    onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" runat="server" size="18" />
                ����ʱ��:<input name="dtpkEndDate" type="text" id="dtpkEndDate" style="width: 130px"
                    class="Wdate" onclick="WdatePicker({dateFmt:'yyyy-MM-dd'})" runat="server" size="18" />
                ����Ա�ʺ�:<asp:TextBox ID="txtUserName" runat="server" MaxLength="20" Width="100"></asp:TextBox>
                <asp:LinkButton ID="btnSearch" class="easyui-linkbutton" iconcls="icon-search" Text="��ѯ"
                    runat="server" OnClientClick="return validateTime();" OnClick="btnSearch_Click" />
                <%if (this.Oper.IsSuper)
                  {%>
                <asp:LinkButton ID="btnClear" class="easyui-linkbutton" iconcls="icon-remove" Text="ɾ����־"
                    runat="server" OnClientClick="return DeleteAll();" />
                <%} %>
            </div>
            <div class="c-datagrid-container">
                <asp:GridView ID="grdLog" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    OnSorting="grdLog_Sorting" DataKeyNames="LOG_ID" PageSize="15" Width="100%" OnRowDataBound="grdLog_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="LOG_ID" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                            FooterText="LOG_ID" HeaderText="��־ID" />
                        <asp:BoundField DataField="User_Login" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                            FooterText="User_Login" HeaderText="����Ա�ʺ�" />
                        <asp:BoundField DataField="LOG_DATETIME" DataFormatString="{0:yyyy-MM-dd  HH:mm:ss}"
                            ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false" FooterText="LOG_DATETIME"
                            HeaderText="����ʱ��" />
                        <asp:BoundField DataField="LOG_Mode" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                            FooterText="LOG_Mode" HeaderText="������ʽ" />
                        <asp:BoundField DataField="LItem_Name" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                            FooterText="LItem_Name" HeaderText="��¼��Ŀ����" />
                        <asp:BoundField DataField="LOG_CONTENT" ItemStyle-HorizontalAlign="center" ItemStyle-Wrap="false"
                            FooterText="LOG_CONTENT" HeaderText="��־����" />
                        <asp:TemplateField HeaderText="����" ItemStyle-Width="60" ItemStyle-HorizontalAlign="center">
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
                                    <span style="padding-left: 6px;">��</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPageIndex" runat="server" Text="1" size="2" CssClass="easyui-numberbox c-pagination-num" />
                                    <asp:LinkButton ID="lbtnPageIndex" CommandName="Page" CommandArgument="Num" Text=""
                                        runat="server" OnCommand="GotoThePage" Style="display: none;" />
                                    <asp:HiddenField ID="hidPageIndex" runat="server" Value="1" />
                                </td>
                                <td>
                                    <span style="padding-right: 6px;">��<asp:Label ID="lblPageCount" Text="0" runat="server" />ҳ</span>
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
                        ��ʾ<asp:Label ID="lblRowStart" Text="1" runat="server" />��<asp:Label ID="lblRowEnd"
                            Text="0" runat="server" />,��<asp:Label ID="lblRowCount" Text="0" runat="server" />��¼</div>
                    <div style="clear: both;">
                    </div>
                </div>
            </div>
            <%--<table width="100%" cellpadding="0" cellspacing="0" class="page-bar">
                                <tbody class="datalist" id="notData" visible="false" runat="server">
                                    <tr>
                                        <td align="center" style="text-align: center; font-size: 15px">
                                            δ�ҵ���������������.
                                        </td>
                                    </tr>
                                </tbody>
                                <tbody id="pageTable" runat="server">
                                    <tr>
                                        <td style="text-align: right; width: 70%; padding: 3px; color: #ff6600;">
                                            <asp:Button ID="btn_first" runat="server" Text=" " ToolTip="��ҳ" CommandArgument="��ҳ"
                                                OnCommand="GotoThePage" CssClass="pagination_first" />
                                            <asp:Button ID="btn_pre" runat="server" Text=" " ToolTip="��һҳ" CommandArgument="��һҳ"
                                                OnCommand="GotoThePage" CssClass="pagination_prev" />
                                            <asp:Button ID="btn_next" runat="server" Text=" " ToolTip="��һҳ" CommandArgument="��һҳ"
                                                OnCommand="GotoThePage" CssClass="pagination_next" />
                                            <asp:Button ID="btn_end" runat="server" Text=" " ToolTip="ĩҳ" CommandArgument="ĩҳ"
                                                OnCommand="GotoThePage" CssClass="pagination_last" />
                                            ��ǰ�ǵ�<asp:Literal ID="pagenum" runat="server" Text="1"></asp:Literal>ҳ ת��<asp:DropDownList
                                                ID="dropGoToPageNumber" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropGoToPageNumber_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            ҳ
                                        </td>
                                        <td style="width: 30%; color: #ff6600; text-align: center; padding-top: 5px;">
                                            ����<%=max_page_num %>ҳ
                                            <%=page_rows%>
                                            ��/ҳ ��<asp:Label ID="labMaxCount" runat="server" Text="0"></asp:Label>
                                            ������
                                        </td>
                                    </tr>
                                </tbody>
                            </table>--%>
            <asp:HiddenField ID="orderbyname" runat="server" Value="LOG_DATETIME" />
            <!--Ҫ��������ֶ�-->
            <asp:HiddenField ID="orderbytype" runat="server" Value="desc" />
            <!--�������ͣ�ASC or DESC-->
            <asp:HiddenField ID="maxpage" runat="server" />
            <!--���ҳ��-->
            <asp:HiddenField ID="filterwords" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
<script type="text/javascript">
    function msAjaxBeginRequest(sender, args) {
        $.messager.progress({ title: '���Ե�', msg: '�ύ������...' });
    }

    // ���°� Jquery easyUi �ؼ���ʽ��
    function msAjaxEndRequest(sender, args) {
        $("#btnSearch").linkbutton();
        $("#btnClear").linkbutton();
        $.messager.progress("close");
    }
</script>
<script src="../../Js2/Comm/AjaxTimeout.js" type="text/javascript"></script>
