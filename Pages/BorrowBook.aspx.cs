using libraryManagementSystem.Services;
using libraryManagementSystem.Utils;
using System;
using System.Linq;

namespace libraryManagementSystem.Pages
{
    public partial class BorrowBook : System.Web.UI.Page
    {
        private readonly BorrowService _borrowService = new BorrowService();
        private readonly BookService _bookService = new BookService();
        private readonly MemberService _memberService = new MemberService();

        protected void Page_Load(object sender, EventArgs e)
        {
            SessionManager.RequireLogin();
            if (!IsPostBack)
            {
                txtBorrowDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                txtDueDate.Text = DateTime.Today.AddDays(Constants.BorrowRules.LoanPeriodDays).ToString("yyyy-MM-dd");
                LoadDropdowns();
                LoadRecentBorrows();
            }
        }

        private void LoadDropdowns()
        {
            // Only show books that have available copies
            var books = _bookService.GetAvailableBooks();
            ddlBook.DataSource = books;
            ddlBook.DataTextField = "Title";
            ddlBook.DataValueField = "BookId";
            ddlBook.DataBind();

            // Only show active members
            var members = _memberService.GetAllMembers().FindAll(m => m.Status == Constants.MemberStatus.Active);
            ddlMember.DataSource = members;
            ddlMember.DataTextField = "FullName";
            ddlMember.DataValueField = "MemberId";
            ddlMember.DataBind();
        }

        private void LoadRecentBorrows()
        {
            var all = _borrowService.GetAllBorrowRecords();
            all.Sort((a, b) => b.BorrowDate.CompareTo(a.BorrowDate));
            gvRecent.DataSource = all.GetRange(0, Math.Min(10, all.Count));
            gvRecent.DataBind();
        }

        protected void btnIssue_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            try
            {
                int bookId = int.Parse(ddlBook.SelectedValue);
                int memberId = int.Parse(ddlMember.SelectedValue);
                _borrowService.BorrowBook(bookId, memberId);

                ToastHelper.QueueToast("Book issued successfully.");
                LoadDropdowns();
                LoadRecentBorrows();
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message, "danger");
            }
        }

        private void ShowAlert(string message, string type)
        {
            litAlert.Text = message;
            pnlAlert.CssClass = $"alert alert-{type} alert-dismissible fade show";
            pnlAlert.Visible = true;
        }
    }
}
