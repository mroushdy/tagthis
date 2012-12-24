using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Services;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.Services.Protocols;
using System.Xml.Linq;

namespace TagThis.Content
{
    /// <summary>
    /// Summary description for PeopleSuggestHandler
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class PeopleSuggestHandler : IHttpHandler
    {

        
        public void ProcessRequest(HttpContext context)
        {
            //get the letters in the search box
            string word = context.Request.QueryString["search"].ToString().Trim().ToLower();
            if (!string.IsNullOrEmpty(word))
            {
                //set response to json
                context.Response.ContentType = "application/json";
                context.Request.ContentEncoding = Encoding.UTF8;


              
                //get all tags from db that partially match the search box
                TagThis.Models.TagThisDataContext db = new TagThis.Models.TagThisDataContext();
                TagThis.Models.UserRepository ur = new Models.UserRepository();
                List<TagThis.Models.UserListItem> users = ur.GetFollowers(ur.GetUserName().ToString()).Where(w => w.fullname.ToLower().Contains(word)).OrderByDescending(o => o.fullname).ToList();


                context.Response.Write("[");
                int i = 0;
                foreach (var u in users)
                {
                    context.Response.Write("[" + '"' + u.username.Trim() + '"' + "," + '"' + u.fullname.Trim() + '"' + ",null," + '"' + "<img src='" + u.profilepictureurl + "' /><span>" + u.fullname + "</span>" + '"' + "]");
                    if (i < users.Count() - 1)
                    {
                        context.Response.Write(",");
                    }
                    i++;
                }
                context.Response.Write("]");
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
