<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="UCSnipeDetails.ascx.cs" Inherits="UltimateSniper_WebSite.UserControls.UCSnipeDetails" %>
<%@ Register Assembly="MetaBuilders.WebControls" Namespace="MetaBuilders.WebControls"
    TagPrefix="mb" %>
    
<asp:Panel ID="pnlAll" runat="server">

<table width="95%">
<tr style="border: solid thin, gray">
    <td valign="top" style="width: 250px" align="center">
    <asp:Panel ID="pnlImage" runat="server">
        <asp:Image ID="img_Main" runat="server"/>
    </asp:Panel>
    </td>
    
    <td>
    <asp:Panel ID="pnlData" runat="server">
    <div style="text-align:right;">
    <asp:Label ID="lblClickNBid" runat="server" Text="<%$ Resources:lang, UCSnipeDetails_ClickNBid%>"></asp:Label>
    <asp:ImageButton ID="btnBid" runat="server" ImageUrl="../Medias/Images/bid.gif" AlternateText="Place bid" Height="32px" onclick="btnBid_Click"/>
    &nbsp;&nbsp;
    <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="../Medias/Images/edit.png" AlternateText="Edit the snipe" Height="20px" onclick="btnEdit_Click" />
    <asp:ImageButton ID="btnSave" runat="server" ImageUrl="../Medias/Images/save.png" AlternateText="Save the changes" Height="20px" onclick="btnSave_Click" Visible="False"/>
    <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="../Medias/Images/cancel.png" AlternateText="Cancel the changes" Height="20px" onclick="btnCancel_Click" Visible="False" />
    <asp:ImageButton ID="btnReduce" runat="server" ImageUrl="../Medias/Images/reduce.png" AlternateText="Show minimum" Height="20px" onclick="btnReduce_Click"/>
    <asp:ImageButton ID="btnNarrow" runat="server" ImageUrl="../Medias/Images/narrow.png" AlternateText="Hide details" Height="20px" onclick="btnNarrow_Click" Visible="False"/>
    <asp:ImageButton ID="btnExpand" runat="server" ImageUrl="../Medias/Images/expand.png" AlternateText="Show all details" Height="20px" onclick="btnExpand_Click"/>
    <asp:ImageButton ID="btnDelete" runat="server"  ImageUrl="../Medias/Images/delete.gif" AlternateText="Cancel the snipe" Height="20px" onclick="btnDelete_Click" Visible="False" />
    </div>
    <asp:Label ID="lblItemTitle" runat="server" CssClass="lblItemTitle"></asp:Label>
    <br /><br />
    <table style="width: 100%;">
    <tr>
        <td class="TDSnipeDetails">
            <%=Resources.lang.UCSnipeDetails_ItemID%> 
        </td>
        <td class="TDSnipeDetailsInfo">
            <asp:HyperLink ID="lnkItem" runat="server"></asp:HyperLink>
        </td>
    </tr>
    <tr>
        <td class="TDSnipeDetails">
            <%=Resources.lang.UCSnipeDetails_ItemEndDate%>
        </td>
        <td class="TDSnipeDetailsInfo">
            <asp:Label ID="lblItemEndDate" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="TDSnipeDetails">
            <%=Resources.lang.UCSnipeDetails_ItemCurrentPrice%>
        </td>
        <td class="TDSnipeDetailsInfo">
            <asp:Label ID="lblCurrentPrice" runat="server"></asp:Label>
            <asp:Label ID="lblCurrencyCurrentPrice" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="TDSnipeDetails">
            <%=Resources.lang.UCSnipeDetails_SnipeName%>
        </td>
        <td class="TDSnipeDetailsInfo">
            <asp:TextBox ID="txtBxSnipeName" runat="server" CssClass="TextBoxStandard" MaxLength="100"></asp:TextBox>
            <asp:Label ID="lblSnipeName" runat="server"></asp:Label>
        </td>
    </tr>
    </table>
    <asp:Panel ID="pnlNarrowDetails" runat="server" Visible="true">
    <table style="width: 100%;">
        <tr>
            <td class="TDSnipeDetails">
                <%=Resources.lang.UCSnipeDetails_ItemSellerID%>
            </td>
            <td class="TDSnipeDetailsInfo">
                <asp:Label ID="lblSellerID" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="TDSnipeDetails">
                <%=Resources.lang.UCSnipeDetails_SnipeStatus%>
            </td>
            <td class="TDSnipeDetailsInfo">
                <asp:Label ID="lblSnipeStatus" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="TDSnipeDetails">
                <%=Resources.lang.UCSnipeDetails_SnipeBid%>
            </td>
            <td class="TDSnipeDetailsInfo">
                <asp:TextBox ID="txtbxSnipeBid" runat="server" CssClass="TextBoxStandard" Width="60px" MaxLength="10"></asp:TextBox>
                <asp:Label ID="lblSnipeBid" runat="server"></asp:Label>
                <asp:Label ID="lblCurrencySnipeBid" runat="server"></asp:Label>&nbsp;
                <asp:Image ID="imgCurrency" runat="server" ImageUrl="../Medias/Images/help.gif" Height="15px" />
            </td>
        </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlStyleChoice" runat="server" Visible="false">
        <table style="width: 100%;">
        <tr>
            <td class="TDSnipeDetails">
                <%=Resources.lang.UCSnipeDetails_SnipeStyle%>
            </td>
            <td class="TDSnipeDetailsInfo">
                <asp:RadioButton ID="RadioButtonStandard" GroupName="SnipeStyle" runat="server" /> <%=Resources.lang.UCSnipeDetails_SnipeStyleClassic%>&nbsp;
                <asp:RadioButton ID="RadioButtonBidOptimizer" GroupName="SnipeStyle" runat="server" /> <%=Resources.lang.UCSnipeDetails_SnipeStyleBidOptimizer%>&nbsp;
                <asp:RadioButton ID="RadioButtonManual" GroupName="SnipeStyle" runat="server" /> <%=Resources.lang.UCSnipeDetails_SnipeStyleManual%>&nbsp;
            </td>
        </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlFurtherDetails" runat="server" Visible="false">
        <table style="width: 100%;">
        <tr>
            <td class="TDSnipeDetails">
                <%=Resources.lang.UCSnipeDetails_SnipeDescription%>
            </td>
            <td class="TDSnipeDetailsInfo">
                <asp:TextBox ID="txtBxSnipeDescription"  CssClass="TextBoxStandard" runat="server" Height="52px" MaxLength="500"></asp:TextBox>
                <asp:Label ID="lblSnipeDescription" runat="server"></asp:Label>
            </td>
        </tr>
        <!--
        <tr>
            <td class="TDSnipeDetails">
                <%=Resources.lang.UCSnipeDetails_SnipeDelay%>
            </td>
            <td class="TDSnipeDetailsInfo">
                <%=Resources.lang.UCSnipeDetails_SnipeDelayInfo%>&nbsp;
                <asp:Image ID="imgDelayHelp" runat="server" ImageUrl="../Medias/Images/help.gif" Height="15px" />
                
                <asp:DropDownList ID="drpDwnDelay" runat="server">
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem Selected="True">7</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblDelay" runat="server"></asp:Label>
            </td>
        </tr>
        -->
        <tr>
            <td class="TDSnipeDetails">
                <%=Resources.lang.UCSnipeDetails_SnipeCategories%>
            </td>
            <td class="TDSnipeDetailsInfo">
                <mb:CheckedListBox ID="ckdLstBxCategories" runat="server" Width="153px" 
                    Height="55px">
                    <asp:ListItem>Unbound</asp:ListItem>
                </mb:CheckedListBox>
            </td>
        </tr>
        <tr>
            <td class="TDSnipeDetails">
            <%=Resources.lang.UCSnipeDetails_ckbx_AutoSnipe%>
            </td>
            <td class="TDSnipeDetailsInfo">
                <asp:CheckBox ID="ckbx_AutoSnipe" runat="server"/>&nbsp;
                <asp:Image ID="imgHelp" runat="server" ImageUrl="../Medias/Images/help.gif" Height="15px" />
            </td>
        </tr>
            <tr>
            <td class="TDSnipeDetails">
            <%=Resources.lang.UCSnipeDetails_NbRetry%> / <%=Resources.lang.UCSnipeDetails_Increase%>
            </td>
            <td class="TDSnipeDetailsInfo">
                <asp:TextBox ID="txtBx_NbRetry" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                <asp:Label ID="lbl_NbRetry" runat="server"></asp:Label>
                &nbsp;
                <asp:TextBox ID="txtBx_Increase" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
                <asp:Label ID="lbl_Increase" runat="server"></asp:Label>
                <asp:Label ID="lblCurrencyIncrease" runat="server"></asp:Label>
            </td>
        </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="lblSnipeID" runat="server" Visible="False"></asp:Label> <asp:CheckBox ID="ckbBoxInsertion" runat="server" Visible="False" />
    </asp:Panel>
    </td>
</tr>
</table>

</asp:Panel>