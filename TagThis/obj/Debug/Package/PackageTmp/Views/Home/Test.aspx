<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Test</title>
</head>
<body>
    <div>

    <h2>Login using Facebook</h2>


<% Html.RenderPartial("FacebookInit"); %>


    <fb:login-button perms="email,user_birthday,publish_stream,offline_access"></fb:login-button>
    </div>
</body>
</html>
