<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Error
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Error</h2>
An error occured while loading the URL provided. Please check the URL and try again.
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
		<script type="text/javascript" charset="utf-8">	
			$(document).ready(function(){			
			$('#MainPage').css('width','90%');
			$('#menucontainer').css('display','none');
			$('#footer').css('display','none');
			});
			</script>
</asp:Content>
