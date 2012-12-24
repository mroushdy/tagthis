<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%  TagThis.Models.SearchRepository sr = new TagThis.Models.SearchRepository();
    List<TagThis.Models.SuggestionTags> st = sr.MostPopularTags("1y").Take(10).ToList();  %>

<div style="float:left; position:absolute;">
<h2 style="padding-bottom:10px; padding-top:25px;">Explore</h2>
<ul>

<%foreach(TagThis.Models.SuggestionTags s in st){ %>

<li><a title="<%= Html.Encode(s.tagname) %>" href="../../Search/<%=Html.Encode(s.tagname)%>"><%= Html.Encode(s.tagname) %></a></li>

<% } %>

</ul>
</div>



             

 