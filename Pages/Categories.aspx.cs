using libraryManagementSystem.Services;
using libraryManagementSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace libraryManagementSystem.Pages
{
    public partial class Categories : System.Web.UI.Page
    {
        private readonly BookService _bookService = new BookService();

        protected void Page_Load(object sender, EventArgs e)
        {
            SessionManager.RequireLogin();
            if (!IsPostBack) LoadCategories();
        }

        private void LoadCategories()
        {
            // Derive distinct categories from the Books table
            var books = _bookService.GetAllBooks();
            var categories = books
                .Where(b => !string.IsNullOrWhiteSpace(b.Category))
                .Select(b => b.Category.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(c => c)
                .Select(c => new { Category = c })
                .ToList();

            gvCategories.DataSource = categories;
            gvCategories.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            // Categories are free-text on books — just show confirmation
            ShowAlert($"Category '{txtCategoryName.Text.Trim()}' saved. Use this name when adding/editing books.", "success");
            txtCategoryName.Text = "";
            litFormTitle.Text = "Add Category";
            hfCategoryName.Value = "";
            LoadCategories();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtCategoryName.Text = "";
            litFormTitle.Text = "Add Category";
            hfCategoryName.Value = "";
        }

        protected void gvCategories_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditCat")
            {
                txtCategoryName.Text = e.CommandArgument.ToString();
                hfCategoryName.Value = e.CommandArgument.ToString();
                litFormTitle.Text = "Edit Category";
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
