<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Join SixtySongs
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%

    Random random = new Random();
    int randomNumber = random.Next(1, 5);
 %>
<div style="width:100%; background-color:White; position:absolute;">
<div style=" background: url(../../content/images/HeaderRule.png) no-repeat scroll center 0 transparent; padding-top:5px; position:fixed; top:44px; width:100%;"></div>
<div style="width:430px; margin-left:auto; margin-right:auto; margin-top:50px; margin-bottom:50px;">

<div style="height: 27px; padding-bottom: 25px;">
<h1 style="font-size: 27px; font-weight:300; margin: 0px; padding-bottom: 10px; border-bottom: 1px solid rgb(204, 204, 204); color:#222222;">Sign Up</h1>
</div>
<span style="margin:10px 0 20px 0;">Connect with Facebook to get started.</span>



<div style="text-align:center; margin-bottom:30px; margin-top:30px;">

<div id="connectbuttons">
<script>
    //your fb login function
    function fblogin() {
        FB.init({ appId: '<%= Facebook.FacebookApplication.Current.AppId%>', status: true, cookie: true,
            xfbml: true
        });
        FB.login(function (response) {
            window.location = '<%=Url.Action("GetFb","account")%>';
        }, { perms: 'email, publish_actions' });
    }
</script>
<a id="fbcon" href="javascript:void(0);" onclick="fblogin();return false;"></a>
</div>

</div>



<div style="width: 350px; padding: 30px 40px 10px 40px; border-top: 1px solid rgb(234, 234, 234);" >


<h1 style="font-size: 18px; font-weight:300; margin: 0px; padding-bottom: 10px; color:#444;">Why connect with Facebook?</h1>
<span style="margin:10px 0 20px 0;">Connecting helps us surface music that is relevant to you and conenct you with your friends so you can listen to their music. We will never spam your FB wall. We hate that too.</span>





</div>

</div>
</div>
 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

<script type="text/javascript">
    $(document).ready(function () {
        $("#pageLogo").css("position", "relative").css("left", "-69px").css("margin", "0 50%");
        $("#logon").hide(); $("#SearchBox").hide();
        $("html").css("background-color", "#ffffff");
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

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="LeftContent" runat="server">
</asp:Content>
