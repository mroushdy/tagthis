<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Integrate TagThis
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Integrate TagThis</h2>
    <p>The TagThis tag-strip is for website and blog publishers that want to add tagging functionallity.</p>
    The TagThis strip is smart enough to:
    <ul>
    <li>Encourage users to organize your content and submit all tags to TagThis.</li>
    <li>Find TagThis users that are intrested in simillar content, and suggest your content to them.</li>
    <li>Show the most popular tags submitted on your page and on TagThis.</li>
    <li>Automagically gets updated to the newest TagThis strip version.</li>
    </ul>
    <h3 style="color: #E5001C;"><strong>Example:</strong></h3>
    <script src="http://www.tagthis.com/js?u=last.fm" type="text/javascript"></script><br /><br />
<div class="instructions">
<h3 style="color: #E5001C;"><strong>Example:</strong> Simple usage for a web page.</h3>
<p>Just drop the following code onto the page. It's that easy.</p>
<div class="integrate-url"><%=Html.Encode("<script src=" + '"' + "http://www.tagthis.com/js" + '"' + " type=" + '"' + "text/javascript" + '"' + "></script>")%> </div>
</div>

<div class="instructions">
<h3 style="color: #E5001C;"><strong>Example:</strong> Specify the number of tags you wish to be shown.</h3>
<p>What if you want to specify a certian number of tags to be shown on your page? No problem, Just change the code like this:</p>
<div class="integrate-url"><%=Html.Encode("<script src=" + '"' + "http://www.tagthis.com/js?n=NumberOfTags" + '"' + " type=" + '"' + "text/javascript" + '"' + "></script>")%> </div>
<p>Replace NumberOfTags with the number you want to show. The default is 5.</p>
</div>

<div class="instructions">
<h3 style="color: #E5001C;"><strong>Example:</strong> When the URL you wish tagging differs from the current webpage.</h3>
<p>What if you'd like to use a specific URL from a different webpage? No problem, just change the code like this:</p>
<div class="integrate-url"><%=Html.Encode("<script src=" + '"' + "http://www.tagthis.com/js?u=YOURSITE.COM&n=NumberOfTags" + '"' + " type=" + '"' + "text/javascript" + '"' + "></script>")%> </div>
<p>Replace YOURSITE.COM, with  the URL that you want to use. Don't worry about includding "Http://www.".</p>
</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
