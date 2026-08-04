<%@ Page Title="Members" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MemberList.aspx.cs" Inherits="libraryManagementSystem.Pages.MemberList" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Members</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="PageHeaderContent" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <i class="fa-solid fa-users me-2"></i>Members
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show">
        <asp:Literal ID="litAlert" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </asp:Panel>

    <asp:Panel ID="pnlForm" runat="server" Visible="false" CssClass="card mb-4">
        <div class="card-header d-flex justify-content-between align-items-center">
            <span><i class="fa-solid fa-user-pen me-2"></i><asp:Literal ID="litFormTitle" runat="server" Text="Add Member" /></span>
            <asp:LinkButton ID="btnCancelForm" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnCancelForm_Click">
                <i class="fa-solid fa-xmark me-1"></i>Cancel
            </asp:LinkButton>
        </div>
        <div class="card-body">
            <asp:HiddenField ID="hfMemberId" runat="server" Value="0" />
            <div class="row g-3">
                <div class="col-md-6">
                    <label class="form-label fw-semibold">Full Name <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Full name" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFullName" ValidationGroup="MemberForm"
                        CssClass="text-danger small" Display="Dynamic" ErrorMessage="Full name is required." />
                </div>
                <div class="col-md-6">
                    <label class="form-label fw-semibold">NIC <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtNIC" runat="server" CssClass="form-control" placeholder="e.g. 123456789V" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNIC" ValidationGroup="MemberForm"
                        CssClass="text-danger small" Display="Dynamic" ErrorMessage="NIC is required." />
                </div>
                <div class="col-md-6">
                    <label class="form-label fw-semibold">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="email@example.com" />
                </div>
                <div class="col-md-6">
                    <label class="form-label fw-semibold">Phone</label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="e.g. 0771234567" />
                </div>
                <div class="col-md-9">
                    <label class="form-label fw-semibold">Address</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Address" />
                </div>
                <div class="col-md-3">
                    <label class="form-label fw-semibold">Status</label>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                        <asp:ListItem Value="Active">Active</asp:ListItem>
                        <asp:ListItem Value="Inactive">Inactive</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="mt-3">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save Member"
                    ValidationGroup="MemberForm" OnClick="btnSave_Click" />
            </div>
        </div>
    </asp:Panel>

    <div class="card">
        <div class="card-header d-flex flex-wrap gap-2 justify-content-between align-items-center">
            <span><i class="fa-solid fa-list me-2"></i>All Members</span>
            <div class="d-flex gap-2 flex-wrap">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm" placeholder="Search name, NIC..." style="width:220px;" />
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="Search" OnClick="btnSearch_Click" />
                <asp:Button ID="btnClearSearch" runat="server" CssClass="btn btn-sm btn-outline-secondary" Text="Clear" OnClick="btnClearSearch_Click" />
                <asp:Button ID="btnAddNew" runat="server" CssClass="btn btn-sm btn-primary" Text="+ Add Member" OnClick="btnAddNew_Click" />
            </div>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvMembers" runat="server" CssClass="table table-hover mb-0"
                    AutoGenerateColumns="false" GridLines="None"
                    EmptyDataText="No members found."
                    OnRowCommand="gvMembers_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                        <asp:BoundField DataField="NIC" HeaderText="NIC" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="Phone" HeaderText="Phone" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="RegisteredDate" HeaderText="Registered" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="EditMember" CommandArgument='<%# Eval("MemberId") %>'
                                    CssClass="btn btn-sm btn-outline-primary me-1">
                                    <i class="fa-solid fa-pen"></i>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="DeleteMember" CommandArgument='<%# Eval("MemberId") %>'
                                    CssClass="btn btn-sm btn-outline-danger"
                                    OnClientClick="return confirm('Delete this member?');">
                                    <i class="fa-solid fa-trash"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <div class="card-footer d-flex justify-content-between align-items-center">
            <small class="text-muted"><asp:Literal ID="litPageInfo" runat="server" /></small>
            <div class="d-flex gap-1">
                <asp:LinkButton ID="btnPrev" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnPrev_Click">&#8249; Prev</asp:LinkButton>
                <asp:LinkButton ID="btnNext" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnNext_Click">Next &#8250;</asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
