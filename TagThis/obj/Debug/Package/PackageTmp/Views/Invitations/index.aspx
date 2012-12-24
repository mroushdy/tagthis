<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
Share TagThis with friends
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("InviteMethods"); %>
    <br />
        <div style="width:80%; background-color:#F1F1F1; margin:auto; margin-top:-12px;">
        <fieldset>
                <legend></legend>
                    <% using (Html.BeginForm("index","Invitations")) { %>
                <p>
                    <label for="username">Invite folks by sending them an email. Enter their emails below.</label><br />
                    <%= Html.TextBox("emails", null, new { style="width:400px;" })%>
                    <%= Html.ValidationMessage("emls") + "<br/>" %>
                    Separate multiple email addresses with commas, ex: joe@tagthis.com,jane@tagthis.com
                </p>
                <p>
                    <input type="submit" value="Invite" />
                </p>
                <p>
                 TagThis won’t spam any of your friends.
                </p>
                    <% } %>
        </fieldset>
        </div>
        <p></p>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
		<script type="text/javascript" charset="utf-8">	
		$(document).ready(function(){  
		$("#index").addClass("selected");
		 var emls = new TextboxList('#emails', {unique: true,bitsOptions:{editable:{addKeys:188, addOnBlur:true}}});
		});
		</script>
				<style type="text/css" media="screen">
			
			/* Setting widget width example */
			#emails { width: 400px; }
		</style>
</asp:Content>
