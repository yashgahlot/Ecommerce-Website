<%@ Page Title="" Language="C#" MasterPageFile="~/Administration/Admin.Master" AutoEventWireup="true"
    CodeBehind="UserMaintenance.aspx.cs" Inherits="FormsAuthenticateProject.Administration.UserMaintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="width: 1000px; margin: 0 auto;">

        <h2 style="text-align:center;">User Maintenance</h2>

        <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
        <br />

        <asp:GridView ID="gvUsers" runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="UserID"
            Width="100%"
            OnRowEditing="gvUsers_RowEditing"
            OnRowCancelingEdit="gvUsers_RowCancelingEdit"
            OnRowUpdating="gvUsers_RowUpdating"
            OnRowDataBound="gvUsers_RowDataBound"
            CellPadding="6"
            GridLines="Horizontal">

            <Columns>
                <asp:BoundField DataField="UserID" HeaderText="UserID" ReadOnly="true" />
                <asp:BoundField DataField="FirstName" HeaderText="First Name" ReadOnly="true" />
                <asp:BoundField DataField="LastName" HeaderText="Last Name" ReadOnly="true" />
                <asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="true" />

                <%-- Role --%>
                <asp:TemplateField HeaderText="Role">
                    <ItemTemplate>
                        <asp:Label ID="lblRole" runat="server" Text='<%# Eval("RoleName") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlRole" runat="server"></asp:DropDownList>
                        <asp:HiddenField ID="hfRoleID" runat="server" Value='<%# Eval("RoleID") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <%-- Status --%>
                <asp:TemplateField HeaderText="Active">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server"
                            Text='<%# (Convert.ToBoolean(Eval("Status")) ? "Yes" : "No") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Convert.ToBoolean(Eval("Status")) %>' />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:CommandField ShowEditButton="true" />
            </Columns>

        </asp:GridView>

    </div>

</asp:Content>
