<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Bookmarklet
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>This page will introduce you to tagging with TAGTHIS.</h2>
    <br />
        <center>
    <img style="text-align:Center;" src="../../content/images/bookmarklet.png" />
    <br />
    <br />
        </center>
        <br />
        <p></p>
        <table border="0" width="100%" cellspacing="0" cellpadding="0">
  <tr>
    <td> 
    <div style="width:560px; text-align:left;">
    <strong style="color:Black;">Make the most out of TagThis!</strong><br /><br />
    Drag the TagThis link below to your browser's bookmarks toolbar to quickly Tag, and Rate websites.
    <br />
    <br />
    </div>
    <div class="dragme" style="width:460px; text-align:left;">
<a href="javascript:(function(){f='http://www.tagthis.com/lightbox?u='+encodeURIComponent(window.location.href);self.name='owo';a=function(){if(!window.open(f,'tagthisv01','location=yes,links=no,scrollbars=no,toolbar=no,width=550,height=550'))location.href=f};if(/Firefox/.test(navigator.userAgent)){setTimeout(a,0)}else{a()}})()">TagThis</a>
</div>
    <div style="width:560px; text-align:left;">
    <strong style="color:Black;">How to use it:</strong><br /><br />
    Once you find an intresting page, just click on the bookmarklet and a new TAGTHIS popup window containing that page's information will open. Once finished, close the TAGTHIS window and continue surfing.
    </div><br />
    <div style="width:560px; text-align:left;">
    <strong style="color:Black;">Another way to tag!</strong><br /><br />
    If you are not using your default browser or the bookmarklet is not compatible with your browser. A fast an easy way to tag is to visit:<br /> <a href="http://www.tagthis.com/add/">www.tagthis.com/add/</a>
    </div>
</td>
<td>
<strong style="color:Black;">Compatibillity</strong>
<br />
<br />
TagThis bookmarklet is compatible with:
<ul>
<li>Firefox</li>
<li>Safari</li>
<li>Chrome</li>
<li>Internet Explorer 7<ul style="list-style:decimal;"><li style="list-style:none;">IE7 Instructions:</li><li>Right-click on the link</li><li>Select "Add to Favourites"</li><li>Click "Yes" on the security warning</li><li>Select "Favourites Bar" from the "creating in" dropdown </li><li>Click "Add" button</li></ul></li>
</ul>
<span class="error">note: TAGTHIS bookmarklet is not compatible with IE6.</span>
</td>
</tr>
</table>
<br />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
