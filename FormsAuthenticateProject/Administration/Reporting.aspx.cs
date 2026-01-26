using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace FormsAuthenticateProject.Administration
{
    public partial class Reporting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtTo.Text = DateTime.Today.ToString("yyyy-MM-dd");
                txtFrom.Text = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            gvItems.DataSource = null;
            gvItems.DataBind();

            if (!DateTime.TryParse(txtFrom.Text, out DateTime fromDate) ||
                !DateTime.TryParse(txtTo.Text, out DateTime toDate))
            {
                ShowError("Enter valid From and To dates.");
                return;
            }

            LoadInvoices(fromDate, toDate);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = false;
            lblSummary.Text = "";

            gvInvoices.DataSource = null;
            gvInvoices.DataBind();

            gvItems.DataSource = null;
            gvItems.DataBind();

            txtTo.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtFrom.Text = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
        }

        private void LoadInvoices(DateTime fromDate, DateTime toDate)
        {
            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                SELECT InvoiceID, CustomerUserID, InvoiceDate, SubTotal, Tax, Total, Shipped, ShippedDate
                FROM dbo.Invoice
                WHERE InvoiceDate >= @FromDate AND InvoiceDate < @ToDatePlus1
                ORDER BY InvoiceDate DESC;", con))
            {
                cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                cmd.Parameters.AddWithValue("@ToDatePlus1", toDate.Date.AddDays(1));

                con.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);

                    gvInvoices.DataSource = dt;
                    gvInvoices.DataBind();

                    // Summary
                    decimal revenue = 0;
                    foreach (DataRow row in dt.Rows)
                        revenue += Convert.ToDecimal(row["Total"]);

                    lblSummary.Text = $"Invoices: {dt.Rows.Count} | Total Revenue: {revenue:C}";
                }
            }
        }

        protected void gvInvoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            int invoiceId = Convert.ToInt32(gvInvoices.SelectedDataKey.Value);
            LoadInvoiceItems(invoiceId);
        }

        private void LoadInvoiceItems(int invoiceId)
        {
            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                SELECT ProductName, UnitPrice, Quantity, LineTotal
                FROM dbo.InvoiceItem
                WHERE InvoiceID = @InvoiceID;", con))
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

        private void ShowError(string msg)
        {
            lblMsg.Visible = true;
            lblMsg.Text = msg;
        }
    }
}
