<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%TagThis.Models.TagRepository TR = new TagThis.Models.TagRepository();%>
<%String[] tagnames = TR.GetUserTags((int)ViewData["pageid"]);%>
		<script type="text/javascript" charset="utf-8">	
		
			$(document).ready(function(){			
				
                inittagbox<%=ViewData["pageid"].ToString()%>()
			});

            function inittagbox<%=ViewData["pageid"].ToString()%>()
            {
            				// With custom adding keys
				var t<%=ViewData["pageid"].ToString()%> = new TextboxList('#Tags<%=ViewData["pageid"].ToString()%>', {unique: true ,bitsOptions:{editable:{addKeys: 32, addOnBlur:true}}});
			        <%if (tagnames != null){foreach (string tag in tagnames){ %>
                      t<%=ViewData["pageid"].ToString()%>.add('<%=tag %>');
                      <%}} %>
            }
	    </script>
    <% if (Request.IsAuthenticated)
   { %>
    <% using (Ajax.BeginForm("SubmitTags", "Ajax", new { pid = ViewData["pageid"] }, new AjaxOptions() { UpdateTargetId = "tagbox" + ViewData["pageid"].ToString(), OnSuccess="inittagbox"+ViewData["pageid"].ToString() }, new { name = "tagform" + ViewData["pageid"].ToString() }))
       {%>

    <div class="tag-textbox"><%= Html.TextBox("Tags" + ViewData["pageid"].ToString())%></div>     
    <div class="help" align="left">Type tag names above, seperated by spaces.</div>
    <a href="javascript:void(0);" onclick="$(this).next('.submit').click(); $('#tagbox<%= ViewData["pageid"].ToString()%>').slideUp();" class="bluebutton">Save</a>
    <a href="javascript:void(0);" onclick="$('#tagbox<%= ViewData["pageid"].ToString()%>').slideUp();" class="bluebutton" style="margin-left:4px;">Close</a>
    
    <input type="submit" class="invisible submit"/>
    <% } %>
    <% } else{%>
    Log in to add tags
    <% } %>