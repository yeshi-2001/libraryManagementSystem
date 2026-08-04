using libraryManagementSystem.Services;
using libraryManagementSystem.Utils;
using System;

namespace libraryManagementSystem.Pages
{
    public partial class BorrowHistory : System.Web.UI.Page
    {
        private readonly BorrowService _borrowService = new BorrowService();
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
        private string StatusFilter
        {
            get { return ViewState["Status"] as string ?? ""; }
            set { ViewState["Status"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SessionManager.RequireLogin();
            if (!IsPostBack) LoadHistory();
        }

        private void LoadHistory()
        {
            var result = _borrowService.SearchBorrowRecords(SearchTerm, StatusFilter, CurrentPage, 10);
            gvHistory.DataSource = result.Items;
            gvHistory.DataBind();
            litPageInfo.Text = $"Page {result.PageNumber} of {result.TotalPages} ({result.TotalCount} records)";
            btnPrev.Enabled = result.PageNumber > 1;
            btnNext.Enabled = result.PageNumber < result.TotalPages;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchTerm = txtSearch.Text.Trim();
            StatusFilter = ddlStatus.SelectedValue;
            CurrentPage = 1;
            LoadHistory();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            SearchTerm = "";
            StatusFilter = "";
            txtSearch.Text = "";
            ddlStatus.SelectedValue = "";
            CurrentPage = 1;
            LoadHistory();
        }

        protected void btnPrev_Click(object sender, EventArgs e) { CurrentPage--; LoadHistory(); }
        protected void btnNext_Click(object sender, EventArgs e) { CurrentPage++; LoadHistory(); }
    }
}
