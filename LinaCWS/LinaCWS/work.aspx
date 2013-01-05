<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="work.aspx.cs" Inherits="LinaCWS.work" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>考勤管理</title>
    <link href="Sources/jquery_easyui/themes/metro/easyui.css" rel="stylesheet" />
    <link href="Sources/jquery_easyui/themes/icon.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="Sources/jquery_easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="Sources/jquery_easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="Sources/jquery_easyui/easyui_ex.js"></script>
    <script src="Scripts/jquery_easyUI_dialog_form.js" type="text/javascript"></script>
</head>
<body>
    <div id="dd">

    </div>
    <script type="text/javascript">
        $(function () {
             
            var curData = new Date();
            var $dd = $("#dd").datagrid({
                // title: "全部文章",
                rownumbers: true,
                url: "workList.ashx",
                queryParams: { y: curData.getFullYear(), m: curData.getMonth()+1 },
                iconCls: "icon-wrench",
                pagination: true,
                pageSize: 15,
                pageList: [15, 20, 25, 30, 35, 40],
                fit: true,
                fitColumns: true,
                nowarp: false,//设置是否折行，false为折行
                border: true,//设置是否显示border
                idField: 'wkID',//设置跨页选择的标识栏
                sortName: "wkEntryTime",
                SortOrder: "asc",
                columns: [[
                    {
                        title: "员工编号",
                        field: "wkID",
                        width: 80,
                        sortable: true,
                        checkbox: true
                    }, {
                        title: "姓名",
                        field: "wkName", sortable: true,
                        width: 180,
                        align: 'center'
                    }, {
                        title: "职位",
                        field: "wkPosition", //sortable: true,
                        width: 180,
                        align: 'center'
                    }, {
                        title: "考勤时间",
                        field: "wkYear", //sortable: true,
                        width: 180,
                        align: 'center',
                        formatter: function (value, rowData, rowIndex) {
                            
                            var html = value + '年' + rowData.wkMonth + "月";
                            return html;
                        }
                    }, {
                        title: "加班",
                        field: "owkTimes", //sortable: true,
                        width: 180,
                        align: 'center'
                    }, {
                        title: "缺勤",
                        field: "uwkTimes", //sortable: true,
                        width: 180,
                        align: 'center'

                    }, {
                        title: "操作",
                        field: "_wkID",
                        width: 220,
                        align: 'center',
                        formatter: function (value, rowData, rowIndex) {
                            var did = rowData.wkID;
                            var html = '<a href="javascript:;" wName="' + rowData.wkName + '" did="' + did +
                                '" wkYear="' + rowData.wkYear + '" wkMonth="' + rowData.wkMonth + '" class="art_edit_btn">修改</a>';
                            return html;
                        }
                    }
                ]],//
                toolbar: ["-", {
                        text: "更改考勤年月",
                        iconCls: "icon-search",
                        handler: function () {
                            $("#dialog_YM").dialog("open");
                        }
                    },
                    "-"
                ],
                onClickRow: function (rowIndex, rowData) {

                },
                onDblClickRow: function (rowIndex, rowData) {

                },
                onLoadSuccess: function (data) {
                    //数据加载完成后，格式化数据单元格内的按钮
                    $(".art_view_btn").linkbutton({ plain: true, iconCls: 'icon-page' });
                    $(".art_edit_btn").linkbutton({ plain: true, iconCls: 'icon-pencil' });
                    //数据单元格内的，按钮绑定事件
                    //修改
                    $(".art_edit_btn").click(function () {
                        var did = $(this).attr("did");
                        var wy = $(this).attr('wkYear');
                        var wm = $(this).attr("wkMonth");
                        var wName = $(this).attr("wName");
                        $.sr_edit_dialog("workEdit.aspx",
                            { title: "员工 " + wName+" " +wy+"年 "+wm+ "月 份考勤", form: "editForm", did: did, wy: wy, wm: wm },
                            function (data) {
                                data = $.parseJSON(data);
                                if (data.status === 1) {
                                    $dd.datagrid("reload");
                                    console.info($dd);
                                    $.messager.show({ title: "提示", msg: data.msg, timeout: 2000 });
                                } else {
                                    $.messager.alert("错误：", data.msg, "error");
                                }
                        });
                    });

                }
            });
            $("#dialog_YM").dialog({
                title: "选择考勤年月",
                closed: true,
                width: 240,
                modal: true
            });
            $('input[name="Y_text"]').val(curData.getFullYear());
            $('.pre_year').click(function () {
                $('input[name="Y_text"]').val(parseInt($('input[name="Y_text"]').val()) - 1);
            });
            $('.next_year').click(function () {
                $('input[name="Y_text"]').val(parseInt($('input[name="Y_text"]').val()) + 1);
                if (parseInt($('input[name="Y_text"]').val()) > curData.getFullYear()) {
                    $('input[name="Y_text"]').val(curData.getFullYear());
                }
            });
            $(".re_M").click(function () {
                var sM = parseInt($(this).text());
                var sY = parseInt($('input[name="Y_text"]').val());
                $("#dd").datagrid("reload", { y: sY, m: sM });
                $("#dialog_YM").dialog("close");
            });
        });
    </script>
    <div id="dialog_YM">
        <table style="width:220px;margin:3px auto;text-align:center;">
            <tr><td colspan="4" style="text-align:center;"><a class="easyui-linkbutton pre_year"  data-options="plain:true">上一年</a> <input type="text" name="Y_text" value="" style="width:50px;"/> <a class="easyui-linkbutton next_year" data-options="plain:true">下一年</a></td></tr>
            <tr><td><a class="easyui-linkbutton re_M" data-options="plain:true">1</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">2</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">3</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">4</a></td></tr>
            <tr><td><a class="easyui-linkbutton re_M" data-options="plain:true">5</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">6</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">7</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">8</a></td></tr>
            <tr><td><a class="easyui-linkbutton re_M" data-options="plain:true">9</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">10</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">11</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">12</a></td></tr>            
        </table>
    </div>
</body>
</html>
