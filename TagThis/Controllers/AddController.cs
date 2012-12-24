using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Web.Mvc.Ajax;
using TagThis.Models;
using HtmlAgilityPack;
using TagThis.ActionFilters;
using System.Web.Profile;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Net;
using System.Dynamic;

using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.YouTube;
using Google.GData.Extensions.MediaRss;
using Google.YouTube;

namespace TagThis.Controllers
{
    public class AddController : Controller
    {
        //
        // GET: /Add/

        public ActionResult Index()
        {
            return View();
        }



        //move it to ajax as it should be
        [Authorize, AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(string Tags, string url, string Desc, string Genre, int? RepostedFrom, string People)
        {

            int postid = AddPage(Tags, url, Desc, Genre, RepostedFrom, People, DateTime.Now);
            //success return page element

            ViewData["error"] = TempData["error"];

            if (postid != 0)
            {
                SearchRepository sr = new SearchRepository();
                Result pg = sr.Search(postid);
                ViewData["page"] = pg;
                UserRepository ur = new UserRepository();
                ViewData["CurrentUserName"] = ur.GetUserName();

                if (ur.GetUserData().FBautopost)
                {
                    //post to facebook
                    var fb = new Facebook.Web.FacebookWebClient();
                    dynamic parameters = new ExpandoObject();
                    parameters.song = "http://www.sixtysongs.com/post/" + postid.ToString();
                    try
                    {
                        dynamic result = fb.Post("me/sixtysongs:post", parameters);
                    }
                    catch { }
                }
            }
            else 
            {
                ViewData["page"] = null;
            }
            return PartialView("ShareBoxReturn");
        }


        public static readonly Regex YoutubeVideoRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase);

        public int AddPage(string Tags, string url, string Desc, string Genre, int? RepostedFrom, string People, DateTime date)
        {
            bool error = false;
            bool genreerror = false;
            SearchRepository sr = new SearchRepository();

            if (Desc.ToLower().Trim() == "describe this song")
            {
                Desc = null;
            }

            if (Genre.ToLower() == "pick a genre")
            {
                genreerror = true;
            }

            //both for youtube videos only
            string videoid = "";
            string cleanurl = "";

            bool notyoutubeerror = false;

            Match youtubeMatch = YoutubeVideoRegex.Match(url);
            if (youtubeMatch.Success) { videoid = youtubeMatch.Groups[1].Value; cleanurl = "http://www.youtube.com/watch?v=" + videoid; }
            else { notyoutubeerror = true; } //error not youtube video

            WebPage Page = new WebPage();

            bool urlerror = false;
            bool notmusicerror = false;
            string type = "";
            int pageid = 0;
            int postid = 0;
            List<string> videotags = new List<string>();

            if (!notyoutubeerror)
            {

                try
                {

                    YouTubeRequestSettings settings = new YouTubeRequestSettings("Music", "AI39si4oYhNceIvZ4v1V4jfxZ1H2RXutywnX8F19T2geUjGzHUSdKOWi2NKyAj1BQwuv9w86ipfWp5S57l0oA8eUTGZ6n-qZ2A");
                    YouTubeRequest request = new YouTubeRequest(settings);

                    Uri videoEntryUrl = new Uri("http://gdata.youtube.com/feeds/api/videos/" + videoid);
                    Video video = request.Retrieve<Video>(videoEntryUrl);

                    if ((from c in video.Categories where c.Label == "Music" select c).Count() > 0)
                    { }else{ notmusicerror = true; } // not music

                    string Description = video.Description;
                    string Title = video.Title;
                    if (Title.Length >= 96) { Title = shave(100, Title); }
                    if (Description.Length >= 252) { Description = shave(256, Description); }

                    //get the information from the view
                    Page.url = cleanurl;
                    Page.description = Description;
                    Page.name = Title;
                    Page.html = "";
                    Page.type = "video";
                    Page.thumburl = "http://img.youtube.com/vi/" + videoid + "/0.jpg";
                    if (date == null) { date = DateTime.Now; }
                    Page.date = date;
                    
                    //extract tags
                    videotags = video.Keywords.Split(',').ToList<string>();
   
                    type = "Video";

                }
                catch { urlerror = true; }
            }

           
            String Formtags = Tags;

            if (!string.IsNullOrEmpty(Formtags) && !urlerror && !notyoutubeerror && !notmusicerror && !genreerror)
            {

                UserRepository userrepository = new UserRepository();
                TagRepository tagRepository = new TagRepository();
                //Check if the current page exists in the database otherwise insert it and get its page id
                pageid = tagRepository.Add(Page);


                //Handles who shares the page along with his micro blog post
                String MicroBlogPost = Desc;
                postid = tagRepository.HandlePost(pageid, MicroBlogPost, RepostedFrom, Page.date);
                
                if(Genre != "#camefromfb")
                {

                //Add genre to tag
                Formtags = Formtags + "," + Genre;

                //trim end and start to ensure that no sneaky person tries to add a tag twice
                string[] notuniqueTaglist = Formtags.Split(',');


                //add genre keys as tags (incase user selects genre sub category only)


                    //get genres from xml file
                    List<string> xmlGenres = new List<string>();
                    XDocument xdoc = XDocument.Load(HttpContext.Server.MapPath("~/Content/genre-list.xml"));
                    var GenreKeys = from gk in xdoc.Descendants("GenreKey") select new { Header = gk.Attribute("name").Value, Children = gk.Descendants("Genre") };
                    //get matching genre keys
                    var mainkeys = (from gk in GenreKeys from gc in gk.Children from t in notuniqueTaglist where t.Trim().ToLower() == gc.Attribute("name").Value.Trim().ToLower() && gk.Children.Contains(gc) select gk.Header).ToList<string>();
               


                //adds the youtube tags and the main genres to the taglist to save to database
                notuniqueTaglist = notuniqueTaglist.Concat(videotags).Concat(mainkeys).ToArray();

                /*
                Before we used to auto like everyones post now we dont.
                 * 
                //create a new Userpagerelation and rating
                UserPageRelation UPR = new UserPageRelation();
                UPR.Page_id = pageid;
                //Rate is Rating values -1 , 0 , 1 .. Hate, Neutral, Love
                UPR.Rate = 1;
                userrepository.AddRelation(UPR);
                 
                */


                //To Make TagList have only unique values, to avoid the page being tagged more than once
                List<string> uniquetags = notuniqueTaglist.Distinct().ToList<string>();
                string[] Taglist = uniquetags.ToArray();


                //Add each tag in the array to the database
                foreach (String tag in Taglist)
                {
                    if (!string.IsNullOrEmpty(tag.Trim()))
                    {
                        tag Tag = new tag();
                        Tag.name = tag.ToLower().TrimEnd().TrimStart();
                        tagRepository.Add(Tag);
                        tagRepository.Save();
                    }
                }
                
                //tag the people in the post
                People = People.Trim();
                if (!string.IsNullOrEmpty(People))
                {
                    string[] usernames = People.Split(',');
                    foreach (string u in usernames)
                    {
                        userrepository.AddPostUserTag(u, postid);
                    }
                }

                //this function adds the tags to the users interest
                userrepository.AddNewUserInterest(pageid);

            }

            }

            else if (notyoutubeerror)
            {
                TempData["error"] = ("Currently we only support music from youtube. Please enter a youtube video url.");
                error = true;
            }
            else if (notmusicerror)
            {
                TempData["error"] = ("It seems like this video is not music.");
                error = true;
            }
            else if (urlerror)
            {
                TempData["error"] = ("There is something wrong with the url you entered.");
                error = true;
            }
            else if (genreerror)
            {
                TempData["error"] = ("Please pick a genre.");
                error = true;
            }
            else
            {
                TempData["error"] = ("Please add at least one tag.");
                error = true;
            }


            if (!error)
            {
                TempData["error"] = "none";
            }

            return postid;
        }


        //could need more work
        public string CleanTags(string metatags)
        {
            if (string.IsNullOrEmpty(metatags) == false)
            {
                metatags = metatags.Replace(' ', ',').Replace("...", "");

                //gets a maximum of 10 meta tags only
                metatags = metatags.Remove(0, metatags.NthIndexOf(",", 10) + 1);

            }
            return metatags;
        }


        public string shave(int length, string text)
        {
            //-4 because because 3 for ... and one because index starts from 0
            text = text.Remove(length - 4);
            int lastspace = text.LastIndexOf(' ');
            text = text.Remove(lastspace + 1);
            text = text + "...";
            return text;
        }


    }
}
