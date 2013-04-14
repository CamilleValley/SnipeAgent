<%@ Page Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="WhatsSnipe.aspx.cs" Inherits="UltimateSniper_WebSite.MasterPages.WhatsSnipe" Title="Snipe Agent" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMainData" runat="server">
    <asp:Panel ID="PanelTitle" runat="server" CssClass="Title"><%=Resources.lang.WhatsSnipe_Title%></asp:Panel>
    <div id="DivActive" class="DivActive">
        <p style="font-size: 25px;">
        <asp:Image ID="imgTarget" runat="server" ImageUrl="Medias/Images/target.gif" height="40px"/>
        <%=Resources.lang.WhatsSnipe_Snipes%>
        </p>
        <ul>
        <li><%=Resources.lang.WhatsSnipe_BiddingWar%></li>
        <li><%=Resources.lang.WhatsSnipe_SecretBid%></li>
        <li><%=Resources.lang.WhatsSnipe_Change%></li>
        </ul>
        
        <p style="font-size: 25px;">
        <asp:Image ID="imgOnline" runat="server" ImageUrl="Medias/Images/online.gif" height="40px"/>
        <%=Resources.lang.WhatsSnipe_Online%>
        </p> 
        <ul>
        <li><%=Resources.lang.WhatsSnipe_NoPC%></li>
        <li><%=Resources.lang.WhatsSnipe_NoDL%></li>
        </ul>
        
        <p style="font-size: 25px;">
        <asp:Image ID="imgFeatures" runat="server" ImageUrl="Medias/Images/features.gif" height="30px"/>
        <%=Resources.lang.WhatsSnipe_ExtraFeatures%>
        </p> 
        <ul>
        <li><%=Resources.lang.WhatsSnipe_Group%></li>
        <li><%=Resources.lang.WhatsSnipe_AutoRetry%></li>
        </ul>
    
        <p style="font-size: 25px;">
        <asp:Image ID="imgPolicies" runat="server" ImageUrl="Medias/Images/policies.gif" height="30px"/>
        <%=Resources.lang.WhatsSnipe_Policies%>
        </p> 
        <ul>
        <li><%=Resources.lang.WhatsSnipe_RandomDelay%></li>
        <li><%=Resources.lang.WhatsSnipe_RandomCredentials%></li>
        </ul>
    </div>
</asp:Content>
