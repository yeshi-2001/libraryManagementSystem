using System;
using System.Security.Cryptography;

namespace libraryManagementSystem.Utils
{
    /// <summary>
    /// Handles password hashing and verification. Passwords are NEVER stored or
    /// compared as plain text anywhere in the app — Login/UserService only ever
    /// call HashPassword() (when creating/changing a password) or
    /// VerifyPassword() (when checking a login attempt).
    ///
    /// Uses PBKDF2 (Rfc2898DeriveBytes) with a random salt per user, which is the
    /// standard "no external NuGet package needed" approach available in the
    /// full .NET Framework.
    /// </summary>
    public static class PasswordHelper
    {
        private const int SaltSize = 16;      // 128-bit salt
        private const int HashSize = 32;      // 256-bit derived key
        private const int Iterations = 10000; // slows down brute-force attempts

        /// <summary>
        /// Produces a hash string to store in Users.Password. The output format is
        /// "{iterations}.{saltBase64}.{hashBase64}" so VerifyPassword() can rebuild
        /// the exact same computation later without needing separate salt storage.
        /// </summary>
        public static string HashPassword(string plainTextPassword)
        {
            if (string.IsNullOrEmpty(plainTextPassword))
            {
                throw new ArgumentException("Password cannot be empty.", nameof(plainTextPassword));
            }

            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = ComputeHash(plainTextPassword, salt, Iterations);

            return string.Format("{0}.{1}.{2}",
                Iterations,
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash));
        }

        /// <summary>
        /// Compares a login attempt's plain text password against the stored hash.
        /// Returns true only on an exact match.
        /// </summary>
        public static bool VerifyPassword(string plainTextPassword, string storedHash)
        {
            if (string.IsNullOrEmpty(plainTextPassword) || string.IsNullOrEmpty(storedHash))
            {
                return false;
            }

            string[] parts = storedHash.Split('.');
            if (parts.Length != 3)
            {
                return false; // malformed/legacy hash — treat as no match
            }

            int iterations = int.Parse(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] storedSubHash = Convert.FromBase64String(parts[2]);

            byte[] computedHash = ComputeHash(plainTextPassword, salt, iterations);

            return SlowEquals(storedSubHash, computedHash);
        }

        private static byte[] ComputeHash(string password, byte[] salt, int iterations)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }

        /// <summary>
        /// Constant-time byte comparison — prevents timing attacks that could
        /// otherwise leak how many leading bytes of the hash matched.
        /// </summary>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }
    }
}