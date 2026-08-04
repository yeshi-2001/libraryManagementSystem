<%@ Page Title="Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="libraryManagementSystem.Pages.UserList" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Users</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="PageHeaderContent" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <i class="fa-solid fa-user-shield me-2"></i>System Users
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show">
        <asp:Literal ID="litAlert" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </asp:Panel>

    <asp:Panel ID="pnlForm" runat="server" Visible="false" CssClass="card mb-4">
        <div class="card-header d-flex justify-content-between align-items-center">
            <span><i class="fa-solid fa-user-pen me-2"></i><asp:Literal ID="litFormTitle" runat="server" Text="Add User" /></span>
            <asp:LinkButton ID="btnCancelForm" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnCancelForm_Click">
                <i class="fa-solid fa-xmark me-1"></i>Cancel
            </asp:LinkButton>
        </div>
        <div class="card-body">
            <asp:HiddenField ID="hfUserId" runat="server" Value="0" />
            <div class="row g-3">
                <div class="col-md-4">
                    <label class="form-label fw-semibold">Full Name <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFullName" ValidationGroup="UserForm"
                        CssClass="text-danger small" Display="Dynamic" ErrorMessage="Full name is required." />
                </div>
                <div class="col-md-4">
                    <label class="form-label fw-semibold">Username <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsername" ValidationGroup="UserForm"
                        CssClass="text-danger small" Display="Dynamic" ErrorMessage="Username is required." />
                </div>
                <div class="col-md-4">
                    <label class="form-label fw-semibold">Role <span class="text-danger">*</span></label>
                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select">
                        <asp:ListItem Value="Staff">Staff</asp:ListItem>
                        <asp:ListItem Value="Admin">Admin</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <asp:Panel ID="pnlPassword" runat="server">
                    <div class="col-md-6 mt-2">
                        <label class="form-label fw-semibold">Password <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                            ValidationGroup="UserForm" CssClass="text-danger small" Display="Dynamic"
                            ErrorMessage="Password is required." />
                    </div>
                </asp:Panel>
                <div class="col-md-3 mt-2">
                    <label class="form-label fw-semibold">Active</label>
                    <asp:CheckBox ID="chkIsActive" runat="server" Checked="true" CssClass="form-check-input ms-2" />
                </div>
            </div>
            <div class="mt-3">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save User"
                    ValidationGroup="UserForm" OnClick="btnSave_Click" />
            </div>
        </div>
    </asp:Panel>

    <div class="card">
        <div class="card-header d-flex flex-wrap gap-2 justify-content-between align-items-center">
            <span><i class="fa-solid fa-list me-2"></i>All Users</span>
            <asp:Button ID="btnAddNew" runat="server" CssClass="btn btn-sm btn-primary" Text="+ Add User" OnClick="btnAddNew_Click" />
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvUsers" runat="server" CssClass="table table-hover mb-0"
                    AutoGenerateColumns="false" GridLines="None"
                    EmptyDataText="No users found."
                    OnRowCommand="gvUsers_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Username" HeaderText="Username" />
                        <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                        <asp:BoundField DataField="Role" HeaderText="Role" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:TemplateField HeaderText="Active">
                            <ItemTemplate><%# (bool)Eval("IsActive") ? "✔" : "✘" %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="EditUser" CommandArgument='<%# Eval("UserId") %>'
                                    CssClass="btn btn-sm btn-outline-primary me-1">
                                    <i class="fa-solid fa-pen"></i>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="DeleteUser" CommandArgument='<%# Eval("UserId") %>'
                                    CssClass="btn btn-sm btn-outline-danger"
                                    OnClientClick="return confirm('Delete this user?');">
                                    <i class="fa-solid fa-trash"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
