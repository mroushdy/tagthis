<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%TagThis.Controllers.OmniResults results = (TagThis.Controllers.OmniResults)ViewData["results"]; %>
<%TagThis.Controllers.Spider s = new TagThis.Controllers.Spider(); %>
<div id="lbfavdiv" class="lbfavediv" style="height:330px; width:100%; overflow:auto;">
    <% foreach (TagThis.Models.Result r in results.Results){ %>
    
    
<%string url = "http://www.google.com/s2/favicons?domain="+s.cleanURL(r.page.url);%>
<%TagThis.Controllers.utils utils = new TagThis.Controllers.utils(); %> 
<%string date = utils.Timedifference((System.DateTime)r.page.date);%>
<div style="border-bottom:1px solid #999999; padding:5px 0px 5px;">
<div>
<img alt="" style="width:16; height:16;" src="<%=url%>"/> <a href='<%= Html.Encode(r.page.url)%>' target="owo"  class="link"><%=r.page.name%></a>
</div>
</div>

    <%} %>
</div>