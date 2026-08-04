<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="libraryManagementSystem.Pages.Dashboard" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Dashboard</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="PageHeaderContent" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <i class="fa-solid fa-gauge-high me-2"></i>Dashboard
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="false">
        <asp:Literal ID="litError" runat="server" />
    </asp:Panel>

    <h5 class="fw-bold mb-1">Welcome back<asp:Literal ID="litWelcomeName" runat="server" />!</h5>
    <p class="text-muted mb-4">Here's what's happening in your library today.</p>

    <%-- STAT CARDS --%>
    <div class="row g-3 mb-4">
        <div class="col-6 col-md-3">
            <div class="card stat-card text-center py-3 px-2">
                <i class="fa-solid fa-book fa-2x text-primary mb-2"></i>
                <h3 class="fw-bold mb-0"><asp:Literal ID="litTotalBooks" runat="server" Text="0" /></h3>
                <small class="text-muted">Total Books</small>
            </div>
        </div>
        <div class="col-6 col-md-3">
            <div class="card stat-card text-center py-3 px-2">
                <i class="fa-solid fa-users fa-2x text-success mb-2"></i>
                <h3 class="fw-bold mb-0"><asp:Literal ID="litTotalMembers" runat="server" Text="0" /></h3>
                <small class="text-muted">Total Members</small>
            </div>
        </div>
        <div class="col-6 col-md-3">
            <div class="card stat-card text-center py-3 px-2">
                <i class="fa-solid fa-book-open-reader fa-2x text-warning mb-2"></i>
                <h3 class="fw-bold mb-0"><asp:Literal ID="litBorrowedBooks" runat="server" Text="0" /></h3>
                <small class="text-muted">Books Borrowed</small>
            </div>
        </div>
        <div class="col-6 col-md-3">
            <div class="card stat-card text-center py-3 px-2">
                <i class="fa-solid fa-triangle-exclamation fa-2x text-danger mb-2"></i>
                <h3 class="fw-bold mb-0"><asp:Literal ID="litOverdueBooks" runat="server" Text="0" /></h3>
                <small class="text-muted">Overdue Books</small>
            </div>
        </div>
    </div>

    <%-- QUICK LINKS --%>
    <div class="row g-3 mb-4">
        <div class="col-6 col-md-3">
            <a href="BookList.aspx" class="card stat-card text-center py-3 px-2 text-decoration-none d-block">
                <i class="fa-solid fa-plus-circle fa-2x mb-2" style="color:var(--lms-primary);"></i>
                <div class="small fw-semibold">Add Book</div>
            </a>
        </div>
        <div class="col-6 col-md-3">
            <a href="MemberList.aspx" class="card stat-card text-center py-3 px-2 text-decoration-none d-block">
                <i class="fa-solid fa-user-plus fa-2x text-success mb-2"></i>
                <div class="small fw-semibold">Add Member</div>
            </a>
        </div>
        <div class="col-6 col-md-3">
            <a href="BorrowBook.aspx" class="card stat-card text-center py-3 px-2 text-decoration-none d-block">
                <i class="fa-solid fa-right-from-bracket fa-2x text-warning mb-2"></i>
                <div class="small fw-semibold">Borrow Book</div>
            </a>
        </div>
        <div class="col-6 col-md-3">
            <a href="ReturnBook.aspx" class="card stat-card text-center py-3 px-2 text-decoration-none d-block">
                <i class="fa-solid fa-right-to-bracket fa-2x text-info mb-2"></i>
                <div class="small fw-semibold">Return Book</div>
            </a>
        </div>
    </div>

    <%-- RECENT BORROWS --%>
    <div class="card">
        <div class="card-header fw-bold">
            <i class="fa-solid fa-clock-rotate-left me-2"></i>Recently Borrowed Books
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvRecentBorrowed" runat="server" CssClass="table table-hover mb-0"
                    AutoGenerateColumns="false" GridLines="None" EmptyDataText="No recent activity to show.">
                    <Columns>
                        <asp:BoundField DataField="BookTitle" HeaderText="Book Title" />
                        <asp:BoundField DataField="MemberName" HeaderText="Borrowed By" />
                        <asp:BoundField DataField="BorrowDate" HeaderText="Borrow Date" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
