<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Snipes.aspx.cs" Inherits="UltimateSniper_MobileWebSite.Snipes" Title="Snipes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Details</title>
</head>
<body>
    <form id="formSnipes" runat="server">
    Item ID to snipe: 
    <asp:TextBox ID="txtBoxItemID" runat="server" Width="83px"></asp:TextBox>
    &nbsp;<asp:Button ID="btnLoadDetails" runat="server" Text="Load" onclick="btnLoadDetails_Click"/>
    <br /><br />
    For the complete options, please visit the website.<br />
    <asp:Panel ID="pnlSnipeList" runat="server" Visible="true">
    List of open snipes:
    <asp:GridView ID="GrdVwSnipeList" runat="server" AutoGenerateColumns="False" 
        onselectedindexchanged="GrdVwSnipeList_SelectedIndexChanged">
        <Columns>
            <asp:BoundField DataField="SnipeID" HeaderText="Snipe ID" />
            <asp:BoundField DataField="SnipeName" HeaderText="Name" />
            <asp:CommandField ShowSelectButton="True" />
        </Columns>
    </asp:GridView>
    </asp:Panel>
    
    <asp:Panel ID="pnlStyleChoice" runat="server" Visible="false">
    Snipe style: 
    <asp:RadioButton ID="RadioButtonStandard" GroupName="SnipeStyle" runat="server" /> Classic snipe &nbsp;
    <asp:RadioButton ID="RadioButtonManual" GroupName="SnipeStyle" runat="server" /> Manual snipe &nbsp;
    <asp:RadioButton ID="RadioButtonBidOptimizer" GroupName="SnipeStyle" runat="server" /> Bid optimizer
    </asp:Panel>
    
    <asp:Panel ID="pnlAll" runat="server" Visible="false">
    Name: <asp:TextBox ID="txtbxSnipeName" runat="server"></asp:TextBox><br />
    Item Title: <asp:Label ID="lblItemTitle" runat="server" Text="Label"></asp:Label><br />
    Bid:  <asp:TextBox ID="txtbxSnipeBid" runat="server"></asp:TextBox>
        <asp:Label ID="lblCurrency" runat="server"></asp:Label>
        <br />
    Description: <asp:TextBox ID="txtbxSnipeDescription" runat="server"></asp:TextBox>
    <!--
        <br />
    Delay: random btw 5 and 15 sec
    
    <asp:DropDownList ID="drpDwnDelay" runat="server">
        <asp:ListItem>5</asp:ListItem>
        <asp:ListItem Selected="True">7</asp:ListItem>
        <asp:ListItem>10</asp:ListItem>
    </asp:DropDownList>
    -->
        <br />
        Last known price:
        <asp:Label ID="lblLastKnownPrice" runat="server" Text="Label"></asp:Label>
        <br />
        Seller ID:
        <asp:Label ID="lblsellerID" runat="server" Text="Label"></asp:Label>
        <br />
        Categories:
    <asp:CheckBoxList ID="ckbxListCategories" runat="server"></asp:CheckBoxList><br />
    
    <asp:CheckBox ID="ckbx_AutoSnipe" runat="server" Text="Snipe similar items from seller if failed."/><br />
            
    Number of auto snipes:
    <asp:TextBox ID="txtBx_NbRetry" runat="server" Width="65px"></asp:TextBox><br />
            
    Increase between each attempt:
    <asp:TextBox ID="txtBx_Increase" runat="server" Width="65px"></asp:TextBox><br />
    
    <asp:Panel ID="pnlItemInfo" runat="server" Visible="false">
    
    Item ID: <asp:HyperLink ID="lnkItem" runat="server">HyperLink</asp:HyperLink><br />
    End date: <asp:Label ID="lblItemEndDate" runat="server"></asp:Label><br />
    
    </asp:Panel>
    
    <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" />
    &nbsp;<asp:Button ID="btnDisable" runat="server" Text="Disable" onclick="btnDisable_Click" />
    &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" onclick="btnCancel_Click" />
    
    </asp:Panel>
    <asp:Label ID="lblSnipeID" runat="server" Visible="False"></asp:Label>  
    <br /><br />
    <asp:Label ID="lblComments" runat="server"></asp:Label>
    <br />
    <asp:LinkButton ID="lnkMenu" runat="server" onclick="lnkMenu_Click">Back</asp:LinkButton>
    </form>
</body>
</html>