using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using libraryManagementSystem.Models;
using libraryManagementSystem.Repository;
using libraryManagementSystem.Utils;

namespace libraryManagementSystem.Services
{
    /// <summary>
    /// Business logic for borrowing and returning books. This is the most
    /// sensitive service in the app: every Borrow/Return operation touches
    /// TWO tables (BorrowRecords and Books) that must stay in sync, so both
    /// operations run inside a single database transaction via
    /// Database.ExecuteTransaction — either everything succeeds, or nothing
    /// does, so AvailableQuantity can never drift from reality.
    /// </summary>
    public class BorrowService
    {
        private readonly BorrowRepository _borrowRepository;
        private readonly BookRepository _bookRepository;
        private readonly MemberRepository _memberRepository;
        private readonly MemberService _memberService;

        public BorrowService()
        {
            _borrowRepository = new BorrowRepository();
            _bookRepository = new BookRepository();
            _memberRepository = new MemberRepository();
            _memberService = new MemberService();
        }

        public List<BorrowRecord> GetAllBorrowRecords()
        {
            return _borrowRepository.GetAll();
        }

        public BorrowRecord GetBorrowRecordById(int borrowId)
        {
            BorrowRecord record = _borrowRepository.GetById(borrowId);
            if (record == null)
            {
                throw new InvalidOperationException("Borrow record not found.");
            }
            return record;
        }

        public PagedResult<BorrowRecord> SearchBorrowRecords(string searchTerm, string status, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = Constants.Pagination.DefaultPageSize;

            return _borrowRepository.Search(ValidationHelper.SanitizeInput(searchTerm), status, pageNumber, pageSize);
        }

        /// <summary>
        /// Issues a book to a member. Validates member eligibility and book
        /// availability, then atomically inserts the BorrowRecord and
        /// decrements Books.AvailableQuantity — if either step fails
        /// (e.g. another request took the last copy first), the whole
        /// transaction rolls back and no record or stock change persists.
        /// </summary>
        /// <returns>The newly created BorrowId.</returns>
        public int BorrowBook(int bookId, int memberId)
        {
            Member member = _memberRepository.GetById(memberId);
            if (member == null)
            {
                throw new InvalidOperationException("Member not found.");
            }
            if (!_memberService.IsEligibleToBorrow(member))
            {
                throw new InvalidOperationException("This member's account is inactive and cannot borrow books.");
            }

            int activeBorrows = _borrowRepository.GetActiveBorrowCount(memberId);
            if (activeBorrows >= Constants.BorrowRules.MaxBorrowLimit)
            {
                throw new InvalidOperationException(
                    $"This member already has {activeBorrows} book(s) borrowed. " +
                    $"The maximum limit is {Constants.BorrowRules.MaxBorrowLimit}. " +
                    "Please return the borrowed book(s) before borrowing again.");
            }

            Book book = _bookRepository.GetById(bookId);
            if (book == null)
            {
                throw new InvalidOperationException("Book not found.");
            }
            if (book.Status != Constants.BookStatus.Active)
            {
                throw new InvalidOperationException("This book is inactive and cannot be borrowed.");
            }
            if (book.AvailableQuantity <= 0)
            {
                throw new InvalidOperationException("No copies of this book are currently available.");
            }

            var record = new BorrowRecord
            {
                BookId = bookId,
                MemberId = memberId,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(Constants.BorrowRules.LoanPeriodDays),
                Fine = 0,
                Status = Constants.BorrowStatus.Borrowed
            };

            int newBorrowId = 0;
            bool stockWasAvailable = true;

            Database.ExecuteTransaction(new List<Action<SqlConnection, SqlTransaction>>
            {
                (connection, transaction) =>
                {
                    // Re-check and decrement availability INSIDE the transaction — this
                    // is the real concurrency guard. Two staff members clicking "Borrow"
                    // on the last copy at the same instant: only one decrement can
                    // succeed (the row is locked for the transaction's duration),
                    // the second sees AvailableQuantity already at 0 and gets rolled back.
                    stockWasAvailable = _borrowRepository.DecrementAvailableWithinTransaction(connection, transaction, bookId);
                    if (!stockWasAvailable)
                    {
                        throw new InvalidOperationException("No copies of this book are currently available.");
                    }

                    newBorrowId = _borrowRepository.InsertWithinTransaction(connection, transaction, record);
                }
            });

            return newBorrowId;
        }

        /// <summary>
        /// Returns a borrowed book. Automatically calculates any overdue fine,
        /// then atomically updates the BorrowRecord (ReturnDate/Fine/Status) and
        /// increments Books.AvailableQuantity back up.
        /// </summary>
        /// <returns>The fine amount charged (0 if returned on time).</returns>
        public decimal ReturnBook(int borrowId)
        {
            BorrowRecord record = _borrowRepository.GetById(borrowId);
            if (record == null)
            {
                throw new InvalidOperationException("Borrow record not found.");
            }
            if (record.Status == Constants.BorrowStatus.Returned)
            {
                throw new InvalidOperationException("This book has already been returned.");
            }

            DateTime returnDate = DateTime.Now;
            decimal fine = CalculateFine(record.DueDate, returnDate);

            Database.ExecuteTransaction(new List<Action<SqlConnection, SqlTransaction>>
            {
                (connection, transaction) =>
                {
                    _borrowRepository.UpdateReturnWithinTransaction(
                        connection, transaction, borrowId, returnDate, fine, Constants.BorrowStatus.Returned);

                    _borrowRepository.IncrementAvailableWithinTransaction(connection, transaction, record.BookId);
                }
            });

            return fine;
        }

        /// <summary>
        /// Calculates the fine for a return based on how many days late it is.
        /// Public + static-friendly logic so Reports can reuse the exact same
        /// calculation when displaying "fine owed so far" on still-borrowed
        /// overdue books, without duplicating the formula.
        /// </summary>
        public decimal CalculateFine(DateTime dueDate, DateTime returnDate)
        {
            if (returnDate.Date <= dueDate.Date)
            {
                return 0m;
            }

            int daysLate = (returnDate.Date - dueDate.Date).Days;
            return daysLate * Constants.BorrowRules.FinePerDay;
        }

        /// <summary>
        /// Returns all currently-borrowed records that are past their due date —
        /// used by the Dashboard's "Overdue Books" card and the Overdue Report.
        /// </summary>
        public List<BorrowRecord> GetOverdueRecords()
        {
            List<BorrowRecord> all = _borrowRepository.GetAll();
            return all.FindAll(r => r.Status == Constants.BorrowStatus.Borrowed && r.IsOverdue);
        }
    }
}