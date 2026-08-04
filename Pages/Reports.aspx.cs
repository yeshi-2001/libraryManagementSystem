using libraryManagementSystem.Services;
using libraryManagementSystem.Utils;
using System;

namespace libraryManagementSystem.Pages
{
    public partial class Reports : System.Web.UI.Page
    {
        private readonly BookService _bookService = new BookService();
        private readonly MemberService _memberService = new MemberService();
        private readonly BorrowService _borrowService = new BorrowService();

        protected void Page_Load(object sender, EventArgs e)
        {
            SessionManager.RequireLogin();
            if (!IsPostBack) LoadReports();
        }

        private void LoadReports()
        {
            var books = _bookService.GetAllBooks();
            var members = _memberService.GetAllMembers();
            var allBorrows = _borrowService.GetAllBorrowRecords();
            var overdue = _borrowService.GetOverdueRecords();

            litTotalBooks.Text = books.Count.ToString();
            litTotalMembers.Text = members.Count.ToString();
            litBorrowed.Text = allBorrows.FindAll(b => b.Status == Constants.BorrowStatus.Borrowed).Count.ToString();
            litOverdue.Text = overdue.Count.ToString();

            gvOverdue.DataSource = overdue;
            gvOverdue.DataBind();

            allBorrows.Sort((a, b) => b.BorrowDate.CompareTo(a.BorrowDate));
            gvAllBorrows.DataSource = allBorrows;
            gvAllBorrows.DataBind();
        }
    }
}
