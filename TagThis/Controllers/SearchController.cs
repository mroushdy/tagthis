using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using TagThis.Models;
using System.Web.Security;
using TagThis.ActionFilters;

namespace TagThis.Controllers
{

    //A class that has all the objects that is needed to be passed to any view that has results
    public class OmniResults
    {
     public string query;
     public int? page;
     public string sort;
     public string time;
     public string SearchWhat;
     public PaginatedList<Result> Results;
     public string ResultsKey;
    }

    public class SearchController : Controller
    {
        const int pageSize = 40;

        private TagThis.Models.TagThisDataContext db = new TagThis.Models.TagThisDataContext();
        SearchRepository S = new SearchRepository();
        List<Result> FR = new List<Result>();
        //
        // GET: /Search/
        public ActionResult Index()
        {

          return View();
        }



        //catches the search form, does some encoding to the query then redirects to the Search function
        //the searchbox has the id simple. I dont know why I called it simple. But thats its id in the html.
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Find(string Simple, string SearchWhat, string time, string sort)//FormCollection form)
        {
            string Query = "";

            //send the search terms to the view to be put back again into the search box
            TempData["s-terms"] = Simple.Split(',').ToList<string>();

            Query = Simple.Replace(',', '+');

            //encode the url and then remove % and sub with _
            string encoded = HttpUtility.UrlEncode(Query);

            encoded = encoded.Replace('%', '_');

            //redirect to results using the url routing MAJIC
            return RedirectToAction(encoded, new { SearchWhat = SearchWhat, time = time, sort = sort, page = 0 });
        }


        //catches any link to sixtysongs.com/search/query and also catches the searchbox text after being manipulated
        public ActionResult Search(string query, int? page, string sort, string time, string SearchWhat)
        {
            return RedirectToAction("Results", new {query = query, page = page, sort = sort, time = time, SearchWhat = SearchWhat});
        }


        //also handles ajax requests from homepage in suggest and all pages
        [AjaxViewSelector]
        public ActionResult Results(string query,int? page, string sort, string time,string SearchWhat)
        {
            //to send search terms to view
            if (TempData["s-terms"] != null) { ViewData["s-terms"] = TempData["s-terms"]; }
            else if (query != null) { var nl = new List<string>(); nl.Add(query); ViewData["s-terms"] = nl; }

                OmniResults Results = new OmniResults();

                    if (SearchWhat.StartsWith("!user"))
                    {
                        string username = SearchWhat.Replace("!user", "");
                        return RedirectToAction("User", "Users", new { username = username, page = page, sort = sort, time = time, searchwhat = SearchWhat, query = query });
                    }
                    else  // normal query so search what exactly??
                    {
                        Results = GetViewContent(query, page, sort, time, SearchWhat);
                        ViewData["Results"] = Results;
                        return PartialView("ResultsPartial");
                    }
        }

        public OmniResults GetViewContent(string query, int? page, string sort, string time, string SearchWhat)
        {
            //SelectWhat could be search faves loves or hates or all
            // get the results and perform the search
                OmniResults R = new OmniResults();
                PaginatedList<Result> Results;
                IQueryable<Result> R1 = null;
                string username = (Membership.GetUser() == null ? "" : Membership.GetUser().UserName);

                R.ResultsKey = "Results";

                //defaults
                //test if user follows a lot of people searchwhat = followers otherwise search what = suggestions to make sure the home feed is not empty
                if (string.IsNullOrEmpty(SearchWhat)) { SearchWhat = "everyone"; }
                if (string.IsNullOrEmpty(time)) { time = "now"; }
                if (string.IsNullOrEmpty(sort)) { sort = "popular"; }


                    if (SearchWhat.StartsWith("!user")) 
                    {
                        username = SearchWhat.Substring(SearchWhat.IndexOf(';') + 1);
                        if(SearchWhat.StartsWith("!userlikes"))
                        {
                            R1 = S.GetUserLikes(username);
                            R.ResultsKey = "UserLikes";
                        }
                        if (SearchWhat.StartsWith("!usertags"))
                        {
                            R1 = S.SearchUserTags(username); //user tags are the posts the user was tagged in
                            R.ResultsKey = "UserTags";
                        }
                        else
                        {
                            R1 = S.SearchUser(username);
                            R.ResultsKey = "User";
                        }
                    
                    }
                    else if (SearchWhat == "suggestions")
                    {
                        //This gets suggestions or people with similar tastes
                        R1 = S.GetSuggestions();
                        R.ResultsKey = "Suggestions";
                    }
                    else if (SearchWhat == "following")
                    {
                        //This gets the suggestions and passes the query incase user is searching suggestions
                        R1 = S.GetSubscriptions();
                        R.ResultsKey = "following";
                    }
                    else  //(SearchWhat == "everyone")
                    {
                        R.ResultsKey = "All";
                        R1 = S.Search();
                    }

                    
                    //finalize return
                    
                    //check if there is a search query and filter the results
                    if (!string.IsNullOrEmpty(query) && query.ToLower() != "everything")
                    {
                        query = query.Replace('_', '%');
                        string decoded = HttpUtility.UrlDecode(query);
                        //make sure that the query is not a special query such as "!loves" or "!hates"
                        if (!query.StartsWith("!")) { R1 = S.GetResults(decoded, R1); }
                    }
                    else
                    {
                        query = "!all";
                    }

                    // below removes hated pages :)
                    R1 = R1.Where(m => m.Userupr.Rate != -1 || m.Userupr.Rate == null);


                    //if user is a new user
                    if (SearchWhat == "firsttime")
                    {
                        //This puts the user facebook shares first then adds the popular now stuff after. This is due to the idea that the user would be hooked on the songs he shares the most first
                        IQueryable<Result> userfollowing = S.Sort(S.GetSubscriptions(),"popular", "now") ;
                        IQueryable<Result> FBshares = S.Sort(S.SearchUser(username), "newest", "");

                        IQueryable<Result> FinalMix = FBshares.ToList<Result>().AsQueryable<Result>().Union(userfollowing);
                        Results = new PaginatedList<Result>(FinalMix, page ?? 0, pageSize);
                        R.ResultsKey = "firsttime";
                    }
                    else
                    {
                        Results = new PaginatedList<Result>(S.Sort(R1, sort, time), page ?? 0, pageSize);
                    }
            
                    R.Results = Results;
                    R.query = query;
                    R.sort = sort;
                    R.SearchWhat = SearchWhat;
                    R.time = time;    
                    return R;

                
        }



        /* maybe replace with get widget loves instead kickasses
         
        public OmniResults GetWidgetFaves(string query, string sort, string time)
        {
            //SelectWhat could be search faves loves or hates or all
            // get the results and perform the search
            if (!string.IsNullOrEmpty(query))
            {
                OmniResults R = new OmniResults();
                PaginatedList<Result> Results;
                string username = Membership.GetUser().UserName;
                const int pageSize = 1000;
                R.query = query;
                if (string.IsNullOrEmpty(time)) { time = "all"; }
                R.time = time;
                query = query.Replace('_', '%');
                string decoded = HttpUtility.UrlDecode(query);
                if (string.IsNullOrEmpty(sort)) { sort = "relevancy"; }
                R.sort = sort;
                R.ResultsKey = "Results";
                IQueryable<Result> R1;
                if (query == "!all") { R1 = S.Search(); }
                else { R1 = S.GetResults(decoded); }
                Results = new PaginatedList<Result>(S.Sort(R1.Where(m => m.Userupr.Fav == true), sort, time),0, pageSize);
                R.Results = Results;
                R.ResultsKey = "Faves";
                R.Results = Results;
                return R;
            }
            else
            {
                return null;
            }
        }
         
         */

    }
}
