<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%string rating = ViewData["commentrating"].ToString(); %>
<%if (rating != "0"){ %> <%= Html.Encode(rating)%>
                <img src="../../content/icons/ksmall.png" style="margin:0 -2px -1px -7px;" />
                ·
                <%}%> 