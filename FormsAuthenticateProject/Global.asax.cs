using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace FormsAuthenticateProject
{
    public class Global : HttpApplication
    {
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            if (Context?.User?.Identity == null) return;

            var formsId = Context.User.Identity as FormsIdentity;
            if (formsId == null) return;

            var ticket = formsId.Ticket;
            if (ticket == null) return;

            string[] roles = string.IsNullOrWhiteSpace(ticket.UserData)
                ? new string[0]
                : ticket.UserData.Split(',');

            Context.User = new GenericPrincipal(formsId, roles);
        }
    }
}
