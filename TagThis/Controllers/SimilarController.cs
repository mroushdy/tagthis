using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using TagThis.Models;

namespace TagThis.Controllers
{
    public class SimilarController : Controller
    {
        //
        // GET: /Similar/

        UserRepository ur = new UserRepository();
        TagRepository tr = new TagRepository();
        SearchRepository sr = new SearchRepository();
        Result page;
        List<Result> relevant;
        [Authorize]
        public ActionResult Index(int? pageid)
        {
            if (pageid.HasValue)
            {
                int pid = pageid.Value;
                page = sr.Search(pid);
                relevant = sr.FindRelavantPages(page).Take(10).ToList<Result>();
                ViewData["RelevantPages"] = relevant;
                ViewData["page"] = page;
                ViewData["Pageid"] = pageid;
            }
            else { ViewData["page"] = null; ViewData["RelevantPages"] = null; }
            return View("Index");
        }

    }
}
