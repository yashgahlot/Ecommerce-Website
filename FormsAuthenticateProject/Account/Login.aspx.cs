using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;

namespace FormsAuthenticateProject.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim().ToLowerInvariant();
            string password = txtPassword.Text;

            int userId;
            string role;
            byte[] salt;
            byte[] hash;

            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
        SELECT u.UserID, u.PasswordSalt, u.PasswordHash, r.[Description] AS RoleDesc
        FROM dbo.Users u
        INNER JOIN dbo.Roles r ON u.RoleID = r.RoleID
        WHERE u.Email=@Email AND u.Status=1 AND r.Status=1", con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                con.Open();

                using (var rd = cmd.ExecuteReader())
                {
                    if (!rd.Read())
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "User not found (email/DB).";
                        return;
                    }

                    userId = Convert.ToInt32(rd["UserID"]);
                    role = rd["RoleDesc"].ToString();
                    salt = (byte[])rd["PasswordSalt"];
                    hash = (byte[])rd["PasswordHash"];
                }
            }

            byte[] enteredHash = Crypto.Sha256(password, salt);
            if (!Crypto.AreEqual(enteredHash, hash))
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Password mismatch.";
                return;
            }

            var ticket = new FormsAuthenticationTicket(
                1, email, DateTime.Now, DateTime.Now.AddMinutes(60),
                false, role);

            string encTicket = FormsAuthentication.Encrypt(ticket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            authCookie.HttpOnly = true;
            authCookie.Path = FormsAuthentication.FormsCookiePath; // usually "/"
            Response.Cookies.Add(authCookie);


            if (role == "Administration")
                Response.Redirect("~/Administration/default.aspx");
            else if (role == "Shipping")
                Response.Redirect("~/Shipping/default.aspx");
            else
                Response.Redirect("~/Customer/default.aspx");
        }

    }
}
