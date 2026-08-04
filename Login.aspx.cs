using libraryManagementSystem.Services;
using libraryManagementSystem.Utils;
using System;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace libraryManagementSystem
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly UserService _userService = new UserService();

        protected void Page_Load(object sender, EventArgs e)
        {
            // If already logged in, skip straight to the dashboard instead of
            // showing the login form again.
            if (!IsPostBack && SessionManager.IsLoggedIn)
            {
                Response.Redirect("~/Pages/Dashboard.aspx", true);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
           
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                var user = _userService.Authenticate(txtUsername.Text.Trim(), txtPassword.Text);

                if (user == null)
                {
                    ShowError("Invalid username or password.");
                    return;
                }

                SessionManager.CreateSession(user.UserId, user.Username, user.FullName, user.Role);
                Response.Redirect("~/Pages/Dashboard.aspx", true);
            }
            catch (Exception)
            {
              
                ShowError("Something went wrong while signing in. Please try again.");
            }
        }

        private void ShowError(string message)
        {
            litError.Text = message;
            pnlError.Visible = true;
        }
    }
}