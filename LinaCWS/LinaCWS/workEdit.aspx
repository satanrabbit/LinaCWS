<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="workEdit.aspx.cs" Inherits="LinaCWS.workEdit" %>
    <div style="padding:20px;">
        <form id="editForm" runat="server">
            <table style="text-align:center; line-height:30px;"> 
                <tr><td>加班：</td><td>
                    <asp:textbox ID="owkTimes" runat="server"></asp:textbox>天
                    <asp:hiddenfield ID="wkID" runat="server"></asp:hiddenfield>
                    <asp:hiddenfield ID="wkY" runat="server"></asp:hiddenfield>
                    <asp:hiddenfield ID="wkM" runat="server"></asp:hiddenfield>

                </td></tr>              
                <tr><td>缺勤：</td><td>
                    <asp:textbox ID="uwkTimes" runat="server" ></asp:textbox>天
                </td></tr>
                
            </table>
        </form> 
        <script type="text/javascript">
            $("#uwkTimes,#owkTimes").numberbox({
                min:0,max:30,
                precision: 2,
                required:true
            });
        </script>
    </div>