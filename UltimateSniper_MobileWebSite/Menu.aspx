<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="UltimateSniper_MobileWebSite.Menu" Title="Menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Menu</title>
</head>
<body>
    <form id="formMenu" runat="server">
    Menu:<br /><br />
    <asp:Panel ID="divLog" runat="server">
        Login: <asp:TextBox ID="txtBxLogin" runat="server"></asp:TextBox><br />
        Password: <asp:TextBox ID="txtBxPassword" runat="server"></asp:TextBox><br />
        <asp:Button ID="btnLog" runat="server" Text="Log" onclick="btnLog_Click" />
    </asp:Panel>
    <asp:Panel ID="divMenu" runat="server">
        Welcome,
        <asp:Label ID="lblUserInfo" runat="server" Text="Label"></asp:Label>.
        <br /><br />
        <asp:LinkButton ID="lnkInfo" runat="server" onclick="lnkInfo_Click">My info</asp:LinkButton><br />
        <asp:LinkButton ID="lnkCategory" runat="server" onclick="lnkCategory_Click">My categories</asp:LinkButton><br />
        <asp:LinkButton ID="lnkSnipes" runat="server" onclick="lnkSnipes_Click">My snipes</asp:LinkButton><br /><br />
        <asp:LinkButton ID="lnkLogOut" runat="server" onclick="lnkLogOut_Click">Log out</asp:LinkButton>
        </asp:Panel>
        <asp:Panel ID="Panel1" runat="server">
    </asp:Panel>
    <asp:Label ID="lblComments" runat="server"></asp:Label>
    </form>
</body>
</html>
