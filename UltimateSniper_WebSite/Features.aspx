<%@ Page Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Features.aspx.cs" Inherits="UltimateSniper_WebSite.MasterPages.Features" Title="Snipe Agent" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMainData" runat="server">
    <asp:Panel ID="PanelTitle" runat="server" CssClass="Title"><%=Resources.lang.Features_Title%></asp:Panel>
    <div id="DivActive" class="DivActive">
    <div class="DivFeatureFree">
        <div style="position: relative; left:130px; top:30px; height: 65px; width: 213px; font-size: 30px;"><%=Resources.lang.Features_Free%></div>
        <div style="position: relative; padding: 5px 10px 10px 10px;">
            <p>
            <asp:Image ID="imgTarget" runat="server" ImageUrl="Medias/Images/target.gif" height="30px"/>
            <%=Resources.lang.Features_SnipeSystem%>
            </p> 
            <%=Resources.lang.Features_SnipeSystemExp%>
            <p>
            <asp:Image ID="imgGroup" runat="server" ImageUrl="Medias/Images/group.gif" height="30px"/>
            <%=Resources.lang.Features_Group%>
            </p>
            <%=Resources.lang.Features_GroupExp%>
            <p>
            <asp:Image ID="imgEmail" runat="server" ImageUrl="Medias/Images/emailFeature.gif" height="20px"/>
            <%=Resources.lang.Features_Alert%>
            </p>
            <%=Resources.lang.Features_AlertExp%>
        </div>
    </div>
    <div class="DivFeatureOption">
        <div style="position: relative; left:130px; top:30px; height: 65px; width: 213px; font-size: 30px;"><%=Resources.lang.Features_OptionPack%></div>
        <div style="position: relative; padding: 5px 10px 10px 10px;">
        <p>
        <asp:Image ID="imgRetry" runat="server" ImageUrl="Medias/Images/retry.gif" height="30px"/>
        <%=Resources.lang.Features_AutoRetry%>
        </p>
        <%=Resources.lang.Features_AutoRetryExp%>
        <p>
        <asp:Image ID="imgUpdate" runat="server" ImageUrl="Medias/Images/update.gif" height="30px"/>
        <%=Resources.lang.Features_Update%>
        </p>
        <%=Resources.lang.Features_UpdateExp%>
        <p>
        <asp:Image ID="imgMobile" runat="server" ImageUrl="Medias/Images/mobileFeature.gif" height="30px"/>
        <%=Resources.lang.Features_Mobile%>
        </p>
        <%=Resources.lang.Features_MobileExp%>
        </div>
    </div>
    <div style="position: relative; float: left; width:10%">
    <asp:Image ID="imgSecurity" runat="server" ImageUrl="Medias/Images/security.gif" height="50px"/>
    </div>
    <div style="position: relative; float: left; width:80%">
    <%=Resources.lang.Features_Security%><br />
    <%=Resources.lang.Features_SecurityExp%>
    </div>
    </div>
</asp:Content>
