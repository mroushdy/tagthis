using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace TagThis.Controllers
{

    //extends string to get nth occurance of a character or string

    public static class StringExtender
    {
        public static int NthIndexOf(this string target, string value, int n)
        {
            Match m = Regex.Match(target, "((" + value + ").*?){" + n + "}");

            if (m.Success)
                return m.Groups[2].Captures[n - 1].Index;
            else
                return -1;
        }
    }

    public class country
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class utils
    {
        public static bool ChangeUserName(string oldUserName,string newUserName)
        {
            bool IsSuccsessful = false;
            string ApplicationName;
            if(IsUserNameValid(newUserName))
            {
                if ((ConfigurationManager.ConnectionStrings["applicationName"] == null) || String.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["applicationName"].ToString()))
                {ApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;}
                else
                { ApplicationName = ConfigurationManager.ConnectionStrings["applicationName"].ToString(); }
                
                SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
               SqlCommand cmdChangeUserName= new SqlCommand();
               cmdChangeUserName.CommandText="dbo.aspnet_Membership_ChangeUserName";
               cmdChangeUserName.CommandType = CommandType.StoredProcedure;
               cmdChangeUserName.Connection = myConn;
               cmdChangeUserName.Parameters.Add("@ApplicationName", SqlDbType.NVarChar);
               cmdChangeUserName.Parameters.Add("@OldUserName", SqlDbType.NVarChar);
               cmdChangeUserName.Parameters.Add("@NewUserName", SqlDbType.NVarChar);
               cmdChangeUserName.Parameters["@ApplicationName"].Value = ApplicationName;
               cmdChangeUserName.Parameters["@OldUserName"].Value = oldUserName;
               cmdChangeUserName.Parameters["@NewUserName"].Value = newUserName;
                try
                {
               myConn.Open();
               cmdChangeUserName.ExecuteNonQuery();
               myConn.Close();
               IsSuccsessful = true;
               }
                catch(Exception ex )
                {IsSuccsessful = false;}

            }
            else{IsSuccsessful = false;}
            return IsSuccsessful;
        }


        private static bool IsUserNameValid(string username)
        {
            
          //Add whatever username requirement validation you want here, doesnt
          //the membership provider have some build in functionality for this

            if (Membership.GetUser(username) == null) { return true; }
            else { return false; }
        }


        public int GetAge(DateTime start, DateTime end)
        {

            // Compute the difference between start

            //year and end year.

            int years = end.Year - start.Year;

            int months = 0;

            int days = 0;

            // Check if the last year was a full year.

            if (end < start.AddYears(years) && years != 0)
            {

                --years;

            }

            start = start.AddYears(years);

            // Now we know start <= end and the diff between them

            // is < 1 year.

            if (start.Year == end.Year)
            {

                months = end.Month - start.Month;

            }

            else
            {

                months = (12 - start.Month) + end.Month;

            }

            // Check if the last month was a full month.

            if (end < start.AddMonths(months) && months != 0)
            {

                --months;

            }

            start = start.AddMonths(months);

            // Now we know that start < end and is within 1 month

            // of each other.

            days = (end - start).Days;

            return years;

        }

        public string GetGender(string abv)
        {
            switch (abv)
            {
                case "gu":
                    return "guy";
                    break;
                case "gi":
                    return "girl";
                    break;
                case "du":
                    return "dude";
                    break;
                case "ch":
                    return "chick";
                    break;
                case "bl":
                    return "bloke";
                    break;
                case "bi":
                    return "bird";
                    break;
                case "la":
                    return "lady";
                    break;
                case "ge":
                    return "gentleman";
                    break;
                case "ma":
                    return "male";
                    break;
                case "fa":
                    return "female";
                    break;
                case "tr":
                    return "transgender";
                    break;
                default:
                    return null;
            }

        }

        public IEnumerable<country> GetCountries()
        {
        List<country> countries = new List<country>();
        string[] codes = new string[] {"af","al","dz","as","ad","ao","ai","aq","ag","ar","am","aw","au","at","az","bs","bh","bd","bb","by","be","bz","bj","bm","bt","bo","ba","bw","br","io","bn","bg","bf","bi","kh","cm","ca","cv","ky","cf","td","cl","cn","co","km","cg","ck","cr","ci","hr","cu","cy","cz","dk","dj","dm","do","ec","eg","sv","gq","er","ee","et","fk","fo","fm","fj","fi","fr","gf","pf","ga","gm","ge","de","gh","gi","gr","gl","gd","gp","gu","gt","gn","gw","gy","ht","va","hn","hk","hu","is","in","id","iq","ie","ir","il","it","jm","jp","jo","kz","ke","ki","kw","kg","la","lv","lb","ls","lr","ly","li","lt","lu","mo","mg","mw","my","mv","ml","mt","mq","mr","mu","yt","mx","mc","mn","ms","ma","mz","mm","na","nr","np","nl","an","nc","nz","ni","ne","ng","nu","nf","mp","no","om","pk","pw","ps","pa","pg","py","pe","ph","pl","pt","pr","qa","kr","md","re","ro","ru","rw","kn","lc","vc","ws","sm","st","sa","sn","cs","sc","sl","sg","sk","si","sb","so","za","gs","es","lk","sd","sr","sz","se","ch","sy","tw","tj","th","cd","mk","tl","tg","tk","to","tt","tn","tr","tm","tv","ug","ua","ae","gb","tz","us","um","uy","uz","vu","ve","vn","vg","vi","ye","zm","zw"};
        string[] names = new string[] {"Afghanistan","Albania","Algeria","American Samoa","Andorra","Angola","Anguilla","Antarctica","Antigua And Barbuda","Argentina","Armenia","Aruba","Australia","Austria","Azerbaijan","Bahamas","Bahrain","Bangladesh","Barbados","Belarus","Belgium","Belize","Benin","Bermuda","Bhutan","Bolivia","Bosnia And Herzegovina","Botswana","Brazil","British Indian Ocean Territory","Brunei Darussalam","Bulgaria","Burkina Faso","Burundi","Cambodia","Cameroon","Canada","Cape Verde","Cayman Islands","Central African Republic","Chad","Chile","China","Colombia","Comoros","Congo","Cook Islands","Costa Rica","Cote Ivoire","Croatia","Cuba","Cyprus","Czech Republic","Denmark","Djibouti","Dominica","Dominican Republic","Ecuador","Egypt","El Salvador","Equatorial Guinea","Eritrea","Estonia","Ethiopia","Falkland Islands (Malvinas)","Faroe Islands","Federated States Of Micronesia","Fiji","Finland","France","French Guiana","French Polynesia","Gabon","Gambia","Georgia","Germany","Ghana","Gibraltar","Greece","Greenland","Grenada","Guadeloupe","Guam","Guatemala","Guinea","Guinea-Bissau","Guyana","Haiti","Holy See (Vatican City State)","Honduras","Hong Kong","Hungary","Iceland","India","Indonesia","Iraq","Ireland","Islamic Republic Of Iran","Israel","Italy","Jamaica","Japan","Jordan","Kazakhstan","Kenya","Kiribati","Kuwait","Kyrgyzstan","Lao People&#39;S Democratic Republic","Latvia","Lebanon","Lesotho","Liberia","Libyan Arab Jamahiriya","Liechtenstein","Lithuania","Luxembourg","Macao","Madagascar","Malawi","Malaysia","Maldives","Mali","Malta","Martinique","Mauritania","Mauritius","Mayotte","Mexico","Monaco","Mongolia","Montserrat","Morocco","Mozambique","Myanmar","Namibia","Nauru","Nepal","Netherlands","Netherlands Antilles","New Caledonia","New Zealand","Nicaragua","Niger","Nigeria","Niue","Norfolk Island","Northern Mariana Islands","Norway","Oman","Pakistan","Palau","Palestinian Territory, Occupied","Panama","Papua New Guinea","Paraguay","Peru","Philippines","Poland","Portugal","Puerto Rico","Qatar","Republic Of Korea","Republic Of Moldova","Reunion","Romania","Russian Federation","Rwanda","Saint Kitts And Nevis","Saint Lucia","Saint Vincent And The Grenadines","Samoa","San Marino","Sao Tome And Principe","Saudi Arabia","Senegal","Serbia And Montenegro","Seychelles","Sierra Leone","Singapore","Slovakia","Slovenia","Solomon Islands","Somalia","South Africa","South Georgia And The South Sandwich Islands","Spain","Sri Lanka","Sudan","Suriname","Swaziland","Sweden","Switzerland","Syrian Arab Republic","Taiwan","Tajikistan","Thailand","The Democratic Republic Of The Congo","The Former Yugoslav Republic Of Macedonia","Timor-Leste","Togo","Tokelau","Tonga","Trinidad And Tobago","Tunisia","Turkey","Turkmenistan","Tuvalu","Uganda","Ukraine","United Arab Emirates","United Kingdom","United Republic Of Tanzania","United States","United States Minor Outlying Islands","Uruguay","Uzbekistan","Vanuatu","Venezuela","Viet Nam","Virgin Islands, British","Virgin Islands, U.S.","Yemen","Zambia","Zimbabwe"};
        for (int i = 0; i < codes.Length; i++)
        { 
        country c = new country();
        c.code = codes[i];
        c.name = names[i];
        countries.Add(c);
        }
        return countries.AsEnumerable();
        }

        public string GetCountry(string code)
        {
            var country = GetCountries().Where(i => i.code == code).SingleOrDefault();
            if (country != null) { return country.name; }
            else{return null;}
        }


        public string MakeFirstUpper(string name)
        {
            if (name.Length <= 1) return name.ToUpper();
            Char[] letters = name.ToCharArray();
            letters[0] = Char.ToUpper(letters[0]);
            return new string(letters);
        }

        public void SendEmail(string from, string to, string subject, string body, string displayname, bool html)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress(from,displayname);
            mailMsg.To.Add(to);
            mailMsg.Subject = subject;
            mailMsg.IsBodyHtml = html;
            mailMsg.BodyEncoding = Encoding.UTF8;
            mailMsg.Body = body;
            mailMsg.Priority = MailPriority.Normal;
            // Smtp configuration
            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential("noreply@sixtysongs.com", "Sr794W9c9");
            client.Port = 587; //or use 465            
            client.Host = "mail.sixtysongs.com";
           // client.EnableSsl = true;
            object userState = mailMsg;
                //you can also call client.Send(msg)
                client.SendAsync(mailMsg, userState);
        }

        //gets page rating for the widget
        public string GetRating(int pageid)
        {
            Models.SearchRepository sr = new Models.SearchRepository();
            int r = sr.GetPageRating(pageid);

            return ShortNumber(r);

        }



        //decrease any number to 999 or 100k or 1m etc
        public string ShortNumber(int number)
        {
            string rt = number.ToString();


            //rounding to the nearest million or thousand does not occur here
            if (rt.Length > 6)
            { return rt.Remove(rt.Length - 6, 6) + "m"; } // this means its a million or more
            else if (rt.Length > 3)
            { return rt.Remove(rt.Length - 3, 3) + "k"; } // this means its a thousand or more
            else { return rt; }
        }


        public List<string> GetGenres()
        {
            List<string> xmlGenres = new List<string>();
            XDocument xdoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Content/genre-list.xml"));

            //Run query
            var GenreKeys = from gk in xdoc.Descendants("GenreKey")
                            select new
                            {
                                Header = gk.Attribute("name").Value,
                                Children = gk.Descendants("Genre")
                            };

            //Loop through results
            foreach (var GenreKey in GenreKeys)
            {
                xmlGenres.Add(GenreKey.Header);
                foreach (var Genre in GenreKey.Children) { xmlGenres.Add(Genre.Attribute("name").Value); }
            }

            return xmlGenres;
        }

        public string Timedifference(DateTime date)
        {
            string text = "";
            int years = DateTime.Now.Year - date.Year;
            int months = DateTime.Now.Month - date.Month;
            TimeSpan difference = DateTime.Now.Subtract(date);
            if (years >= 1)
            {
                text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month).Substring(0, 3) + " " + date.Day.ToString() + " " + date.Year.ToString();
            }
            else if (months > 1)
            {
                text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month).Substring(0, 3) + " " + date.Day.ToString();
            }
            else if (difference.TotalDays > 1)
            {
                text = difference.Days.ToString() + " days ago";
            }
            else if (difference.TotalHours > 1)
            {
                text = difference.Hours.ToString() + " hours ago";
            }
            else if (difference.Minutes > 1)
            {
                text = difference.Minutes.ToString() + " minutes ago";
            }
            else if (difference.Seconds > 1)
            {
                text = difference.Seconds.ToString() + " seconds ago";
            }
            else if (difference.Seconds <= 1)
            {
                text = "one second ago";
            }
            return text;
        }
    }
}