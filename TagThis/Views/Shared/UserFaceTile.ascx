<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% Guid UserId = (Guid)ViewData["Item"]; %>
<% TagThis.Models.UserRepository ur = new TagThis.Models.UserRepository();%>

<a href="<%= Url.Action(Membership.GetUser(UserId).UserName, "Users") %>" data-tooltip = "<%= ur.GetFullName(UserId) %>" class="ajax"><span><img src='<%=  ur.GetProfileImage(UserId,"mini") %>' width="48" height="48"/></span></a>