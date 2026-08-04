<%@ Page Title="Borrow Book" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BorrowBook.aspx.cs" Inherits="libraryManagementSystem.Pages.BorrowBook" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Borrow Book</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="PageHeaderContent" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <i class="fa-solid fa-right-from-bracket me-2"></i>Borrow Book
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show">
        <asp:Literal ID="litAlert" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </asp:Panel>

    <div class="row g-4">
        <%-- ISSUE FORM --%>
        <div class="col-lg-5">
            <div class="card">
                <div class="card-header"><i class="fa-solid fa-book-open-reader me-2"></i>Issue Book</div>
                <div class="card-body">
                    <div class="mb-3">
                        <label class="form-label fw-semibold">Select Book <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlBook" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                            <asp:ListItem Value="">-- Select a Book --</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlBook" InitialValue=""
                            ValidationGroup="BorrowForm" CssClass="text-danger small" Display="Dynamic"
                            ErrorMessage="Please select a book." />
                    </div>
                    <div class="mb-3">
                        <label class="form-label fw-semibold">Select Member <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlMember" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                            <asp:ListItem Value="">-- Select a Member --</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlMember" InitialValue=""
                            ValidationGroup="BorrowForm" CssClass="text-danger small" Display="Dynamic"
                            ErrorMessage="Please select a member." />
                    </div>
                    <div class="mb-3">
                        <label class="form-label fw-semibold">Borrow Date</label>
                        <asp:TextBox ID="txtBorrowDate" runat="server" CssClass="form-control" TextMode="Date" Enabled="false" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label fw-semibold">Due Date <small class="text-muted">(14 days from today)</small></label>
                        <asp:TextBox ID="txtDueDate" runat="server" CssClass="form-control" TextMode="Date" Enabled="false" />
                    </div>
                    <asp:Button ID="btnIssue" runat="server" CssClass="btn btn-primary w-100"
                        Text="Issue Book" ValidationGroup="BorrowForm" OnClick="btnIssue_Click" />
                </div>
            </div>
        </div>

        <%-- RECENT BORROWS --%>
        <div class="col-lg-7">
            <div class="card">
                <div class="card-header"><i class="fa-solid fa-clock-rotate-left me-2"></i>Recent Borrow Records</div>
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <asp:GridView ID="gvRecent" runat="server" CssClass="table table-hover mb-0"
                            AutoGenerateColumns="false" GridLines="None" EmptyDataText="No records yet.">
                            <Columns>
                                <asp:BoundField DataField="BookTitle" HeaderText="Book" />
                                <asp:BoundField DataField="MemberName" HeaderText="Member" />
                                <asp:BoundField DataField="BorrowDate" HeaderText="Issued" DataFormatString="{0:dd MMM yyyy}" />
                                <asp:BoundField DataField="DueDate" HeaderText="Due" DataFormatString="{0:dd MMM yyyy}" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
