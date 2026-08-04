using System;
using System.Text.RegularExpressions;

namespace libraryManagementSystem.Utils
{
    /// <summary>
    /// Reusable server-side validation checks. ASP.NET validator controls
    /// (RequiredFieldValidator, RegularExpressionValidator, etc.) handle most
    /// client-side + basic server-side validation on the .aspx pages, but
    /// Services also call into these methods directly for rules that need to
    /// run in C# regardless of which page/entry point calls them (defense in
    /// depth — never trust the client alone).
    /// </summary>
    public static class ValidationHelper
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            // Accepts digits, spaces, +, - ; 7 to 15 digits total (covers local + international)
            return Regex.IsMatch(phone, @"^[0-9+\-\s]{7,15}$");
        }

        public static bool IsValidNIC(string nic)
        {
            if (string.IsNullOrWhiteSpace(nic)) return false;
            // Sri Lankan NIC: old format 9 digits + V/X, or new format 12 digits
            return Regex.IsMatch(nic, @"^([0-9]{9}[vVxX]|[0-9]{12})$");
        }

        public static bool IsValidISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn)) return false;
            string cleaned = isbn.Replace("-", "").Replace(" ", "");
            return cleaned.Length == 10 || cleaned.Length == 13;
        }

        public static bool IsNotEmpty(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool IsPositiveInteger(int value)
        {
            return value > 0;
        }

        public static bool IsValidYear(int? year)
        {
            if (!year.HasValue) return true; // optional field
            return year.Value >= 1450 && year.Value <= DateTime.Now.Year;
        }

        /// <summary>
        /// Strips a string down before it's ever used to build dynamic content,
        /// as a defense-in-depth measure against stored XSS in fields like
        /// Book.Title or Member.Address that get echoed back into grids.
        /// (Parameterized SQL already prevents injection at the DB layer —
        /// this is specifically about safe HTML rendering.)
        /// </summary>
        public static string SanitizeInput(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return input.Replace("<", "&lt;").Replace(">", "&gt;").Trim();
        }
    }
}