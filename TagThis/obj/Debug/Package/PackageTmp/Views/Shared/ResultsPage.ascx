<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%TagThis.Controllers.OmniResults results = (TagThis.Controllers.OmniResults)ViewData["results"]; %>
 

 <% foreach (TagThis.Models.Result r in results.Results){ %>
    <% ViewData["ResultSingle"] = r;  %>
    <% Html.RenderPartial("ResultSingle"); %>
    <%} %>


           <%TagThis.Controllers.PaginatedList<TagThis.Models.Result> R = results.Results;%>
       <% if (R.HasNextPage) { %>
    
        <div id="p-load<%= R.PageIndex %>" class="invisible" style="background: url('../../content/icons/moreload.gif') no-repeat center center; height:36px; width:100%; margin-top:40px;">
        &nbsp;
        </div> 


    <div id="next-button<%= R.PageIndex %>" class="showmorebutton" style="margin-top:20px; width:100%; text-align:center;">
    <%= Ajax.ActionLink("Show More", "MoreResults", "Ajax", new { query = results.query, page = (R.PageIndex + 1), sort = results.sort, time = results.time, SearchWhat = results.SearchWhat }, new AjaxOptions { UpdateTargetId = "resultcontent", LoadingElementId = "p-load" + results.Results.PageIndex.ToString(), InsertionMode = InsertionMode.InsertAfter, OnBegin = "function(){remove('next-button" + R.PageIndex.ToString() + "');}", OnSuccess = "function(){remove('p-load" + R.PageIndex.ToString() + "'); masonry();}" }, new { Class = "greybutton", style = "width:150px; text-align:center;" })%>
    </div> 
     <% } %>