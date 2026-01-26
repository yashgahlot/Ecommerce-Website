<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/Admin.Master"
    AutoEventWireup="true" CodeBehind="ProductManagement.aspx.cs"
    Inherits="FormsAuthenticateProject.Administration.Products" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2 style="text-align:center;">Product Maintenance</h2>

    <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Visible="false" />
    <br />

    <div style="width:950px;margin:0 auto;">

        <table style="width:100%;">
            <tr>
                <td style="width:160px;">Supplier:</td>
                <td>
                    <asp:DropDownList ID="ddlSupplier" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlSupplier_SelectedIndexChanged" />
                    <asp:RequiredFieldValidator ID="rfvSupplier" runat="server"
                        ControlToValidate="ddlSupplier" InitialValue=""
                        ErrorMessage="Supplier required" ForeColor="Red" Text=" *" />
                </td>
            </tr>

            <tr>
                <td>Category:</td>
                <td>
                    <asp:DropDownList ID="ddlCategory" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvCategory" runat="server"
                        ControlToValidate="ddlCategory" InitialValue=""
                        ErrorMessage="Category required" ForeColor="Red" Text=" *" />
                </td>
            </tr>

            <tr>
                <td>Product Name:</td>
                <td>
                    <asp:TextBox ID="txtProductName" runat="server" Width="420" />
                    <asp:RequiredFieldValidator ID="rfvName" runat="server"
                        ControlToValidate="txtProductName"
                        ErrorMessage="Product name required" ForeColor="Red" Text=" *" />
                </td>
            </tr>

            <tr>
                <td>Retail Price:</td>
                <td>
                    <asp:TextBox ID="txtRetailPrice" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvPrice" runat="server"
                        ControlToValidate="txtRetailPrice"
                        ErrorMessage="Price required" ForeColor="Red" Text=" *" />
                    <asp:RegularExpressionValidator ID="revPrice" runat="server"
                        ControlToValidate="txtRetailPrice"
                        ErrorMessage="Enter valid price (e.g., 99.99)"
                        ValidationExpression="^\d+(\.\d{1,2})?$"
                        ForeColor="Red" Text=" *" />
                </td>
            </tr>

            <tr>
                <td>Status:</td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Value="1" Text="Active" />
                        <asp:ListItem Value="0" Text="Inactive" />
                    </asp:DropDownList>
                </td>
            </tr>
        </table>

        <div style="margin-top:12px;">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
            <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" Enabled="false" />
            <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" Enabled="false" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" />
        </div>

        <hr style="margin:18px 0;" />

        <asp:HiddenField ID="hfProductID" runat="server" />

        <asp:GridView ID="gvProducts" runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="ProductID"
            Visible="false"
            CellPadding="6"
            GridLines="Horizontal"
            OnSelectedIndexChanged="gvProducts_SelectedIndexChanged">

            <Columns>
                <asp:CommandField ShowSelectButton="true" SelectText="Select" />
                <asp:BoundField DataField="ProductID" HeaderText="ID" />
                <asp:BoundField DataField="ProductName" HeaderText="Product" />
                <asp:BoundField DataField="CategoryDesc" HeaderText="Category" />
                <asp:BoundField DataField="RetailPrice" HeaderText="Price" DataFormatString="{0:C}" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <%# Convert.ToBoolean(Eval("Status")) ? "Active" : "Inactive" %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </div>
</asp:Content>
