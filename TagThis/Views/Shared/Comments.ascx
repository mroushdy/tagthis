<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%int pageid = (int)ViewData["pageid"];%>
<%int postid = (int)ViewData["postid"];%>

<div class="commentwrap">

<div class="comments">
<div class="invisible" id="ctb<%= Html.Encode(postid)%>">
    <% if (Request.IsAuthenticated)
       { %>
    <% using (Ajax.BeginForm("AddComment", "Ajax", new { pid = pageid, postid = postid }, new AjaxOptions() { InsertionMode = InsertionMode.InsertBefore, UpdateTargetId = "c-form" + postid.ToString(), LoadingElementId = "c-l" + postid.ToString(), OnBegin = "function() {$('#cbox" + postid.ToString() + "').hide();}", OnSuccess = "function() {$('#cbox" + postid.ToString() + "').show(); $('#cbox" + postid.ToString() + "').find('input').val('').blur(); DelayedMasonry();}" }))
       {%>
    <input type="hidden" class="c-text<%= postid %>" id="Text" name="Text" value=""/>
    
            
     <input type="submit" id="c-sub<%= postid %>" value="Save" class="invisible" />        
     
    <% }%>

<div class="clearfix comment" >

<img class="invisible" style="position:relative; left:69px;" id="c-l<%= postid %>" src="../../content/icons/c-load2.gif"/>

<div id="cbox<%= postid %>" class="Wrap c-box">
<div class="outerWrap">
<div class="innerWrap">
<input type="text" value="" title="What do you think?" class="textBox dummy-text<%= postid %>" onkeydown="if (event.keyCode == 13) { var text<%= postid %> = $('.dummy-text<%= postid %>').val(); $('.c-text<%= postid %>').val(text<%= postid %>); $('#c-sub<%= postid %>').click();}"/>
</div>
</div>
</div>


</div>

      <% } %>
</div>

<div class="holder" id="c-form<%= postid %>">

<%if(ViewData["comments"] != null) {
      
      foreach (TagThis.Models.CommentResult r in (ViewData["comments"] as List<TagThis.Models.CommentResult>))
       { 
       
       ViewData["comment"] = r; 
       Html.RenderPartial("comment");
        
       } 
      }
      %>


</div>

<%if (ViewData["cshowmore"].ToString() == "true")
  { %>
<div class="older-posts">

<img class="invisible" style="position:relative;" id="cmts-l<%= postid %>" src="../../content/icons/c-load2.gif"/>


<%= Ajax.ActionLink("See more comments", "GetComments", "Ajax", new { pageid = pageid, postid = postid, pageno = (int)ViewData["pageno"] }, new AjaxOptions() { UpdateTargetId = "c-box" + postid.ToString(), LoadingElementId = "cmts-l" + postid, OnBegin = "function() {$('#c-more-link" + postid.ToString() + "').hide();}", OnSuccess = "DelayedMasonry" }, new { id = "c-more-link"+postid.ToString() })%>

</div>
<%} %>


</div>

</div>


