<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Finish
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Finish</h2>
			        <%if (ViewData["Messages"] != null)
                    {
                        foreach (string Message in (ViewData["Messages"] as List<string>))
                        { %>
                        <%=Message%><br />
                    
                        <%}} %>
                        <br />
                        Please <a href="javascript:window.close();">close this window</a> and click TagThis again to tag another site.
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="HeadContent" runat="server">
		<script type="text/javascript" charset="utf-8">	
			$(document).ready(function(){			
			$('#MainPage').css('width','90%');
			$('#menucontainer').css('display','none');
			$('#footer').css('display','none');
			});
			</script>
</asp:Content>
