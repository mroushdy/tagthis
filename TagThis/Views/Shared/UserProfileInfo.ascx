<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%
  string SearchWhat = ViewData["SearchWhat"].ToString();
  %>

       <div class="result" style="width:200px; margin:5px; padding: 15px 15px 0px;" >
       
       
             <div class="clearfix" style="overflow:hidden;">
       <h1 style="font-size:16px; color:#000;"><%=Html.Encode(ViewData["Name"]) %></h1>
       </div>

       <div><h2 style=" font:13px/1.5 Helvetica Neue,Arial,Helvetica,'Liberation Sans',FreeSans,sans-serif; font-weight:bold; font-size:12px; line-height: 22px; color:#444; padding-bottom:10px;">@<%=Html.Encode(ViewData["username"]) %></h2></div>


       <img src="<%= ViewData["ProfileImage"].ToString() %>" style="width:200px;" />

       <%
           if (Request.IsAuthenticated)
           {
               if (Membership.GetUser().UserName != ViewData["username"].ToString())
               { %>
       <div id="subscribe<%= ViewData["username"].ToString() %>" style="margin-top:15px;">
       <%ViewData["username"] = ViewData["username"]; %><%Html.RenderPartial("SubscriptionButton"); %>
       </div>
       <%}
           }%>

       <div class="ProfileLinks" style="padding-bottom:10px; margin-top:15px; border-bottom:1px solid #e0e0e0;">
       <ul style="padding-left:0; ">
       <li class="<%if(SearchWhat.StartsWith("!user;")){%>select<%}%>"><a href='<%= Url.Action(ViewData["username"].ToString(), "Users", new {sort = "newest", time = "all", SearchWhat = "!user;"+ViewData["username"].ToString()}) %>' class="ajax"><span><%= ViewData["Posts"].ToString()%></span> Posts</a></li>
       <li class="<%if(SearchWhat.StartsWith("!userlikes;")){%>select<%}%>"><a href='<%= Url.Action(ViewData["username"].ToString(), "Users", new {sort = "newest", time = "all", SearchWhat = "!userlikes;"+ViewData["username"].ToString()}) %>' class="ajax"><span><%= ViewData["Likes"].ToString()%></span> Likes</a></li>
       <li class="<%if(SearchWhat.StartsWith("!usertags;")){%>select<%}%>"><a href='<%= Url.Action(ViewData["username"].ToString(), "Users", new {sort = "newest", time = "all", SearchWhat = "!usertags;"+ViewData["username"].ToString()}) %>' class="ajax"><span><%= ViewData["Utags"].ToString()%></span> Dropbox</a></li>
       <li class="<%if(SearchWhat.StartsWith("!userfollowers;")){%>select<%}%>"><a href='<%= Url.Action(ViewData["username"].ToString(), "Users", new {sort = "newest", time = "all", SearchWhat = "!userfollowers;"+ViewData["username"].ToString()}) %>' class="ajax"><span><%= ViewData["Subscribers"].ToString()%></span> Followers</a></li>
       <li class="<%if(SearchWhat.StartsWith("!userfollowing;")){%>select<%}%>"><a href='<%= Url.Action(ViewData["username"].ToString(), "Users", new {sort = "newest", time = "all", SearchWhat = "!userfollowing;"+ViewData["username"].ToString()}) %>' class="ajax"><span><%= ViewData["Subscribed"].ToString()%></span> Following</a></li>
       </ul>
       </div>

       <div class="song-tags" style="margin:15px 0 0 0; padding:0 0 10px 0; color:#777;">
       <div style="font-weight:bold; color:#000; padding: 0 0 10px;">Interests</div>

       <% var tags = ViewData["Tags"].ToString().Split(',');
          foreach (var t in tags)
          {
              if (!string.IsNullOrEmpty(t.Trim()))
              { %>
         <span class="tag"><%= Html.ActionLink(t.Trim(), t.Trim(), "Search", new { }, new { Class = "ajax" })%></span>
       <%}
          } %>
       </div>

       </div>