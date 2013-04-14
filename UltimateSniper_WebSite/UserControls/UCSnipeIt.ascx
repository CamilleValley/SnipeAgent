<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCSnipeIt.ascx.cs" Inherits="UltimateSniper_WebSite.UserControls.UCSnipeIt" %>
<%@ Register src="UCSnipeDetails.ascx" tagname="SnipeDetails" tagprefix="uc1" %>

<div>
    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lang, UCSnipeIt_ItemID%>"></asp:Label>
    <asp:TextBox ID="txtBoxItemID" runat="server" ></asp:TextBox>
    <asp:Button ID="btnLoadDetails" runat="server" Text="<%$ Resources:lang, UCSnipeIt_btnGetInfo%>" onclick="btnLoadItem_Click" CssClass="BoutonStandard"/>
    <br /><br />
    <asp:Panel ID="pnlSnipeDetails" runat="server">
        <div style="position: relative; border: solid thin gray; left:-250px; width:700px; padding: 10px 10px 10px 10px; z-index:10; background-color: White;">
            <%=Resources.lang.UCSnipeIt_NewSnipe%><br /><br />
            <uc1:SnipeDetails ID="SnipeDetails" runat="server" />
        </div>
    </asp:Panel>
</div>