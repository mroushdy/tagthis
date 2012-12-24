using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using TagThis.Models;
using TagThis.ActionFilters;
using System.Web.Security;

namespace TagThis.Controllers
{
    public class FaveGroup
    {
        public int groupid;
        public string groupname;
        public List<Result> Pages;
        public List<string> grouptags;
    }
    public class UsersController : Controller
    {
        //
        // GET: /Faves/
        [AjaxViewSelector]
        public ActionResult User(string username, int? page, string sort, string time, string searchwhat, string query)
        {
            if (Membership.GetUser(username) != null)
            {
                searchwhat = searchwhat ?? "!user;" + username;

                UserRepository upr = new UserRepository();
                utils u = new utils();

                //Get User info
                ViewData["Subscribers"] = u.ShortNumber(upr.GetNSubscribers(username));

                ViewData["Subscribed"] = u.ShortNumber(upr.GetNSubscribedTo(username));

                ViewData["Likes"] = u.ShortNumber(upr.GetNLikes(username));

                ViewData["Posts"] = u.ShortNumber(upr.GetNPosts(username));

                ViewData["Utags"] = u.ShortNumber(upr.GetNUtags(username)); //get number of posts user is tagged in

                ViewData["Name"] = upr.GetFullName(username);

                ViewData["SearchWhat"] = searchwhat;

                //gets the top 10 tags that the user is interested in
                string tags = "";
                SearchRepository sr = new SearchRepository();
                var tgs = sr.SuggestTags().Take(10).ToList<SuggestionTags>();
                foreach (SuggestionTags t in tgs)
                {
                    tags = tags + t.tagname.Trim()+ ", ";
                }

                ViewData["Tags"] = tags;

                string currentusername = (Membership.GetUser() == null ? "" : Membership.GetUser().UserName);

                //checks if the current user is viewing his own profile;
                if (currentusername.ToLower().Trim() == username.ToLower().Trim()){ViewData["MyProfile"] = "true";}
                else{ViewData["MyProfile"] = "false";}

                //gets profile image
                Guid uid = (Guid)Membership.GetUser(username).ProviderUserKey;
                ViewData["ProfileImage"] = upr.GetProfileImage(uid,"large");
                ViewData["username"] = username;

                if (searchwhat.StartsWith("!userfollowers"))
                {
                    List<Models.UserListItem> userlist = upr.GetFollowers(username);
                    ViewData["userlist"] = userlist;
                    ViewData["usercount"] = userlist.Count();
                    return PartialView("ProfileUserList");
                }
                else if (searchwhat.StartsWith("!userfollowing"))
                {
                    List<Models.UserListItem> userlist = upr.GetFollowing(username);
                    ViewData["userlist"] = userlist;
                    ViewData["usercount"] = userlist.Count();
                    return PartialView("ProfileUserList");                
                }
                else
                {
                    SearchController search = new SearchController();
                    OmniResults R = search.GetViewContent(query ?? "", page ?? 0, sort ?? "newest", time ?? "all", searchwhat ?? "!user;" + username);
                    ViewData["results"] = R;
                    return PartialView("User");
                }


            }
            else
            {
                ViewData["username"] = username;
                return View("UserNotFound");
            }
        }

        [Authorize, AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Subscribe(string username)
        {
                UserRepository upr = new UserRepository();
                upr.AddSubscription((Guid)Membership.GetUser(username).ProviderUserKey);
                upr.Save();
                ViewData["username"] = username;
                return PartialView("SubscriptionButton");
        }

        [Authorize, AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UnSubscribe(string username, FormCollection formValues)
        {
                UserRepository upr = new UserRepository();
                upr.RemoveSubscription(username);
                upr.Save();
                ViewData["username"] = username;
                return PartialView("SubscriptionButton");
        }

    }
}
