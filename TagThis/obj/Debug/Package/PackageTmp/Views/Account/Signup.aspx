<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Signup
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div style="width:430px; margin-left:auto; margin-right:auto; margin-top:100px; margin-bottom:100px;">

<div style="height: 27px;margin-bottom: 10px;">
<h1 style="font-size: 1.4em; font-weight:bold; border-bottom: 1px solid rgb(204, 204, 204); margin: 0px; padding-bottom: 3px;">Signup</h1>
</div>

<h2 style="font-size: 1.2em; margin: 0px 0px 5px; margin-left: -1.15em;">1. Find Your Friends</h2>
<div style="width: 320px; padding:5px 0 10px 0;">
<div id="connectbuttons">
  <% Html.RenderPartial("FacebookInit"); %>
  <fb:login-button perms="email,publish_stream,read_stream,offline_access" >&nbsp; Connect With Facebook &nbsp;</fb:login-button>
</div>

<div style="padding:10px 0 5px 0;">
Connecting with facebook helps us find kickass content that is relevant to you. We'll never post without your permission.
</div>

<%if(ViewData["invitetype"].ToString() != "facebook") {%>
<div>
<%=Html.ActionLink("I don't have a Facebook account.", "Register", new { id = ViewData["Iid"].ToString() })%>
</div>
<%} %>

</div>
</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="LeftContent" runat="server">
</asp:Content>
