using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TagThis.ActionFilters;

namespace TagThis.Controllers
{
    public class PostController : Controller
    {
        //
        // GET: /Post/
        [AjaxViewSelector]
        public ActionResult Index(int id)
        {
            TagThis.Models.SearchRepository sr = new TagThis.Models.SearchRepository();
            var post = sr.SearchGetPost(id);
            ViewData["ResultSingle"] = post;
            ViewData["HeadContent"] = "<meta property='og:type' content='sixtysongs:song' /> <meta property='og:title' content='"+ post.page.name +"' /> <meta property='og:image' content='"+  post.page.thumburl +"' /> <meta property='og:description' content='"+ post.pageOwner.OwnerPost +"' /> <meta property='og:url' content='" + Request.Url + "'>";
            return PartialView("Post");
        }

    }
}
