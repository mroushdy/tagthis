using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TagThis.Models;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Net;
using System.Xml.Linq;
using WindowsLive;
using System.Xml;
using System.IO;

namespace TagThis.Controllers
{
    public class InvitationsController : Controller
    {
        private TagThisDataContext db = new TagThisDataContext();
        string InvitationText = "Hi,//n//nWe are pleased to inform you that you are one of the first people to use TagThis!//n//nAs a chosen one, you will be able to take advantage of the new search features TagThis has to offer.//n//nPlease bear in mind that TagThis is still in its infancy and is relying on your input for it to grow.//n//nGo ahead and use this link to register://n%%link%%//n//nIf you find any creepy crawlies, please report them for extermination at bugs@tagthis.com.//n//n Happy tagging!//nThe TagThis team.";
        public System.Timers.Timer timer = new System.Timers.Timer();
        //start live msn

        //Comma-delimited list of offers to be used.
        const string Offers = "Contacts.View";
        //Name of cookie to use to cache the consent token. 
        const string LoginCookie = "webauthtoken";

        // Initialize the WindowsLiveLogin module.
        static WindowsLiveLogin wll = new WindowsLiveLogin(true);

        protected static string AppId = wll.AppId;
        protected string UserId;

        protected WindowsLiveLogin.ConsentToken CToken;
        protected string ConsentUrl;

        //Landing pages to use after processing login and logout respectively.
        const string LoginPage = "/invitations/live";
        const string LogoutPage = LoginPage;
        //Name of cookie to use to cache the user token obtained through Web Authentication.
        static DateTime ExpireCookie = DateTime.Now.AddYears(-10);
        static DateTime PersistCookie = DateTime.Now.AddYears(10);

        
        
        //
        // GET: /Invitations/
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Site(string Email)
        {
            string email = Email;
            string message;

            string color = "red";

            AccountController Ac = new AccountController();
            UserRepository upr = new UserRepository();
            if (Ac.isEmail(email) == true && string.IsNullOrEmpty(Email)==false)
            {
                bool successfull = upr.invite(email);
                if (!successfull) { message = "Your email has already been submitted."; }
                else {
                    color = "white";
                    message = "Thanks, you will hear from us as soon as we start letting people in."; }
            }
            else { message= "The e-mail address provided is invalid."; }

            return Content("<span style='margin-top:4px; color:"+color+";'>"+message+"</span>");
        }

        [Authorize]
        public ActionResult index()
        {
                return View();
        }

        [Authorize, AcceptVerbs(HttpVerbs.Post)]
        public ActionResult index(string emails)
        {
            if (!string.IsNullOrEmpty(emails))
            {
                AccountController ac = new AccountController();
                List<MailContact> mcl = new List<MailContact>();
                string[] ems = emails.Split(',');
                foreach (string e in ems)
                {
                    if (ac.isEmail(e))
                    {
                        MailContact mc = new MailContact();
                        mc.Email = e;
                        mcl.Add(mc);
                        UserRepository upr = new UserRepository();
                        upr.PersonalInvite(e);
                    }
                }
                ViewData["contacts"] = mcl;
                return View("EmailInvites");
            }
            else 
            {
                ModelState.AddModelError("emls", "Enter email addresses in the box above.");
                return View("index");
            }
        }
        [Authorize]
        public ActionResult facebook()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult gmail()
        {
            return View();
        }


        [Authorize, AcceptVerbs(HttpVerbs.Post)]
        public ActionResult gmail(string email, string password)
        {
            if(string.IsNullOrEmpty(email)){ModelState.AddModelError("email","Please enter your email.");}
            if(string.IsNullOrEmpty(password)){ModelState.AddModelError("password","Please enter your password.");}
            try
            {
            Contacts co = new Contacts();
            List<MailContact> mc = co.Gmail(email, password).ToList<MailContact>();
            ViewData["contacts"] = mc;
            }
            catch
            {
                ModelState.AddModelError("signin", "The username or password you entered is incorrect.");
            }
            if (ModelState.IsValid)
            {
                return View("EmailInvites");
            }
            else
            {
                return View();
            }
        }

        [Authorize, AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InviteSingleEmail(string email, string name, FormCollection formValues)
        {
            if (Request.IsAjaxRequest() == true & string.IsNullOrEmpty(email) == false)
            {
                email = HttpUtility.UrlDecode(email);
                UserRepository upr = new UserRepository();
                upr.PersonalInvite(email);
                if (!string.IsNullOrEmpty(name))
                {
                    name = HttpUtility.UrlDecode(name);
                    if (email.StartsWith(name)) { name = ""; }
                }
                ViewData["email"] = email;
                return PartialView("InviteButton");
            }
            else
            {
                return null;
            }


        }

        public void LiveHandler()
        {

            HttpRequest req = System.Web.HttpContext.Current.Request;
            HttpResponse res = System.Web.HttpContext.Current.Response;
            HttpApplicationState app = System.Web.HttpContext.Current.Application;

        // Extract the 'action' parameter, if any, from the request.
        string action = req["action"];

        /*
            If action is 'logout', clear the login cookie and redirect to the
            logout page.

            If action is 'clearcookie', clear the login cookie and return a GIF
            as a response to signify success.

            If action is 'login', try to process sign-in. If the sign-in is  
            successful, cache the user token in a cookie and redirect to the  
            site's main page. If sign-in failed, clear the cookie and redirect 
            to the main page.

            If action is 'delauth', get user token from the cookie. Process the 
            consent token. If the consent token is valid, store the raw consent 
            token in persistent storage. Redirect to the site's main page.
        */

        if (action == "logout")
        {
            HttpCookie loginCookie = new HttpCookie(LoginCookie);
            loginCookie.Expires = ExpireCookie;
            res.Cookies.Add(loginCookie);
            res.Redirect(LogoutPage);
            res.End();
        } 
        else if (action == "clearcookie")
        {
            HttpCookie loginCookie = new HttpCookie(LoginCookie);
            loginCookie.Expires = ExpireCookie;
            res.Cookies.Add(loginCookie);

            string type;
            byte[] content;
            wll.GetClearCookieResponse(out type, out content);
            res.ContentType = type;
            res.OutputStream.Write(content, 0, content.Length);

            res.End();
        } 
        else if (action == "login")
        {
            HttpCookie loginCookie = new HttpCookie(LoginCookie);

            WindowsLiveLogin.User user = wll.ProcessLogin(req.Form);

            if (user != null)
            {
                loginCookie.Value = user.Token;

                if (user.UsePersistentCookie)
                {
                    loginCookie.Expires = PersistCookie;
                }
            } 
            else 
            {
                loginCookie.Expires = ExpireCookie;
            }   

            res.Cookies.Add(loginCookie);
            if (user.Context != null && user.Context.Length > 0)
               res.Redirect(user.Context);
            else
               res.Redirect(LoginPage);
            res.End();
        }
        else if (action == "delauth")
        {
            HttpCookie loginCookie = req.Cookies[LoginCookie];

            if (loginCookie != null)
            {
                string token = loginCookie.Value;

                WindowsLiveLogin.User user = wll.ProcessToken(token);

                if (user != null)
                {
                   WindowsLiveLogin.ConsentToken ct = wll.ProcessConsent(req.Form);
                   if ((ct != null) && ct.IsValid())
                   {
                      app[user.Id] = ct.Token;
                   }
                   if (ct.Context != null && ct.Context.Length > 0)
                      res.Redirect(ct.Context);
                   else
                      res.Redirect(LoginPage);
                }
                else
                   res.Redirect(LoginPage);
            }

            res.End();
        }
        else 
        {
            res.Redirect(LoginPage);
            res.End();
        }
    }

        public ActionResult Live()
        {
            try
            {
                // If the user token obtained from sign-in through Web Authentication 
                // has been cached in a site cookie, attempt to process it and extract 
                // the user ID.\

                System.Web.HttpRequest req = System.Web.HttpContext.Current.Request;
                System.Web.HttpApplicationState app = System.Web.HttpContext.Current.Application;

                System.Web.HttpCookie loginCookie = req.Cookies[LoginCookie];

                if (loginCookie == null)
                {
                    //Redirect user to windows live login if they are not authenticated
                    //this is the iframe
                    //Response.Redirect("http://login.live.com/controls/WebAuth.htm?appid=" + AppId + "&context=" + Server.UrlEncode("automate_contacts.aspx"));
                    Response.Redirect("http://login.live.com/wlogin.srf?appid=" + AppId + "&alg=wsignin1.0&appctx=" + Server.UrlEncode("/Invitations/LiveHandler"));
                    Response.End();
                }
                else
                {
                    string token = loginCookie.Value;

                    WindowsLiveLogin.User user = wll.ProcessToken(token);

                    // If the user ID is obtained successfully, prepare the message 
                    // to include the consent URL if a valid token is not present in the 
                    // persistent store; otherwise display the contents of the token.
                    if (user != null)
                    {
                        UserId = user.Id;
                        // Attempt to get the raw consent token from persistent store for the 
                        // current user ID.
                        string cts = (string)app[UserId];
                        WindowsLiveLogin.ConsentToken ct = wll.ProcessConsentToken(cts);

                        // If a consent token is found and is stale, try to refresh it and store  
                        // it in persistent storage.
                        if (ct != null)
                        {
                            if (!ct.IsValid())
                            {
                                if (ct.Refresh() && ct.IsValid())
                                {
                                    app[user.Id] = ct.Token;
                                }
                            }

                            if (ct.IsValid())
                            {
                                CToken = ct;
                            }
                        }
                    }
                }

                if (CToken == null)
                {
                    //redirect user to get the consent token         
                    Response.Redirect(wll.GetConsentUrl(Offers, Server.UrlEncode("/Invitations/LiveHandler")));
                    Response.End();
                }
                else
                {
                    // Construct the request URI.
                    string uri = "https://livecontacts.services.live.com/@L@" + CToken.LocationID + "/rest/LiveContacts/Contacts/";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.UserAgent = "Windows Live Data Interactive SDK";
                    request.ContentType = "application/xml; charset=utf-8";
                    request.Method = "GET";

                    // Add the delegation token to a request header.
                    request.Headers.Add("Authorization", "DelegatedToken dt=\"" + CToken.DelegationToken + "\"");

                    //Issue the HTTP GET request to Windows Live Contacts.
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    //The response body is an XML stream. Read the stream into an XmlDocument.
                    XDocument contacts = XDocument.Parse(new StreamReader(response.GetResponseStream()).ReadToEnd());
                    XmlDocument ms = new XmlDocument();
                    List<MailContact> x = (from c in contacts.Descendants("Contact") select new MailContact { Email = (string)(c.Element("Emails").Element("Email").Element("Address")), Name = (string)(c.Element("Profiles").Element("Personal").Element("DisplayName")) }).ToList<MailContact>();
                    //Use the document. For example, display contacts.InnerXml.
                    ViewData["contacts"] = x;
                    //Close the response.
                    response.Close();
                }
                return View("EmailInvites");
            }
            catch
            {
                int i = 0;
                while (i < 10000000) { i++; }
                return RedirectToAction("Live");
            }
        }

        public void check(string id)
        {
            utils u = new utils();
            var result = (from i in db.Invitations where i.ID.ToString() ==id select i).SingleOrDefault();
            InvitationText = InvitationText.Replace("//n", System.Environment.NewLine);
            InvitationText = InvitationText.Replace("%%link%%", "http://www.tagthis.com/account/register/"+result.ID.ToString());
            result.Checked = true;
            db.SubmitChanges();
            string subject = "You can now join TagThis!";
            string body = InvitationText;
            u.SendEmail("noreply@tagthis.com", result.email, subject, body,"TagThis",false);
            Response.Write("Success");
        }

        public void delete(string id)
        {
            var result = (from i in db.Invitations where i.ID.ToString() == id select i).SingleOrDefault();
            db.Invitations.DeleteOnSubmit(result);
            db.SubmitChanges();
            Response.Write("deleted");
        }

        public void checkall()
        {
            utils u = new utils();
            var result = (from i in db.Invitations where i.Checked == false select i);
            foreach (var r in result)
            {
                r.Checked = true;
                db.SubmitChanges();
                InvitationText = InvitationText.Replace("//n", System.Environment.NewLine);
                InvitationText = InvitationText.Replace("%%link%%", "http://www.tagthis.com/account/register/" + r.ID.ToString());
                string subject = "You can now join TagThis!";
                string body = InvitationText;
                u.SendEmail("noreply@tagthis.com", r.email, subject, body,"TagThis",false);
            }
            Response.Write("Success");
        }

    }
}
