using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI.WebControls;

namespace FormsAuthenticateProject.Administration
{
    public partial class UserMaintenance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMsg.Visible = false;
                BindUsers();
            }
        }

        private void BindUsers()
        {
            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                SELECT 
                    u.UserID,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.RoleID,
                    r.Description AS RoleName,
                    u.Status
                FROM dbo.Users u
                INNER JOIN dbo.Roles r ON u.RoleID = r.RoleID
                ORDER BY u.LastName, u.FirstName;", con))
            {
                con.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);

                    gvUsers.DataSource = dt;
                    gvUsers.DataBind();
                }
            }
        }

        private DataTable GetActiveRoles()
        {
            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                SELECT RoleID, Description
                FROM dbo.Roles
                WHERE Status = 1
                ORDER BY Description;", con))
            {
                con.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            lblMsg.Visible = false;
            BindUsers();
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            lblMsg.Visible = false;
            BindUsers();
        }

        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            // When in edit mode, populate the Role dropdown
            if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                var ddlRole = e.Row.FindControl("ddlRole") as DropDownList;
                var hfRoleID = e.Row.FindControl("hfRoleID") as HiddenField;

                if (ddlRole == null || hfRoleID == null) return;

                DataTable roles = GetActiveRoles();

                ddlRole.DataSource = roles;
                ddlRole.DataTextField = "Description";
                ddlRole.DataValueField = "RoleID";
                ddlRole.DataBind();

                // select current role
                var currentRoleId = hfRoleID.Value;
                if (!string.IsNullOrWhiteSpace(currentRoleId) && ddlRole.Items.FindByValue(currentRoleId) != null)
                {
                    ddlRole.SelectedValue = currentRoleId;
                }
            }
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

            GridViewRow row = gvUsers.Rows[e.RowIndex];
            var ddlRole = row.FindControl("ddlRole") as DropDownList;
            var chkActive = row.FindControl("chkActive") as CheckBox;

            if (ddlRole == null || chkActive == null)
            {
                ShowMsg("Could not find edit controls. Rebuild solution and try again.", false);
                return;
            }

            int newRoleId = Convert.ToInt32(ddlRole.SelectedValue);
            bool newStatus = chkActive.Checked;

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                UPDATE dbo.Users
                SET RoleID = @RoleID,
                    Status = @Status
                WHERE UserID = @UserID;", con))
            {
                cmd.Parameters.AddWithValue("@RoleID", newRoleId);
                cmd.Parameters.AddWithValue("@Status", newStatus ? 1 : 0);
                cmd.Parameters.AddWithValue("@UserID", userId);

                con.Open();
                int updated = cmd.ExecuteNonQuery();

                gvUsers.EditIndex = -1;
                BindUsers();

                ShowMsg(updated == 1 ? "User updated successfully." : "No rows updated.", updated == 1);
            }
        }

        private void ShowMsg(string text, bool success)
        {
            lblMsg.Visible = true;
            lblMsg.ForeColor = success ? Color.Green : Color.Red;
            lblMsg.Text = text;
        }
    }
}
