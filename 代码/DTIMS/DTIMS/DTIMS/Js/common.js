
// 得到包含汉字的字符串的字节数
function getByteNum(s) {
   var rt = 0;
   if ('string' != typeof(s)) return rt;
   for (var i=0; i<s.length; i++) {
      var c = s.charCodeAt(i);
      if (c > 0xff) rt++;
      rt++;
   }
   return rt;
}

// 处理字符串使之可以作为html控件的value
function getAsValue(s) {
    var rt = "";
    if (typeof(s) == "undefined") return rt;

    var v = s;
    v = v.replace(/\&/, "&#38;");
    v = v.replace(/\'/, "&#39;");
    v = v.replace(/\"/, "&#34;");
    v = v.replace(/\r/, "&#13;");
    v = v.replace(/\n/, "&#10;");

    return v;
}

// 得到对象的绝对y位置
function getAbsTop(obj) {
    var o = obj;
    var t = o.offsetTop;
    while (o = o.offsetParent) t += o.offsetTop;
    return t;
}

// 得到对象的绝对x位置
function getAbsLeft(obj) {
    var o = obj;
    var l = o.offsetLeft;
    while (o = o.offsetParent) l += o.offsetLeft;
    return l;
}

// 禁止某事件，如用户执行页面文本拖选操作

function selectstart() {
    window.event.cancelBubble = true;
    window.event.returnValue = false;
    return false;
}

// 给String对象添加trim()方法
String.prototype.trim = function() {
    return this.replace(/(^(\s|\u3000)*)|((\s|\u3000)*$)/g, "") ;
}

// 通过选择或取消选择一个checkbox而选择或取消选择一组checkbox
function checkAllByClickBox(checkboxsname) {
    var eventObj = window.event.srcElement;
    var formObj  = eventObj.form;
    if (formObj == null) formObj = document.all;
    var boo = false;
    if (eventObj.checked) boo = true;

    var checkObj = eval("formObj." + checkboxsname);
    if (typeof(checkObj) != 'undefined') checkAll(checkObj, boo);
}

// 选择或取消选择一组checkbox
function checkAll(checkObj, boo) {
    if (typeof(checkObj.length) != 'undefined') {
        for (var i=0; i<checkObj.length; i++) checkObj[i].checked = boo;
    } else {
        checkObj.checked = boo;
    }
}

// 一组对象是否都不为空

function checkNotNull(obj) {
    if (typeof(obj) == 'undefined') return true;
    if (typeof(obj.type) == 'string') {
        if (obj.value.trim() == "") return false;
        return true;
    }
    for (var i=0; i<obj.length; i++) if (obj[i].value.trim() == "") return false;
    return true;
}

// 一组checkbox是否有选择了的
function hasChecked(checkObj) {
    if (typeof(checkObj) == 'undefined') return false;
    if (typeof(checkObj.length) != 'undefined') {
        for (var i=0; i<checkObj.length; i++) {
            if (checkObj[i].checked) return true;
        }
    } else return checkObj.checked;
    return false;
}

// 是否一组checkbox都选择了

function isAllChecked(checkObj) {
    if (typeof(checkObj) == 'undefined') return false;
    if (typeof(checkObj.length) != 'undefined') {
        for (var i=0; i<checkObj.length; i++) {
            if (!checkObj[i].checked) return false;
        }
    } else return checkObj.checked;
    return true;
}

function openWinNoReturn(url, winName, w, h, otherPara) {
   openWin(url, winName, w, h, otherPara);
}

// 开启一般窗口

function openWin(url, winName, w, h, otherPara) {
   var wp = .7;
   var hp = .8;
   if (typeof(w)=='number' && w>0) wp = w;
   if (typeof(h)=='number' && h>0) hp = h;

   var width;
   if (wp > 1) width = wp;
   else width = screen.availWidth*wp;

   var height;
   if (hp > 1) height = hp;
   else height = screen.availHeight*hp;

   var str = "height="+(height-56)+",width=" + (width-4);

   var xc = (screen.availWidth - width)/2;
   var yc = (screen.availHeight-height)/2;
   str += ",left=" + xc + ",top=" + yc;

   if (typeof(otherPara)!='undefined' && otherPara!='') str += "," + otherPara;

   var wn;
   if (typeof(winName)=='undefined' || winName=='') wn = "w" + Math.random();
   else wn = winName;
   wn = wn.replace('.','_');

   var theWin = window.open(url, wn, str);
   theWin.moveTo(-4, -4);

   return theWin;
}

/*
 * 预定的open window显示方式
 *
 *  mode参数：

 *  　　小于0为较小窗体（40%）

 *  　　等于0为适中窗体（60%）

 *  　　大于0为较大窗体（80%）

 */
function showWin(url, mode, winName) {
   var m = mode;
   if ('undefined' == typeof(m)) m = 0;
   else if ('string' == typeof(m)) m = parseFloat(m);
   else if ('number' != typeof(m)) return;

   if (m < 0) return openWin(url, winName, .4, .4, 'resizable,scrollbars');
   else if (m == 0) return openWin(url, winName, .6, .6, 'resizable,scrollbars');
   else if (m > 0) return openWin(url, winName, .8, .8, 'resizable,scrollbars');
}

/*
自定义窗口大小：
	url：链接地址
	width：窗口宽度

	height:窗口高度
	winName:窗口名称
*/
function ShowCustomWin(url,width,height,winName)
{
    openWin(url, winName, width, height, 'status=no,toolbars=no,menubar=no,resizable');

	
}


// 开启模式窗口

function openDialog(url, args, w, h, otherPara) {
   var wp = .7;
   var hp = .8;
   if (typeof(w)=='number' && w>0) wp = w;
   if (typeof(h)=='number' && h>0) hp = h;

   var width;
   if (wp > 1) width = wp;
   else width = screen.availWidth*wp;

   var height;
   if (hp > 1) height = hp;
   else height = screen.availHeight*hp;

   var str = "dialogHeight:"+height+"px;dialogWidth:" + width+"px";

   if (typeof(otherPara)=='string' && otherPara!='') str += ";" + otherPara;

   var a = '';
   if (typeof(args) != 'undefined') a = args;

   return showModalDialog(url, a, str);
}

/*
 * 预定的modalDialog显示方式
 *
 *  mode参数：

 *  　　小于0为较小窗体（40%）

 *  　　等于0为适中窗体（60%）

 *  　　大于0为较大窗体（80%）

 */
function showDialog(url, mode, args) {
   var m = mode;
   if ('undefined' == typeof(m)) m = 0;
   else if ('string' == typeof(m)) m = parseFloat(m);
   else if ('number' != typeof(m)) return;

   if (m < 0) return openDialog(url, args, .4, .4, 'status:no;help:no');
   else if (m == 0) return openDialog(url, args, .6, .6, 'status:no;help:no');
   else if (m > 0) return openDialog(url, args, .8, .8, 'status:no;help:no');
}

function isInt(i) {
    return !isNaN(i) && (new String(i).indexOf('.') == -1);
}

// 强制输入整数
function forceInt(max) {

   // 限定该方法必须使用 onkeydown 事件触发
   if (event.type != 'keydown') return;

    var i = window.event.keyCode;

    // 8是backspace的代码，9是tab的代码，46是delete的代码

    if (i==8 || i==9 || i==46) return;
    // 37,38,39,40分别是左上右下方向键的代码

    if (i>=37 && i<=40) return;
    // ctrl + c
    if (window.event.ctrlKey && i==67) return;
    // ctrl + x
    if (window.event.ctrlKey && i==88) return;

    // 48是0的代码, 57是9的代码, 96是小键盘的0的代码, 105是小键盘的9的代码

    if  ( i < 48 || (i>57&&i<96) || i > 105) {
        window.event.returnValue = false;
        return;
    }
    if (i > 57) i -= 48;

    if (typeof(max)=='undefined' || !isInt(max)) return;

    var obj = document.selection;
    var obj2 = obj.createRange(); // TextRange
    var selectText = obj2.text;
    if (selectText != '') obj.clear();

    var value = window.event.srcElement.value.trim();
    var num = new Number(value) * 10 + (i - 48);
    if (num > max) window.event.returnValue = false;
}

// 强制输入浮点数

function forceFloatValue(max, min) {
    var o = window.event.srcElement;

    var av = "0123456789.";
    var v = o.value;
    if (v == o.prevValue) return;

    // 去掉第一个'+'号

    if (v.charAt(0) == '+') v = v.substr(1);

    // 去掉多余的'0', 消除8进制歧义
    while (v.length > 0) {
        if (v.charAt(0) != '0') break;
        v = v.substr(1);
    }

    if (v == '')  v = 0;
    if (v == o.prevValue) {
        o.value = v;
        return;
    }

    for (var i=0; i<v.length; i++) if (av.indexOf(v.charAt(i)) == -1) {
        alert("只能输入字符(0123456789.)！");
        o.value = o.prevValue;
        return;
    }
    v = parseFloat(v);
    if (v>max || v<min) {
        alert('要求输入 '+min+' - '+max+' 间的数！');
        o.value = o.prevValue;
        return;
    }

    o.value = v;
    o.prevValue = v;
}

// 插入由html指定table行

// oTable: table 对象
// sTr: 一个table行的html代码，如: <tr><td></td></tr>
// i: 插入位置，默认插到最后

function insertRow2Table(oTable, sTr, i) {
   if ('undefined' == typeof(oTable)) return false;
   if ('TABLE' != oTable.nodeName) return false;
   if ('undefined' == typeof(sTr)) return false;
   if ('' == sTr) return false;
   var idx = -1;
   if ('undefined' != typeof(i)) idx = i;
   if ('number' != typeof(idx)) return false;

   oTable.insertAdjacentHTML("beforeBegin", "<table style='display:none'>"+sTr+"</table>");
   oTable.childNodes[0].insertAdjacentElement("afterBegin", oTable.previousSibling.rows[0]);
   oTable.previousSibling.outerHTML = '';

   oTable.moveRow(0, idx);
}

// 使FORM formObj 内的所有控件 disabled，除了 <input type=image> 控件
// 参数 formObj 为FORM对象
// 参数 disabled 为bool值，表示是否disable控件
function disableAll(formObj, disabled) {
   if ('undefined' == typeof(formObj)) return;
   if ('FORM' != formObj.nodeName) return;

   var flg = true;
   if ('boolean' == typeof(disabled)) flg = disabled;

   var allElements = formObj.elements;
   for (var i=0; i<allElements.length; i++) {
      allElements[i].disabled = flg;
   }
}

// disable一组控件controls
// 参数 controls 为控件集合，也可以是一个控件

// 参数 disabled 为bool值，表示是否disable控件
function disableControl(controls, disabled) {
   if ('object' != typeof(controls)) return;

   var flg = true;
   if ('boolean' == typeof(disabled)) flg = disabled;

   if ('undefined' == typeof(controls.length)) return disableAControl(controls, flg);

   for (var i=0; i<controls.length; i++) {
      disableAControl(controls[i], flg);
   }
}

// disable一个控件 controlObj
// 参数 controlObj 为控件对象

// 参数 disabled 为bool值，表示是否disable控件
function disableAControl(controlObj, disabled) {
   if ('undefined' == typeof(controlObj)) return;
   if ('undefined' == typeof(controlObj.disabled)) return;

   var flg = true;
   if ('boolean' == typeof(disabled)) flg = disabled;

   controlObj.disabled = flg;
}

// 隐藏一组控件controls
// 参数 controls 为控件集合，也可以是一个控件

// 参数 disabled 为bool值，表示是否disable控件
function hideControls(controls, isHide) {

   if ('object' != typeof(controls)) return;

   var flg = true;
   if ('boolean' == typeof(disabled)) flg = isHide;

   if ('undefined' == typeof(controls.length)) return disableControl(controls, flg);

   for (var i=0; i<controls.length; i++) {
      disableControl(controls[i], flg);
   }
}

// 隐藏一个控件 controlObj
// 参数 controlObj 为控件对象

// 参数 disabled 为bool值，表示是否disable控件
function hideControl(controlObj, isHide) {
	 
// if ('undefined' == typeof(controlObj)) return;
   //if ('undefined' == typeof(controlObj.style.display)) return;
 
   var flg = true;
   if ('boolean' == typeof(isHide)) flg = isHide;

   if (flg)
   {
    document.getElementById(controlObj).className="HideTable";
   }
   else
   {
    document.getElementById(controlObj).className="ShowTable";	
   }
}
//当人员没有权限时自动转到首页面,并提示用户没有权限

function doGoHome()
{
	//window.alert('你没有权限1！');
	//window.alert(window.top.document.frames[0].document.all.xframe.src);
	window.top.document.frames[0].document.all.xframe.src = '/DDCSTwo/withoutpopedom.htm';
	//window.alert('你没有权限！');
}

//刷新Session.
function dofreshSession()
{
	try
	{
		var obj = window.top.parent;
		var i = 0;
		while(i<=10)
		{
			//alert(obj.name);
			if(obj.name == '_main1')
			{
				break;
			}
			else
			{
				obj = obj.opener.top;
			}
			i++;
		}
		
		if(obj.name == '_main1')
		{
			obj.dofreshwindow();
		}
		else
		{
			//alert('请打开引导主页面');
		}
	}catch(e)
	{
		//alert('error');
	}
}