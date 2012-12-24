using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Embedly;
using Embedly.OEmbed;
using System.Dynamic;
using System.Text.RegularExpressions;

namespace TagThis.Controllers
{

    public class Embed
    {
        public string type;
        public string title;
        public string url;
        public string description;
        public string thumb;
        public string html;
        public string tags;

    }


    public class EmbedlyController : Controller
    {
        //http://www.youtube.com/watch?v=YwSZvHqf9qM   // video

        //http://www.amazon.com/Times-They-Are--Changin/dp/B0009MAP9A/ref=sr_1_34?ie=UTF8&qid=1310348558&sr=8-34

        //http://img402.yfrog.com/i/mfe.jpg/

        //


        // GET: /Embedly/

        public Embed GetEmbed(string u)
        {


            Spider s = new Spider();

            u = s.CheckURL(u);
            s.Seturl(u);

            string desc = HttpUtility.HtmlDecode(s.GetDescription());

            string tags = HttpUtility.HtmlDecode(s.GetMetaTags());

            Embed e = new Embed();

            var key = "613e276eb5c411e0a3d94040d3dc5c07";
            var client = new Client(key);
            var url = new Uri(u);

            var result = client.GetOEmbed(url, new RequestOptions { MaxWidth = 420, AutoPlay = true });

            // basic response information
            var response = result.Response;
            e.type = response.Type.ToString();

            // link details
            var link = result.Response.AsLink;
           // e.description = link.Description;
            e.description = RemoveNastyness(desc); 
            e.thumb = link.ThumbnailUrl;
            e.title = link.Title;
            e.url = u;
            e.tags = tags;

            if (result.Response.Type == Embedly.ResourceType.Video)
            {
                var r = result.Response.AsVideo;
                e.html = r.Html;
            }
            else if (result.Response.Type == Embedly.ResourceType.Rich)
            {
                var r = result.Response.AsRich;
                e.html = r.Html;
            }
            else e.html = null;

            return e;

        }


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



    }
}
