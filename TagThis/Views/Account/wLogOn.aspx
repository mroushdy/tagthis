<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SixtySongs Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    
<div style="width:100%; background-color:White; position:absolute;">
<div style=" background: url(../../content/images/HeaderRule.png) no-repeat scroll center 0 transparent; padding-top:5px; position:fixed; top:44px; width:100%;"></div>
<div style="width:430px; margin-left:auto; margin-right:auto; margin-top:50px; margin-bottom:50px;">


<div style="height: 27px; padding-bottom: 25px;">
<h1 style="font-size: 27px; font-weight:300;  margin: 0px; padding-bottom: 10px; color:#222222;">Create a Sixtysongs Account</h1>
</div>


<div style="width: 300px; padding:30px 0 10px 0; border-top: 1px solid rgb(204, 204, 204);" >


    <% using (Html.BeginForm()) { %>

                    <label style="width:97px; float:left;">Username or E-mail:</label>

					<div class="Wrap">
					<div class="outerWrap">
					<div class="innerWrap">
					<%= Html.TextBox("username", null, new { Class = "textbox" })%>
					</div>
					</div>
					</div>
                                
                    <%= Html.ValidationMessage("username") %>
                    <br />

                    <label style="width:97px; float:left;">Password</label>

					<div class="Wrap">
					<div class="outerWrap">
					<div class="innerWrap">
					<input type="password" value="" class="textBox" id="password" name="password"/>
					</div>
					</div>
					</div>

                    <%= Html.ValidationMessage("password", new { style = "display:block; margin-bottom:5px;" })%>
                    <br />

                    <p>
                    <%= Html.CheckBox("rememberMe") %> <label class="inline" for="rememberMe">Remember me?</label>
                    </p>

                    <input id="bka" type="submit" value="Login" style="display:none;" />
                    <a class="bluebutton" onclick="$('#bka').click();" style="padding-top:5px;">Login</a>


</div>
</div>

</div>
    <% } %>

    <script type="text/javascript">
        $(function () {
            $("#pageLogo").css("position", "relative").css("left", "-85px").css("margin", "0 50%");
            $("#logon").hide(); $("#SearchBox").hide();
            $("html").css("background-color", "#ffffff");
        });
</script>
    
    
    
    <h2>Log On</h2>
    <p>
        Please enter your username and password.
    </p>
    <%= Html.ValidationSummary("Login was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <legend>Account Information</legend>
                <p>
                    <label for="username">Username or E-mail:</label>
  
                </p>
                <p>
                    <label for="password">Password:</label>
                   
                    <%= Html.ValidationMessage("password") %>
                </p>
         
                <p>
                    <input type="submit" value="Log On" />
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
		<script type="text/javascript" charset="utf-8">	
			</script>
</asp:Content>
