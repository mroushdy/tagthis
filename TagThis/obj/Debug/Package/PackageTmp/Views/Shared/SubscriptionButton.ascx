<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%  if (Request.IsAuthenticated)
    {
        %>

        <img alt="Loading" id="subscribeloading<%= ViewData["username"].ToString() %>" src="../../Content/icons/p-load.gif" class="invisible subscriptionloading" />

        <%
        TagThis.Models.UserRepository ur = new TagThis.Models.UserRepository();
        TagThis.Models.Subscription s = ur.GetSubscription(ViewData["username"].ToString());
        if (s != null)
        {

            using (Ajax.BeginForm("UnSubscribe", "Users/" + ViewData["username"].ToString(), new AjaxOptions() { UpdateTargetId = "subscribe" + ViewData["username"].ToString(), LoadingElementId = "subscribeloading" + ViewData["username"].ToString() }))
            {%>



    <a href="javascript:void(0);" id="u-link<%= ViewData["username"].ToString() %>" class="bluebutton subscriptionbtn"  onclick="$('#UnSubscribe<%= ViewData["username"].ToString() %>').click(); $('#u-link<%= ViewData["username"].ToString() %>').hide();"> Unfollow </a>
     <input id="UnSubscribe<%= ViewData["username"].ToString() %>" class="invisible" type="submit" value="UnSubscribe" />
         
    <% }
        }
        else
        {%>
        
            <%using (Ajax.BeginForm("Subscribe", "Users/" + ViewData["username"].ToString(), new AjaxOptions() { UpdateTargetId = "subscribe" + ViewData["username"].ToString(), LoadingElementId = "subscribeloading" + ViewData["username"].ToString() }))
            {%>
       
       <a href="javascript:void(0);" id="s-link<%= ViewData["username"].ToString() %>" class="bluebutton subscriptionbtn"  onclick="$('#Subscribe<%= ViewData["username"].ToString() %>').click(); $('#s-link<%= ViewData["username"].ToString() %>').hide();"> Follow </a>
     <input id="Subscribe<%= ViewData["username"].ToString() %>" class="invisible" type="submit" value="Subscribe" />
    <%}
        }%>
    <%}%>
   