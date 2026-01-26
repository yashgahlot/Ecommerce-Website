using System;
using System.Data.SqlClient;

namespace FormsAuthenticateProject.Customer
{
    public partial class Customer : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Block unauthenticated users from customer area
            if (!Context.Request.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCustomerName();
            }
        }

        private void LoadCustomerName()
        {
            // login sets Identity.Name = Email
            string email = Context.User.Identity.Name;

            // fallback
            lblName.Text = email;

            try
            {
                using (var con = Db.Conn())
                using (var cmd = new SqlCommand(@"
                    SELECT FirstName, LastName
                    FROM dbo.Users
                    WHERE Email = @Email;", con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    con.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            string first = rd["FirstName"].ToString();
                            string last = rd["LastName"].ToString();

                            string full = (first + " " + last).Trim();
                            lblName.Text = string.IsNullOrEmpty(full) ? email : full;
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}
