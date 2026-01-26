using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace FormsAuthenticateProject.Administration
{
    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSuppliers();
                LoadCategories();
                ClearForm();
                gvProducts.Visible = false; // no products initially
            }
        }

        private void LoadSuppliers()
        {
            ddlSupplier.Items.Clear();
            ddlSupplier.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select --", ""));

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                SELECT SupplierID, CompanyName
                FROM dbo.Suppliers
                WHERE Status = 1
                ORDER BY CompanyName;", con))
            {
                con.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        ddlSupplier.Items.Add(new System.Web.UI.WebControls.ListItem(
                            rd["CompanyName"].ToString(),
                            rd["SupplierID"].ToString()
                        ));
                    }
                }
            }
        }

        private void LoadCategories()
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select --", ""));

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                SELECT CategoryID, Description
                FROM dbo.Category
                WHERE Status = 1
                ORDER BY Description;", con))
            {
                con.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        ddlCategory.Items.Add(new System.Web.UI.WebControls.ListItem(
                            rd["Description"].ToString(),
                            rd["CategoryID"].ToString()
                        ));
                    }
                }
            }
        }

        protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearForm(keepSupplier: true);

            if (string.IsNullOrEmpty(ddlSupplier.SelectedValue))
            {
                gvProducts.Visible = false;
                gvProducts.DataSource = null;
                gvProducts.DataBind();
                return;
            }

            LoadProductsGrid();
        }

        private void LoadProductsGrid()
        {
            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                SELECT p.ProductID,
                       p.ProductName,
                       c.Description AS CategoryDesc,
                       p.RetailPrice,
                       p.Status
                FROM dbo.Products p
                INNER JOIN dbo.Category c ON p.CategoryID = c.CategoryID
                WHERE p.SupplierID = @SupplierID
                ORDER BY p.ProductName;", con))
            {
                cmd.Parameters.AddWithValue("@SupplierID", ddlSupplier.SelectedValue);

                con.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);

                    gvProducts.DataSource = dt;
                    gvProducts.DataBind();
                    gvProducts.Visible = true;
                }
            }
        }

        protected void gvProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            int productId = Convert.ToInt32(gvProducts.SelectedDataKey.Value);
            hfProductID.Value = productId.ToString();

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                SELECT ProductName, CategoryID, RetailPrice, Status
                FROM dbo.Products
                WHERE ProductID = @ProductID;", con))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);

                con.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        txtProductName.Text = rd["ProductName"].ToString();
                        ddlCategory.SelectedValue = rd["CategoryID"].ToString();

                        decimal price = Convert.ToDecimal(rd["RetailPrice"]);
                        txtRetailPrice.Text = price.ToString("0.00", CultureInfo.InvariantCulture);

                        // Status may come back as bit => True/False
                        bool active = Convert.ToBoolean(rd["Status"]);
                        ddlStatus.SelectedValue = active ? "1" : "0";
                    }
                }
            }

            btnSave.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = true;

            if (string.IsNullOrEmpty(ddlSupplier.SelectedValue))
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Please select a supplier.";
                return;
            }

            if (!decimal.TryParse(txtRetailPrice.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price))
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Enter a valid retail price.";
                return;
            }

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                IF EXISTS (
                    SELECT 1 FROM dbo.Products
                    WHERE ProductName = @ProductName
                      AND SupplierID = @SupplierID
                      AND CategoryID = @CategoryID
                )
                BEGIN
                    SELECT -1;
                    RETURN;
                END

                INSERT INTO dbo.Products (ProductName, SupplierID, CategoryID, RetailPrice, Status)
                VALUES (@ProductName, @SupplierID, @CategoryID, @RetailPrice, @Status);

                SELECT SCOPE_IDENTITY();", con))
            {
                cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim());
                cmd.Parameters.AddWithValue("@SupplierID", ddlSupplier.SelectedValue);
                cmd.Parameters.AddWithValue("@CategoryID", ddlCategory.SelectedValue);
                cmd.Parameters.AddWithValue("@RetailPrice", price);
                cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue == "1" ? 1 : 0);

                con.Open();
                var result = cmd.ExecuteScalar();

                if (Convert.ToDecimal(result) == -1)
                {
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.Text = "This product already exists for the selected supplier + category.";
                    return;
                }

                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "Product saved.";
            }

            ClearForm(keepSupplier: true);
            LoadProductsGrid();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = true;

            if (string.IsNullOrEmpty(hfProductID.Value))
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Select a product first.";
                return;
            }

            if (!decimal.TryParse(txtRetailPrice.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price))
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Enter a valid retail price.";
                return;
            }

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                UPDATE dbo.Products
                SET ProductName = @ProductName,
                    CategoryID = @CategoryID,
                    RetailPrice = @RetailPrice,
                    Status = @Status
                WHERE ProductID = @ProductID;", con))
            {
                cmd.Parameters.AddWithValue("@ProductID", hfProductID.Value);
                cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim());
                cmd.Parameters.AddWithValue("@CategoryID", ddlCategory.SelectedValue);
                cmd.Parameters.AddWithValue("@RetailPrice", price);
                cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue == "1" ? 1 : 0);

                con.Open();
                cmd.ExecuteNonQuery();

                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "Product updated.";
            }

            ClearForm(keepSupplier: true);
            LoadProductsGrid();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = true;

            if (string.IsNullOrEmpty(hfProductID.Value))
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Select a product first.";
                return;
            }

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                UPDATE dbo.Products
                SET Status = 0
                WHERE ProductID = @ProductID;", con))
            {
                cmd.Parameters.AddWithValue("@ProductID", hfProductID.Value);
                con.Open();
                cmd.ExecuteNonQuery();

                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "Product set to inactive.";
            }

            ClearForm(keepSupplier: true);
            LoadProductsGrid();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm(keepSupplier: true);
            lblMsg.Visible = false;
        }

        private void ClearForm(bool keepSupplier = false)
        {
            hfProductID.Value = "";
            txtProductName.Text = "";
            txtRetailPrice.Text = "";
            ddlStatus.SelectedValue = "1";
            ddlCategory.SelectedIndex = 0;

            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            if (!keepSupplier)
                ddlSupplier.SelectedIndex = 0;
        }
    }
}
