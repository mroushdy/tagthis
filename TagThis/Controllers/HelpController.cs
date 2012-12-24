using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace TagThis.Controllers
{
    public class HelpController : Controller
    {
        //
        // GET: /Help/

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult Widget()
        {
            return View();
        }

        public ActionResult Press()
        {
            return View();
        }

        public ActionResult Bookmarklet()
        {
            return View();
        }

        public ActionResult AdvancedSearch()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }
    }
}
