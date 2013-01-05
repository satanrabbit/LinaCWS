<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wagesCount.aspx.cs" Inherits="LinaCWS.wagesCount" %>
 
    <form id="editForm" runat="server">
   <table style="line-height:28px;">
       <tr><td>补助金额</td><td><asp:textbox Id="wlpSubsidy" runat="server" ></asp:textbox>元</td></tr>
       <tr><td colspan="2">提示:<br />补助为该员工的全部补贴，包含话费补贴、差旅补贴、住宿补贴等</td></tr>       
   </table>
        <asp:hiddenfield ID="wkID" runat="server"></asp:hiddenfield>
        <asp:hiddenfield ID="wkYear" runat="server"></asp:hiddenfield>
        <asp:hiddenfield ID="wkMonth" runat="server"></asp:hiddenfield>
    </form> 
<script type="text/javascript">
    $(function () {
        $("#wlpSubsidy").numberbox({
            min: 0, required: true,
            precision: 2
        });
    });
</script>
