<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	TagThis
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <%ViewData["ResultSingle"] = ViewData["page"]; Html.RenderPartial("ResultSingle"); %>
      <br />
      <br />
      <span id="resultskey">
       Similar Pages
       </span>
       <br />
       <br />
       <% foreach (TagThis.Models.Result result in (ViewData["RelevantPages"] as List<TagThis.Models.Result>))
       { %>

                <%ViewData["ResultSingle"] = result; Html.RenderPartial("ResultSingle"); %>
    <% } %>
           </asp:Content>

