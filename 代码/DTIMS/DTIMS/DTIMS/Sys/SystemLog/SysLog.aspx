<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysLog.aspx.cs" Inherits="DTIMS.Web.SysLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统日志管理</title>
     <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/icon.css" />
    <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/Comm.css" />
    <script type="text/javascript" src="../../Js2/Jquery/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/locale/easyui-lang-zh_CN.js"></script>
      <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/GridView.css" />
    <script type="text/javascript" src="../../Js2/Comm/Comm.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            init();
           });

        function init() {
            $("#Log").datagrid({
                width: 1024,
                height: 360,
                striped: true,
                singleSelect: true,
                remoteSort: false,
                fitColumns: true,
                pagination: true,
                pageSize: 20,
                columns: [[
                    { field: 'LOG_ID', title: 'ID', width: 100, align: 'center', sortable: false },
                    { field: 'User_Login', title: '操作员', width: 120, align: 'center', sortable: false },
                    { field: 'LOG_DATETIME', title: '操作时间', width: 150, align: 'center', sortable: false },
                    { field: 'LOG_Mode', title: '操作方式', width: 120, align: 'center', sortable: false },
                    { field: 'Client_IP', title: 'IP地址', width: 150, align: 'center' },
                    { field: 'LOG_CONTENT', title: '日志内容', width: 500, align: 'center', sortable: false }
                    
                ]],
                onBeforeLoad: function (param) {
                    getData(param.page, param.rows);
                }
                        });
        }

        function getData(page, rows) {
            // 显示进度条。
            //$("#Log").datagrid("loading");

            var User_Login = $("#txtUser").val();
            var Start = $("#txtStart").datebox("getValue");
            var End = $("#txtEnd").datebox("getValue");

            // 获取数据。
            var aj = new AjaxJson({ data: { act: "Log", index: page, size: rows, User: User_Login, startTime: Start, endTime: End} });
            aj.send({
                complete: function (JR) {
                    //$("#Log").datagrid("loaded");
                },
                success: function (JR) {
                    $("#Log").datagrid("loadData", JR.responseData.data);
                }
            });
        }

        function Query() {
            $("#Log").datagrid("reload");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table style="width: 1050px; margin-left: 10px;">
        <tr>
            <td>
                <table class="table_Dialog">
                    <tr>
                        <td style="width: 100%" class="Search">
                            开始时间：<input type="text" class="easyui-datebox" id="txtStart" runat="server" />&nbsp;&nbsp;
                            结束时间：<input type="text" class="easyui-datebox" id="txtEnd" runat="server" />&nbsp;&nbsp;
                            操作员账号：<input type="text" id="txtUser" runat="server" maxlength="10" />&nbsp;&nbsp;
                            <a id="search" class="easyui-linkbutton" onclick="javascript:return Query();" iconcls="icon-search">
                                查 询</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div style="margin: 0px; padding-top: 5px; background-color: #FFF;">
                    <table id="Log">
                    </table>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
<script src="../../Js2/Comm/AjaxTimeout.js" type="text/javascript"></script>