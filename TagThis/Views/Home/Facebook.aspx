<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Facebook</title>
</head>
<body>
    <div>
        <img src="https://graph.facebook.com/<%= ViewBag.id %>/picture" />
        <br />
        <%= Html.Encode(ViewBag.name) %>
        <br />
        <%= Html.Encode(ViewBag.id) %>
        <br />
        <%= Html.Encode(ViewBag.gender) %>
        <br />
        <%= Html.Encode(ViewBag.birthday) %>
        <br />
        <%= Html.Encode(ViewBag.email) %>
        <br />
    </div>
</body>
</html>
