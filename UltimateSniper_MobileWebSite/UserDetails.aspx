<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="UltimateSniper_MobileWebSite.UserDetails" Title="Details" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Details</title>
</head>
<body>
    <form id="formMenu" runat="server">
    Details:<br /><br />
    For the complete options, please visit the website.<br /><br />
    User name: <asp:Label ID="lblUserName" runat="server" Text="Label"></asp:Label><br />
    Password: <asp:TextBox ID="txtBoxUserPassword" runat="server"></asp:TextBox><br />
    Email: <asp:TextBox ID="txtBoxUserEmailAddress" runat="server"></asp:TextBox><br />
    Ebay user ID: <asp:TextBox ID="txtBoxUserEbayUserID" runat="server"></asp:TextBox><br />
    Ebay user password: <asp:TextBox ID="txtBoxUserEbayUserPwd" runat="server"></asp:TextBox><br />
    Currency: <asp:Label ID="lblCurrency" runat="server"></asp:Label><br />
    Site: <asp:Label ID="lblSiteID" runat="server"></asp:Label><br />
    Active since:<asp:Label ID="lblActivityDate" runat="server"></asp:Label><br />
    Options available till: <asp:Label ID="lblUserOptionPackEndDate" runat="server"></asp:Label><br /><br />
    <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btn_Save_Click" />
    <br /><br />
    <asp:Label ID="lblComments" runat="server"></asp:Label>
    <br />
    <asp:LinkButton ID="lnkMenu" runat="server" onclick="lnkMenu_Click">Back</asp:LinkButton>
    <asp:Label ID="lblUserID" runat="server" Text="Label" Visible="False"></asp:Label>
    </form>
</body>
</html>