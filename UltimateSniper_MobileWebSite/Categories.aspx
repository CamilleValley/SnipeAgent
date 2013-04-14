<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="UltimateSniper_MobileWebSite.CategoriesView" Title="Categories" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="HeadCategories" runat="server">
    <title>Categories</title>
</head>
<body>
    <form id="formCategories" runat="server">
    Categories:<br /><br />
    <asp:GridView ID="grdVwCategories" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="grdVwCategories_SelectedIndexChanged">
        <Columns>
            <asp:BoundField DataField="CategoryID" HeaderText="ID" />
            <asp:BoundField DataField="CategoryName" HeaderText="Name" />
            <asp:CommandField ShowSelectButton="True" />
        </Columns>
    </asp:GridView>
    <asp:Panel ID="pnlEdit" runat="server" Visible="False">
        <br />
        Name:
        <asp:TextBox ID="txtBxName" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="btnUpdate" runat="server" Text="Update" 
            onclick="btnUpdate_Click"/>
        &nbsp;<asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete"/>
        &nbsp;<asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel"/>
        <asp:Label ID="lblCategoryID" runat="server" Visible="False"></asp:Label>
    </asp:Panel>
    <br />
    <asp:Button ID="btnAddNew" runat="server" Text="Add new" 
        onclick="btnAddNew_Click"  />
    <br /><br />
    <asp:Label ID="lblComments" runat="server"></asp:Label>
    <br />
    <asp:LinkButton ID="lnkMenu" runat="server" onclick="lnkMenu_Click">Back</asp:LinkButton>
    </form>
</body>
</html>
