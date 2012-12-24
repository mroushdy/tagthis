<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>


<%
    if (Request.IsAuthenticated) {
        TagThis.Models.UserRepository upr = new TagThis.Models.UserRepository();
        string username = Page.User.Identity.Name;
        string fullname = upr.GetFullName(username);
        Guid uid = (Guid)Membership.GetUser(username).ProviderUserKey;
        string profileimage = upr.GetProfileImage(uid, "mini");

%>



    <ul class="PostHeaderContainer headlinks" style="float:left;">

     <li><a href="javascript:void(0)" onclick = "callAppReq();">Invite Friends</a></li>

     <li><a href="javascript:void(0)" onclick="ModalShow('AddModal');">Add a Song</a></li>
     <li style="display:none;"><%= Html.ActionLink("Invite", "index", "Invitations") %></li>


    <li class="submenu">
    <a class="nav ajax" href="<%= Url.Action(Page.User.Identity.Name, "Users", new {}) %>">
    <img alt="" src="<%= profileimage %>" style="width:23px; height:23px; border:1px solid #cccccc; margin: -5px 5px 3px 3px; float:left;"/>
    <%= Html.Encode(fullname)%>
    <span></span>
    </a>

    <ul>
    <li><%= Html.ActionLink("Profile", Page.User.Identity.Name, "Users", new { }, new { Class = "ajax"})%></li>
    <li><a href="javascript:void(0)" onclick="ModalShow('SettingsModal');">Settings</a></li>
    <li><%= Html.ActionLink("Log Out", "LogOff", "Account", new { }, new { onclick = "FBlogout();" })%></li>
    </ul>
    </li>


     
     
     </ul>

      <%
            if (Request.IsAuthenticated)
            {
%>
<span id="notificationscontainer" class="notifications" style="float:right;">
<% Html.RenderPartial("notifications"); %>
</span>

       <%} %>

<%
    }
    else {%> 
    <div style="position:relative; top:-3px;">
    <div class="fb-like" data-href="https://www.facebook.com/pages/Sixtysongs/195738287209026" data-send="false" data-layout="button_count" data-width="90" data-show-faces="false"></div>
    </div>
    <%}
%>

