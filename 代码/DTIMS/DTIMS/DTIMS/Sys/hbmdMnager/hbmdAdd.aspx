<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hbmdAdd.aspx.cs" Inherits="BJ.DTIMS.Sys.hbmdMnager.hbmdAdd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>新增学员</title>
    <script language="javascript" src="../../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/icon.css" />
    <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/Comm.css" />
    <script type="text/javascript" src="../../Js2/Jquery/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../../Js2/Comm/Comm.js"></script>
    <script type="text/javascript">
        //window.onload = function ()//用window的onload事件，窗体加载完毕的时候
        //{
        //    document.getElementById('txtDate').valueAsDate = new Date();
        //}


        //校验身份证号码正确性
        function testId(id) {
            var format = /^(([1][1-5])|([2][1-3])|([3][1-7])|([4][1-6])|([5][0-4])|([6][1-5])|([7][1])|([8][1-2]))\d{4}(([1][9]\d{2})|([2]\d{3}))(([0][1-9])|([1][0-2]))(([0][1-9])|([1-2][0-9])|([3][0-1]))\d{3}[0-9xX]$/;
            //号码规则校验
            if (!format.test(id)) {
                return false;
            }
            //区位码校验
            //出生年月日校验   前正则限制起始年份为1900;
            var year = id.substr(6, 4),//身份证年
                month = id.substr(10, 2),//身份证月
                date = id.substr(12, 2),//身份证日
                time = Date.parse(month + '-' + date + '-' + year),//身份证日期时间戳date
                now_time = Date.parse(new Date()),//当前时间戳
                dates = (new Date(year, month, 0)).getDate();//身份证当月天数
            if (time > now_time || date > dates) {
                return false;
            }
            //校验码判断
            var c = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2];   //系数
            var b = ['1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2'];  //校验码对照表
            var id_array = id.split("");
            var sum = 0;
            for (var k = 0; k < 17; k++) {
                sum += parseInt(id_array[k]) * parseInt(c[k]);
            }
            return id_array[17].toUpperCase() === b[sum % 11].toUpperCase();
        }

        var cDialog = null;
        $(document).ready(function () {

            var url = location.search;
            if (url.substr(0, 1) == '?')
                url = url.substr(1);
            url = url.split("&");

            if (url != "type=add") {
                var type = url[0].split('=')[1];
                var num = url[1].split('=')[1];

                var dataString = new Array();
                $.ajax({ //调用的静态方法，所以下面必须参数按照下面来
                    url: 'hbmdAdd.aspx/StudentLoadInfo',
                    type: 'post',
                    contentType: "application/json",
                    dataType: 'json',
                    traditional: true,
                    data: "{type:'" + type + "',id:'" + num + "'}",//,txtName :'" + txtName + "',txtDate  :'" + txtDate + "',txtLsh   :'" + txtLsh 
                    //+ "',ddkscs   :'" + ddkscs + "',IsJf     :'" + IsJf + "',ddMd     :'" + ddMd + "', ddkm     :'" + ddkm + "'}", //必须的，为空的话也必须是json字符串 
                    success: function (data) {//这边返回的是个对象
                        // $.messager.progress({ title: '提示', msg: data });
                        document.getElementById('txtDate').value = data.d;
                        //if (data != null) {
                        //    $.messager.alert("提示", data.d, "info", function () { }); return false;
                        //}
                    }
                });
            }
            else {
                //初始化模态对话框
                var date = new Date();
                var year = date.getFullYear();
                var month = date.getMonth() + 1;
                var day = date.getDate();
                document.getElementById('txtDate').value = date.getFullYear() + "-" + month + "-" + date.getDate();
            }
        });
        function addStudent() {
            var url = location.search;
            if (url.substr(0, 1) == '?')
                url = url.substr(1);
            url = url.split("&");
            var type = null;
            var num = null;
            if (url != "type=add") {
                type = url[0].split('=')[1];
                num = url[1].split('=')[1];
            }
            var txtCardId = $("#txtCardId").val();
            var txtName = $("#txtName").val();
            var txtDate = $("#txtDate").val();
            var txtLsh = $("#txtLsh").val();
            var ddkscs = document.getElementById("ddkscs").value
            var IsJf = document.getElementById("IsJf").value
            var ddMd = document.getElementById("ddMd").value
            var ddkm = document.getElementById("ddkm").value

            

             if (testId(txtCardId.trim()) == false) {
                $.messager.alert("提示", "身份证位数不对或身份证无效,请核对!", "info", function () {
                    $("#txtCardId").get(0).focus();
                }); return false;
            }

            if (txtCardId == "") {
                $.messager.alert("提示", "身份证不能为空!", "info", function () {
                    $("#txtCardId").get(0).focus();
                }); return false;
            }
            if (txtName == "") {
                $.messager.alert("提示", "姓名不能为空!", "info", function () {
                    $("#txtName").get(0).focus();
                }); return false;
            }
            if (txtDate == "") {
                $.messager.alert("提示", "日期不能为空!", "info", function () {
                    $("#txtDate").get(0).focus();
                }); return false;
            }
            if (txtLsh == "") {
                $.messager.alert("提示", "流水号不能为空!", "info", function () {
                    $("#txtLsh").get(0).focus();
                }); return false;
            }
            //{"zone":"海淀","zone_en":"haidian"}
            var dataString = new Array();
            dataString[0] = txtCardId;
            dataString[1] = txtName;
            dataString[2] = txtDate;
            dataString[3] = txtLsh;
            dataString[4] = ddkscs;
            dataString[5] = IsJf;
            dataString[6] = ddMd;
            dataString[7] = ddkm;

            var jsonT = "{";
            for (var i = 0; i < dataString.length; i++) {
                var data = dataString[i];
                if (i == 0)
                    jsonT += "\"txtCardId\":\"" + data + "\",";
                if (i == 1)
                    jsonT += "\"txtName\":\"" + data + "\",";
                if (i == 2)
                    jsonT += "\"txtDate\":\"" + data + "\",";
                if (i == 3)
                    jsonT += "\"txtLsh\":\"" + data + "\",";
                if (i == 4)
                    jsonT += "\"ddkscs\":\"" + data + "\",";
                if (i == 5)
                    jsonT += "\"IsJf\":\"" + data + "\",";
                if (i == 6)
                    jsonT += "\"ddMd\":\"" + data + "\",";
                if (i == 7)
                    jsonT += "\"ddkm\":\"" + data + "\"";
            }
            jsonT += "}";
            var jsonvar = JSON.stringify(jsonT);
            $.ajax({ //调用的静态方法，所以下面必须参数按照下面来
                url: 'hbmdAdd.aspx/AddStudent',
                type: 'post',
                contentType: "application/json",
                dataType: 'json',
                traditional: true,
                data: "{jsonvar:'" + jsonT + "',id:'" + num + "'}",//,txtName :'" + txtName + "',txtDate  :'" + txtDate + "',txtLsh   :'" + txtLsh 
                //+ "',ddkscs   :'" + ddkscs + "',IsJf     :'" + IsJf + "',ddMd     :'" + ddMd + "', ddkm     :'" + ddkm + "'}", //必须的，为空的话也必须是json字符串 
                success: function (data) {//这边返回的是个对象
                    // $.messager.progress({ title: '提示', msg: data });
                    
                    if (data != null) {alert(data.d);
                        $.messager.alert("提示", data.d, "info", function () { }); return false;
                    }
                }
            });
            //}
            //else{
            //    var type = url[0].split('=')[1];
            //    var num = url[1].split('=')[1];
            //    alert(num);
            //}
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table class="table_Dialog" style="width: 100%; height: 345px;">
            <tr>
                <td class="header" style="width: 80px; border-left: 0px">身份证：
                </td>
                <td>
                    <asp:TextBox ID="txtCardId" runat="server" Width="200" MaxLength="20"></asp:TextBox>
                    <span style="color: red">*</span>
                </td>
            </tr>
            <tr id="pwd1" runat="server">
                <td class="header">姓名：
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" Width="200" MaxLength="10"></asp:TextBox>
                    <span style="color: red">*</span>
                </td>
            </tr>
            <tr id="Tr1" runat="server">
                <td class="header">考试日期：
                </td>
                <td>


                    <%-- <input name="txtDate" type="text" id="txtDate" style="width: 200px" class="Wdate"
                        onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd' })" value="2018-01-01" runat="server" size="18" />--%>
                    <asp:TextBox ID="txtDate" type="text" class="Wdate" onclick="WdatePicker({ dateFmt: 'yyyy-MM-dd' })" runat="server" Width="200" MaxLength="10"></asp:TextBox>
                    <span style="color: red">*</span>
                </td>
            </tr>
            <tr id="Tr2" runat="server" style="display:none">
                <td class="header">流水号：
                </td>
                <td>
                    <asp:TextBox ID="txtLsh" runat="server" value="1"  Width="200" MaxLength="10"></asp:TextBox>
                    <span style="color: red">*</span>
                </td>
            </tr>
            <tr style="display:none">
                <td class="header">考试次数：
                </td>
                <td>
                    <asp:DropDownList ID="ddkscs" Width="200" runat="server">
                        <asp:ListItem Text="0" Value="0"></asp:ListItem>
                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="display:none">
                <td class="header">是否缴费：
                </td>
                <td>
                    <asp:DropDownList ID="IsJf" Width="200" runat="server">
                        <asp:ListItem Text="未缴费" Value="1"></asp:ListItem>
                        <asp:ListItem Text="已缴费" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="display:none">
                <td class="header">名单：
                </td>
                <td>
                    <asp:DropDownList ID="ddMd" Width="200" runat="server">
                        <asp:ListItem Text="黑名单" Value="1"></asp:ListItem>
                        <asp:ListItem Text="白名单" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="header">考试科目：
                </td>
                <td>
                    <asp:DropDownList ID="ddkm" Width="200" runat="server">
                        <asp:ListItem Text="科目一" Value="1"></asp:ListItem>
                        <asp:ListItem Text="科目二" Value="2"></asp:ListItem>
                        <asp:ListItem Text="科目三" Value="3"></asp:ListItem>
                        <asp:ListItem Text="科目四" Value="4"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="header"></td>
                <td style="text-align: center">
                    <asp:LinkButton ID="btnAdd" class="easyui-linkbutton" Text="新   增" runat="server" OnClientClick="return addStudent();" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
<script type="text/javascript">
    function msAjaxBeginRequest(sender, args) {
        $.messager.progress({ title: '请稍等', msg: '提交数据中...' });
    }

    // 重新绑定 Jquery easyUi 控件样式。
    function msAjaxEndRequest(sender, args) {
        $("#btnAdd").linkbutton();
        $.messager.progress("close");
    }
</script>
