using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TagThis.Models;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace TagThis.Controllers
{
    public class AjaxController : Controller
    {
        //
        // GET: /Ajax/

        public void Index()
        {

        }

        [Authorize]
        public ActionResult LHF(int? pid, string value, int postid)
        {
            if (pid.HasValue == true & string.IsNullOrEmpty(value) == false)
            {

                int rate=0;
                UserRepository ur = new UserRepository();
                UserPageRelation upr = new UserPageRelation();
                SearchRepository sr = new SearchRepository();
                rate = ur.GetRating(pid.Value, postid);
                if (value == "l") 
                {
                    if (rate == 1) { upr.Rate = 0; rate = 0; ur.RemoveUserInterest(pid.Value); } 
                    else { 
                        upr.Rate = 1; rate = 1; ur.AddNewUserInterest(pid.Value);
                        ur.AddNotification(ur.GetUserId(), ur.GetPostOwner(postid), "postlike", postid.ToString());
                        } 
                
                }
                else if (value == "h") { if (rate == -1) { upr.Rate = 0; rate = 0; } else { upr.Rate = -1; rate = -1; }}
                upr.Page_id = pid.Value;
                upr.Post_id = postid;
                ur.AddRelation(upr);

                return Content(sr.GetPageRating(pid.Value).ToString());
            }
            else return Content("0");
        
        }


        [Authorize]
        public ActionResult DeletePost(int id)
        {
                UserRepository ur = new UserRepository();
                ur.DeletePost(id);
                return Content("1");
        }

        [Authorize]
        public ActionResult DeleteComment(int id)
        {
            UserRepository ur = new UserRepository();
            ur.DeleteComment(id);
            return Content("1");
        }
        
        /*
        //Currently not working and if needed to work should be replaced with the one in Search controller
        [Authorize, AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Searchfilter(string query, int? page, string sort, string time, string SearchWhat)
        {
            if (!string.IsNullOrEmpty(query))
            {
                SearchRepository S = new SearchRepository();
                PaginatedList<Result> Results;
                const int pageSize = 10;
                ViewData["query"] = query;
                if (string.IsNullOrEmpty(SearchWhat)) { SearchWhat = "all"; }
                if (string.IsNullOrEmpty(time)) { time = "all"; }
                ViewData["SearchWhat"] = SearchWhat;
                ViewData["time"] = time;
                query = query.Replace('_', '%');
                string decoded = HttpUtility.UrlDecode(query);
                if (string.IsNullOrEmpty(sort)) { sort = "relevancy"; }
                ViewData["sort"] = sort;
                if (Request.IsAjaxRequest())
                {
                    //these results are not a normal query
                    if (query == "!all")
                    {
                        Results = new PaginatedList<Result>(S.Sort(S.Search(), sort, time), page ?? 0, pageSize);
                    }
                    else if (query == "!suggest")
                    {
                        Results = new PaginatedList<Result>(S.Sort(S.SuggestPages(10), sort, time), page ?? 0, pageSize);
                    }

                    else  // normal query so search what exactly??
                    {
                        if (SearchWhat == "loves")
                        {
                            Results = new PaginatedList<Result>(S.Sort(S.GetResults("music").Where(m => m.Userupr.Rate == 1), sort, time), page ?? 0, pageSize);
                        }
                        else if (SearchWhat == "Hates")
                        {
                            Results = new PaginatedList<Result>(S.Sort(S.GetResults("music").Where(m => m.Userupr.Rate == -1), sort, time), page ?? 0, pageSize);
                        }
                        else  //search everything
                        {
                            Results = new PaginatedList<Result>(S.Sort(S.GetResults("music"), sort, time), page ?? 0, pageSize);
                        }
                    }
                    ViewData["results"] = Results;
                    return PartialView("ResultsPartial");
                }
                else
                {
                    return Content("This is not an Ajax request.");
                }
            }
            else
            {
                return Content("No search query submitted.");
            }
        }
        */



        [Authorize, AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubmitTags(int? pid, FormCollection formValues)
        {
            if (Request.IsAjaxRequest() == true & pid.HasValue == true)
            {
                int pageid = (int)pid.Value;
                TagRepository tagRepository = new TagRepository();
                //check if page exists
                if (tagRepository.GetPage(pageid) != null)
                {
                    ViewData["nothinghappened"] = "no";
                    //The array that will contain the success messages
                    List<string> messages = new List<string>();
                    //split tags into an Array
                    String Formtags = formValues["Tags"+pageid.ToString()];
                    //trim end and start to ensure that no sneaky person tries to add a tag twice
                    string[] notuniqueTaglist = Formtags.Split(',');

                    //To Make TagList have only unique values, to avoid the page being tagged more than once
                    List<string> uniquetags = notuniqueTaglist.Distinct().ToList<string>();
                    string[] Taglist = uniquetags.ToArray();

                    //To check which tags should be added and which tags should be removed.. tags in old but not in new will be removed
                    //and tags in new but not in old will be added and tags in both should not be either added or removes=d
                    //first get tags already made by the user
                    String[] oTaglist = tagRepository.GetUserTags(pageid);
                    int i = 0;
                    bool tagsdeleted = false;
                    bool tagsadded = false;
                    string tagsDeleted = "";
                    string tagsAdded = "";
                    foreach (String tag in oTaglist)
                    {

                        int index = Array.IndexOf(Taglist, tag.TrimEnd().TrimStart());
                        //Unmodiied user tag so remove from taglist not to be added twice
                        if (index >= 0) { Taglist.SetValue(null, index); }
                        //Tag exists in original list but not in new one so should be removed from the TagMap
                        else
                        {
                            tagRepository.DeleteTagMap(tagRepository.GetUserTagMap(tag, pageid));
                            tagRepository.Save();
                            tagsdeleted = true;
                            tagsDeleted = tagsDeleted + ", " + tag;
                        }
                        i++;
                    }

                    //Add each tag in the array to the database
                    foreach (String tag in Taglist)
                    {
                        if (!string.IsNullOrEmpty(tag))
                        {
                            tag Tag = new tag();
                            Tag.name = tag.ToLower().TrimEnd().TrimStart();
                            tagRepository.Add(Tag);
                            tagRepository.Save();
                            tagsadded = true;
                            tagsAdded = tagsAdded + ", " + tag;
                        }
                    }
                    if (tagsadded) { messages.Add("The following tags has been added: " + tagsAdded.Substring(2)); }
                    if (tagsdeleted) { messages.Add("The following tags has been deleted: " + tagsDeleted.Substring(2)); }
                    if (tagsadded == false & tagsdeleted == false) { ViewData["nothinghappened"] = "yes"; }

                    ViewData["pageid"] = pageid;
                    
                }
                else
                {
                    //The page you requested does not exist.
                }
            }
            else
            {
                //This is not an Ajax reguest.
            }

            return PartialView("TagSubmitForm");
        }

        //gets embed html
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetHtml(int pageid)
        {
            TagRepository tagRepository = new TagRepository();
            WebPage p = tagRepository.GetPage(pageid);

            //adds that this current user has clicked to view this html
            tagRepository.AddPageClick(pageid);

            return Content(p.html);
        }


        //rates comments
        [Authorize]
        public ActionResult CommentRate(int? id, string value)
        {
            UserRepository ur = new UserRepository();
            if (Request.IsAjaxRequest() == true & string.IsNullOrEmpty(value) == false & id.HasValue == true)
            {
                if (value == "u") 
                { 
                    ur.SetCommentRating(id.Value, 1); ur.Save();
                    ur.AddNotification(ur.GetUserId(), ur.GetComment(id.Value).userid, "commentlike", id.Value.ToString());

                }
                else if (value == "d") { ur.SetCommentRating(id.Value, -1); ur.Save(); }
                ViewData["commentrating"] = ur.GetCommentRating(id.Value).ToString();
                return PartialView("CommentRating");
            }
            else
                ViewData["commentrating"] = "0";
                return PartialView("CommentRating");
        }


        public ActionResult GetComments(int? pageid, int? postid, int? pageno)
        {
            if (pageid.HasValue && postid.HasValue)
            {
                UserRepository ur = new UserRepository();
                List<CommentResult> comments = new List<CommentResult>();

                int alreadyshowed = 3;
                int pagesize = 6;

                int pid = pageid.Value;
                /*if (sort == "newest") { comments = ur.GetComments(pid).OrderByDescending(s => s.comment.date).ToList(); ViewData["Clicked"] = "newest"; }
                else if (sort == "hated") { comments = ur.GetComments(pid).OrderBy(s => s.rating ?? 0).ToList(); ViewData["Clicked"] = "hated"; }
                else if (sort == "oldest") { comments = ur.GetComments(pid).OrderBy(s => s.comment.date).ToList(); ViewData["Clicked"] = "oldest"; }
                */

                //comments = ur.GetComments(pid).OrderByDescending(s => s.rating ?? 0).ToList();

                pageno = pageno.Value + 1;


                comments = ur.GetComments(postid.Value).OrderByDescending(s => s.comment.date).ToList();

                if (comments.Count() > ((pageno.Value) * pagesize)+alreadyshowed) { ViewData["cshowmore"] = "true"; } else { ViewData["cshowmore"] = "false"; }

                comments = comments.Take(alreadyshowed+(pageno.Value * pagesize)).ToList();


                ViewData["pageno"] = pageno.Value;
                ViewData["comments"] = comments;
                ViewData["pageid"] = pageid;
                ViewData["postid"] = postid;
            }
            else { ViewData["page"] = null; ViewData["comments"] = null; ViewData["RelevantPages"] = null; }
            return PartialView("Comments");
        }



        [Authorize, AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddComment(int? pid, FormCollection formValues, int? postid)
        {
            if (Request.IsAjaxRequest() == true & string.IsNullOrEmpty(formValues["Text"].ToString()) == false & pid.HasValue == true & postid.HasValue == true)
            {
                UserRepository ur = new UserRepository();

                var previouscommentors = ur.GetComments(postid.Value).OrderByDescending(o => o.comment.date).Select(s => s.comment.userid).Take(3);

                CommentResult CR = new CommentResult();
                Comment c = new Comment();
                c.text = formValues["Text"].ToString();
                CR.comment = ur.AddComment(c, pid.Value, postid.Value);
                CR.ProfileImage = ur.GetProfileImage(ur.GetUserId(),"mini");
                CR.rating = 0;
                ur.Save();

                ViewData["comment"] = CR;

                ViewData["CurrentUserName"] = ur.GetUserName();

                var postownerid = ur.GetPostOwner(postid.Value);

                ur.AddNotification(ur.GetUserId(), postownerid, "postcomment", CR.comment.id.ToString());

                foreach (var user in previouscommentors)
                {
                    if (user.ToString() != postownerid.ToString())
                    {
                        ur.AddNotification(ur.GetUserId(), user, "postcommentreply", CR.comment.id.ToString());
                    }
                }

            }
            return PartialView("comment");
        }



        public ActionResult GetNotifications()
        {
            UserRepository ur = new UserRepository();
            ViewData["Notifications"] = ur.GetNotifications();
            return PartialView("notifications");
        }

        public ActionResult ReadNotifications()
        {
            UserRepository ur = new UserRepository();
            ur.ReadNotifications();
            return Content("");
        }

        public ActionResult MoreResults(int page, string sort, string time, string SearchWhat,string query)
        {

                OmniResults Results = new OmniResults();
                SearchController sc = new SearchController();
                Results = sc.GetViewContent(query, page, sort, time, SearchWhat);
                ViewData["results"] = Results;
                return PartialView("ResultsPage");

        }




        /*
        public ActionResult GetPlayListItem(int pageid)
        {
            
            TagRepository tr = new TagRepository();
            TagThis.Models.WebPage page = tr.GetPage(pageid);
            return Json(new
            {
                Title = customerViewModel.ReturnStatus,
                ViewModel = customerViewModel,
                MessageBoxView = RenderPartialView(this, "_MessageBox", customerViewModel),
                CustomerInquiryView = RenderPartialView(this, "CustomerInquiryGrid", customerViewModel)
            });
        }
        */


    }
}
