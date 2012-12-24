<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Welcome
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div style="width:100%; background-color:White; position:absolute;">
<div style=" background: url(../../content/images/HeaderRule.png) no-repeat scroll center 0 transparent; padding-top:5px; position:fixed; top:44px; width:100%;"></div>


<div style="width:900px; margin-left:auto; margin-right:auto; margin-top:50px; margin-bottom:50px;">


<div style="height: 27px; padding-bottom: 25px;">
<h1 style="font-size: 27px; font-weight:300; border-bottom: 1px solid rgb(204, 204, 204); margin: 0px; padding-bottom: 10px; color:#222222;">Select a few genres you like so we can get to know you.</h1>
</div>

<div>
<a class="bluebutton submitbtn" onclick="$('#submit').click();" style="padding-top: 5px; padding-bottom: 5px; margin-top:20px; display:none;">Next</a>
</div>

<div style="padding:5px 0 10px 0;">

<% foreach(string genre in ViewData["genres"] as List<string>){ %>
<a class="welcome-genre" href="javascript:void(0)">
<span class="category"><%= Html.Encode(genre) %></span>
<span class="square">
<span class="check">
<img alt="Checkmark" src="../../content/icons/CheckMark.png" />
</span>
<span class="shadow"></span>
<span class="image" style="background: url(http://passets-cdn.pinterest.com/images/orientation/10.png) 0 -1px repeat;"></span>
</span>
</a>
<%} %>

<div>
<a class="bluebutton submitbtn" onclick="$('#submit').click();" style="padding-top: 5px; padding-bottom: 5px; margin-top:20px; display:none;">Next</a>
</div>

   <% using (Html.BeginForm()) { %>
                    <%=Html.Hidden("genresbox",null)%>
                    <input id="submit" type="submit" value="submit" style="display:none;" />
                     <% } %>
</div>

 


</div>

</div>
   

    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">


<script type="text/javascript">
    $(function () {
        $("#pageLogo").css("position", "relative").css("left", "-85px").css("margin", "0 50%");
        $("#headNav").hide();
        $("html").css("background-color", "#ffffff")
    });

    $(document).ready(function () {
        $('.welcome-genre').click(function () {

            var genre = $(this).find(".category").text();

            if (!$(this).hasClass('selected')) {
                $(this).addClass('selected');
                $('#genresbox').val($('#genresbox').val() + genre + ",");

            } else {
                $(this).removeClass('selected');
                $('#genresbox').val($('#genresbox').val().replace(genre + ",", ""));
            }

            if ($('#genresbox').val().length > 0 && !$('.submitbtn').is(":visible")) { $('.submitbtn').fadeIn('slow'); } else if (!$('#genres-box').val().length > 0) { $('.submitbtn').fadeOut('slow'); }
        });
    });
</script>

<style type="text/css">
    
body{
    background-color: #ffffff;
    height:100%;
    }
    
.welcome-genre {
    cursor: pointer;
    float: left;
    font-weight: normal;
    margin: 0 14px 22px 0;
    position: relative;
    width: 113px;
}

.welcome-genre .category {
    display: block;
    font-size: 13px;
    font-weight: 300;
    line-height: 17px;
    overflow: hidden;
    padding: 10px 0;
    white-space: nowrap;
}

.welcome-genre .square {
    border: 1px solid #F2F0F0;
    display: block;
    height: 112px;
    position: relative;
    width: 112px;
    z-index: 1;
}

.welcome-genre .check {
    bottom: 35px;
    display: none;
    position: absolute;
    right: 32px;
    z-index: 3;
}


.selected {
    background-color:transparent;
    }

.welcome-genre .square .image {
    display: block;
    height: 112px;
    width: 112px;
}


.selected .square {
    background-color: green;
    border-color: darkgreen;
}

.selected .check {
    display: block;
    opacity: 0.9;
}


.selected .square .image {
    opacity: 0.2;
}


a{
    color: #221919;
    font-weight: bold;
    text-decoration: none;
}

a:hover {
    color: #CB2027;
}



</style>

</asp:Content>
