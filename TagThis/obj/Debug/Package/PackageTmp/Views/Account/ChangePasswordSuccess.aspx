<%@Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Change Password
</asp:Content>

<asp:Content ID="changePasswordSuccessContent" ContentPlaceHolderID="MainContent" runat="server">
           <span id="resultskey">
       Change Password
       </span>
       <span id="keylinks">
       <%=Html.Encode("|")%>&nbsp;<%=Html.ActionLink("Change Email","ChangeEmail","Account") %>
       </span>
    <p>
        Your password has been changed successfully. <%if (!Request.IsAuthenticated) {%>
        Click <%= Html.ActionLink("here", "LogOn", "Account") %> to sign in.<%} %>
    </p>
</asp:Content>
