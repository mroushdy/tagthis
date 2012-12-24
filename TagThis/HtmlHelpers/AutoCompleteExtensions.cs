using System.Text;
using System.Web.Mvc;

namespace AjaxControlToolkitMvc
{
    public static class AutoCompleteExtensions
    {

        public static string AutoComplete(this AjaxHelper helper, string elementId, string servicePath, int minimumPrefixLength, int completionSetCount)
        {
            var sb = new StringBuilder();

            // Add Microsoft Ajax library
            sb.AppendLine(helper.MicrosoftAjaxLibraryInclude());

            // Add toolkit scripts
            sb.AppendLine(helper.ToolkitInclude
                (
                    "AjaxControlToolkit.ExtenderBase.BaseScripts.js",
                    "AjaxControlToolkit.Common.Common.js",
                    "AjaxControlToolkit.Animation.Animations.js",
                    "AjaxControlToolkit.PopupExtender.PopupBehavior.js",
                    "AjaxControlToolkit.Animation.AnimationBehavior.js",
                    "AjaxControlToolkit.Compat.Timer.Timer.js",
                    "AjaxControlToolkit.AutoComplete.AutoCompleteBehavior.js"
                ));

            // Create properties
            var props = new
            {
                serviceMethod = "GetCompletionList",
                servicePath = servicePath,
                minimumPrefixLength = minimumPrefixLength,
                completionSetCount = completionSetCount,
                delimiterCharacters = " +-(|"
            };

            // Perform $create
            sb.AppendLine(helper.Create("AjaxControlToolkit.AutoCompleteBehavior", props, elementId));

            return sb.ToString();
        }

    }
}
