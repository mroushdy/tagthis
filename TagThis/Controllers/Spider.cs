using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using HtmlAgilityPack;

namespace TagThis.Controllers
{
    public class Spider : Controller
    {
        //
        // GET: /Spider/
        WebClient x = new WebClient();
        HtmlWeb htmlWeb;
        HtmlDocument doc;
        public void Seturl(String Url)
        {
           /*source = x.DownloadString(Url);*/
            htmlWeb = new HtmlWeb();
            doc = htmlWeb.Load(Url);
        }
        public string GetTitle()
        {  
            /*string title = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;*/
            HtmlNode Title = doc.DocumentNode.SelectSingleNode("//title");
            string title = Title.InnerText.ToString();
            return title;
        }

        public string GetDescription()
        {
            string metaDescription="";

            HtmlNode Metadesc = doc.DocumentNode.SelectSingleNode("//meta[@name='description']");
            //check if page has meta description
            if (Metadesc != null)
            { metaDescription = Metadesc.Attributes["content"].Value; }
             
            else
            {
                //get the first <p>
                HtmlNodeCollection descriptions = doc.DocumentNode.SelectNodes("//p");
                if (descriptions != null)
                {
                    foreach (HtmlNode description in descriptions)
                    {
                        if (description.InnerText.Length > 50) { metaDescription = description.InnerText; break; }
                    }
                }
                else
                { 
                
                }
            }

         /* REGEX IMPLEMENTATION: 
            Match DescriptionMatch = Regex.Match(source, "<meta name=\"description\" content=\"([^<]*)\" ?/?>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (DescriptionMatch.Success)
            {metaDescription = DescriptionMatch.Groups[1].Value.ToString();}
          */
      

            return metaDescription;
        }

        public string GetMetaTags()
        {

            string metaTags = "";

            HtmlNode MetaTG = doc.DocumentNode.SelectSingleNode("//meta[@name='keywords']");
            //check if page has meta keywords
            if (MetaTG != null)
            { 
                metaTags = MetaTG.Attributes["content"].Value; 
            }

            else
            {
                metaTags = "";
            }

            return metaTags;
        }

        public string cleanURL(string url)
        {
            //removes the http before the url and anything after it in order to retreive its icon
            url = url.Substring(7);
            int i = url.IndexOf('/');
            if(i>=0)
            {
                url = url.Remove(i);
            }
                return url;
        }

        public string CheckURL(string url)
        {
            //makes sure the url starts with http://www. other wise change it so that this happens
            if (!url.StartsWith("http://", true ,null) )
            {
                url = url.Insert(0, "http://");
            }
            string urls = url.Substring(7);
            if(!urls.StartsWith("www.",true,null))
            {
                url = url.Insert(7, "www.");
            }

            return url;
        }

        public string GetIconUrl()
        {
            string url = "";
            HtmlNode Icourl = doc.DocumentNode.SelectSingleNode("//link[@rel='icon']");
            //check if page has meta description
            if (Icourl != null)
            { url = Icourl.Attributes["href"].Value; }

            else
            {
                HtmlNode Icourl2 = doc.DocumentNode.SelectSingleNode("//link[@rel='SHORTCUT ICON']");
                if (Icourl2 != null)
                { url = Icourl2.Attributes["href"].Value; }
                else { url = "none"; }
            }

            /* REGEX IMPLEMENTATION: 
               Match DescriptionMatch = Regex.Match(source, "<meta name=\"description\" content=\"([^<]*)\" ?/?>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
               if (DescriptionMatch.Success)
               {metaDescription = DescriptionMatch.Groups[1].Value.ToString();}
             */


            return url;
        }

    }
}
