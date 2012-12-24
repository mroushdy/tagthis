<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%TagThis.Controllers.Spider s = new TagThis.Controllers.Spider(); %>
<%TagThis.Models.Result r = (TagThis.Models.Result)ViewData["ResultSingle"]; %>
<%string url = "http://www.google.com/s2/favicons?domain="+s.cleanURL(r.page.url);%>
<%TagThis.Controllers.utils utils = new TagThis.Controllers.utils(); %> 
<%string date = utils.Timedifference((System.DateTime)r.page.date);%>
<%ViewData["pageid"] = r.page.page_id; %>
<% TagThis.Models.TagRepository tr = new TagThis.Models.TagRepository(); %>
<% TagThis.Models.UserRepository ur = new TagThis.Models.UserRepository(); %>
<% tr.AddPageView(r.page.page_id);%>
<% ViewData["rating"] = r.rating;
   ViewData["postid"] = r.pageOwner.PostId;
   bool postowner = false;
   if (Request.IsAuthenticated) { ViewData["CurrentUserName"] = ViewData["CurrentUserName"]; if (ViewData["CurrentUserName"].ToString().Trim() == r.pageOwner.OwnerId.Trim()) { postowner = true; } }
    ViewData["postowner"] = postowner;
    %>

<%if (r.Userupr != null) { ViewData["oRate"] = r.Userupr.Rate.ToString(); } else { ViewData["oRate"] = "0"; } %>

<div class="result" id="r<%= r.pageOwner.PostId %>" style="padding-top:2px; padding-bottom:2px;">


<%if (Request.IsAuthenticated){%>
<div class="delete" style="display:none;" onclick="$('#r<%=r.pageOwner.PostId %>').fadeOut('slow');"><%= Ajax.ActionLink("[replaceme]", "LHF", "Ajax", new { pid = ViewData["pageid"].ToString(), value = "h" }, new AjaxOptions() { UpdateTargetId = "r" + r.pageOwner.PostId.ToString() }, new { Style = "text-decoration:none;", alt = "remove", id = "h" + ViewData["pageid"].ToString() }).ToHtmlString().Replace("[replaceme]", "&nbsp;")%></div>
<%} %>



    <div class="image-buttons-container" id="html<%= Html.Encode(r.pageOwner.PostId)%>" class="clearfix">
    <%if(!string.IsNullOrEmpty(r.page.thumburl)){ %>

    <div class="playquecontainer play" onclick="playvideo('<%= Html.Encode(r.page.url)%>','<%= Html.Encode(r.page.page_id)%>','<%= Html.Encode(r.pageOwner.PostId)%>');">
    <img src="../../content/icons/playwhite.png"><span>Play</span>
    </div>

    <div class="playquecontainer que" onclick="Que('<%= Html.Encode(r.page.url)%>','<%= Html.Encode(r.page.page_id)%>','<%= Html.Encode(r.pageOwner.PostId)%>');">
    <img src="../../content/icons/quewhite.png"><span>Cue</span>
    </div>


    <div class="imagecontainer">
    <img src="<%= Url.Action("index", "getthumb", new { u = r.page.thumburl , width = 240}, Request.Url.Scheme) %>" onerror="this.style.display='none'"  />
    </div>

 <%} %>   

</div>

 <span class="linkscontainer">

            <%if (Request.IsAuthenticated)
              {%>
            <a class="Button CommentButton" href="javascript:void(0);" onclick="toggleLayer('ctb<%= Html.Encode(r.pageOwner.PostId)%>'); doinline();  DelayedMasonry();">
            Comment
            </a>

            <a class="Button LikeButton" href="javascript:void(0);" style="text-align:center;" data-pid="<%= r.page.page_id %>" data-postid="<%= r.pageOwner.PostId %>" onclick="Like(this);">
            <%if (ViewData["oRate"].ToString() != "1")
              { %> Like<%}
              else
              {%>Unlike<%} %>
            </a>

            <%if (postowner)
              { %>
            <a class="Button RepostButton" href="javascript:void(0);" data-url="<%= r.page.url %>" data-pid="<%= r.page.page_id %>" data-postid="<%= r.pageOwner.PostId %>" onclick="deletepost(this);">
            Delete
            </a>
            <%}
              else
              {%>
            <a class="Button RepostButton" href="javascript:void(0);" data-url="<%= r.page.url %>" data-pid="<%= r.page.page_id %>" data-postid="<%= r.pageOwner.PostId %>" onclick="repost(this);">
            Repost
            </a>
            <% }%>

            <a class="Button ShareButton" href="javascript:void(0);" url="<%= r.page.url %>" data-url="<%= r.page.url %>" data-pid="<%= r.page.page_id %>" data-postid="<%= r.pageOwner.PostId %>" onclick="callpublishtofeed(this);">
            Share
            </a>
            <%} %>
            </span>

<div class="title" id="title<%= Html.Encode(r.pageOwner.PostId)%>">
   <%=Html.ActionLink(r.page.name,r.pageOwner.PostId.ToString(), "Post", new { }, new { Class = "ajax" })%>
   </div>


   <div class="ownername">  

                <div class="cimage">
                <img src="<%= r.pageOwner.OwnerPictureUrl%>">
                </div>

                <div class="orighttop">
               <%=Html.ActionLink(ur.GetFullName(r.pageOwner.OwnerId), r.pageOwner.OwnerId, "Users", new { }, new { Class = "ajax"})%> 
                <% if(!string.IsNullOrEmpty(r.pageOwner.RepostedFromUserId)){ %>
                via <%=Html.ActionLink(ur.GetFullName(r.pageOwner.RepostedFromUserId), r.pageOwner.RepostedFromUserId, "Users", new { }, new { Class = "ajax" })%> 
               <%} %>
                <span class="ownerpost"><%= r.pageOwner.OwnerPost %></span>
                
                <div class="orightdown">
                <span data="where date used to be"></span>
                </div>
                </div>


</div>



   <div class="r-activity-info">
   <span id="likes<%= Html.Encode(r.pageOwner.PostId)%>"><%= r.pageOwner.rating.ToString() %></span> Likes <span id="reposts<%= Html.Encode(r.pageOwner.PostId)%>" style="margin:4px;"><%= r.pageOwner.reposts.ToString() %></span> Reposts <span style="float:right;"> <%=Html.Encode(utils.Timedifference(r.pageOwner.Date))%></span>
   </div>

            
<!-- start comments --->
<div id="c-box<%= Html.Encode(r.pageOwner.PostId)%>">
<%List<TagThis.Models.CommentResult> cr = (List<TagThis.Models.CommentResult>)ur.GetComments(r.pageOwner.PostId).OrderByDescending(a => a.comment.date).Take(3).ToList(); ViewData["comments"] = cr; if (r.comments > cr.Count()) { ViewData["cshowmore"] = "true"; } else { ViewData["cshowmore"] = "false"; } ViewData["pageno"] = 0; %>

<% Html.RenderPartial("Comments"); %>
</div>
<!--- end comments--->


</div>

