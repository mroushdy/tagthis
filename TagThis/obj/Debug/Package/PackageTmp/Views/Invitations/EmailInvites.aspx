<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Share TagThis with friends
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.RenderPartial("InviteMethods"); %>
    <div class="clearfix" style="width:80%; background-color:#F1F1F1; padding:10px 0px 10px 0px; margin-bottom:20px; margin:auto;">
    <%string c = "background-color:#F1F1F1;"; int i = 0;%>
    <%foreach(TagThis.Controllers.MailContact m in (ViewData["contacts"] as List<TagThis.Controllers.MailContact>)){ 
      if(c =="background-color:#F1F1F1;"){c="background-color:#FFFFFF;";}else{c= "background-color:#F1F1F1;";}    %>
    <div style="width:95%; padding:10px 5px 10px 5px; clear:both; margin:auto; <%=c.ToString()%>">
    <span style="text-align:left"><%=Html.Encode(m.Email) %></span>
    <div id="invite<%=i.ToString()%>" style="text-align:right; float:right;">
<%ViewData["email"]= m.Email;%>
<%ViewData["DivID"]= "invite"+i.ToString();%>
<%Html.RenderPartial("InviteButton"); i++;%>     
    </div>
    </div>
<%} %>
    </div>
<p></p>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<%string action = ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();%>
		<script type="text/javascript" charset="utf-8">	
				$(document).ready(function(){  
		$("#<%= action.ToString()%>").addClass("selected");
		});
		</script>
</asp:Content>
