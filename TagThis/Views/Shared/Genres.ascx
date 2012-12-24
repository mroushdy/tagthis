<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%TagThis.Controllers.OmniResults results = (TagThis.Controllers.OmniResults)ViewData["results"]; %>
<%TagThis.Controllers.utils u = new TagThis.Controllers.utils(); %>

<%  //split genres into two columns
    TagThis.Models.TagRepository tr = new TagThis.Models.TagRepository();
    List<string> g = tr.GetXmlMainGenres().OrderBy(o => o).ToList<string>();
    g.Insert(0, "Everything");
    int rows = g.Count;
    int col1rows = 0;
    int col2rows = 0;
    if (rows % 2 == 0) { col1rows = col2rows = (rows / 2); } // even number of rows
    else { col1rows = (rows + 1) / 2; col2rows = rows - col1rows; } // odd number of rows

    var col1 = g.Take(col1rows).ToList(); 
    var col2 = g.Skip(col1rows).Take(col2rows);

    
    //check if we are on a users profile
    string username = "";
    if (results.SearchWhat.StartsWith("!user")) { username = results.SearchWhat.Substring(results.SearchWhat.IndexOf(';') + 1); }
      %>

<div class="resultsbar">

<ul class="PostHeaderContainer">

<%if(Request.IsAuthenticated){ %>

<li class="submenu" id="feedselector" style="position:relative;">
<a class="nav " href="javascript:void(0)">
<%if(username != "") {%> <%= Html.Encode(u.MakeFirstUpper(username)) %> <%} else if (results.SearchWhat == "everyone"){%>Everyone<%} else if (results.SearchWhat == "suggestions"){%>Similar people<%} else {%>People you follow<%}%>
<span></span>
</a>
&nbsp;·
<ul>
<%if(username != "") {%> <li><%=Html.ActionLink(u.MakeFirstUpper(username) , results.query, "Search", new { sort = results.sort, time = results.time, SearchWhat = results.SearchWhat }, new { id = "aeveryone", Class = "ajax" })%></li> <%} %>
<li><%=Html.ActionLink("Everyone", results.query, "Search", new { sort = results.sort, time = results.time, SearchWhat = "everyone" }, new { id = "aeveryone", Class = "ajax" })%></li>
<li><%=Html.ActionLink("Similar people", results.query, "Search", new { sort = results.sort, time = results.time, SearchWhat = "suggestions" }, new { id = "asuggestions", Class = "ajax" })%></li>
<li><%=Html.ActionLink("People you follow", results.query, "Search", new { sort = results.sort, time = results.time, SearchWhat = "following" }, new { id = "afollowing", Class = "ajax" })%></li>
</ul>
</li>

<%} %>

<li class="submenu">
<a class="nav" href="javascript:void(0)">
<%if (g.Contains(results.query.Trim(),StringComparer.OrdinalIgnoreCase)){%>Genre: <%=Html.Encode(u.MakeFirstUpper(results.query.ToLower()))%> <%} else {%>Genre: Everything <%} %>
<span></span>
</a>
&nbsp;·
<ul id="CategoriesDropdown">
<li>
<span class="SubmenuColumn">
<%foreach (string genre in col1)
  {  %>
<%=Html.ActionLink(genre, genre, "Search", new {  sort = results.sort, results.time, SearchWhat = results.SearchWhat }, new { Class="ajax" })%>
<%} %>
</span>
<span class="SubmenuColumn">
<%foreach (string genre in col2)
  {  %>
<%=Html.ActionLink(genre, genre, "Search", new { sort = results.sort, results.time, SearchWhat = results.SearchWhat }, new { Class = "ajax" })%>
<%} %>
</span>
</li>
</ul>
</li>



<li class="submenu">
<a class="nav" href="javascript:void(0)">
<%if(results.sort == "newest") {%> Latest <%} else if (results.time == "now"){%>Popular Now<%} else if (results.time == "1w"){%>Popular This Week<%} else if (results.time == "1m"){%>Popular This Month<%} else if (results.time == "1y"){%>Popular This Year<%} else {%>Popular All Time<%} %>
<span></span>
</a>
<ul>
<li><%=Html.ActionLink("Latest", results.query, "Search", new {  sort = "newest", time = "irrelevant", SearchWhat = results.SearchWhat }, new { id = "anewest", Class="ajax" })%></li>
<li><%=Html.ActionLink("Popular Now", results.query, "Search", new {  sort = "popular", time = "now", SearchWhat = results.SearchWhat }, new { id = "anow", Class="ajax" })%></li>
<li><%=Html.ActionLink("Popular This Week", results.query, "Search", new {  sort = "popular", time = "1w", SearchWhat = results.SearchWhat }, new { id = "a1w", Class="ajax" })%></li>
<li><%=Html.ActionLink("Popular This Month", results.query, "Search", new {  sort = "popular", time = "1m", SearchWhat = results.SearchWhat }, new { id = "a1m", Class="ajax" })%></li>
<li><%=Html.ActionLink("Popular This Year", results.query, "Search", new {  sort = "popular", time = "1y", SearchWhat = results.SearchWhat }, new { id = "a1y", Class="ajax" })%></li>
<li><%=Html.ActionLink("Popular All Time", results.query, "Search", new {  sort = "popular", time = "all", SearchWhat = results.SearchWhat }, new { id = "aall", Class="ajax" })%></li>
</ul>
</li>

</ul>



<div style="position:absolute; right:10px; margin-top:-23px;">
    <a class="playall" onclick="playall();">
    <img src="../../content/icons/playblack.png"><span>Play all</span>
    </a>
</div>

</div>
