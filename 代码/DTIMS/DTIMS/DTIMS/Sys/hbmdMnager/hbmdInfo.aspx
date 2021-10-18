<%@ Page Language="c#" Inherits="DTIMS.Web.hbmdInfo" CodeBehind="hbmdInfo.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>日志详细信息</title>
    <link href="<%=Request.ApplicationPath%>/css/main.css.aspx" type="text/css" rel="stylesheet">

    <script src="<%=Request.ApplicationPath%>/js/common.js.aspx"></script>

    <script language="javascript" src="../../Js2/Jquery/jquery-1.7.1.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/icon.css" />
    <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/Comm.css" />
    <script type="text/javascript">
        function Send() {
            $.ajax({ //调用的静态方法，所以下面必须参数按照下面来
                url: 'hbmdInfo.aspx/SendInfo',
                type: 'post',
                contentType: "application/json",
                dataType: 'json',
                data: "{}", //必须的，为空的话也必须是json字符串 
                success: function (data) {//这边返回的是个对象
                    // $.messager.progress({ title: '提示', msg: data });
                    var d = data.d.split("|");
                    if (d != "" || d != null) {
                        if (d[0] == "true") {
                            var ip = d[1];
                            $.messager.confirm('确认', '' + d[2] + '，是否继续开启闸机？', function (r) {
                                if (r) {
                                    $.ajax({
                                        url: 'hbmdInfo.aspx/SendKz',
                                        type: 'post',
                                        contentType: "application/json",
                                        dataType: 'json',
                                        data: "{ip:'"+ip+"'}", //必须的，为空的话也必须是json字符串 
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
                    <td class="header" style="width: 200px" align="center">&nbsp;&nbsp;&nbsp;&nbsp;身份证：</td>
                    <td>
                        <asp:Label ID="lbSfzmhm" runat="server" Width="156px"></asp:Label></td>
                    <td class="header" align="center" style="width: 150px">考试科目：</td>
                    <td style="width: 200px">
                        <asp:Label ID="lbKskm" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="header" style="width: 200px" align="center">&nbsp;缴费标记：</td>
                    <td>
                        <asp:Label ID="lbJfbj" runat="server" Width="156px"></asp:Label></td>
                    <td class="header" align="center" style="width: 150px">考试次数：</td>
                    <td style="width: 200px">
                        <asp:Label ID="lbKscs" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="header" style="width: 200px" align="center">&nbsp;开闸次数：</td>
                    <td>
                        <asp:Label ID="lbKzcs" runat="server" Width="156px"></asp:Label></td>
                </tr>
                <tr>
                    <td class="header" colspan="4" style="width: 200px; text-align: center" align="center">
                        <%--<asp:LinkButton ID="btnClear" class="easyui-linkbutton" Text="开  闸"
                            runat="server"  />--%>
                        <input id="btnClear" class="easyui-linkbutton" onclick="Send()" type="button" value="开  闸" />
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
