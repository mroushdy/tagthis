using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using TagThis.Models;

namespace TagThis.Controllers
{
    public class CommentsController : Controller
    {
        //
        // GET: /Comments/

        UserRepository ur = new UserRepository();
        TagRepository tr = new TagRepository();
        SearchRepository sr = new SearchRepository();
        Result page;
        List<Result> relevant;
        
        //receive comment id and returns the post it is within
        // future expansion to highlight that comment or show it if there are way too many comments
        public ActionResult Index(int id)
        {
            return RedirectToAction("", "Post", new { id = ur.GetComment(id).postid });
        }


        //underneath adds a new comment using ajax
        [Authorize,AcceptVerbs(HttpVerbs.Post)]
        public ActionResult index(int? pageid, FormCollection formValues, int? postid)
        {
            if (Request.IsAjaxRequest() == true & string.IsNullOrEmpty(formValues["Text"].ToString()) == false & pageid.HasValue == true & postid.HasValue == true)
            {
                CommentResult CR = new CommentResult();
                Comment c = new Comment();
                c.text = formValues["Text"].ToString();
                CR.comment = ur.AddComment(c, pageid.Value, postid.Value);
                CR.rating = 0;
                ur.Save();
                ViewData["comment"] = CR;
                return PartialView("comment");
            }
            else 
            {
                return null;
            }

          
        }



    }
}
