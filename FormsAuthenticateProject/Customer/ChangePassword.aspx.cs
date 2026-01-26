using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace FormsAuthenticateProject.Customer
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Customer/default.aspx");
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = true;
            lblMsg.ForeColor = System.Drawing.Color.Red;

            string currentPwd = txtCurrent.Text.Trim();
            string newPwd = txtNew.Text.Trim();

            if (!IsStrongEnough(newPwd))
            {
                lblMsg.Text = "Password must be at least 8 characters and contain a letter and a number.";
                return;
            }

            // logged in username = email
            string email = Context.User.Identity.Name;

            int userId;
            byte[] salt;
            byte[] hash;

            // Load current hash + salt
            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                SELECT UserID, PasswordSalt, PasswordHash
                FROM dbo.Users
                WHERE Email = @Email;", con))
            {
                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (!rd.Read())
                    {
                        lblMsg.Text = "Account not found.";
                        return;
                    }

                    userId = Convert.ToInt32(rd["UserID"]);
                    salt = (byte[])rd["PasswordSalt"];
                    hash = (byte[])rd["PasswordHash"];
                }
            }

            // Verify current password
            byte[] enteredHash = Crypto.Sha256(currentPwd, salt);

            if (!Crypto.AreEqual(enteredHash, hash))
            {
                lblMsg.Text = "Current password is incorrect.";
                return;
            }

            // Create new salt + hash
            byte[] newSalt = Crypto.CreateSalt(16);
            byte[] newHash = Crypto.Sha256(newPwd, newSalt);

            // Update DB
            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                UPDATE dbo.Users
                SET PasswordSalt = @Salt,
                    PasswordHash = @Hash
                WHERE UserID = @UserID;", con))
            {
                cmd.Parameters.AddWithValue("@Salt", newSalt);
                cmd.Parameters.AddWithValue("@Hash", newHash);
                cmd.Parameters.AddWithValue("@UserID", userId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Password updated successfully.";
        }

        private bool IsStrongEnough(string pwd)
        {
            if (pwd.Length < 8) return false;

            bool hasLetter = Regex.IsMatch(pwd, "[A-Za-z]");
            bool hasNumber = Regex.IsMatch(pwd, "[0-9]");

            return hasLetter && hasNumber;
        }
    }
}
