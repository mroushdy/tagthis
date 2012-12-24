<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Share TagThis with friends
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("InviteMethods"); %>
        <div style="width:80%; background-color:#F1F1F1; margin:auto; ">
        <fieldset>
                <legend>Account Information </legend>
                    <% using (Html.BeginForm("gmail","Invitations")) { %>
                <p>
                <%= Html.ValidationMessage("signin") %>
                    <label for="username">Gmail Address</label>
                    <%= Html.TextBox("email", null, new { Class = "textbox" })%>
                    <%= Html.ValidationMessage("email") %>
                </p>
                <p>
                    <label for="password">Gmail Password:</label>
                    <%= Html.Password("password", null, new { Class = "textbox" }) %>
                    <%= Html.ValidationMessage("password") %>
                </p>
                <p>
                    <input type="submit" value="Get Contacts" />
                </p>
                <p>
                 TagThis won’t store your password or spam any of your friends.
                </p>
                    <% } %>
        </fieldset>
        </div>
        <p></p>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
		<script type="text/javascript" charset="utf-8">	
		$(document).ready(function(){  
		$("#gmail").addClass("selected");
		});
		</script>
</asp:Content>
