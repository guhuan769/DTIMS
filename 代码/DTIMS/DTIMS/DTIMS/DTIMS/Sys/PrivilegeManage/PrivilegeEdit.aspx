<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrivilegeEdit.aspx.cs"
    Inherits="CTQS.Sys.PrivilegeManage.PrivilegeEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>权限组属性</title>
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/default/easyui.css" />
    <link rel="stylesheet" type="text/css" href="../../Css2/Jquery/themes/icon.css" />
    <link rel="Stylesheet" type="text/css" href="../../Css2/Comm/Comm.css" />
    <script type="text/javascript" src="../../Js2/Jquery/jquery-1.7.1.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/jquery.easyui.min.js"></script>
    <script type="text/javascript" src="../../Js2/Jquery/locale/easyui-lang-zh_CN.js"></script>
    <script type="text/javascript" src="../../Js2/Comm/Comm.js"></script>
    <script type="text/javascript" language="javascript">
        function closeWindow(r) {
            window.parent.CloseWindow(r);
        }

        function validateForm() {
            if ($("#txtPrivGroup_Name").val() == "") {
                $.messager.alert("提示", "权限组名称不能为空!", "info", function () {
                    $("#txtPrivGroup_Name").get(0).focus();
                }); return false;
            }
        }

        function prompt(result) {
            $.messager.alert("提示", result, "info", function () {
                closeWindow('succ');
            });
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

    </script>
</head>
<body style="margin: 0px; padding: 0px;">
    <form id="form1" runat="server" defaultfocus="txtPrivGroup_Name">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="margin: 0px; padding: 0px;">
                <tr>
                    <td>
                        <fieldset style="width: 250px; height: 300px;">
                            <legend>功能权限</legend>
                            <div style="overflow: auto; height: 290px; margin: 0px; padding: 0px;">
                                <asp:TreeView ID="trvFunctionItem" runat="server" ParentNodeStyle-ImageUrl="~/Images/TreeView/folder.gif"
                                    RootNodeStyle-ImageUrl="~/images/TreeView/control_panel.gif" LeafNodeStyle-ImageUrl="~/images/TreeView/word.gif"
                                    ExpandImageUrl="~/images/TreeView/folder_open.gif" ShowLines="true" CssClass="TreeView"
                                    SelectedNodeStyle-CssClass="SelectedTreeNode" HoverNodeStyle-CssClass="HoverTreeNode"
                                    Height="280px" Width="200px">
                                </asp:TreeView>
                            </div>
                        </fieldset>
                    </td>
                    <td>
                        <table class="table_Dialog" style="height: 303px;">
                            <tr>
                                <td class="header" style="width: 90px; height: 30px;">
                                    所属地区:
                                </td>
                                <td style="width: 190px;">
                                    <asp:Label ID="lblAreaName" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="header" style="height: 30px;">
                                    权限组名称:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPrivGroup_Name" MaxLength="10" runat="server"></asp:TextBox><span
                                        style="color: red">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="header">
                                    权限组说明:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPrivGroup_Desc" MaxLength="25" runat="server" Height="158px"
                                        TextMode="MultiLine" Style="width: 180px; max-width: 180px; min-width: 180px;
                                        max-height: 158px; min-height: 158px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="left" style="color: Red;">
                                    说明:权限组名称不能为空.
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <%--    <div style="margin: 3px 5px 5px 5px; padding: 0px; text-align: right;">
                    <asp:Button ID="btnAdd" Text = "确　定" OnClientClick="return validateForm();" CssClass="button2d" runat="server" OnClick="btnAdd_Click" />
                    <input type="button" value="关　闭" class="button2d" onclick="closeWindow('');" />
                </div>--%>
            <div class="dialog-container-footer">
                <asp:LinkButton ID="btnAdd" class="easyui-linkbutton" iconcls="icon-save" Text="确定"
                    runat="server" OnClientClick="return validateForm();" OnClick="btnAdd_Click" />
                <a id="lbtnCancel" href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-cancel"
                    onclick="closeWindow('');">取消</a>
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
    </script>
</body>
</html>
<script type="text/javascript">
    // 重新绑定 Jquery easyUi 控件样式。
    function msAjaxEndRequest(sender, args) {
        $("#btnAdd").linkbutton();
        $("#lbtnCancel").linkbutton();
    }
</script>
<script src="../../Js2/Comm/AjaxTimeout.js" type="text/javascript"></script>
