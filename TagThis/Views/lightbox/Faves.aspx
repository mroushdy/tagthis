<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	My Faves
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <span id="resultskey" style="font-size:medium;">
        My Faves
       </span>
       <span id="keylinks">
       <%=Html.Encode(" | ")%><%=Html.ActionLink("Tag a Page","index?u="+Request.QueryString["u"],"lightbox")%>
       </span>    
    </div>
            <div id="searchcontainer" style="margin-bottom:-8px; padding-top:3px;">
             <% using (Ajax.BeginForm("faves", "lightbox", new AjaxOptions() { UpdateTargetId = "lbfavdiv" }))
                {%>
<table border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td valign="baseline">
    <div id="simple" style="width:420px;"><%= Html.TextBox("favebox", "")%></div>
    </td>
    <td>
        <input type="submit" value="" alt="Search" class="button" style="margin:4px 0 0 -4px; background:transparent url(../../content/images/dropdown/searchb.png) no-repeat; width:15px; border:none;" />  
</td>
</tr>
</table>
 <% } %>
        </div>
<%Html.RenderPartial("LBfaves"); %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="HeadContent" runat="server">
		<script type="text/javascript" charset="utf-8">	
			$(document).ready(function(){			
			$('#MainPage').css('width','90%');
			$('#menucontainer').css('display','none');
			$('#footer').css('display','none');
			var favessearch = new TextboxList('#favebox', {unique: true,bitsOptions:{editable:{addKeys: 32}},plugins: {autocomplete: {minLength: 3,queryRemote: true,remote: {url: '../../content/TagSuggestHandler.ashx'}}}});
			});
			</script>
</asp:Content>
