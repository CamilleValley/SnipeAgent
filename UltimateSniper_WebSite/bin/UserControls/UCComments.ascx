<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCComments.ascx.cs" Inherits="UltimateSniper_WebSite.Comments" %>
<asp:Panel ID="PanelComments" runat="server" CssClass="Comments">
    <div id="DivComments" class="DivComments">
    <div class="CoinHautGauche"></div>
    <div class="CoinHautDroit"></div>
    <div class="CoinBasGauche"></div>
    <div class="CoinBasDroit"></div>
    <asp:BulletedList ID="BulletListComments" runat="server" CssClass="ListComments"></asp:BulletedList>
    </div>
</asp:Panel>