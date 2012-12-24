<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage"  %>

<asp:Content ID="Title" ContentPlaceHolderID="TitleContent" runat="server">
    Add tags
</asp:Content>

<asp:Content ID="Head" ContentPlaceHolderID="HeadContent" runat="server">
		
		<!-- sample initialization -->
		<script type="text/javascript" charset="utf-8">	
		
			$(document).ready(function(){			
			   var t2 = new TextboxList('#Tags', {unique: true ,bitsOptions:{editable:{addKeys: 32, addOnBlur:true}}});
			   		 <%if (ViewData["NewPage"] == "0" && ViewData["oTags"] != null)
                    {
                        foreach (string tag in (ViewData["oTags"] as String[]))
                        { %>
                      t2.add('<%=tag %>');
                      <%}
                    } %>
			   
			$('#MainPage').css('width','90%');
			$('#menucontainer').css('display','none');
			$('#footer').css('display','none');
			});
			
			
			function fav(){
			if(document.getElementById('Fav').value=="0")
			{document.getElementById('Fav').value="1"; 
	        document.getElementById('FavButton').style.backgroundImage = "url(../../Content/icons/F1.gif)";}
			else{
			document.getElementById('Fav').value="0"; 
	        document.getElementById('FavButton').style.backgroundImage = "url(../../Content/icons/F0.gif)";}
			}
			
			function Love(){
			if(document.getElementById('Rate').value=="1")
			{
			document.getElementById('Rate').value="0"; 
	        document.getElementById('LoveButton').style.backgroundImage = "url(../../Content/icons/L0.gif)";
	        document.getElementById('HateButton').style.backgroundImage = "url(../../Content/icons/H0.gif)";
			}
			else
			{
			document.getElementById('Rate').value="1"; 
			document.getElementById('LoveButton').style.backgroundImage = "url(../../Content/icons/L1.gif)";
			document.getElementById('HateButton').style.backgroundImage = "url(../../Content/icons/H0.gif)";
			}
			}
			
			function Hate(){
			if (document.getElementById('Rate').value=="-1")
			{
			document.getElementById('Rate').value="0"; 
	        document.getElementById('LoveButton').style.backgroundImage = "url(../../Content/icons/L0.gif)";
	        document.getElementById('HateButton').style.backgroundImage = "url(../../Content/icons/H0.gif)";
			}
		    else
			{
			document.getElementById('Rate').value="-1"; 
			document.getElementById('HateButton').style.backgroundImage = "url(../../Content/icons/H1.gif)";
			document.getElementById('LoveButton').style.backgroundImage = "url(../../Content/icons/L0.gif)";
			}
			}
			
			function addLoadEvent(func) {
            var oldonload = window.onload;
            if (typeof window.onload != 'function') {
            window.onload = func;
          } else {
            window.onload = function() {
              if (oldonload) {
                oldonload();
              }
              func();
            }
          }
        }
        
        addLoadEvent(function() {
        /* more code to run on page load */ 
        if('<%=ViewData["oFav"].ToString()%>' == "1")
        {
	    document.getElementById('FavButton').style.backgroundImage = "url(../../Content/icons/F1.gif)";
	    }
	    else
	    {
	    document.getElementById('FavButton').style.backgroundImage = "url(../../Content/icons/F0.gif)";
	    }
	    
	    if('<%=ViewData["oRate"].ToString()%>' == "0")
        {
        
	    document.getElementById('LoveButton').style.backgroundImage = "url(../../Content/icons/L0.gif)";
	    document.getElementById('HateButton').style.backgroundImage = "url(../../Content/icons/H0.gif)";}
	    else if ('<%=ViewData["oRate"].ToString()%>' == "1")
	    {
	    document.getElementById('LoveButton').style.backgroundImage = "url(../../Content/icons/L1.gif)";
	    document.getElementById('HateButton').style.backgroundImage = "url(../../Content/icons/H0.gif)";}
	    else if ('<%=ViewData["oRate"].ToString()%>' == "-1")
	    {
	    document.getElementById('HateButton').style.backgroundImage = "url(../../Content/icons/H1.gif)";
	    document.getElementById('LoveButton').style.backgroundImage = "url(../../Content/icons/L0.gif)";
	    }
        });
        
        $(document).ready(function(){
        $("#FavButton").hover(function(){
          if (document.getElementById('Fav').value != "1"){$('#FavButton').css('background','url(../../Content/icons/F1.gif)');
          document.getElementById("ALT").innerHTML="Add to Favourites";}
	     },function(){
   		if (document.getElementById('Fav').value != "1"){$('#FavButton').css('background','url(../../Content/icons/F0.gif)');
   		document.getElementById("ALT").innerHTML="";} // This sets the opacity back to 60% on mouseout
	                  });

        $("#LoveButton").hover(function(){
		if (document.getElementById('Rate').value != "1"){$('#LoveButton').css('background','url(../../Content/icons/L1.gif)');
		document.getElementById("ALT").innerHTML="Love This Website";}
	     },function(){
   		if (document.getElementById('Rate').value != "1"){$('#LoveButton').css('background','url(../../Content/icons/L0.gif)');
   		document.getElementById("ALT").innerHTML="";} // This sets the opacity back to 60% on mouseout
	                  });	
	    $("#HateButton").hover(function(){
		if (document.getElementById('Rate').value != "-1"){$('#HateButton').css('background','url(../../Content/icons/H1.gif)');
		document.getElementById("ALT").innerHTML="Hate This Website";}
	     },function(){
   		if (document.getElementById('Rate').value != "-1"){$('#HateButton').css('background','url(../../Content/icons/H0.gif)');
   		document.getElementById("ALT").innerHTML="";} // This sets the opacity back to 60% on mouseout
	                  });	
	     });
        
		</script>
		
		<!-- sample style -->
		<style type="text/css" media="screen">
			.form_tags { margin-bottom: 10px;}
			
			/* Setting widget width example */
			.form_tags .textboxlist, #form_friends .textboxlist { width: 400px; }
			
			/* Preloader for autocomplete */
			.textboxlist-loading { background: url('../content/close.gif') no-repeat 380px center; }
			
			/* Autocomplete results styling */
			#form_friends .textboxlist-autocomplete-result { overflow: hidden; zoom: 1; }
			#form_friends .textboxlist-autocomplete-result img { float: left; padding-right: 10px; }
			
			.note { color: #666; font-size: 90%; }
			#footer { margin: 50px; text-align: center; }
			
			.content {
            width: 400px ;
            margin-left: auto ;
            margin-right: auto ;
                   }
               .button {
            width: 21px ;
            height: 19px;
            cursor: pointer;
                   }
		</style>
</asp:Content>

<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <span id="resultskey" style="font-size:medium;">
        Tag a Page
       </span>
       <span id="keylinks">
       <%=Html.Encode(" | ")%><%=Html.ActionLink("My Faves","Faves?u="+Request.QueryString["u"],"lightbox")%>
       </span>    
    </div>
    <br />
    <div class="content">
    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
     <%= Html.ValidationMessage("Pageload") %>
    <div class="Box" align="left">
    <br />
    <div class="Title"><img alt="" style="width:16; height:16;" src="<%=ViewData["IconUrl"].ToString()%>"/> <strong><%=Html.Encode(ViewData["Title"])%></strong></div>
    <div class="Desc"><%=ViewData["Desc"].ToString()%></div>
    <br />
    </div>
    <% using (Html.BeginForm()) {%>
    <div style="width:400px">
             <%= Html.TextBox("Tags") %>
                </div>

     <div class="help" align="left">Type tag names above, seperated by spaces.</div>
            <p>
            <%= Html.Hidden("Title",ViewData["Title"].ToString())%>
            <%= Html.Hidden("Fav", ViewData["oFav"].ToString())%>
            <%= Html.Hidden("Rate", ViewData["oRate"].ToString())%>
            <%= Html.Hidden("oFav", ViewData["oFav"].ToString())%>
            <%= Html.Hidden("oRate", ViewData["oRate"].ToString())%>
            <%= Html.Hidden("Desc",Html.Encode(ViewData["Desc"]))%>
            <%= Html.Hidden("url",ViewData["url"].ToString())%>
            <%= Html.Hidden("NewPage", ViewData["NewPage"].ToString())%>
            <%= Html.Hidden("IconUrl", ViewData["IconUrl"].ToString())%>
       
       <table border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td><input type="submit" value="Save Changes" />&nbsp;&nbsp;</td>
    <td style="width:30px;"><div id="FavButton" class="button" onclick="fav()" /></td>
    <td style="width:30px;"><div id="LoveButton" class="button" onclick="Love()"  />&nbsp;&nbsp;</td>
    <td style="width:30px;"><div id="HateButton" class="button" onclick="Hate()" style="height:20px;" /></td>
    <td><span id="ALT"></span></td>
  </tr>
</table>
            </p>
         

    <% } %>
        <div class="error" style="width:400px">
<%if (ViewData["error"] != null)
  {%>
  <%=Html.Encode(ViewData["error"].ToString()) %>
  <%} %>
  </div>
          </div>
</asp:Content>

