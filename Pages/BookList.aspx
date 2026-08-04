<%@ Page Title="Books" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookList.aspx.cs" Inherits="libraryManagementSystem.Pages.BookList" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Books</asp:Content>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ID="PageHeaderContent" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <i class="fa-solid fa-book me-2"></i>Books
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show">
        <asp:Literal ID="litAlert" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </asp:Panel>

    <%-- ADD / EDIT FORM --%>
    <asp:Panel ID="pnlForm" runat="server" Visible="false" CssClass="card mb-4">
        <div class="card-header d-flex justify-content-between align-items-center">
            <span><i class="fa-solid fa-pen-to-square me-2"></i><asp:Literal ID="litFormTitle" runat="server" Text="Add Book" /></span>
            <asp:LinkButton ID="btnCancelForm" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnCancelForm_Click">
                <i class="fa-solid fa-xmark me-1"></i>Cancel
            </asp:LinkButton>
        </div>
        <div class="card-body">
            <asp:HiddenField ID="hfBookId" runat="server" Value="0" />
            <div class="row g-3">
                <div class="col-md-4">
                    <label class="form-label fw-semibold">ISBN <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtISBN" runat="server" CssClass="form-control" placeholder="e.g. 978-3-16-148410-0" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtISBN" ValidationGroup="BookForm"
                        CssClass="text-danger small" Display="Dynamic" ErrorMessage="ISBN is required." />
                </div>
                <div class="col-md-8">
                    <label class="form-label fw-semibold">Title <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="Book title" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitle" ValidationGroup="BookForm"
                        CssClass="text-danger small" Display="Dynamic" ErrorMessage="Title is required." />
                </div>
                <div class="col-md-4">
                    <label class="form-label fw-semibold">Author <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtAuthor" runat="server" CssClass="form-control" placeholder="Author name" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAuthor" ValidationGroup="BookForm"
                        CssClass="text-danger small" Display="Dynamic" ErrorMessage="Author is required." />
                </div>
                <div class="col-md-4">
                    <label class="form-label fw-semibold">Category</label>
                    <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control" placeholder="e.g. Fiction" />
                </div>
                <div class="col-md-4">
                    <label class="form-label fw-semibold">Publisher</label>
                    <asp:TextBox ID="txtPublisher" runat="server" CssClass="form-control" placeholder="Publisher name" />
                </div>
                <div class="col-md-3">
                    <label class="form-label fw-semibold">Publish Year</label>
                    <asp:TextBox ID="txtPublishYear" runat="server" CssClass="form-control" TextMode="Number" placeholder="e.g. 2020" />
                </div>
                <div class="col-md-3">
                    <label class="form-label fw-semibold">Quantity <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" TextMode="Number" Text="1" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuantity" ValidationGroup="BookForm"
                        CssClass="text-danger small" Display="Dynamic" ErrorMessage="Quantity is required." />
                </div>
                <div class="col-md-3">
                    <label class="form-label fw-semibold">Shelf Location</label>
                    <asp:TextBox ID="txtShelfLocation" runat="server" CssClass="form-control" placeholder="e.g. A-12" />
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
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save Book"
                    ValidationGroup="BookForm" OnClick="btnSave_Click" />
            </div>
        </div>
    </asp:Panel>

    <%-- BOOK LIST --%>
    <div class="card">
        <div class="card-header d-flex flex-wrap gap-2 justify-content-between align-items-center">
            <span><i class="fa-solid fa-list me-2"></i>All Books</span>
            <div class="d-flex gap-2 flex-wrap">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm" placeholder="Search title, ISBN, author..." style="width:220px;" />
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-outline-primary" Text="Search" OnClick="btnSearch_Click" />
                <asp:Button ID="btnClearSearch" runat="server" CssClass="btn btn-sm btn-outline-secondary" Text="Clear" OnClick="btnClearSearch_Click" />
                <asp:Button ID="btnAddNew" runat="server" CssClass="btn btn-sm btn-primary" Text="+ Add Book" OnClick="btnAddNew_Click" />
            </div>
        </div>
        <div class="card-body p-0">
            <div class="table-responsive">
                <asp:GridView ID="gvBooks" runat="server" CssClass="table table-hover mb-0"
                    AutoGenerateColumns="false" GridLines="None"
                    EmptyDataText="No books found."
                    OnRowCommand="gvBooks_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="ISBN" HeaderText="ISBN" />
                        <asp:BoundField DataField="Title" HeaderText="Title" />
                        <asp:BoundField DataField="Author" HeaderText="Author" />
                        <asp:BoundField DataField="Category" HeaderText="Category" />
                        <asp:BoundField DataField="Quantity" HeaderText="Qty" />
                        <asp:BoundField DataField="AvailableQuantity" HeaderText="Available" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="EditBook" CommandArgument='<%# Eval("BookId") %>'
                                    CssClass="btn btn-sm btn-outline-primary me-1">
                                    <i class="fa-solid fa-pen"></i>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="DeleteBook" CommandArgument='<%# Eval("BookId") %>'
                                    CssClass="btn btn-sm btn-outline-danger"
                                    OnClientClick="return confirm('Delete this book?');">
                                    <i class="fa-solid fa-trash"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <%-- Pagination --%>
        <div class="card-footer d-flex justify-content-between align-items-center">
            <small class="text-muted"><asp:Literal ID="litPageInfo" runat="server" /></small>
            <div class="d-flex gap-1">
                <asp:LinkButton ID="btnPrev" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnPrev_Click">&#8249; Prev</asp:LinkButton>
                <asp:LinkButton ID="btnNext" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnNext_Click">Next &#8250;</asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
