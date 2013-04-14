<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="UltimateSniper_MobileWebSite.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Error</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    We are sorry but an unexpected error occured.<br />
    Please try again or visit the standard website.<br />
    <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="Menu.aspx">Back</asp:HyperLink>
    </div>
    </form>
</body>
</html>
