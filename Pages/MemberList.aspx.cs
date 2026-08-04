using libraryManagementSystem.Models;
using libraryManagementSystem.Services;
using libraryManagementSystem.Utils;
using System;
using System.Web.UI.WebControls;

namespace libraryManagementSystem.Pages
{
    public partial class MemberList : System.Web.UI.Page
    {
        private readonly MemberService _memberService = new MemberService();
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
            if (!IsPostBack) LoadMembers();
        }

        private void LoadMembers()
        {
            var result = _memberService.SearchMembers(SearchTerm, CurrentPage, 10);
            gvMembers.DataSource = result.Items;
            gvMembers.DataBind();
            litPageInfo.Text = $"Page {result.PageNumber} of {result.TotalPages} ({result.TotalCount} members)";
            btnPrev.Enabled = result.PageNumber > 1;
            btnNext.Enabled = result.PageNumber < result.TotalPages;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchTerm = txtSearch.Text.Trim();
            CurrentPage = 1;
            LoadMembers();
        }

        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            SearchTerm = "";
            txtSearch.Text = "";
            CurrentPage = 1;
            LoadMembers();
        }

        protected void btnPrev_Click(object sender, EventArgs e) { CurrentPage--; LoadMembers(); }
        protected void btnNext_Click(object sender, EventArgs e) { CurrentPage++; LoadMembers(); }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            litFormTitle.Text = "Add Member";
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
                int memberId = int.Parse(hfMemberId.Value);
                var member = new Member
                {
                    MemberId = memberId,
                    FullName = txtFullName.Text.Trim(),
                    NIC = txtNIC.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Status = ddlStatus.SelectedValue
                };

                if (memberId == 0) _memberService.AddMember(member);
                else _memberService.UpdateMember(member);

                ToastHelper.QueueToast(memberId == 0 ? "Member added successfully." : "Member updated successfully.");
                pnlForm.Visible = false;
                ClearForm();
                LoadMembers();
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message, "danger");
            }
        }

        protected void gvMembers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int memberId = int.Parse(e.CommandArgument.ToString());
            if (e.CommandName == "EditMember")
            {
                var m = _memberService.GetMemberById(memberId);
                hfMemberId.Value = m.MemberId.ToString();
                txtFullName.Text = m.FullName;
                txtNIC.Text = m.NIC;
                txtEmail.Text = m.Email;
                txtPhone.Text = m.Phone;
                txtAddress.Text = m.Address;
                ddlStatus.SelectedValue = m.Status;
                litFormTitle.Text = "Edit Member";
                pnlForm.Visible = true;
            }
            else if (e.CommandName == "DeleteMember")
            {
                try
                {
                    _memberService.DeleteMember(memberId);
                    ToastHelper.QueueToast("Member deleted successfully.");
                    LoadMembers();
                }
                catch (Exception ex) { ShowAlert(ex.Message, "danger"); }
            }
        }

        private void ClearForm()
        {
            hfMemberId.Value = "0";
            txtFullName.Text = txtNIC.Text = txtEmail.Text = txtPhone.Text = txtAddress.Text = "";
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
