using System;

namespace libraryManagementSystem.Models
{

    public class BorrowRecord
    {
        public int BorrowId { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal Fine { get; set; }

        public string Status { get; set; }

        public string BookTitle { get; set; }
        public string ISBN { get; set; }
        public string MemberName { get; set; }
        public string NIC { get; set; }

        public BorrowRecord()
        {
            BorrowDate = DateTime.Now;
            Fine = 0;
            Status = "Borrowed";
        }

        public bool IsOverdue => Status == "Borrowed" && DateTime.Now.Date > DueDate.Date;
    }
}