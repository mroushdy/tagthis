<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
   
<%
      // if (Request.ServerVariables["SERVER_NAME"].StartsWith("www.") || Request.ServerVariables["SERVER_NAME"].StartsWith("http://www."))
  // {
  //     Response.Redirect("http://tagthis.com");
  // }
    
    if (Facebook.Web.FacebookWebContext.Current.IsAuthenticated())
    {
        Response.Redirect("~/account/GetFb/");
    }

 %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	TagThis | less searching more finding and more sharing.
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div style="width:100%; height:100%; background-color:White; position:absolute;">

<div style="width:390px; background-color:rgb(255,255,255,0.75); -webkit-border-radius: 2px; -moz-border-radius: 2px; border-radius: 2px;"></div>
<div style="font:normal normal bold 49px/95% helvetica Neu; color:#000;">Discover music together</div>
<div style="font:normal normal bold 49px/95% helvetica Neu; color:#000;">Sixtysongs lets you share music that inspires you, discover new songs, appreciate what others share, and interact in the name of music.</div>
<div id="connectbuttons" style="margin-top:20px;">
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
<meta name="description" content="TagThis understands who you are and helps you discover awesome websites shared by random people just like you. The more you use it the smarter it gets the more awesome things you discover." />
<meta name="keywords" content="tagthis.com, tagthis, tagging, tag, share, social, personalization, recomendation "/>


<script type="text/javascript">
    $(function () {
        $("#headNav").hide();
        $("html").css("background-color", "#ffffff")
    });

    $(document).ready(function () {

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


        }, { perms: 'email,read_stream,publish_stream,offline_access' });
    }
</script>

</asp:Content>
