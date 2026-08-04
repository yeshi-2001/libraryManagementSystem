<%@ Page Title="Categories" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="libraryManagementSystem.Pages.Categories" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Categories</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="PageHeaderContent" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <i class="fa-solid fa-tags me-2"></i>Book Categories
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert alert-dismissible fade show">
        <asp:Literal ID="litAlert" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </asp:Panel>

    <div class="row g-4">
        <%-- ADD/EDIT FORM --%>
        <div class="col-lg-4">
            <div class="card">
                <div class="card-header"><i class="fa-solid fa-tag me-2"></i><asp:Literal ID="litFormTitle" runat="server" Text="Add Category" /></div>
                <div class="card-body">
                    <asp:HiddenField ID="hfCategoryName" runat="server" Value="" />
                    <div class="mb-3">
                        <label class="form-label fw-semibold">Category Name <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" placeholder="e.g. Fiction" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCategoryName" ValidationGroup="CatForm"
                            CssClass="text-danger small" Display="Dynamic" ErrorMessage="Category name is required." />
                    </div>
                    <div class="d-flex gap-2">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save"
                            ValidationGroup="CatForm" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-outline-secondary" Text="Clear"
                            OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </div>

        <%-- CATEGORY LIST --%>
        <div class="col-lg-8">
            <div class="card">
                <div class="card-header"><i class="fa-solid fa-list me-2"></i>All Categories</div>
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <asp:GridView ID="gvCategories" runat="server" CssClass="table table-hover mb-0"
                            AutoGenerateColumns="false" GridLines="None"
                            EmptyDataText="No categories found."
                            OnRowCommand="gvCategories_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="Category" HeaderText="Category Name" />
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CommandName="EditCat" CommandArgument='<%# Eval("Category") %>'
                                            CssClass="btn btn-sm btn-outline-primary me-1">
                                            <i class="fa-solid fa-pen"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
