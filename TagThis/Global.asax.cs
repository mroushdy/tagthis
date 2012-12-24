using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TagThis
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
            "post",                                              // Route name
            "Post/{id}",                           // URL with parameters
            new { controller = "Post", action = "Index" }  // Parameter defaults
            );

             routes.MapRoute(
                "comments",                                              // Route name
                "Comments/{id}",                           // URL with parameters
                new { controller = "Comments", action = "Index"}  // Parameter defaults
            );

                                     routes.MapRoute(
                "Subscribe",                                              // Route name
                "Users/{username}/Subscribe",                           // URL with parameters
                new { controller = "Users", action = "Subscribe"}  // Parameter defaults
            );

                                     routes.MapRoute(
            "UnSubscribe",                                              // Route name
            "Users/{username}/UnSubscribe",                           // URL with parameters
            new { controller = "Users", action = "UnSubscribe" }  // Parameter defaults
            );

                                     routes.MapRoute(
                "User",                                              // Route name
                "Users/{username}",                           // URL with parameters
                new { controller = "Users", action = "User"}  // Parameter defaults
            );

            routes.MapRoute(
                "Users",                                              // Route name
                "Users/{username}/{page}",                           // URL with parameters
                new { controller = "Users", action = "User", page = UrlParameter.Optional }  // Parameter defaults
            );


                routes.MapRoute(
                "commentsRatingAjax",                                              // Route name
                "Comments/Rate/{id}/{value}",                           // URL with parameters
                new { controller = "Comments", action = "Rate"}  // Parameter defaults
            );

                routes.MapRoute(
        "SearchAll",                                              // Route name
        "Search",                           // URL with parameters
        new { controller = "Search", action = "Results" }  // Parameter defaults
    );


                routes.MapRoute(
        "SearchBox",                                              // Route name
        "Search/Find",                           // URL with parameters
        new { controller = "Search", action = "Find" }  // Parameter defaults
    );
            
            routes.MapRoute(
                "SearchResults",                                              // Route name
                "Search/{query}",                           // URL with parameters
                new { controller = "Search", action = "Search"}  // Parameter defaults
            );
            
            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }  // Parameter defaults
            );



        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}