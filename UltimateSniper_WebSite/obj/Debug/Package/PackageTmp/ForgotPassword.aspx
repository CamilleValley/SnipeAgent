<%@ Page Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="UltimateSniper_WebSite.MasterPages.ForgotPassword" Title="Snipe Agent" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMainData" runat="server">
    <asp:Panel ID="PanelTitle" runat="server" CssClass="Title"><%=Resources.lang.ForgotPassword_Title%></asp:Panel>
    <div id="DivActive" class="DivActive">
    <%=Resources.lang.ForgotPassword_Explaination%><br /><br /><br />
    <center>
    <%=Resources.lang.ForgotPassword_EmailOrName%>&nbsp;<asp:TextBox ID="txtBoxEmailOrName" runat="server" Width="180px" MaxLength="200"></asp:TextBox>
    <asp:Button ID="btnSendPassword" runat="server" 
            Text="<%$ Resources:lang, ForgotPassword_btnSendPassword%>" 
            onclick="btnSendPassword_Click" />
    </center>
    </div>
</asp:Content>
