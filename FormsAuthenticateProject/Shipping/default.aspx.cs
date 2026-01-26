using System;
using System.Data;
using System.Data.SqlClient;

namespace FormsAuthenticateProject.Shipping
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
                LoadGrids();
        }

        private void LoadGrids()
        {
            lblMsg.Visible = false;

            using (var con = Db.Conn())
            {
                con.Open();

                // Unshipped
                using (var cmd = new SqlCommand(@"
                    SELECT i.InvoiceID, u.Email, i.InvoiceDate, i.Total
                    FROM dbo.Invoice i
                    INNER JOIN dbo.Users u ON i.CustomerUserID = u.UserID
                    WHERE i.Shipped = 0
                    ORDER BY i.InvoiceDate DESC;", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    gvUnshipped.DataSource = dt;
                    gvUnshipped.DataBind();
                }

                // Shipped (last 20)
                using (var cmd = new SqlCommand(@"
                    SELECT TOP 20 i.InvoiceID, u.Email, i.InvoiceDate, i.ShippedDate, i.Total
                    FROM dbo.Invoice i
                    INNER JOIN dbo.Users u ON i.CustomerUserID = u.UserID
                    WHERE i.Shipped = 1
                    ORDER BY i.ShippedDate DESC;", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    gvShipped.DataSource = dt;
                    gvShipped.DataBind();
                }
            }
        }

        protected void gvUnshipped_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName != "MarkShipped") return;

            // ButtonField sends row index as CommandArgument
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int invoiceId = Convert.ToInt32(gvUnshipped.DataKeys[rowIndex].Value);

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                UPDATE dbo.Invoice
                SET Shipped = 1,
                    ShippedDate = GETDATE()
                WHERE InvoiceID = @InvoiceID AND Shipped = 0;", con))
            {
                cmd.Parameters.AddWithValue("@InvoiceID", invoiceId);
                con.Open();

                int updated = cmd.ExecuteNonQuery();

                lblMsg.Visible = true;
                lblMsg.ForeColor = (updated == 1) ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                lblMsg.Text = (updated == 1)
                    ? $"Invoice #{invoiceId} marked shipped."
                    : "Nothing updated (maybe already shipped).";
            }

            LoadGrids();
        }
    }
}
