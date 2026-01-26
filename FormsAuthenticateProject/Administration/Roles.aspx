<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Administration/Admin.Master"
    CodeBehind="Roles.aspx.cs" Inherits="FormsAuthenticateProject.Administration.Roles" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2 style="text-align:center;">Role Maintenance</h2>

    <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>

    <div style="width:900px;margin:0 auto;">

        <fieldset style="padding:12px;">
            <legend>Add Role</legend>

            <table style="width:100%;">
                <tr>
                    <td style="width:200px;">Role Name</td>
                    <td>
                        <asp:TextBox ID="txtRole" runat="server" MaxLength="50" Width="280px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvRole" runat="server"
                            ControlToValidate="txtRole" ErrorMessage="Role is required"
                            ForeColor="Red" Text=" * " />
                    </td>
                </tr>

                <tr>
                    <td>Status</td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="140px">
                            <asp:ListItem Text="Active" Value="1" />
                            <asp:ListItem Text="Inactive" Value="0" />
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td style="padding-top:10px;">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
        </fieldset>

        <br />

        <asp:GridView ID="gvRoles" runat="server" AutoGenerateColumns="False" Width="100%">
            <Columns>
                <asp:BoundField DataField="RoleID" HeaderText="RoleID" />
                <asp:BoundField DataField="Description" HeaderText="Role" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <%# (Convert.ToInt32(Eval("Status")) == 1) ? "Active" : "Inactive" %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </div>

</asp:Content>
