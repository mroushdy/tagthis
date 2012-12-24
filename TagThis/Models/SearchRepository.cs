using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq.Expressions;
using System.Data.Linq.Mapping;

namespace TagThis.Models
{



    //The class to make the list containing the information from the advanced search query
    public class SearchQuerylist
    {
        // Properties
        public char opr { get; set; }
        public string tag { get; set; }
        public bool inbracket { get; set; }
        public int bracketid { get; set; }
        public bool firstinbracket { get; set; }
        public bool lastinbracket { get; set; }

        //constructor
        public SearchQuerylist()
        {
            tag = null;
            firstinbracket = false;
            lastinbracket = false;
            inbracket = false;
            opr = 'm';
            bracketid = 0;

        }
    }

    public class Result
    {
        // Properties
        public double? sortscore; //calculated depending on which search function was used to search
        public int tagcount;
        public int relevance;
        public int comments;
        public WebPage page;
        public IQueryable<CommentResult> CommentsList;
        public string subscriber;
        public IQueryable<TopTagsInPage> toptags;
        public UserPageRelation Userupr;
        public PageOwner pageOwner;
        public int rating;
        public int reposts;
    }

    //The pageowner is actually not the page owner but is the Owner's Post of the pages
    public class PageOwner
    {
        public string OwnerId;
        public string RepostedFromUserId;
        public string OwnerPost; //is actually the post comment
        public int PostId;
        public string OwnerPictureUrl;
        public int rating;
        public int reposts;
        public DateTime Date;

    }
    
    public class TopTagsInPage
    {
        // Properties
        public int count;
        public tag tagbody;

    }

    public class PageSuggestInfo
    {
        // Properties
        public int pageid;
        public IQueryable<TopTagsInPage> toptags;

    }

    public class SuggestionTags
    {
        public int count;
        public int tagid;
        public string tagname;
    }


    public class SearchRepository
    {
        //the number of comments to be shown by default on each post
        int ncomments = 3;

        private TagThis.Models.TagThisDataContext db = new TagThis.Models.TagThisDataContext();
        UserRepository ur = new UserRepository();


        Guid UID = (Guid)(Membership.GetUser() == null ? Guid.Empty : (Guid?)Membership.GetUser().ProviderUserKey);


        public Guid GetUserId()
        {
     
            // get user id from database
            Guid userGuid = (Guid)(Membership.GetUser() == null ? Guid.Empty : (Guid?)Membership.GetUser().ProviderUserKey);
            return userGuid;
        }


        public int GetPageRating(int pageid)
        {
           return (from a in db.UserPageRelations where a.Page_id == pageid select a).Sum(v => (int?)v.Rate) ?? 0;    
        }

        public int GetPostRating(int postid)
        {
            return (from a in db.UserPageRelations where a.Post_id == postid select a).Sum(v => (int?)v.Rate) ?? 0;
        }

        public int GetPageReposts(int pageid)
        {
            return (from a in db.Posts where a.pageid == pageid select a).Count();
        }

        public int GetPostReposts(int postid)
        {
            return (from a in db.Posts where a.RepostedFrom_postid == postid select a).Count();
        }

        public int GetPageComments(int postid)
        {
            return (from c in db.Comments where c.postid == postid select c).Count();
        
        }

        public IQueryable<TopTagsInPage> GetTopTags(int pageid)
        {
            return (IQueryable<TopTagsInPage>)(from tm in db.tagmaps where tm.page_id == pageid group tm by tm.tag_id into grp select new TopTagsInPage { count = grp.Count(), tagbody = (tag)(from t in db.tags where t.tag_id == grp.Key select t).SingleOrDefault() }).OrderByDescending(g => g.count).Take(5);
        }

        public IQueryable<SuggestionTags> MostPopularTags(string timespan)
        {

            IQueryable<SuggestionTags> unsorted = null;
            DateTime datecomparer;
            switch (timespan)
            {
                case "1y":
                    unsorted = (IQueryable<SuggestionTags>)(from tm in db.tagmaps where DateTime.Now.Year - tm.date.Year <= 1 group tm by tm.tag_id into grp select new SuggestionTags { tagname = (from t in db.tags where t.tag_id == grp.Key select t.name).SingleOrDefault(), count = grp.Count() });
                    break;
                case "1m":
                    datecomparer = DateTime.Now.Subtract(new TimeSpan(31, 0, 0, 0, 0));
                    unsorted = (IQueryable<SuggestionTags>)(from tm in db.tagmaps where tm.date >= datecomparer group tm by tm.tag_id into grp select new SuggestionTags { tagname = (from t in db.tags where t.tag_id == grp.Key select t.name).SingleOrDefault(), count = grp.Count() });
                    break;
                case "1w":
                    datecomparer = DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0, 0));
                    unsorted = (IQueryable<SuggestionTags>)(from tm in db.tagmaps where tm.date >= datecomparer group tm by tm.tag_id into grp select new SuggestionTags { tagname = (from t in db.tags where t.tag_id == grp.Key select t.name).SingleOrDefault(), count = grp.Count() });
                    break;
                case "1d":
                    datecomparer = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0, 0));
                    unsorted = (IQueryable<SuggestionTags>)(from tm in db.tagmaps where tm.date >= datecomparer group tm by tm.tag_id into grp select new SuggestionTags { tagname = (from t in db.tags where t.tag_id == grp.Key select t.name).SingleOrDefault(), count = grp.Count() });
                    break;
                case "1h":
                    datecomparer = DateTime.Now.Subtract(new TimeSpan(0, 1, 0, 0, 0));
                    unsorted = (IQueryable<SuggestionTags>)(from tm in db.tagmaps where tm.date >= datecomparer group tm by tm.tag_id into grp select new SuggestionTags { tagname = (from t in db.tags where t.tag_id == grp.Key select t.name).SingleOrDefault(), count = grp.Count() });
                    break;
            }
            return unsorted.OrderByDescending(o => o.count);
        }


        //Gets the page owner's posts. Currently it just selects a random page owner.
        public Post GetPageOwner(int pageid, string searchwhat)
        {
            if (searchwhat == "subscriptions")
            { 
                return (from p in db.Posts from s in db.Subscriptions where s.Subscriber == UID && s.Subscribedto == p.UserId orderby p.Date descending select p).FirstOrDefault();
            }
            else
            {
                return (from dp in db.Posts where dp.pageid == pageid orderby dp.Date descending select dp).FirstOrDefault();
            }


            /*
             * 
                       return (Post)(from m in db.tags
                    let posts = (from dp in db.Posts where dp.pageid == pageid select dp)
                    let latest = posts.OrderByDescending(o => o.Date).FirstOrDefault()
                    let sharedsamedayaslatest = (from p in posts where p.Date.AddDays(1) >= latest.Date select p)
                    let sharedsamedayaslatestcount = sharedsamedayaslatest.Count()
                    let followedpost = (from p in posts )
                    let followedpostcount = followedpost.Count()

                    let result = (posts.Count() > 0 ? (

                       sharedsamedayaslatestcount > 1 ? (
                           followedpostcount == 1 ? followedpost : (

                               followedpostcount > 1 ? (from s in followedpost let reposts = GetPostReposts(s.id) let rating = GetPostRating(s.id) let score = reposts + rating orderby score descending select s) : (
                                   
                               )
                           )

                       ) : sharedsamedayaslatest

                    ) : posts)

                    select result).FirstOrDefault();
             * 
             * 
            var posts = (from dp in db.Posts where dp.pageid == pageid select dp);
            var returned = posts;

            if(posts.Count() > 0)
            {
                var latest = posts.OrderByDescending(o => o.Date).FirstOrDefault();
                var sharedsamedayaslatest = (from p in posts where p.Date.AddDays(1) >= latest.Date select p);
                if (sharedsamedayaslatest.Count() > 1)
                {
                    var followedpost = (from p in posts from s in db.Subscriptions where s.Subscriber == UID && s.Subscribedto == p.UserId select p);
                    var count = followedpost.Count();
                    if (count == 1)
                    {
                        returned = followedpost;
                    }
                    else if (count > 1)
                    {
                        returned = (from s in followedpost let reposts = GetPostReposts(s.id) let rating = GetPostRating(s.id) let score = reposts + rating orderby score descending select s);
                    }
                    else
                    { 
                        //no follower shared this post so return the most liked
                        returned = (from s in sharedsamedayaslatest let reposts = GetPostReposts(s.id) let rating = GetPostRating(s.id) let score = reposts + rating orderby score descending select s);
                    }
                }
                else
                {
                    //no shares on the day the latest share
                    returned = sharedsamedayaslatest;
                }
            }
            else
            {
                //only one post
                returned = posts;
            }


            return returned.FirstOrDefault(); //order by userid gets a random one

     */

        }


        //IMPORTANT!!Also incorporate the HATE by making the tag count negative so that it would have an effect too!!!
        public IQueryable<SuggestionTags> SuggestTags()
        { 
            //gets a list with the top tags that the user likes
            Guid UID = GetUserId();
            IQueryable<SuggestionTags> SuggestionTagList = (IQueryable<SuggestionTags>)(from imaps in db.interestmaps where imaps.userid.ToString() == UID.ToString() group imaps by imaps.tag_id into grp select new SuggestionTags { count = grp.Count(),tagid = grp.Key, tagname = (from t in db.tags where t.tag_id == grp.Key select t.name).SingleOrDefault() }).OrderByDescending(g => g.count); 
            return SuggestionTagList;
        }
        

        public double GetAVG(int count,double avg)
        {
            return (count * 50) / avg;
        }
       
       

        //////Search algorithms that get final page results of type Result starts//////////


        //gets a list of pageids of all the pages that have the tags in the aray with an OR search
        public IQueryable<int> Search(List<string> tagarray)
        {
            return (IQueryable<int>)(from tagmaps in db.tagmaps
                                     from tags in db.tags
                                     where tagmaps.tag_id == tags.tag_id && tagarray.Contains(tags.name)
                                     group tagmaps by new { tagmaps.page_id } into grp
                                     select grp.Key.page_id);
        }

        //gets a list of pages that have the single tag
        public IQueryable<int> Search(string tag)
        {
            return (IQueryable<int>)(from tagmaps in db.tagmaps
                                     from tags in db.tags
                                     where tagmaps.tag_id == tags.tag_id && tag == tags.name
                                     group tagmaps by new { tagmaps.page_id } into grp
                                     select grp.Key.page_id);

        }

        //done- half done- grouping still not done - this function should be changed to accept iqueryable of type post and not webpage however this could create a problem that the user could see the same Page(song) twice in the search result. This should be fixed by grouping the post iqueryable by post.page id. The number of people that shared the same posts within a given time could go in the ranking. This could also be used to show a facebook like Marwan and 28 others shared blabla.
        private IQueryable<Result> toResults(IQueryable<Post> query, Guid UID)
        {
            return (from post in query 
                    select new Result
            {
                tagcount = 0,
                relevance = 0,
                sortscore = 0,
                Userupr = (UserPageRelation)(from u in post.UserPageRelations where u.Post_id == post.id && u.UserId == UID select u).SingleOrDefault(),
                page = post.WebPage,
                comments = GetPageComments(post.id),
                pageOwner = new PageOwner
                {
                    OwnerId = post.User_Name,
                    RepostedFromUserId = post.Post1.User_Name,
                    OwnerPictureUrl = ur.GetProfileImage(post.UserId, "mini"),
                    OwnerPost = post.Comment,
                    Date = post.Date,
                    PostId = post.id,
                    rating = GetPostRating(post.id),
                    reposts = GetPostReposts(post.id),
                },
                rating = GetPageRating(post.WebPage.page_id),
                reposts = GetPageReposts(post.WebPage.page_id),
                toptags = GetTopTags(post.WebPage.page_id)
            });
        }

        
        //Gets the final results of the search where a query is included to get tagcount of query tags in result
        public IQueryable<Result> Search(IQueryable<int> pageids)
        {
            Guid UID = GetUserId();
            var p = (from post in db.Posts where pageids.Contains(post.pageid) select post);
            return toResults(p,UID);
        }

        public IQueryable<Result> FindRelavantPages(Result p)
        {

            Guid UID = GetUserId();


            return (from page in db.WebPages let dp = GetPageOwner(page.page_id, "everyone")
                    select new Result
                    {
                        relevance = (from o in db.tagmaps from s in db.tagmaps where s.page_id == page.page_id && s.tag_id == o.tag_id && o.page_id == p.page.page_id select s).Count(),
                        tagcount = (from tm in db.tagmaps where tm.page_id == page.page_id select tm.page_id).Count(),
                        sortscore = 0,
                        Userupr = (UserPageRelation)(from u in db.UserPageRelations where u.Page_id == page.page_id && u.UserId == UID select u).SingleOrDefault(),
                        page = page,
                        toptags = GetTopTags(page.page_id),
                        comments = GetPageComments(dp.id),
                        pageOwner = new PageOwner
                        {
                            OwnerId = dp.User_Name,
                            RepostedFromUserId = dp.Post1.User_Name,
                            OwnerPictureUrl = ur.GetProfileImage(dp.UserId, "mini"),
                            OwnerPost = dp.Comment,
                            Date = dp.Date,
                            PostId = dp.id,
                            rating = GetPostRating(dp.id),
                            reposts = GetPostReposts(dp.id),
                        },
                        rating = GetPageRating(page.page_id),
                        reposts = GetPageReposts(page.page_id),
                    }).Where(r => r.page.page_id != p.page.page_id).OrderByDescending(o => o.relevance / (o.tagcount + 1));


        }

        //get pages that were shared by my subscribers
        public IQueryable<Result> GetSubscriptions()
        {
            var posts = (from s in db.Subscriptions from pst in db.Posts where s.Subscriber.ToString() == UID.ToString() && pst.UserId.ToString() == s.Subscribedto.ToString() select pst);
            return toResults(posts,UID);
        }


        //the effect of relevance will not be incorporated in the sorting
        public IQueryable<Result> GetSuggestions()
        {
            var posts = (from s in SuggestTags().Take(100) from tm in db.tagmaps where tm.tag_id == s.tagid group tm by new { tm.page_id } into grp let pst = (from p in db.Posts where p.pageid == grp.Key.page_id select p).SingleOrDefault() select pst);
            return toResults(posts, UID);
        }


        //Gets all the posts on sixtysongs
        public IQueryable<Result> Search()
        {
            return toResults(db.Posts, UID);
        }

        public IQueryable<Result> GetUserLikes(string username)
        {
            Guid UID = (Guid)Membership.GetUser(username).ProviderUserKey;
            var posts = (from p in db.Posts from u in db.UserPageRelations where u.UserId.ToString() == UID.ToString() && u.Rate == 1 && u.Post_id == p.id select p);
            return toResults(posts, UID);
        }


        public IQueryable<Result> SearchUserTags(string username)
        {
            Guid userid = (Guid)Membership.GetUser(username).ProviderUserKey;
            var posts = (from dp in db.Posts from p in db.PostUserTags where p.Userid.ToString() == userid.ToString() && dp.id == p.Postid select dp);
            return toResults(posts, UID);
        }


        public IQueryable<Result> SearchUser(string username)
        {
            Guid UID = (Guid)Membership.GetUser(username).ProviderUserKey;
            UserRepository ur = new UserRepository();
            return (from page in db.WebPages
                    from dp in db.Posts
                    where page.page_id == dp.pageid && dp.UserId == UID
                    select new Result
                    {
                        tagcount = 0,
                        relevance = 0,
                        //usually sorted by latest but sort score could be there anyway just incase.
                        sortscore = 0,
                        Userupr = (UserPageRelation)(from u in db.UserPageRelations where u.Page_id == page.page_id && u.UserId == UID select u).SingleOrDefault(),
                        page = page,
                        comments = GetPageComments(dp.id),
                        pageOwner = new PageOwner { PostId = dp.id, OwnerId = username, RepostedFromUserId = dp.Post1.User_Name, OwnerPost = dp.Comment, OwnerPictureUrl = ur.GetProfileImage(UID, "mini"), Date = dp.Date, reposts = GetPostReposts(dp.id), rating = GetPostRating(dp.id) },
                        rating = GetPageRating(page.page_id),
                        reposts = GetPageReposts(page.page_id),
                        toptags = GetTopTags(page.page_id)
                    });
        }

        //Gets the latest post for a given page
        public Result Search(int postid)
        {
            var p = (from pst in db.Posts
                    where pst.id == postid
                    select pst);
            return toResults(p, UID).SingleOrDefault();
        }


        //Gets a Result by postid
        public Result SearchGetPost(int postid)
        {
            var post = (from dp in db.Posts where postid == dp.id select dp).SingleOrDefault();
            Guid UID = GetUserId();
            return (from page in db.WebPages let dp = post
                    where page.page_id == dp.pageid
                    select new Result
                    {
                        tagcount = 0,
                        relevance = 0,
                        //usually sorted by latest but sort score could be there anyway just incase.
                        sortscore = 0,
                        Userupr = (UserPageRelation)(from u in db.UserPageRelations where u.Page_id == page.page_id && u.UserId == UID select u).SingleOrDefault(),
                        page = page,
                        comments = GetPageComments(dp.id),
                        pageOwner = new PageOwner { PostId = dp.id, RepostedFromUserId = dp.Post1.User_Name, OwnerId = dp.User_Name, OwnerPost = dp.Comment, OwnerPictureUrl = ur.GetProfileImage(dp.UserId, "mini"), Date = dp.Date, reposts = GetPostReposts(dp.id), rating = GetPostRating(dp.id) },
                        rating = GetPageRating(page.page_id),
                        reposts = GetPageReposts(page.page_id),
                        toptags = GetTopTags(page.page_id)
                    }).SingleOrDefault();
        }


        //////Algorithms that get final page results of type Result ends//////////


        //filters the results by the given timespan in text then orders it
        public IQueryable<Result> Sort(IQueryable<Result> unsorted, string sortby, string timespan)
        {
            DateTime datecomparer;
            bool ignoretime = false;

            switch (sortby)
            {
                case "newest":
                    return unsorted.OrderByDescending(r => r.pageOwner.Date);
                    break;
                case "popular":
                    {
                        switch (timespan)
                        {
                            case "1y":
                                unsorted = from u in unsorted where DateTime.Now.Year - u.page.date.Year <= 1 select u;
                                ignoretime = true;
                                break;
                            case "1m":
                                datecomparer = DateTime.Now.Subtract(new TimeSpan(31, 0, 0, 0, 0));
                                unsorted = from u in unsorted where u.page.date >= datecomparer select u;
                                ignoretime = true;
                                break;
                            case "1w":
                                datecomparer = DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0, 0));
                                unsorted = from u in unsorted where u.page.date >= datecomparer select u;
                                ignoretime = true;
                                break;
                            case "all": // do nothing since we need to get the popular all time
                                ignoretime = true;
                                break;
                        }
                        break;
                    }
            }

            //default is popular things now so time is not ignored
            return Rank(unsorted, ignoretime);
        }




        //start sorting



        public IQueryable<Result> Rank(IQueryable<Result> Results, bool ignoretime)
        {
            DateTime now = DateTime.Now;

            return (from r in Results

                    // page edge //

                    /*
                    //gets the tag ids of all the tags that the user likes
                    let usertags = (IQueryable<int>)(from im in db.interestmaps where im.userid.ToString() == UID.ToString() select im.tag_id)

                    //counts the user tags
                    let usertagscount = usertags.Count()

                    // sees how many of the tags that the user likes are in the page
                    let usertagsinpage = (from tm in db.tagmaps from ut in usertags where tm.page_id == r.page.page_id && ut == tm.tag_id select 1).Count()

                    let pageafinity = usertagsinpage / (usertagscount + 1)
                     
                    let pageEdge = pageafinity * pagetimedecayfactor
                    */


                    //Takes the post that will be viewed
                    let Post = (from s in db.Posts where s.id == r.pageOwner.PostId select s).SingleOrDefault()

                    //share edge


                    //Note: this line --> (ignoretime ? 1 : (now - s.date).TotalSeconds) checks if ignore date is set to true and does not divide by date and instead divides by one

                    let CommentsEdge = (double?)(from c in db.Comments where c.postid == r.pageOwner.PostId select c).Sum(s => 1 / (ignoretime ? 1 : (now - s.date).TotalHours) ) ?? 0

                    let RepostsEdge = (double?)(from s in db.Posts where s.RepostedFrom_postid == r.pageOwner.PostId select s).Sum(s => 1 / (ignoretime ? 1 : (now - s.Date).TotalHours) ) ?? 0

                    let LikesEdge = (double?)(from u in db.UserPageRelations where u.Post_id == r.pageOwner.PostId select u).Sum(s => s.Rate / (ignoretime ? 1 : (now - s.Date).TotalHours) ) ?? 0

                    let CreatedEdge = 10

                    //if statement if ignoretime set to true lets tiemdecay = 1
                    //let CreatedEdge = ignoretime ? 1 : Math.Exp(-(DateTime.Now - Post.Date).TotalHours)



                    let TotalEdge = (1 * RepostsEdge + 2 * CreatedEdge + 3 * LikesEdge + 2 * CommentsEdge) * (1000000000 / (ignoretime ? 1 : (now - Post.Date).TotalDays) )

                    orderby (TotalEdge) descending
                    select new Result 
                    {
                        sortscore = TotalEdge,
                        tagcount = r.tagcount,
                        relevance = r.relevance,
                        Userupr = r.Userupr, 
                        page = r.page,
                        comments = r.comments,
                        pageOwner = r.pageOwner,
                        rating = r.rating,
                        reposts = r.reposts,
                        toptags = r.toptags
                    });

        }




        public bool isoperator(char c) { char[] operators = { '(', ')', '+', '-', '|' }; if (operators.Contains(c)) { return true; } else { return false; } }

        //Give it a query it gives you back the results
        public IQueryable<Result> GetResults(string query, IQueryable<Result> Results)
        {
            //Search where options
            //all searches everything, Loves searches 

            //string query = "(music|tunes)+(free|boob)";
            List<SearchQuerylist> querylist = new List<SearchQuerylist>();
            bool bracket = false;
            int bracketid = 0;
            SearchQuerylist item = new SearchQuerylist();
            //code to make sense out of the query string
            foreach (char l in query)
            {

                //if the character is not an operater build the tag string
                if (!isoperator(l))
                { item.tag = item.tag + l; }
                else
                {
                    //if we first get the operator then add the item if we dont check if this is last bracket then it will be added to the next item which is not right
                    if (l == ')') { item.lastinbracket = true; bracket = false; }
                    //once we reach an operator add the tag to the tag array and solve a bug when last item is not considered in bracket
                    if (!string.IsNullOrEmpty(item.tag)) { if (item.lastinbracket | bracket == true) { item.inbracket = true; item.bracketid = bracketid; } querylist.Add(item); item = new SearchQuerylist(); }
                    //if item is in a bracket only the "|" is allowed in a bracket so we should show an error otherwise
                    if (bracket)
                    {
                        if (l == '|') { /*we are good and no error*/ item.opr = '|'; }
                        else { /*there is an error*/}
                    }
                    //check if we are going to go into a bracket if not then it must be another operator
                    if (l == '(')
                    {
                        if (l == '(') { bracket = true; bracketid++; item.firstinbracket = true; }
                    }
                    else if (bracket == false & l != '|') { item.opr = l; }
                    else if (bracket == false & l == '|') { /*Show error since opr | cannot be used except in bracket*/}
                }
            }
            //add the last item that does not have an operator after it so is not added in the loop
            if (!string.IsNullOrEmpty(item.tag)) { if (item.lastinbracket) { item.inbracket = true; } querylist.Add(item); item = new SearchQuerylist(); }


            //perform the search
            IQueryable<int> results = null;
            IQueryable<int> orResults = null;
            List<String> brackettags = new List<String>();
            List<String> tagnames = new List<String>();
            char bracketOperator = 'm';
            foreach (SearchQuerylist q in querylist)
            {
                if (q.inbracket)
                {
                    if (q.firstinbracket) {  brackettags = new List<String>(); bracketOperator = q.opr; }
                    brackettags.Add(q.tag);
                    if (q.lastinbracket)
                    {
                        orResults = Search(brackettags);
                        //bracket is at the begining
                        if (bracketOperator == 'm') { results = orResults; }
                        else if (bracketOperator == '+') { results = (from or in orResults from r in results where or == r select r); }
                        else if (bracketOperator == '-') { results = (from or in orResults from r in results where r != or select r); }
                    }
                }
                else if (q.opr == '+') { results = (from r in results from s in Search(q.tag) where r == s select r); }
                else if (q.opr == '-') { results = (from r in results from s in Search(q.tag) where r != s select r); }
                else if (q.opr == 'm') { results = Search(q.tag); }

                //Put all the tag names in a string list
                tagnames.Add(q.tag);

            }

            //List<Result> FullResults = new List<Result>();
            //get the results with the full details we need
           // FullResults = 
            return Search(results);
        }
    }
}
