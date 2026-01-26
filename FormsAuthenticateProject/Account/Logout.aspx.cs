using System;
using System.Web;
using System.Web.Security;

namespace FormsAuthenticateProject.Account
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // End auth + session
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            // Expire the auth cookie explicitly 
            var c = new HttpCookie(FormsAuthentication.FormsCookieName, "")
            {
                Expires = DateTime.Now.AddYears(-1),
                Path = FormsAuthentication.FormsCookiePath
            };
            Response.Cookies.Add(c);

            // Prevent cached pages showing after logout (Back button)
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.Now.AddYears(-1));

            Response.Redirect("~/Account/Login.aspx?loggedout=1");
        }
    }
}
