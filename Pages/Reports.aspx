<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="libraryManagementSystem.Pages.Reports" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Reports</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="PageHeaderContent" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <i class="fa-solid fa-chart-column me-2"></i>Reports
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <%-- SUMMARY CARDS --%>
    <div class="row g-3 mb-4">
        <div class="col-6 col-md-3">
            <div class="card stat-card text-center py-3">
                <i class="fa-solid fa-book fa-2x text-primary mb-2"></i>
                <h3 class="fw-bold mb-0"><asp:Literal ID="litTotalBooks" runat="server" Text="0" /></h3>
                <small class="text-muted">Total Books</small>
            </div>
        </div>
        <div class="col-6 col-md-3">
            <div class="card stat-card text-center py-3">
                <i class="fa-solid fa-users fa-2x text-success mb-2"></i>
                <h3 class="fw-bold mb-0"><asp:Literal ID="litTotalMembers" runat="server" Text="0" /></h3>
                <small class="text-muted">Total Members</small>
            </div>
        </div>
        <div class="col-6 col-md-3">
            <div class="card stat-card text-center py-3">
                <i class="fa-solid fa-book-open-reader fa-2x text-warning mb-2"></i>
                <h3 class="fw-bold mb-0"><asp:Literal ID="litBorrowed" runat="server" Text="0" /></h3>
                <small class="text-muted">Currently Borrowed</small>
            </div>
        </div>
        <div class="col-6 col-md-3">
            <div class="card stat-card text-center py-3">
                <i class="fa-solid fa-triangle-exclamation fa-2x text-danger mb-2"></i>
                <h3 class="fw-bold mb-0"><asp:Literal ID="litOverdue" runat="server" Text="0" /></h3>
                <small class="text-muted">Overdue Books</small>
            </div>
        </div>
    </div>

    <%-- OVERDUE BOOKS --%>
    <div class="card mb-4">
        <div class="card-header text-danger fw-bold">
            <i class="fa-solid fa-triangle-exclamation me-2"></i>Overdue Books
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvOverdue" runat="server" CssClass="table table-hover mb-0"
                    AutoGenerateColumns="false" GridLines="None" EmptyDataText="No overdue books.">
                    <Columns>
                        <asp:BoundField DataField="BookTitle" HeaderText="Book" />
                        <asp:BoundField DataField="MemberName" HeaderText="Member" />
                        <asp:BoundField DataField="NIC" HeaderText="NIC" />
                        <asp:BoundField DataField="BorrowDate" HeaderText="Issued" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField DataField="DueDate" HeaderText="Due" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:TemplateField HeaderText="Days Overdue">
                            <ItemTemplate><%# ((DateTime.Today - (DateTime)Eval("DueDate")).Days) %> days</ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <%-- FULL BORROW HISTORY --%>
    <div class="card">
        <div class="card-header fw-bold">
            <i class="fa-solid fa-clock-rotate-left me-2"></i>Full Borrow History
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvAllBorrows" runat="server" CssClass="table table-hover mb-0"
                    AutoGenerateColumns="false" GridLines="None" EmptyDataText="No records.">
                    <Columns>
                        <asp:BoundField DataField="BookTitle" HeaderText="Book" />
                        <asp:BoundField DataField="MemberName" HeaderText="Member" />
                        <asp:BoundField DataField="BorrowDate" HeaderText="Issued" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField DataField="DueDate" HeaderText="Due" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField DataField="ReturnDate" HeaderText="Returned" DataFormatString="{0:dd MMM yyyy}" NullDisplayText="-" />
                        <asp:BoundField DataField="Fine" HeaderText="Fine (Rs.)" DataFormatString="{0:F2}" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
