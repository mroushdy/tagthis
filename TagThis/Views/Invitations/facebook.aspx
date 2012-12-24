<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Share TagThis with friends
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("InviteMethods"); %>

        <div style="width:80%; background-color:#F1F1F1; margin:auto; padding:10px; height:300px; margin-top:7px;">
       A facebook popup window should apear now. If it does not open please refresh this page.
        </div>
        <p></p>

        <script src="http://connect.facebook.net/en_US/all.js"></script>
    <div id="fb-root"></div>
    <script>
        // assume we are already logged in
        FB.init({ appId: '<%= Facebook.FacebookApplication.Current.AppId%>', xfbml: true, cookie: true });

        FB.ui({
            method: 'send',
            name: 'Facebook Dialogs',
            display: 'popup',
            link: 'http://kickass.io/'
        });
     </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
		<script type="text/javascript" charset="utf-8">	
		$(document).ready(function(){  
		$("#facebook").addClass("selected");
		});
		</script>
</asp:Content>
