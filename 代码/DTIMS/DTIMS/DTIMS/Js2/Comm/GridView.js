(function () {
    $(".c-datagrid-row").live("mouseover", function () {
        $(this).addClass("c-datagrid-row-over");
    });

    $(".c-datagrid-row").live("mouseleave", function () {
        $(this).removeClass("c-datagrid-row-over");
    });

    $(".c-datagrid-row").live("click", function () {
        $(".c-datagrid-row").each(function () {
            $(this).removeClass("c-datagrid-row-selected");
        });
        $(this).addClass("c-datagrid-row-selected");
    });

    $(".c-pagination-num").live("keydown", function (e) {
        switch (e.keyCode) {
            case 13:
                var n = $(this).val();
                if (!$.isNumeric(n)) {
                    $.messager.alert("提示", "请输入数字。", "info", function () {
                        setTimeout(function () {
                            $(".c-pagination-num").select();
                        }, 50);
                    });
                    e.preventDefault();
                    e.stopPropagation();
                    return;
                }

                var thisId = $(this).prop("id");
                if (thisId == "txtPageIndex") {
                    __doPostBack("lbtnPageIndex", "");
                }
                else {
                    var id = $(".c-datagrid-view").prop("id");
                    var page = "Page$" + $(this).val();
                    __doPostBack(id, page)
                }
                e.preventDefault();
                e.stopPropagation();
                break;
            default:
                break;
        }
    });


})();