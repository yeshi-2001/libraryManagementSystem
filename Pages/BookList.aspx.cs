using libraryManagementSystem.Models;
using libraryManagementSystem.Services;
using libraryManagementSystem.Utils;
using System;
using System.Web.UI.WebControls;

namespace libraryManagementSystem.Pages
{
    public partial class BookList : System.Web.UI.Page
    {
        private readonly BookService _bookService = new BookService();
        private int CurrentPage
        {
            get { return ViewState["Page"] == null ? 1 : (int)ViewState["Page"]; }
            set { ViewState["Page"] = value; }
        }
        private string SearchTerm
        {
            get { return ViewState["Search"] as string ?? ""; }
            set { ViewState["Search"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SessionManager.RequireLogin();
            if (!IsPostBack) LoadBooks();
        }

        private void LoadBooks()
        {
            var result = _bookService.SearchBooks(SearchTerm, CurrentPage, 10);
            gvBooks.DataSource = result.Items;
            gvBooks.DataBind();
            litPageInfo.Text = $"Page {result.PageNumber} of {result.TotalPages} ({result.TotalCount} books)";
            btnPrev.Enabled = result.PageNumber > 1;
            btnNext.Enabled = result.PageNumber < result.TotalPages;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchTerm = txtSearch.Text.Trim();
            CurrentPage = 1;
            LoadBooks();
        }

        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            SearchTerm = "";
            txtSearch.Text = "";
            CurrentPage = 1;
            LoadBooks();
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            CurrentPage--;
            LoadBooks();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            LoadBooks();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            litFormTitle.Text = "Add Book";
            pnlForm.Visible = true;
        }

        protected void btnCancelForm_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = false;
            ClearForm();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            try
            {
                int bookId = int.Parse(hfBookId.Value);
                var book = new Book
                {
                    BookId = bookId,
                    ISBN = txtISBN.Text.Trim(),
                    Title = txtTitle.Text.Trim(),
                    Author = txtAuthor.Text.Trim(),
                    Category = txtCategory.Text.Trim(),
                    Publisher = txtPublisher.Text.Trim(),
                    PublishYear = string.IsNullOrWhiteSpace(txtPublishYear.Text) ? (int?)null : int.Parse(txtPublishYear.Text),
                    Quantity = int.Parse(txtQuantity.Text),
                    ShelfLocation = txtShelfLocation.Text.Trim(),
                    Status = ddlStatus.SelectedValue
                };

                if (bookId == 0)
                    _bookService.AddBook(book);
                else
                    _bookService.UpdateBook(book);

                ToastHelper.QueueToast(bookId == 0 ? "Book added successfully." : "Book updated successfully.");
                pnlForm.Visible = false;
                ClearForm();
                LoadBooks();
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message, "danger");
            }
        }

        protected void gvBooks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int bookId = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "EditBook")
            {
                var book = _bookService.GetBookById(bookId);
                hfBookId.Value = book.BookId.ToString();
                txtISBN.Text = book.ISBN;
                txtTitle.Text = book.Title;
                txtAuthor.Text = book.Author;
                txtCategory.Text = book.Category;
                txtPublisher.Text = book.Publisher;
                txtPublishYear.Text = book.PublishYear?.ToString();
                txtQuantity.Text = book.Quantity.ToString();
                txtShelfLocation.Text = book.ShelfLocation;
                ddlStatus.SelectedValue = book.Status;
                litFormTitle.Text = "Edit Book";
                pnlForm.Visible = true;
            }
            else if (e.CommandName == "DeleteBook")
            {
                try
                {
                    _bookService.DeleteBook(bookId);
                    ToastHelper.QueueToast("Book deleted successfully.");
                    LoadBooks();
                }
                catch (Exception ex)
                {
                    ShowAlert(ex.Message, "danger");
                }
            }
        }

        private void ClearForm()
        {
            hfBookId.Value = "0";
            txtISBN.Text = txtTitle.Text = txtAuthor.Text = txtCategory.Text =
            txtPublisher.Text = txtPublishYear.Text = txtShelfLocation.Text = "";
            txtQuantity.Text = "1";
            ddlStatus.SelectedValue = "Active";
        }

        private void ShowAlert(string message, string type)
        {
            litAlert.Text = message;
            pnlAlert.CssClass = $"alert alert-{type} alert-dismissible fade show";
            pnlAlert.Visible = true;
        }
    }
}
