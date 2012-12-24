<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TagThis.HtmlHelpers" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>test</title>
</head>
<body>
    <div>
       
    <h2>test</h2>
        <% using (Html.BeginForm())
           { %>
<%=Html.GenerateCaptcha()%>


            <p>
                    <input type="submit" value="Register" /><%=Html.ViewData["check"].ToString()%>
                </p>
<%} %>
 
    </div>
</body>
</html>
