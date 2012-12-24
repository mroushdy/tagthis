<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Index</title>
</head>
<body>
    <div>

    <% TagThis.Controllers.Embed e = (TagThis.Controllers.Embed)ViewData["e"];  %>
    
    <%=Html.Encode(e.type) %><br /><%=Html.Encode(e.url) %>

    <br />

    <img id="Img1" src="../../GetThumb?u=<%=e.thumb %>" class="slide" alt="" />
    
    <%= Html.Encode(e.title) %>
    <br />
    <%= Html.Encode(e.description) %>
        <br />
    <%= e.html %>
    <br />
    <br />
    <br />




    </div>
</body>
</html>
