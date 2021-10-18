<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Inphase.CTQS.App.Test.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/icon.css" />
    <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/Comm.css" />
    <script type="text/javascript" src="../../Js2/Jquery/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../../Js2/Comm/Comm.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            InitGroupMembers();
        });

        function actionQuery() {
            //判断集团号码
            var groupNum = $("#txtGroupNum").val();
            if (groupNum == null || groupNum == "") {
                $.messager.alert('错误', '集团号码不能为空！', 'error');
                $("#txtGroupNum").focus();
                return false;
            }

            $("#queryMembers").linkbutton("disable");
            $('#IvpnGroupMembers').datagrid("loading");

            // Ajax方式
            //            var $pager = $("#IvpnGroupMembers").datagrid("getPager");
            //            var opt = $pager.pagination("options");
//            var aj = new AjaxJson({ type: "POST", data: { action: "query", pageIndex: 1, pageSize: 50, groupNum: groupNum} });
//            aj.send({
//                complete: function (JR) {
//                    $('#IvpnGroupMembers').datagrid("loaded");
//                    //                    if (flag == "q") {
//                    $("#queryMembers").linkbutton("enable");
//                    //                    }
//                },
//                success: function (JR) {
//                    $('#IvpnGroupMembers').datagrid("loadData", JR.responseData.data);
//                    //                    var $pager = $("#IvpnGroupMembers").datagrid("getPager");
//                    //                    var opt = $pager.pagination("options");
//                    //                    if (opt.pageNumber != pageIndex) {
//                    //                        $pager.pagination({ pageNumber: pageIndex });
//                    //                    }
//                }
            //            });


            //get方法
//            $.get("TestJson.aspx?userName=123", function (data) {

//                //index.ashx后台地址 userName参数  123参数值
//                //data是从后台返回来的数据

//                $('#IvpnGroupMembers').datagrid("loadData", data);
//                        });

//            //post方法：
//            $.post("TestJson.aspx", { userName: "你好" }, function (data) {
//              //  //index.ashx后台地址 userName参数  123参数值
//               //data是从后台返回来的数据
//            });


//get方法：$.get("index.ashx?userName=123",function(data){
//   //index.ashx后台地址 userName参数  123参数值
//   //data是从后台返回来的数据
//});
//post方法：
//$.post("index.ashx",{userName:"你好"},function(data){
//  //  //index.ashx后台地址 userName参数  123参数值
//   //data是从后台返回来的数据
//});

        }


        function InitGroupMembers() {
//            $('#IvpnGroupMembers').datagrid({
//                title: '集团用户',
//                height: 365,
//                fitColumns: true,
//                pagination: true,
//                singleSelect: true,
//                rownumbers: true,
//                nowrap: true,
//                striped: true,
//                collapsible: false,
//                idField: 'MSISDN',
//                sortName: 'MSISDN',
//                sortOrder: 'desc',
//                remoteSort: true,
//                pageSize: 50,
//                pageNumber: 1,
//                pageList: [10, 20, 30, 40, 50],
//                columns: [[
//                                { field: 'GroupNumber', title: '集团号码', width: 138, align: 'center', sortable: false },
//                                { field: 'MSISDN', title: '物理号码', width: 140, align: 'center', sortable: false },
//                                { field: 'ShortNumber', title: '分机号码', width: 80, align: 'center', sortable: false },
//                                { field: 'UserState', title: '用户状态', width: 80, align: 'center', sortable: false },
//                                { field: 'NetType', title: '网络类型', width: 80, align: 'center', sortable: false },
//                                { field: 'CallRight', title: '话机呼叫权限', width: 130, align: 'center', sortable: false },
//                                { field: 'Offnetid', title: '网外呼叫权限', width: 130, align: 'center', sortable: false },
//                                { field: 'ShowType', title: '短号显示方式', width: 138, align: 'center', sortable: false }
//                        ]]
            //            });

            var jsonUrl = 'TestJson.aspx';
            $('#IvpnGroupMembers').datagrid({
                title: '',
                iconCls: 'icon-save',
                height: 385,
                nowrap: true,
                striped: true,
                singleSelect: true,
                collapsible: false,
                url: jsonUrl,
                sortName: 'CLLS',
                sortOrder: 'desc',
                remoteSort: false,
                idField: 'ID',
                pageSize: 20,
                pageNumber: 1,
                pageList: [5, 10, 20, 30, 40, 50, 100],
                frozenColumns: [[
                 	{ title: '工单流水号', field: 'ID', width: 175, sortable: false,
                 	    formatter: function (value, rec) {
                 	        para = '<a href="#" title="点击查看详细" onclick="ShowDetial(\'' + value + '\',' + type + ')">';
                 	        var arryMsgStreamNo = value.toString().split(":");
                 	        if (type == 1) {
                 	            para += '<span>' + arryMsgStreamNo[3] + '</span></a>';
                 	        }
                 	        else {
                 	            para += '<span>' + arryMsgStreamNo[1] + '</span></a>';
                 	        }
                 	        return para;
                 	    }
                 	},
                    { title: '逻辑号码', field: 'CLDH', width: 80, sortable: false },
                    { title: '新逻辑号码', field: 'NCLDH', width: 80, sortable: false },
                    { field: 'ResultDescription', title: '处理结果', width: 100,
                        formatter: function (value) {
                            if (value == '自动执行失败' || value == '人工处理失败') {
                                return '<span style="color:#ff0000">' + value + '</span>';
                            }
                            else if (value == '正在处理') {
                                return '<span style="color:#dd33ff">' + value + '</span>';
                            }
                            else if (value == '待处理') {
                                return '<span style="color:#6f6f6f">' + value + '</span>';
                            }
                            else {
                                return '<span >' + value + '</span>';
                            }
                        }
                    },
                    { title: '地区', field: 'AREA_Name', width: 80, sortable: false }
				]],
                columns: [[
                    { field: 'BSCE_ID', title: '工单来源', width: 80, align: 'center' },
                    { field: 'DETT_Name', title: '工单类型', width: 110 },
                    { field: 'SBILL_RecevDateTime', title: '接收时间', width: 115 },
                    { field: 'BTASK_ExecTime', title: '执行时间', width: 115 },
                    { field: 'EXEC_PrintNo', title: '打印编号', width: 200 }
				]],
                pagination: true,
                rownumbers: true,
                onDblClickRow: function () {
                    var selected = $('#IvpnGroupMembers').datagrid('getSelected');
                    if (selected) {
                        ShowDetial(selected.ID, type);
                    }
                }
            });
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
     <table style="width: 100%;" class="table_Dialog">
        <tr>
            <td>
                <div style="width: 100%;" class="Search">
                    集团号码：<input id="txtGroupNum" type="text" maxlength="20"
                        class="textfield" runat="server" />
                    <a id="queryMembers" href="javascript:void(0);" class="easyui-linkbutton" onclick="javascript:return actionQuery();"
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
