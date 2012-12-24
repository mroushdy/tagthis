using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace TagThis.ActionFilters
{
    public class AjaxViewSelectorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as PartialViewResult;
            if (filterContext.HttpContext.Request.IsAjaxRequest() == false && result != null)
            {
                ViewDataDictionary viewdata = new ViewDataDictionary();
                viewdata = result.ViewData;
                viewdata.Add("PartialViewName",result.ViewName);
                

                var ViewResult = new ViewResult
                {
                    ViewName = "AjaxContainer",
                    ViewData = viewdata
                };

                filterContext.Result = ViewResult;
            }
        }
    }
}