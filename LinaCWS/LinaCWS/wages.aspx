<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wages.aspx.cs" Inherits="LinaCWS.wages" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>工资管理</title>
    <link href="Sources/jquery_easyui/themes/metro/easyui.css" rel="stylesheet" />
    <link href="Sources/jquery_easyui/themes/icon.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="Sources/jquery_easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="Sources/jquery_easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="Sources/jquery_easyui/easyui_ex.js"></script>
    <script src="Scripts/jquery_easyUI_dialog_form.js" type="text/javascript"></script>
</head>
<body class="easyui-layout" >     
		<div region="east" data-options="href:'rate.aspx'"  title="工资系数与税率" split="true" collapsed="true" style="width:200px;">
           
		</div>		 
		<div region="center" style="padding:2px;background:#fff;">
            <div id="dd">
            </div>
            
            <div id="dialog_YM">
                <table style="width:220px;margin:3px auto;text-align:center;">
                    <tr><td colspan="4" style="text-align:center;"><a class="easyui-linkbutton pre_year"  data-options="plain:true">上一年</a> <input type="text" name="Y_text" value="" style="width:50px;"/> <a class="easyui-linkbutton next_year" data-options="plain:true">下一年</a></td></tr>
                    <tr><td><a class="easyui-linkbutton re_M" data-options="plain:true">1</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">2</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">3</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">4</a></td></tr>
                    <tr><td><a class="easyui-linkbutton re_M" data-options="plain:true">5</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">6</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">7</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">8</a></td></tr>
                    <tr><td><a class="easyui-linkbutton re_M" data-options="plain:true">9</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">10</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">11</a></td><td><a class="easyui-linkbutton re_M" data-options="plain:true">12</a></td></tr>            
                </table>
            </div>
            <div id="fordialog"></div>
		</div>
    <script type="text/javascript">
        $(function () {
            $.extend({
                delete_data: function (url, did) {
                    $.post(url, did, function (data) {
                        data = $.parseJSON(data);
                        if (data.status === 1) {
                            $.messager.show({ titie: "提示", msg: data.msg, timeout: 2000 });
                            $dd.datagrid("reload");
                        } else {
                            $.messager.alert("错误", data.msg, "error");
                        }
                    });
                }
            });
            var curData = new Date();
            var isopen = false;
            var $dd = $("#dd").datagrid({
                // title: "全部文章",
                //rownumbers: true,
                url: "wagesList.ashx",
                queryParams: { y: curData.getFullYear(), m: curData.getMonth() + 1 },
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
                        title: "编号",
                        field: "wkID",
                        width: 60,
                        sortable: true, align: 'center'
                        //checkbox: true
                    }, {
                        title: "姓名",
                        field: "wkName", sortable: true,
                        width: 80,
                        align: 'center'
                    }, {
                        title: "职位",
                        field: "wkPosition", //sortable: true,
                        width: 80,
                        align: 'center'
                    }, {
                        title: "时间",
                        field: "wkYear", //sortable: true,
                        width: 60,
                        align: 'center',
                        formatter: function (value, rowData, rowIndex) {

                            var html = value + '/' + rowData.wkMonth ;
                            return html;
                        }
                    }, {
                        title: "基础工资",
                        field: "wkSalary", //sortable: true,
                        width: 80,
                        align: 'center'
                    }, {
                        title: "统计工资",
                        field: "wkpAll", //sortable: true,
                        width: 80,
                        align: 'center',
                        formatter: function (value, rowData, rowIndex) {
                            var html = "";
                            if (value == "") {
                                html = "未统计";
                            } else {
                                var total = value + rowData.wlpGS + rowData.wlpSY + rowData.wlpYLiao + rowData.wlpSYe
                                html = value;
                            }
                            return html;
                        }
                    }, {
                        title: "五险一金",
                        field: "wlpYL", //sortable: true,
                        width: 150,
                        align: 'center',
                        formatter: function (value, rowData, rowIndex) {                            
                            var html = "";
                            if (value == "") {
                                html = "未统计";
                            } else {
                                var total = value + rowData.wlpGS + rowData.wlpSY + rowData.wlpYLiao + rowData.wlpSYe + rowData.wlpZF
                                html = '共：'+ total + '  <a href="javascript:;" wlpyl="' + rowData.wlpYL +
                                    '" wlpgs="' + rowData.wlpGS +
                                    '" wlpsy="' + rowData.wlpSY +
                                    '" wlpyliao="' + rowData.wlpYLiao +
                                    '" wlpsye="' + rowData.wlpSYe +
                                    '" wlpzf="' + rowData.wlpZF +
                                    '" class="view_insure_btn">详细</a>';
                            }
                            return html;
                        }
                    }, {
                        title: "所得税",
                        field: "wlpTax", //sortable: true,
                        width: 80,
                        align: 'center'
                    }, {
                        title: "补贴",
                        field: "wlpSubsidy", //sortable: true,
                        width: 80,
                        align: 'center'
                    }, {
                        title: "应发",
                        field: "wlpShould", //sortable: true,
                        width: 80,
                        align: 'center'
                    }, {
                        title: "实发",
                        field: "wlpReal", //sortable: true,
                        width: 80,
                        align: 'center'
                    }, {
                        title: "经办人",
                        field: "adminName", //sortable: true,
                        width: 80,
                        align: 'center'
                    }, {
                        title: "发放时间",
                        field: "wlpTime", //sortable: true,
                        width: 80,
                        align: 'center'

                    }, {
                        title: "操作",
                        field: "_wkID",
                        width: 130,
                        align: 'center',
                        formatter: function (value, rowData, rowIndex) {
                            var did = rowData.wkID;
                            var html = '<a href="javascript:;" wName="' + rowData.wkName + '" did="' + did +
                                '" wkYear="' + rowData.wkYear + '" wkMonth="' + rowData.wkMonth + '" class="art_count_btn">统计</a>'+

                                '<a href="javascript:;" wName="' + rowData.wkName + '" did="' + rowData.wkpID +
                                '" wkYear="' + rowData.wkYear + '" wkMonth="' + rowData.wkMonth + '" class="art_pay_btn">发放</a>';
                            return html;
                        }
                    }
                ]],//
                toolbar: ["-", {
                    text: "查询年月",
                    iconCls: "icon-search",
                    handler: function () {
                        $("#dialog_YM").dialog("open");
                    }
                },
                "-", {
                    text: "工资系数与税率",
                    iconCls: "icon-sum",
                    handler: function () {
                        if (isopen) {
                            isopen = false;
                            $('body').layout('collapse', 'east');
                        } else {
                            isopen = true;
                            $('body').layout('expand', 'east');
                        }
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
                    $(".art_count_btn").linkbutton({ plain: true, iconCls: 'icon-calculator' });
                    $(".art_pay_btn").linkbutton({ plain: true, iconCls: 'icon-drive_go' });
                    $(".view_insure_btn").linkbutton({ plain: true, iconCls: 'icon-page' });
                    //查看五险一金详细
                    $(".view_insure_btn").click(function () {
                        $this=$(this);
                        $(".view_insure_dialog").dialog("destroy");
                        $("#fordialog").append('<div class="view_insure_dialog" >' +
                            '<table style="text-algin:center; line-height:26px; margin:20px;">' +
                            '<tr><td>养老保险：</td><td>' + $this.attr("wlpyl") + '</td></tr>' +
                            '<tr><td>生育险：</td><td>' + $this.attr("wlpsy") + '</td></tr>' +
                            '<tr><td>医疗保险：</td><td>' + $this.attr("wlpyliao") + '</td></tr>' +
                            '<tr><td>失业保险：</td><td>' + $this.attr("wlpsye") + '</td></tr>' +
                            '<tr><td>工伤保险：</td><td>' + $this.attr("wlpgs") + '</td></tr>' +
                            '<tr><td>住房公积金：</td><td>' + $this.attr("wlpzf") + '</td></tr>' +
                            '</table>' +
                            '</div>')
                        $(".view_insure_dialog").dialog({
                                title: "五险一金详细",
                                modal: true
                            });
                    });
                    //数据单元格内的，按钮绑定事件
                    //统计
                    $(".art_count_btn").click(function () {
                        var did = $(this).attr("did");
                        var wy = $(this).attr('wkYear');
                        var wm = $(this).attr("wkMonth");
                        var wName = $(this).attr("wName");
                        $.sr_edit_dialog("wagesCount.aspx",
                            { title: "统计员工 " + wName + " " + wy + "年 " + wm + "月 份工资", form: "editForm", did: did, wy: wy, wm: wm },
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
                    //发放
                    $(".art_pay_btn").click(function () {
                        var did = $(this).attr("did");
                        var wy = $(this).attr('wkYear');
                        var wm = $(this).attr("wkMonth");
                        var wName = $(this).attr("wName");
                        if (did == "" || did == null) {
                            $.messager.alert("未统计", "还没统计该员工的本月的工资！", "info")
                        } else {
                            $.sr_edit_dialog("pay.aspx",
                                { title: "发放员工 " + wName + " " + wy + "年 " + wm + "月 份工资", form: "editForm", did: did, wy: wy, wm: wm },
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
                        }
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
</body>
</html>
