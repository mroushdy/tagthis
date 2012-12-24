using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace TagThis.Controllers
{
    public class JsController : Controller
    {
        //
        // GET: /Js/

        public void Index(string u,int? n)
        {
            if (string.IsNullOrEmpty(u))
            {
                u = Request.UrlReferrer.ToString();
            }
            TagThis.Models.TagThisDataContext db = new TagThis.Models.TagThisDataContext();
            Spider s = new Spider();
            //linking css style
            Response.Write("function TagThis(){");
            Response.Write("{f='http://www.tagthis.com/lightbox?u='+encodeURIComponent(window.location.href);a=function(){if(!window.open(f,'tagthisv01','location=yes,links=no,scrollbars=no,toolbar=no,target=_blank,width=550,height=550'))location.href=f};if(/Firefox/.test(navigator.userAgent)){setTimeout(a,0)}else{a()}}");
            Response.Write("}");
            Response.Write("var headID = document.getElementsByTagName(" + '"' + "head" + '"' + ")[0];");
            Response.Write(System.Environment.NewLine);
            Response.Write("var cssNode = document.createElement('link');");
            Response.Write(System.Environment.NewLine);
            Response.Write("cssNode.type = 'text/css';");
            Response.Write(System.Environment.NewLine);
            Response.Write("cssNode.rel = 'stylesheet';");
            Response.Write(System.Environment.NewLine);
            Response.Write("cssNode.href = '../../Scripts/Widget.css';");
            Response.Write(System.Environment.NewLine);
            Response.Write("cssNode.media = 'screen';");
            Response.Write(System.Environment.NewLine);
            Response.Write("headID.appendChild(cssNode);");
            Response.Write(System.Environment.NewLine);
            //starting the list items
            Response.Write("document.write('<div class=" + '"' + "Meta" + '"' + ">');");
            Response.Write("document.write('<ul class=" + '"' + "tagchain" + '"' + ">');");
            Response.Write(System.Environment.NewLine);

            string url = s.CheckURL(u); 
            var tags = (from p in db.tagmaps from t in db.tags where p.WebPage.url== url && t.tag_id == p.tag_id group t by t.name into grp select new
                      {
                         Count = grp.Count(),
                         TagName = grp.Key
                      }).OrderByDescending(g => g.Count).Take(n ?? 5);
            foreach(var tag in tags)
            {
                Response.Write("document.write('<li class=" + '"' + "tagchain" + '"' + ">');");
                Response.Write(System.Environment.NewLine);
                Response.Write("document.write('<a class=" + '"' + "usertag" + '"' + " href=" + '"' + "../../Search/" + tag.TagName + "" + '"' + ">');");
                Response.Write(System.Environment.NewLine);
                Response.Write("document.write('<span class=" + '"' + "" + '"' + ">" + tag.TagName + "</span>');");
                Response.Write(System.Environment.NewLine);
                Response.Write("document.write('</a>');");
                Response.Write(System.Environment.NewLine);
                Response.Write("document.write('</li>');");
                Response.Write(System.Environment.NewLine);
            }
            //ending list and adding Tagthis button
            Response.Write("document.write('<li>');");
            Response.Write(System.Environment.NewLine);
            Response.Write("document.write('<a class=" + '"' + "usertag" + '"' + " href=" + '"' + "javascript:(TagThis())" + '"' + ">');");
            Response.Write(System.Environment.NewLine);
            Response.Write("document.write('<span class=" + '"' + "" + '"' + ">TAGTHIS</span>');");
            Response.Write(System.Environment.NewLine);
            Response.Write("document.write('</a>');");
            Response.Write(System.Environment.NewLine);
            Response.Write("document.write('</li>');");
            Response.Write(System.Environment.NewLine);
            Response.Write("document.write('</div>');");
        }

    }
}
