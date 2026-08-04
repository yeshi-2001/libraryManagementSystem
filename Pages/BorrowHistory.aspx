<%@ Page Title="Borrow History" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BorrowHistory.aspx.cs" Inherits="libraryManagementSystem.Pages.BorrowHistory" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Borrow History</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="PageHeaderContent" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <i class="fa-solid fa-clock-rotate-left me-2"></i>Borrow History
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
        <div class="card-header d-flex flex-wrap gap-2 justify-content-between align-items-center">
            <span><i class="fa-solid fa-list me-2"></i>All Borrow Records</span>
            <div class="d-flex gap-2 flex-wrap">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm" placeholder="Search book or member..." style="width:200px;" />
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select form-select-sm" style="width:130px;">
                    <asp:ListItem Value="">All Status</asp:ListItem>
                    <asp:ListItem Value="Borrowed">Borrowed</asp:ListItem>
                    <asp:ListItem Value="Returned">Returned</asp:ListItem>
                    <asp:ListItem Value="Overdue">Overdue</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="Search" OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" runat="server" CssClass="btn btn-sm btn-outline-secondary" Text="Clear" OnClick="btnClear_Click" />
            </div>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvHistory" runat="server" CssClass="table table-hover mb-0"
                    AutoGenerateColumns="false" GridLines="None" EmptyDataText="No records found.">
                    <Columns>
                        <asp:BoundField DataField="BorrowId" HeaderText="#" />
                        <asp:BoundField DataField="BookTitle" HeaderText="Book" />
                        <asp:BoundField DataField="MemberName" HeaderText="Member" />
                        <asp:BoundField DataField="BorrowDate" HeaderText="Issued" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField DataField="DueDate" HeaderText="Due" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField DataField="ReturnDate" HeaderText="Returned" DataFormatString="{0:dd MMM yyyy}" NullDisplayText="-" />
                        <asp:BoundField DataField="Fine" HeaderText="Fine" DataFormatString="{0:F2}" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
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
