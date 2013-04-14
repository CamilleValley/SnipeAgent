<%@ Page Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="UltimateSniper_WebSite.UserDetails" Title="User Details" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolderMainData" runat="server">
    <asp:Panel ID="PanelTitle" runat="server" CssClass="Title"><%=Resources.lang.UserDetails_Title%></asp:Panel>
    <div id="DivActive" class="DivActive">
        <div class="DivUserDetails">
            <div style="position: relative; left:130px; top:30px; height: 36px; width: 213px; font-size: 30px;">My details</div>
            <asp:Panel ID="pnlUserInfo" runat="server">
            <br /><br />
            <table cellspacing="7px" width="100%">
            <tr>
                <td style="width:40%;"><%=Resources.lang.UserDetails_UserName%></td>
                <td><asp:TextBox ID="txtBoxUserName" runat="server" MaxLength="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td><%=Resources.lang.UserDetails_UserPassword%></td>
                <td><asp:TextBox ID="txtBoxUserPassword" runat="server" MaxLength="100" TextMode="Password"></asp:TextBox></td>
            </tr>
            <tr>
                <td><%=Resources.lang.UserDetails_UserEmail%></td>
                <td><asp:TextBox ID="txtBoxUserEmailAddress" runat="server" MaxLength="100"></asp:TextBox></td>
            </tr>
            <!--
            <tr>
                <td><%=Resources.lang.UserDetails_UserEbayUserID%></td>
                <td><asp:TextBox ID="txtBoxUserEbayUserID" runat="server" MaxLength="100"></asp:TextBox></td>
            </tr>
            <tr>
                <td><%=Resources.lang.UserDetails_UserEbayUserPwd%></td>
                <td><asp:TextBox ID="txtBoxUserEbayUserPwd" runat="server" MaxLength="100"></asp:TextBox></td>
            </tr>
            -->
            <tr>
                <td><%=Resources.lang.UserDetails_UserCurrency%></td>
                <td><asp:DropDownList ID="drpDwnCurrency" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td><%=Resources.lang.UserDetails_UserSiteID%></td>
                <td>
                    <asp:DropDownList ID="DrpDwnSiteID" runat="server" Width="140px"></asp:DropDownList>&nbsp;
                    <asp:Image ID="imgSiteIDHelp" runat="server" ImageUrl="../Medias/Images/help.gif" Height="15px" />
                </td>
            </tr>
            <tr>
                <td><asp:Label ID="lblActivityDateExplaination" runat="server" Text="<%$ Resources:lang, UserDetails_UserActivityDate%>"></asp:Label></td>
                <td><asp:Label ID="lblActivityDate" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblUserOptionPackEndDateExplaination" runat="server" Text="<%$ Resources:lang, UserDetails_UserOptionPackEndDate%>"></asp:Label></td>
                <td><asp:Label ID="lblUserOptionPackEndDate" runat="server"></asp:Label></td>
            </tr>
            </table>
            <br />
            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:lang, UserDetails_btnSave%>" onclick="btn_Save_Click" CssClass="BoutonStandard" />
            <asp:Button ID="btnDisable" runat="server" Text="<%$ Resources:lang, UserDetails_btnDisable%>" onclick="btnDisable_Click" CssClass="BoutonStandard" />
            </asp:Panel>
            
            <asp:Label ID="lblEBayUserToken" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lblUserID" runat="server" Text="Label" Visible="False"></asp:Label>

            <asp:Panel ID="PanelConfirmationRegistration" runat="server" Width="100%" ForeColor="Red">
            <%=Resources.lang.UserDetails_RegistrationConfirmation%></asp:Panel>
            
            <asp:Panel ID="PanelGetToken" runat="server" style="position:relative;" Width="100%">
                <br />
                <%=Resources.lang.UserDetails_GetToken%>
                <asp:Label ID="lblEbayAccount" runat="server" Text="Label"></asp:Label>
                <asp:Label ID="lblEBayUserTokenExpirationDateExplaination" runat="server" Text="<%$ Resources:lang, UserDetails_TokenExpirationDate%>"></asp:Label>
                <asp:Label ID="lblEBayUserTokenExpirationDate" runat="server"></asp:Label>
                <br />
                <br />
                <asp:Button ID="btnRedirect" runat="server" onclick="btnRedirect_Click" Text="<%$ Resources:lang, UserDetails_btnRedirect%>" CssClass="BoutonStandard" Width="120px" />
                <asp:Image ID="imgRedirectHelp" runat="server" ImageUrl="../Medias/Images/help.gif" Height="15px" />
                &nbsp;
                <asp:Button ID="btnUpdateToken" runat="server" CssClass="BoutonStandard" Text="<%$ Resources:lang, UserDetails_btnUpdate%>" Width="120px" onclick="btnUpdateToken_Click" />
                <asp:Image ID="imgUpdateHelp" runat="server" ImageUrl="../Medias/Images/help.gif" Height="15px" />
            </asp:Panel>
        </div>
    
        <div class="DivUserDetails">
            <asp:Panel ID="pnlUserOption" runat="server">
            <div style="position: relative; left:130px; top:30px; height: 65px; width: 213px; font-size: 30px;">Options</div>
            <asp:Label ID="lblOptionsExplained" runat="server" Text="<%$ Resources:lang, UserDetails_OptionsExplaination%>"></asp:Label>
            <table cellspacing="10px">
            <tr>
                <td style="width: 25%" align="center"><asp:Image ID="imgRetry" runat="server" ImageUrl="Medias/Images/retry.gif" height="30px"/></td>
                <td>Auto retry on similar item, if unsuccessful</td>
            </tr>
            <tr>
                <td align="center"><asp:Image ID="imgUpdate" runat="server" ImageUrl="Medias/Images/update.gif" height="30px"/></td>
                <td>Item checked every 10 minutes</td>
            </tr>
            <tr>
                <td align="center"><asp:Image ID="imgMobile" runat="server" ImageUrl="Medias/Images/mobileFeature.gif" height="30px"/></td>
                <td>Access to the mobile website m.snipeagent.com</td>
            </tr>
            </table>

            <br />Learn more, click <a href="Features.aspx">here</a>.
            
            <br />
            <br />
            
            <asp:Panel ID="pnlExtendOptions" runat="server">
            
            <asp:Label ID="lblExtendOptions" runat="server" Text="<%$ Resources:lang, UserDetails_ExtendOptions%>"></asp:Label>
               
            <div style="text-align: right;">
            <asp:ImageButton ID="paypalPay" runat="server" 
                    ImageUrl="https://www.paypal.com/en_US/i/btn/btn_buynowCC_LG.gif" 
                    onclick="paypalPay_Click" />
            </div>
                    
            </asp:Panel>
            
            <%=Resources.lang.UserDetails_Donation%>
            
            <div style="text-align: right;">
            <asp:ImageButton ID="paypalDonate" runat="server" 
            ImageUrl="https://www.paypal.com/en_US/i/btn/btn_donateCC_LG.gif" 
            onclick="paypalDonate_Click" />
            </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>