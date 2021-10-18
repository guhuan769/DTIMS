<%@ Page Language="c#" Inherits="DTIMS.Web.hbmdInfo" CodeBehind="hbmdInfo.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>��־��ϸ��Ϣ</title>
    <link href="<%=Request.ApplicationPath%>/css/main.css.aspx" type="text/css" rel="stylesheet">

    <script src="<%=Request.ApplicationPath%>/js/common.js.aspx"></script>

    <script language="javascript" src="../../Js2/Jquery/jquery-1.7.1.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/icon.css" />
    <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/Comm.css" />
    <script type="text/javascript">
        function Send() {
            $.ajax({ //���õľ�̬������������������������������
                url: 'hbmdInfo.aspx/SendInfo',
                type: 'post',
                contentType: "application/json",
                dataType: 'json',
                data: "{}", //����ģ�Ϊ�յĻ�Ҳ������json�ַ��� 
                success: function (data) {//��߷��ص��Ǹ�����
                    // $.messager.progress({ title: '��ʾ', msg: data });
                    var d = data.d.split("|");
                    if (d != "" || d != null) {
                        if (d[0] == "true") {
                            var ip = d[1];
                            $.messager.confirm('ȷ��', '' + d[2] + '���Ƿ��������բ����', function (r) {
                                if (r) {
                                    $.ajax({
                                        url: 'hbmdInfo.aspx/SendKz',
                                        type: 'post',
                                        contentType: "application/json",
                                        dataType: 'json',
                                        data: "{ip:'"+ip+"'}", //����ģ�Ϊ�յĻ�Ҳ������json�ַ��� 
                                        success: function (data) {
                                            alert(data.d);
                                        }
                                    });
                                }
                            });
                        } else {
                            if (data != null)
                                alert(data.d);
                        }
                    }
                }
            });
        }
    </script>
</head>
<body>
    <form method="post" enctype="multipart/form-data" id="Form1" style="margin-left: -8px" runat="server">
        <div>
            <table visible="false" border="0" width="100%" align="center" cellpadding="0" cellspacing="0"
                class="table_Dialog" id="SelectTeleNum">
                <tr>
                    <td class="header" style="width: 200px" align="center">&nbsp;&nbsp;&nbsp;&nbsp;���֤��</td>
                    <td>
                        <asp:Label ID="lbSfzmhm" runat="server" Width="156px"></asp:Label></td>
                    <td class="header" align="center" style="width: 150px">���Կ�Ŀ��</td>
                    <td style="width: 200px">
                        <asp:Label ID="lbKskm" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="header" style="width: 200px" align="center">&nbsp;�ɷѱ�ǣ�</td>
                    <td>
                        <asp:Label ID="lbJfbj" runat="server" Width="156px"></asp:Label></td>
                    <td class="header" align="center" style="width: 150px">���Դ�����</td>
                    <td style="width: 200px">
                        <asp:Label ID="lbKscs" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="header" style="width: 200px" align="center">&nbsp;��բ������</td>
                    <td>
                        <asp:Label ID="lbKzcs" runat="server" Width="156px"></asp:Label></td>
                </tr>
                <tr>
                    <td class="header" colspan="4" style="width: 200px; text-align: center" align="center">
                        <%--<asp:LinkButton ID="btnClear" class="easyui-linkbutton" Text="��  բ"
                            runat="server"  />--%>
                        <input id="btnClear" class="easyui-linkbutton" onclick="Send()" type="button" value="��  բ" />
                        <%-- OnClientClick="return Send();" --%>
                    </td>
                </tr>

            </table>
        </div>
    </form>
</body>
</html>
<script type="text/javascript" src="../../Js2/Jquery/jquery-1.7.1.min.js"></script>
<script type="text/javascript" src="../../Js2/Jquery/jquery.easyui.min.js"></script>
<script type="text/javascript" src="../../Js2/Jquery/locale/easyui-lang-zh_CN.js"></script>

<script src="../../Js2/Comm/AjaxTimeout.js" type="text/javascript"></script>
