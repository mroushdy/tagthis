<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">



    <h2>Index</h2>
   <br /> <%=Html.ActionLink("check all","checkall/","Invitations") %><br />
<%foreach(TagThis.Models.Invitation i in(ViewData["invites"] as List<TagThis.Models.Invitation>)){ %>
<%=Html.Encode(i.email) %>------------<%=Html.Encode(i.ID) %> <%=Html.ActionLink("check","check/"+i.ID,"Invitations") %> <%=Html.ActionLink("delete","delete/"+i.ID,"Invitations") %><br />
<%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
  <script type="text/javascript">var _siteRoot='index.html',_root='index.html';</script>
  <script type="text/javascript" src="../../scripts/Slidescript.js"></script>
</asp:Content>
