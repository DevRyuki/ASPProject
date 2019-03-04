<%@ Page Language="C#" AutoEventWireup="true"  CodeBehind="Login.aspx.cs" Inherits="ASPProject.Login" %>



   
</asp:Content>
<form id="form1" runat="server">
    <p>
        Username:
        <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
    </p>
    <p>
        Password:
        <asp:TextBox ID="Password" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:Button ID="Log" runat="server" OnClick="Log_Click" Text="Login" />
&nbsp;&nbsp;
        <asp:Button ID="Signup" runat="server" OnClick="Signup_Click" Text="Signup" />
    </p>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Login %>" SelectCommand="SELECT * FROM [Users]"></asp:SqlDataSource>
</form>
