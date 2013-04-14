<%@ Page Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="SnipeManagement.aspx.cs" Inherits="UltimateSniper_WebSite.SnipeManagement" Title="Snipe Agent" %>
<%@ Register src="~/UserControls/UCSnipeDetails.ascx" tagname="SnipeDetails" tagprefix="uc2" %>
<%@ Register src="~/UserControls/UCSnipeIt.ascx" tagname="SnipeIt" tagprefix="uc1" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMainData" runat="server">
    <asp:Panel ID="SnipeTitle" runat="server" CssClass="Title"><%=Resources.lang.SnipeManagement_Title%></asp:Panel>
    <div id="DivActive" class="DivActive">
        <%=Resources.lang.SnipeManagement_Explainations%>
        <br /><br />
        
        <!--
        <uc1:SnipeIt ID="SnipeIt1" runat="server" />
        <%=Resources.lang.SnipeManagement_Details%>
        <br /><br />
        -->
        
        <%=Resources.lang.SnipeManagement_ListTypeSelection%>
        <asp:DropDownList ID="drpDwnListType" runat="server" 
        onselectedindexchanged="drpDwnListType_SelectedIndexChanged" AutoPostBack="True">
        <asp:ListItem Value="Recent" Text="<%$ Resources:lang, SnipeManagement_Recent%>"></asp:ListItem>
        <asp:ListItem Value="Won" Text="<%$ Resources:lang, SnipeManagement_Won%>"></asp:ListItem>
        <asp:ListItem Value="Open" Text="<%$ Resources:lang, SnipeManagement_Open%>"></asp:ListItem>
        <asp:ListItem Value="All" Text="<%$ Resources:lang, SnipeManagement_All%>"></asp:ListItem>
        </asp:DropDownList>
        
        <asp:repeater id="RepeaterSnipeList" runat="server" OnItemDataBound="DataBound">
        <HeaderTemplate>
        </HeaderTemplate>
        <ItemTemplate>
        <uc2:SnipeDetails id="SnipeDetails" runat="server"></uc2:SnipeDetails>
        </ItemTemplate> 
        </asp:repeater>
        
        <asp:Panel ID="PanelNoSnipe" runat="server" ForeColor="Red">
        <br /><br />
        <%=Resources.lang.SnipeManagement_NoSnipe%>
        </asp:Panel>
        
        <uc2:SnipeDetails ID="SnipeDetails" runat="server" />
    </div>  
</asp:Content>
