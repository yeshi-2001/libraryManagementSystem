using System;

namespace libraryManagementSystem.Models
{

    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public User()
        {

            Role = "Staff";
            IsActive = true;
            CreatedDate = DateTime.Now;
        }
    }
}