using System;

namespace libraryManagementSystem.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Publisher { get; set; }
        public int? PublishYear { get; set; }

        public int Quantity { get; set; }

        public int AvailableQuantity { get; set; }

        public string ShelfLocation { get; set; }

        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public Book()
        {
            Quantity = 0;
            AvailableQuantity = 0;
            Status = "Active";
            CreatedDate = DateTime.Now;
        }

        public bool IsAvailable => AvailableQuantity > 0 && Status == "Active";
    }
}