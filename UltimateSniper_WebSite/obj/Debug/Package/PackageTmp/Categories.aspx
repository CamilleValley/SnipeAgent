<%@ Page Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="UltimateSniper_WebSite.MasterPages.Categories" Title="Snipe Agent" %>
<%@ Register src="~/UserControls/UCCategoryManagement.ascx" tagname="CategoryManagement" tagprefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderMainData" runat="server">
<asp:Panel runat="server" CssClass="Title"><%=Resources.lang.Categories_Title%></asp:Panel>
<div id="DivActive" class="DivActive">
<%=Resources.lang.Categories_Explainations%>
<uc1:CategoryManagement ID="CategoyHandler" runat="server" />
</div>
</asp:Content>

