<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Account.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="FormsAuthenticateProject.Account.Registration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div>
    <table width="100%">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="First Name"></asp:Label>
                
            </td>
            <td>
                <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                   <asp:RequiredFieldValidator
                   ID="rfFirstName" runat="server" 
                   ErrorMessage="First Name Required" 
                   ControlToValidate="txtFirstName" Text ="*" 
                   ForeColor="Red" ToolTip="First Name must be entered" 
                   ValidationGroup="Profile"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Last Name"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator
                   ID="rfLastName" runat="server" 
                   ErrorMessage="Last Name Required" 
                   ControlToValidate="txtLastName" Text ="*" 
                   ForeColor="Red" ToolTip="Last Name must be entered" 
                   ValidationGroup="Profile"></asp:RequiredFieldValidator>
            </td>            
        </tr>
        <tr>
            <td>
            <asp:Label ID="Label6" runat="server" Text="User Name"></asp:Label>
            </td>
            <td>
             <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator
                   ID="RequiredFieldValidator1" runat="server" 
                   ErrorMessage="User Name Required" 
                   ControlToValidate="txtUserName" Text ="*" 
                   ForeColor="Red" ToolTip="User Name must be entered" 
                   ValidationGroup="Profile"></asp:RequiredFieldValidator>
            </td>
            <td></td>
            <td></td>
        </tr>
        <tr>

        <td>
            <asp:Label ID="Label3" runat="server" Text="Email Address"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtEmailAddress" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator
                   ID="rfEmail" runat="server" 
                   ErrorMessage="Email Address Required" 
                   ControlToValidate="txtEmailAddress" Text ="*" 
                   ForeColor="Red" ToolTip="Email must be entered" 
                   ValidationGroup="Profile"></asp:RequiredFieldValidator>
            
                   <asp:RegularExpressionValidator ID="revEmail" runat="server"
                     ErrorMessage="Email is not in a Valid format"
                        ControlToValidate="txtEmailAddress" Text="*" ForeColor="Red"
                        ValidationGroup="Profile"
                         ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                    </asp:RegularExpressionValidator>

            <asp:ValidationSummary ID="vs1" runat="server"
    ValidationGroup="Profile"
    ForeColor="Red" />
        </td>
        <td>
            <asp:Label ID="lblPhone" runat="server" Text="Phone Number"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtPhoneNumber" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator
                   ID="rfPhoneNumber" runat="server" 
                   ErrorMessage="Phone Number Required" 
                   ControlToValidate="txtPhoneNumber" Text ="*" 
                   ForeColor="Red" ToolTip="Phone number must be entered" 
                   ValidationGroup="Profile"></asp:RequiredFieldValidator>
        </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Password"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" MaxLength="12"></asp:TextBox>
                <asp:RequiredFieldValidator
                   ID="rfPassword" runat="server" 
                   ErrorMessage="Password Required" 
                   ControlToValidate="txtPassword" Text ="*" 
                   ForeColor="Red" ToolTip="Password must be entered" 
                   ValidationGroup="Profile"></asp:RequiredFieldValidator>
                
                

            </td>
            <td>
                <asp:Label ID="Label5" runat="server" Text="Confirm Password"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtConfirmPassword" runat="server"></asp:TextBox>
                <asp:CompareValidator ID="cvPassword" runat="server"
                 ErrorMessage="Passwords do not match" Text="*"
                 ForeColor="Red" ControlToCompare="txtPassword"
                 ControlToValidate="txtConfirmPassword"
                 ValidationGroup="Profile">
                </asp:CompareValidator>
            </td>
        </tr>
    </table>
    <br />
    <strong>Secret Question
    </strong>
    <table>
        <tr>       
            <td>
                <asp:Label ID="Label7" runat="server" Text="Select the Question"></asp:Label>
            </td>
            <td>
                &nbsp;&nbsp;
                <asp:DropDownList ID="dlSecretQuestion1" runat="server" Width="300px">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label8" runat="server" Text="Answer"></asp:Label>
            </td>
            <td>
                &nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="txtSecretAnswer1" runat="server" Width="400px"></asp:TextBox>
            </td>
        </tr>        
    </table>  
    <br />    
    <div style="text-align:center;">
        <asp:Label ID="lblMsg" runat="server" Text="Label" Visible="False" 
            Font-Bold="True" ForeColor="#CC0000"></asp:Label>
        <br />
       <asp:Button ID="btnCreateProfile" runat="server" Text="Create Profile"
    OnClick="btnCreateProfile_Click"
    ValidationGroup="Profile" />

<asp:Button ID="btnCancel" runat="server" Text="Cancel"
    OnClick="btnCancel_Click"
    CausesValidation="false" />

    &nbsp;<asp:TextBox ID="txtID" runat="server" Visible="False" Width="2px"></asp:TextBox>
    </div>
</div>
</asp:Content>
