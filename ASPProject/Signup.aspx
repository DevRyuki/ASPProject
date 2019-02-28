<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="ASPProject.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Signup<br />
            <br />
            Username:
            <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
            <br />
            <br />
            Password:
            <asp:TextBox ID="Password" runat="server"></asp:TextBox>
            <br />
            <br />
            Email:
            <asp:TextBox ID="Email" runat="server" Height="16px" Width="142px"></asp:TextBox>
            <br />
            <br />
            Phone Number:
            <asp:TextBox ID="PhoneNum" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="Signup" runat="server" Text="Button" />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
        </div>
    </form>
</body>
</html>
