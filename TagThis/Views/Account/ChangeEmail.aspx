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
        Use the form below to change your email. 
    </p>
    <%= Html.ValidationSummary("Email change was unsuccessful. Please correct the errors and try again.")%>

    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <legend>Account Information</legend>
                <p>
                    <label for="Email">New email:</label>
                    <%= Html.TextBox("Email") %> <input type="submit" value="Save" />
                    <br />
                    <%= Html.ValidationMessage("email") %>
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
