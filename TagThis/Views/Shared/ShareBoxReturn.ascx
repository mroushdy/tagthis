<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div id = "s-error" class="invisible">
<%
    if (ViewData["error"] != null)
    {%>
        <%= Html.Encode(ViewData["error"].ToString()) %>
    <%}
%>
</div>


<div id = "s-p-result" class="invisible">
<%
    if (ViewData["page"] != null)
    {
        
        ViewData["ResultSingle"] = ViewData["page"];
        Html.RenderPartial("ResultSingle");
    }
%>
</div>
