<%@ Page Title="" Language="C#" MasterPageFile="~/Shipping/Shipping.Master" AutoEventWireup="true"
    CodeBehind="default.aspx.cs" Inherits="FormsAuthenticateProject.Shipping._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2>Shipping Console</h2>

    <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="msg"></asp:Label>

    <h3>Unshipped Orders</h3>

    <asp:GridView ID="gvUnshipped" runat="server"
        AutoGenerateColumns="False"
        DataKeyNames="InvoiceID"
        OnRowCommand="gvUnshipped_RowCommand"
        CssClass="grid"
        GridLines="None"
        CellPadding="6"
        Width="100%">

        <Columns>
            <asp:BoundField DataField="InvoiceID" HeaderText="Invoice #" />
            <asp:BoundField DataField="Email" HeaderText="Customer Email" />
            <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:C}" />
            <asp:ButtonField CommandName="MarkShipped" Text="Mark Shipped" ButtonType="Button" />
        </Columns>

        <EmptyDataTemplate>
            <div style="padding:10px;">No unshipped orders.</div>
        </EmptyDataTemplate>

    </asp:GridView>

    <hr />

    <h3>Recently Shipped</h3>

    <asp:GridView ID="gvShipped" runat="server"
        AutoGenerateColumns="False"
        CssClass="grid"
        GridLines="None"
        CellPadding="6"
        Width="100%">

        <Columns>
            <asp:BoundField DataField="InvoiceID" HeaderText="Invoice #" />
            <asp:BoundField DataField="Email" HeaderText="Customer Email" />
            <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="ShippedDate" HeaderText="Shipped Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
            <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:C}" />
        </Columns>

        <EmptyDataTemplate>
            <div style="padding:10px;">No shipped orders found.</div>
        </EmptyDataTemplate>

    </asp:GridView>

</asp:Content>
