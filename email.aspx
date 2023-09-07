<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="email.aspx.cs" Inherits="testeemail.email" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
    <asp:Label ID="lblemail" runat="server" >Digite seu email novo: </asp:Label>
        <asp:TextBox ID="txtEnviaEmail" runat="server"></asp:TextBox>
        <asp:Button ID="BtnEnviar" runat="server" Text="Enviar Planilha por Email" OnClick="BtnEnviar_Click" />
            <br />
            <br />
        <asp:Label ID="lblMensagem" runat="server" ></asp:Label>
            </div>
</form>
    
</body>
</html>
