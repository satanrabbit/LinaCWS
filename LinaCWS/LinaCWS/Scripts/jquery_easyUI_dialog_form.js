$.extend({
   
    /**
     *  基于jQuery EasyUi的dialog内表单加载插件
     * 
     *  @author SatanRabbit -- xqx -- 夏千祥
     *
     *  url：加载表单HTML代码的地址
     *  data：加载参数，title和form为关键字不可再用
     *  data.title-dialog的Titile，data.form ：加载进来的表单ID，其他为提交参数。
     */
    sr_edit_dialog: function (url, data, fn) {
        var u, d, f;
        if (arguments.length == 0) {
            $.messager.alert("错误", "错误:未填写发送请求地址！", "error");
        }
        else {

            if (arguments.length == 1) {
                u = url;
                d = {};
                f = function () { }
            }
            if (arguments.length == 2) {
                u = url;
                if ($.isFunction(data)) {
                    d = {};
                    f = data;
                } else {
                    d = data;
                    f = function () { }
                }
            }
            if (arguments.length == 3) {
                u = url;
                d = data;
                f = fn;
            }

            $(".sr_edit_dialog").dialog('destroy').remove();
            $("body").append('<div class="sr_edit_dialog"></div>');
            $.messager.progress();
            $('.sr_edit_dialog').load(u, d, function () {
                $.messager.progress("close");
                var dg = $('.sr_edit_dialog');
                var fm;
                if (d.title != null) {
                    dg.dialog({ title: d.title });
                } else {
                    dg.dialog({ title: "填写表单" });
                }

                if (d.maximized != null) {
                    dg.dialog({ maximized: d.maximized });
                } else {
                    dg.dialog({ maximized: false });
                }

                //表单
                if (d.form == null) {
                    fm = $('.sr_edit_dialog').find("form:first");
                } else {
                    fm = $('.sr_edit_dialog').find("form#" + d.form);
                }
                if (fm.length == 0) {
                    $.messager.alert("错误", "错误:<br /> 表单名-" + d.form + "不存在！", "error");
                    //$(".sr_edit_dialog").dialog('destroy').remove();
                } else {
                    dg.dialog({
                        iconCls: 'icon-page_edit', modal: true,maximizable :true,  resizable: true,
                        top: 10,
                         
                        buttons: [{
                            text: "保存",
                            iconCls: 'icon-disk',modal:true,
                            handler: function () {
                                //提交表单
                                fm.form('submit', {
                                    url: fm.attr("action"),
                                    onSubmit: function () {
                                        return fm.form("validate")
                                    },
                                    success: function (data) {
                                        dg.dialog('destroy');
                                        f(data);
                                    }
                                });
                            }
                        }, {
                            text: "取消",
                            iconCls: 'icon-cross',
                            handler: function () {
                                dg.dialog('destroy');
                            }
                        }]
                    });
                }
            });
        }
    }
});
