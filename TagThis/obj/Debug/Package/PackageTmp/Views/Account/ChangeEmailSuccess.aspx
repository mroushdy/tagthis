<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Change Email
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <span id="resultskey">
       Change Email
       </span>
       <span id="keylinks">
       <%=Html.Encode("|")%>&nbsp;<%=Html.ActionLink("Change Password","ChangePassword","Account") %>
       </span>
    <p>
        Your email has been changed successfully. 
    </p>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
