using libraryManagementSystem.Utils;
using System;
using System.Data;
using System.Data.SqlClient;

namespace libraryManagementSystem.Auth
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSendReset_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string username = txtEmail.Text.Trim();

            try
            {
                object userIdObj = Database.ExecuteScalar(
                    "SELECT UserId FROM Users WHERE Username = @Username",
                    CommandType.Text,
                    new SqlParameter("@Username", username));

                if (userIdObj == null)
                {
                    ShowGenericConfirmation();
                    return;
                }

                int userId = Convert.ToInt32(userIdObj);
                string token = GenerateToken();
                DateTime expiresOn = DateTime.Now.AddMinutes(30);

                Database.ExecuteNonQuery(
                    @"INSERT INTO PasswordResetTokens (UserId, Token, ExpiresOn, IsUsed)
                      VALUES (@UserId, @Token, @ExpiresOn, 0)",
                    CommandType.Text,
                    new SqlParameter("@UserId", userId),
                    new SqlParameter("@Token", token),
                    new SqlParameter("@ExpiresOn", expiresOn));

                string resetUrl = ResolveUrl("~/Pages/Auth/ResetPassword.aspx") + "?token=" + Uri.EscapeDataString(token);

                pnlForm.Visible = false;
                pnlError.Visible = false;
                hlResetLink.HRef = resetUrl;
                hlResetLink.InnerText = Request.Url.GetLeftPart(UriPartial.Authority) + resetUrl;
                pnlResetLink.Visible = true;
            }
            catch (Exception ex)
            {
                ShowError("Something went wrong. Please try again.");
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
        }

        private void ShowGenericConfirmation()
        {
            pnlForm.Visible = false;
            pnlError.Visible = false;
            pnlResetLink.Visible = true;
            hlResetLink.Visible = false;
            pnlResetLink.Controls.Clear();
            pnlResetLink.Controls.Add(new System.Web.UI.LiteralControl(
                "If that email is registered, a reset link has been generated for it."));
        }

        private string GenerateToken()
        {
            byte[] bytes = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        private void ShowError(string message)
        {
            litError.Text = message;
            pnlError.Visible = true;
        }
    }
}