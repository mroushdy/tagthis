<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Import
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%



    Random random = new Random();
    int randomNumber = random.Next(1, 5);
 %>
   
<div style="width:100%; background-color:White; position:absolute;">
<div style=" background: url(../../content/images/HeaderRule.png) no-repeat scroll center 0 transparent; padding-top:5px; position:fixed; top:44px; width:100%;"></div>
<div style="width:560px; margin-left:auto; margin-right:auto; margin-top:50px; margin-bottom:50px;">

<div style="height: 27px; padding-bottom: 25px;">
<h1 style="font-size: 27px; font-weight:300; border-bottom: 1px solid rgb(204, 204, 204); margin: 0px; padding-bottom: 10px; color:#222222;">Import Songs You Shared on Facebook</h1>
</div>

<span style="margin:10px 0 20px 0;">Save the songs that inspire you from being lost on your wall and import them to SixtySongs for you and your followers to enjoy.</span>


<div style="width: 450px; padding:5px 0 10px 0;">

<div id="connectbuttons" style="margin-top:30px;">
<div id="fb-root"></div>
<script>
    //initializing API
    window.fbAsyncInit = function () {
        FB.init({ appId: '<%= Facebook.FacebookApplication.Current.AppId%>', status: true, cookie: true,
            xfbml: true
        });
    };
    (function () {
        var e = document.createElement('script'); e.async = true;
        e.src = document.location.protocol +
              '//connect.facebook.net/en_US/all.js';
        document.getElementById('fb-root').appendChild(e);
    } ());
</script>

<a id="fbimport" class="bluebutton" href="javascript:void(0);" onclick="fblogin();return false;" style="width:200px; text-align:center;">Import Songs</a>
</div>

<div style="margin:10px 0 0;">
<%= Html.ActionLink("Skip This Step","People","Account",new {},new {})%>
</div>

</div>



</div>

</div>

    <script type="text/javascript">
        $(function () {
            $("#pageLogo").css("position", "relative").css("left", "-85px").css("margin", "0 50%");
            $("#headNav").hide();
            $("html").css("background-color", "#ffffff")
        });
</script>
</asp:Content>



<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

<script>
    //your fb login function
    function fblogin() {
        FB.login(function (response) {


            window.location.href = '/account/DoImport/';


        }, { perms: 'read_stream' });
    }
</script>

</asp:Content>