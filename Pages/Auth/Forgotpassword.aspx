<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="libraryManagementSystem.Auth.ForgotPassword" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Forgot Password - Library Management System</title>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/5.3.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" rel="stylesheet" />
    <link href="../../Contents/css/site.css" rel="stylesheet" />
</head>
<body class="login-page">
    <form id="form1" runat="server">
        <div class="card login-card">
            <div class="card-body">
                <div class="text-center mb-4">
                    <div class="login-logo">
                        <i class="fa-solid fa-key"></i>
                    </div>
                    <h4 class="fw-bold mb-0">Forgot Password</h4>
                    <p class="text-muted small">Enter your username to reset your password</p>
                </div>

                <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger py-2" Visible="false">
                    <asp:Literal ID="litError" runat="server" />
                </asp:Panel>

                <asp:Panel ID="pnlResetLink" runat="server" CssClass="alert alert-success py-2 small" Visible="false">
                    A reset link was generated. Since email sending isn't configured yet,
                    here it is for testing:<br />
                    <a id="hlResetLink" runat="server">Reset Password Link</a>
                </asp:Panel>

                <asp:Panel ID="pnlForm" runat="server">
                    <div class="mb-3">
                        <label for="<%= txtEmail.ClientID %>" class="form-label">Username</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter your username" autocomplete="username" />
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                            ControlToValidate="txtEmail" Display="Dynamic" CssClass="text-danger small"
                            ErrorMessage="Username is required." ValidationGroup="ForgotGroup" />
                    </div>

                    <asp:Button ID="btnSendReset" runat="server" Text="Send Reset Link" CssClass="btn btn-primary w-100 py-2"
                        ValidationGroup="ForgotGroup" OnClick="btnSendReset_Click" />
                </asp:Panel>

                <p class="text-center text-muted small mt-4 mb-0">
                    Remembered your password? <a href="~/Login.aspx" runat="server">Sign in</a>
                </p>
            </div>
        </div>
    </form>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/5.3.2/js/bootstrap.bundle.min.js"></script>
</body>
</html>