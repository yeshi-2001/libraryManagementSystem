using System;

namespace libraryManagementSystem.Models
{

    public class Member
    {
        public int MemberId { get; set; }
        public string FullName { get; set; }
        public string NIC { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime RegisteredDate { get; set; }


        public string Status { get; set; }

        public Member()
        {
            Status = "Active";
            RegisteredDate = DateTime.Now;
        }
    }
}