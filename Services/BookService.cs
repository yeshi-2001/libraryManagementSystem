using System;
using System.Collections.Generic;
using libraryManagementSystem.Models;
using libraryManagementSystem.Repository;
using libraryManagementSystem.Utils;

namespace libraryManagementSystem.Services
{
    /// duplicate checks, and any cross-entity rules live, keeping code-behind
 
    public class BookService
    {
        private readonly BookRepository _bookRepository;

        public BookService()
        {
            _bookRepository = new BookRepository();
        }

        public List<Book> GetAllBooks()
        {
            return _bookRepository.GetAll();
        }

        public Book GetBookById(int bookId)
        {
            if (!ValidationHelper.IsPositiveInteger(bookId))
            {
                throw new ArgumentException("Invalid book ID.");
            }

            Book book = _bookRepository.GetById(bookId);
            if (book == null)
            {
                throw new InvalidOperationException("Book not found.");
            }
            return book;
        }

        public PagedResult<Book> SearchBooks(string searchTerm, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = Constants.Pagination.DefaultPageSize;

            return _bookRepository.Search(ValidationHelper.SanitizeInput(searchTerm), pageNumber, pageSize);
        }

        /// <summary>Adds a new book after validating all fields and checking for a duplicate ISBN.</summary>
        public int AddBook(Book book)
        {
            ValidateBook(book);
            CheckDuplicateIsbn(book.ISBN, excludeBookId: null);

            book.ISBN = book.ISBN.Trim();
            book.Title = ValidationHelper.SanitizeInput(book.Title.Trim());
            book.Author = ValidationHelper.SanitizeInput(book.Author.Trim());
            book.Status = string.IsNullOrEmpty(book.Status) ? Constants.BookStatus.Active : book.Status;

            return _bookRepository.Insert(book);
        }

        /// <summary>Updates an existing book after validating all fields and checking for a duplicate ISBN.</summary>
        public void UpdateBook(Book book)
        {
            if (!ValidationHelper.IsPositiveInteger(book.BookId))
            {
                throw new ArgumentException("Invalid book ID.");
            }

            ValidateBook(book);
            CheckDuplicateIsbn(book.ISBN, excludeBookId: book.BookId);

            book.ISBN = book.ISBN.Trim();
            book.Title = ValidationHelper.SanitizeInput(book.Title.Trim());
            book.Author = ValidationHelper.SanitizeInput(book.Author.Trim());

            _bookRepository.Update(book);
        }

        /// <summary>
        /// Deletes a book. 
        /// borrow history and cannot be removed (the DB layer enforces this;

        public void DeleteBook(int bookId)
        {
            bool deleted = _bookRepository.Delete(bookId);
            if (!deleted)
            {
                throw new InvalidOperationException(
                    "This book cannot be deleted because it has borrow history. " +
                    "Consider setting its status to Inactive instead.");
            }
        }

        public List<Book> GetAvailableBooks()
        {
            List<Book> all = _bookRepository.GetAll();
            return all.FindAll(b => b.IsAvailable);
        }

        private void ValidateBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            if (!ValidationHelper.IsValidISBN(book.ISBN))
            {
                throw new ArgumentException("ISBN must be 10 or 13 characters (digits only, hyphens allowed).");
            }
            if (!ValidationHelper.IsNotEmpty(book.Title))
            {
                throw new ArgumentException("Title is required.");
            }
            if (!ValidationHelper.IsNotEmpty(book.Author))
            {
                throw new ArgumentException("Author is required.");
            }
            if (book.Quantity < 0)
            {
                throw new ArgumentException("Quantity cannot be negative.");
            }
            if (!ValidationHelper.IsValidYear(book.PublishYear))
            {
                throw new ArgumentException($"Publish year must be between 1450 and {DateTime.Now.Year}.");
            }
        }

        private void CheckDuplicateIsbn(string isbn, int? excludeBookId)
        {
            List<Book> allBooks = _bookRepository.GetAll();
            foreach (Book existing in allBooks)
            {
                bool isSameBook = excludeBookId.HasValue && existing.BookId == excludeBookId.Value;
                if (!isSameBook && existing.ISBN.Equals(isbn?.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException($"A book with ISBN '{isbn}' already exists.");
                }
            }
        }
    }
}