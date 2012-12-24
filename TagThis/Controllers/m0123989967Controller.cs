using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace TagThis.Controllers
{
    public class m0123989967Controller : Controller
    {
        //
        // GET: /m0123989967/
        private TagThis.Models.TagThisDataContext db = new TagThis.Models.TagThisDataContext();
        public ActionResult Index()
        {
            var results = (from i in db.Invitations select i).OrderByDescending(m =>m.date).ToList();
            ViewData["invites"] = results;
            return View();
        }

    }
}
