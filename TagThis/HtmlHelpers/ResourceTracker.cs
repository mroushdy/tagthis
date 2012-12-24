using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AjaxControlToolkitMvc
{
    public class ResourceTracker
    {
        const string resourceKey = "__resources";

        private List<string> _resources;

        public ResourceTracker(HttpContextBase context)
        {
            _resources = (List<string>)context.Items[resourceKey];
            if (_resources == null)
            {
                _resources = new List<string>();
                context.Items[resourceKey] = _resources;
            }
        }

        public void Add(string url)
        {
            url = url.ToLower();
            _resources.Add(url);
        }

        public bool Contains(string url)
        {
            url = url.ToLower();
            return _resources.Contains(url);
        }

    }
}
