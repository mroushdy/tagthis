<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%string url = ViewData["shareurl"].ToString(); %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Index</title>
</head>
<body style="margin:0;">
<div id="fb-root"></div>
<script>    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/en_US/all.js#xfbml=1&appId=258026920902510";
        fjs.parentNode.insertBefore(js, fjs);
    } (document, 'script', 'facebook-jssdk'));</script>

         <!--facebook -->
        
        <div style="margin:0 0 4px 0;" class="shareit-fb">
        <div class="fb-like" data-href="<%= url %>" data-send="false" data-layout="button_count" data-width="150" data-show-faces="false"></div>
        </div>

        <!-- tweet -->
        <a href="http://twitter.com/share" class="twitter-share-button" data-url="<%= url %>" data-count="horizontal" data-via="sixtysongs">Tweet</a><script type="text/javascript" src="http://platform.twitter.com/widgets.js"></script>

        <div style="height:4px;">&nbsp;</div>

        <!-- linkedin share -->

        <script type="text/javascript" src="http://platform.linkedin.com/in.js"></script><script type="in/share" data-url="<%= url %>" class="shareit-li" data-counter="right"></script>

        <div style="height:4px;">&nbsp;</div>

        <!--- google + --->

        <!-- Place this tag where you want the +1 button to render -->
        <g:plusone size="medium" href="<%= url %>" class="shareit-ggl"></g:plusone>

        <script type="text/javascript">
            (function () {
                var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
                po.src = 'https://apis.google.com/js/plusone.js';
                var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
            })();
            </script>

</body>
</html>
