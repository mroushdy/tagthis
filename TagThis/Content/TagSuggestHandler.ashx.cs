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
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class TagSuggestHandler : IHttpHandler
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


                List<string> tags = new List<string>();
                //get all tags from db that partially match the search box
                TagThis.Models.TagThisDataContext db = new TagThis.Models.TagThisDataContext();
                List<string> dbtags = (from p in db.tags where p.name.ToLower().Contains(word) orderby p.name select p.name).ToList<string>();


                //get all tags from the html genres list that partially matches the search box
                TagThis.Controllers.utils u = new TagThis.Controllers.utils();
                List<string> xmlGenres = u.GetGenres();

                //get partial results
                List<string> xmlResults = xmlGenres.Where(p => p.ToLower().Contains(word)).ToList<string>();

                tags.AddRange(xmlResults);
                tags.AddRange(dbtags);
                tags.Distinct().OrderBy(o => o);

                context.Response.Write("[");
                int i = 0;
                foreach (var tag in tags)
                {
                    if (i < tags.Count() - 1)
                    {
                        context.Response.Write("[null," + '"' + tag.Trim() + '"' + ',' + "null" + ',' + "null" + "],");
                    }
                    else
                    {
                        context.Response.Write("[null," + '"' + tag.Trim() + '"' + ',' + "null" + ',' + "null" + "]");
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
