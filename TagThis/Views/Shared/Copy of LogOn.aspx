<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<% 
    if (Facebook.Web.FacebookWebContext.Current.IsAuthenticated())
    {
        Response.Redirect("~/account/GetFb/" + Membership.GetUser("marwan").ProviderUserKey.ToString());
    }
    
    %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
<title>Untitled Document</title>

<style type="text/css">

h1 {
font-family:  'Helvetica Neue', Helvetica, Arial, sans-serif;
font-size:30px;
font-weight:bold;
letter-spacing:-1px;
color:#FFFFFF;
margin:0;

}


h2 {
font-family:  'Helvetica Neue', Helvetica, Arial, sans-serif;
font-size:20px;
font-weight:bold;
color:#FFFFFF;
margin:0;

}

a {
    color: #024AF6;
    cursor: pointer;
    text-decoration: none;
    cursor: pointer;
	font-family:Arial, Helvetica, sans-serif;
    font-size: 12px;
}

body {
    color: #666666;
    direction: ltr;
    font-family:Arial, Helvetica, sans-serif;
    font-size: 14px;
    text-align: left;
}

#redBar {
    background-color: #F33B3B;
    height: 41px;
    left: 0;
    position: fixed;
    width: 100%;
    z-index: 5;
}


#globalContainer {
    left: 100px;
    position: fixed;
	width: 981px;
	margin: 0 auto;
	z-index: 6;
}

#pageHead {
    margin: 0;
    width: auto;
	display: block;
	padding-top: 5px;
}

#pageLogo {
    float: left;
    margin-left: -6px;
    z-index: 5;
	margin:0;
}

#pageLogo a {

    background-image: url("../../content/icons/kickass.png");
    background-position: 0 0;
    background-repeat: no-repeat;
    display: block;
    height: 31px;
    width: 150px;
}





.textBox {
    border: 0 none;
    font: 13px "arial";
	color:#726666;
    height: 14px;
    padding: 2px 0;
	outline:none;
	
	overflow: hidden;
	resize:none;
	width:100%;
}


.Wrap {
    background: none repeat scroll 0 0 #FFFFFF;
    color: #1A1A00;
    cursor: text;
    font: 11px "Lucida Grande",Verdana;


}


.bluelink {

	background-color: rgb(6, 158, 236);
	color: white !important;
	cursor: pointer;
	display: inline-block;
	font: normal normal bold 18px/18px Verdana !important;
	padding: 7px 30px;
}

.outerWrap {
    border: 1px solid #999999;
    margin: 0;
    overflow: hidden;
    padding: 3px 4px 0;

	   
}

.innerWrap {
	border: 1px solid #FFFFFF;
	float: left;
    list-style-type: none;
    margin: 0 5px 3px 0;
    padding: 0;
	width:100%;

}





/*experiment start*/

html {
    background: url("../../content/images/home/bg_tile.jpg") repeat fixed 0 50px transparent;
    overflow-y: scroll;
}


#title {
    left: 100px;
    position: fixed;
    top: 60px;
}


#cover {
    background: url("../../content/images/home/cover.png") no-repeat scroll 0 0 transparent;
    height: 181px;
    margin-right: -12px;
    position: fixed;
    right: 12%;
    width: 514px;
    z-index: 3;
}



#roll #paper {

    background-image: url("../../content/images/home/t-paper.png");
    padding-top: 180px;
	
   background-repeat:no-repeat;
	
	width:450px;
	
	
	
	
	
	height:1515px;
	
	position:fixed;
	
	
	top:-1200px;
	
	margin-right: 18px;
	right: 12%;
	
	z-index: 2;
	
	margin-bottom: 50px;
}



#base {
    background: url("../../content/images/home/bottom.png") no-repeat scroll 0 0 transparent;
    height: 446px;
    margin-right: -106px;
    position: fixed;
    right: 12%;
    width: 682px;
    z-index: 1;
}

.intro {

font-size:14px;
font-family:  'Helvetica Neue', Helvetica, Arial, sans-serif;
width:250px;
color:#CCCCCC;

}


.invisible {

display:none;

}


#dummy {

position:absolute;
top:0px;
width:10px;
height:10px;
z-index:100;

}

</style>


<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>

    	<script src="../../Scripts/MicrosoftAjax.js" type="text/javascript"></script>
	<script src="../../Scripts/MicrosoftMvcAjax.js" type="text/javascript"></script>

<script type="text/javascript">





    $(document).ready(function () {


        $('input[title]').each(function () {
            if ($(this).val() === '') { $(this).val($(this).attr('title')); }

            $(this).focus(function () {
                if ($(this).val() === $(this).attr('title')) { $(this).val(''); }
            });

            $(this).blur(function () {
                if ($(this).val() === '') { $(this).val($(this).attr('title')); }
            });
        });

        //adapt the height of the window for proper scroll

        wadapt();


    });


    //avoid repition on resize
    var delay = (function () {
        var timer = 0;
        return function (callback, ms) {
            clearTimeout(timer);
            timer = setTimeout(callback, ms);
        };
    })();


    $(window).resize(function () {
        delay(function () {

            wadapt();

        }, 500);
    });


    function wadapt() {

        var hw = $(window).height();
        var hp = $('#paper').height();

        var h = hw + hp;

        $('#dummy').height(h);

    }






    $(window).scroll(function () {

        // -1200 is the initial top value of the paper

        var x = $(window).scrollTop() - 1200;

        //boundaries
        if (x >= 150) { x = 150; }
        if (x <= -1200) { x = -1200; }

        $("#paper").css('top', x);


    });



</script>


</head>

<body style="margin:0px;">

<div id="redBar" class=""></div>
<div id="globalContainer" class="">
<div id="pageHead" class="clearfix" role="banner">
<h1 id="pageLogo">
<a title="Home" href="http://www.facebook.com/?ref=logo"></a>
</h1>
</div>
</div>





<div id="title">



<h1 style="margin-top:10px; width:250px;">kickass is ridiculously fun</h1>


<h2 style="margin-top:20px;">Are you kickass?</h2>

<p class="intro"> Connect with facebook to start. </p>


<div id="connectbuttons">
  <% Html.RenderPartial("FacebookInit"); %>
  <fb:login-button perms="email,publish_stream,read_stream,offline_access" >&nbsp;&nbsp; Connect With Facebook &nbsp;&nbsp;</fb:login-button>
</div>

<div style="display:none;">
        <% using (Ajax.BeginForm("Site", "Invitations", new { }, new AjaxOptions() { InsertionMode = InsertionMode.InsertBefore, UpdateTargetId = "invitediv", OnSuccess = "function() {$('#invitebutton').show(); }" }, new { }))
           { %>


<div style="width:250px; margin-top:10px;">
<div class="Wrap">
<div class="outerWrap">
<div class="innerWrap">
<input type="text" name="Email" id="Email" class="textBox" title="email"/>
</div>
</div>
</div>
</div>
              
<div id="invitediv" style="width:200px; margin-top:4px;">
<a href="javascript:void(0);" id="invitebutton" class="bluelink" onclick="$('#invitesub').click(); $('#invitebutton').hide();" style="width:190px; margin-top:6px; margin-right:50px; text-align:center;"> be kickass </a>
<input id="invitesub" type="submit" class="invisible" />
</div>
                  
    <% } %>

    </div>

<h2 style="margin-top:20px;">Of course I'm kickass</h2>



    <% using (Html.BeginForm("LogOn","Account")) { %>    

 
<div style="width:250px; margin-top:20px;">
<div class="Wrap">
<div class="outerWrap">
<div class="innerWrap">
<input type="text" name="username" id="username" class="textBox" title="user name or email"/>
</div>
</div>
</div>
</div>
<%= Html.ValidationMessage("username", new { style="color:red;"})%>   
    
<div style="width:250px; margin-top:10px;">
<div class="Wrap">
<div class="outerWrap">
<div class="innerWrap">
<input type="password" onfocus="" name="password" id="password" class="textBox"/>
</div>
</div>
</div>
</div>
<%= Html.ValidationMessage("password", new { style = "color:red; display:block;" })%>


<a href="#" class="bluelink" style="margin-top:10px; width:190px; margin-right:50px;  text-align:center;" onclick="$('#logon').click();"> sign in </a>

<input type="submit" id="logon" class="invisible"/>

<div style="color:white; font-size:13px; margin-top:10px;">
<%= Html.CheckBox("rememberMe", false, new { style="margin-bottom:-4px; margin-left:-5px;"})%> <label class="inline" for="rememberMe" style="margin-right:13px; margin-left:-3px;">Remember me?</label><%= Html.ActionLink("Forgot your password?", "Forgot", "Account", new { style = "color:white;font-size:13px;" })%>
</div>

<% } %>

</div>




<div id="cover"></div>


<div id="roll">
<div id="paper" style="display: block;">

</div>


</div>


<div id="base"></div>


<div id="dummy"> &nbsp; </div>

</body>

</html>
