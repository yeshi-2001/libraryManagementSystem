<%@ Page Title="Return Book" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReturnBook.aspx.cs" Inherits="libraryManagementSystem.Pages.ReturnBook" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Return Book</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="PageHeaderContent" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <i class="fa-solid fa-right-to-bracket me-2"></i>Return Book
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show">
        <asp:Literal ID="litAlert" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </asp:Panel>

    <div class="card">
        <div class="card-header d-flex flex-wrap gap-2 justify-content-between align-items-center">
            <span><i class="fa-solid fa-list me-2"></i>Currently Borrowed Books</span>
            <div class="d-flex gap-2">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm" placeholder="Search book or member..." style="width:220px;" />
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="Search" OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" runat="server" CssClass="btn btn-sm btn-outline-secondary" Text="Clear" OnClick="btnClear_Click" />
            </div>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvBorrowed" runat="server" CssClass="table table-hover mb-0"
                    AutoGenerateColumns="false" GridLines="None"
                    EmptyDataText="No borrowed books found."
                    OnRowCommand="gvBorrowed_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="BorrowId" HeaderText="#" />
                        <asp:BoundField DataField="BookTitle" HeaderText="Book" />
                        <asp:BoundField DataField="MemberName" HeaderText="Member" />
                        <asp:BoundField DataField="BorrowDate" HeaderText="Issued" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField DataField="DueDate" HeaderText="Due" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:TemplateField HeaderText="Fine (est.)">
                            <ItemTemplate><%# GetEstimatedFine(Eval("DueDate"), Eval("Status").ToString()) %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="ReturnBook" CommandArgument='<%# Eval("BorrowId") %>'
                                    CssClass="btn btn-sm btn-success"
                                    OnClientClick="return confirm('Confirm return of this book?');">
                                    <i class="fa-solid fa-rotate-left me-1"></i>Return
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
