using libraryManagementSystem.Services;
using libraryManagementSystem.Utils;
using System;

namespace libraryManagementSystem.Pages
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private readonly BorrowService _borrowService = new BorrowService();
        private readonly BookService _bookService = new BookService();
        private readonly MemberService _memberService = new MemberService();

        protected void Page_Load(object sender, EventArgs e)
        {
            SessionManager.RequireLogin();
            if (!IsPostBack) LoadDashboardStats();
        }

        private void LoadDashboardStats()
        {
            try
            {
                var allBooks = _bookService.GetAllBooks();
                var allMembers = _memberService.GetAllMembers();
                var allBorrows = _borrowService.GetAllBorrowRecords();
                var overdueRecords = _borrowService.GetOverdueRecords();

                litTotalBooks.Text = allBooks.Count.ToString();
                litTotalMembers.Text = allMembers.Count.ToString();
                litBorrowedBooks.Text = allBorrows.FindAll(b => b.Status == Constants.BorrowStatus.Borrowed).Count.ToString();
                litOverdueBooks.Text = overdueRecords.Count.ToString();
                litWelcomeName.Text = ", " + System.Web.HttpUtility.HtmlEncode(SessionManager.CurrentFullName);

                allBorrows.Sort((a, b) => b.BorrowDate.CompareTo(a.BorrowDate));
                gvRecentBorrowed.DataSource = allBorrows.GetRange(0, Math.Min(10, allBorrows.Count));
                gvRecentBorrowed.DataBind();
            }
            catch (Exception)
            {
                pnlError.Visible = true;
                litError.Text = "Failed to load dashboard data. Please try again.";
            }
        }
    }
}
