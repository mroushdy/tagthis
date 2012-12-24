<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%TagThis.Controllers.OmniResults results = (TagThis.Controllers.OmniResults)ViewData["results"]; %>
<div id="divResults">
 <%ViewData["results"] = ViewData["results"]; %><% Html.RenderPartial("Genres"); %>



       <div id="resultcontent">
       

       <% Html.RenderPartial("UserProfileInfo"); %>
       
       <% ViewData["Results"] = results; %>
       <% Html.RenderPartial("ResultsPage"); %>
        </div>

       
</div>

<div id="NewPost"></div> 
              

<script type="text/javascript">
    $(function () {
        DelayedMasonry();
    });
</script>