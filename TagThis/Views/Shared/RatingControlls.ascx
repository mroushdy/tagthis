<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% TagThis.Controllers.utils u = new TagThis.Controllers.utils();  %>
<% int rating =(int)ViewData["rating"]; %>

    <%if (ViewData["oRate"].ToString() != "1"){%> 
    <%= Ajax.ActionLink(" Like", "LHF", "Ajax", new { pid = ViewData["pageid"].ToString(), value = "l", postid = ViewData["postid"].ToString() }, new AjaxOptions() { UpdateTargetId = "rating" + ViewData["pageid"].ToString(), OnBegin = "function(){$('#likes" + ViewData["pageid"].ToString() + "').text('" + (rating + 1).ToString() + " Likes');}" }, new { Class = "BlackButton", id = "l" + ViewData["pageid"].ToString() })%>
    <%} else {%>
    <%= Ajax.ActionLink(" Unlike ", "LHF", "Ajax", new { pid = ViewData["pageid"].ToString(), value = "l", postid = ViewData["postid"].ToString() }, new AjaxOptions() { UpdateTargetId = "rating" + ViewData["pageid"].ToString(), OnBegin = "function(){$('#likes" + ViewData["pageid"].ToString() + "').text('" + (rating - 1).ToString() + " Likes');}" }, new { Class = "BlackButton", id = "l" + ViewData["pageid"].ToString() })%>
    <%}%>