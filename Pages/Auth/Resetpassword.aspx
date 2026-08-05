<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="libraryManagementSystem.Auth.ResetPassword" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Reset Password - Library Management System</title>
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
                        <i class="fa-solid fa-lock"></i>
                    </div>
                    <h4 class="fw-bold mb-0">Reset Password</h4>
                    <p class="text-muted small">Choose a new password for your account</p>
                </div>

                <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger py-2" Visible="false">
                    <asp:Literal ID="litError" runat="server" />
                </asp:Panel>

                <asp:Panel ID="pnlSuccess" runat="server" CssClass="alert alert-success py-2" Visible="false">
                    <asp:Literal ID="litSuccess" runat="server" />
                    <div class="mt-2">
                        <a href="~/Login.aspx" runat="server" class="btn btn-sm btn-outline-success">Go to Sign In</a>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlForm" runat="server">
                    <div class="mb-3">
                        <label for="<%= txtNewPassword.ClientID %>" class="form-label">New Password</label>
                        <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter new password" autocomplete="new-password" />
                        <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server"
                            ControlToValidate="txtNewPassword" Display="Dynamic" CssClass="text-danger small"
                            ErrorMessage="New password is required." ValidationGroup="ResetGroup" />
                        <asp:RegularExpressionValidator ID="revNewPassword" runat="server"
                            ControlToValidate="txtNewPassword" Display="Dynamic" CssClass="text-danger small"
                            ErrorMessage="Password must be at least 6 characters." ValidationGroup="ResetGroup"
                            ValidationExpression=".{6,}" />
                    </div>

                    <div class="mb-3">
                        <label for="<%= txtConfirmPassword.ClientID %>" class="form-label">Confirm New Password</label>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Re-enter new password" autocomplete="new-password" />
                        <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server"
                            ControlToValidate="txtConfirmPassword" Display="Dynamic" CssClass="text-danger small"
                            ErrorMessage="Please confirm your new password." ValidationGroup="ResetGroup" />
                        <asp:CompareValidator ID="cvPassword" runat="server"
                            ControlToValidate="txtConfirmPassword" ControlToCompare="txtNewPassword"
                            Display="Dynamic" CssClass="text-danger small"
                            ErrorMessage="Passwords do not match." ValidationGroup="ResetGroup" />
                    </div>

                    <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" CssClass="btn btn-primary w-100 py-2"
                        ValidationGroup="ResetGroup" OnClick="btnResetPassword_Click" />
                </asp:Panel>
            </div>
        </div>
    </form>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/5.3.2/js/bootstrap.bundle.min.js"></script>
</body>
</html>