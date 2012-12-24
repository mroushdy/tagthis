using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Security;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Web.Profile;
using System.IO;

namespace TagThis.Models
{
    public class CommentResult
    {
        public Comment comment { get; set; }
        public int? rating { get; set; }
        public string ProfileImage { get; set; }
    }

    public class UserListItem
    {
        public string username { get; set; }
        public string fullname { get; set; }
        public string profilepictureurl { get; set; }
    }

    public class NotificationResult
    {
        public string NotificationHTML { get; set; }
        public string OwnerImage { get; set; }
        public bool Read { get; set; }
        public string Time { get; set; }
        public string LinkAction { get; set; }
        public string LinkController { get; set; }
        public string LinkId { get; set; }
    }

    public class NotificationType
    {
        public int id { get; set; }
        public string type { get; set; }
        public string message { get; set; }
        public string LinkAction { get; set; }
        public string LinkController { get; set; }

    }

    public class NotificationTypesDB
    {

        public List<NotificationType> GetTypes()
        {
            List<NotificationType> db = new List<NotificationType>();

            //important note: to add a new notification type a lot of things need to happen. 1- add it here. 2- add it in the function sendnotificationsemail() 3- call the function addnotification() when the action happens 4- add a new bool to the userdata table in the database to control this notification email settings 5-update the checkboxes in the settings modal to include this notification


            NotificationType n1 = new NotificationType();
            n1.id = 1;
            n1.type = "postlike";
            n1.message = "liked your post.";
            n1.LinkAction = "";
            n1.LinkController = "Post";
            db.Add(n1);

            NotificationType n2 = new NotificationType();
            n2.id = 2;
            n2.type = "follow";
            n2.message = "followed you.";
            n2.LinkAction = "";
            n2.LinkController = "Users";
            db.Add(n2);

            NotificationType n3 = new NotificationType();
            n3.id = 3;
            n3.type = "postcomment";
            n3.message = "commented on your post.";
            n3.LinkAction = "";
            n3.LinkController = "Comments";
            db.Add(n3);

            NotificationType n4 = new NotificationType();
            n4.id = 4;
            n4.type = "commentlike";
            n4.message = "liked your comment.";
            n4.LinkAction = "";
            n4.LinkController = "Comments";
            db.Add(n4);


            NotificationType n5 = new NotificationType();
            n5.id = 5;
            n5.type = "postcommentreply";
            n5.message = "replied on a post you commented on.";
            n5.LinkAction = "";
            n5.LinkController = "Comments";
            db.Add(n5);

            NotificationType n6 = new NotificationType();
            n6.id = 6;
            n6.type = "postusertag";
            n6.message = "dropped a song on your dropbox.";
            n6.LinkAction = "";
            n6.LinkController = "Post";
            db.Add(n6);

            return db;
        }

    }



    public class UserRepository
    {
        private TagThisDataContext db = new TagThisDataContext();
        
        public Guid GetUserId() 
        {
            // get user id from database
            Guid userGuid = (Guid)(Membership.GetUser() == null ? Guid.Empty : (Guid?)Membership.GetUser().ProviderUserKey);
            return userGuid;    
        }

        public string GetProfileImage(Guid userid,string size)
        {
            var r = (from u in db.UserDatas where u.UserId.ToString() == userid.ToString() select u).SingleOrDefault();
            if (r != null)
            {
                //checks if there is a facebook id gets profile picture otherwise gets a default image.
                if (r.FBid != null)
                {
                    if (size == "mini")
                    {
                        return "http://graph.facebook.com/" + r.FBid + "/picture?type=square";
                    }
                    else if (size == "normal")
                    {
                        return "http://graph.facebook.com/" + r.FBid + "/picture?type=normal";
                    }
                    else //size == "large"
                    {
                        return "http://graph.facebook.com/" + r.FBid + "/picture?type=large";
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        
        public List<CommentResult> GetComments(int postid)
        {
            return (from c in db.Comments where c.postid == postid select new CommentResult { comment = c, rating = c.CommentRatings.Sum(r => r.rating), ProfileImage = GetProfileImage(c.userid,"mini") }).ToList<CommentResult>();
        }

        public IQueryable<Guid> GetPostLikes(int postid)
        {
            return (IQueryable<Guid>)(from l in db.UserPageRelations
                    where l.Post_id == postid && l.Rate == 1
                    select l.UserId);
        }

        public IQueryable<Guid> GetPostReposts(int postid)
        {
            return (IQueryable<Guid>)(from l in db.Posts
                                      where l.RepostedFrom_postid == postid
                                      select l.UserId);
        }


        //the effect of relevance will not be incorporated in the sorting
        public IQueryable<Guid> GetSimilarUsers()
        {
            string guid = GetUserId().ToString();
            return (IQueryable<Guid>)(from sme in db.interestmaps from ss in db.interestmaps where sme.userid.ToString() == guid && sme.tag_id == ss.tag_id group ss by new { ss.userid } into grp orderby grp.Count() descending select grp.Key.userid);
        }

        public string GetUserName()
        {
            return Membership.GetUser().UserName;
        }

        public List<UserListItem> GetFollowers(string userid) //userid is username
        {
            string guid = Membership.GetUser(userid).ProviderUserKey.ToString();
            return (from s in db.Subscriptions
                    where s.Subscribedto.ToString() == guid
                    let un = Membership.GetUser(s.Subscriber).UserName
                    select new UserListItem
                    {
                        username = un,
                        fullname = GetFullName(un),
                        profilepictureurl = GetProfileImage(s.Subscriber, "mini")
                    }).ToList<UserListItem>();
        }

        public List<UserListItem> GetFollowing(string userid)
        {
            string guid = Membership.GetUser(userid).ProviderUserKey.ToString();
            return (from s in db.Subscriptions
                    where s.Subscriber.ToString() == guid
                    let un = Membership.GetUser(s.Subscribedto).UserName
                    select new UserListItem
                    {
                        username = un,
                        fullname = GetFullName(un),
                        profilepictureurl = GetProfileImage(s.Subscribedto, "mini")
                    }).ToList<UserListItem>();
        }

        public string GetAccesToken()
        {
            string guid = Membership.GetUser().ProviderUserKey.ToString();
            return (from s in db.UserDatas
                    where s.UserId.ToString() == guid
                    select s.FBtoken).SingleOrDefault().ToString();
        }

        public void SetCommentRating(int commentid, int rating)
        {
            var Result = (from CR in db.CommentRatings where CR.comment_id == commentid && CR.userid == GetUserId() select CR).SingleOrDefault();
            if (Result != null)
            {
                if (Result.rating < 1 & rating == 1) { Result.rating++; }
                else if (Result.rating > -1 & rating == -1) { Result.rating--; }
               
            }
            else
            { 
                CommentRating cr = new CommentRating();
                cr.userid = GetUserId();
                cr.rating = rating;
                cr.comment_id = commentid;
                db.CommentRatings.InsertOnSubmit(cr);
            }
        }

        public int GetCommentRating(int commentid)
        {
            var result = (from cr in db.CommentRatings where cr.comment_id == commentid select cr.rating).ToList();
            if (result != null)
            { return result.Sum(r=>r); }
            else { return 0; }
        }


        public Comment GetComment(int commentid)
        {
            return (from c in db.Comments where c.id == commentid select c).SingleOrDefault();
        }

        public UserPageRelation GetRelation(int pageid, int postid)
        {
            //Get the relation witht the given page id
            var Result = (from upr in db.UserPageRelations
                             where upr.Page_id == pageid && upr.Post_id == postid && upr.UserId == GetUserId()
                             select upr).SingleOrDefault();
            return Result;
        }

        public int GetRating(int pageid, int postid)
        {
            //check if there is a row with the provided page
            var Result = GetRelation(pageid, postid);
            if (Result != null)
            { return Result.Rate; }
            else
            { return 0; }
        }

        public WebPage GetPage(int id)
        {
            return db.WebPages.SingleOrDefault(p => p.page_id == id);
        }

        public Guid GetPostOwner(int postid)
        {
            return db.Posts.SingleOrDefault(p => p.id == postid).UserId;
        }

        public void AddPostUserTag(string username, int postid)
        {
            Guid Taggedby = GetUserId();
            var user = Membership.GetUser(username);
            Post post = (Post)(from p in db.Posts where p.id == postid select p).SingleOrDefault();

            if(post != null && user != null)
            {
                PostUserTag put = new PostUserTag();
                put.Date = DateTime.Now;
                put.Userid = (Guid)user.ProviderUserKey;
                put.Taggedby = Taggedby;
                post.PostUserTags.Add(put);
                db.SubmitChanges();
                AddNotification(GetUserId(), put.Userid, "postusertag", postid.ToString());
            }

        }


        public void DeletePost(int postid)
        {
            var post = db.Posts.Where(p => p.id == postid).SingleOrDefault();
            if (GetUserId().ToString() == post.UserId.ToString())
            {
                //delete all the comments, tags, and rating on that specific post and remove the reposts foreign key but leaving the reposts
                db.UserPageRelations.DeleteAllOnSubmit(post.UserPageRelations);
                db.Comments.DeleteAllOnSubmit(post.Comments);
                db.PostUserTags.DeleteAllOnSubmit(post.PostUserTags);
                post.Posts.Clear();

                db.Posts.DeleteOnSubmit(post);
                db.SubmitChanges();
            }
        }

        public void DeleteComment(int commentid)
        {
            var comment = db.Comments.Where(c => c.id == commentid).SingleOrDefault();
            if (GetUserId().ToString() == comment.userid.ToString())
            {
                //delete all the comments, tags, and rating on that specific post and remove the reposts foreign key but leaving the reposts
                db.CommentRatings.DeleteAllOnSubmit(comment.CommentRatings);
                db.Comments.DeleteOnSubmit(comment);
                db.SubmitChanges();
            }
        }

        public Comment AddComment(Comment comment, int Pageid,int postid)
        {
           Guid userGuid = GetUserId();
           comment.userid = userGuid;
           comment.Name = Membership.GetUser().UserName;
           comment.date = DateTime.Now;
           comment.postid = postid;
           comment.Full_Name = GetFullName(comment.Name);
           WebPage Page = GetPage(Pageid);
           Page.Comments.Add(comment);
           db.SubmitChanges();
           return comment;
        }

        public void AddNotification(Guid Sender, Guid Receiver, string type, string objectid)
        {
            if (Sender.ToString() != Receiver.ToString())
            {
                notification n = new notification();
                n.is_read = false;
                n.object_id = objectid;
                n.recipient_id = Receiver;
                n.sender_id = Sender;
                n.time = DateTime.Now;

                NotificationTypesDB ntdb = new NotificationTypesDB();
                var m = ntdb.GetTypes().Where(t => t.type.ToLower() == type.ToLower()).SingleOrDefault();
                if (m != null)
                {
                    n.activity_id = m.id;
                    db.notifications.InsertOnSubmit(n);
                    db.SubmitChanges();
                }

                SendNotificationEmail(Receiver, type, objectid);
            }
        
        }

        public void ReadNotifications()
        {
            string uid = GetUserId().ToString();
            var u = (from n in db.notifications where n.is_read == false && n.recipient_id.ToString() == uid select n);
            foreach (var n in u)
            {
                n.is_read = true;
                n.date_read = DateTime.Now;
            }
            db.SubmitChanges();
        }

        public List<NotificationResult> GetNotifications()
        {
            TagThis.Controllers.utils u = new Controllers.utils();
            string uid = GetUserId().ToString();

            var notifications = (from n in db.notifications where n.recipient_id.ToString() == uid.ToString() group n by new { n.object_id, n.activity_id, n.is_read} into grp let latestdate = grp.OrderByDescending(g => g.time).FirstOrDefault().time orderby latestdate descending select grp).ToList();
            NotificationTypesDB ndb = new NotificationTypesDB();
            List<NotificationResult> result = new List<NotificationResult>();

            foreach (var n in notifications)
            {
                NotificationResult nr = new NotificationResult();
                
                //below gets the notification type && object type by the first element in the group since all the group elements must have the same type
                var nt = ndb.GetTypes().Where(t => t.id == n.FirstOrDefault().activity_id).SingleOrDefault();
                //nr.NotificationLink = nt.urlformat.Replace("{id}", n.FirstOrDefault().object_id.ToString());

                //Groups the notification by userid (to avoid duplicate notifications by same user on same post with same type) and arranges the results by date
                var UserIDs = (from not in n group not by not.sender_id into grp let latestdate = grp.OrderByDescending(g => g.time).FirstOrDefault().time orderby latestdate descending select grp.Key);

                nr.OwnerImage = GetProfileImage(UserIDs.ElementAtOrDefault(0), "mini");
                nr.Read = n.FirstOrDefault().is_read;
                nr.Time = u.Timedifference(n.OrderByDescending(o => o.time).FirstOrDefault().time);

                nr.LinkAction = nt.LinkAction;
                nr.LinkController = nt.LinkController;
                nr.LinkId = n.Key.object_id;

                int count = UserIDs.Count();
                if (count == 4)
                {
                    nr.NotificationHTML = "<span class='ntName'>" + GetFullName(UserIDs.ElementAtOrDefault(0)) + "</span>, " + "<span class='ntName'>" + GetFullName(UserIDs.ElementAtOrDefault(1)) + "</span>, and 2 others " + nt.message;
                }
                else if (count > 3)
                {
                    nr.NotificationHTML = "<span class='ntName'>" + GetFullName(UserIDs.ElementAtOrDefault(0)) + "</span>, " + "<span class='ntName'>" + GetFullName(UserIDs.ElementAtOrDefault(1)) + "</span>, " + "<span class='ntName'>" + GetFullName(UserIDs.ElementAtOrDefault(2)) + "</span>, and" + (count - 3).ToString() + " others " + nt.message;
                }
                else if (count == 3)
                {
                    nr.NotificationHTML = "<span class='ntName'>" + GetFullName(UserIDs.ElementAtOrDefault(0)) + "</span>, " + "<span class='ntName'>" + GetFullName(UserIDs.ElementAtOrDefault(1)) + "</span>, and " + "<span class='ntName'>" + GetFullName(UserIDs.ElementAtOrDefault(2)) + "</span> " + nt.message;
                }
                else if (count == 2)
                {
                    nr.NotificationHTML = "<span class='ntName'>" + GetFullName(UserIDs.ElementAtOrDefault(0)) + "</span>, and " + "<span class='ntName'>" + GetFullName(UserIDs.ElementAtOrDefault(1)) + "</span> " + nt.message;
                }
                else //count = 1
                {
                    nr.NotificationHTML = "<span class='ntName'>" + GetFullName(UserIDs.ElementAtOrDefault(0)) + "</span> " + nt.message;
                }
                result.Add(nr);
            }
            return result;
        
        }

        public Subscription GetSubscription(string username)
        {
            string guid = Membership.GetUser(username).ProviderUserKey.ToString();
            string MyGuid = GetUserId().ToString();

            return (from subscription in db.Subscriptions where subscription.Subscribedto.ToString() == guid && subscription.Subscriber.ToString() == MyGuid select subscription).SingleOrDefault();
        }

        public void RemoveSubscription(string username)
        {
            string guid = Membership.GetUser(username).ProviderUserKey.ToString();
            string MyGuid = GetUserId().ToString();

            Subscription s = (from subscription in db.Subscriptions where subscription.Subscribedto.ToString() == guid && subscription.Subscriber.ToString() == MyGuid select subscription).SingleOrDefault();
            db.Subscriptions.DeleteOnSubmit(s);
        }

        public void FollowByFacebookId(string facebookid)
        {
            Guid userGuid = GetUserId(); 
            //check if that facebook user exists
            var f = (from ud in db.UserDatas where ud.FBid == facebookid select ud).SingleOrDefault();
            if (f != null)
            {
                AddSubscription(f.UserId);
            }
        }

        public Subscription AddSubscription(Guid SubscribedToId)
        {
            Guid userGuid = GetUserId();
            if (SubscribedToId.ToString() != userGuid.ToString())
            {
                var check = (from s in db.Subscriptions where s.Subscriber.ToString() == userGuid.ToString() && s.Subscribedto.ToString() == SubscribedToId.ToString() select s).SingleOrDefault();
                if (check == null)
                {
                    Subscription s = new Subscription();
                    s.Subscribedto = SubscribedToId;
                    s.SubscribedtoName = Membership.GetUser(SubscribedToId).UserName;
                    s.Subscriber = userGuid;
                    s.date = DateTime.Now;
                    db.Subscriptions.InsertOnSubmit(s);
                    AddNotification(userGuid, SubscribedToId, "follow", Membership.GetUser(userGuid).UserName);
                    return s;
                }
                else { return check; }
            }
            else { return null; }
        }

        public int GetNPosts(string username)
        {
            string guid = Membership.GetUser(username).ProviderUserKey.ToString();
            return (from dp in db.Posts where dp.UserId.ToString() == guid select dp).Count();
        }

        public int GetNSubscribers(string username)
        {
            string guid = Membership.GetUser(username).ProviderUserKey.ToString();
            return (from s in db.Subscriptions where s.Subscribedto.ToString() == guid select s).Count();
        }

        public int GetNSubscribedTo(string username)
        {
            string guid = Membership.GetUser(username).ProviderUserKey.ToString();
            return (from s in db.Subscriptions where s.Subscriber.ToString() == guid select s).Count();
        }

        public int GetNLikes(string username)
        {
            string guid = Membership.GetUser(username).ProviderUserKey.ToString();
            return (from s in db.UserPageRelations where s.UserId.ToString() == guid && s.Rate == 1 select s).Count();
        }

        //get number of posts user is tagged in
        public int GetNUtags(string username)
        {
            string guid = Membership.GetUser(username).ProviderUserKey.ToString();
            return (from t in db.PostUserTags where t.Userid.ToString() == guid select t).Count();
        }
        
        public int GetScore(string username)
        {
            string guid = Membership.GetUser(username).ProviderUserKey.ToString();
            int Subscribers = GetNSubscribers(username);

            //gets all likes received by the page the user shared. Those likes are not a true representation of the users score since other users also would post the same page
            int LikesReceived = (from dp in db.Posts from upr in db.UserPageRelations where dp.UserId.ToString() == guid && upr.Page_id == dp.pageid && upr.Rate == 1 select upr).Count();

            return LikesReceived + Subscribers;
            
        }



        public string GetFullName(Guid userid)
        {
            return GetFullName(Membership.GetUser(userid).UserName);
        }

        public string GetFullName(string username)
        {
            ProfileBase pb = ProfileBase.Create(username);
            return pb.GetPropertyValue("Name").ToString();
        }

        /*
         GetNSubscribers(username)
         GetNLoves(username)
         */


        public void AddInitialUserTags(string Tags)
        {
            var tgs = Tags.Split(',');
            Guid UserId = GetUserId();
            foreach (string tg in tgs)
            {
                int tagid = 0;
                var result = (from t in db.tags where t.name == tg select t).SingleOrDefault();
                if (result != null) { tagid = result.tag_id; }
                else
                {
                    tag Tag = new tag(); Tag.name = tg.ToLower().Trim();
                    db.tags.InsertOnSubmit(Tag);
                    db.SubmitChanges();
                    var result2 = (from t in db.tags where t.name == Tag.name select t).SingleOrDefault();
                    if (result2 == null)
                    {
                        //failure adding tag to database
                    }
                    else
                    {
                        tagid = result2.tag_id;
                    }

                }


                interestmap im = new interestmap();
                im.pageid = 0;
                im.tag_id = tagid;
                im.userid = UserId;
                im.date = DateTime.Now;
                db.interestmaps.InsertOnSubmit(im);
            }

            db.SubmitChanges();

        }


        public void AddRelation(UserPageRelation userpagerelation)
        {
            Guid userGuid = GetUserId();
            var Result = GetRelation(userpagerelation.Page_id, userpagerelation.Post_id);
            userpagerelation.Date = DateTime.Now;
            bool changesmade = false;
            //check if a row has ever been created with this user for this page
            if (Result != null)
            {

                if (userpagerelation.Rate == 1)
                {
                    if (Result.Rate == 1) { /*Already loved*/}
                    else { Result.Rate = 1; changesmade = true; }
                }

                else if (userpagerelation.Rate == -1)
                {
                    if (Result.Rate == -1) { /*Already Hated*/}
                    else { Result.Rate = -1; changesmade = true; }
                }
                else if (userpagerelation.Rate == 0) { Result.Rate = 0; changesmade = true; }
            }
            //if not then create a row and add some relations
            else
            {
                changesmade = true;
                userpagerelation.UserId = userGuid;
                db.UserPageRelations.InsertOnSubmit(userpagerelation);
            }
            //to avoid error when no changes is made and database is required to save nothing!
            if (changesmade) { db.SubmitChanges(); }
        }

        public bool invite(string email)
        {
            var result = (from invites in db.Invitations where invites.email == email select invites).SingleOrDefault();
            if (result != null) { return false; }
            else
            {
                Invitation invite = new Invitation();
                invite.email = email;
                invite.date = DateTime.Now;
                invite.ID = Guid.NewGuid();
                invite.Checked = false;
                db.Invitations.InsertOnSubmit(invite);
                Save();
                return true;
            }
        }

        public bool PersonalInvite(string email)
        {
            TagThis.Controllers.utils utils = new TagThis.Controllers.utils();
            string guid = GetUserId().ToString();
            var result = (from invites in db.Invitations where invites.email == email && invites.invitedBy.ToString() == guid.ToString() select invites).SingleOrDefault();
            if (result != null) { return false; }
            else
            {
                Invitation invite = new Invitation();
                invite.email = email;
                invite.invitedBy = GetUserId();
                invite.date = DateTime.Now;
                invite.ID = Guid.NewGuid();
                invite.Checked = true;
                db.Invitations.InsertOnSubmit(invite);
                Save();
                ProfileBase pb =ProfileBase.Create(HttpContext.Current.User.Identity.Name);
                string name = utils.MakeFirstUpper(pb.GetPropertyValue("FirstName").ToString()) + " "+ utils.MakeFirstUpper(pb.GetPropertyValue("LastName").ToString());
                string body = "Hi,//n//n"+name+" thinks that you are an interesting person that finds great content on the web and loves to share it. TagThis is tailor-made for you.//n//nTagThis is a suggestion engine that knows about what you like and gives you more. TagThis helps you do more finding and less seeking. On TagThis you can organize the internet the way you want it to be, discover the web, save your favorites and have people subscribe to the content that you post.//n//nYou are one of the first people to use TagThis. As a chosen one, you will be able to take advantage of the new revolutionary search features TagThis has to offer.//n//nPlease bear in mind that TagThis is still in its infancy and is relying on your input for it to grow.//n//nGo ahead and use this link to register://n%%link%%//n//nIf you find any creepy crawlies, please report them for extermination at bugs@tagthis.com.//n//n Happy tagging!//nThe TagThis team.";
                body = body.Replace("//n", System.Environment.NewLine);
                body = body.Replace("%%link%%", "http://www.tagthis.com/account/register/" + invite.ID.ToString());
                utils.SendEmail(Membership.GetUser().Email, email, name + " invites you to use TagThis",body,name, false);
                return true;
            }
        }



        public void SendNotificationEmail(Guid Receiver, string type, string objectid)
        {
            NotificationTypesDB ntdb = new NotificationTypesDB();
            var m = ntdb.GetTypes().Where(t => t.type.ToLower() == type.ToLower()).SingleOrDefault();
            if (m != null)
            {
                Guid Sender = GetUserId();
                SearchRepository sr = new SearchRepository();
                string MiddleMessage = m.message.Remove(m.message.Length-1);
                string Message = "";
                bool sendemail = true;
                string senderfullname = GetFullName(Sender).Trim();
                string senderfirstname = senderfullname.Remove(senderfullname.IndexOf(' '));
                string Senderlink = "<a href='http://www.sixtysongs.com/Users/"+ Membership.GetUser(Sender).UserName.ToString()+"'>" + senderfullname + "</a>";
                string objectaction = ""; if (!string.IsNullOrEmpty(m.LinkAction)) { objectaction = m.LinkAction + "/"; }
                string objectlink = "<a href='http://www.sixtysongs.com/" + m.LinkController + "/" + objectaction + objectid +"'>#linktext#</a>";
                UserData ud = db.UserDatas.Where(a => a.UserId.ToString() == Membership.GetUser().ProviderUserKey.ToString()).SingleOrDefault();
                switch (type)
                {
                    case "postlike":
                        //gets post title
                        objectlink = objectlink.Replace("#linktext#", sr.SearchGetPost(int.Parse(objectid)).page.name); 
                        Message = Senderlink + " " + MiddleMessage + " " + objectlink;
                        //check whether user wants to receive notifications or not
                        sendemail = ud.EmailLikes;
                        break;
                    case "follow":
                        Message = Senderlink + " " + MiddleMessage + ".";
                        //check whether user wants to receive notifications or not
                        sendemail = ud.EmailFollows;
                        break;
                    case "postcomment":
                        //gets post title
                        objectlink = objectlink.Replace("#linktext#", GetComment(int.Parse(objectid)).Post.WebPage.name);
                        Message = Senderlink + " " + MiddleMessage + " " + objectlink;
                        //check whether user wants to receive notifications or not
                        sendemail = ud.EmailComments;
                        break;
                    case "commentlike":
                        //gets post title
                        objectlink = objectlink.Replace("#linktext#", GetComment(int.Parse(objectid)).text);
                        Message = Senderlink + " " + MiddleMessage + " " + objectlink;
                        //check whether user wants to receive notifications or not
                        sendemail = ud.EmailCommentsLike;
                        break;
                    case "postcommentreply":
                        //gets post title
                        objectlink = objectlink.Replace("#linktext#", GetComment(int.Parse(objectid)).Post.WebPage.name);
                        Message = Senderlink + " " + MiddleMessage + " " + objectlink;
                        //check whether user wants to receive notifications or not
                        sendemail = ud.EmailCommentsReply;
                        break;
                    case "postusertag":
                        //gets post title
                        objectlink = objectlink.Replace("#linktext#", sr.SearchGetPost(int.Parse(objectid)).page.name);
                        Message = Senderlink + " " + MiddleMessage + " " + objectlink;
                        //check whether user wants to receive notifications or not
                        sendemail = ud.EmailUserPostTag;
                        break;
                }

                TagThis.Controllers.utils utils = new TagThis.Controllers.utils();
                string email = Membership.GetUser(Receiver).Email;

                if (!string.IsNullOrEmpty(email) && sendemail)
                {
                    string subject = GetFullName(Sender) + " " + m.message;
                    //generate url
                    string receiverfullname = GetFullName(Receiver).Trim();
                    string receiverfirstname = receiverfullname.Remove(receiverfullname.IndexOf(' '));
                    string body = "Hi " + receiverfirstname + ",//n//n" + Message + "//n//nHappy Listening!//n//nThe Sixtysongs team.//n//n//n---//n<a href='http://www.sixtysongs.com/account/settings'>Change</a> your email preferences.";
                    body = body.Replace("//n", "<br/>");
                    utils.SendEmail("donotreply@sixtysongs.com", email, subject, body, "Sixtysongs", true);
                }
            }
        }
        
        public void SendWelcomeMessage()
        {
            TagThis.Controllers.utils utils = new TagThis.Controllers.utils();
            Guid guid = GetUserId();
            string email = Membership.GetUser().Email;
            if (!string.IsNullOrEmpty(email))
            { 

                string fullname = GetFullName(guid).Trim();
                string firstname = fullname.Remove(fullname.IndexOf(' '));
                string body = "Hi " + firstname + ",//n//nYou are the newest member of SixtySongs, a place to interact in the name of music. We can't wait to listen to songs you share.//n//nA few tips to get the most out of Sixtysongs://n//n-Follow a few interesting people. Sixtysongs is just about discovering new songs as it is about sharing.//n//n-Appreciate what others share. People love it when you like the music they share, discuss your opinion, and recommend them new songs. They will do the same for you.//n//n-Share carefully! As one of the first members of Sixtysongs, your taste will help set the tone for the whole community. Share great music, write catchy descriptions, and tag songs to make them easier to find. The more you share the better Sixtysongs will become.//n//n-Invite your friends! Sixtysongs is nothing without its cool members. Got some friends with great taste in music? Go ahead and invite them.//n//nWelcome to the family!//nThe Sixtysongs team.";
                body = body.Replace("//n", "<br/>");
                utils.SendEmail("team@sixtysongs.com", email,"Welcome to Sixtysongs!", body, "Sixtysongs", true);
            }
        }

        public bool isInvited(string email)
        {
            string userID = GetUserId().ToString();
            var result = (from i in db.Invitations where i.email == email && i.invitedBy.ToString() == userID select i).SingleOrDefault();
            if (result == null) { return false; }
            else {return true;}
        }

        public bool isRegistered(string email)
        {
            string result = Membership.GetUserNameByEmail(email);
            if (string.IsNullOrEmpty(result)) { return false; }
            else { return true; }
        }

        public string GetEmailFromInvitation(string id)
        {
        var result = (from invites in db.Invitations where invites.ID.ToString() == id select invites).SingleOrDefault();
        if (result != null)
        {
          if(result.Checked){return result.email;}
          else{return "notchecked";}
        }
        else { return null; }
        }

        public void DeleteInvitation(string email)
        {
            var result = (from invites in db.Invitations where invites.email == email select invites);
            foreach( Invitation r in result)
            {
                db.Invitations.DeleteOnSubmit(r);

            }
            db.SubmitChanges();
        }


        public string CheckIfFacebookIsConnected(string FBid)
        {
            var u = (UserData)(from a in db.UserDatas where a.FBid.Trim() == FBid.Trim() select a).SingleOrDefault();
            if (u == null)
            {
                return null;
            }
            else 
            {
                return Membership.GetUser(u.UserId).UserName;
            }
        }


        //this function is used to update or create the userdata which is any additional data needing to be stored outside the provider membership class
        public void UpdateUserData(string UserId, string FBtoken, string FBid, bool? FBautopost, string UserName, string FullName)
        { 
            var u = (UserData)(from a in db.UserDatas where a.UserId.ToString().Trim() == UserId select a).SingleOrDefault();
            if (u == null)
            {
                UserData ud = new UserData();
                ud.UserId = new Guid(UserId);
                ud.FBid = FBid;
                ud.FBtoken = FBtoken;
                ud.UserName = UserName;
                ud.FullName = FullName;

                ud.FBautopost = true;
                ud.EmailComments = true;
                ud.EmailCommentsReply = true;
                ud.EmailFollows = true;
                ud.EmailLikes = true;
                ud.EmailReposts = true;
                ud.EmailUserPostTag = true;
                ud.EmailCommentsLike = true;

                db.UserDatas.InsertOnSubmit(ud);
                db.SubmitChanges();
            }
            else 
            {

                bool changes = false;
                if (FBtoken != null) { u.FBtoken = FBtoken; changes = true; }
                if (FBid != null) { u.FBid = FBid; changes = true; }
                if (UserName != null) { u.UserName = UserName; changes = true; }
                if (FullName != null) { u.FullName = FullName; changes = true; }
                if (changes) { db.SubmitChanges(); }
            
            }
        }


        //this function is used to update or create the userdata which is any additional data needing to be stored outside the provider membership class
        public void UpdateUserSettings(bool EmailFollows, bool EmailReposts, bool EmailLikes, bool EmailComments, bool EmailCommentsReply, bool EmailCommentsLike, bool FBautopost,bool EmailUserPostTag)
        {
            string UserId = GetUserId().ToString();
            var u = (UserData)(from a in db.UserDatas where a.UserId.ToString().Trim() == UserId select a).SingleOrDefault();
            if (u == null)
            {
            }
            else
            {
                u.FBautopost = FBautopost;
                u.EmailFollows = EmailFollows;
                u.EmailComments = EmailComments;
                u.EmailCommentsReply = EmailCommentsReply;
                u.EmailLikes = EmailLikes;
                u.EmailCommentsLike = EmailCommentsLike;
                u.EmailReposts = EmailReposts;
                u.EmailUserPostTag = EmailUserPostTag;
                db.SubmitChanges();
            }
        }


        //this function is used when the user adds or likes a page to save his interests in the form of keywords
        public void AddNewUserInterest(int pageid, Guid userid)
        {
            var tm = (from t in db.tagmaps where t.page_id == pageid select t).ToList<tagmap>();
            foreach (tagmap tgmp in tm)
            {
                int exists = (from i in db.interestmaps where i.userid.ToString() == userid.ToString() && i.pageid == pageid && i.tag_id == tgmp.tag_id select i).Count();
                if (exists == 0)
                {
                    interestmap im = new interestmap();
                    im.pageid = pageid;
                    im.tag_id = tgmp.tag_id;
                    im.userid = userid;
                    im.date = DateTime.Now;
                    db.interestmaps.InsertOnSubmit(im);
                }
                else
                {
                /* does nothing to avoid noise from heavily tagged pages*/
                }
            }
            db.SubmitChanges();
        }

        public UserData GetUserData()
        {
            string UserId = GetUserId().ToString();
            return (UserData)(from a in db.UserDatas where a.UserId.ToString().Trim() == UserId select a).SingleOrDefault();
            
        }


        //this function is used when the user adds or likes a page to save his interests in the form of keywords
        public void AddNewUserInterest(int pageid)
        {
            Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
            var tm = (from t in db.tagmaps where t.page_id == pageid select t).ToList<tagmap>();
            foreach (tagmap tgmp in tm)
            {
                int exists = (from i in db.interestmaps where i.userid.ToString() == userid.ToString() && i.pageid == pageid && i.tag_id == tgmp.tag_id select i).Count();
                if (exists == 0)
                {
                    interestmap im = new interestmap();
                    im.pageid = pageid;
                    im.tag_id = tgmp.tag_id;
                    im.userid = userid;
                    im.date = DateTime.Now;
                    db.interestmaps.InsertOnSubmit(im);
                }
                else
                {
                    /* does nothing to avoid noise from heavily tagged pages*/
                }
                db.SubmitChanges();
            }
        }

        //this function is used when a user takes back his like for a page so the interest is also removed
        public void RemoveUserInterest(int pageid)
        {
            Guid userid = (Guid)Membership.GetUser().ProviderUserKey;
            var im = (from i in db.interestmaps where i.pageid == pageid && i.userid.ToString() == userid.ToString() select i).AsEnumerable<interestmap>();
            db.interestmaps.DeleteAllOnSubmit(im);
           
        }

        public void DeleteUser()
        {
            string userid = Membership.GetUser().ProviderUserKey.ToString();

            var commentrating = (from cr in db.CommentRatings where cr.userid.ToString() == userid select cr);
            db.CommentRatings.DeleteAllOnSubmit(commentrating);

            var comments = (from c in db.Comments where c.userid.ToString() == userid select c);
            var scommentrating = (from cr in db.CommentRatings from c in comments where cr.comment_id == c.id select cr);
            db.CommentRatings.DeleteAllOnSubmit(scommentrating);
            db.Comments.DeleteAllOnSubmit(comments);

            var notifications = (from n in db.notifications where n.recipient_id.ToString() == userid || n.sender_id.ToString() == userid select n);
            db.notifications.DeleteAllOnSubmit(notifications);

            var interestmaps = (from im in db.interestmaps where im.userid.ToString() == userid select im);
            db.interestmaps.DeleteAllOnSubmit(interestmaps);

            var subscriptions = (from s in db.Subscriptions where s.Subscriber.ToString() == userid || s.Subscribedto.ToString() == userid select s);
            db.Subscriptions.DeleteAllOnSubmit(subscriptions);

            var tagmap = (from t in db.tagmaps where t.user_id.ToString() == userid select t);
            db.tagmaps.DeleteAllOnSubmit(tagmap);

            var userdata = (from ud in db.UserDatas where ud.UserId.ToString() == userid select ud);
            db.UserDatas.DeleteAllOnSubmit(userdata);

            var userpagerelations = (from upr in db.UserPageRelations where upr.UserId.ToString() == userid select upr);
            db.UserPageRelations.DeleteAllOnSubmit(userpagerelations);

            var posts = (from p in db.Posts where p.UserId.ToString() == userid select p);
            foreach (var p in posts)
            {
                DeletePost(p.id);
            }
            db.Posts.DeleteAllOnSubmit(posts);

            db.SubmitChanges();

        
        }


        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
