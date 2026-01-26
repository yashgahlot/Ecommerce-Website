<%@ Page Title="" Language="C#" MasterPageFile="~/Account/Account.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FormsAuthenticateProject.Account.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div>

<table width="100%">
    <tr>
        <td width="30%"></td>
        <td>
    <table width ="60% >    
        <tr>
            <td style="text-align:right;"> 
                
            </td>
            <td >
                <asp:Label ID="Label3" runat="server" Text="Username"></asp:Label>
            </td>
            <td>             
               <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Password"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnLogin" runat="server" Text="Login" 
                    onclick="btnLogin_Click" />
            </td>
        </tr>
        <td>
        <td>
            <asp:LinkButton ID="lnkForgotPassword" runat="server" 
                PostBackUrl="~/ForgotPassword.aspx">Forgot Password</asp:LinkButton></td>
        <td>
            <asp:LinkButton ID="lnkCreateProfile" runat="server" 
                PostBackUrl="~/Account/Registration.aspx">Create Account</asp:LinkButton></td>
        <td></td>
        </td>
    
</table>
        </td>
    </tr>
</table>
</div>
<div style="text-align:center;">
    <asp:Label ID="lblMsg" runat="server" Text="Label" Visible="False" 
        Font-Bold="True" ForeColor="#CC0000"></asp:Label>
</div>
</asp:Content>
