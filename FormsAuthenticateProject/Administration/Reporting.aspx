<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/Admin.Master"
    AutoEventWireup="true" CodeBehind="Reporting.aspx.cs"
    Inherits="FormsAuthenticateProject.Administration.Reporting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="width: 1100px; margin: 0 auto;">

        <h2 style="text-align:center;">Sales Reporting</h2>

        <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Visible="false" />
        <br />

        <fieldset style="padding:12px;">
            <legend><b>Invoice Report</b></legend>

            <table>
                <tr>
                    <td>From Date:</td>
                    <td>
                        <asp:TextBox ID="txtFrom" runat="server" Width="150" />
                    </td>

                    <td style="padding-left:20px;">To Date:</td>
                    <td>
                        <asp:TextBox ID="txtTo" runat="server" Width="150" />
                    </td>

                    <td style="padding-left:20px;">
                        <asp:Button ID="btnRun" runat="server" Text="Run Report" OnClick="btnRun_Click" />
                        &nbsp;
                        <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                    </td>
                </tr>
            </table>

            <br />
            <asp:Label ID="lblSummary" runat="server" Font-Bold="true" />
        </fieldset>

        <br />

        <h3>Invoices</h3>
        <asp:GridView ID="gvInvoices" runat="server"
            AutoGenerateColumns="false"
            DataKeyNames="InvoiceID"
            Width="100%"
            GridLines="Horizontal"
            CellPadding="6"
            OnSelectedIndexChanged="gvInvoices_SelectedIndexChanged">

            <Columns>
                <asp:CommandField ShowSelectButton="true" SelectText="View Items" />
                <asp:BoundField DataField="InvoiceID" HeaderText="Invoice #" />
                <asp:BoundField DataField="CustomerUserID" HeaderText="Customer ID" />
                <asp:BoundField DataField="InvoiceDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                <asp:BoundField DataField="SubTotal" HeaderText="SubTotal" DataFormatString="{0:C}" />
                <asp:BoundField DataField="Tax" HeaderText="Tax" DataFormatString="{0:C}" />
                <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="{0:C}" />
                <asp:CheckBoxField DataField="Shipped" HeaderText="Shipped" />
                <asp:BoundField DataField="ShippedDate" HeaderText="Shipped Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
            </Columns>
        </asp:GridView>

        <br />

        <h3>Invoice Items</h3>
        <asp:GridView ID="gvItems" runat="server"
            AutoGenerateColumns="false"
            Width="100%"
            GridLines="Horizontal"
            CellPadding="6">

            <Columns>
                <asp:BoundField DataField="ProductName" HeaderText="Product" />
                <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" DataFormatString="{0:C}" />
                <asp:BoundField DataField="Quantity" HeaderText="Qty" />
                <asp:BoundField DataField="LineTotal" HeaderText="Line Total" DataFormatString="{0:C}" />
            </Columns>
        </asp:GridView>

    </div>

</asp:Content>
