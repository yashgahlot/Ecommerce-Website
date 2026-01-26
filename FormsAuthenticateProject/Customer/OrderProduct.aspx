<%@ Page Title="" Language="C#" MasterPageFile="~/Customer/Customer.Master"
    AutoEventWireup="true" CodeBehind="OrderProduct.aspx.cs"
    Inherits="FormsAuthenticateProject.Customer.OrderProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        .wrap { width: 900px; margin: 0 auto; }
        .title { text-align: center; margin: 10px 0 15px; }
        .box { border: 1px solid #ddd; padding: 14px; margin-bottom: 14px; background: #fafafa; }
        .box h3 { margin: 0 0 10px; }
        .row { display: flex; gap: 12px; margin-bottom: 10px; align-items: center; }
        .row label { width: 220px; font-weight: 600; }
        .input { width: 260px; padding: 6px; }
        .dd { width: 272px; padding: 6px; }
        .msg { text-align: center; margin: 10px 0; }
        .totals { font-weight: 700; margin-top: 8px; }
        .actions { margin-top: 10px; }
        .btn { padding: 6px 14px; margin-right: 8px; }
        .muted { font-size: 12px; color: #666; }
    </style>

    <div class="wrap">

        <h2 class="title">Order Products</h2>

        <div class="msg">
            <asp:Label ID="lblLoggedIn" runat="server" Font-Bold="true"></asp:Label>
        </div>

        <div class="msg">
            <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
        </div>

        <!-- 1) Select Product -->
        <div class="box">
            <h3>1) Select Product</h3>

            <div class="row">
                <label>Supplier:</label>
                <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="dd"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlSupplier_SelectedIndexChanged" />
                <asp:RequiredFieldValidator ID="rfvSupplier" runat="server"
                    ControlToValidate="ddlSupplier" InitialValue=""
                    ErrorMessage="Supplier required" ForeColor="Red" Text=" *" />
            </div>

            <div class="row">
                <label>Category:</label>
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="dd"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" />
                <asp:RequiredFieldValidator ID="rfvCategory" runat="server"
                    ControlToValidate="ddlCategory" InitialValue=""
                    ErrorMessage="Category required" ForeColor="Red" Text=" *" />
            </div>

            <div class="row">
                <label>Product:</label>
                <asp:DropDownList ID="ddlProduct" runat="server" CssClass="dd"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged" />
                <asp:RequiredFieldValidator ID="rfvProduct" runat="server"
                    ControlToValidate="ddlProduct" InitialValue=""
                    ErrorMessage="Product required" ForeColor="Red" Text=" *" />
            </div>

            <div class="row">
                <label>Description:</label>
                <asp:Label ID="lblDescription" runat="server" Text="-"></asp:Label>
            </div>

            <div class="row">
                <label>Retail Price:</label>
                <asp:Label ID="lblPrice" runat="server" Text="$0.00"></asp:Label>
            </div>

            <div class="row">
                <label>Quantity:</label>
                <asp:TextBox ID="txtQty" runat="server" CssClass="input"
                    AutoPostBack="true" OnTextChanged="txtQty_TextChanged" />
                <asp:RequiredFieldValidator ID="rfvQty" runat="server"
                    ControlToValidate="txtQty" ErrorMessage="Quantity required"
                    ForeColor="Red" Text=" *" />
                <asp:RegularExpressionValidator ID="revQty" runat="server"
                    ControlToValidate="txtQty" ForeColor="Red" Text=" *"
                    ErrorMessage="Quantity must be a whole number"
                    ValidationExpression="^\d+$" />
            </div>

            <div class="totals">
                <asp:Label ID="lblTotals" runat="server"
                    Text="SubTotal: $0.00 | Tax: $0.00 | Total: $0.00"></asp:Label>
                <div class="muted">Tax is 13% as per assignment.</div>
            </div>
        </div>

        <!-- 2) Billing -->
        <div class="box">
            <h3>2) Billing Information (Credit Card Only)</h3>

            <div class="row">
                <label>Card Holder Name:</label>
                <asp:TextBox ID="txtCardHolderName" runat="server" CssClass="input" />
                <asp:RequiredFieldValidator ID="rfvCardHolder" runat="server"
                    ControlToValidate="txtCardHolderName"
                    ErrorMessage="Card holder name required" ForeColor="Red" Text=" *" />
            </div>

            <div class="row">
                <label>Card Type:</label>
                <asp:DropDownList ID="ddlCardType" runat="server" CssClass="dd" />
                <asp:RequiredFieldValidator ID="rfvCardType" runat="server"
                    ControlToValidate="ddlCardType" InitialValue=""
                    ErrorMessage="Card type required" ForeColor="Red" Text=" *" />
            </div>

            <div class="row">
                <label>Card Number:</label>
                <asp:TextBox ID="txtCardNumber" runat="server" CssClass="input" MaxLength="25" />
                <asp:RequiredFieldValidator ID="rfvCardNumber" runat="server"
                    ControlToValidate="txtCardNumber"
                    ErrorMessage="Card number required" ForeColor="Red" Text=" *" />
            </div>

            <div class="row">
                <label>Security Code (3 digits):</label>
                <asp:TextBox ID="txtSecurityCode" runat="server" CssClass="input" MaxLength="4" />
                <asp:RequiredFieldValidator ID="rfvSec" runat="server"
                    ControlToValidate="txtSecurityCode"
                    ErrorMessage="Security code required" ForeColor="Red" Text=" *" />
                <asp:RegularExpressionValidator ID="revSec" runat="server"
                    ControlToValidate="txtSecurityCode"
                    ErrorMessage="Security code must be 3 digits"
                    ValidationExpression="^\d{3}$" ForeColor="Red" Text=" *" />
            </div>

            <div class="row">
                <label>Expiry Month:</label>
                <asp:DropDownList ID="ddlExpMonth" runat="server" CssClass="dd" />
                <asp:RequiredFieldValidator ID="rfvExpMonth" runat="server"
                    ControlToValidate="ddlExpMonth" InitialValue=""
                    ErrorMessage="Expiry month required" ForeColor="Red" Text=" *" />
            </div>

            <div class="row">
                <label>Expiry Year:</label>
                <asp:DropDownList ID="ddlExpYear" runat="server" CssClass="dd" />
                <asp:RequiredFieldValidator ID="rfvExpYear" runat="server"
                    ControlToValidate="ddlExpYear" InitialValue=""
                    ErrorMessage="Expiry year required" ForeColor="Red" Text=" *" />
            </div>
        </div>

        <!-- 3) Delivery -->
        <div class="box">
            <h3>3) Delivery Information</h3>

            <div class="row">
                <label>House Number:</label>
                <asp:TextBox ID="txtHouseNumber" runat="server" CssClass="input" />
                <asp:RequiredFieldValidator ID="rfvHouse" runat="server"
                    ControlToValidate="txtHouseNumber"
                    ErrorMessage="House number required" ForeColor="Red" Text=" *" />
            </div>

            <div class="row">
                <label>Street Name:</label>
                <asp:TextBox ID="txtStreetName" runat="server" CssClass="input" />
                <asp:RequiredFieldValidator ID="rfvStreetName" runat="server"
                    ControlToValidate="txtStreetName"
                    ErrorMessage="Street name required" ForeColor="Red" Text=" *" />
            </div>

            <div class="row">
                <label>Street Type:</label>
                <asp:DropDownList ID="ddlStreetType" runat="server" CssClass="dd" />
                <asp:RequiredFieldValidator ID="rfvStreetType" runat="server"
                    ControlToValidate="ddlStreetType" InitialValue=""
                    ErrorMessage="Street type required" ForeColor="Red" Text=" *" />
            </div>

            <div class="row">
                <label>Apartment Unit (optional):</label>
                <asp:TextBox ID="txtApartment" runat="server" CssClass="input" />
            </div>

            <div class="row">
                <label>City:</label>
                <asp:TextBox ID="txtCity" runat="server" CssClass="input" />
                <asp:RequiredFieldValidator ID="rfvCity" runat="server"
                    ControlToValidate="txtCity"
                    ErrorMessage="City required" ForeColor="Red" Text=" *" />
            </div>

            <div class="row">
                <label>Province:</label>
                <asp:TextBox ID="txtProvince" runat="server" CssClass="input" />
                <asp:RequiredFieldValidator ID="rfvProvince" runat="server"
                    ControlToValidate="txtProvince"
                    ErrorMessage="Province required" ForeColor="Red" Text=" *" />
            </div>

            <div class="row">
                <label>Postal Code:</label>
                <asp:TextBox ID="txtPostalCode" runat="server" CssClass="input" MaxLength="15" />
                <asp:RequiredFieldValidator ID="rfvPostal" runat="server"
                    ControlToValidate="txtPostalCode"
                    ErrorMessage="Postal code required" ForeColor="Red" Text=" *" />
            </div>

            <div class="actions">
                <asp:Button ID="btnSave" runat="server" Text="Save Order" CssClass="btn" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn"
                    CausesValidation="false" OnClick="btnCancel_Click" />
            </div>
        </div>

    </div>
</asp:Content>
