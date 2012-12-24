using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TagThis.Controllers
{
    public class SocialFrameController : Controller
    {
        //
        // GET: /SocialFrame/

        public ActionResult Index(string url)
        {
            ViewData["shareurl"] = url;
            return View();
        }

    }
}
