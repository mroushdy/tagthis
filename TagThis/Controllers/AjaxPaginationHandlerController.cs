using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TagThis.Controllers
{
    //Easy Ajax Pagination. How To:
    //Step 1 create a new PaginationSettings object in view and pass all the right data
    //Step 2 Pass this object as a ViewData["Settings"]
    //Step 3 create the appropriate get next function here. note: most of the function will not change
    //Step 4 create a new partial view for each single pagination item. Within this partial view the apropriate item will be inside the ViewData["item"]
    public class PaginationSettings
    {
        public string GetMoreAction { get; set; }
        public string GetMoreLinktext { get; set; }
        public string ItemPartialViewName { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public IQueryable<dynamic> Iqueryable { get; set; }
        public string ParentId { get; set; }
    }

       public class PaginatedList<T> : List<T> 
    {
    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public int TotalPages { get; private set; }

    public PaginatedList(IQueryable<T> source, int pageIndex, int pageSize) 
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = source.Count();
        TotalPages = (int) Math.Ceiling(TotalCount / (double)PageSize);
        this.AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
     }
     public bool HasPreviousPage 
     {
        get {return (PageIndex > 0);}
     }
     public bool HasNextPage
     {
        get{return (PageIndex + 1 < TotalPages);}
     }
    }


    public class AjaxPaginationHandlerController : Controller
    {
        //
        // GET: /AjaxPaginationHandler/


        public ActionResult GetNextReposts(string GetMoreAction, string GetMoreLinktext, string ItemPartialViewName, int? Page, int? PageSize, string ParentId)
        {
            //do not change below
            PaginationSettings ps = new PaginationSettings();
            ps.GetMoreAction = GetMoreAction;
            ps.ItemPartialViewName = ItemPartialViewName;
            ps.Page = Page;
            ps.PageSize = PageSize;
            ps.ParentId = ParentId;

            //change this part
            TagThis.Models.UserRepository ur = new TagThis.Models.UserRepository();
            int pid = int.Parse(ParentId); //convert the string parent id to the right type
            ps.Iqueryable = ur.GetPostReposts(pid).Cast<dynamic>();
            ps.GetMoreLinktext = (ps.Iqueryable.Count() - (ps.PageSize * (ps.Page.Value + 1))).ToString() + " More";

            //do not change
            var random = new Random(System.DateTime.Now.Millisecond);
            int r = random.Next(1, 5000000);
            ViewData["random"] = r;
            ViewData["settings"] = ps;
            return PartialView("PaginationPage");
        }
        
        public ActionResult GetNextLikes(string GetMoreAction, string GetMoreLinktext, string ItemPartialViewName, int? Page, int? PageSize, string ParentId)
        {
            //do not change below
            PaginationSettings ps = new PaginationSettings();
            ps.GetMoreAction = GetMoreAction;
            ps.ItemPartialViewName = ItemPartialViewName;
            ps.Page = Page;
            ps.PageSize = PageSize;
            ps.ParentId = ParentId;

            //change this part
            TagThis.Models.UserRepository ur = new TagThis.Models.UserRepository();
            int pid = int.Parse(ParentId); //convert the string parent id to the right type
            ps.Iqueryable = ur.GetPostLikes(pid).Cast<dynamic>();
            ps.GetMoreLinktext = (ps.Iqueryable.Count() - (ps.PageSize*(ps.Page.Value+1))).ToString() + " More";

            //do not change
            var random = new Random(System.DateTime.Now.Millisecond);
            int r = random.Next(1, 5000000);
            ViewData["random"] = r;
            ViewData["settings"] = ps;
            return PartialView("PaginationPage");
        }

    }
}
