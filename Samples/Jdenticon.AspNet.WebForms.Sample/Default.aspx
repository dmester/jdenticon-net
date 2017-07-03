<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Jdenticon.AspNet.WebForms.Sample._Default" %>
<!DOCTYPE html>
<html>
<head>
    <title>Jdenticon ASP.NET WebForms sample</title>
</head>
<body>
    <h1>Jdenticon</h1>

    <p>This is a sample illustrating how Jdenticon can be used in ASP.NET WebForms environments.</p>

    <jdenticon:Icon runat="server" StringValue="TestIcon" />
    
    <asp:Repeater runat="server" ID="repeater">
        <ItemTemplate>
            <jdenticon:Icon runat="server" Value='<%# 
                Container.DataItem 
                %>' />
            <asp:Literal runat="server" Text='<%# 
                Container.DataItem 
                %>' />
        </ItemTemplate>
    </asp:Repeater>
</body>
</html>