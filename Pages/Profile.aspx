<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="libraryManagementSystem.Pages.Profile" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">My Profile</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="PageHeaderContent" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <i class="fa-solid fa-user me-2"></i>My Profile
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show">
        <asp:Literal ID="litAlert" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </asp:Panel>

    <div class="row g-4">
        <div class="col-lg-4">
            <div class="card text-center">
                <div class="card-body py-4">
                    <span class="rounded-circle d-inline-flex align-items-center justify-content-center fw-bold text-white mb-3"
                        style="width:90px;height:90px;background:var(--lms-primary);font-size:2rem;">
                        <asp:Literal ID="litInitials" runat="server" />
                    </span>
                    <h5 class="fw-bold mb-0"><asp:Literal ID="litFullName" runat="server" /></h5>
                    <p class="text-muted mb-2">@<asp:Literal ID="litUsername" runat="server" /></p>
                    <span class="badge badge-status-active"><asp:Literal ID="litRole" runat="server" /></span>
                </div>
            </div>
        </div>

        <div class="col-lg-8">
            <div class="card">
                <div class="card-header"><i class="fa-solid fa-id-card me-2"></i>Account Details</div>
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-sm-4 text-muted small text-uppercase">Full Name</div>
                        <div class="col-sm-8 fw-semibold"><asp:Literal ID="litDetailFullName" runat="server" /></div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-sm-4 text-muted small text-uppercase">Username</div>
                        <div class="col-sm-8 fw-semibold"><asp:Literal ID="litDetailUsername" runat="server" /></div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-sm-4 text-muted small text-uppercase">Role</div>
                        <div class="col-sm-8 fw-semibold"><asp:Literal ID="litDetailRole" runat="server" /></div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-muted small text-uppercase">User ID</div>
                        <div class="col-sm-8 fw-semibold"><asp:Literal ID="litDetailUserId" runat="server" /></div>
                    </div>

                  
                </div>
            </div>
        </div>
    </div>
</asp:Content>