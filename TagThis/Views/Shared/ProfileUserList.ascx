<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

       <div id="result" style="margin:20px auto; width:860px;">
       <div style="float:left; width:230px;"><% Html.RenderPartial("UserProfileInfo"); %></div>
       <div style="margin-left:260px;"><% Html.RenderPartial("UserList"); %></div>
        </div>