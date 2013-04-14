<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCategoryManagement.ascx.cs"
    Inherits="UltimateSniper_WebSite.UserControls.UCCategoryManagement" %>
<asp:GridView ID="grdVwCategories" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="grdVwCategories_SelectedIndexChanged"
    CssClass="DataGridsStandard" Width="90%">
    <RowStyle Height="20px" />
    <Columns>
        <asp:BoundField DataField="CategoryID" HeaderText="ID" />
        <asp:BoundField DataField="CategoryName" HeaderText="Name" />
        <asp:CommandField ShowSelectButton="True" SelectText="Edit" >
            <ItemStyle Width="50px" />
        </asp:CommandField>
    </Columns>
</asp:GridView>
<br />
<asp:Panel ID="pnlEdit" runat="server" Visible="False">
    <%=Resources.lang.UCCategoryManagement_txtBxName%>
    <asp:TextBox ID="txtBxName" runat="server" MaxLength="100"></asp:TextBox>
    <asp:Button ID="btnUpdate" runat="server" Text="<%$ Resources:lang, UCCategoryManagement_btnUpdate%>" OnClick="btnUpdate_Click"
        CssClass="BoutonStandard" />
    &nbsp;<asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="<%$ Resources:lang, UCCategoryManagement_btnDelete%>"
        CssClass="BoutonStandard" />
    &nbsp;<asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="<%$ Resources:lang, UCCategoryManagement_btnCancel%>"
        CssClass="BoutonStandard" />
    <asp:Label ID="lblCategoryID" runat="server" Visible="False"></asp:Label>
</asp:Panel>
<br />
<asp:Button ID="btnAddNew" runat="server" Text="<%$ Resources:lang, UCCategoryManagement_btnAddNew%>" OnClick="btnAddNew_Click"
    CssClass="BoutonStandard" />
