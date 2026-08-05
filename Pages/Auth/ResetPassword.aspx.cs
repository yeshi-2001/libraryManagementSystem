using libraryManagementSystem.Utils;
using System;
using System.Data;
using System.Data.SqlClient;

namespace libraryManagementSystem.Auth
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        private string Token => Request.QueryString["token"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(Token))
                {
                    ShowError("This reset link is invalid.");
                    pnlForm.Visible = false;
                    return;
                }

                if (!TokenIsValid(Token))
                {
                    ShowError("This reset link is invalid or has expired. Please request a new one.");
                    pnlForm.Visible = false;
                }
            }
        }

        private bool TokenIsValid(string token)
        {
            object result = Database.ExecuteScalar(
                @"SELECT COUNT(1) FROM PasswordResetTokens
                  WHERE Token = @Token AND IsUsed = 0 AND ExpiresOn > @Now",
                CommandType.Text,
                new SqlParameter("@Token", token),
                new SqlParameter("@Now", DateTime.Now));

            return Convert.ToInt32(result) > 0;
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (string.IsNullOrEmpty(Token) || !TokenIsValid(Token))
            {
                ShowError("This reset link is invalid or has expired. Please request a new one.");
                pnlForm.Visible = false;
                return;
            }

            try
            {
                object userIdObj = Database.ExecuteScalar(
                    "SELECT UserId FROM PasswordResetTokens WHERE Token = @Token",
                    CommandType.Text,
                    new SqlParameter("@Token", Token));

                int userId = Convert.ToInt32(userIdObj);
                string newPasswordHash = PasswordHelper.HashPassword(txtNewPassword.Text);

                Database.ExecuteNonQuery(
                    "UPDATE Users SET Password = @Password WHERE UserId = @UserId",
                    CommandType.Text,
                    new SqlParameter("@Password", newPasswordHash),
                    new SqlParameter("@UserId", userId));

                Database.ExecuteNonQuery(
                    "UPDATE PasswordResetTokens SET IsUsed = 1 WHERE Token = @Token",
                    CommandType.Text,
                    new SqlParameter("@Token", Token));

                pnlForm.Visible = false;
                pnlError.Visible = false;
                litSuccess.Text = "Your password has been reset successfully.";
                pnlSuccess.Visible = true;
            }
            catch (Exception ex)
            {
                ShowError("Something went wrong while resetting your password. Please try again.");
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
        }

        private void ShowError(string message)
        {
            litError.Text = message;
            pnlError.Visible = true;
        }
    }
}