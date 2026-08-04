<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="libraryManagementSystem.Login" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Login - Library Management System</title>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/5.3.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" rel="stylesheet" />
    <link href="Contents/css/site.css" rel="stylesheet" />
</head>
<body class="login-page">
    <form id="form1" runat="server">
        <div class="card login-card">
            <div class="card-body">
                <div class="text-center mb-4">
                    <div class="login-logo">
                        <i class="fa-solid fa-book"></i>
                    </div>
                    <h4 class="fw-bold mb-0">Library Management System</h4>
                    <p class="text-muted small">Sign in to continue</p>
                </div>
                <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger py-2" Visible="false">
                    <asp:Literal ID="litError" runat="server" />
                </asp:Panel>
                <div class="mb-3">
                    <label for="<%= txtUsername.ClientID %>" class="form-label">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter your username" autocomplete="username" />
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server"
                        ControlToValidate="txtUsername" Display="Dynamic" CssClass="text-danger small"
                        ErrorMessage="Username is required." ValidationGroup="LoginGroup" />
                </div>
                <div class="mb-3">
                    <label for="<%= txtPassword.ClientID %>" class="form-label">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter your password" autocomplete="current-password" />
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                        ControlToValidate="txtPassword" Display="Dynamic" CssClass="text-danger small"
                        ErrorMessage="Password is required." ValidationGroup="LoginGroup" />
                </div>
                <asp:Button ID="btnLogin" runat="server" Text="Sign In" CssClass="btn btn-primary w-100 py-2"
                    ValidationGroup="LoginGroup" OnClick="btnLogin_Click" />
                <p class="text-center text-muted small mt-3 mb-0">
                    Don't have an account? <a href="Pages/Register.aspx">Register</a>
                </p>
                <p class="text-center text-muted small mt-4 mb-0">
                    &copy; <%= DateTime.Now.Year %> Library Management System
                </p>
            </div>
        </div>
    </form>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/5.3.2/js/bootstrap.bundle.min.js"></script>
</body>
</html>