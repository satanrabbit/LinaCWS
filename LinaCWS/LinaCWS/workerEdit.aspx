<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="workerEdit.aspx.cs" Inherits="LinaCWS.workerEdit" %>
<asp:panel runat="server" ID="FormPanel">
 <form ID="editForm" runat="server">
     <table>
         <tr><td>姓名</td><td><asp:textbox ID="wkName" runat="server"></asp:textbox></td></tr>
         <tr><td>职位</td><td><asp:textbox ID="wkPosition" runat="server"></asp:textbox></td></tr>
         <tr><td>起薪（元/月）</td><td><asp:textbox ID="wkSalary" runat="server"></asp:textbox></td></tr>
         <tr><td>入职时间</td><td><asp:textbox ID="wkEntryTime" runat="server"></asp:textbox></td></tr>
         
     </table>
     <asp:HiddenField ID="wkID"  runat="server"></asp:HiddenField>
     <script type="text/javascript">
         $(function () {
             $("#wkName").validatebox({
                 required:true
             });
             $("#wkSalary").numberbox({
                 required: true,
                 precision:2
             });
             $("#wkEntryTime").datebox({
                 required: true
             });
         });
     </script>
 </form>
 </asp:panel>
<asp:hyperlink ID="LogOnLink" runat="server" NavigateUrl="LogOn.aspx" Target="Windows">您未登录或登录超时！请登录！</asp:hyperlink>
