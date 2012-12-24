using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Dynamic;
using Facebook.Web;
using Microsoft.CSharp;
using TagThis.Models;
using TagThis.ActionFilters;

namespace TagThis.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {

        [AjaxViewSelector]
        public ActionResult Index()
        {
                SearchController search = new SearchController();
                OmniResults R;
                if (Membership.GetUser() != null)
                {
                    R = search.GetViewContent("", 0, "popular", "now", "following");
                }
                else
                {
                    R = search.GetViewContent("", 0, "popular", "now", "everyone");
                }
                ViewData["results"] = R;
                ViewData["HeadContent"] = "<meta property='og:site_name' content='SixtySongs' /> <meta content='website' property='og:type'> <meta content='http://sixtysongs.com' property='og:url'> <meta property='og:title' content='SixtySongs - Discover Music Together' /> <meta property='og:image' content='http://www.sixtysongs.com/content/images/fbtl.png' /> <meta property='og:description' content='SixtySongs is the place to discover, share, and appreciate music with interesting people.' /> <meta property='og:url' content='" + Request.Url + "'>";
                return PartialView("ResultsPartial");
   
        }


        [Authorize,AjaxViewSelector]
        public ActionResult ft()
        {
            SearchController search = new SearchController();
            OmniResults R = search.GetViewContent("", 0, "popular", "now", "following");
            ViewData["results"] = R;
            return PartialView("FirstTime");

        }

        public ActionResult Test()
        {
           
            if (FacebookWebContext.Current.IsAuthenticated())
            {
                return RedirectToAction("Facebook");
            }
            return View();
        }

        public ActionResult Facebook()
        {
            var client = new FacebookWebClient();

            string accesstoken = client.AccessToken;

            dynamic me = client.Get("me");

            ViewBag.name = me.name;

            ViewBag.id = me.id;

            ViewBag.gender = me.gender;

            ViewBag.birthday = me.birthday;

            ViewBag.email = me.email;


            //post to wall
            dynamic parameters = new ExpandoObject();
            parameters.message = "Check out this funny article";
            parameters.link = "http://www.example.com/article.html";
            parameters.picture = "http://www.example.com/article-thumbnail.jpg";
            parameters.name = "Article Title";
            parameters.caption = "Caption for the link";
            parameters.description = "Longer description of the link";
            parameters.actions = new
            {
                name = "View on Zombo",
                link = "http://www.zombo.com",
            };
            parameters.privacy = new
            {
                value = "ALL_FRIENDS",
            };

            dynamic result = client.Post("me/feed", parameters);



            return View();
        }





        public ActionResult About()
        {
            
            return View();
        }

    }
}
