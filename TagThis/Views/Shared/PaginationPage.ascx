<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>


<%if (ViewData["settings"] !=null)
  {
      TagThis.Controllers.PaginationSettings ps = (TagThis.Controllers.PaginationSettings)ViewData["settings"];
      TagThis.Controllers.PaginatedList<dynamic> data = new TagThis.Controllers.PaginatedList<dynamic>(ps.Iqueryable, ps.Page ?? 0, ps.PageSize ?? 10);
%>
      <div class="item-container">
      <%foreach (var row in data)
       { 
       ViewData["item"] = row; 
       Html.RenderPartial(ps.ItemPartialViewName);
       } 
     %>
     </div>

<span class="next-button" id="next-button<%= ViewData["random"].ToString() %>-<%=data.PageIndex.ToString()%>">
<%if (data.HasNextPage)
  { %>
     <%= Ajax.ActionLink( ps.GetMoreLinktext, ps.GetMoreAction, "AjaxPaginationHandler", new { page = data.PageIndex + 1, GetMoreAction = ps.GetMoreAction, GetMoreLinktext = ps.GetMoreLinktext, ItemPartialViewName = ps.ItemPartialViewName,  PageSize = ps.PageSize, ParentId = ps.ParentId  }, new AjaxOptions { UpdateTargetId = "data-"+ ViewData["random"].ToString(), InsertionMode = InsertionMode.InsertAfter, OnBegin = "function(){remove('next-button" + ViewData["random"].ToString() + "-" + data.PageIndex.ToString() + "');}" }, new {})%>
<%} %>
</span>

<% }
      %>
