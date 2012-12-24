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
using System.Xml.Linq;
using System.Collections.Generic;

namespace TagThis.Models
{
    public class TagRepository
    {
        private TagThisDataContext db = new TagThisDataContext();
        int Pageid = 0;

        public Guid GetUserId()
        {
            // get user id from database
            Guid userGuid = (Guid)(Membership.GetUser() == null ? Guid.Empty : (Guid?)Membership.GetUser().ProviderUserKey);
            return userGuid;
        }


        public void AddPageView(int Pageid)
        {
            //WebPage page = GetPage(Pageid);

            //number of views should not be unique to individual users since they have no control
            //however there could exist a table in the future to save all user ids who viewed a sertain thing
            //page.views = page.views + 1;

            //db.SubmitChanges();

        }

        public void AddPageClick(int Pageid)
        {
            WebPage page = GetPage(Pageid);
            //number of clicks is not unique to the user and could be changed in the future
            page.clicks = page.clicks + 1;

            db.SubmitChanges();
        
        }

        public WebPage GetPage(int id)
        {
            WebPage wp = db.WebPages.SingleOrDefault(p => p.page_id == id);
            if (wp != null) { Pageid = id; }
            return wp;
        }

        public int CheckPage(string url)
        { 
            var UrlResult = (from p in db.WebPages
                          where p.url == url
                          select p).SingleOrDefault();
            if (UrlResult != null)
            {
                return UrlResult.page_id;
            }
             //if it does not exist return null
            else
            {
                return -1;
            }
        }

        public tagmap GetUserTagMap(string tagname,int pageid)
        {
           var tag = db.tags.SingleOrDefault(t => t.name == tagname);
           return (from TM in db.tagmaps
                          where TM.tag_id==tag.tag_id && TM.page_id == pageid && TM.user_id == GetUserId()
                          select TM).FirstOrDefault();
        }

        public void DeleteTagMap(tagmap tagmap)
        {
            db.tagmaps.DeleteOnSubmit(tagmap);
        }

        public string[] GetUserTags(int pageid)
        {
            var Result = (from TM in db.tagmaps
                          from t in db.tags
                          where TM.page_id == pageid && TM.user_id == GetUserId() && t.tag_id == TM.tag_id
                          select t.name);

            if (Result != null)
            {
                return Result.ToArray();
            }
            else 
            {
                return null; 
            }
        }

        public int[] GetUserTagMapIDs(int pageid)
        {
            var Result = (from TM in db.tagmaps
                          where TM.page_id == pageid && TM.user_id == GetUserId()
                          select TM.tagmap_id);

            if (Result != null)
            {
                return Result.ToArray();
            }
            else { return null; }
        }

        public string CheckUserTags(int pageid)
        { 
                    var Result = (from TM in db.tagmaps
                          where TM.page_id == pageid && TM.user_id == GetUserId()
                          select new { TM.tagmap_id, TM.tag_id });

                    if (Result != null)
                    {
                        return Result.Count().ToString();
                    }
                    else { return null; }
        }


        public int HandlePost(int pageid, string microblogpost, int? RepostedFrom,DateTime date)
        {
            Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;
            UserRepository ur = new UserRepository();

            var r = (from h in db.Posts where h.pageid == pageid && h.UserId.ToString() == userGuid.ToString() select h).SingleOrDefault();

            if (r == null)
            {
                WebPage w = GetPage(pageid);
                Post dp = new Post();
                dp.UserId = userGuid;
                dp.Date = date;
                dp.Comment = microblogpost;
                dp.User_Name = Membership.GetUser().UserName;
                dp.Full_Name = ur.GetFullName(dp.User_Name);
                if (RepostedFrom.HasValue) { dp.RepostedFrom_postid = RepostedFrom.Value; }
                w.Posts.Add(dp);
                db.SubmitChanges();
            }
            else 
            { 
                //page is added twice by same guy. sneaky.
            }

            //returns postid
            return (from h in db.Posts where h.pageid == pageid && h.UserId.ToString() == userGuid.ToString() select h).SingleOrDefault().id;
        }

        public int Add(WebPage page)
        {
            //check if url exists
            var UrlResult = (from p in db.WebPages
                          where p.url == page.url
                          select p).SingleOrDefault();
            if (UrlResult != null)
            {
                Pageid = UrlResult.page_id;

            }
                //if it does not exist create a new one
            else
            {
                //WebPage newpage = new WebPage();
                page.AddedBy = Membership.GetUser().UserName;
                page.date = page.date;
                db.WebPages.InsertOnSubmit(page);
                db.SubmitChanges();
                var UrlResult2 = (from p in db.WebPages
                                 where p.url == page.url
                                 select p).SingleOrDefault();
                if (UrlResult2 == null)
                {
                    //failure adding page to database
                }
                else
                {
                    Pageid = UrlResult2.page_id;
                }
            }
            return Pageid;
        }

        public void Add(tag Tag)
        {
 
            var result = (from t in db.tags
                 where t.name == Tag.name
                 select t).SingleOrDefault();
            WebPage Page = GetPage(Pageid);
            tagmap TM = new tagmap();
            TM.date = DateTime.Now;
            if (result != null)
            {
                TM.tag_id = result.tag_id;
                TM.user_id = GetUserId();
                Page.tagmaps.Add(TM);
            }
            else
            {

                db.tags.InsertOnSubmit(Tag);
                db.SubmitChanges();
                var result2 = (from t in db.tags
                              where t.name == Tag.name
                              select t).SingleOrDefault();
                if (result2 == null)
                {
                    //failure adding tag to database
                }
                else
                {
                    TM.tag_id = result2.tag_id;
                    TM.user_id = GetUserId();
                    Page.tagmaps.Add(TM);
                }
            }
        }
        public void Save()
        {
            
            db.SubmitChanges();
        }


        public List<string> GetXmlMainGenres()
        {

            XDocument xdoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Content/genre-list.xml"));
            return (from gk in xdoc.Descendants("GenreKey") select gk.Attribute("name").Value).ToList<string>();
        }
    }
}
