<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%TagThis.Controllers.Spider s = new TagThis.Controllers.Spider(); %>
<%TagThis.Models.Result r = (TagThis.Models.Result)ViewData["ResultSingle"]; %>
<%TagThis.Controllers.utils utils = new TagThis.Controllers.utils(); %> 
<%string date = utils.Timedifference((System.DateTime)r.page.date);%>
<%ViewData["pageid"] = r.page.page_id; %>
<% TagThis.Models.TagRepository tr = new TagThis.Models.TagRepository(); %>
<% TagThis.Models.UserRepository ur = new TagThis.Models.UserRepository(); %>
<% tr.AddPageView(r.page.page_id);%>
<% ViewData["rating"] = r.rating;
   ViewData["postid"] = r.pageOwner.PostId;
   %>



<%if (!Request.IsAuthenticated)
             {%>

<script type="text/javascript">
$(document).ready(function () {

FB.init({
    appId  : <%= Facebook.FacebookApplication.Current.AppId%>, // the app ID you get when you register your app in http://www.facebook.com/developers/
    status : true, // check login status
    cookie : true, // enable cookies to allow the server to access the session
    xfbml  : true  // parse XFBML
 });

var perms           = ['publish_actions'],
    permsString     = perms.join(','),
    permissionsUrl  = 'https://www.facebook.com/dialog/oauth';
    permissionsUrl  += '?client_id=' + <%= Facebook.FacebookApplication.Current.AppId%>;
    permissionsUrl  += '&redirect_uri=' + encodeURI(window.location.href);
    permissionsUrl  += '&scope=' + permsString;

    FB.getLoginStatus(function (response) {

        if (response.status === 'connected') { 

			var poststring = '/me/sixtysongs:listen_to?song=http://www.sixtysongs.com/Post/<%= r.pageOwner.PostId %>&access_token=' + response.authResponse.accessToken.toString();
			FB.api(poststring,'post',function (response) { if (!response || response.error) { } else { } });
			
        } else { 
			window.location.href = permissionsUrl;
		}

    }, true);


});

function gotopermissions()
{
	
}
</script>


<%}%>



<%if (r.Userupr != null) { ViewData["oRate"] = r.Userupr.Rate.ToString(); } else { ViewData["oRate"] = "0"; } %>


<div class="post-container" style="width:680px; position:relative; margin:25px 0 25px 50%; left:-340px;">

<div style="width:230px; float:left;">



  <%if (Request.IsAuthenticated)
              {%>
<div class="result facetiles " style="margin:0 0 10px 0; text-align:center; padding:0; width:220px">
            <span class="linkscontainer result-large-buttons">
            <a class="Button CommentButton" href="javascript:void(0);" onclick="toggleLayer('ctb<%= Html.Encode(r.pageOwner.PostId)%>');  DelayedMasonry();">
            Comment
            </a>

            <a class="Button LikeButton" href="javascript:void(0);" style="text-align:center;" data-pid="<%= r.page.page_id %>" data-postid="<%= r.pageOwner.PostId %>" onclick="Like(this);">
            <%if (ViewData["oRate"].ToString() != "1")
              { %> Like<%}
              else
              {%>Unlike<%} %>
            </a>

            <a class="Button RepostButton" href="javascript:void(0);" data-url="<%= r.page.url %>" data-pid="<%= r.page.page_id %>" data-postid="<%= r.pageOwner.PostId %>" onclick="repost(this);">
            Repost
            </a>

            <a class="Button ShareButton" rel="shareit" href="javascript:void(0);" url="<%= r.page.url %>">
            Share
            </a>
             </span>
</div>
            <%} %>



<div class="result facetiles" style="margin:0px; padding:12px 10px 2px 10px;">

  <div class="ownername" style="padding-top:5px;">  

                <div class="cimage">
                <img src="<%= r.pageOwner.OwnerPictureUrl%>">
                </div>

                <div class="orighttop">
               <%=Html.ActionLink(ur.GetFullName(r.pageOwner.OwnerId), r.pageOwner.OwnerId, "Users", new { }, new { Class = "ajax"})%> 
                <% if(!string.IsNullOrEmpty(r.pageOwner.RepostedFromUserId)){ %>
                via <%=Html.ActionLink(ur.GetFullName(r.pageOwner.RepostedFromUserId), r.pageOwner.RepostedFromUserId, "Users", new { }, new { Class = "ajax" })%> 
               <%} %>

                
                <div class="orightdown">
                <span data="where date used to be"></span>
                </div>
                </div>


</div>

<div style="padding:0 0 5px;"><span class="ownerpost"><%= r.pageOwner.OwnerPost %></span></div>

   <div class="r-activity-info" style="width:186px; margin-left:-8px">
   <span id="likes<%= Html.Encode(r.page.page_id)%>"><%= r.pageOwner.rating.ToString() %></span> Likes <span id="reposts<%= Html.Encode(r.page.page_id)%>" style="margin:4px;"><%= r.pageOwner.reposts.ToString() %></span> Reposts <span style="float:right;"> <%=Html.Encode(utils.Timedifference(r.pageOwner.Date))%></span>
   </div>

</div>



<% TagThis.Controllers.PaginationSettings ps = new TagThis.Controllers.PaginationSettings();
   ps.GetMoreAction = "GetNextLikes";
   ps.ItemPartialViewName = "UserFaceTile";
   ps.PageSize = 11;
   ps.ParentId = r.pageOwner.PostId.ToString();
   ps.Iqueryable = ur.GetPostLikes(r.pageOwner.PostId).Cast<dynamic>();
   int count = ps.Iqueryable.Count();
   ps.GetMoreLinktext = (count - ps.PageSize).ToString() + " More";
   ViewData["settings"] = ps;

   if (count > 0)
   {
    %>
<div class="result facetiles" style="margin:10px 0 0; padding:12px 10px 10px 10px;">
<div style="color:#000; font-size:12px; font-weight:bold; margin:0 0 7px 0;">Liked by</div>

    <% Html.RenderPartial("Pagination"); %>
</div>
<% }%>




<% TagThis.Controllers.PaginationSettings ps2 = new TagThis.Controllers.PaginationSettings();
   ps2.GetMoreAction = "GetNextLikes";
   ps2.ItemPartialViewName = "UserFaceTile";
   ps2.PageSize = 11;
   ps2.ParentId = r.pageOwner.PostId.ToString();
   ps2.Iqueryable = ur.GetPostReposts(r.pageOwner.PostId).Cast<dynamic>();
   int count2 = ps2.Iqueryable.Count();
   ps.GetMoreLinktext = (count2 - ps2.PageSize).ToString() + " More";
   ViewData["settings"] = ps2;

   if (count2 > 0)
   {
    %>
<div class="result facetiles" style="margin:10px 0 0; padding:12px 10px 10px 10px;">
<div style="color:#000; font-size:12px; font-weight:bold; margin:0 0 7px 0;">Reposted by</div>

    <% Html.RenderPartial("Pagination"); %>
</div>
<% }%>


</div>


<div class="result result-large" style="margin:0 0 0 240px;" id="r<%= r.pageOwner.PostId %>">


<div class="delete" style="display:none;" onclick="$('#r<%=r.page.page_id %>').fadeOut('slow');"><%= Ajax.ActionLink("[replaceme]", "LHF", "Ajax", new { pid = ViewData["pageid"].ToString(), value = "h" }, new AjaxOptions() { UpdateTargetId = "r" + r.page.page_id.ToString() }, new {Style="text-decoration:none;",alt="remove", id = "h" + ViewData["pageid"].ToString() }).ToHtmlString().Replace("[replaceme]", "&nbsp;")%></div>


   <div class="title" id="title<%= Html.Encode(r.pageOwner.PostId)%>">
   <%=Html.ActionLink(r.page.name,r.pageOwner.PostId.ToString(), "Post", new { }, new { Class = "ajax" })%>
   </div>

    
    <div class="image-buttons-container" id="html<%= Html.Encode(r.page.page_id)%>" class="clearfix">
    <%if(!string.IsNullOrEmpty(r.page.thumburl)){ %>
    

    <div class="imagecontainer">
    <img style="position:absolute;" src="<%= Url.Action("index", "getthumb", new { u = r.page.thumburl , width = 480}, Request.Url.Scheme) %>" onerror="this.style.display='none'"  />
    <div class="playlarge" onclick="playvideo('<%= Html.Encode(r.page.url)%>','<%= Html.Encode(r.page.page_id)%>','<%= Html.Encode(r.pageOwner.PostId)%>');"></div>
    </div>

    <div class="playquecontainer que" onclick="Que('<%= Html.Encode(r.page.url)%>','<%= Html.Encode(r.page.page_id)%>','<%= Html.Encode(r.pageOwner.PostId)%>');">
    <img src="../../content/icons/quewhite.png"><span>Cue</span>
    </div>

 <%} %>   

</div>


   <div class="song-tags">
        <% foreach (var t in r.toptags)
         {  %>
         <span class="tag"><%= Html.ActionLink(t.tagbody.name.Trim(), t.tagbody.name.Trim(), "Search", new { }, new { Class = "ajax" })%></span>
       <%} %>
      <a class="addtag" href="javascript:void(0);" onclick="var m = $('#tagbox<%= Html.Encode(r.page.page_id)%>'); m.slideDown(); setTimeout(function(){m.find('li:last input').focus();},1000); ">Add a Tag</a>
      <div class="invisible tag-box" id='tagbox<%= Html.Encode(r.page.page_id)%>'>
      <% Html.RenderPartial("TagSubmitForm"); %>
      </div>
   </div>



            
<!-- start comments --->
<div id="c-box<%= Html.Encode(r.pageOwner.PostId)%>" style="padding-bottom:2px;">
<%List<TagThis.Models.CommentResult> cr = (List<TagThis.Models.CommentResult>)ur.GetComments(r.pageOwner.PostId).OrderByDescending(a => a.comment.date).Take(10).ToList(); ViewData["comments"] = cr; if (r.comments > cr.Count()) { ViewData["cshowmore"] = "true"; } else { ViewData["cshowmore"] = "false"; } ViewData["pageno"] = 0; %>

<% Html.RenderPartial("Comments"); %>
</div>
<!--- end comments--->


</div>


</div>


