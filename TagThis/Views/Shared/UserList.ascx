<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%List<TagThis.Models.UserListItem> users = (List<TagThis.Models.UserListItem>)ViewData["userlist"]; %>

<div style="width:600px; padding-top:5px;">
<%foreach (var u in users)
  {  %>

  <%ViewData["username"] = u.username; %>
 <div style="padding: 10px 15px; overflow:hidden; background-color:#272727;  border-top: 1px solid #323232; border-bottom: 1px solid #1b1b1b;">  

                <div style="float:left;">
                <img src="<%=  u.profilepictureurl %>" style="width:32px; height:32px;">
                </div>

                <div style="margin-left:41px;">
                        <%=Html.ActionLink(u.fullname, u.username, "Users", new { }, new { style = "font-weight:bold; color:#CCCCCC; font-size:16px; position:relative; bottom:-5px;",Class = "ajax"})%>
                        <span id="subscribe<%= ViewData["username"].ToString() %>" style="float:right; margin-top: 3px;">
                        <%Html.RenderPartial("SubscriptionButton"); %>
                        </span>
                </div>


                </div>
<%} %>
</div>