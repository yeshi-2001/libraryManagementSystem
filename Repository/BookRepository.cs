using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using libraryManagementSystem.Models;
using libraryManagementSystem.Utils;

namespace libraryManagementSystem.Repository
{
  
    public class BookRepository
    {
        public List<Book> GetAll()
        {
            DataTable table = Database.ExecuteQuery("sp_Book_GetAll", CommandType.StoredProcedure);
            return MapDataTableToList(table);
        }

        public Book GetById(int bookId)
        {
            DataTable table = Database.ExecuteQuery(
                "sp_Book_GetById",
                CommandType.StoredProcedure,
                Database.Param("@BookId", bookId));

            return table.Rows.Count > 0 ? MapRowToBook(table.Rows[0]) : null;
        }

       
        public int Insert(Book book)
        {
            return Database.ExecuteInsertAndGetId(
                "sp_Book_Insert",
                CommandType.StoredProcedure,
                Database.Param("@ISBN", book.ISBN),
                Database.Param("@Title", book.Title),
                Database.Param("@Author", book.Author),
                Database.Param("@Category", book.Category),
                Database.Param("@Publisher", book.Publisher),
                Database.Param("@PublishYear", book.PublishYear),
                Database.Param("@Quantity", book.Quantity),
                Database.Param("@ShelfLocation", book.ShelfLocation),
                Database.Param("@Status", book.Status));
        }

        public void Update(Book book)
        {
            Database.ExecuteNonQuery(
                "sp_Book_Update",
                CommandType.StoredProcedure,
                Database.Param("@BookId", book.BookId),
                Database.Param("@ISBN", book.ISBN),
                Database.Param("@Title", book.Title),
                Database.Param("@Author", book.Author),
                Database.Param("@Category", book.Category),
                Database.Param("@Publisher", book.Publisher),
                Database.Param("@PublishYear", book.PublishYear),
                Database.Param("@Quantity", book.Quantity),
                Database.Param("@ShelfLocation", book.ShelfLocation),
                Database.Param("@Status", book.Status));
        }

        public bool Delete(int bookId)
        {
            object result = Database.ExecuteScalar(
                "sp_Book_Delete",
                CommandType.StoredProcedure,
                Database.Param("@BookId", bookId));

            int code = result == null || result == System.DBNull.Value ? 0 : (int)result;
            return code == 1;
        }

        public PagedResult<Book> Search(string searchTerm, int pageNumber, int pageSize)
        {
            DataTable table = Database.ExecuteQuery(
                "sp_Book_Search",
                CommandType.StoredProcedure,
                Database.Param("@SearchTerm", searchTerm),
                Database.Param("@PageNumber", pageNumber),
                Database.Param("@PageSize", pageSize));

            var result = new PagedResult<Book>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = MapDataTableToList(table),
                TotalCount = table.Rows.Count > 0 ? (int)table.Rows[0]["TotalCount"] : 0
            };
            return result;
        }


        private List<Book> MapDataTableToList(DataTable table)
        {
            var books = new List<Book>();
            foreach (DataRow row in table.Rows)
            {
                books.Add(MapRowToBook(row));
            }
            return books;
        }

        private Book MapRowToBook(DataRow row)
        {
            return new Book
            {
                BookId = (int)row["BookId"],
                ISBN = row["ISBN"].ToString(),
                Title = row["Title"].ToString(),
                Author = row["Author"].ToString(),
                Category = row["Category"] == System.DBNull.Value ? null : row["Category"].ToString(),
                Publisher = row["Publisher"] == System.DBNull.Value ? null : row["Publisher"].ToString(),
                PublishYear = row["PublishYear"] == System.DBNull.Value ? (int?)null : (int)row["PublishYear"],
                Quantity = (int)row["Quantity"],
                AvailableQuantity = (int)row["AvailableQuantity"],
                ShelfLocation = row["ShelfLocation"] == System.DBNull.Value ? null : row["ShelfLocation"].ToString(),
                Status = row["Status"].ToString(),
                CreatedDate = (System.DateTime)row["CreatedDate"]
            };
        }
    }
}