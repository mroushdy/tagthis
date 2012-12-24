var clicked="Sl1";
      
$(document).ready(function(){  
  
           
    $("ul.subnav").parent().append("<span></span>"); //Only shows drop down trigger when js is enabled (Adds empty span tag after ul.subnav*)  
  
    $("ul.topnav li span").click(function() { //When trigger is clicked...  
  
        //Following events are applied to the subnav itself (moving subnav up and down)  
        $(this).parent().find("ul.subnav").slideDown('fast').show(); //Drop down the subnav on click  
  
        $(this).parent().hover(function() {  
        }, function(){  
            $(this).parent().find("ul.subnav").slideUp('fast'); //When the mouse hovers out of the subnav, move it back up  
        });  
  
        //Following events are applied to the trigger (Hover events for the trigger)  
        }).hover(function() {  
            $(this).addClass("subhover"); //On hover over, add class "subhover"  
        }, function(){  //On Hover Out  
            $(this).removeClass("subhover"); //On hover out, remove class "subhover"    	
    }); 
    
    $("html ul.topnav li ul.subnav li a ").hover(function(){
                $(this).css('background','#222 url(../../content/images/dropdown/dropdown_linkbgHover.gif) no-repeat 10px center'); //On hover over, add class "subhover"  
        }, function(){  //On Hover Out  
           if($(this).attr('id')!=clicked){ $(this).css('background','#333 url(../../content/images/dropdown/dropdown_linkbg.gif) no-repeat 10px center');}
    }); 
  
});

function clickedSl(args)
{
$('#'+clicked).css('background','#333 url(../../content/images/dropdown/dropdown_linkbg.gif) no-repeat 10px center');
$('#Sl'+args).css('background','#222 url(../../content/images/dropdown/dropdown_linkbgHover.gif) no-repeat 10px center');
if(args==5 & clicked !='Sl5')
{
$("#SimpleButton").toggle('slow');
$("#simple").css('display','none');
$("#advanced").css('display','block');
document.getElementById('SorA').value="a";
document.getElementById('SearchWhat').value="all"; 
}
if(args==6)
{
$("#SimpleButton").toggle('slow');
$("#simple").css('display','block');
$("#advanced").css('display','none');
document.getElementById('SorA').value="s";
document.getElementById('SearchWhat').value="all"; 
}
if(args==1){document.getElementById('SearchWhat').value="all"; document.getElementById("SearchText").innerHTML="Find Anything:";}
if(args==2){document.getElementById('SearchWhat').value="faves"; document.getElementById("SearchText").innerHTML="Find Favourites:"; }
if(args==3){document.getElementById('SearchWhat').value="loves"; document.getElementById("SearchText").innerHTML="Find Loves:";}
if(args==4){document.getElementById('SearchWhat').value="hates"; document.getElementById("SearchText").innerHTML="Find Hates:";}

clicked = 'Sl'+args;
}

function toggleLayer( whichLayer )
{
$('#'+whichLayer).toggle('slow');
}

function setclickedblue(args){$('#'+args).css('background','#48FFFE'); return null;}
function setclickedyellow(args){$('#'+args).css('background','#FFFF00'); return null;}