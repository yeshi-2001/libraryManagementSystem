namespace libraryManagementSystem.Utils
{
    /// <summary>
    /// Centralizes "magic strings" used across the app (roles, statuses, session
    /// keys, business rule numbers) so they're defined once and referenced
    /// everywhere else — avoids typos like "Admin" vs "admin" scattered across
    /// 20 .aspx.cs files.
    /// </summary>
    public static class Constants
    {
        // ---------------- User Roles ----------------
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Staff = "Staff";
        }

        // ---------------- Book Status ----------------
        public static class BookStatus
        {
            public const string Active = "Active";
            public const string Inactive = "Inactive";
        }

        // ---------------- Member Status ----------------
        public static class MemberStatus
        {
            public const string Active = "Active";
            public const string Inactive = "Inactive";
        }

        // ---------------- Borrow Status ----------------
        public static class BorrowStatus
        {
            public const string Borrowed = "Borrowed";
            public const string Returned = "Returned";
            public const string Overdue = "Overdue";
        }

        // ---------------- Session Keys ----------------
        public static class SessionKeys
        {
            public const string UserId = "UserId";
            public const string Username = "Username";
            public const string FullName = "FullName";
            public const string Role = "Role";
        }

        // ---------------- Business Rules ----------------
        public static class BorrowRules
        {
            /// <summary>Default loan period in days when a book is borrowed.</summary>
            public const int LoanPeriodDays = 14;

            /// <summary>Fine charged per day overdue.</summary>
            public const decimal FinePerDay = 10.00m;

            /// <summary>Maximum number of books a member can have borrowed at once.</summary>
            public const int MaxBorrowLimit = 2;
        }

        // ---------------- Pagination ----------------
        public static class Pagination
        {
            public const int DefaultPageSize = 10;
        }
    }
}