 <%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
 <%  TagThis.Models.TagRepository tr = new TagThis.Models.TagRepository();
    List<string> g = tr.GetXmlMainGenres().OrderBy(o => o).ToList<string>(); %>

<div id="preurlcontainer">

<div id="Div1" style="width:400px;">
<div class="Wrap">
<div class="outerWrap">
<div class="innerWrap">
<input type="text" value="" title="http:// youtube url" id="preurl" name="preurl" class="textBox" />
</div>
</div>
</div>
</div>

<div id="urldown" style="margin-top:15px;">
<div class="error" id="preurlerror" style="width:208px;  float:left;">
</div>

<div style="margin-left:266px;" class="clearfix">
<a href="javascript:void(0);" id="preurlattach" class="bluebutton"  onclick="attachurl();"> Attach song </a>
<img id="preurlloader" src="../../content/icons/p-load.gif" style="position:relative; top:5px; left:40px;" class="invisible"/>
</div>
</div>

</div>


<div id="addform" class="invisible">

<div style="width:400px;" class="clearfix">
<span style="float: left; margin: 0px 8px 0px 0px;"><img alt="" src="" id="moviethumb" /></span>
<span style="color: rgb(51, 51, 51); height:18px; display:block; padding-top:4px; overflow:hidden; font-size: 13px; font-weight: bold;" id="movietitle"></span>
<span style="color: rgb(102, 102, 102); height:67px; padding-top:5px; overflow:hidden; display: block; font-size: 0.9166em" id="moviedescription"></span>
</div>


<% 

    //this posts the results to the partial view to update it. From there the Jquery gets the error message if existing.
    using (Ajax.BeginForm("Add", "Add", new { }, new AjaxOptions() { UpdateTargetId = "NewPost", InsertionMode = InsertionMode.Replace, OnSuccess = "finishshare" }, new { }))
   {%>


<div>



<input type="hidden" value="" id="url" name="url" />
<input type="hidden" value="" id="RepostedFrom" name="RepostedFrom" />


<div id="desc-box" style="width:400px; margin-top:20px;">
<div class="Wrap">
<div class="outerWrap">
<div class="innerWrap">
<textarea rows="2" title="Describe this song" name="desc" id="desc" style="height:3em; overflow-y:scroll;" class="textBox" ></textarea>
</div>
</div>
</div>
</div>

<div id="select-list-box" style="width:400px; margin-top:10px;">
<div class="Wrap">
<div class="outerWrap">
<div class="innerWrap">
    <select name="Genre" id="Genre" class="textBox" style="height:auto;">
        <option value="Pick a genre">Pick a genre</option>
        <% foreach (string gr in g)
           { %>
        <option value="<%= gr %>"><%= Html.Encode(gr) %></option>
        <%} %>
    </select>
</div>
</div>
</div>
</div>


<div id="tags-box" style="width:400px; margin-top:10px;">
<div class="Wrap">
<div class="outerWrap">
<div class="innerWrap">
<textarea onfocus="$('#tags-box').hide(); $('#s-t-box').show(); $('#s-t-box').find('li:last input').focus();" id="Textarea1" class="textBox">Tag this song with some keywords</textarea>
</div>
</div>
</div>
</div>


<div id="s-t-box" style="width:400px; margin-top:10px;" class="invisible">
<input id="Tags" type="text" value="" name="Tags" />
</div>


<div id="peoplebox" style="width:400px; margin-top:10px;">
<a href="javascript:(function(event) {$('#peoplebox').hide(); $('#p-t-box').show(); $('#p-t-box').find('li:last input').focus();})();" id="peoplelink">Drop this song on somebody's profile.</a>
</div>

<div id="p-t-box" style="width:400px; margin-top:10px;" class="invisible">
<%= Html.TextBox("People")%>
</div>

</div>


<div id="AddModalDown" style="margin-top:15px;">
<div class="error" id="error" style="width:276px;  float:left;">
</div>
<div style="margin-left:281px;" class="clearfix">
<a href="javascript:void(0);" id="s-link" class="bluebutton" style="width:77px; text-align:center;"  onclick="addbit(); $('#AddShare').click(); $('#s-link').hide(); $('#onesec').show();"> Add it </a>
<img id="onesec" src="../../content/icons/p-load.gif" style="position:relative; top:5px; left:40px;" class="invisible"/>
</div>
</div>


 <input type="submit" id="AddShare" value="Save" class="invisible"/>      


             <%} %>
 </div>      
 
