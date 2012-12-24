<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% TagThis.Models.CommentResult r = (TagThis.Models.CommentResult)ViewData["comment"];%>
<% string rating="";%>
<%TagThis.Controllers.utils utils = new TagThis.Controllers.utils(); %> 
<%string date = utils.Timedifference((System.DateTime)r.comment.date);%>
<%if(r.rating == null){rating="0";}else{rating = r.rating.ToString();}%>
<div id="comment">
<table id="commentsplitter">
<tr>
<td valign="middle" width="34">
<% if (Request.IsAuthenticated)
   { %>
<table id="Table_01" style="height:30px; width:34px;" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td width="30" align="center"><%= Ajax.ActionLink("[replacethis]", "Rate", "Comments", new { id = r.comment.id.ToString(), value = "u" }, new AjaxOptions() { UpdateTargetId = "div" + r.comment.id.ToString() }).Replace("[replacethis]", "<img style='cursor:pointer;' src='../../Content/images/comments/up.gif' width='20' height='11' alt='Good Comment'>")%></td>
      </tr>
      <tr>
        <td align="center" style="height:10px;"><strong><div id="div<%=Html.Encode(r.comment.id)%>"><%= Html.Encode(rating)%></div></strong> </td>
      </tr>
      <tr>
        <td align="center"><%= Ajax.ActionLink("[replacethis]", "Rate", "Comments", new { id = r.comment.id.ToString(), value = "d" }, new AjaxOptions() { UpdateTargetId = "div" + r.comment.id.ToString() }).Replace("[replacethis]", "<img style='cursor:pointer;' src='../../Content/images/comments/down.gif' width='20' height='11' alt='Bad Comment'>")%></td>
      </tr>
    </table>
    <% }%>
    </td>
    <td valign="middle" >
    <div id="commentRight">
    <%= Html.Encode(r.comment.text)%> <span style="color:Black;">by <%=Html.Encode(r.comment.Name)%></span> <%=Html.Encode(date)%>
</div>
</td>
</tr>
</table>
</div>
<br />