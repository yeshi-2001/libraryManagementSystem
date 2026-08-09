using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using libraryManagementSystem.Models;
using libraryManagementSystem.Utils;

namespace libraryManagementSystem.Repository
{
    /// <summary>
    /// Data access for the BorrowRecords table. No business logic — due-date
    /// calculation, fine calculation, and availability checks all live in
    /// BorrowService. This class also exposes "WithinTransaction" overloads
    /// used exclusively by BorrowService/ReturnService via Database.ExecuteTransaction,
    /// since a borrow/return must update BorrowRecords AND Books.AvailableQuantity
    /// together or not at all.
    /// </summary>
    public class BorrowRepository
    {
        public List<BorrowRecord> GetAll()
        {
            DataTable table = Database.ExecuteQuery("sp_Borrow_GetAll", CommandType.StoredProcedure);
            return MapDataTableToList(table);
        }

        public BorrowRecord GetById(int borrowId)
        {
            DataTable table = Database.ExecuteQuery(
                "sp_Borrow_GetById",
                CommandType.StoredProcedure,
                Database.Param("@BorrowId", borrowId));

            return table.Rows.Count > 0 ? MapRowToBorrowRecord(table.Rows[0]) : null;
        }

        public PagedResult<BorrowRecord> Search(string searchTerm, string status, int pageNumber, int pageSize)
        {
            DataTable table = Database.ExecuteQuery(
                "sp_Borrow_Search",
                CommandType.StoredProcedure,
                Database.Param("@SearchTerm", searchTerm),
                Database.Param("@Status", status),
                Database.Param("@PageNumber", pageNumber),
                Database.Param("@PageSize", pageSize));

            return new PagedResult<BorrowRecord>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = MapDataTableToList(table),
                TotalCount = table.Rows.Count > 0 ? (int)table.Rows[0]["TotalCount"] : 0
            };
        }

        /// <summary>
        /// Simple, non-transactional insert — used only where no stock adjustment
        /// is needed alongside it. The real Borrow flow uses InsertWithinTransaction.
        /// </summary>
        public int Insert(BorrowRecord record)
        {
            return Database.ExecuteInsertAndGetId(
                "sp_Borrow_Insert",
                CommandType.StoredProcedure,
                Database.Param("@BookId", record.BookId),
                Database.Param("@MemberId", record.MemberId),
                Database.Param("@DueDate", record.DueDate));
        }

        public void Update(BorrowRecord record)
        {
            Database.ExecuteNonQuery(
                "sp_Borrow_Update",
                CommandType.StoredProcedure,
                Database.Param("@BorrowId", record.BorrowId),
                Database.Param("@ReturnDate", record.ReturnDate),
                Database.Param("@Fine", record.Fine),
                Database.Param("@Status", record.Status));
        }

        /// <summary>Only succeeds for records already in "Returned" status (enforced by the SP).</summary>
        public int GetActiveBorrowCount(int memberId)
        {
            object result = Database.ExecuteScalar(
                "sp_Borrow_GetActiveBorrowCount",
                CommandType.StoredProcedure,
                Database.Param("@MemberId", memberId));

            return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
        }

        public bool Delete(int borrowId)
        {
            object result = Database.ExecuteScalar(
                "sp_Borrow_Delete",
                CommandType.StoredProcedure,
                Database.Param("@BorrowId", borrowId));

            int rowsAffected = result == null || result == DBNull.Value ? 0 : (int)result;
            return rowsAffected > 0;
        }

        // ==================================================================
        // TRANSACTION-AWARE METHODS
        // These run against a connection/transaction already opened by
        // Database.ExecuteTransaction (see BorrowService/ReturnService in Step 5).
        // They do NOT open their own connection - that's the whole point:
        // multiple statements share one atomic transaction.
        // ==================================================================

        /// <summary>Inserts the borrow record and returns the new BorrowId.</summary>
        public int InsertWithinTransaction(SqlConnection connection, SqlTransaction transaction, BorrowRecord record)
        {
            using (SqlCommand command = new SqlCommand("sp_Borrow_Insert", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(Database.Param("@BookId", record.BookId));
                command.Parameters.Add(Database.Param("@MemberId", record.MemberId));
                command.Parameters.Add(Database.Param("@DueDate", record.DueDate));

                object result = command.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }

        /// <summary>
        /// Attempts to reduce a book's AvailableQuantity by 1. Returns false if no
        /// copy was actually available (@@ROWCOUNT = 0) — BorrowService checks this
        /// and throws, which rolls back the whole transaction including the insert above.
        /// </summary>
        public bool DecrementAvailableWithinTransaction(SqlConnection connection, SqlTransaction transaction, int bookId)
        {
            using (SqlCommand command = new SqlCommand("sp_Book_DecrementAvailable", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(Database.Param("@BookId", bookId));

                object result = command.ExecuteScalar();
                int rowsAffected = result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
                return rowsAffected > 0;
            }
        }

        /// <summary>Marks a borrow record as returned (ReturnDate/Fine/Status).</summary>
        public void UpdateReturnWithinTransaction(SqlConnection connection, SqlTransaction transaction,
            int borrowId, DateTime returnDate, decimal fine, string status)
        {
            using (SqlCommand command = new SqlCommand("sp_Borrow_Update", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(Database.Param("@BorrowId", borrowId));
                command.Parameters.Add(Database.Param("@ReturnDate", returnDate));
                command.Parameters.Add(Database.Param("@Fine", fine));
                command.Parameters.Add(Database.Param("@Status", status));
                command.ExecuteNonQuery();
            }
        }

        /// <summary>Increases a book's AvailableQuantity by 1 (capped at Quantity by the SP itself).</summary>
        public void IncrementAvailableWithinTransaction(SqlConnection connection, SqlTransaction transaction, int bookId)
        {
            using (SqlCommand command = new SqlCommand("sp_Book_IncrementAvailable", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(Database.Param("@BookId", bookId));
                command.ExecuteNonQuery();
            }
        }

        // ------------------------------------------------------------------
        // Mapping helpers
        // ------------------------------------------------------------------
        private List<BorrowRecord> MapDataTableToList(DataTable table)
        {
            var records = new List<BorrowRecord>();
            foreach (DataRow row in table.Rows)
            {
                records.Add(MapRowToBorrowRecord(row));
            }
            return records;
        }

        private BorrowRecord MapRowToBorrowRecord(DataRow row)
        {
            return new BorrowRecord
            {
                BorrowId = (int)row["BorrowId"],
                BookId = (int)row["BookId"],
                MemberId = (int)row["MemberId"],
                BorrowDate = (DateTime)row["BorrowDate"],
                DueDate = (DateTime)row["DueDate"],
                ReturnDate = row["ReturnDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["ReturnDate"],
                Fine = (decimal)row["Fine"],
                Status = row["Status"].ToString(),
                BookTitle = row.Table.Columns.Contains("BookTitle") ? row["BookTitle"].ToString() : null,
                ISBN = row.Table.Columns.Contains("ISBN") ? row["ISBN"].ToString() : null,
                MemberName = row.Table.Columns.Contains("MemberName") ? row["MemberName"].ToString() : null,
                NIC = row.Table.Columns.Contains("NIC") ? row["NIC"].ToString() : null
            };
        }
    }
}