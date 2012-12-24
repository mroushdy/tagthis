<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	People
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
<div style="width:100%; background-color:White; position:absolute;">
<div style=" background: url(../../content/images/HeaderRule.png) no-repeat scroll center 0 transparent; padding-top:5px; position:fixed; top:44px; width:100%;"></div>
<div style="width:560px; margin-left:auto; margin-right:auto; margin-top:50px; margin-bottom:50px;">

<div style="height: 27px; padding-bottom: 25px;">
<h1 style="font-size: 27px; font-weight:300; border-bottom: 1px solid rgb(204, 204, 204); margin: 0px; padding-bottom: 10px; color:#222222;">We Found You Some Cool People.</h1>
</div>

<span style="margin:10px 0 20px 0;">Based on the genres you selected, we've followed you some exciting people with similar taste in music. Simply unfollow anyone who doesn't seem cool enough.</span>

<%List<TagThis.Models.UserListItem> users = (List<TagThis.Models.UserListItem>)ViewData["userlist"]; %>

<div style="width:600px; margin:30px 0;">
<%foreach (var u in users)
  {  %>

  <%ViewData["username"] = u.username; %>
 <div class="peopleitem" style="padding: 10px 15px; overflow:hidden; border-top: 1px solid rgb(243,243,243); border-bottom: 1px solid rgb(223,223,223);">  

                <div style="float:left;">
                <img src="<%=  u.profilepictureurl %>" style="width:32px; height:32px;">
                </div>

                <div style="margin-left:41px;">
                        <%=Html.ActionLink(u.fullname, u.username, "Users", new { }, new { style = "font-weight:bold; color:#333; font-size:16px; position:relative; bottom:-5px;"})%>
                        <span id="subscribe<%= ViewData["username"].ToString() %>" style="float:right; margin-top: 3px;">
                        <%Html.RenderPartial("SubscriptionButton"); %>
                        </span>
                </div>


                </div>
<%} %>
</div>


<%= Html.ActionLink("Take Me Home", "ft" ,"Home",new {},new { Class="bluebutton"})%>
</div>

</div>

    <script type="text/javascript">
        $(function () {
            $("#pageLogo").css("position", "relative").css("left", "-69px").css("margin", "0 50%");
            $("#headNav").hide();
            $("html").css("background-color", "#ffffff")
        });
</script>
</asp:Content>



<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
    .peopleitem .bluebutton
    {
        width:100px;
        background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#e0e0e0), color-stop(50%,#e0e0e0), color-stop(50%,#d3d3d3), color-stop(100%,#d3d3d3));
        background: -moz-linear-gradient(center top, #e0e0e0 0%, #e0e0e0 50%, #d3d3d3 50%, #d3d3d3 100%);
        color: #666666;
        border:1px solid #ccc;
        text-shadow: none;
    }
    
        .peopleitem .subscriptionloading
    {
        left:-38px;
    }
</style>

</asp:Content>