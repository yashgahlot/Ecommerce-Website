<%@ Page Title="View History" Language="C#" MasterPageFile="~/Customer/Customer.Master"
    AutoEventWireup="true" CodeBehind="ViewHistory.aspx.cs"
    Inherits="FormsAuthenticateProject.Customer.ViewHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="width:950px;margin:0 auto;">
        <h2 style="text-align:center;">Order History</h2>

        <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
        <br />

        <asp:GridView ID="gvInvoices" runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="InvoiceID"
            OnSelectedIndexChanged="gvInvoices_SelectedIndexChanged"
            GridLines="Horizontal"
            CellPadding="6"
            Width="100%">

            <Columns>
                <asp:CommandField ShowSelectButton="true" SelectText="View Items" />
                <asp:BoundField DataField="InvoiceID" HeaderText="Invoice #" />
                <asp:BoundField DataField="InvoiceDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                <asp:BoundField DataField="SubTotal" HeaderText="SubTotal" DataFormatString="{0:C}" />
                <asp:BoundField DataField="Tax" HeaderText="Tax" DataFormatString="{0:C}" />
                <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:C}" />

                <asp:TemplateField HeaderText="Shipped">
                    <ItemTemplate>
                        <%# Convert.ToBoolean(Eval("Shipped")) ? "Yes" : "No" %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Shipped Date">
                    <ItemTemplate>
                        <%# Eval("ShippedDate") == DBNull.Value ? "" : Convert.ToDateTime(Eval("ShippedDate")).ToString("yyyy-MM-dd HH:mm") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <hr />

        <h3>Invoice Items</h3>

        <asp:GridView ID="gvItems" runat="server"
            AutoGenerateColumns="False"
            GridLines="Horizontal"
            CellPadding="6"
            Width="100%">

            <Columns>
                <asp:BoundField DataField="ProductName" HeaderText="Product" />
                <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" DataFormatString="{0:C}" />
                <asp:BoundField DataField="Quantity" HeaderText="Qty" />
                <asp:BoundField DataField="LineTotal" HeaderText="Line Total" DataFormatString="{0:C}" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
