using libraryManagementSystem.Models;
using libraryManagementSystem.Services;
using libraryManagementSystem.Utils;
using System;
using System.Web.UI.WebControls;

namespace libraryManagementSystem.Pages
{
    public partial class UserList : System.Web.UI.Page
    {
        private readonly UserService _userService = new UserService();

        protected void Page_Load(object sender, EventArgs e)
        {
            SessionManager.RequireAdmin(); // Admin only
            if (!IsPostBack) LoadUsers();
        }

        private void LoadUsers()
        {
            gvUsers.DataSource = _userService.GetAllUsers();
            gvUsers.DataBind();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            pnlPassword.Visible = true;
            rfvPassword.Enabled = true;
            litFormTitle.Text = "Add User";
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
                int userId = int.Parse(hfUserId.Value);
                var user = new User
                {
                    UserId = userId,
                    FullName = txtFullName.Text.Trim(),
                    Username = txtUsername.Text.Trim(),
                    Role = ddlRole.SelectedValue,
                    IsActive = chkIsActive.Checked
                };

                if (userId == 0)
                    _userService.AddUser(user, txtPassword.Text);
                else
                    _userService.UpdateUser(user);

                ToastHelper.QueueToast(userId == 0 ? "User added successfully." : "User updated successfully.");
                pnlForm.Visible = false;
                ClearForm();
                LoadUsers();
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message, "danger");
            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int userId = int.Parse(e.CommandArgument.ToString());
            if (e.CommandName == "EditUser")
            {
                var u = _userService.GetUserById(userId);
                hfUserId.Value = u.UserId.ToString();
                txtFullName.Text = u.FullName;
                txtUsername.Text = u.Username;
                ddlRole.SelectedValue = u.Role;
                chkIsActive.Checked = u.IsActive;
                // Hide password field on edit
                pnlPassword.Visible = false;
                rfvPassword.Enabled = false;
                litFormTitle.Text = "Edit User";
                pnlForm.Visible = true;
            }
            else if (e.CommandName == "DeleteUser")
            {
                try
                {
                    // Prevent deleting yourself
                    if (userId == SessionManager.CurrentUserId)
                    {
                        ShowAlert("You cannot delete your own account.", "warning");
                        return;
                    }
                    _userService.DeleteUser(userId);
                    ToastHelper.QueueToast("User deleted.");
                    LoadUsers();
                }
                catch (Exception ex) { ShowAlert(ex.Message, "danger"); }
            }
        }

        private void ClearForm()
        {
            hfUserId.Value = "0";
            txtFullName.Text = txtUsername.Text = txtPassword.Text = "";
            ddlRole.SelectedValue = "Staff";
            chkIsActive.Checked = true;
        }

        private void ShowAlert(string message, string type)
        {
            litAlert.Text = message;
            pnlAlert.CssClass = $"alert alert-{type} alert-dismissible fade show";
            pnlAlert.Visible = true;
        }
    }
}
