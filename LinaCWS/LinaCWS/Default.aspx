<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LinaCWS._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>企业工资管理系统</title>
    <link href="Sources/jquery_easyui/themes/metro/easyui.css" rel="stylesheet" />
    <link href="Sources/jquery_easyui/themes/icon.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="Sources/jquery_easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="Sources/jquery_easyui/easyui_ex.js"></script>
    <script src="Scripts/jquery_easyUI_dialog_form.js" type="text/javascript"></script>
</head>
    <body class="easyui-layout">
		<div region="north" style="height:60px;">
            <a class="easyui-linkbutton" data-options="iconCls:'icon-door_out'" href="LogOut.aspx">退出</a>
		</div>
		<div region="south" style="height:60px;"></div>
		<div region="east" iconCls="icon-reload" title="关于" split="true" collapsed="true" style="width:200px;">
		</div>

		<div region="west" split="true" title="管理面板" style="width:200px; padding:20px 10px; text-align:center;">
            <a  href="worker.aspx" class="easyui-linkbutton menuLink" data-options="iconCls:'icon-page_white_gear'">员工管理</a>
            <br />
            <br />
            <a   href="work.aspx" class="easyui-linkbutton menuLink" data-options="iconCls:'icon-page_white_gear'">考勤管理</a>
            <br /><br />
            <a   href="wages.aspx" class="easyui-linkbutton menuLink" data-options="iconCls:'icon-page_white_gear'">工资统计</a>
            <br /><br />
            <%if (Convert.ToInt32(Session["adminClass"]) == 0)
              { %>
            <a   href="users.aspx" class="easyui-linkbutton menuLink" data-options="iconCls:'icon-page_white_gear'">管理员管理</a>
            <br />
            <%} %>
            <br />

		</div>
		<div region="center" style="padding:5px;background:#fff;">
             <div id="centerTabs" >
              <div title="首页">
                <div id="system_intro">管理系统</div>
              </div>
            </div>
		</div>
	</body>

     <script type="text/javascript">
         $(function () {
             var centerTabs = $('#centerTabs').tabs({
                 fit: true,
                 border: false
             });
             $.extend({
                 addTab: function (node, centerTabs) {
                     if (centerTabs.tabs('exists', node.text)) {
                         centerTabs.tabs('select', node.text);
                     }
                     else {
                         if (node.attributes.url && node.attributes.url.length > 0) {
                             $.messager.progress({
                                 text: '页面加载中....',
                                 interval: 100
                             });
                             centerTabs.tabs('add', {
                                 title: node.text,
                                 closable: true,
                                 content: '<iframe src="' + node.attributes.url + '" frameborder="0" style="border:0;width:100%;height:99.4%;"></iframe>'
                             });
                         }
                         else {
                             centerTabs.tabs('add', {
                                 title: node.text,
                                 closable: true,
                                 herf: node.attributes.url
                             });
                         }
                     }
                 },
                 msg_progress_close: function () {
                     window.setTimeout(function () {
                         $.messager.progress('close');
                     }, 200);
                 }
             });
             $(".menuLink").click(function () {
                 var $this = $(this);
                 var nd = { text: $this.text(), attributes: { url: $this.attr("href") } };
                 $.addTab(nd, centerTabs);
                 return false;
             });
         });
    </script>

</html>
