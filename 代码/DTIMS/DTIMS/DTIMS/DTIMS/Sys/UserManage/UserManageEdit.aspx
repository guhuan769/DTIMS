<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserManageEdit.aspx.cs"
    Inherits="CTQS.Sys.UserManage.UserManageEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户管理</title>
    <%--<link rel="stylesheet" type="text/css" href="../../Css/tablestyle.css" />--%>
    <%--<script language="javascript" src="../../Js/jquery-1.4.2.min.js" type="text/javascript"></script>--%>
    <%--<script language="javascript" src="../../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>--%>
    <%--<link href="../../Css/main.css.aspx" type="text/css" rel="stylesheet" />--%>
    <%--<script type="text/javascript" src="<%=Request.ApplicationPath%>/js/common.js.aspx"></script>--%>
    <%--<script language="javascript" src="../../Js/jquery.easyui.min.js" type="text/javascript"
        charset="gb2312"></script>--%>
    <%--<link href="../../Css/customer.ui.css" type="text/css" rel="stylesheet" />--%>
    <%--<link href="../../Css/Common/Common.css" type="text/css" rel="stylesheet" />--%>
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/icon.css" />
    <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/Comm.css" />
    <script type="text/javascript" src="../../Js2/Jquery/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../../Js2/Comm/Comm.js"></script>
    <script type="text/javascript">
        var cDialog = null;
        $(document).ready(function () {
            //加载模态对话框
            cDialog = new customerDialog("用户信息", true);
            InitPage();
        });

        function InitPage() {
            InitPwdTextBox();
        }

        function InitPwdTextBox() {
            if ($("#TextBox2").get(0) == null) {
                $("#showPwdImg").css("display", "none");
                return;
            }
            var pos = getElementPos("TextBox2");
            $("#showPwdImg").css({ top: pos.y + "px", left: pos.x + "px" });
            $("#showPwdImg").click(function () {
                $("#showPwdImg").css("display", "none");
                $("#TextBox2").focus();
            });
            $("#TextBox2").blur(function () {
                //如果有值，则不隐藏
                if ($("#TextBox2").val() == "") {
                    $("#showPwdImg").css("display", "");
                }
            });
            $("#TextBox2").focus(function () {
                $("#showPwdImg").css("display", "none");
            });
            // alert("距左边距离"+ pos.x +",距上边距离"+pos.y);
        }

        //查看
        function doView(id) {
            var url = 'UserManage.aspx?id=' + id + '&type=view&time=' + new Date().getDate();
            cDialog.open(url, { width: "500px", height: "500px" });
        }

        function validateForm() {
            if ($("#txtUserName").val() == "") {
                $.messager.alert("提示", "请输入用户名!", "info", function () {
                    $("#txtUserName").get(0).focus(); InitPage();
                }); return false;
            }
            if ($("#TextBox1").val() == "") {
                $.messager.alert("提示", "请输入登录名!", "info", function () {
                    $("#TextBox1").get(0).focus();
                }); return false;
            }
            var checkList = "";
        }

        //添加成功，清空数据
        function isSuccess() {
            window.parent.closeDialog("succ");
        }

        function closeDia() {
            window.parent.closeDialog("false");
        }

        function closeWindow(re) {
            if (re == "succ") {
                window.parent.closeDialog(re);
            }
        }

        function checkAll() {
            var isCheck = document.getElementById("selectAll").checked;
            if (isCheck) {
                $("#roleDiv input[type='checkbox']").attr("checked", "checked");
            }
            else {
                $("#roleDiv input[type='checkbox']").attr("checked", "");
            }
        }


        function public_GetParentByTagName(element, tagName) {
            var parent = element.parentNode;
            var upperTagName = tagName.toUpperCase();

            while (parent && (parent.tagName.toUpperCase() != upperTagName)) {
                parent = parent.parentNode ? parent.parentNode : parent.parentElement;
            }
            return parent;
        }

        function setParentChecked(objNode) {
            var objParentDiv = public_GetParentByTagName(objNode, "div");
            if (objParentDiv == null || objParentDiv == "undefined") {
                return;
            }
            var objID = objParentDiv.getAttribute("ID");
            objID = objID.substring(0, objID.indexOf("Nodes"));
            objID = objID + "CheckBox";
            var objParentCheckBox = document.getElementById(objID);
            if (objParentCheckBox == null || objParentCheckBox == "undefined") {
                return;
            }
            if (objParentCheckBox.tagName != "INPUT" && objParentCheckBox.type == "checkbox")
                return;
            objParentCheckBox.checked = true;
            setParentChecked(objParentCheckBox);
        }

        function setParentUnChecked(objNode) {
            //获取父节点类
            var objParentDiv = public_GetParentByTagName(objNode, "div");
            if (objParentDiv == null || objParentDiv == "undefined") {
                return;
            }
            var objID = objParentDiv.getAttribute("ID");
            var parentIndex = parseFloat(objID.substr(objID.indexOf("Nodes") - 1, 1));
            objID = objID.substring(0, objID.indexOf("Nodes") - 1);

            var hasChildChecked = false;
            var objchild = objParentDiv.children; //获取父节点类所有子节点
            for (var i = 1; i <= objchild.length; i++) {
                var childObjID = objID + (parentIndex + i) + "CheckBox";
                var tmpObj = document.getElementById(childObjID);
                if (tmpObj.checked == true) {
                    hasChildChecked = true;
                }
            }

            //如果所有子节点都没有选中，将父节点选择取消
            if (hasChildChecked == false) {
                objID = objID + parentIndex + "CheckBox";
                var objParentCheckBox = document.getElementById(objID);
                if (objParentCheckBox == null || objParentCheckBox == "undefined") {
                    return;
                }
                if (objParentCheckBox.tagName != "INPUT" && objParentCheckBox.type == "checkbox")
                    return;
                objParentCheckBox.checked = false;
                setParentUnChecked(objParentCheckBox);
            }

        }

        function setChildUnChecked(divID) {
            var objchild = divID.children;
            var count = objchild.length;
            for (var i = 0; i < objchild.length; i++) {
                var tempObj = objchild[i];
                if (tempObj.tagName == "INPUT" && tempObj.type == "checkbox") {
                    tempObj.checked = false;
                }
                setChildUnChecked(tempObj);
            }
        }
        function setChildChecked(divID) {
            var objchild = divID.children;
            var count = objchild.length;
            for (var i = 0; i < objchild.length; i++) {
                var tempObj = objchild[i];
                if (tempObj.tagName == "INPUT" && tempObj.type == "checkbox") {
                    tempObj.checked = true;
                }
                setChildChecked(tempObj);
            }
        }

        //触发事件
        function CheckEvent(event) {
            var evt = event ? event : (window.event ? window.event : null);
            var objNode = evt.srcElement ? evt.srcElement : evt.target;

            if (objNode.tagName != "INPUT" || objNode.type != "checkbox")
                return;

            if (objNode.checked == true) {
                setParentChecked(objNode);
                var objID = objNode.getAttribute("ID");
                var objID = objID.substring(0, objID.indexOf("CheckBox"));
                var objParentDiv = document.getElementById(objID + "Nodes");
                if (objParentDiv == null || objParentDiv == "undefined") {
                    return;
                }
                setChildChecked(objParentDiv);
            }
            else {
                //setParentUnChecked(objNode);//检查是否要取消父节点勾选项

                var objID = objNode.getAttribute("ID");
                var objID = objID.substring(0, objID.indexOf("CheckBox"));
                var objParentDiv = document.getElementById(objID + "Nodes");

                if (objParentDiv == null || objParentDiv == "undefined") {
                    return;
                }
                setChildUnChecked(objParentDiv);
            }
        }

        function getElementPos(elementId) {
            var ua = navigator.userAgent.toLowerCase();
            var isOpera = (ua.indexOf('opera') != -1);
            var isIE = (ua.indexOf('msie') != -1 && !isOpera); // not opera spoof
            var el = document.getElementById(elementId);
            if (el.parentNode === null || el.style.display == 'none') {
                return false;
            }
            var parent = null;
            var pos = [];
            var box;
            if (el.getBoundingClientRect)    //IE
            {
                box = el.getBoundingClientRect();
                var scrollTop = Math.max(document.documentElement.scrollTop, document.body.scrollTop);
                var scrollLeft = Math.max(document.documentElement.scrollLeft, document.body.scrollLeft);
                return { x: box.left + scrollLeft, y: box.top + scrollTop };
            } else if (document.getBoxObjectFor)    // gecko    
            {
                box = document.getBoxObjectFor(el);
                var borderLeft = (el.style.borderLeftWidth) ? parseInt(el.style.borderLeftWidth) : 0;
                var borderTop = (el.style.borderTopWidth) ? parseInt(el.style.borderTopWidth) : 0;
                pos = [box.x - borderLeft, box.y - borderTop];
            } else    // safari & opera    
            {
                pos = [el.offsetLeft, el.offsetTop];
                parent = el.offsetParent;
                if (parent != el) {
                    while (parent) {
                        pos[0] += parent.offsetLeft;
                        pos[1] += parent.offsetTop;
                        parent = parent.offsetParent;
                    }
                }
                if (ua.indexOf('opera') != -1 || (ua.indexOf('safari') != -1 && el.style.position == 'absolute')) {
                    pos[0] -= document.body.offsetLeft;
                    pos[1] -= document.body.offsetTop;
                }
            }
            if (el.parentNode) {
                parent = el.parentNode;
            } else {
                parent = null;
            }
            while (parent && parent.tagName != 'BODY' && parent.tagName != 'HTML') { // account for any scrolled ancestors
                pos[0] -= parent.scrollLeft;
                pos[1] -= parent.scrollTop;
                if (parent.parentNode) {
                    parent = parent.parentNode;
                } else {
                    parent = null;
                }
            }
            return { x: pos[0], y: pos[1] };
        }
    </script>
</head>
<body>
    <input type="hidden" value="false" id="isUpdate" />
    <form id="form1" runat="server" defaultfocus="txtUserName" style="overflow: hidden;">
    <asp:HiddenField ID="hidPwd" runat="server" Value="123456" />
    <asp:ScriptManager ID="scriptManager1" runat="server" />
    <table width="100%">
        <tr>
            <td id="isSuperNo" runat="server">
                <div id="tt" runat="server" class="easyui-tabs" style="height: 345px; width: 200px">
                    <div title="权限组" style="overflow: auto; width: 100%; text-align: left" id="roleDiv">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <input id="selectAll" value="" type="checkbox" onclick="checkAll()" />全选
                                <asp:CheckBoxList ID="checkListRole" runat="server" Width="180" RepeatDirection="Vertical">
                                </asp:CheckBoxList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div title="私有权限" style="overflow: auto;" id="privateDiv">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:TreeView ID="trvFunctionItem" runat="server" ParentNodeStyle-ImageUrl="~/Images/TreeView/folder.gif"
                                    RootNodeStyle-ImageUrl="~/images/TreeView/control_panel.gif" LeafNodeStyle-ImageUrl="~/images/TreeView/word.gif"
                                    ExpandImageUrl="~/images/TreeView/folder_open.gif" ShowLines="true" CssClass="TreeView"
                                    SelectedNodeStyle-CssClass="SelectedTreeNode" HoverNodeStyle-CssClass="HoverTreeNode"
                                    Height="280px" Width="195px">
                                </asp:TreeView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <div id="progressBar" runat="server">
                                    <img alt="" src="../../Images/Common/loading.gif" /><span>正在操作，请稍候…</span></div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>--%>
                        <asp:HiddenField ID="hidPrivateID" runat="server" />
                        <table class="table_Dialog" style="width: 100%; height: 345px;">
                            <tr>
                                <td class="header" style="width: 80px; border-left: 0px">
                                    用户名：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUserName" runat="server" Width="200" MaxLength="10"></asp:TextBox>
                                    <span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="header">
                                    登录名：
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox1" runat="server" Width="200" MaxLength="10"></asp:TextBox>
                                    <span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr id="pwd1" runat="server">
                                <td class="header">
                                    密码：
                                </td>
                                <td>
                                    <input id="TextBox2" maxlength="100" style="width: 200px" type="password" name="textBoxPwd"
                                        runat="server" />
                                    <span style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="header">
                                    角色：
                                </td>
                                <td>
                                    <asp:DropDownList ID="dropRule" Width="200" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="header">
                                    地区：
                                </td>
                                <td>
                                    <asp:DropDownList ID="dropAreaLists" Width="200" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="header">
                                    状态：
                                </td>
                                <td>
                                    <asp:RadioButton ID="ok" runat="server" Checked="true" GroupName="status" Text="正常" />
                                    <asp:RadioButton ID="no" runat="server" GroupName="status" Text="禁用" />
                                </td>
                            </tr>
                            <tr>
                                <td class="header">
                                    备注：
                                </td>
                                <td>
                                    <asp:TextBox ID="TextBox4" runat="server" Width="250" MaxLength="25" Height="100"
                                        TextMode="MultiLine" Style="max-width: 250px; min-width: 250px; max-height: 100px; min-height: 100px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" id="showDis" runat="server">
                                    <span style="color: red">注：*号为必填项<span id="spanMsg" runat="server"></span></span>。<br />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="fdasf" runat="server">
        <ContentTemplate>
            <%--<div style="float: right; text-align: right; margin-top: 15px;">
                <asp:Button ID="btnAdd" runat="server" Text=" 保 存 " OnClientClick="return validateForm()"
                    CssClass="button2d" OnClick="btnAdd_Click" />
                <input type="button" onclick="closeDia();" class="button2d" value=" 关 闭 " />
                &nbsp;&nbsp;
            </div>--%>
            <div class="dialog-container-footer">
                <asp:LinkButton ID="btnAdd" class="easyui-linkbutton" iconcls="icon-save" Text="确定"
                    runat="server" OnClientClick="javascript:return validateForm();" OnClick="btnAdd_Click" />
                <a id="lbtnCancel" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-cancel"
                    onclick="javascript:return closeDia();">取消</a>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
    <script type="text/javascript">
        //让FireFox兼容支持Children属性
        function isIE() { //ie? 判断是不是ie 
            if (window.navigator.userAgent.indexOf("MSIE") >= 1) {
                return true;
            }
            else {
                return false;
            }
        }

        if (!isIE()) {
            HTMLElement.prototype.__defineGetter__("children",
            function () {
                var returnValue = new Object();
                var number = 0;
                for (var i = 0; i < this.childNodes.length; i++) {
                    if (this.childNodes[i].nodeType == 1) {
                        returnValue[number] = this.childNodes[i];
                        number++;
                    }
                }
                returnValue.length = number;
                return returnValue;
            }
            );
        }

        function prompt(result) {
            InitPage();
            $.messager.alert("提示", result, "info", function () {
                closeWindow('succ');
            });

        }

        function error(result) {
            InitPage();
            $.messager.alert("错误", result, "error", function () {
                $("#TextBox1").get(0).focus();
            });

        }
    </script>
    <div id="showPwdImg" style="position: absolute; cursor: pointer; background-image: url(../../Images/Common/pwdImg.gif);
        width: 210px; height: 21px" title="点击输入密码">
    </div>
</body>
</html>
<script type="text/javascript">
    function msAjaxBeginRequest(sender, args) {
        window.parent.$.messager.progress({ title: "请稍等", msg: "提交数据中..." });
    }

    // 重新绑定 Jquery easyUi 控件样式。
    function msAjaxEndRequest(sender, args) {
        $("#btnAdd").linkbutton();
        $("#lbtnCancel").linkbutton();
        window.parent.$.messager.progress("close");
    }
</script>
<script src="../../Js2/Comm/AjaxTimeout.js" type="text/javascript"></script>
