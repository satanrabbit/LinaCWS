<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rate.aspx.cs" Inherits="LinaCWS.rate" %>

<div style="padding:10px ;">
    <table style="line-height:20px;text-align:center;">
        <tr><td colspan="2">工资统计系数 <a id="edit_rate_btn">修改</a></td></tr>
        <tr><td>加班补贴</td><td><asp:label id="rOverWork" runat="server" text=""></asp:label>元/次</td></tr>
        <tr><td>养老险</td><td><asp:label id="rYL"  runat="server" text=""></asp:label>%</td></tr>
        <tr><td>生育险</td><td><asp:label id="rSYu"  runat="server" text=""></asp:label>%</td></tr>
        <tr><td>医疗险</td><td><asp:label id="rYiLiao"  runat="server" text=""></asp:label>%</td></tr>
        <tr><td>失业险</td><td><asp:label id="rSYe"  runat="server" text=""></asp:label>%</td></tr>
        <tr><td>工伤险</td><td><asp:label id="rGS"  runat="server" text=""></asp:label>%</td></tr>
        <tr><td>公积金</td><td><asp:label id="rZF"  runat="server" text=""></asp:label>%</td></tr>
        <tr><td>个税起征点</td><td><asp:label id="rTaxPoint"  runat="server" text=""></asp:label>元/月</td></tr>
    </table>
    <table style="line-height:20px;text-align:center;">
        <tr><td colspan="3">个税税率 <a id="edit_tax_btn">修改</a></td></tr> 
        <tr><td>税级点</td><td>税率</td><td>速算扣除数</td></tr>
        
         <asp:Repeater id="RepeatRt" runat="server"  >
            <ItemTemplate>
                <tr><td><%# Eval("rtPoint") %></td><td><%# Eval("rt") %></td><td><%# Eval("rtQuick") %></td></tr>
            </ItemTemplate>
        </asp:Repeater>
            
        
    </table>
    <script type="text/javascript">
        $("#edit_rate_btn,#edit_tax_btn").linkbutton({
            iconCls:'icon-page_edit'
        });
        $("#edit_rate_btn").click(function () {
           
        });
    </script>
</div>