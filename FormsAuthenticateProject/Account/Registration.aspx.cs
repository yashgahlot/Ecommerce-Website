using System;
using System.Data.SqlClient;

namespace FormsAuthenticateProject.Account
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadSecretQuestions();
        }

        private void LoadSecretQuestions()
        {
            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(
                "SELECT SecretQuestionID, QuestionText FROM dbo.SecretQuestions WHERE Status=1 ORDER BY QuestionText", con))
            {
                con.Open();
                dlSecretQuestion1.DataSource = cmd.ExecuteReader();
                dlSecretQuestion1.DataTextField = "QuestionText";
                dlSecretQuestion1.DataValueField = "SecretQuestionID";
                dlSecretQuestion1.DataBind();
            }
        }
 

        protected void btnCreateProfile_Click(object sender, EventArgs e)
        {
            lblMsg.Visible = true;
            lblMsg.Text = "Clicked!";
            if (!Page.IsValid) return;

            string first = txtFirstName.Text.Trim();
            string last = txtLastName.Text.Trim();
            string email = txtEmailAddress.Text.Trim().ToLowerInvariant();
            string phone = txtPhoneNumber.Text.Trim();
            string password = txtPassword.Text; // do NOT trim passwords
            int secretQId = int.Parse(dlSecretQuestion1.SelectedValue);
            string secretAnswer = txtSecretAnswer1.Text.Trim();

            // Check email unique
            using (var con = Db.Conn())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.Users WHERE Email=@Email", con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                con.Open();
                int exists = (int)cmd.ExecuteScalar();
                if (exists > 0)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "That email is already registered. Please use another one.";
                    return;
                }
            }

            // Get Customer RoleID
            int roleIdCustomer;
            using (var con = Db.Conn())
            using (var cmd = new SqlCommand("SELECT RoleID FROM dbo.Roles WHERE [Description]='Customer' AND Status=1", con))
            {
                con.Open();
                object val = cmd.ExecuteScalar();
                if (val == null)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Customer role not found in DB.";
                    return;
                }
                roleIdCustomer = Convert.ToInt32(val);
            }

            // Hash password + secret answer
            byte[] pwdSalt = Crypto.CreateSalt(16);
            byte[] pwdHash = Crypto.Sha256(password, pwdSalt);

            byte[] ansSalt = Crypto.CreateSalt(16);
            byte[] ansHash = Crypto.Sha256(secretAnswer, ansSalt);

            // Insert user
            using (var con = Db.Conn())
            using (var cmd = new SqlCommand(@"
                INSERT INTO dbo.Users
    (FirstName, LastName, Email, Phone, RoleID, Status,
     PasswordSalt, PasswordHash,
     SecretQuestionID, SecretAnswerSalt, SecretAnswerHash, CreatedAt)
    VALUES
    (@First, @Last, @Email, @Phone, @RoleID, 1,
     @PwdSalt, @PwdHash,
     @SQ, @AnsSalt, @AnsHash, GETDATE())", con))
            {
                cmd.Parameters.AddWithValue("@First", first);
                cmd.Parameters.AddWithValue("@Last", last);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@RoleID", roleIdCustomer);
                cmd.Parameters.AddWithValue("@SQ", secretQId);
                cmd.Parameters.AddWithValue("@PwdSalt", pwdSalt);
                cmd.Parameters.AddWithValue("@PwdHash", pwdHash);
                cmd.Parameters.AddWithValue("@AnsSalt", ansSalt);
                cmd.Parameters.AddWithValue("@AnsHash", ansHash);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("~/Account/Login.aspx?created=1", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/Login.aspx");
        }
    }
}
