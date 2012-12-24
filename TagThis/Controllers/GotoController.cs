using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TagThis.Models;

namespace TagThis.Controllers
{
    public class GotoController : Controller
    {
        //
        // GET: /Goto/

        public ActionResult Index(int id)
        {
            TagRepository tr = new TagRepository();
            WebPage page = tr.GetPage(id);
            tr.AddPageClick(id);
            return RedirectPermanent(page.url);
        }

    }
}
