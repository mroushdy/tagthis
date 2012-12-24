<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%
   var random = new Random(System.DateTime.Now.Millisecond); 
   int r = random.Next(1, 5000000);
   ViewData["settings"] = ViewData["settings"];
   ViewData["random"] = r;
%>

<div class="clearfix" id="data-<%= Html.Encode(r)%>">
<% Html.RenderPartial("PaginationPage"); %>
</div>