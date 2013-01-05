<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userEdit.aspx.cs" Inherits="LinaCWS.userEidt" %>

    <form id="editForm" runat="server">
    <table style="line-height:28px ;text-align:center;">
        <tr><td>姓名</td><td>
            <asp:textbox runat="server" ID="adminName"></asp:textbox>
        </td></tr>
        <tr><td>帐号</td><td>
            <asp:textbox runat="server" ID="adminAccount"></asp:textbox>
        </td></tr>
        <tr><td>初始化密码</td><td>
            <label><input id="resetPwd1" name="resetPwd" value="1" type="radio" />是 </label><label><input id="resetPwd2" name="resetPwd" checked="checked" type="radio"  value="0" />否</label>
        </td></tr>
        <tr id="pwd_wp"><td>密码</td><td>
            <asp:textbox runat="server" ID="adminPWD"  Text="12345"></asp:textbox>
        </td></tr>
        <tr id="pwd_cp_wp"><td>确认密码</td><td>
            <asp:textbox runat="server" ID="adminCpPwd"  Text="12345"></asp:textbox>
        </td></tr>
        <asp:hiddenfield runat="server" ID="adminID"></asp:hiddenfield>
    </table>
    </form> 
<script type="text/javascript">
    $(function () {
        $("#adminName,#adminAccount,#adminPWD").validatebox({
            required: true
        });
        $("#adminCpPwd").validatebox({
            required: true,
            validType: "eqPassword[$('#adminPWD')]"
        });
        $("#pwd_wp,#pwd_cp_wp").hide();
        $("#adminPWD,#adminCpPwd").val("12345")
        $("[name='resetPwd']").click(function () {
            if ($(":radio[name='resetPwd'][checked]").val()==1) {
                $("#pwd_wp,#pwd_cp_wp").show();
            } else {
                $("#pwd_wp,#pwd_cp_wp").hide();
            }
        });
    });
</script>