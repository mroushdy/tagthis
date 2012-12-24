using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Profile;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;
using TagThis.ActionFilters;
using System.Net.Mail;
using Facebook.Web;
using System.Dynamic;
using System.Threading;
using System.Xml.Linq;
using System.Runtime.InteropServices; 

namespace TagThis.Controllers
{

    
    [HandleError]
    public class AccountController : Controller
    {


        static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }


        static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        // function gets all the links shared on the users facebook wall and performs some fixes
        public void ProcessFacebookLinks()
        {

                string FbToken = TempData["FbToken"].ToString();

                var client = new Facebook.FacebookClient(FbToken);

                string FBtoken = client.AccessToken;

                dynamic me = client.Get("me");

                string FBid = me.id;

                var query = "SELECT owner_comment, title, summary, picture, url, created_time FROM link where owner= " + FBid.ToString();


                dynamic results = client.Query(query);



                foreach (dynamic link in results)
                {
                    if (string.IsNullOrEmpty(link.url) == false)
                    {
                        if (link.url.ToLower().Contains("youtube.com"))
                        {
                            
                            string comment = link.owner_comment;
                            string title = link.title;
                            string summary = link.summary;
                            string picture = link.picture;
                            string url = link.url;
                            DateTime date = ConvertFromUnixTimestamp((double)link.created_time);

                            //gets the actual url of the thumbnail from facebook
                            if (string.IsNullOrEmpty(picture) == false)
                            {
                                link.picture = HttpUtility.UrlDecode(picture.Remove(0, picture.IndexOf("url=") + 4));
                            }


                            if (string.IsNullOrEmpty(url) == false)
                            {
                                // youtube api fix
                                if (url.ToLower().Contains("gdata.youtube.com"))
                                {
                                    string videoid = url.Remove(0, url.NthIndexOf("/", 6) + 1);
                                    link.url = "http://www.youtube.com/watch?v=" + videoid;
                                }
                                //try
                                //{
                                    AddController AddCont = new AddController();
                                    int id = AddCont.AddPage(" ", url, comment, "#camefromfb", null,"",date);

                                //}
                                //catch { }
                            }


                        }
                    }
                }
        }


        // This constructor is used by the MVC framework to instantiate the controller using
        // the default forms authentication and membership providers.
        private TagThis.Models.TagThisDataContext db = new TagThis.Models.TagThisDataContext();
        public utils utils = new utils();
        public AccountController()
            : this(null, null)
        {
        }

        // This constructor is not used by the MVC framework but is instead provided for ease
        // of unit testing this type. See the comments at the end of this file for more
        // information.
        public AccountController(IFormsAuthentication formsAuth, IMembershipService service)
        {
            FormsAuth = formsAuth ?? new FormsAuthenticationService();
            MembershipService = service ?? new AccountMembershipService();
        }

        public IFormsAuthentication FormsAuth
        {
            get;
            private set;
        }

        public IMembershipService MembershipService
        {
            get;
            private set;
        }

        public ActionResult LogOn()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
        
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LogOn(string userName, string password, bool rememberMe, string ReturnUrl)
        {
            //allows login by both username or email
            if (isEmail(userName))
            {
                userName = Membership.GetUserNameByEmail(userName);
            }

            if (!ValidateLogOn(userName, password))
            {
                return View();
            }
            FormsAuth.SignIn(userName, rememberMe);
                if (!String.IsNullOrEmpty(ReturnUrl))
            {
                ReturnUrl = HttpUtility.UrlDecode(ReturnUrl);
                return Redirect(ReturnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult wLogOn()
        {

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult wLogOn(string userName, string password, bool rememberMe, string returnUrl)
        {
            //allows login by both username or email
            if (isEmail(userName))
            {
                userName = Membership.GetUserNameByEmail(userName);
            }

            if (!ValidateLogOn(userName, password))
            {
                return View();
            }

            FormsAuth.SignIn(userName, rememberMe);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public void LogOff()
        {
            FormsAuth.SignOut();
            string token = FacebookWebContext.Current.AccessToken;
            FacebookWebContext.Current.DeleteAuthCookie();
            Response.Redirect("https://www.facebook.com/logout.php?next=" + Url.Encode(Url.Action("Index", "Home", null, "http")) + "&access_token=" + token);
        }


        //this is when the user is invited using facebook --- string id is the Userid who sent the invitation
        //this could be used later to limit the number of invitations
        public ActionResult FBSignup(string id)
        {
            if (FacebookWebContext.Current.IsAuthenticated())
            {
                return RedirectToAction("GetFb", new { id = id });
            }
            
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToRoute(new { action = "Index", controller = "Home" });
            }

            if (!string.IsNullOrEmpty(id))
            {
                //currently it only tests if the invitors guid exists but 
                //later you can check for how many invites are left
                Guid gid = new Guid(id);
                var m = Membership.GetUser(gid);
                
                if (m == null)
                {
                    return View("UnSuccessfull");
                }
                else
                {
                    //no email because this user was invited by a facebook message.
                    ViewData["invitetype"] = "facebook";
                    ViewData["Iid"] = id;
                    return View("Signup");
                }
            }
            else
            {
                return View("UnSuccessfull");
            }
        }
        
        //this controller is when the user is invited via email
        public ActionResult Signup()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToRoute(new { action = "Index", controller = "Home" });
            }
            else
            {
                return View();
            }
            /*
            TagThis.Models.UserRepository upr = new TagThis.Models.UserRepository();
            if (!string.IsNullOrEmpty(id))
            {
                string email = upr.GetEmailFromInvitation(id);
                if (email == null | email == "notchecked")
                {
                    return View("UnSuccessfull");
                }
                else
                {
                    ViewData["invitetype"] = "email";
                    ViewData["email"] = email;
                    ViewData["Iid"] = id;
                    return View();
                }
            }
            else
            {
                return View("UnSuccessfull");
            }
             */

        }


        public ActionResult GetFb()
        {
            if (Facebook.Web.FacebookWebContext.Current.IsAuthenticated())
            {
                string id = ""; //id of the user who invited the current user.

                var client = new Facebook.Web.FacebookWebClient();

                string FBtoken = client.AccessToken;

                dynamic me = client.Get("me");

                string Name = me.name;

                string FBid = me.id;

                string FBemail = me.email;

                TagThis.Models.UserRepository ur = new Models.UserRepository();

                string check = ur.CheckIfFacebookIsConnected(FBid);
                if (string.IsNullOrEmpty(check))
                {
                    return RedirectToAction("Register", new { id = id, FBid = FBid, FBtoken = FBtoken, Name = Name, FBemail = FBemail });
                }
                else
                {
                    FormsAuth.SignIn(check, true);
                    return RedirectToAction("", "Home", new { });
                }
            }
            else
            {
                return RedirectToAction("", "Home", new { });
            }
        }

        //this is when the user tries to connect to facebook but he is already registered
        public ActionResult AlreadyConnected(string FBid, string UserName)
        {
            ViewData["Fbid"] = FBid;
            ViewData["UserName"] = UserName;
            return View();
        }


        /* this was for the DOB which is not included anymore
//populate the DOB DD/MM/YYYY
List<int> years= new List<int>();
List<int> days = new List<int>();
IEnumerable<string> months;
int year = DateTime.Now.Year;
for (int y = year - 13; y > year + 13 - 100; y--) { years.Add(y); }
for (int d =  1; d <= 31; d++) { days.Add(d); }
months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.AsEnumerable().Take(12);
ViewData["Years"] = new SelectList(years.AsEnumerable<int>());
ViewData["Days"] = new SelectList(days.AsEnumerable<int>());
ViewData["Months"] = new SelectList(months);
ViewData["Countries"] = new SelectList(utils.GetCountries(),"code","name");
*/

        public ActionResult Register(string id,string FBemail, string Name, string FBid,string FBtoken)
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToRoute(new {action="Index", controller="Home" });
            }

            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            
            
            string email = "";
            id = "";
            //comment above was removed from here

            //invitations start
            TagThis.Models.UserRepository upr = new TagThis.Models.UserRepository();
          //  if (!string.IsNullOrEmpty(id))
          //  {
          //      email =  upr.GetEmailFromInvitation(id);
          //      if (email != null)                                        //the highlighted code is when using email to invite instead of fb
          //      {
                 //do nothing because email is still email
          //      }
                if (string.IsNullOrEmpty(FBemail) == false)
                {
                    email = FBemail;
                }
                else
                {
                    //this meanse that the facebook account does not have an email
                    //and the user was not invited via email
                    //this could be solved by putting a textbox and allowing email verification
                    //but keep in mind if this is the case then you must put a captcha because facebook signup is what prevented robots.
                    return View("UnSuccessfull");
                }

                //Pass the facebook data to view
                ViewData["FBid"] = FBid;
                ViewData["Fbtoken"] = FBtoken;
                ViewData["Name"] = Name;

                ViewData["email"] = email;
                ViewData["Iid"] = id;
                return View();

            //}
            //else
            //{
            //    return View("UnSuccessfull");
            //}
            //invitations end
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(string userName,string email ,string password, string confirmPassword,string Name, string Iid, string FBid,string FBtoken,bool? FBautopost)
        {
            //set default to false
            FBautopost = FBautopost ?? true;

         
            //comment above was deleted from here

            //set some viewdata
            ViewData["Iid"] = Iid;
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            ViewData["email"] = email;

            ViewData["FBid"] = FBid;
            ViewData["FBtoken"] = FBtoken;
            ViewData["Name"] = Name;

            TagThis.Models.UserRepository upr = new TagThis.Models.UserRepository();

            
            /* email invitation specific
            string Iemail = upr.GetEmailFromInvitation(Iid);
            Guid gid = new Guid(Iid);
            var ugid = Membership.GetUser(gid);

            //check if the invitation id is correct. It must return an email or be a valid used id.
            if (Iemail == null && ugid == null)
            {
                //user was not invited and is cheating somehow
                return View("UnSuccessfull");
                
            }
            */

            if (ValidateRegistration(userName, email, password, confirmPassword, Name))
            {
                    // Attempt to register the user
                    MembershipCreateStatus createStatus = MembershipService.CreateUser(userName, password, email);
                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        
                        ProfileBase pb = ProfileBase.Create(userName);
                       
                        //converts name to title case to become capitalised
                        TextInfo myTI = new CultureInfo("en-US",false).TextInfo;
                        Name = myTI.ToTitleCase(Name.ToLower());
                        pb.SetPropertyValue("Name", Name);
                        pb.Save();

                        FormsAuth.SignIn(userName, false /* createPersistentCookie */);
                        
                        //create facebook table to add the facebook data
                        upr.UpdateUserData(Membership.GetUser(userName).ProviderUserKey.ToString(), FBtoken, FBid, FBautopost, userName, Name);



                        //delete invitations here
                        if (email != null) { upr.DeleteInvitation(email); }
                        else { /* user was not invited by email*/ }

                        return RedirectToAction("Welcome");

                        //return RedirectToAction("Welcome", new { Token = FBtoken , FBap = FBautopost});
                    }
                    else
                    {
                        ModelState.AddModelError("_FORM", ErrorCodeToString(createStatus));
                    }
             
            }

            // If we got this far, something failed, redisplay form
            return View();
        }


        [Authorize, AjaxViewSelector]
        public ActionResult Settings()
        {
            //gets the homepage view then opens the settings modal
            SearchController search = new SearchController();
            OmniResults R = search.GetViewContent("", 0, "popular", "now", "following");
            ViewData["results"] = R;
            return View();
        }


        //move it to ajax as it should be
        [Authorize, AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeSettings(string Email, bool? EmailFollows, bool? EmailReposts, bool? EmailLikes, bool? EmailComments, bool? EmailCommentsReply, bool? EmailCommentsLike, bool? FBautopost, bool? EmailUserPostTag)
        {
            Models.UserRepository ur = new Models.UserRepository();
            ur.UpdateUserSettings(EmailFollows ?? false, EmailReposts ?? false, EmailLikes ?? false, EmailComments ?? false, EmailCommentsLike ?? false, EmailCommentsReply ?? false, FBautopost ?? false, EmailUserPostTag ?? false);

            ChangeEmail(Email);

            //success return page element

            ViewData["error"] = TempData["error"];
            return PartialView("SettingsBox");
        }

        [Authorize]
        public ActionResult GetSettingBox()
        {
            return PartialView("SettingsBox");
        }


        [Authorize]
        public ActionResult DeleteAccount(string check)
        {
            if (check == Membership.GetUser().ProviderUserKey.ToString())
            {
                Models.UserRepository ur = new Models.UserRepository();
                ur.DeleteUser();
                Membership.DeleteUser(Membership.GetUser().UserName);
                LogOff();
            }

            return RedirectToAction("index","home");
        }


        /// captcha test
        public ActionResult test()
        {
            ViewData["check"] = "";
            return View();
        }

        [CaptchaValidator]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult test(string key, bool captchaValid)
        {
            //to check for valid captcha: if(captchaValid == true)

            ViewData["check"] = captchaValid.ToString();
            return View();
        }


        public void FollowFacebookFriends()
        {
            //follow all friends
            TagThis.Models.UserRepository ur = new TagThis.Models.UserRepository();
            var m = (from dp in db.UserDatas where dp.UserId.ToString() == ur.GetUserId().ToString() select dp).SingleOrDefault();
            var fb = new Facebook.FacebookClient(m.FBtoken);
            dynamic myInfo = fb.Get("/me/friends");
            foreach (dynamic friend in myInfo.data)
            {
                ur.FollowByFacebookId(friend.id.ToString());
            }
        }


        public void FollowSimilarPeople()
        {
            //follow all friends
            TagThis.Models.UserRepository ur = new TagThis.Models.UserRepository();
            var users = ur.GetSimilarUsers();
            IEnumerable<Guid> results;

            //the few lines below take a few random users from the top 25% match of list of similar users
            int count = users.Count();
            int take = 10;
            if (count > 60)
            {
                take = (int)(count / 4);
            }
            results = users.Take(take).AsEnumerable().OrderBy(n => Guid.NewGuid()).Take(10);

            foreach (Guid result in results)
            {
                ur.AddSubscription(result);
            }
        }


        [Authorize]
        public ActionResult Welcome()   //if you want to autopost or do things like this welcome(string Token, bool FBap)
        {
            /*
            //gets all the links shared on fb and adds them to kickass (runs in a new thread)
            if (string.IsNullOrEmpty(Token) == false)
            {
                TempData["FbToken"] = Token;
                Thread t = new Thread(ProcessFacebookLinks);
                t.IsBackground = true;
                t.Start();
            }

            //autopost to facebook
            if (FBap)
            {
                var client = new Facebook.FacebookClient(Token);
                dynamic parameters = new ExpandoObject();
                parameters.message = "I have just joined TagThis. Are you on TagThis?";
                parameters.link = "http://tagthis.com";
                parameters.picture = "http://www.tagthis.com/fblarge.png";
                parameters.name = "Had enough of your friends?";
                parameters.caption = "tagthis.com";
                parameters.description = "TagThis understands who you are and helps you discover awesome websites shared by random people just like you. The more you use it the smarter it gets the more awesome things you discover.";
                parameters.actions = new
                {
                    name = "Join me on TagThis",
                    link = "http://TagThis.com",
                };
                parameters.privacy = new
                {
                    value = "ALL_FRIENDS",
                };

                dynamic result = client.Post("me/feed", parameters);
            }
            */

            //follow facebook friends
            Thread t = new Thread(FollowFacebookFriends);
            t.IsBackground = true;
            t.Start();

            List<string> xmlGenres = new List<string>();
            XDocument xdoc = XDocument.Load(HttpContext.Server.MapPath("~/Content/genre-list.xml"));

            //Run query
            ViewData["genres"] = (from gk in xdoc.Descendants("GenreKey") select gk.Attribute("name").Value).ToList<string>();

            //send the welcome email
            Models.UserRepository ur = new Models.UserRepository();
            ur.SendWelcomeMessage();

            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Welcome(string genresbox)   //if you want to autopost or do things like this welcome(string Token, bool FBap)
        {
            Models.UserRepository ur = new Models.UserRepository();
            ur.AddInitialUserTags(genresbox);

            //follow 10 similar people
            FollowSimilarPeople();
            return RedirectToAction("Import","Account");
        }

        [Authorize]
        public ActionResult Import()   
        {
            return View();
        }

        [Authorize]
        public ActionResult DoImport()
        {
            TagThis.Models.UserRepository ur = new TagThis.Models.UserRepository();
            var m = (from dp in db.UserDatas where dp.UserId.ToString() == ur.GetUserId().ToString() select dp).SingleOrDefault();
            TempData["FbToken"] = m.FBtoken;

            HttpContext ctx = System.Web.HttpContext.Current;

            Thread t = new Thread(() =>
            {
                System.Web.HttpContext.Current = ctx;
                ProcessFacebookLinks();
            });
            t.IsBackground = true;
            t.Start();
            return RedirectToAction("People", "Account");
        }


        [Authorize]
        public ActionResult People()
        {
            TagThis.Models.UserRepository upr = new Models.UserRepository();
            string username = Membership.GetUser().UserName;
            List<Models.UserListItem> userlist = upr.GetFollowing(username);
            ViewData["userlist"] = userlist;
            ViewData["usercount"] = userlist.Count(); 
            return View();
        }


        [Authorize]
        public ActionResult ChangePassword()
        {

            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }


        public ActionResult Forgot()
        {
            return View();
        }

        // creates in the database a record of request to reset password and sends the user a link.
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Forgot(string Email)
        {
            string uname = Membership.GetUserNameByEmail(Email);
            if (!string.IsNullOrEmpty(uname))
            {
                Guid PrId = System.Guid.NewGuid();
                var result = (from pr in db.PasswordResets where pr.UserId == PrId select pr).SingleOrDefault();
                if (result != null)
                {
                    db.PasswordResets.DeleteOnSubmit(result);
                }
                db.SubmitChanges();
                TagThis.Models.PasswordReset passr = new TagThis.Models.PasswordReset();
                passr.ID = PrId;
                passr.UserId = (Guid)Membership.GetUser(uname).ProviderUserKey;
                passr.DateTime = System.DateTime.Now;
                db.PasswordResets.InsertOnSubmit(passr);
                db.SubmitChanges();
                utils u = new utils();
                string subject = "Your TagThis account";
                string body = "Hi,//n//nYou just asked Kickass to reset your password.//n//nClick on the following link to reset your password://nhttp://www.kickass.io/Account/ResetPassword/"+PrId.ToString()+"//n//nIf you didn't request this, don't worry, your password is still the same.//n//nKickass.io";
                body = body.Replace("//n",System.Environment.NewLine);
                u.SendEmail("noreply@kickass.io", Email, subject, body,"Kickass", false);
                ViewData["ForgotSuccess"] = "Please check your email.";
            }
            else 
            {
                ModelState.AddModelError("email", "We could not recognize the email you entered.");
            }
            return View();
        }

        //page to reset the password
        public ActionResult ResetPassword(string id)
        {
           var result = (from pr in db.PasswordResets where pr.ID.ToString() == id select pr).SingleOrDefault();
           if (result != null)
           {
               if (DateTime.Now.Subtract(result.DateTime).TotalHours > 2)
               {
                   db.PasswordResets.DeleteOnSubmit(result);
                   db.SubmitChanges();
                   return View("PassResetError");
               }
           }
           else
           {
               return View("PassResetError"); 
           }
           return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ResetPassword(string newPassword, string confirmPassword, string id)
        {
            var result = (from pr in db.PasswordResets where pr.ID.ToString() == id select pr).SingleOrDefault();
            if (result != null)
            {
                if (DateTime.Now.Subtract(result.DateTime).TotalHours > 2)
                {
                    return View("PassResetError");
                }
                else 
                {
                   string TempPass = Membership.Provider.ResetPassword(Membership.GetUser(result.UserId).UserName, null);
                    //change password
                   ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
                   if (!ValidateChangePassword(TempPass, newPassword, confirmPassword))
                   {
                       return View();
                   }

                   try
                   {
                       if (MembershipService.ChangePassword(Membership.GetUser(result.UserId).UserName, TempPass, newPassword))
                       {
                           db.PasswordResets.DeleteOnSubmit(result);
                           db.SubmitChanges();
                           return RedirectToAction("ChangePasswordSuccess");
                       }
                       else
                       {
                           ModelState.AddModelError("_FORM", "The new password is invalid.");
                           return View();
                       }
                   }
                   catch
                   {
                       ModelState.AddModelError("_FORM", "The new password is invalid.");
                       return View();
                   }
                }
            }
            else
            {
                return View("PassResetError");
            }
        }




        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Exceptions result in password not being changed.")]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {

            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            if (!ValidateChangePassword(currentPassword, newPassword, confirmPassword))
            {
                return View();
            }

            try
            {
                if (MembershipService.ChangePassword(User.Identity.Name, currentPassword, newPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("_FORM", "The current password is incorrect or the new password is invalid.");
                    return View();
                }
            }
            catch
            {
                ModelState.AddModelError("_FORM", "The current password is incorrect or the new password is invalid.");
                return View();
            }
        }


        [Authorize]
        public void ChangeEmail(string Email)
        {
            TempData["error"] = "none";
            if (string.IsNullOrEmpty(Email)){TempData["error"] = "You must specify an email address.";}
            else if (isEmail(Email))
            {
                if (Email.ToLower() == Membership.GetUser().Email.ToLower())
                { 
                //email not changed
                }
                else if (string.IsNullOrEmpty(Membership.GetUserNameByEmail(Email)))
                {
                    MembershipUser u = Membership.GetUser();
                    u.Email = Email;
                    Membership.UpdateUser(u);
                }
                else
                {
                    TempData["error"] = "A user for that e-mail address already exists. Please enter a different e-mail address.";
                }
            }
            else{TempData["error"] = "The e-mail address provided is invalid.";}

        }

        public ActionResult ChangeEmailSuccess()
        {

            return View();
        }

        public ActionResult ChangePasswordSuccess()
        {

            return View();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity is WindowsIdentity)
            {
                throw new InvalidOperationException("Windows authentication is not supported.");
            }
        }

        #region Validation Methods

        private bool ValidateChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (String.IsNullOrEmpty(currentPassword))
            {
                ModelState.AddModelError("currentPassword", "You must specify a current password.");
            }
            if (newPassword == null || newPassword.Length < MembershipService.MinPasswordLength)
            {
                ModelState.AddModelError("newPassword",
                    String.Format(CultureInfo.CurrentCulture,
                         "You must specify a new password of {0} or more characters.",
                         MembershipService.MinPasswordLength));
            }

            if (!String.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("_FORM", "The new password and confirmation password do not match.");
            }

            return ModelState.IsValid;
        }

        private bool ValidateLogOn(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("username", "You must specify a username.");
            }
            if (String.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "You must specify a password.");
            }
            if (!MembershipService.ValidateUser(userName, password))
            {
                ModelState.AddModelError("_FORM", "The username or password provided is incorrect.");
            }

            return ModelState.IsValid;
        }

        public bool isEmail(string inputEmail)
        {
            if (!string.IsNullOrEmpty(inputEmail))
            {
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                      @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                      @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(strRegex);
                if (re.IsMatch(inputEmail))
                    return (true);
                else
                    return (false);
            }
            else
                return true; // so that we dont show two errors.. invalid email and no email
        }

        private bool ValidateRegistration(string userName, string email, string password, string confirmPassword, string Name)
        {
            if (String.IsNullOrEmpty(userName))
            {
                ModelState.AddModelError("userName", "You must specify a username.");
            }
            if (Membership.GetUser(userName) != null)
            {
                ModelState.AddModelError("userName", "That username is taken.");
            }
            if (String.IsNullOrEmpty(Name))
            {
                ModelState.AddModelError("Name", "You must specify your full name.");
            }
            if (Name.Split(' ').Length < 2 || Name.Split(' ').Length > 4)
            {
                ModelState.AddModelError("Name", "Please check that this is your real full name.");
            }
            if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("email", "You must specify an email address.");
            }

            if (!isEmail(email))
            {
                ModelState.AddModelError("email", "This e-mail address is not valid.");
            }

            if (string.IsNullOrEmpty(Membership.GetUserNameByEmail(email)) == false)
            {
                ModelState.AddModelError("email", "This e-mail address is already regisered.");
            }

            if (password == null || password.Length < MembershipService.MinPasswordLength)
            {
                ModelState.AddModelError("password",
                    String.Format(CultureInfo.CurrentCulture,
                         "You must specify a password of {0} or more characters.",
                         MembershipService.MinPasswordLength));
            }
            if (!String.Equals(password, confirmPassword, StringComparison.Ordinal))
            {
                ModelState.AddModelError("confirmPassword", "The password and confirmation password do not match.");
            }
            return ModelState.IsValid;
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://msdn.microsoft.com/en-us/library/system.web.security.membershipcreatestatus.aspx for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "A user for that username address already exists. Please enter a different username.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }

    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IFormsAuthentication
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthentication
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }

    public class AccountMembershipService : IMembershipService
    {
        private MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
            return currentUser.ChangePassword(oldPassword, newPassword);
        }
    }
}
