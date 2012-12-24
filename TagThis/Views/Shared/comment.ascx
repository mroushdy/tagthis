<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% TagThis.Models.CommentResult r = (TagThis.Models.CommentResult)ViewData["comment"];
   TagThis.Models.UserRepository ur = new TagThis.Models.UserRepository();
   %>
<% string rating="";%>
<%TagThis.Controllers.utils utils = new TagThis.Controllers.utils(); %> 
<% if(r.comment.date == null) { r.comment.date = DateTime.Now; }
string date = utils.Timedifference((System.DateTime)r.comment.date);%>
<%if(r.rating == null){rating="0";}else{rating = r.rating.ToString();}%>
<%bool commentowner = false;
  if (Request.IsAuthenticated) {if (ViewData["CurrentUserName"].ToString().Trim() == r.comment.Name) { commentowner = true; } }
  %>


<div class="comment clearfix" id="c<%= r.comment.id.ToString() %>">  

                <% if (Request.IsAuthenticated)
                 { %>
<span class="likebox" style="font-size:12px;">
            <%= Ajax.ActionLink("Like", "CommentRate", "Ajax", new { id = r.comment.id, value = "u" }, new AjaxOptions() { UpdateTargetId = "c-r" + r.comment.id.ToString(), OnBegin = "function() {$('#" + r.comment.id + "u" + "').hide();}", OnSuccess = "function() {$('#" + r.comment.id + "u" + "').show();}" }, new { Class = "darkgreybutton", id = r.comment.id.ToString() + "u" })%>
            
            <%if (commentowner) { %>
            <a class="darkgreybutton" href="javascript:void(0);" data-commentid="<%= r.comment.id.ToString() %>" onclick="deletecomment(this);">
            Delete
            </a>
            <%} else {%>
                  <%= Ajax.ActionLink("Hide", "CommentRate", "Ajax", new { id = r.comment.id, value = "d" }, new AjaxOptions() { UpdateTargetId = "c-r" + r.comment.id.ToString(), OnBegin = "function() {$('#" + r.comment.id + "d" + "').hide();}", OnSuccess = "function() {$('#" + r.comment.id + "d" + "').show();}" }, new { Class = "darkgreybutton", id = r.comment.id.ToString() + "d" })%>
            <%} %>
</span>
                <% }%>


                <div class="cimage">
                <img src="<%= r.ProfileImage %>" style="width:32px; height:32px;">
                </div>

                <div class="info">
               <%=Html.ActionLink(ur.GetFullName(r.comment.Name), r.comment.Name, "Users", new { }, new { style = "font-weight:bold; color:#524d4d; font-size:12px;", Class = "ajax" })%>
                <span style="color:#686868; font-size:12px;"><%= r.comment.text %></span>
                <div style="margin:2px 0 0 0;">
                <span  style="font-size:11px;" id="c-r<%=Html.Encode(r.comment.id)%>">
                <%ViewData["commentrating"] = rating; %>
                <% Html.RenderPartial("CommentRating"); %>
                </span> 
                <span style="font-size:11px;">
                 <%=Html.Encode(date)%></span>
                </div>
                </div>


                </div>