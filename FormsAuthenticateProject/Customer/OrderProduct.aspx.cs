using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace FormsAuthenticateProject.Customer
{
    public partial class OrderProduct : System.Web.UI.Page
    {
        private const decimal TAX_RATE = 0.13m;

        private decimal SelectedPrice
        {
            get => (ViewState["SelectedPrice"] == null) ? 0m : (decimal)ViewState["SelectedPrice"];
            set => ViewState["SelectedPrice"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            // always show logged in user (even on postbacks)
            lblLoggedIn.Text = "Logged in as: " + Context.User.Identity.Name;

            if (!IsPostBack)
            {
                LoadSuppliers();
                LoadCategoriesEmpty();
                LoadProductsEmpty();

                LoadCardTypes();
                LoadExpMonths();
                LoadExpYears();
                LoadStreetTypes();

                ResetProductDisplay();
                UpdateTotals();
            }
        }

        private void LoadSuppliers()
        {
            ddlSupplier.Items.Clear();
            ddlSupplier.Items.Add(new ListItem("-- Select --", ""));

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
                        ddlSupplier.Items.Add(new ListItem(
                            rd["CompanyName"].ToString(),
                            rd["SupplierID"].ToString()
                        ));
                    }
                }
            }
        }

        private void LoadCategoriesEmpty()
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("-- Select supplier first --", ""));
        }

        private void LoadProductsEmpty()
        {
            ddlProduct.Items.Clear();
            ddlProduct.Items.Add(new ListItem("-- Select supplier & category first --", ""));
        }

        private void LoadCategoriesForSupplier()
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Add(new ListItem("-- Select --", ""));

            if (string.IsNullOrEmpty(ddlSupplier.SelectedValue))
                return;

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                SELECT DISTINCT c.CategoryID, c.Description
                FROM dbo.Products p
                INNER JOIN dbo.Category c ON p.CategoryID = c.CategoryID
                WHERE p.Status = 1
                  AND c.Status = 1
                  AND p.SupplierID = @SupplierID
                ORDER BY c.Description;", con))
            {
                cmd.Parameters.AddWithValue("@SupplierID", ddlSupplier.SelectedValue);

                con.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        ddlCategory.Items.Add(new ListItem(
                            rd["Description"].ToString(),
                            rd["CategoryID"].ToString()
                        ));
                    }
                }
            }
        }

        private void LoadProductsForSupplierCategory()
        {
            ddlProduct.Items.Clear();
            ddlProduct.Items.Add(new ListItem("-- Select --", ""));

            if (string.IsNullOrEmpty(ddlSupplier.SelectedValue) || string.IsNullOrEmpty(ddlCategory.SelectedValue))
                return;

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                SELECT ProductID, ProductName
                FROM dbo.Products
                WHERE Status = 1
                  AND SupplierID = @SupplierID
                  AND CategoryID = @CategoryID
                ORDER BY ProductName;", con))
            {
                cmd.Parameters.AddWithValue("@SupplierID", ddlSupplier.SelectedValue);
                cmd.Parameters.AddWithValue("@CategoryID", ddlCategory.SelectedValue);

                con.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        ddlProduct.Items.Add(new ListItem(
                            rd["ProductName"].ToString(),
                            rd["ProductID"].ToString()
                        ));
                    }
                }
            }
        }

        private void LoadCardTypes()
        {
            ddlCardType.Items.Clear();
            ddlCardType.Items.Add(new ListItem("-- Select --", ""));
            ddlCardType.Items.Add(new ListItem("Visa", "Visa"));
            ddlCardType.Items.Add(new ListItem("MasterCard", "MasterCard"));
            ddlCardType.Items.Add(new ListItem("American Express", "American Express"));
        }

        private void LoadExpMonths()
        {
            ddlExpMonth.Items.Clear();
            ddlExpMonth.Items.Add(new ListItem("-- Select --", ""));

            for (int m = 1; m <= 12; m++)
                ddlExpMonth.Items.Add(new ListItem(m.ToString("00"), m.ToString()));
        }

        private void LoadExpYears()
        {
            ddlExpYear.Items.Clear();
            ddlExpYear.Items.Add(new ListItem("-- Select --", ""));

            int year = DateTime.Now.Year;
            for (int y = year; y <= year + 15; y++)
                ddlExpYear.Items.Add(new ListItem(y.ToString(), y.ToString()));
        }

        private void LoadStreetTypes()
        {
            ddlStreetType.Items.Clear();
            ddlStreetType.Items.Add(new ListItem("-- Select --", ""));
            ddlStreetType.Items.Add(new ListItem("Street", "Street"));
            ddlStreetType.Items.Add(new ListItem("Avenue", "Avenue"));
            ddlStreetType.Items.Add(new ListItem("Court", "Court"));
            ddlStreetType.Items.Add(new ListItem("Circle", "Circle"));
        }

        protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearMessages();

            LoadCategoriesForSupplier();
            LoadProductsEmpty();

            ResetProductDisplay();
            UpdateTotals();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearMessages();

            LoadProductsForSupplierCategory();
            ResetProductDisplay();
            UpdateTotals();
        }

        protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearMessages();

            LoadSelectedProductDetails();
            UpdateTotals();
        }

        protected void txtQty_TextChanged(object sender, EventArgs e)
        {
            ClearMessages();
            UpdateTotals();
        }

        private void ResetProductDisplay()
        {
            lblDescription.Text = "-";
            lblPrice.Text = "$0.00";
            SelectedPrice = 0m;
        }

        private void LoadSelectedProductDetails()
        {
            ResetProductDisplay();

            if (string.IsNullOrEmpty(ddlProduct.SelectedValue))
                return;

            int productId = int.Parse(ddlProduct.SelectedValue);

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                SELECT ProductName, RetailPrice
                FROM dbo.Products
                WHERE ProductID = @ProductID AND Status = 1;", con))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);
                con.Open();

                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        // Your Products table has no Description column,
                        // so we display ProductName as the "description"
                        lblDescription.Text = rd["ProductName"].ToString();

                        decimal p = Convert.ToDecimal(rd["RetailPrice"]);
                        SelectedPrice = p;
                        lblPrice.Text = p.ToString("C");
                    }
                }
            }
        }

        private void UpdateTotals()
        {
            int qty = 0;
            int.TryParse((txtQty.Text ?? "").Trim(), out qty);
            if (qty < 0) qty = 0;

            decimal subtotal = SelectedPrice * qty;
            decimal tax = subtotal * TAX_RATE;
            decimal total = subtotal + tax;

            lblTotals.Text = $"SubTotal: {subtotal:C} | Tax: {tax:C} | Total: {total:C}";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ClearMessages();
            lblMsg.Visible = true;
            lblMsg.ForeColor = System.Drawing.Color.Red;

            // basic check (validators will also fire)
            if (string.IsNullOrEmpty(ddlSupplier.SelectedValue) ||
                string.IsNullOrEmpty(ddlCategory.SelectedValue) ||
                string.IsNullOrEmpty(ddlProduct.SelectedValue))
            {
                lblMsg.Text = "Select supplier, category, and product.";
                return;
            }

            if (!int.TryParse((txtQty.Text ?? "").Trim(), out int qty) || qty <= 0)
            {
                lblMsg.Text = "Enter a valid quantity.";
                return;
            }

            // load product details (safe)
            int productId = int.Parse(ddlProduct.SelectedValue);
            string productName;
            decimal unitPrice;

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                SELECT ProductName, RetailPrice
                FROM dbo.Products
                WHERE ProductID = @ProductID AND Status = 1;", con))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);
                con.Open();

                using (var rd = cmd.ExecuteReader())
                {
                    if (!rd.Read())
                    {
                        lblMsg.Text = "Selected product not found or inactive.";
                        return;
                    }

                    productName = rd["ProductName"].ToString();
                    unitPrice = Convert.ToDecimal(rd["RetailPrice"]);
                }
            }

            decimal subtotal = unitPrice * qty;
            decimal tax = subtotal * TAX_RATE;
            decimal total = subtotal + tax;

            // get logged-in customer user id
            string email = Context.User.Identity.Name;
            int userId;

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand("SELECT UserID FROM dbo.Users WHERE Email = @Email;", con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                con.Open();

                object result = cmd.ExecuteScalar();
                if (result == null)
                {
                    lblMsg.Text = "User not found.";
                    return;
                }
                userId = Convert.ToInt32(result);
            }

            // save invoice + ONE invoice item
            using (var con = Db.Conn())
            {
                con.Open();
                using (var tx = con.BeginTransaction())
                {
                    try
                    {
                        int invoiceId;

                        using (var cmdInv = new SqlCommand(@"
                            INSERT INTO dbo.Invoice
                            (
                                CustomerUserID, InvoiceDate, SubTotal, Tax, Total, Shipped, ShippedDate,
                                CardHolderName, CardType, CardNumber, SecurityCode, ExpMonth, ExpYear,
                                HouseNumber, StreetName, StreetType, ApartmentNumber, City, Province, PostalCode
                            )
                            VALUES
                            (
                                @CustomerUserID, GETDATE(), @SubTotal, @Tax, @Total, 0, NULL,
                                @CardHolderName, @CardType, @CardNumber, @SecurityCode, @ExpMonth, @ExpYear,
                                @HouseNumber, @StreetName, @StreetType, @ApartmentNumber, @City, @Province, @PostalCode
                            );
                            SELECT SCOPE_IDENTITY();", con, tx))
                        {
                            cmdInv.Parameters.AddWithValue("@CustomerUserID", userId);
                            cmdInv.Parameters.AddWithValue("@SubTotal", subtotal);
                            cmdInv.Parameters.AddWithValue("@Tax", tax);
                            cmdInv.Parameters.AddWithValue("@Total", total);

                            cmdInv.Parameters.AddWithValue("@CardHolderName", txtCardHolderName.Text.Trim());
                            cmdInv.Parameters.AddWithValue("@CardType", ddlCardType.SelectedValue);
                            cmdInv.Parameters.AddWithValue("@CardNumber", txtCardNumber.Text.Trim());
                            cmdInv.Parameters.AddWithValue("@SecurityCode", txtSecurityCode.Text.Trim());
                            cmdInv.Parameters.AddWithValue("@ExpMonth", int.Parse(ddlExpMonth.SelectedValue));
                            cmdInv.Parameters.AddWithValue("@ExpYear", int.Parse(ddlExpYear.SelectedValue));

                            cmdInv.Parameters.AddWithValue("@HouseNumber", txtHouseNumber.Text.Trim());
                            cmdInv.Parameters.AddWithValue("@StreetName", txtStreetName.Text.Trim());
                            cmdInv.Parameters.AddWithValue("@StreetType", ddlStreetType.SelectedValue);
                            cmdInv.Parameters.AddWithValue("@ApartmentNumber", txtApartment.Text.Trim());
                            cmdInv.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                            cmdInv.Parameters.AddWithValue("@Province", txtProvince.Text.Trim());
                            cmdInv.Parameters.AddWithValue("@PostalCode", txtPostalCode.Text.Trim());

                            invoiceId = Convert.ToInt32(Convert.ToDecimal(cmdInv.ExecuteScalar()));
                        }

                        // IMPORTANT: Do NOT insert LineTotal if your DB has it computed
                        using (var cmdItem = new SqlCommand(@"
                            INSERT INTO dbo.InvoiceItem (InvoiceID, ProductName, UnitPrice, Quantity)
                            VALUES (@InvoiceID, @ProductName, @UnitPrice, @Quantity);", con, tx))
                        {
                            cmdItem.Parameters.AddWithValue("@InvoiceID", invoiceId);
                            cmdItem.Parameters.AddWithValue("@ProductName", productName);
                            cmdItem.Parameters.AddWithValue("@UnitPrice", unitPrice);
                            cmdItem.Parameters.AddWithValue("@Quantity", qty);

                            cmdItem.ExecuteNonQuery();
                        }

                        tx.Commit();

                        lblMsg.ForeColor = System.Drawing.Color.Green;
                        lblMsg.Text = $"Order saved successfully. Invoice #{invoiceId}";

                        // reset form but keep dropdown lists
                        ClearFormKeepLists();
                        ResetProductDisplay();
                        UpdateTotals();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        lblMsg.Text = "Order failed: " + ex.Message;
                    }
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearMessages();
            ClearFormKeepLists();
            ResetProductDisplay();
            UpdateTotals();
        }

        private void ClearMessages()
        {
            lblMsg.Visible = false;
            lblMsg.Text = "";
        }

        private void ClearFormKeepLists()
        {
            // selection
            ddlSupplier.SelectedIndex = 0;
            LoadCategoriesEmpty();
            LoadProductsEmpty();

            txtQty.Text = "";

            // billing
            txtCardHolderName.Text = "";
            ddlCardType.SelectedIndex = 0;
            txtCardNumber.Text = "";
            txtSecurityCode.Text = "";
            ddlExpMonth.SelectedIndex = 0;
            ddlExpYear.SelectedIndex = 0;

            // delivery
            txtHouseNumber.Text = "";
            txtStreetName.Text = "";
            ddlStreetType.SelectedIndex = 0;
            txtApartment.Text = "";
            txtCity.Text = "";
            txtProvince.Text = "";
            txtPostalCode.Text = "";
        }
    }
}
