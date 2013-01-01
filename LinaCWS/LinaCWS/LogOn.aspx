<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogOn.aspx.cs" Inherits="LinaCWS.LogOn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>登录</title>
    <link href="Style/base.css" rel="stylesheet" />
    <link href="Sources/jquery_easyui/themes/metro/easyui.css" rel="stylesheet" />
    <link href="Sources/jquery_easyui/themes/icon.css" rel="stylesheet" />


    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="Sources/jquery_easyui/jquery.easyui.min.js"></script>
    <script src="Sources/jquery_easyui/locale/easyui-lang-zh_CN.js"></script>
    <script src="Sources/jquery_easyui/easyui_ex.js"></script>
</head>
<body style="  background-image:url('/Sources/file/202hfy166.jpg'); background-repeat:no-repeat; background-position-x:center;">
    
    <div id="dd" style="padding:20px;line-height:2em;"><form id="form1" runat="server">
        <table style="width: 100%;" border="0">
            <tr>
                <td style="text-align:right;">账号：</td>
                <td><input type="text" name="account" class="easyui-validatebox" data-options=" required: true" id="account"/></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align:right;">密码：</td>
                <td><input type="password" name="pwd" id="pwd"/></td>
                <td>&nbsp;</td>
            </tr>
            
        </table> </form>
    </div>
   
    <script type="text/javascript">
        $(function () {
            $("#dd").dialog({
                title: "工资管理系统登录",
                width: 400,closable:false,
                buttons: [{
                    text: "登录",
                    iconCls: "icon-ok",
                    handler: function () {
                        fm = $("#form1");
                        fm.form("submit", {
                            url: fm.attr("action"),
                            onSubmit: function () {
                                //进行表单验证
                                //如果返回false阻止提交  
                                return fm.form("validate");
                            },
                            success: function (data) {
                                data = $.parseJSON(data);
                                $.messager.show({ title: "提示", msg: data.name, timeout: 2000 });
                            }
                        });
                    }
                }]
            });
            
        });
    </script>
</body>
</html>
