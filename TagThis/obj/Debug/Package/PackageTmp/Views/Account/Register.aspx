<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TagThis.HtmlHelpers" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Register
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">

<div style="width:100%; background-color:White; position:absolute;">
<div style=" background: url(../../content/images/HeaderRule.png) no-repeat scroll center 0 transparent; padding-top:5px; position:fixed; top:44px; width:100%;"></div>
<div style="width:430px; margin-left:auto; margin-right:auto; margin-top:50px; margin-bottom:50px;">

<%if (ViewData["FBid"] != null)
  {
      if (string.IsNullOrEmpty(ViewData["FBid"].ToString()) == false)
      {  %>
<img style="float:left; position:absolute; width:100px; margin-left:-140px;" src='http://graph.facebook.com/<%= ViewData["FBid"].ToString() %>/picture?type=normal' />
<%}
  } %>

<div style="height: 27px; padding-bottom: 25px;">
<h1 style="font-size: 27px; font-weight:300; border-bottom: 1px solid rgb(204, 204, 204); margin: 0px; padding-bottom: 10px; color:#222222;">Create a Sixtysongs Account</h1>
</div>


<div style="width: 450px; padding:5px 0 10px 0;">


    <% using (Html.BeginForm()) { %>

                    <label style="width:97px; float:left;">Username</label>
                    <%= Html.TextBox("userName", null, new { Class = "textbox", style = "width:150px;" })%>
                    <br />
                    <%= Html.ValidationMessage("userName", new { style = "display:block; margin-bottom:5px;" })%>
                    <br />
                    <label style="width:97px; float:left;">Full Name</label>
                    <%= Html.TextBox("Name", null , new { Class = "textbox", style = "width:150px;" })%>
                    <br />
                    <%= Html.ValidationMessage("Name", new { style = "display:block; margin-bottom:5px;" })%>
                    <br />
                    <label style="width:97px; float:left;">Email</label>
                    <%=Html.Encode(ViewData["email"])%>
                    <br />
                   <%= Html.ValidationMessage("email", new { style = "display:block; margin-bottom:5px;" })%>
                    <br />
                    <label style="width:97px; float:left;">Password</label>
                    <%= Html.Password("password", null, new { Class = "textbox", style = "width:150px;" })%>
                    <br />
                    <%= Html.ValidationMessage("password", new { style = "display:block; margin-bottom:5px;" })%>
                    <br />
                    <label style="width:97px; float:left;">Confirm</label>
                    <%= Html.Password("confirmPassword", null, new { Class = "textbox", style = "width:150px;" })%>
                    <br />
                    <%= Html.ValidationMessage("confirmPassword", new { style = "display:block; margin-bottom:5px;" })%>
                    <br />

                    <div style="display:none; padding-bottom:10px;">
                    <%if (ViewData["FBid"] != null)
                    {
                    if (string.IsNullOrEmpty(ViewData["FBid"].ToString()) == false)
                    {  %>
                    <%=Html.CheckBox("FBautopost", false) %> Autopost things I share to my facebook wall.
                    <div style="font-size: 0.8em;">note: You can always disable this later.</div> 
                    <%}
                    } %>
                    </div>

                    <%=Html.Hidden("email",null)%>
                    <%=Html.Hidden("Iid",ViewData["Iid"].ToString())%>
                    <%=Html.Hidden("FBid", null)%>
                    <%=Html.Hidden("FBtoken", null)%>

                    <input id="bka" type="submit" value="Register" style="display:none;" />
                    <a class="bluebutton" onclick="$('#bka').click();" style="padding-top:5px;">Create Account</a>

 <p style="font-size: 0.8em; width:300px; padding-top:10px;">By clicking "Create Account", you are indicating that you have read and agreed to our <a href="http://www.sixtysongs.com/help/privacy" target="_blank">privacy policy</a>.</p>
    

</div>
</div>

</div>
    <% } %>

    <script type="text/javascript">
        $(function () {
            $("#pageLogo").css("position", "relative").css("left", "-85px").css("margin", "0 50%");
            $("#logon").hide(); $("#SearchBox").hide();
        });
</script>
</asp:Content>
