<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Customer/Customer.Master"
    AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs"
    Inherits="FormsAuthenticateProject.Customer.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="width:700px;margin:0 auto;">

        <h2 style="text-align:center;">Change Password</h2>

        <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
        <br />

        <table style="width:100%;">
            <tr>
                <td style="width:200px;">Current Password:</td>
                <td>
                    <asp:TextBox ID="txtCurrent" runat="server" TextMode="Password" Width="300" />
                    <asp:RequiredFieldValidator ID="rfvCurrent" runat="server"
                        ControlToValidate="txtCurrent" ErrorMessage="Required"
                        ForeColor="Red" Text=" *" />
                </td>
            </tr>

            <tr>
                <td>New Password:</td>
                <td>
                    <asp:TextBox ID="txtNew" runat="server" TextMode="Password" Width="300" />
                    <asp:RequiredFieldValidator ID="rfvNew" runat="server"
                        ControlToValidate="txtNew" ErrorMessage="Required"
                        ForeColor="Red" Text=" *" />
                </td>
            </tr>

            <tr>
                <td>Confirm New Password:</td>
                <td>
                    <asp:TextBox ID="txtConfirm" runat="server" TextMode="Password" Width="300" />
                    <asp:RequiredFieldValidator ID="rfvConfirm" runat="server"
                        ControlToValidate="txtConfirm" ErrorMessage="Required"
                        ForeColor="Red" Text=" *" />
                    <asp:CompareValidator ID="cvMatch" runat="server"
                        ControlToValidate="txtConfirm" ControlToCompare="txtNew"
                        ErrorMessage="Passwords do not match" ForeColor="Red" Text=" *" />
                </td>
            </tr>
        </table>

        <div style="margin-top:14px;">
            <asp:Button ID="btnChange" runat="server" Text="Change Password" OnClick="btnChange_Click" />
            &nbsp;
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
        </div>

    </div>

</asp:Content>
