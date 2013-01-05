<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pay.aspx.cs" Inherits="LinaCWS.pay" %>
 
    <form id="editForm" runat="server">
    <table style="line-height:30px; margin:20px; text-align:center;">
        <tr><td width="100">发放</td><td><asp:textbox ID="wlpReal" runat="server"></asp:textbox>元</td></tr>
        <asp:hiddenfield runat="server" ID="wkpID"></asp:hiddenfield>
    </table>
    </form> 
<script type="text/javascript">
    $(function () {
        $("#wlpReal").numberbox({
            min: 0, required: true,
            precision: 2
        });
    });
</script>
