using System;
using static libraryManagementSystem.Utils.Constants;

namespace libraryManagementSystem.Pages
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[SessionKeys.UserId] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProfile();
            }
        }

        private void LoadProfile()
        {
            string userId = Session[SessionKeys.UserId]?.ToString() ?? "";
            string username = Session[SessionKeys.Username]?.ToString() ?? "";
            string fullName = Session[SessionKeys.FullName]?.ToString() ?? "";
            string role = Session[SessionKeys.Role]?.ToString() ?? "";

            litFullName.Text = fullName;
            litUsername.Text = username;
            litRole.Text = role;
            litInitials.Text = GetInitials(fullName);

            litDetailFullName.Text = fullName;
            litDetailUsername.Text = username;
            litDetailRole.Text = role;
            litDetailUserId.Text = userId;
        }

        private string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return "?";

            string[] parts = fullName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
                return parts[0].Substring(0, 1).ToUpper();

            return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpper();
        }
    }
}