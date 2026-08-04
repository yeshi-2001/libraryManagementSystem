using System.Collections.Generic;
using System.Data;
using libraryManagementSystem.Models;
using libraryManagementSystem.Utils;

namespace libraryManagementSystem.Repository
{
    /// <summary>
    /// Data access for the Users table. No business logic — password hashing
    /// and login verification happen in UserService via Utils/PasswordHelper.cs.
    /// </summary>
    public class UserRepository
    {
        public List<User> GetAll()
        {
            DataTable table = Database.ExecuteQuery("sp_User_GetAll", CommandType.StoredProcedure);
            return MapDataTableToList(table);
        }

        public User GetById(int userId)
        {
            DataTable table = Database.ExecuteQuery(
                "sp_User_GetById",
                CommandType.StoredProcedure,
                Database.Param("@UserId", userId));

            return table.Rows.Count > 0 ? MapRowToUser(table.Rows[0]) : null;
        }

        /// <summary>Used exclusively by the login flow — looks up a user by their username.</summary>
        public User GetByUsername(string username)
        {
            DataTable table = Database.ExecuteQuery(
                "sp_User_GetByUsername",
                CommandType.StoredProcedure,
                Database.Param("@Username", username));

            return table.Rows.Count > 0 ? MapRowToUser(table.Rows[0]) : null;
        }

        /// <summary>Inserts a new user. Caller must pass an already-hashed password (see PasswordHelper).</summary>
        public int Insert(User user)
        {
            return Database.ExecuteInsertAndGetId(
                "sp_User_Insert",
                CommandType.StoredProcedure,
                Database.Param("@Username", user.Username),
                Database.Param("@Password", user.Password),
                Database.Param("@FullName", user.FullName),
                Database.Param("@Role", user.Role));
        }

        /// <summary>Updates profile fields only — never touches the password. See UpdatePassword().</summary>
        public void Update(User user)
        {
            Database.ExecuteNonQuery(
                "sp_User_Update",
                CommandType.StoredProcedure,
                Database.Param("@UserId", user.UserId),
                Database.Param("@FullName", user.FullName),
                Database.Param("@Role", user.Role),
                Database.Param("@IsActive", user.IsActive));
        }

        /// <summary>Updates only the password hash. Caller must pass an already-hashed value.</summary>
        public void UpdatePassword(int userId, string newHashedPassword)
        {
            Database.ExecuteNonQuery(
                "sp_User_UpdatePassword",
                CommandType.StoredProcedure,
                Database.Param("@UserId", userId),
                Database.Param("@Password", newHashedPassword));
        }

        public void Delete(int userId)
        {
            Database.ExecuteNonQuery(
                "sp_User_Delete",
                CommandType.StoredProcedure,
                Database.Param("@UserId", userId));
        }

        public PagedResult<User> Search(string searchTerm, int pageNumber, int pageSize)
        {
            DataTable table = Database.ExecuteQuery(
                "sp_User_Search",
                CommandType.StoredProcedure,
                Database.Param("@SearchTerm", searchTerm),
                Database.Param("@PageNumber", pageNumber),
                Database.Param("@PageSize", pageSize));

            return new PagedResult<User>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = MapDataTableToList(table),
                TotalCount = table.Rows.Count > 0 ? (int)table.Rows[0]["TotalCount"] : 0
            };
        }

        private List<User> MapDataTableToList(DataTable table)
        {
            var users = new List<User>();
            foreach (DataRow row in table.Rows)
            {
                users.Add(MapRowToUser(row));
            }
            return users;
        }

        private User MapRowToUser(DataRow row)
        {
            return new User
            {
                UserId = (int)row["UserId"],
                Username = row["Username"].ToString(),
                Password = row["Password"].ToString(),
                FullName = row["FullName"].ToString(),
                Role = row["Role"].ToString(),
                CreatedDate = (System.DateTime)row["CreatedDate"],
                IsActive = (bool)row["IsActive"]
            };
        }
    }
}