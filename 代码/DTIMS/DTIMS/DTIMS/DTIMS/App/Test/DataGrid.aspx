<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataGrid.aspx.cs" Inherits="Inphase.CTQS.App.Test.DataGrid" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DataGrid测试页面</title>
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/icon.css" />
    <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/Comm.css" />
    <script type="text/javascript" src="../../Js2/Jquery/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../../Js2/Comm/Comm.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            InitDataGrid();
        });

        function InitDataGrid() {
            $('#IvpnGroupMembers').datagrid({
                title: '测试',
                height: 500,
                fitColumns: true,
                pagination: true,
                singleSelect: true,
                rownumbers: false,
                nowrap: true,
                striped: true,
                collapsible: false,
                idField: 'HLR_LogInfoID',
                sortName: 'HLR_LogInfoID',
                sortOrder: 'desc',
                remoteSort: false,
                pageSize: 20,
                pageNumber: 1,
                pageList: [10, 20, 30, 40, 50],
                columns: [[
                                { field: 'HLR_LogInfoID', title: 'ID', width: 138, align: 'center', sortable: true },
                                { field: 'MDN', title: 'MDN', width: 140, align: 'center', sortable: false },
                                { field: 'HLR_OperationTime', title: '操作时间', width: 80, align: 'center', sortable: true },
                                { field: 'HLR_OperationStatus', title: '操作结果', width: 80, align: 'center', sortable: false,
                                    formatter: function (value) {
                                        if (value == '0') {
                                            return '成功';
                                        }
                                        else {
                                            return '<span style="color:#6f6f6f">失败</span>';
                                        }
                                    }
                                },
                                { field: 'Client_IP', title: '操作员IP', width: 80, align: 'center', sortable: false },
                                { field: 'operator', title: '操作', width: 70, align: 'center',
                                    formatter: function (value, row, index) {
                                        //                                        var t = '<a href="javascript:void(' + row.TR_ID + ');" class="menuAnchor" onclick="javascript:doViewDetail(' + row.TR_ID + ');return false;" title="查看详细信息">查看</a>';
                                        //                                        return t;
                                        //alert(row.HLR_LogInfoID);
                                    }
                                }
                        ]]
            });

            //设置分页控件
            var p = $('#IvpnGroupMembers').datagrid('getPager');
            $(p).pagination({
                onSelectPage: function (pageIndex, pageSize) {
                    //                                    displayData(pageIndex, pageSize);
                    actionQuery(pageIndex);
                }
            });
        }

        function actionQuery(pageIndex) {
            var mdn = $("#txtMdn").val();
            //            if (mdn == null || mdn == "") {
            //                $.messager.alert('错误', '号码不能为空！', 'error');
            //                $("#mdn").focus();
            //                return false;
            //            }

            $("#queryData").linkbutton("disable");
            $('#IvpnGroupMembers').datagrid("loading");

            // Ajax方式
            var $pager = $("#IvpnGroupMembers").datagrid("getPager");
            var opt = $pager.pagination("options");
            var aj = new AjaxJson({ type: "POST", data: { action: "query", pageIndex: pageIndex, pageSize: opt.pageSize, mdn: mdn} });
            aj.send({
                complete: function (JR) {
                    $('#IvpnGroupMembers').datagrid("loaded");
                    //                    if (flag == "q") {
                    $("#queryData").linkbutton("enable");
                    //                    }
                },
                success: function (JR) {
                    $('#IvpnGroupMembers').datagrid("loadData", JR.responseData.data);
                    var $pager = $("#IvpnGroupMembers").datagrid("getPager");
                    var opt = $pager.pagination("options");
                    //                    if (opt.pageNumber != pageIndex) {
                    //                        $pager.pagination({ pageNumber: pageIndex });
                    //                    }
                }
            });

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table style="width: 1000px;" class="table_Dialog">
        <tr>
            <td>
                <div style="width: 100%;" class="Search">
                    MDN：<input id="txtMdn" type="text" maxlength="20" class="textfield" runat="server" />
                    <a id="queryData" class="easyui-linkbutton" onclick="javascript:return actionQuery(1);"
                        iconcls="icon-search">查询</a>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <table id="IvpnGroupMembers">
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
