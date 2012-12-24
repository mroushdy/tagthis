using System;
using System.Text;
using System.Web.Mvc;
using System.Collections.Generic;

namespace AjaxControlToolkitMvc
{
    public static class AjaxExtensions
    {
        private static string _microsoftAjaxLibraryUrl = "/Content/MicrosoftAjax.js";
        private static string _toolkitFolderUrl = "/Content/AjaxControlToolkit/3.0.20820.16598/3.0.20820.0/";

        public static void SetMicrosoftAjaxLibraryUrl(this AjaxHelper helper, string url)
        {
            _microsoftAjaxLibraryUrl = url;
        }

        public static string GetMicrosoftAjaxLibraryUrl(this AjaxHelper helper)
        {
            return _microsoftAjaxLibraryUrl;
        }


        public static void SetToolkitFolderUrl(this AjaxHelper helper, string url)
        {
            _toolkitFolderUrl = url;
        }

        public static string GetToolkitFolderUrl(this AjaxHelper helper)
        {
            return _toolkitFolderUrl;
        }

        public static string MicrosoftAjaxLibraryInclude(this AjaxHelper helper)
        {
            return ScriptExtensions.ScriptInclude(helper, _microsoftAjaxLibraryUrl);
        }

        public static string ToolkitInclude(this AjaxHelper helper, params string[] fileName)
        {
            var sb = new StringBuilder();
            foreach (string item in fileName)
            {
                var fullUrl = _toolkitFolderUrl + item;
                sb.AppendLine(ScriptExtensions.ScriptInclude(helper, fullUrl));
            }
            return sb.ToString();
        }


        public static string DynamicToolkitCssInclude(this AjaxHelper helper, string fileName)
        {
            var fullUrl = _toolkitFolderUrl + fileName;
            return helper.DynamicCssInclude(fullUrl);
        }

        public static string DynamicCssInclude(this AjaxHelper helper, string url)
        {
            var tracker = new ResourceTracker(helper.ViewContext.HttpContext);
            if (tracker.Contains(url))
                return String.Empty;

            var sb = new StringBuilder();
            sb.AppendLine("<script type='text/javascript'>");
            sb.AppendLine("var link=document.createElement('link')");
            sb.AppendLine("link.setAttribute('rel', 'stylesheet');");
            sb.AppendLine("link.setAttribute('type', 'text/css');");
            sb.AppendFormat("link.setAttribute('href', '{0}');", url);
            sb.AppendLine();
            sb.AppendLine("var head = document.getElementsByTagName('head')[0];");
            sb.AppendLine("head.appendChild(link);");
            sb.AppendLine("</script>");
            return sb.ToString();
        }

        public static string Create(this AjaxHelper helper, string clientType, string elementId)
        {
            return Create(helper, String.Empty, clientType, elementId);
        }


        public static string Create(this AjaxHelper helper, string clientType, object props, string elementId)
        {
            var strProps = ObjectToString(props);
            var sb = new StringBuilder();
            sb.AppendLine("<script type='text/javascript'>");
            sb.AppendLine("Sys.Application.add_init(function(){");
            sb.AppendFormat("$create({0},{1},null,null,$get('{2}'))", clientType, strProps, elementId);
            sb.AppendLine("});");
            sb.AppendLine("</script>");
            return sb.ToString();
        }


        private static string ObjectToString(object thing)
        {
            var colProps = new List<string>();
            var props = thing.GetType().GetProperties();
            foreach (var prop in props)
            {
                var val = prop.GetValue(thing, null);
                if (val is string)
                    colProps.Add(String.Format("{0}:'{1}'", prop.Name, prop.GetValue(thing, null)));
                else
                    colProps.Add(String.Format("{0}:{1}", prop.Name, prop.GetValue(thing, null)));
            }
            return "{" + String.Join(",", colProps.ToArray()) + "}";
        }
    }
}
