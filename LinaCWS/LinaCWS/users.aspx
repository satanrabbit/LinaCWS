<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="users.aspx.cs" Inherits="LinaCWS.users" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
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
            var $dd = $("#dd").datagrid({
                // title: "全部文章",
                rownumbers: true,
                url: "userList.ashx",
                queryParams: { clm: "" },
                iconCls: "icon-wrench",
                pagination: true,
                pageSize: 15,
                pageList: [15, 20, 25, 30, 35, 40],
                fit: true,
                fitColumns: true,
                nowarp: false,//设置是否折行，false为折行
                border: true,//设置是否显示border
                idField: 'adminID',//设置跨页选择的标识栏
                sortName: "adminID",
                SortOrder: "asc",
                columns: [[
                    {
                        title: "编号",
                        field: "adminID",
                        width: 80,
                        sortable: true,
                        checkbox: true
                    }, {
                        title: "姓名",
                        field: "adminName", sortable: true,
                        width: 180,
                        align: 'center'
                    }, {
                        title: "帐号",
                        field: "adminAccount", //sortable: true,
                        width: 180,
                        align: 'center'
                    }, {
                        title: "级别",
                        field: "adminClass", //sortable: true,
                        width: 180,
                        align: 'center',
                        formatter: function (value, rowData, rowIndex) {
                            var html = "";
                            if (value == 0) {
                                html = "超管";
                            } else {
                                html = "普管";
                            }                            
                            return html;
                        }
                    
                    }, {
                        title: "操作",
                        field: "_wkID",
                        width: 220,
                        align: 'center',
                        formatter: function (value, rowData, rowIndex) {
                            var did = rowData.adminID;
                            var html = '<a href="javascript:;" did=' + did +
                                ' class="art_edit_btn">修改</a><a href="javascript:;" did=' + did +
                                ' class="art_del_btn" >删除</a>';
                            return html;
                        }
                    }
                ]],//
                toolbar: [
                    {
                        text: "增加",
                        iconCls: "icon-add",
                        handler: function () {
                            $.sr_edit_dialog("userEdit.aspx", { title: "添加管理员", form: "editForm" }, function (data) {
                                data = $.parseJSON(data);
                                if (data.status === 1) {
                                    $dd.datagrid("reload");
                                    $.messager.show({ title: "提示", msg: data.msg, timeout: 2000 });
                                } else {
                                    $.messager.alert("错误：", data.msg, "error");
                                }
                            });

                        }
                    }, "-", {
                        text: "删除",
                        iconCls: "icon-delete",
                        handler: function () {
                            var rows = $dd.datagrid("getSelections");
                            if (rows.length <= 0) {
                                $.messager.show({ title: "提示", msg: "您未选择要删除数据！", timeout: 2000 });
                            }
                            $.messager.confirm("提示", "删除操作将不可逆！<br />请确认！", function (b) {
                                if (b) {
                                    var dids = [];
                                    $.each(rows, function (i, item) {
                                        dids.push(item.adminID);
                                    });
                                    dids = dids.join(",");
                                    $.delete_data("userDelete.ashx", { did: dids });
                                }
                            });
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
                    $(".art_del_btn").linkbutton({ plain: true, iconCls: 'icon-delete' });
                    //数据单元格内的，按钮绑定事件
                    //修改
                    $(".art_edit_btn").click(function () {
                        var did = $(this).attr("did");
                        $.sr_edit_dialog("userEdit.aspx", { title: "修改管理员信息", form: "editForm", did: did }, function (data) {
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

                    //删除
                    $(".art_del_btn").click(function () {
                        var did = $(this).attr("did");
                        $.messager.confirm("提示", "删除后数据将不可找回!<br />请确认！", function (b) {
                            if (b) {                              
                                $.delete_data("userDelete.ashx", { did: did });
                            }
                        });

                    });
                }
            });
        });
    </script>
</body>
</html>
