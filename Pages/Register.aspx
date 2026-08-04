<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="libraryManagementSystem.Pages.Register" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Register - Library Management System</title>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/5.3.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" rel="stylesheet" />
    <link href="../Contents/css/site.css" rel="stylesheet" />
</head>
<body class="login-page">
    <form id="form1" runat="server">
        <div class="card login-card">
            <div class="card-body">
                <div class="text-center mb-4">
                    <div class="login-logo">
                        <i class="fa-solid fa-user-plus"></i>
                    </div>
                    <h4 class="fw-bold mb-0">Library Management System</h4>
                    <p class="text-muted small">Create a new account</p>
                </div>

                <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger py-2" Visible="false">
                    <asp:Literal ID="litError" runat="server" />
                </asp:Panel>
                <asp:Panel ID="pnlSuccess" runat="server" CssClass="alert alert-success py-2" Visible="false">
                    <asp:Literal ID="litSuccess" runat="server" />
                </asp:Panel>

                <div class="mb-3">
                    <label for="<%= txtFullName.ClientID %>" class="form-label">Full Name</label>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Enter your full name" autocomplete="name" />
                    <asp:RequiredFieldValidator ID="rfvFullName" runat="server"
                        ControlToValidate="txtFullName" Display="Dynamic" CssClass="text-danger small"
                        ErrorMessage="Full name is required." ValidationGroup="RegisterGroup" />
                </div>

                <div class="mb-3">
                    <label for="<%= txtUsername.ClientID %>" class="form-label">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Choose a username" autocomplete="username" />
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server"
                        ControlToValidate="txtUsername" Display="Dynamic" CssClass="text-danger small"
                        ErrorMessage="Username is required." ValidationGroup="RegisterGroup" />
                </div>

                <div class="mb-3">
                    <label for="<%= txtEmail.ClientID %>" class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="Enter your email" autocomplete="email" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                        ControlToValidate="txtEmail" Display="Dynamic" CssClass="text-danger small"
                        ErrorMessage="Email is required." ValidationGroup="RegisterGroup" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server"
                        ControlToValidate="txtEmail" Display="Dynamic" CssClass="text-danger small"
                        ErrorMessage="Enter a valid email address." ValidationGroup="RegisterGroup"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />
                </div>

                <div class="mb-3">
                    <label for="<%= txtPassword.ClientID %>" class="form-label">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Create a password" autocomplete="new-password" />
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                        ControlToValidate="txtPassword" Display="Dynamic" CssClass="text-danger small"
                        ErrorMessage="Password is required." ValidationGroup="RegisterGroup" />
                    <asp:RegularExpressionValidator ID="revPassword" runat="server"
                        ControlToValidate="txtPassword" Display="Dynamic" CssClass="text-danger small"
                        ErrorMessage="Password must be at least 6 characters." ValidationGroup="RegisterGroup"
                        ValidationExpression=".{6,}" />
                </div>

                <div class="mb-3">
                    <label for="<%= txtConfirmPassword.ClientID %>" class="form-label">Confirm Password</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Re-enter your password" autocomplete="new-password" />
                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server"
                        ControlToValidate="txtConfirmPassword" Display="Dynamic" CssClass="text-danger small"
                        ErrorMessage="Please confirm your password." ValidationGroup="RegisterGroup" />
                    <asp:CompareValidator ID="cvPassword" runat="server"
                        ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword"
                        Display="Dynamic" CssClass="text-danger small"
                        ErrorMessage="Passwords do not match." ValidationGroup="RegisterGroup" />
                </div>

                <asp:Button ID="btnRegister" runat="server" Text="Create Account" CssClass="btn btn-primary w-100 py-2"
                    ValidationGroup="RegisterGroup" OnClick="btnRegister_Click" />

                <p class="text-center text-muted small mt-4 mb-0">
                    Already have an account? <a href="../Login.aspx">Sign in</a>
                </p>
                <p class="text-center text-muted small mt-2 mb-0">
                    &copy; <%= DateTime.Now.Year %> Library Management System
                </p>
            </div>
        </div>
    </form>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/5.3.2/js/bootstrap.bundle.min.js"></script>
</body>
</html>