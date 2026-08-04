using libraryManagementSystem.Utils;
using System;
using System.Web.SessionState;

namespace libraryManagementSystem
{
   
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
      
            SessionManager.RequireLogin();

            phAdminNav.Visible = SessionManager.IsAdmin;

            litFullName.Text = System.Web.HttpUtility.HtmlEncode(SessionManager.CurrentFullName);
            litRole.Text = System.Web.HttpUtility.HtmlEncode(SessionManager.CurrentRole);
            litUserInitials.Text = GetInitials(SessionManager.CurrentFullName);

            HighlightActiveNavLink();
            ShowQueuedToastIfAny();
        }

        private void ShowQueuedToastIfAny()
        {
            var toast = ToastHelper.ConsumeQueuedToast();
            if (!string.IsNullOrEmpty(toast.Message))
            {
                hfToastMessage.Value = toast.Message;
                hfToastType.Value = string.IsNullOrEmpty(toast.Type) ? "success" : toast.Type;
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            SessionManager.ClearSession();
            Response.Redirect("~/Login.aspx", true);
        }

        /// <summary>Builds a 1-2 letter avatar badge (e.g. "Nimal Perera" -> "NP") from the full name.</summary>
        private string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return "?";
            }

            string[] parts = fullName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
            {
                return parts[0].Substring(0, 1).ToUpper();
            }
            return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpper();
        }


        private void HighlightActiveNavLink()
        {
            string currentPath = Request.Url.AbsolutePath;

            var navLinks = new[]
            {
                navDashboard, navBooks, navCategories, navMembers,
                navBorrow, navReturn, navBorrowHistory, navReports,
                navUsers, navSettings, navAudit
            };

            foreach (var link in navLinks)
            {
                if (link == null) continue;

                string linkPath = ResolveUrl(link.HRef);
                if (currentPath.EndsWith(linkPath.TrimStart('~'), StringComparison.OrdinalIgnoreCase))
                {
                    link.Attributes["class"] = (link.Attributes["class"] + " active").Trim();
                }
            }
        }
    }
}