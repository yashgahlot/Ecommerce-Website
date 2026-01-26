using System;
using System.Data;
using System.Data.SqlClient;

namespace FormsAuthenticateProject.Customer
{
    public partial class TrackOrders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                gvItems.DataSource = null;
                gvItems.DataBind();
                ddlFilter.SelectedValue = "ALL";
                LoadOrders();
            }
        }

        private int GetCurrentUserId()
        {
            string email = Context.User.Identity.Name;

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand("SELECT UserID FROM dbo.Users WHERE Email = @Email;", con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                con.Open();

                object result = cmd.ExecuteScalar();
                if (result == null) return -1;

                return Convert.ToInt32(result);
            }
        }

        protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvItems.DataSource = null;
            gvItems.DataBind();
            LoadOrders();
        }

        private void LoadOrders()
        {
            lblMsg.Visible = false;

            int userId = GetCurrentUserId();
            if (userId == -1)
            {
                lblMsg.Visible = true;
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "User account not found.";
                gvOrders.DataSource = null;
                gvOrders.DataBind();
                return;
            }

            string where = "";
            if (ddlFilter.SelectedValue == "YES")
                where = " AND Shipped = 1 ";
            else if (ddlFilter.SelectedValue == "NOT")
                where = " AND Shipped = 0 ";

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand($@"
                SELECT InvoiceID, InvoiceDate, Total, Shipped, ShippedDate
                FROM dbo.Invoice
                WHERE CustomerUserID = @UserID {where}
                ORDER BY InvoiceDate DESC;", con))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);

                con.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);

                    gvOrders.DataSource = dt;
                    gvOrders.DataBind();

                    if (dt.Rows.Count == 0)
                    {
                        lblMsg.Visible = true;
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        lblMsg.Text = "No orders found for the selected filter.";
                    }
                }
            }
        }

        protected void gvOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvOrders.SelectedDataKey == null)
                return;

            int invoiceId = Convert.ToInt32(gvOrders.SelectedDataKey.Value);
            LoadItems(invoiceId);
        }

        private void LoadItems(int invoiceId)
        {
            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                SELECT ProductName, UnitPrice, Quantity, LineTotal
                FROM dbo.InvoiceItem
                WHERE InvoiceID = @InvoiceID
                ORDER BY InvoiceItemID;", con))
            {
                cmd.Parameters.AddWithValue("@InvoiceID", invoiceId);

                con.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);

                    gvItems.DataSource = dt;
                    gvItems.DataBind();
                }
            }
        }
    }
}
