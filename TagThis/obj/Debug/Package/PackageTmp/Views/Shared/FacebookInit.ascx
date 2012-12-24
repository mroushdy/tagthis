<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

      <div id="fb-root"></div>
      <script src="http://connect.facebook.net/en_US/all.js"></script>
      <script>
      $(document).ready(function () {
          FB.init({
        appId: '<%= Facebook.FacebookApplication.Current.AppId%>', status: true, cookie: true, xfbml: true
    });
    FB.Event.subscribe('auth.login', function (response) {
        window.location.reload();
    });

});
</script>