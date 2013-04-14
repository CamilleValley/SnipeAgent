<%@ Page Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="UltimateSniper_WebSite.Home" Title="Snipe Agent" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMainData" runat="server">
    <asp:Panel ID="PanelTitle" runat="server" CssClass="Title"><%=Resources.lang.Home_Title%></asp:Panel>
    <div id="DivActive" class="DivActive">
        <%=Resources.lang.Home_Explanations%><br /><br />
        <div class="DivHomePoint" style="background-image: url('Medias/Images/home_freebackground.gif');">
        <%=Resources.lang.Home_Free%>
        </div>
        <div class="DivHomePoint" style="background-image: url('../Medias/Images/home_securebackground.gif');">
        <%=Resources.lang.Home_Security%>
        </div>
        <div class="DivHomePoint" style="background-image: url('../Medias/Images/home_mobilebackground.gif');">
        <%=Resources.lang.Home_Mobility%>
        </div>
    </div>
</asp:Content>
