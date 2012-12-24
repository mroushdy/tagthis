<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<% List<TagThis.Models.NotificationResult> notifications = (List<TagThis.Models.NotificationResult>)ViewData["Notifications"]; %>
<% int unreadcount =  0;
   int totalcount = 0;
    if (notifications != null)
    { unreadcount = notifications.Where(n => n.Read == false).Count(); totalcount = notifications.Count(); }
    %>   
    

    <span class="linkcontainer<%if(unreadcount == 0) {%> nonotifications<%}%>">
    <a class="nav" href="javascript:void(0)">
    <%= unreadcount.ToString() %>
    </a>
    
    <div class="container">
    <div class="mousescrollblack" style="max-height:300px;">
    <ul>
    <%if (totalcount != 0)
      {
          foreach (var n in notifications)
          { %>
          <% string url = ""; if (string.IsNullOrEmpty(n.LinkAction)) { url = Url.Action(n.LinkId.Trim(), n.LinkController); } else { Url.Action(n.LinkAction, n.LinkController, new { id = n.LinkId.Trim() });  }%>

    <li <%if(n.Read == false){%>class="unread"<%} %> ><a href='<%= url.Trim() %>' class="ajax nLink">
    <img src="<%= n.OwnerImage %>" style="float:left; width:30px; height:30px;"/>
    <div style="margin-left:35px;">
    <div><%= n.NotificationHTML%></div>
    <div class="nTime"><%= n.Time %></div>
    </div>
    </a></li>
        <%}
      }
      else
      {%>
      <li><a href="#">You have no new notifications.</a></li>
      <%} %>
    </ul>
    </div>
    </div>
    </span>