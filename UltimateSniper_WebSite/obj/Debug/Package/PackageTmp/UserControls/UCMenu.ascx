<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCMenu.ascx.cs" Inherits="UltimateSniper_WebSite.UserControls.UCMenu" %>
<%@ Register src="~/UserControls/UCSnipeIt.ascx" tagname="SnipeIt" tagprefix="uc1" %>

<div style="position:relative;">
<div class="DivMenuFirstLine">
    <div class="DivMenuItem"><asp:LinkButton ID="lnkHome" runat="server" onclick="lnkHome_Click"><%=Resources.lang.UCMenu_Home%></asp:LinkButton></div>
    <div class="DivMenuItem"><asp:LinkButton ID="lnkFeatures" runat="server" onclick="lnkFeatures_Click"><%=Resources.lang.UCMenu_Features%></asp:LinkButton></div>
    <div class="DivMenuItem"><asp:LinkButton ID="lnkRegister" runat="server" onclick="lnkRegister_Click"><%=Resources.lang.UCMenu_Register%></asp:LinkButton></div>
    <div class="DivMenuItem"><asp:LinkButton ID="lnkWhatsSnipe" runat="server" 
            onclick="lnkWhatsSnipe_Click"><%=Resources.lang.UCMenu_WhatsSnipe%></asp:LinkButton></div>
    <div class="DivMenuItem"><asp:LinkButton ID="lnkAboutUs" runat="server" onclick="lnkAboutUs_Click"><%=Resources.lang.UCMenu_AboutUs%></asp:LinkButton></div>
    <div class="DivMenuItemLast"></div>
    <div class="DivMenuItemMiddle"></div>

    <asp:Panel ID="PanelLoginForm" runat="server">
        <div class="DivLoginForm">
                <table class="LoginForm" width="100%">
                <tr>
                    <td><%=Resources.lang.UCMenu_Name%></td>
                    <td><%=Resources.lang.UCMenu_Password%></td>
                    <td><asp:LinkButton ID="lnkForgotPassword" runat="server" onclick="lnkForgotPassword_Click"><%=Resources.lang.UCMenu_ForgotPassword%></asp:LinkButton></td>
                </tr>
                <tr>
                    <td><asp:TextBox ID="txtBxUserName" runat="server" CssClass="TextZone" MaxLength="255" Width="70px"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtBxUserPassword" runat="server" CssClass="TextZone" MaxLength="255" Width="70px" TextMode="Password"></asp:TextBox></td>
                    <td><asp:Button ID="btnLogin" runat="server" CssClass="BoutonStandard" onclick="btnLogin_Click" Text="<%$ Resources:lang, UCMenu_LogIn%>" /></td>
                </tr>
            </table>
        </div>
    </asp:Panel>
        
    <asp:Panel ID="PanelLoginDetails" runat="server">
        <div class="DivLoginForm">
            <table class="LoginForm" width="100%">
                <tr>
                    <td style="width:70%; padding-top:5px; padding-left:20px;">
                    Logged as:<br />
                    <div style="font-size:15px; font-weight: bold;"><asp:Label ID="lblConnectionInfo" runat="server"></asp:Label></div>
                    </td>
                    <td style="width:30%; padding-top:5px;">
                    <asp:LinkButton ID="lnkLogOut" runat="server" onclick="lnkLogOut_Click"><%=Resources.lang.UCMenu_LogOut%></asp:LinkButton><br />
                    <asp:ImageButton ID="imgLogOut" runat="server" ImageUrl="../Medias/Images/logout.gif" AlternateText="Log out" Width="30px" onclick="imgLogOut_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</div>
<div style="position:relative; background-color: #363636; width:100%; height:1px;"></div>
<asp:Panel ID="PanelSubMenu" runat="server">
    <div class="DivSubMenu">
        <div class="DivSubMenuFirst"></div>
        <div class="DivSubMenuMiddle"><asp:LinkButton ID="lnkInformation" runat="server" onclick="lnkInformation_Click"><%=Resources.lang.UCMenu_MyInfo%></asp:LinkButton></div>
        <div class="DivSubMenuMiddle"><asp:LinkButton ID="lnkSnipes" runat="server" onclick="lnkSnipes_Click"><%=Resources.lang.UCMenu_MySnipes%></asp:LinkButton></asp:LinkButton></div>
        <div class="DivSubMenuMiddle"><asp:LinkButton ID="lnkCategories" runat="server" onclick="lnkCategories_Click"><%=Resources.lang.UCMenu_MyCategories%></asp:LinkButton></div>
        <div class="DivSubMenuMiddle"></div>
        <div class="DivSubMenuLast"><uc1:SnipeIt ID="SnipeIt" runat="server" /></div>
    </div>
</asp:Panel>
</div>