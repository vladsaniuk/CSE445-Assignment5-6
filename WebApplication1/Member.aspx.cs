using System;
using System.Web.Security;

namespace WebApplication1
{
    public partial class Member : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }

            // Ensure user is a member (not staff) - IsStaff should be false
            if (Session["IsStaff"] == null || Convert.ToBoolean(Session["IsStaff"]))
            {
                // User is authenticated but either role is not set or user is staff
                Response.Redirect("~/Default.aspx");
                return;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Sign out from Forms Authentication
            FormsAuthentication.SignOut();
            
            // Clear all session data
            Session.Clear();
            
            // Redirect to homepage
            Response.Redirect("~/Default.aspx");
        }
    }
}