using System;
using System.Collections.Generic;
using libraryManagementSystem.Models;
using libraryManagementSystem.Repository;
using libraryManagementSystem.Utils;

namespace libraryManagementSystem.Services
{
    /// <summary>
    /// Business logic for user accounts and authentication. Login.aspx calls
    /// only Authenticate() — it never sees a password hash or talks to
    /// UserRepository directly.
    /// </summary>
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        /// <summary>
        /// Verifies a login attempt. Returns the User on success, or null on
        /// any failure (wrong username, wrong password, inactive account) —
        /// deliberately the SAME null result for all three cases, so the
        /// login page can't be used to enumerate which usernames exist.
        /// </summary>
        public User Authenticate(string username, string password)
        {
            if (!ValidationHelper.IsNotEmpty(username) || !ValidationHelper.IsNotEmpty(password))
            {
                return null;
            }

            User user = _userRepository.GetByUsername(username.Trim());
            if (user == null || !user.IsActive)
            {
                return null;
            }

            bool passwordMatches = PasswordHelper.VerifyPassword(password, user.Password);
            return passwordMatches ? user : null;
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public User GetUserById(int userId)
        {
            User user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            return user;
        }

        public PagedResult<User> SearchUsers(string searchTerm, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = Constants.Pagination.DefaultPageSize;

            return _userRepository.Search(ValidationHelper.SanitizeInput(searchTerm), pageNumber, pageSize);
        }

        /// <summary>Creates a new staff/admin account. Password is hashed here before storage.</summary>
        public int AddUser(User user, string plainTextPassword)
        {
            ValidateUser(user);
            if (string.IsNullOrWhiteSpace(plainTextPassword) || plainTextPassword.Length < 6)
            {
                throw new ArgumentException("Password must be at least 6 characters.");
            }
            CheckDuplicateUsername(user.Username, excludeUserId: null);

            user.Username = user.Username.Trim();
            user.FullName = ValidationHelper.SanitizeInput(user.FullName.Trim());
            user.Password = PasswordHelper.HashPassword(plainTextPassword);

            return _userRepository.Insert(user);
        }

        /// <summary>Updates profile fields (name, role, active status). Does not touch the password.</summary>
        public void UpdateUser(User user)
        {
            if (!ValidationHelper.IsPositiveInteger(user.UserId))
            {
                throw new ArgumentException("Invalid user ID.");
            }
            if (!ValidationHelper.IsNotEmpty(user.FullName))
            {
                throw new ArgumentException("Full name is required.");
            }

            user.FullName = ValidationHelper.SanitizeInput(user.FullName.Trim());
            _userRepository.Update(user);
        }

        /// <summary>
        /// Changes a user's password. Requires the current password to be
        /// supplied and verified first — even when called from an Admin-only
        /// Settings page — so a change always proves knowledge of the old value.
        /// </summary>
        public void ChangePassword(int userId, string currentPassword, string newPassword)
        {
            User user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            if (!PasswordHelper.VerifyPassword(currentPassword, user.Password))
            {
                throw new InvalidOperationException("Current password is incorrect.");
            }
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            {
                throw new ArgumentException("New password must be at least 6 characters.");
            }

            string newHash = PasswordHelper.HashPassword(newPassword);
            _userRepository.UpdatePassword(userId, newHash);
        }

        public void DeleteUser(int userId)
        {
            _userRepository.Delete(userId);
        }

        private void ValidateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (!ValidationHelper.IsNotEmpty(user.Username))
            {
                throw new ArgumentException("Username is required.");
            }
            if (!ValidationHelper.IsNotEmpty(user.FullName))
            {
                throw new ArgumentException("Full name is required.");
            }
            if (user.Role != Constants.Roles.Admin && user.Role != Constants.Roles.Staff)
            {
                throw new ArgumentException("Role must be either Admin or Staff.");
            }
        }

        private void CheckDuplicateUsername(string username, int? excludeUserId)
        {
            List<User> allUsers = _userRepository.GetAll();
            foreach (User existing in allUsers)
            {
                bool isSameUser = excludeUserId.HasValue && existing.UserId == excludeUserId.Value;
                if (!isSameUser && existing.Username.Equals(username?.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException($"Username '{username}' is already taken.");
                }
            }
        }
    }
}