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

namespace TagThis.Controllers
{
    public class lightboxController : Controller
    {
        //
        // GET: /lightbox/
        String url="";
        String Title="";
        String Desc="";

        [iAuthenticate(LoginPage = "~/Account/wLogOn")]
        //[Authorize]
            /*
        public ActionResult Index(string u)
        {
            if (!string.IsNullOrEmpty(u))
            {
                url = HttpUtility.UrlDecode(u);
                //try to Get the website information

                    Spider spider = new Spider();
                    url = spider.CheckURL(url);
                    try
                    {
                        spider.Seturl(url);
                    }
                    catch
                    {
                        ModelState.AddModelError("Pageload", "There was an error loading the given url");
                    }
                    Title = spider.GetTitle();
                    Desc = spider.GetDescription();
                    Desc = RemoveNastyness(Desc);
                    Title = RemoveNastyness(Title);
                    ViewData["IconUrl"] = "http://www.google.com/s2/favicons?domain="+spider.cleanURL(url);
                if(Title == null)
                {
                    ModelState.AddModelError("Pageload", "There was an error loading the given url");
                }
                if (Desc == null)
                { 
              //what happens if there is no description
                }
                if (ModelState.IsValid)
                {
                    TagRepository tagRepository = new TagRepository();
                    UserRepository userRepository = new UserRepository();
                    int Pid;
                    //Get the page id
                    Pid = tagRepository.CheckPage(url);
                    if (Pid != -1)
                    {
                        //This page is not new
                        ViewData["NewPage"] = "0";
                       //send rating to the view
                        ViewData["oRate"] = userRepository.GetRating(Pid).ToString();
                        //send tags already added by the user to the vieew
                        String[] tagnames = tagRepository.GetUserTags(Pid);
                        ViewData["oTags"] = tagnames;
                    }
                    else
                    {
                        //Page does not exist everything is set to default
                        ViewData["oRate"] = "0";
                        ViewData["NewPage"] = "1";
                    }
                    //Send the information to the view
                    ViewData["Title"] = Title;
                    ViewData["Desc"] = Desc;
                    ViewData["url"] = url;
                    return View("Index");
                }
                else { return View("Error"); }
                
            }
            else 
            {
                return View("Error"); 
            }
        }
             */

        //a function to try to remove illegal characters from text
        public string RemoveNastyness(string text)
        {
            text = Regex.Replace(text, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            text = Regex.Replace(text, "<.*?>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            //text = Regex.Replace(text, "&#.*?;", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            //text = Regex.Replace(text, @"&nbsp;", " ", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            //text = Regex.Replace(text, @"&quot;", " ", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            //Un comment if you want to disable the potentially threat code error but make sure to remove html.encode from the view not to encode things twice
            //text = HttpUtility.HtmlEncode(text);
            return text;
        }


       public void Test()
        {
            //ProfileBase pb = ProfileBase.Create("mohamed@live.com");
            //Response.Write(pb.GetPropertyValue("FirstName").ToString());
            Spider spider = new Spider();
            spider.Seturl("http://www.last.fm");
            Response.Write(spider.GetIconUrl());

        }
       public void Details()
       {
           TagThis.Models.TagThisDataContext db = new TagThis.Models.TagThisDataContext();
           var tags = from p in db.tagmaps
                      where p.page_id == 1
                      select new
                      {
                          tid = p.tag_id
                      };
           Response.Write("Tags:");
           foreach (var tag in tags)
           {
               TagThis.Models.tag tagn = db.tags.Single(x => x.tag_id == tag.tid);
               Response.Write("<a href=#>");
               Response.Write(tagn.name.ToString());
               Response.Write("</a>");
               Response.Write(", ");
           }
       }

      /* maybe replace with get widget loves kickasses
        [Authorize]
       public ActionResult faves()
       {
           SearchController search = new SearchController();
           OmniResults R = search.GetWidgetFaves("!all", "newest", "all");
           ViewData["results"] = R;
           return View();
       }

       [Authorize,AcceptVerbs(HttpVerbs.Post)]
       public ActionResult faves(string favebox)
       {
           if (Request.IsAjaxRequest() == true & string.IsNullOrEmpty(favebox) == false)
           {
               string Query = favebox.Replace(',', '+');
               //encode the url and then remove % and sub with _
               string encoded = HttpUtility.UrlEncode(Query);
               encoded = encoded.Replace('%', '_');
               SearchController search = new SearchController();
               OmniResults R = search.GetWidgetFaves(encoded, "newest", "all");
               ViewData["results"] = R;
               return PartialView("LBfaves");
           }
           else
           {
               return null;
           }
       }
       
       */

       public ActionResult Finish(List<string> Messages) 
       {
           ViewData["Messages"] = TempData["Messages"];
           return View();
       }

       public void links()
       {
           /* links:::
           HtmlWeb htmlWeb = new HtmlWeb();
           HtmlDocument doc = htmlWeb.Load("http://www.bbc.co.uk/");
           HtmlNodeCollection links =
               doc.DocumentNode.SelectNodes("//a[@href]");

           foreach (HtmlNode link in links)
           {
               Response.Write(link.Attributes["href"].Value + "<br>");
           }*/
          
           Spider spider = new Spider();
           Response.Write(spider.GetDescription());
       }

       //Looks for the last space before the specific length and removes anything after that and adds ...
       public string shave(int length, string text)
       {
           //-4 because because 3 for ... and one because index starts from 0
           text = text.Remove(length-4);
           int lastspace = text.LastIndexOf(' ');
           text = text.Remove(lastspace + 1);
           text = text + "...";
           return text;
       }
        [iAuthenticate(LoginPage = "~/Account/wLogOn"),AcceptVerbs(HttpVerbs.Post)]
       public ActionResult Index(FormCollection formValues)
       {
            //make sure that none of them exceed the limit..
           string Description = HttpUtility.HtmlDecode(formValues["Desc"].ToString());
           string Title = formValues["Title"].ToString();
           if (Title.Length >= 96) { Title = shave(100, Title); }
           if (Description.Length >= 252) { Description = shave(256, Description); }
           //The array that will contain the success messages
           List<string> messages = new List<string>();
           WebPage Page = new WebPage();
           //get the information from the view
           Spider spider = new Spider();
           Page.url = spider.CheckURL(formValues["url"].ToString()).ToLower();
           Page.description = Description;
           Page.name = Title;
           //split tags into an Array
           String Formtags = formValues["Tags"];
           if (!string.IsNullOrEmpty(Formtags))
           {
               //trim end and start to ensure that no sneaky person tries to add a tag twice
               string[] notuniqueTaglist = Formtags.Split(',');
               TagRepository tagRepository = new TagRepository();
               //Check if the current page exists in the database otherwise insert it and get its page id
               int pageid = tagRepository.Add(Page);
               //create a new Userpagerelation and Add the favorites and ratings of the user
               UserRepository userrepository = new UserRepository();
               UserPageRelation UPR = new UserPageRelation();
               // check if there is a need to add a new user relation by seeing if original user openion is diff than the submitted
               bool addUPR = false;
               UPR.Page_id = pageid;
               //Rate is Rating values -1 , 0 , 1 .. Hate, Neutral, Love
               UPR.Rate = short.Parse(formValues["oRate"]); // to make sure that no changes happen on edit
               if (formValues["Rate"] != formValues["oRate"])
               {
                   if (formValues["Rate"] == "1") { UPR.Rate = 1; addUPR = true; messages.Add("Rating changed to loved."); }
                   if (formValues["Rate"] == "0") { UPR.Rate = 0; addUPR = true; messages.Add("Rating changed to neutral."); }
                   if (formValues["Rate"] == "-1") { UPR.Rate = -1; addUPR = true; messages.Add("Rating changed to hated."); }
               }
               if (addUPR) { userrepository.AddRelation(UPR); }


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
               if (tagsadded) { messages.Add("The following tags has been Added: " + tagsAdded.Substring(2)); }
               if (tagsdeleted) { messages.Add("The following tags has been deleted: " + tagsDeleted.Substring(2)); }
               TempData["Messages"] = messages;
               return RedirectToAction("Finish");
           }
           else 
           {
               ViewData["error"] = "Please add at least one tag.";
               ViewData["oFav"] = formValues["Fav"];
               ViewData["oRate"] = formValues["Rate"];
               ViewData["NewPage"] = formValues["Rate"];
               ViewData["Title"] = formValues["Title"];
               ViewData["Desc"] = formValues["Desc"];
               ViewData["url"] = formValues["url"];
               ViewData["IconUrl"] = formValues["IconUrl"];
               return View("Index");
           }
       }
    }
}
