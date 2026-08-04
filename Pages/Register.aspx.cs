using libraryManagementSystem.Models;
using libraryManagementSystem.Services;
using System;

namespace libraryManagementSystem.Pages
{
    public partial class Register : System.Web.UI.Page
    {
        private readonly UserService _userService = new UserService();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var user = new User
                {
                    FullName = txtFullName.Text.Trim(),
                    Username = txtUsername.Text.Trim(),
                    Role = "Staff"
                };

                _userService.AddUser(user, txtPassword.Text);

                pnlSuccess.Visible = true;
                litSuccess.Text = "Account created successfully. <a href='../Login.aspx'>Sign in</a>";
                pnlError.Visible = false;
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                litError.Text = ex.Message;
                pnlSuccess.Visible = false;
            }
        }
    }
}
