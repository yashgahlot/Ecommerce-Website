using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace FormsAuthenticateProject.Administration
{
    public partial class Admin : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Context?.User?.Identity?.IsAuthenticated == true)
                    lblName.Text = "Signed in as: " + Context.User.Identity.Name;
                else
                    lblName.Text = "";
            }
        }

        protected void lnkSignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            cookie.Path = FormsAuthentication.FormsCookiePath;
            Response.Cookies.Add(cookie);

            Response.Redirect("~/Account/Login.aspx");
        }
    }
}
