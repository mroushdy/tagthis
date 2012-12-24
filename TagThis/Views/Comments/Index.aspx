<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	<script src="../../Scripts/MicrosoftAjax.js" type="text/javascript"></script>
	<script src="../../Scripts/MicrosoftMvcAjax.js" type="text/javascript"></script>
	<script src="../../Scripts/jquery-1.3.2.min.js"type="text/javascript"></script>
	
	<script type="text/javascript">
    $(document).ready(function(){ setclickedyellow('a<%=Html.Encode(ViewData["Clicked"])%>'); });
    function begin(args) {
    
        // Animate
        $('#divform').fadeOut('normal');
        $('#divcomment').fadeOut('normal');
    }

    function success() {
        // Animate
        $('#divcomment').fadeIn('normal');
    }

    function failure() {
        alert("Could not retrieve comments.");
    }

</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <%ViewData["ResultSingle"] = ViewData["page"]; Html.RenderPartial("ResultSingle"); %>
      <span id="resultskey">
       Similar Pages
       </span>
       <% foreach (TagThis.Models.Result result in (ViewData["RelevantPages"] as List<TagThis.Models.Result>))
       { %>

                <%ViewData["ResultSingle"] = result; Html.RenderPartial("ResultSingle"); %>
    <% } %>
           <span id="resultskey">
       Comments
       </span>
       <br /><br />
           <span id="sorting" style=" margin-left:20px; width:100%;">
       Sort:
       &nbsp;<%=Html.ActionLink("Newest", ViewData["Pageid"].ToString(), "Comments", new { sort = "newest" }, new { id = "anewest", Class = "link" })%>
       &nbsp;<%=Html.ActionLink("Oldest", ViewData["Pageid"].ToString(), "Comments", new { sort = "oldest" }, new { id = "aoldest", Class = "link" })%>
       &nbsp;<%=Html.ActionLink("Highest Rating", ViewData["Pageid"].ToString(), "Comments", new { sort = "loved" }, new { id = "aloved", Class = "link" })%>
       &nbsp;<%=Html.ActionLink("Lowest Rating", ViewData["Pageid"].ToString(), "Comments", new { sort = "hated" }, new { id = "ahated", Class = "link" })%>
       </span>
       <br />
    <% foreach (TagThis.Models.CommentResult r in (ViewData["comments"] as List<TagThis.Models.CommentResult>))
       { %>

                <%ViewData["comment"] = r; Html.RenderPartial("comment"); %>
    <% } %>

    <div id="divcomment"></div>
    <% if (Request.IsAuthenticated)
   { %>
    <% using (Ajax.BeginForm(ViewData["Pageid"].ToString(), new AjaxOptions() { UpdateTargetId = "divcomment", OnBegin = "begin", OnSuccess = "success", OnFailure = "failure",LoadingElementId="loading" }))
       {%>
    <div id="divform" style="width:400px">
             Your comment: <br />
             <%= Html.TextArea("Text", new { style="width:400px; height:100px;"})%>
                
                <br />
     <input type="submit" value="Save" />
         
    <% } %>
    <% } else{%>   
    Log in to leave a comment
    <% } %>
    </div>
<img id="loading" src="../../Content/icons/loading29.gif" class="Commentloading"/>
</asp:Content>

