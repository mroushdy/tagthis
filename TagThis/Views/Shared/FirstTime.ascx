<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

       <% ViewData["Results"] = ViewData["Results"]; %>
       <% Html.RenderPartial("ResultsPartial"); %>


       <div class="introbox introtop introleft intro1">
       <i class="introarrow"></i>
       <div class="introcontent">
       <div class="introtitle">Welcome to Sixtysongs!</div>
       <div class="introtext">This short tutorial will explain to you how to make the most out of Sixtysongs.</div>
       <div class="introbutton">
       <a class="bluebutton">Next</a>
       </div>
       </div>
       </div>


       <div class="introbox introtop introleft intro2 invisible">
       <i class="introarrow"></i>
       <div class="introcontent">
       <div class="introtitle">What's it all about?</div>
       <div class="introtext">On Sixtysongs people help each other discover good music. The best songs find you through people you trust.</div>
       <div class="introbutton">
       <a class="bluebutton">Next</a>
       </div>
       </div>
       </div>


       <div class="introbox introbottom introleft intro3 invisible">
       <i class="introarrow"></i>
       <div class="introcontent">
       <div class="introtitle">Home away from home.</div>
       <div class="introtext">This is your homepage it should be filled with interesting music from people you follow. Following someone means that you like the music they share. You can unfollow people any time.</div>
       <div class="introbutton">
       <a class="bluebutton">Next</a>
       </div>
       </div>
       </div>


       <div class="introbox introtop introleft intro4 invisible">
       <i class="introarrow"></i>
       <div class="introcontent">
       <div class="introtitle">How do I find followers?</div>
       <div class="introtext">If you change your home feed from "people you follow" to "similar people" you'll find song suggestions from people with similar taste in music. Follow who ever seems cool enough.</div>
       <div class="introbutton">
       <a class="bluebutton">Next</a>
       </div>
       </div>
       </div>


       <div class="introbox introtop introright intro5 invisible">
       <i class="introarrow"></i>
       <div class="introcontent">
       <div class="introtitle">How do I share songs?</div>
       <div class="introtext">Click on "Add a Song", from there you can paste a youtube link to whatever song you want to share.</div>
       <div class="introbutton">
       <a class="bluebutton">Next</a>
       </div>
       </div>
       </div>

       <div class="introbox introtop introright intro6 invisible">
       <i class="introarrow"></i>
       <div class="introcontent">
       <div class="introtitle">Got friends with good taste in music?</div>
       <div class="introtext">Invite them and help us grow the Sixtysongs community.</div>
       <div class="introbutton">
       <a class="bluebutton">Finish</a>
       </div>
       </div>
       </div>

       <style type="text/css">
           .introbox {
            position:absolute;
            z-index:2000;
            }
            .introcontent {
            background-color:#fff;
            border: 1px solid;
            border-color:rgba(0, 0, 0, 0.45) rgba(0, 0, 0, 0.45)  #666666;
            box-shadow: 0 3px 8px rgba(0, 0, 0, 0.3);
            -moz-box-shadow:  0 3px 8px rgba(0, 0, 0, 0.3); /* Firefox */
            -webkit-box-shadow:  0 3px 8px rgba(0, 0, 0, 0.3); /* Safari, Chrome */
            position: relative;
            width:350px;
            }
            .introarrow {
            position: absolute;
            background-image: url("../../content/icons/modalarrows.png");
            background-repeat: no-repeat;
            width: 18px;
            height: 9px;
            z-index:200;
            }


            .introtitle {
             padding:10px 10px 0;
             font: helvetica nue, helvetica, sans;
             font-weight: bold;
             font-size:16px;
             color:#000;
            }
            
            .introtext {
             padding:5px 10px 10px 10px;
             font: helvetica nue, helvetica, sans;
             font-weight: normal;
             color:#777;
             font-size:14px;
            }
            
            .introbutton {
             padding:5px;
             background-color:#f2f2f2;
             border-top:1px solid #ccc;
             position:relative;
             text-align:right;
            }
            
            
            .introtop{
            padding-top: 9px;
            }
            .introtop .introarrow{
            top: 1px;
            background-position: -50px 0px;
            }
            
            .introright .introarrow{
            right: 10px;
            }
            
            .introleft .introarrow{
            left: 10px;
            }
            
            .introbottom{
            padding-bottom: 9px;
            }
            .introbottom .introarrow{
            bottom: 1px;
            background-position: -16px 0;
            }
            
       </style>

       <script type="text/javascript">
           $(document).ready(function () {
               doposition('.intro1', '#pageLogo', 12, 32);
               doposition('.intro2', '#pageLogo', 12, 32);
               doposition('.intro3', '#feedselector', 12, 32);
               doposition('.intro4', '#feedselector', 30, 92);
               doposition('.intro5', '#addasonglink', -301, 25);
               doposition('.intro6', '#invitefriendslink', -297, 25);
           });

           function doposition(modal, target, offsetx, offsety) {
               var pos = $(''+target).position();
               $(''+modal).css({
                   top: pos.top + offsety + "px",
                   left: pos.left + offsetx + "px"
               });
           }

           $('.intro1 .introbutton .bluebutton').bind('click', function () { $('.intro1').fadeOut('slow'); $('.intro2').fadeIn('slow'); });
           $('.intro2 .introbutton .bluebutton').bind('click', function () { $('.intro2').fadeOut('slow'); $('.intro3').fadeIn('slow'); });
           $('.intro3 .bluebutton').bind('click', function () { $('.intro3').fadeOut('slow'); $('.intro4').fadeIn('slow'); });
           $('.intro4 .bluebutton').bind('click', function () { $('.intro4').fadeOut('slow'); $('.intro5').fadeIn('slow'); });
           $('.intro5 .bluebutton').bind('click', function () { $('.intro5').fadeOut('slow'); $('.intro6').fadeIn('slow'); });
           $('.intro6 .bluebutton').bind('click', function () { $('.intro6').fadeOut('slow'); }); //finish
       </script>