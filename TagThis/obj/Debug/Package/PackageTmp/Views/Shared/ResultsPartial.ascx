<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%TagThis.Controllers.OmniResults results = (TagThis.Controllers.OmniResults)ViewData["results"]; %>

<div id="divResults">
 <%ViewData["results"] = ViewData["results"]; %><% Html.RenderPartial("Genres"); %>      


       <div id="resultcontent">
       <% ViewData["Results"] = results; %>
       <% Html.RenderPartial("ResultsPage"); %>
        </div>

       
</div>

<script type="text/javascript">
    $(function () {
        DelayedMasonry();

        clearsearchbox();
        //adds the search terms back in the search box- not working
        <%if (ViewData["s-terms"] != null) {
        foreach (string t in ViewData["s-terms"] as List<string>)
        {
        if(!t.StartsWith("!")) //queries starting with ! like !user or !all are not meant to be shown
        {
        %>
            addsearchterm('<%= t %>');
        <%
        }
        }
        }%>
       
    });


</script>

