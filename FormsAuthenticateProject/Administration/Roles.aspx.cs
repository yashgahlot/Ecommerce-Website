using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace FormsAuthenticateProject.Administration
{
    public partial class Roles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
                ddlStatus.SelectedValue = "1";
            }
        }

        private void BindGrid()
        {
            using (var con = Db.Conn())
            using (var cmd = new SqlCommand("SELECT RoleID, [Description], Status FROM dbo.Roles ORDER BY RoleID", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                da.Fill(dt);

                gvRoles.DataSource = dt;
                gvRoles.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = true;

            string roleName = txtRole.Text.Trim();
            int status = int.Parse(ddlStatus.SelectedValue);

            if (string.IsNullOrWhiteSpace(roleName))
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Role name is required.";
                return;
            }

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand("INSERT INTO dbo.Roles([Description], Status) VALUES(@Desc, @Status)", con))
            {
                cmd.Parameters.AddWithValue("@Desc", roleName);
                cmd.Parameters.AddWithValue("@Status", status);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Role added.";

            txtRole.Text = "";
            ddlStatus.SelectedValue = "1";

            BindGrid();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtRole.Text = "";
            ddlStatus.SelectedValue = "1";
            lblMsg.Visible = false;
        }
    }
}
