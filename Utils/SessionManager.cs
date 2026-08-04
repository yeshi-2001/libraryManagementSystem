using System;
using System.Web;

namespace libraryManagementSystem.Utils
{
    /// <summary>
    /// Wraps ASP.NET Session access for the logged-in user. Pages should never
    /// touch HttpContext.Current.Session directly for auth data — going through
    /// this class means the session keys (and any future change, e.g. switching
    /// to Claims-based auth) only need to change in one place.
    /// </summary>
    public static class SessionManager
    {
        public static void CreateSession(int userId, string username, string fullName, string role)
        {
            HttpContext.Current.Session[Constants.SessionKeys.UserId] = userId;
            HttpContext.Current.Session[Constants.SessionKeys.Username] = username;
            HttpContext.Current.Session[Constants.SessionKeys.FullName] = fullName;
            HttpContext.Current.Session[Constants.SessionKeys.Role] = role;
        }

        public static bool IsLoggedIn
        {
            get { return HttpContext.Current.Session[Constants.SessionKeys.UserId] != null; }
        }

        public static int CurrentUserId
        {
            get
            {
                object value = HttpContext.Current.Session[Constants.SessionKeys.UserId];
                return value == null ? 0 : Convert.ToInt32(value);
            }
        }

        public static string CurrentUsername
        {
            get { return HttpContext.Current.Session[Constants.SessionKeys.Username] as string; }
        }

        public static string CurrentFullName
        {
            get { return HttpContext.Current.Session[Constants.SessionKeys.FullName] as string; }
        }

        public static string CurrentRole
        {
            get { return HttpContext.Current.Session[Constants.SessionKeys.Role] as string; }
        }

        public static bool IsAdmin
        {
            get { return CurrentRole == Constants.Roles.Admin; }
        }

        /// <summary>Clears all session data and effectively logs the user out.</summary>
        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        /// <summary>
        /// Call at the top of every protected page's Page_Load (before IsPostBack
        /// check) to redirect unauthenticated users back to Login.aspx.
        /// </summary>
        public static void RequireLogin()
        {
            if (!IsLoggedIn)
            {
                HttpContext.Current.Response.Redirect("~/Login.aspx", true);
            }
        }

        /// <summary>Call on Admin-only pages (e.g. Settings, User management) after RequireLogin().</summary>
        public static void RequireAdmin()
        {
            RequireLogin();
            if (!IsAdmin)
            {
                HttpContext.Current.Response.Redirect("~/Pages/Dashboard.aspx", true);
            }
        }
    }
}