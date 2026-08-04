using libraryManagementSystem.Services;
using libraryManagementSystem.Utils;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace libraryManagementSystem.Pages
{
    public partial class ReturnBook : System.Web.UI.Page
    {
        private readonly BorrowService _borrowService = new BorrowService();

        private string SearchTerm
        {
            get { return ViewState["Search"] as string ?? ""; }
            set { ViewState["Search"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SessionManager.RequireLogin();
            if (!IsPostBack) LoadBorrowed();
        }

        private void LoadBorrowed()
        {
            // Get only currently borrowed records
            var result = _borrowService.SearchBorrowRecords(SearchTerm, Constants.BorrowStatus.Borrowed, 1, 100);
            gvBorrowed.DataSource = result.Items;
            gvBorrowed.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchTerm = txtSearch.Text.Trim();
            LoadBorrowed();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            SearchTerm = "";
            txtSearch.Text = "";
            LoadBorrowed();
        }

        protected void gvBorrowed_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ReturnBook")
            {
                try
                {
                    int borrowId = int.Parse(e.CommandArgument.ToString());
                    decimal fine = _borrowService.ReturnBook(borrowId);
                    string msg = fine > 0
                        ? $"Book returned successfully. Fine charged: Rs. {fine:F2}"
                        : "Book returned successfully. No fine.";
                    ShowAlert(msg, fine > 0 ? "warning" : "success");
                    LoadBorrowed();
                }
                catch (Exception ex)
                {
                    ShowAlert(ex.Message, "danger");
                }
            }
        }

        // Helper called from the GridView template to show estimated fine
        protected string GetEstimatedFine(object dueDate, string status)
        {
            if (status != Constants.BorrowStatus.Borrowed) return "-";
            decimal fine = _borrowService.CalculateFine((DateTime)dueDate, DateTime.Now);
            return fine > 0 ? $"Rs. {fine:F2}" : "None";
        }

        private void ShowAlert(string message, string type)
        {
            litAlert.Text = message;
            pnlAlert.CssClass = $"alert alert-{type} alert-dismissible fade show";
            pnlAlert.Visible = true;
        }
    }
}
