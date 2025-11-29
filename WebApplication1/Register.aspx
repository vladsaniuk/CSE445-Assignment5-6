<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="WebApplication1.Register" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Member Registration</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Member Registration</h1>
        <p>Create a new member account to access member-only features.</p>
        
        <div>
            <label>Username:</label>
            <asp:TextBox ID="txtUsername" runat="server" />
        </div>
        <div>
            <label>Password:</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
        </div>
        <div>
            <label>Confirm Password:</label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" />
        </div>
        <div>
            <label>Email (Optional):</label>
            <asp:TextBox ID="txtEmail" runat="server" />
        </div>
        <div>
            <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" />
        </div>
        <div>
            <asp:Label ID="lblRegisterMessage" runat="server" ForeColor="Red" />
        </div>
        <div>
            <asp:HyperLink ID="hlBackToLogin" runat="server" 
                          NavigateUrl="~/Login.aspx" 
                          Text="Already have an account? Go to Login" />
        </div>
    </form>
</body>
</html>