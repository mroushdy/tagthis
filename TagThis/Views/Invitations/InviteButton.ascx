<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% TagThis.Models.UserRepository upr = new TagThis.Models.UserRepository();
   string email = ViewData["email"].ToString();
   if (upr.isRegistered(email))
   {%> 
<span style="color:Green; padding-right:5px;">already on TagThis | <%=Html.ActionLink("View profile",System.Web.Security.Membership.GetUserNameByEmail(email),"users") %></span>
 <%}else if(upr.isInvited(email)){ %>
<span style="color:Green; padding-right:5px;">invited</span>
 <%}else{ %>
 <img alt="Loading" id="inviteloading" src="../../Content/icons/loading19.gif" class="Ratingloading"/>
            <%using (Ajax.BeginForm("InviteSingleEmail", "Invitations",new{email = HttpUtility.UrlEncode(ViewData["email"].ToString())} ,new AjaxOptions() { UpdateTargetId = ViewData["DivID"].ToString(), LoadingElementId = "inviteloading" })){%>
     <input type="submit" value="Invite" style="margin-top:-5px;"/>
    <%}
        }%>