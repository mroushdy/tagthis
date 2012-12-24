<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Forgot
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Forgot your password?</h2>
    <p>
    TagThis will email you a link to reset your password.
    </p>
        <% using (Html.BeginForm("Forgot", "Account"))
           { %>
                <p>
                    <label for="email">E-mail:</label>
                    <%= Html.TextBox("Email")%>
                    <%if (ViewData["ForgotSuccess"] != null)
                      {%><%=Html.Encode(ViewData["ForgotSuccess"].ToString())%> <%}
                      else
                      { %>
                    <input type="Submit" value="Send" /><br /> <%} %>                   <%= Html.ValidationMessage("email")%>
                </p>
    <% } %>


      <form>
    <input type="button" value="Cook" onclick="postCook()" />
  </form>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
 <script type="text/javascript">
     function postCook() {

   FB.init({
      appId  : 246834058684710, // the app ID you get when you register your app in http://www.facebook.com/developers/
      status : true, // check login status
      cookie : true, // enable cookies to allow the server to access the session
      xfbml  : true  // parse XFBML
    });

         FB.api(
        '/me/sixtysongs:post?song=http://www.sixtysongs.com/Post/2&access_token=AAADgfohi6SYBAFXpEn9FY3yRcpooMa1tTOVgTWKCZCsZBvqZAOor4lkcXcyJcomHQ3BQWtvmQMv53KZBHLDIqZCzyAsVmfSEZD',
        'post',
        function (response) {
            if (!response || response.error) {
				console.log(response);
                alert(response);
            } else {
                alert('Cook was successful! Action ID: ' + response.id);
            }
        });
     }
  </script>
</asp:Content>
