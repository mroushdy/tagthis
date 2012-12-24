
$(window).resize(function () { resize(); });

$(".playlistitem").live("click", function () { $(".playing").removeClass("playing"); $(this).addClass("playing"); play(); });
$(".playlistitem .remove").live("click", function () { parent = $(this).parent(".playlistitem"); if (parent.hasClass("playing")) { playnext(); } parent.remove(); if ($(".playlistitem").length == 0) { ClosePlaylistWindow(); } return false; });

var PlayListElement = '<div class="playlistitem clearfix" data-vid="#datavideoid" data-pid="#pid" data-postid="#postid" data-reposturl="#reposturl" data-liketext="#liketext" data-shareurl="#shareurl"><a class="remove"></a><div class="playlist-image"><img src="http://img.youtube.com/vi/#videoid/1.jpg"/></div> <div class="playlist-info"><span class="v-title">#title</span></div></div>';
var Open = false;
var PlayerLoaded = false; //fixes an error with play the first time play is clicked and the video window is open.
var ModalOpen = false;


function OpenPlaylistWindow() { 
    if(!Open){
        $("#contentCol").css('width', '70%');
        $("#MainLoading").css('width', '70%');
        $("#logon").css('width', '70%');
		try{$("#calltoactioncontainer").css('top', '20px');} catch (err) { } // will fail if user if logged in
        masonry();
        $("#playerCol").show('slide', { direction: 'right' }, 1000);
        resize();
        Open = true;
    }
}

function ClosePlaylistWindow() {
    if (Open) {
        $("#playerCol").hide('slide', { direction: 'right' }, 1000);
        $("#contentCol").css('width', '100%');
        $("#MainLoading").css('width', '100%');
        $("#logon").css('width', '100%');
        masonry();
        resetPlayer();
        Open = false;
        PlayerLoaded = false;
        ready = false;
        player = null;
        $('#my-player').remove();
        $('#window').append('<div id="flash" style="position:absolute; z-index:-1;"></div>');
    }
}


//still needed to be done. Add a measure to protect against duplicate songs in playlist
function CreatePlayListHtml(pageid, videoID,postid) {
    //get some infor for the play links: like , repost, share.. etc
    var post = $('#r' + postid);
    var liketext = post.find('.LikeButton').text();
    var shareurl = post.find('.ShareButton').attr('url');
    var reposturl = post.find('.RepostButton').attr('data-url');
    return PlayListElement.replace("#videoid", videoID).replace("#liketext", liketext).replace("#reposturl", reposturl).replace("#shareurl", shareurl).replace("#postid", postid).replace("#datavideoid", videoID).replace("#title", $('#title' + postid).text()).replace("#pid", pageid);
}

function playall() {
    //clear playlist
    $('.playlistitem').remove();

    //play first song
    $('.play:first').trigger('click');

    //skip the first song and cue the rest
    var skipped = false;
    $('.que').each(function (idx, item) {
        if (skipped) { $(this).trigger('click'); }
        skipped = true;
    });
}

//plays the video in the div with the class .playing
function play() {
    
    var playing = $(".playing");
    var videoId = playing.attr("data-vid");


    var pid= playing.attr("data-pid");
    var title = playing.find(".v-title").text();
    

    $("#playertitle").text(title);

    var postid = playing.attr("data-postid");
	
	try{ //all the below code will fail if user is not logged in so its put in a try catch to prevent failing
    var commenthref = $("#window .CommentButton").attr("href").toString().replace("0", postid);
    $("#window .CommentButton").attr("data-pid", pid).attr("data-postid", postid).attr("href",commenthref);
    $("#window .LikeButton").text(playing.attr("data-liketext")).attr("data-pid", pid).attr("data-postid", postid);
    $("#window .RepostButton").attr("data-pid", pid).attr("data-postid", postid).attr("data-url", playing.attr("data-reposturl"));
    $("#window .ShareButton").attr("url", playing.attr("data-shareurl"));
	PostListenToFB(postid);
	} catch (err) { }

	if(PlayerLoaded){
		loadVideo(videoId);
		player.playVideo();
	}
	else
	{
	    loadplayer(videoId);
	}


}


function playnext() {

    var playing = $(".playing");
    var next = playing.next(".playlistitem");
    playing.removeClass("playing");

    if (next.length > 0) {
        next.addClass("playing");
        play();
    }
    else { 
    
        //playlist has finished! 
        
    }

}


function playprevious() {

    var playing = $(".playing");
    var previous = playing.prev(".playlistitem");
    playing.removeClass("playing");

    if (previous.length > 0) {
        previous.addClass("playing");
        play();
    }
    else {

        //playlist has finished! 

    }

}

function playvideo(videourl, pageid,postid) {

    OpenPlaylistWindow();

    var videoID = videourl.match(/v=(.{11})/)[1];
    var html = CreatePlayListHtml(pageid, videoID,postid);
    //add to playlist

    
    
    if ($("#playList").children().length == 0) { $("#playList").append(html); $(".playlistitem").addClass("playing"); }
    else {
        var element = $(".playing");
		if(element.length > 0)
		{
        element.after(html);
        element.next(".playlistitem").addClass("playing");
        element.removeClass("playing");
		} else { //finished playing
			$("#playList").append(html); $(".playlistitem:last").addClass("playing");
		}
    }

    play();
}


function Que(videourl, pageid,postid) {

    OpenPlaylistWindow();

    var videoID = videourl.match(/v=(.{11})/)[1];
    var html = CreatePlayListHtml(pageid, videoID,postid);

    if ($("#playList").children().length == 0) { $("#playList").append(html); $(".playlistitem").addClass("playing"); }
    else { $("#playList").append(html); }
}

function resize()
{
    var windowheight = $(window).height();
    var height = Math.round($('#player').width() * (3 / 4));
    $('#window').height(height);
    $('#player').height(height);
    $('#playerCol').height(windowheight);
    var playlistheight = windowheight - $("#playList").offset().top + $(window).scrollTop();
    $('#playList').height(playlistheight);
}



var player = null;
var ready = false;
var seekReady = false;
var playheadInterval = 0;
var tooltip = null;
var documentloaded = false;

$(document).ready(function () {


    resize();

    // player controls

    // play/pause button
    $('.playcontrol .playpause').click(function () {
        if (!ready) return;
        else playPause();
    });

    //playlistbin
    $('.playlistheader .bin').click(function () {
        $('.playlistitem').remove();
        ClosePlaylistWindow();
    });

    //next button
    $('.playcontrol .next').click(function () {
        playnext();
    });

    //previous button
    $('.playcontrol .previous').click(function () {
        playprevious();
    });

    // seekbar
    $('#seekbar').click(function (e) {
        if (!seekReady) return;
        else {

            var ratio = (e.pageX - $('#seekbar').offset().left) / $('#seekbar').outerWidth();

            $('#elapsed').width(ratio * 100 + '%');
            seekToPercent(ratio);

            //var localX = (e.pageX - $(this).offset().left) - 17;
            //if (localX > $(this).innerWidth()) localX = $(this).innerWidth();

            //var percent = localX / $('#seekbar').innerWidth();
            //seekToPercent(percent);
        }
    });

    // mute button
    $('.playcontrol .muteunmute').click(function () {
        if (!ready) return;
        else toggleSound();
    });


    $("#playerCol").css("width", "30%");
    $("#playerCol").css("display", "none");
});

function loadplayer(videoid)
{
        // embed chromless player
        var id = 'flash';
        var src = videoid;
        var params = { allowScriptAccess: "always", wmode: "opaque"};
        var atts = { id: "my-player" };

        swfobject.embedSWF("http://www.youtube.com/apiplayer?autoplay=1&video_id=" + src + "&amp;version=3&amp;enablejsapi=1", id, "100%", "100%", "9.0.0", "../../content/expressInstall.swf", null, params, atts);
		
		PlayerLoaded = true;
		resize();
}






function onPlayerError(errorCode) {
	ready = true;
	playnext();
}


/************************************************ 
* onYouTubePlayerReady:void                    *
*                                              *
* Called from the YouTube player API.          *
* This player is ready for action!             *
************************************************
/                                              */
onYouTubePlayerReady = function () {
    player = document.getElementById('my-player');

    player.addEventListener("onStateChange", "onPlayerStateChange");
	player.addEventListener("onError", "onPlayerError");
    ready = true;
}

/************************************************ 
* onPlayerStateChange:void                     *
*                                              *
* Handle player state change.                  *
************************************************
/                                              */
onPlayerStateChange = function (s) {
    switch (s) {
        case -1: // unstarted
            return;
        case 0: // ended
            resetPlayer();
            playnext();
            return;
        case 1: // playing
            seekReady = true;
			ready = true;
            $('.playpause').removeClass("play"); $('.playpause').addClass("pause");
            playheadInterval = setInterval(updatePlayhead, 10);
            return;
        case 2: // paused
            $('.playpause').removeClass("pause"); $('.playpause').addClass("play");
            clearInterval(playheadInterval);
            return;
        case 3: // buffering
            $('.playpause').removeClass("play"); $('.playpause').addClass("pause");
            return;
        case 5: // video cued
            resetPlayer();
            ready = true;
            return;
    }
}

/************************************************ 
* resetPlayer:void                             *
*                                              *
* Reset the player.                            *
************************************************
/                                              */
resetPlayer = function () {
    clearInterval(playheadInterval);
    $('.playpause').removeClass("pause"); $('.playpause').addClass("play");
    $('#elapsed').width(0);
}

/************************************************ 
* updatePlayhead:void                          *
*                                              *
* Update the seekbar.                          *
************************************************
/                                              */
updatePlayhead = function () {
    if (typeof (player.getCurrentTime) == 'undefined') {
        clearInterval(playheadInterval);
        return;
    }

    var percentage = player.getCurrentTime() / player.getDuration();
    $('#elapsed').width((percentage * 100) + '%');
    //$('#seekbar').css('background-position', Math.round(percentage * $('#seekbar').innerWidth()) + 'px 0px');
}

/************************************************ 
* playPause:void                               *
*                                              *
* Toggle play/pause.                           *
************************************************
/                                              */
playPause = function () {
    switch (player.getPlayerState()) {
        case -1: // unstarted
        case 0: // ended
        case 2: // paused
            player.playVideo();
            return;
        case 1: // playing
            player.pauseVideo();
            return;
        default:
            return;
    }
}

/************************************************ 
* seekToPercent:void                           *
*                                              *
* Seek to the passed percentage.               *
*                                              *
* percent:Number - percent to seek to.         *
************************************************
/                                              */
seekToPercent = function (percent) {
    var time = percent * player.getDuration();
    player.seekTo(time, true);
}

/************************************************ 
* toggleSound:void                             *
*                                              *
* Toggle mute/unmute.                          *
************************************************
/                                              */
toggleSound = function () {
    if (player.isMuted()) {
        player.unMute();
        $('.muteunmute').removeClass("unmute"); $('.muteunmute').addClass("mute");
    } else {
        player.mute();
        $('.muteunmute').removeClass("mute"); $('.muteunmute').addClass("unmute"); 
    }
}

//////////// Video Gallery (Optional) ///////////

/************************************************ 
* loadVideo:void                               *
*                                              *
* Load a new video into the player.            *
*                                              *
* id:String - video id to load.                *
************************************************
/                                              */
loadVideo = function (id) {
	if (!ready)
	    return false;
	
	    player.cueVideoById(id, 0, 'default');
		ready = false;
}

/////////////////////////////////////////////////

