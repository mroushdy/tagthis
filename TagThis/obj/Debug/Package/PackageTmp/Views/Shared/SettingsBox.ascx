<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<%
    TagThis.Models.UserRepository ur = new TagThis.Models.UserRepository();
    TagThis.Models.UserData ud = ur.GetUserData();
    
    string email = Membership.GetUser().Email;

   
    //this posts the results to the partial view to update it. From there the Jquery gets the error message if existing.
    using (Ajax.BeginForm("ChangeSettings", "Account", new { }, new AjaxOptions() { UpdateTargetId = "SettingsDiv", InsertionMode = InsertionMode.Replace, OnSuccess = "finishsettings" }, new { }))
    {%>


<div>
Your Email
</div>
<div id="Div1" style="width:400px; margin-top:10px;">
<div class="Wrap">
<div class="outerWrap">
<div class="innerWrap">
<input type="text" value="<%= email %>" id="Email" name="Email" class="textBox" />
</div>
</div>
</div>
</div>

<div class="error" id="seterror" style="width:400px;">
<%if (ViewData["error"] != null)
  {%><%= Html.Encode(ViewData["error"])%><%} %>
</div>

<div style="margin-top:10px;">
Select which emails you like to receive from SixtySongs
</div>

<div style="margin-top:10px;">
<div><input type="checkbox" <% if(ud.EmailFollows){ %>checked="yes"<% }%> value="true" name="EmailFollows" id="EmailFollows"/> <label for="EmailFollows">Someone follows you</label></div>
<div style="margin-top:2px;"><input type="checkbox" <% if(ud.EmailReposts){ %>checked="yes"<% }%> value="true" name="EmailReposts" id="EmailReposts"/> <label for="EmailReposts">Someone reposts a song you shared</label></div>
<div style="margin-top:2px;"><input type="checkbox" <% if(ud.EmailLikes){ %>checked="yes"<% } %> value="true" name="EmailLikes" id="EmailLikes"/> <label for="EmailLikes">Someone likes a song you shared</label></div>
<div style="margin-top:2px;"><input type="checkbox" <% if(ud.EmailUserPostTag){ %>checked="yes"<% } %> value="true" name="EmailUserPostTag" id="EmailUserPostTag"/> <label for="EmailUserPostTag">Someone drops a song on your dropbox</label></div>
<div style="margin-top:2px;"><input type="checkbox" <% if(ud.EmailComments){ %>checked="yes"<% }  %> value="true" name="EmailComments" id="EmailComments"/> <label for="EmailComments">Someone commented on a song you shared</label></div>
<div style="margin-top:2px;"><input type="checkbox" <% if(ud.EmailCommentsLike){ %>checked="yes"<% } %> value="true" name="EmailCommentsLike" id="EmailCommentsLike"/> <label for="EmailCommentsLike">Someone liked your comment</label></div>
<div style="margin-top:2px;"><input type="checkbox" <% if(ud.EmailCommentsReply){ %>checked="yes"<% } %> value="true" name="EmailCommentsReply" id="EmailCommentsReply"/> <label for="EmailCommentsReply">Someone commented on a song you also commented on</label></div>
</div>


<div style="margin-top:10px;">
Share your SixtySongs activity on Facebook
</div>

<div style="margin-top:10px;">
<input type="checkbox" <% if(ud.FBautopost){ %>checked="yes"<%} %> value="true" name="FBautopost" id="FBautopost"/> <label for="FBautopost">Add SixtySongs to your Facebook Timeline</label>
</div>

<div style="margin-top:10px;">
<a href="javascript:void(0);" onclick="ModalHide('SettingsModal');ModalShow('DeleteAccountModal');">Need to cancel your account?</a>
</div>

<div id="ShareModalDown" style="margin-top:15px;">
<div style="margin-left:291px;" class="clearfix">
<a href="javascript:void(0);" id="set-link" style="width:67px; text-align:center;" class="bluebutton"  onclick="$('#AddSettings').click(); $('#set-link').hide(); $('#set-onesec').show();"> Save </a>
<img id="set-onesec" src="../../content/icons/s-load.gif" style="position:relative; top:5px; left:33px;" class="invisible"/>
</div>
</div>


 <input type="submit" id="AddSettings" value="Save" class="invisible"/>      
             <%} %>