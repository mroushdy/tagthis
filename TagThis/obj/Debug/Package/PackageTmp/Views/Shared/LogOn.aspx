 <%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	TagThis | less searching more finding and more sharing.
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
      // if (Request.ServerVariables["SERVER_NAME"].StartsWith("www.") || Request.ServerVariables["SERVER_NAME"].StartsWith("http://www."))
  // {
  //     Response.Redirect("http://tagthis.com");
  // }
    
    if (Facebook.Web.FacebookWebContext.Current.IsAuthenticated())
    {
        Response.Redirect("~/account/GetFb/");
    }
    

    Random random = new Random();
    int randomNumber = random.Next(1, 5);
 %>

<div>
<img id="backgroundimg" src="../../content/landingpix/1.jpg" style="position:fixed; z-index:0;"/>
</div>

<div style="width:100%; position:absolute; top:50%; bottom:50%; margin-top:-50px; z-index:1; text-align:center;">

<div style="font:normal normal 600 64px/95% Open Sans, Helvetica Neue, Helvetica, Arial, Lucida Grande; letter-spacing:-2px; color:#ffffff;">DISCOVER MUSIC TOGETHER.</div>
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

<a id="fbcon" href="javascript:void(0);" onclick="fblogin();return false;"></a>
</div>

</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
<meta name="description" content="Sixtysongs lets you share music that inspires you, discover new songs, appreciate what others share, and interact in the name of music." />
<meta name="keywords" content="tagthis.com, tagthis, tagging, tag, share, social, personalization, recomendation "/>

<link href='http://fonts.googleapis.com/css?family=Open+Sans:400,300,600' rel='stylesheet' type='text/css'>

<script type="text/javascript">
    $(document).ready(function () {
        $("#headNav").hide();
        $("html").css("background-color", "#ffffff");
        var width = $(window).width();
        $("#backgroundimg").width(width);
    });

    $(window).resize(function () {
        var width = $(window).width();
        $("#backgroundimg").width(width);
     });
</script>

<style type="text/css">
    
body{
    background-color: #ffffff;
    height:100%;
    }
    

#fbcon {
    background: url("../../content/icons/cwf.png") no-repeat scroll 0px 0 transparent;
    display: block;
    height: 38px;
    width: 255px;
    margin:0 auto;
}

#fbcon:hover {
background-position: 0 -50px;
}

#fbcon:active {
background-position: 0 -100px;
}

</style>


<script>
    //your fb login function
    function fblogin() {
        FB.login(function (response) {


            window.location.reload();


        }, { perms: 'email, publish_actions' });
    }
</script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="LeftContent" runat="server">
</asp:Content>
