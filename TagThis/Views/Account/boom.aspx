<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>boom</title>
</head>
<body>
    <div>
        

        <%
      

            foreach (dynamic link in ViewData["results"] as dynamic)
            {

                if (link.picture != null)
                {%>
             <img src="<%= link.picture %>" />
           <% }%>
           

              <%= Html.Encode(link.owner_comment)%>
              <br />
               <%= Html.Encode(link.title)%>
               <br />
                <%= Html.Encode(link.summary)%>
                <br />
                 <%= Html.Encode(link.url)%>
                
                <br /><br /><br /><br />
            <%}
            
             %>


    </div>
</body>
</html>
